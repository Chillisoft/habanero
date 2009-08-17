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
using System.Drawing;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// TODO: does win version flash error providers?
    /// TODO: consider when to remove BusinessObjectUpdated event (it is attached and never removed)
    /// </summary>
    [Obsolete("This has been replaced by IBOGridAndEditorControl : Brett 03 Mar 2009")]
    public class GridWithPanelControlManager<TBusinessObject> where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly IGridWithPanelControl<TBusinessObject> _gridWithPanelControl;
        private IGridWithPanelControlStrategy<TBusinessObject> _strategy;
        private IControlFactory _controlFactory;
        private IBusinessObjectControl _businessObjectControl;
        private IButtonGroupControl _buttonControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private TBusinessObject _lastSelectedBusinessObject;
        private IButton _saveButton;

        ///<summary>
        /// Constructor for the <see cref="GridWithPanelControlManager{TBusinessObject}"/>
        ///</summary>
        ///<param name="gridWithPanelControl"></param>
        ///<param name="controlFactory"></param>
        ///<param name="businessObjectControl"></param>
        ///<param name="uiDefName"></param>
        public GridWithPanelControlManager(IGridWithPanelControl<TBusinessObject> gridWithPanelControl, IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        {
            _gridWithPanelControl = gridWithPanelControl;
            SetupControl(controlFactory, businessObjectControl, uiDefName);
        }

        private void SetupControl(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (businessObjectControl == null) throw new ArgumentNullException("businessObjectControl");

            _controlFactory = controlFactory;
            _businessObjectControl = businessObjectControl;

            SetupReadOnlyGridControl(uiDefName);
            SetupButtonGroupControl();
            UpdateControlEnabledState();

            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(_gridWithPanelControl);
            layoutManager.AddControl(ReadOnlyGridControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(_businessObjectControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttonControl, BorderLayoutManager.Position.South);

            ConfirmSaveDelegate += CheckUserWantsToSave;
        }

        private void SetupReadOnlyGridControl(string gridUiDefName)
        {
            ReadOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            ReadOnlyGridControl.Height = 300;
            ReadOnlyGridControl.Buttons.Visible = false;
            ReadOnlyGridControl.FilterControl.Visible = false;
            IClassDef classDef = ClassDef.Get<TBusinessObject>();
            if (!string.IsNullOrEmpty(gridUiDefName)) ReadOnlyGridControl.Initialise(classDef, gridUiDefName);

            AddGridSelectionEvent();
            ReadOnlyGridControl.DoubleClickEditsBusinessObject = false;
        }

        private void SetupButtonGroupControl()
        {
            _buttonControl = _controlFactory.CreateButtonGroupControl();
            _cancelButton = _buttonControl.AddButton("Cancel", CancelButtonClicked);
            _deleteButton = _buttonControl.AddButton("Delete", DeleteButtonClicked);
            _newButton = _buttonControl.AddButton("New", NewButtonClicked);
            _saveButton = _buttonControl.AddButton("Save", SaveButtonClicked);
            _cancelButton.Enabled = false;
            _deleteButton.Enabled = false;
            _newButton.Enabled = false;
            _saveButton.Enabled = false;
        }

        private void AddGridSelectionEvent()
        {
            ReadOnlyGridControl.Grid.BusinessObjectSelected += GridSelectionChanged;
        }

        private void RemoveGridSelectionEvent()
        {
            ReadOnlyGridControl.Grid.BusinessObjectSelected -= GridSelectionChanged;
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (_lastSelectedBusinessObject == null || ReadOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                if (!CheckRowSelectionCanChange()) return;
                SetSelectedBusinessObject();
            }
        }

        /// <summary>
        /// Using the RowValidating event did not work as expected, so this method provides
        /// a way to check whether the grid selection should be forced back to the previous selection
        /// </summary>
        private bool CheckRowSelectionCanChange()
        {
            if (_lastSelectedBusinessObject != null && ReadOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
            {
                CallApplyChangesOnPanelInfo_IfStrategyAllows();
                if (!_lastSelectedBusinessObject.IsValid())
                {
                    SelectBusinessObjectInGrid_NoEvents(_lastSelectedBusinessObject);
                    CallApplyChangesOnPanelInfo();  // to flash the error providers
                    RefreshGrid_IfStrategyAllows();  // otherwise incorrect row is still selected
                    return false;
                }

                if (!_lastSelectedBusinessObject.Status.IsDirty) return true;

                bool mustConfirmSaveWithUser = (_strategy == null || _strategy.ShowConfirmSaveDialog);
                if (mustConfirmSaveWithUser)
                {
                    DialogResult dialogResult = ConfirmSaveDelegate();
                    if (dialogResult == DialogResult.Yes)
                    {
                        _lastSelectedBusinessObject.Save();
                        return true;
                    }
                    if (dialogResult == DialogResult.No)
                    {
                        CancelChangesToBusinessObject(_lastSelectedBusinessObject, false);
                        RefreshGrid();
                        return true;
                    }
                    //DialogResult.Cancel falls through to code below
                }

                SelectBusinessObjectInGrid_NoEvents(_lastSelectedBusinessObject);
                NotifyUserOfDirtyStatus();
                RefreshGrid_IfStrategyAllows();  // otherwise incorrect row is still selected
                return false;
            }
            return true;
        }

        private void SetSelectedBusinessObject()
        {
            TBusinessObject businessObjectInGrid = CurrentBusinessObject;
            _businessObjectControl.BusinessObject = businessObjectInGrid;
            if (businessObjectInGrid != null)
            {
                //TODO - when do we remove this event?
                businessObjectInGrid.PropertyUpdated += BusinessObjectUpdated;
            }
            UpdateControlEnabledState();
            _lastSelectedBusinessObject = businessObjectInGrid;
        }

        private void BusinessObjectUpdated(object sender, BOPropUpdatedEventArgs eventArgs)
        {
            UpdateControlEnabledState();
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = ReadOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = ReadOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            ReadOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        /// <summary>
        /// Selects a BO in the grid without firing the events, which has unintended results
        /// </summary>
        private void SelectBusinessObjectInGrid_NoEvents(IBusinessObject businessObject)
        {
            RemoveGridSelectionEvent();
            ReadOnlyGridControl.SelectedBusinessObject = businessObject;
            AddGridSelectionEvent();
        }

        private void RefreshGrid_IfStrategyAllows()
        {
            if (_strategy != null && _strategy.RefreshGrid)
            {
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            // Removing the event prevents the flicker event that happens while a grid is being refreshed
            //  - perhaps the SelectionChanged event should be temporarily removed inside the RefreshGrid method itself?
            RemoveGridSelectionEvent();
            ReadOnlyGridControl.Grid.RefreshGrid();
            AddGridSelectionEvent();
        }

        private void RefreshBusinessObjectControl()
        {
            if (_businessObjectControl is IBusinessObjectPanel)
            {
                IBusinessObjectPanel businessObjectPanel = (IBusinessObjectPanel)_businessObjectControl;
                businessObjectPanel.PanelInfo.BusinessObject = CurrentBusinessObject;
            }
        }

        private void SaveButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null)
            {
                CallApplyChangesOnPanelInfo();

                if (!currentBO.IsValid()) return;

                currentBO.Save();
                RefreshGrid_IfStrategyAllows();
                UpdateControlEnabledState();
            }
        }

        // What about giving a save (yes/no/cancel) option, if so, duplicate the code from CheckRowSelectionCanChange
        private void NewButtonClicked(object sender, EventArgs e)
        {
            IBusinessObject currentBO = CurrentBusinessObject;
            if (currentBO != null)
            {
                CallApplyChangesOnPanelInfo_IfStrategyAllows();

                if (!currentBO.IsValid())
                {
                    CallApplyChangesOnPanelInfo();  // to flash the error providers
                    return;
                }
                NotifyUserOfDirtyStatus();
                if (currentBO.Status.IsDirty) return;
            }

            IBusinessObjectCollection collection = ReadOnlyGridControl.Grid.BusinessObjectCollection;
            IBusinessObject newBusinessObject = collection.CreateBusinessObject();
            SelectBusinessObjectInGrid_NoEvents(newBusinessObject);

            GridSelectionChanged(null, null);
            UpdateControlEnabledState();
            _businessObjectControl.Focus();
            //_businessObjectControl.ClearErrors();
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            if (CurrentBusinessObject == null) return;

            IBusinessObject businessObject = CurrentBusinessObject;
            RemoveGridSelectionEvent();
            if (businessObject.Status.IsNew)
            {
                //RemoveGridSelectionEvent();
                ReadOnlyGridControl.SelectedBusinessObject = null;
                ReadOnlyGridControl.Grid.BusinessObjectCollection.Remove(businessObject);
                //AddGridSelectionEvent();
            }
            else
            {
                businessObject.MarkForDelete();
                businessObject.Save();
            }
            AddGridSelectionEvent();
            _lastSelectedBusinessObject = null;

            if (CurrentBusinessObject == null && ReadOnlyGridControl.Grid.Rows.Count > 0)
            {
                SelectLastRowInGrid();
            }
            UpdateControlEnabledState();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            if (CurrentBusinessObject != null)
            {
                IBusinessObject currentBO = CurrentBusinessObject;
                CancelChangesToBusinessObject(currentBO, true);
            }
            UpdateControlEnabledState();
            RefreshGrid();
            RefreshBusinessObjectControl();
        }

        private void CancelChangesToBusinessObject(IBusinessObject currentBO, bool selectLastRowInGrid)
        {
            if (currentBO.Status.IsNew)
            {
                _lastSelectedBusinessObject = null;
                ReadOnlyGridControl.Grid.BusinessObjectCollection.Remove(currentBO);
                if (selectLastRowInGrid) SelectLastRowInGrid();
            }
            else
            {
                currentBO.CancelEdits();
            }
        }

        private void UpdateControlEnabledState()
        {
            IBusinessObject selectedBusinessObject = CurrentBusinessObject;
            bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
            if (selectedBusinessObjectNotNull)
            {
                _saveButton.Enabled = selectedBusinessObject.Status.IsDirty || selectedBusinessObject.Status.IsNew;
                _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty || selectedBusinessObject.Status.IsNew;
                _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
                _newButton.Enabled = !_saveButton.Enabled;
            }
            else
            {
                _deleteButton.Enabled = false;
                _saveButton.Enabled = false;
                _cancelButton.Enabled = false;
                _newButton.Enabled = ReadOnlyGridControl.Grid.BusinessObjectCollection != null;
            }
            _businessObjectControl.Enabled = selectedBusinessObjectNotNull;

            _saveButton.Font = new Font(_saveButton.Font, FontStyle.Regular);   // Set to bold in NotifyUserOfDirtyStatus
            _cancelButton.Font = new Font(_cancelButton.Font, FontStyle.Regular);

            if (_strategy != null)
            {
                _strategy.UpdateControlEnabledState(_lastSelectedBusinessObject);
            }
        }

        private void NotifyUserOfDirtyStatus()
        {
            _saveButton.Font = new Font(_saveButton.Font, FontStyle.Bold);
            _cancelButton.Font = new Font(_cancelButton.Font, FontStyle.Bold);
        }

        private void CallApplyChangesOnPanelInfo_IfStrategyAllows()
        {
            if (_strategy != null && _strategy.CallApplyChangesToEditBusinessObject)
            {
                CallApplyChangesOnPanelInfo();
            }
        }

        /// <summary>
        /// If a custom IBusinessObjectControl has been provided (ie. not an IBusinessObjectPanel)
        /// then the developer is responsible for appropriate feedback to the user and updating
        /// of the business object status.
        /// </summary>
        private void CallApplyChangesOnPanelInfo()
        {
            if (_businessObjectControl is IBusinessObjectPanel)
            {
                IBusinessObjectPanel businessObjectPanel = (IBusinessObjectPanel)_businessObjectControl;
                businessObjectPanel.PanelInfo.ApplyChangesToBusinessObject();
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
            ReadOnlyGridControl.SetBusinessObjectCollection(col);
            ReadOnlyGridControl.SelectedBusinessObject = col.Count > 0 ? col[0] : null;
            _newButton.Enabled = true;
        }

        ///<summary>
        /// Returns the ReadOnly Grid Control that is used as the Selector control for the <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        public IReadOnlyGridControl  ReadOnlyGridControl { get; private set; }

        ///<summary>
        /// Returns the Business Object Control that is used for editing the Business Object for the <see cref="IBOGridAndEditorControl"/>
        ///</summary>
        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _businessObjectControl; }
        }

        ///<summary>
        /// Returns the <see cref="IButtonGroupControl"/> that shows the buttons that are available on this control.
        /// Typically these buttons provide the Add, Save, Cancel Edits.
        ///</summary>
        public IButtonGroupControl Buttons
        {
            get { return _buttonControl; }
        }

        ///<summary>
        /// Returns the Business Object Currently Selected in the Selector and being viewed in the <see cref="IBusinessObjectControl"/>
        ///</summary>
        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)ReadOnlyGridControl.SelectedBusinessObject; }
        }

        ///<summary>
        /// Gets and sets the delegate that is used when the business object is to be saved (e.g. when the save button is clicked)
        ///</summary>
        public ConfirmSave ConfirmSaveDelegate { get; set; }

        ///<summary>
        /// Gets and sets the <see cref="IGridWithPanelControlStrategy{TBusinessObject}"/>
        ///</summary>
        public IGridWithPanelControlStrategy<TBusinessObject> GridWithPanelControlStrategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        /// <summary>
        /// Displays a message box to the user to check if they want to save
        /// the selected business object.
        /// </summary>
        public DialogResult CheckUserWantsToSave()
        {
            return _controlFactory.ShowMessageBox(
                              "Would you like to save your changes?",
                              "Save Changes?",
                              MessageBoxButtons.YesNoCancel,
                              MessageBoxIcon.Question);
        }
    }
}
