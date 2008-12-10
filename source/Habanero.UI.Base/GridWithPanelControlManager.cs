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
    public class GridWithPanelControlManager<TBusinessObject> where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly IGridWithPanelControl<TBusinessObject> _gridWithPanelControl;
        private IGridWithPanelControlStrategy<TBusinessObject> _strategy;
        private ConfirmSave _confirmSaveDelegate;
        private IControlFactory _controlFactory;
        private IBusinessObjectControl _businessObjectControl;
        private IReadOnlyGridControl _readOnlyGridControl;
        private IButtonGroupControl _buttonControl;
        private IButton _newButton;
        private IButton _deleteButton;
        private IButton _cancelButton;
        private TBusinessObject _lastSelectedBusinessObject;
        private IButton _saveButton;

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
            layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.North);
            layoutManager.AddControl(_businessObjectControl, BorderLayoutManager.Position.Centre);
            layoutManager.AddControl(_buttonControl, BorderLayoutManager.Position.South);

            ConfirmSaveDelegate += CheckUserWantsToSave;
        }

        private void SetupReadOnlyGridControl(string gridUiDefName)
        {
            _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
            _readOnlyGridControl.Height = 300;
            _readOnlyGridControl.Buttons.Visible = false;
            _readOnlyGridControl.FilterControl.Visible = false;
            ClassDef classDef = ClassDef.Get<TBusinessObject>();
            if (!string.IsNullOrEmpty(gridUiDefName)) _readOnlyGridControl.Initialise(classDef, gridUiDefName);

            AddGridSelectionEvent();
            _readOnlyGridControl.DisableDefaultRowDoubleClickEventHandler();
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
            _readOnlyGridControl.Grid.BusinessObjectSelected += GridSelectionChanged;
        }

        private void RemoveGridSelectionEvent()
        {
            _readOnlyGridControl.Grid.BusinessObjectSelected -= GridSelectionChanged;
        }

        private void GridSelectionChanged(object sender, EventArgs e)
        {
            if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
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
            if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
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

        private void BusinessObjectUpdated(object sender, BOEventArgs e)
        {
            UpdateControlEnabledState();
        }

        private void SelectLastRowInGrid()
        {
            int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
            IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
            _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        }

        /// <summary>
        /// Selects a BO in the grid without firing the events, which has unintended results
        /// </summary>
        private void SelectBusinessObjectInGrid_NoEvents(IBusinessObject businessObject)
        {
            RemoveGridSelectionEvent();
            _readOnlyGridControl.SelectedBusinessObject = businessObject;
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
            _readOnlyGridControl.Grid.RefreshGrid();
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

            IBusinessObjectCollection collection = _readOnlyGridControl.Grid.GetBusinessObjectCollection();
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
                _readOnlyGridControl.SelectedBusinessObject = null;
                _readOnlyGridControl.Grid.GetBusinessObjectCollection().Remove(businessObject);
                //AddGridSelectionEvent();
            }
            else
            {
                businessObject.Delete();
                businessObject.Save();
            }
            AddGridSelectionEvent();
            _lastSelectedBusinessObject = null;

            if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
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
                _readOnlyGridControl.Grid.GetBusinessObjectCollection().Remove(currentBO);
                if (selectLastRowInGrid) SelectLastRowInGrid();
            }
            else
            {
                currentBO.Restore();
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
                _newButton.Enabled = _readOnlyGridControl.Grid.GetBusinessObjectCollection() != null;
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
            _readOnlyGridControl.SetBusinessObjectCollection(col);
            if (col.Count > 0)
            {
                _readOnlyGridControl.SelectedBusinessObject = col[0];
            }
            else
            {
                _readOnlyGridControl.SelectedBusinessObject = null;
            }
            _newButton.Enabled = true;
        }

        public IReadOnlyGridControl ReadOnlyGridControl
        {
            get { return _readOnlyGridControl; }
        }

        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _businessObjectControl; }
        }

        public IButtonGroupControl Buttons
        {
            get { return _buttonControl; }
        }

        public TBusinessObject CurrentBusinessObject
        {
            get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
        }

        public ConfirmSave ConfirmSaveDelegate
        {
            get { return _confirmSaveDelegate; }
            set { _confirmSaveDelegate = value; }
        }

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
