//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{

    ///<summary>
    /// Control For Win that displays a collection of a Business Object along side an editor/creator panel.
    /// The collection of business objects can be shown using any selector control e.g. an <see cref="IEditableGridControl"/>,
    ///   <see cref="IGridControl"/> etc.
    ///</summary>
    public class GridAndBOEditorControlWin : UserControlWin, IGridAndBOEditorControl
    {
        private readonly ClassDef _classDef;
        private IControlFactory _controlFactory;
        private IBusinessObjectControlWithErrorDisplay _businessObjectControl;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButtonGroupControl _buttonGroupControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private IBusinessObject _lastSelectedBusinessObject;
        private IBusinessObject _newBO;

        public event EventHandler<BOEventArgs<IBusinessObject>> BusinessObjectSelected;

        public GridAndBOEditorControlWin(IControlFactory controlFactory, ClassDef classDef, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (classDef == null) throw new ArgumentNullException("classDef");
            _classDef = classDef;
            BusinessObjectControl businessObjectControl = new BusinessObjectControl(controlFactory, classDef, uiDefName);
            SetupGridAndBOEditorControlWin(controlFactory, businessObjectControl, uiDefName);
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, IBusinessObject businessObject)
            :this(controlFactory,businessObjectControl,"default")
        {
            
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, string gridUiDefName)
        {
            SetupGridAndBOEditorControlWin(controlFactory, businessObjectControl, gridUiDefName);
        }

        private void SetupGridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (businessObjectControl == null) throw new ArgumentNullException("businessObjectControl");

            _controlFactory = controlFactory;
            _businessObjectControl = businessObjectControl;

            SetupReadOnlyGridControl(gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.West);
            layoutManager.AddControl(_businessObjectControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttonGroupControl, BorderLayoutManager.Position.South);

            _readOnlyGridControl.Grid.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));
        }


        private void SetupButtonGroupControl()
        {
            _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
        }

        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        private void SetupReadOnlyGridControl(string gridUiDefName)
        {
            _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            _readOnlyGridControl.Width = 300;
            _readOnlyGridControl.Grid.RowHeadersWidth = 25;
            _readOnlyGridControl.Buttons.Visible = false;
            _readOnlyGridControl.FilterControl.Visible = false;
            //ClassDef classDef = (ClassDef) _businessObject.ClassDef;
            if (!string.IsNullOrEmpty(gridUiDefName))
            {
                _readOnlyGridControl.Initialise(_classDef, gridUiDefName);
                int width = GetGridWidthToFitColumns(_readOnlyGridControl.Grid) + 2;
                if (width < 300)
                {
                    _readOnlyGridControl.Width = width;
                }
            }
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            _readOnlyGridControl.DoubleClickEditsBusinessObject = false;
        }

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (BusinessObjectSelected != null)
                this.BusinessObjectSelected(this, new BOEventArgs<IBusinessObject>(businessObject));
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (_newBO != null)
            {
                if (_newBO.Status.IsDirty)
                {
                    if (_newBO.IsValid())
                    {
                        _newBO.Save();
                    }
                    else
                    {
                        _newBO = null;
                    }
                }
            }
            if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!CheckRowSelectionCanChange()) return;
                SavePreviousBusinessObject();
                SetSelectedBusinessObject();
            }
        }

        /// <summary>
        /// Using the RowValidating event did not work as expected, so this method provides
        /// a way to check whether the grid selection should be forced back to the previous selection
        /// </summary>
        private bool CheckRowSelectionCanChange()
        {
            if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!_lastSelectedBusinessObject.IsValid())
                {
                    _businessObjectControl.DisplayErrors();
                    _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
                    return false;
                }
            }
            return true;
        }

        private void SetSelectedBusinessObject()
        {
            IBusinessObject businessObject = CurrentBusinessObject ?? _newBO;
            _businessObjectControl.BusinessObject = businessObject;
            if (businessObject != null)
            {
                businessObject.PropertyUpdated += PropertyUpdated;
            }
            UpdateControlEnabledState();
            _lastSelectedBusinessObject = businessObject;
        }

        private void PropertyUpdated(object sender, BOPropUpdatedEventArgs eventArgs)
        {
            _cancelButton.Enabled = true;
        }

        private void SavePreviousBusinessObject()
        {
            if (_lastSelectedBusinessObject != null && !_lastSelectedBusinessObject.Status.IsDeleted)
            {
                _lastSelectedBusinessObject.Save();
            }
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject ?? _newBO;
            if (currentBO.Status.IsNew)
            {
                _lastSelectedBusinessObject = null;
                _readOnlyGridControl.Grid.GetBusinessObjectCollection().Remove(currentBO);
                SelectLastRowInGrid();
            }
            else
            {
                currentBO.CancelEdits();
            }
            UpdateControlEnabledState();

            //Note: Removing the event prevents the flicker event that happens while a grid is being refreshed
            //  - perhaps the SelectionChanged event should be temporarily removed inside the RefreshGrid method itself?
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            RemoveGridSelectionChangedEvent();
            _readOnlyGridControl.Grid.RefreshGrid();
            AddGridSelectionChangedEvent();
        }

        public void AddGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        public void RemoveGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        private void UpdateControlEnabledState()
        {
            IBusinessObject selectedBusinessObject = CurrentBusinessObject ?? _newBO;
            bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
            if (selectedBusinessObjectNotNull)
            {
                _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty;
                _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
            }
            else
            {
                _businessObjectControl.Enabled = false;
                _deleteButton.Enabled = false;
                _cancelButton.Enabled = false;
            }
            _businessObjectControl.Enabled = selectedBusinessObjectNotNull;
        }

        /// <summary>
        /// Deliberately adds BO to the grid's collection because current habanero model only
        /// adds BO to grid when it is saved.  This causes a problem when you call col.CreateBO(), since
        /// it adds the BO twice and throws a duplicate key exception.
        /// </summary>a
        private void NewButtonClicked(object sender, EventArgs e)
        {
            _newBO = null;
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null)
            {
                if (!currentBO.IsValid())
                {
                    _businessObjectControl.DisplayErrors();
                    return;
                }
                currentBO.Save();
            }

            IBusinessObjectCollection collection = _readOnlyGridControl.Grid.GetBusinessObjectCollection();
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
            _newBO = collection.CreateBusinessObject();
            // _readOnlyGridControl.Grid.GetBusinessObjectCollection().Add(businessObject);
            _readOnlyGridControl.SelectedBusinessObject = _newBO;
            CurrentBusinessObject = _newBO;
            UpdateControlEnabledState();
            //collection.Add(businessObject);
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            GridSelectionChanged(null, null);
            _businessObjectControl.Focus();
            _businessObjectControl.ClearErrors();
            _cancelButton.Enabled = true;
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject businessObject = CurrentBusinessObject;
            businessObject.MarkForDelete();
            businessObject.Save();

            if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
            {
                SelectLastRowInGrid();
            }
        }

        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            if (col == null) throw new ArgumentNullException("col");
            _readOnlyGridControl.SetBusinessObjectCollection(col);
            _newButton.Enabled = true;
        }

        public IGridControl GridControl
        {
            get { return _readOnlyGridControl; }
        }

        public IBusinessObjectControlWithErrorDisplay BusinessObjectControl
        {
            get { return _businessObjectControl; }
        }

        public IButtonGroupControl ButtonGroupControl
        {
            get { return _buttonGroupControl; }
        }

        IBusinessObject IGridAndBOEditorControl.CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
        }

        public IBusinessObject CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }
    }

    /// <summary>
    /// Provides a generic control with a grid on the left, a BO control on the right and buttons at the bottom right.
    /// This is used by a number of screens in Firestarter, but differs from typical Habanero controls in the fact that
    /// there is no Save button - all changes go to the InMemory database.
    /// TODO: This uses ReadOnlyGridControl due to some flaw in ReadOnlyGrid. Look at switching back
    /// to the grid in the future.  What happens when you double-click?
    /// 
    /// TODO:
    /// - grid caret moves all over, even though selected row is correct
    /// </summary>
    public class GridAndBOEditorControlWin<TBusinessObject> : UserControlWin, IGridAndBOEditorControl
        where TBusinessObject : class, IBusinessObject
    {
        private IControlFactory _controlFactory;
        private IBusinessObjectControlWithErrorDisplay _businessObjectControl;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButtonGroupControl _buttonGroupControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private TBusinessObject _lastSelectedBusinessObject;

        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectSelected;

        public GridAndBOEditorControlWin(IControlFactory controlFactory, string uiDefName)
        {
            BusinessObjectControl<TBusinessObject> businessObjectControl = new BusinessObjectControl<TBusinessObject>(controlFactory, uiDefName);
            SetupGridAndBOEditorControlWin(controlFactory, businessObjectControl, uiDefName);
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, IBusinessObject businessObject)
            :this(controlFactory,businessObjectControl,"default")
        {
            
        }

        public GridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, string gridUiDefName)
        {
            SetupGridAndBOEditorControlWin(controlFactory, businessObjectControl, gridUiDefName);
        }

        private void SetupGridAndBOEditorControlWin(IControlFactory controlFactory, IBusinessObjectControlWithErrorDisplay businessObjectControl, string gridUiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (businessObjectControl == null) throw new ArgumentNullException("businessObjectControl");

            _controlFactory = controlFactory;
            _businessObjectControl = businessObjectControl;

            SetupReadOnlyGridControl(gridUiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.West);
            layoutManager.AddControl(_businessObjectControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttonGroupControl, BorderLayoutManager.Position.South);

            _readOnlyGridControl.Grid.BusinessObjectSelected +=
                ((sender, e) => FireBusinessObjectSelected(e.BusinessObject));
        }


        private void SetupButtonGroupControl()
        {
            _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
            _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
        }

        ///<summary>
        ///</summary>
        ///<param name="grid"></param>
        ///<returns></returns>
        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        private void SetupReadOnlyGridControl(string gridUiDefName)
        {
            _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            _readOnlyGridControl.Width = 300;
            _readOnlyGridControl.Grid.RowHeadersWidth = 25;
            _readOnlyGridControl.Buttons.Visible = false;
            _readOnlyGridControl.FilterControl.Visible = false;
            ClassDef classDef = ClassDef.Get<TBusinessObject>();
            if (!string.IsNullOrEmpty(gridUiDefName))
            {
                _readOnlyGridControl.Initialise(classDef, gridUiDefName);
                int width = GetGridWidthToFitColumns(_readOnlyGridControl.Grid) + 2;
                if (width < 300)
                {
                    _readOnlyGridControl.Width = width;
                }
            }
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            _readOnlyGridControl.DoubleClickEditsBusinessObject = false;
        }

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (BusinessObjectSelected != null)
                this.BusinessObjectSelected(this, new BOEventArgs<TBusinessObject>((TBusinessObject)businessObject));
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!CheckRowSelectionCanChange()) return;
                SavePreviousBusinessObject();
                SetSelectedBusinessObject();
            }
        }

        /// <summary>
        /// Using the RowValidating event did not work as expected, so this method provides
        /// a way to check whether the grid selection should be forced back to the previous selection
        /// </summary>
        private bool CheckRowSelectionCanChange()
        {
            if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!_lastSelectedBusinessObject.IsValid())
                {
                    _businessObjectControl.DisplayErrors();
                    _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
                    return false;
                }
            }
            return true;
        }

        private void SetSelectedBusinessObject()
        {
            TBusinessObject businessObject = CurrentBusinessObject;
            _businessObjectControl.BusinessObject = businessObject;
            if (businessObject != null)
            {
                businessObject.PropertyUpdated += PropertyUpdated;
            }
            UpdateControlEnabledState();
            _lastSelectedBusinessObject = businessObject;
        }

        private void PropertyUpdated(object sender, BOPropUpdatedEventArgs eventArgs)
        {
            _cancelButton.Enabled = true;
        }

        private void SavePreviousBusinessObject()
        {
            if (_lastSelectedBusinessObject != null && !_lastSelectedBusinessObject.Status.IsDeleted)
            {
                _lastSelectedBusinessObject.Save();
            }
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO.Status.IsNew)
            {
                _lastSelectedBusinessObject = null;
                _readOnlyGridControl.Grid.GetBusinessObjectCollection().Remove(currentBO);
                SelectLastRowInGrid();
            }
            else
            {
                currentBO.CancelEdits();
            }
            UpdateControlEnabledState();

            //Note: Removing the event prevents the flicker event that happens while a grid is being refreshed
            //  - perhaps the SelectionChanged event should be temporarily removed inside the RefreshGrid method itself?
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            RemoveGridSelectionChangedEvent();
            _readOnlyGridControl.Grid.RefreshGrid();
            AddGridSelectionChangedEvent();
        }

        public void AddGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        }

        public void RemoveGridSelectionChangedEvent()
        {
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        private void UpdateControlEnabledState()
        {
            IBusinessObject selectedBusinessObject = CurrentBusinessObject;
            bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
            if (selectedBusinessObjectNotNull)
            {
                _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty;
                _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
            }
            else
            {
                _businessObjectControl.Enabled = false;
                _deleteButton.Enabled = false;
                _cancelButton.Enabled = false;
            }
            _businessObjectControl.Enabled = selectedBusinessObjectNotNull;
        }

        /// <summary>
        /// Deliberately adds BO to the grid's collection because current habanero model only
        /// adds BO to grid when it is saved.  This causes a problem when you call col.CreateBO(), since
        /// it adds the BO twice and throws a duplicate key exception.
        /// </summary>a
        private void NewButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null)
            {
                if (!currentBO.IsValid())
                {
                    _businessObjectControl.DisplayErrors();
                    return;
                }
                currentBO.Save();
            }

            IBusinessObjectCollection collection = _readOnlyGridControl.Grid.GetBusinessObjectCollection();
            _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
            IBusinessObject businessObject = collection.CreateBusinessObject();
            UpdateControlEnabledState();
            collection.Add(businessObject);
            _readOnlyGridControl.SelectedBusinessObject = businessObject;
            _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
            GridSelectionChanged(null, null);
            _businessObjectControl.Focus();
            _businessObjectControl.ClearErrors();
            _cancelButton.Enabled = true;
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject businessObject = CurrentBusinessObject;
            businessObject.MarkForDelete();
            businessObject.Save();

            if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
            {
                SelectLastRowInGrid();
            }
        }

        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            if (col == null) throw new ArgumentNullException("col");
            _readOnlyGridControl.SetBusinessObjectCollection(col);
            _newButton.Enabled = true;
        }

        public IGridControl GridControl
        {
            get { return _readOnlyGridControl; }
        }

        public IBusinessObjectControlWithErrorDisplay BusinessObjectControl
        {
            get { return _businessObjectControl; }
        }

        ///<summary>
        /// Returns the <see cref="IButtonGroupControl"/> for the 
        ///</summary>
        public IButtonGroupControl ButtonGroupControl
        {
            get { return _buttonGroupControl; }
        }

        IBusinessObject IGridAndBOEditorControl.CurrentBusinessObject
        {
            get { return _readOnlyGridControl.SelectedBusinessObject; }
        }

        ///<summary>
        /// The Current Business Object that is selected in the grid.
        ///</summary>
        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }
    }
}