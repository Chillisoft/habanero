using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using UserControlWin=Habanero.UI.Win.UserControlWin;

namespace Habanero.UI.Win
{
    public class BusinessObjectControl : UserControlWin, IBusinessObjectControlWithErrorDisplay
    {
        private IPanelInfo _panelInfo;

        public BusinessObjectControl(IControlFactory controlFactory,ClassDef classDef, string uiDefName)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
          
            UIForm uiForm = classDef.UIDefCol[uiDefName].UIForm;
            //_panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            _panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre);
        }

        public IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        public void DisplayErrors()
        {
            _panelInfo.ApplyChangesToBusinessObject();
        }

        public void ClearErrors()
        {
            _panelInfo.ClearErrorProviders();
        }
    }

    public class BusinessObjectControl<T> : UserControlWin, IBusinessObjectControlWithErrorDisplay
        where T : class, IBusinessObject
    {
        private readonly IPanelInfo _panelInfo;

        public BusinessObjectControl(IControlFactory controlFactory, string uiDefName)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            //UIForm uiForm = ClassDef.ClassDefs[businessObject.GetType()].UIDefCol[uiDefName].UIForm;
            _panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            //_panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre);
        }

        public IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        public void DisplayErrors()
        {
            _panelInfo.ApplyChangesToBusinessObject();
        }

        public void ClearErrors()
        {
            _panelInfo.ClearErrorProviders();
        }
    }

    class GridAndBOEditorControlWin : UserControlWin, IGridAndBOEditorControl
    {
        private readonly ClassDef _classDef;
        private readonly IBusinessObject _businessObject;
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
            _classDef = classDef;
            // _businessObject = businessObject;
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
                delegate(object sender, BOEventArgs e) { FireBusinessObjectSelected(e.BusinessObject); };
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

        private void PropertyUpdated(object sender, BOEventArgs e)
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
                currentBO.Restore();
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
            businessObject.Delete();
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

        public IReadOnlyGridControl ReadOnlyGridControl
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
                delegate(object sender, BOEventArgs e) { FireBusinessObjectSelected(e.BusinessObject); };
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

        private void PropertyUpdated(object sender, BOEventArgs e)
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
                currentBO.Restore();
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
            businessObject.Delete();
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

        public IReadOnlyGridControl ReadOnlyGridControl
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

        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
            set { _readOnlyGridControl.SelectedBusinessObject = value; }
        }
    }
}