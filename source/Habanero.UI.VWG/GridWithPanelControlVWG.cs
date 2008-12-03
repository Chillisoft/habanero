using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using DialogResult = Habanero.UI.Base.DialogResult;
using MessageBoxButtons = Habanero.UI.Base.MessageBoxButtons;
using MessageBoxIcon = Habanero.UI.Base.MessageBoxIcon;

namespace Habanero.UI.VWG
{


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
    public class GridWithPanelControlVWG<TBusinessObject> : UserControlVWG, IGridWithPanelControl<TBusinessObject>
        where TBusinessObject : class, IBusinessObject, new()
    {
        //private ConfirmSave _confirmSaveDelegate;
        //private IControlFactory _controlFactory;
        //private IBusinessObjectControl _businessObjectControl;
        //private IReadOnlyGridControl _readOnlyGridControl;
        //private IButtonGroupControl _buttonGroupControl;
        //private IButton _newButton;
        //private IButton _deleteButton;
        //private IButton _cancelButton;
        //private TBusinessObject _lastSelectedBusinessObject;
        //private IButton _saveButton;

        private GridWithPanelControlManager<TBusinessObject> _gridWithPanelControlManager;


        public GridWithPanelControlVWG(IControlFactory controlFactory, string uiDefName)
        {
            IBusinessObjectControl businessObjectControl = new BusinessObjectPanelVWG<TBusinessObject>(controlFactory, uiDefName);
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, controlFactory, businessObjectControl, uiDefName);
            //_gridWithPanelControlManager.SetupControl();
            //SetupControl(controlFactory, businessObjectControl, uiDefName);
            _gridWithPanelControlManager.GridWithPanelControlStrategy = new GridWithPanelControlStrategyVWG<TBusinessObject>(this);
        }
        public GridWithPanelControlVWG(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        public GridWithPanelControlVWG(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        {
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, controlFactory, businessObjectControl, uiDefName);
            //_gridWithPanelControlManager.SetupControl();
            _gridWithPanelControlManager.GridWithPanelControlStrategy = new GridWithPanelControlStrategyVWG<TBusinessObject>(this);
        }

        //private void SetupControl(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        //{

        //    if (controlFactory == null) throw new ArgumentNullException("controlFactory");
        //    if (businessObjectControl == null) throw new ArgumentNullException("businessObjectControl");

        //    _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, );
        //    _controlFactory = controlFactory;
        //    _businessObjectControl = businessObjectControl;

        //    SetupReadOnlyGridControl(uiDefName);
        //    SetupButtonGroupControl();
        //    UpdateControlEnabledState();

        //    BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
        //    layoutManager.AddControl(_readOnlyGridControl, BorderLayoutManager.Position.North);
        //    layoutManager.AddControl(_businessObjectControl, BorderLayoutManager.Position.Centre);
        //    layoutManager.AddControl(_buttonGroupControl, BorderLayoutManager.Position.South);

        //    ConfirmSaveDelegate += CheckUserWantsToSave;
        //}

        //private void SetupButtonGroupControl()
        //{
        //    _buttonGroupControl = _controlFactory.CreateButtonGroupControl();
        //    _cancelButton = _buttonGroupControl.AddButton("Cancel", CancelButtonClicked);
        //    _deleteButton = _buttonGroupControl.AddButton("Delete", DeleteButtonClicked);
        //    _newButton = _buttonGroupControl.AddButton("New", NewButtonClicked);
        //    _saveButton = _buttonGroupControl.AddButton("Save", SaveButtonClicked);
        //    _cancelButton.Enabled = false;
        //    _deleteButton.Enabled = false;
        //    _newButton.Enabled = false;
        //    _saveButton.Enabled = false;
        //}

        //private void SaveButtonClicked(object sender, EventArgs e)
        //{
        //    IBusinessObject currentBO = CurrentBusinessObject;
        //    if (currentBO != null)
        //    {
        //        if (!currentBO.IsValid())
        //        {
        //            //_businessObjectControl.DisplayErrors();
        //            return;
        //        }
        //        currentBO.Save();
        //        UpdateControlEnabledState();
        //    }
        //}

        //private void SetupReadOnlyGridControl(string gridUiDefName)
        //{
        //    _readOnlyGridControl = _controlFactory.CreateReadOnlyGridControl();
        //    _readOnlyGridControl.Height = 300;
        //    _readOnlyGridControl.Buttons.Visible = false;
        //    _readOnlyGridControl.FilterControl.Visible = false;
        //    ClassDef classDef = ClassDef.Get<TBusinessObject>();
        //    if (!string.IsNullOrEmpty(gridUiDefName)) _readOnlyGridControl.Initialise(classDef, gridUiDefName);

        //    _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        //    _readOnlyGridControl.DisableDefaultRowDoubleClickEventHandler();
        //}

        //private void GridSelectionChanged(object sender, EventArgs e)
        //{
        //    if (_lastSelectedBusinessObject == null || _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
        //    {
        //        if (!CheckRowSelectionCanChange()) return;
        //        SetSelectedBusinessObject();
        //    }
        //}

        ///// <summary>
        ///// Using the RowValidating event did not work as expected, so this method provides
        ///// a way to check whether the grid selection should be forced back to the previous selection
        ///// </summary>
        //private bool CheckRowSelectionCanChange()
        //{
        //    if (_lastSelectedBusinessObject != null && _readOnlyGridControl.SelectedBusinessObject != _lastSelectedBusinessObject)
        //    {
        //        if (!_lastSelectedBusinessObject.IsValid())
        //        {
        //            //TODO: indicate BO invalidity to user (eg. flash error providers)
        //            _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
        //            return false;
        //        }

        //        if (!_lastSelectedBusinessObject.Status.IsDirty) return true;

        //        DialogResult dialogResult = ConfirmSaveDelegate();
        //        if (dialogResult == DialogResult.Yes)
        //        {
        //            _lastSelectedBusinessObject.Save();
        //            return true;
        //        }
        //        if (dialogResult == DialogResult.No)
        //        {
        //            CancelChangesToBusinessObject(_lastSelectedBusinessObject, false);
        //            return true;
        //        }

        //        _readOnlyGridControl.SelectedBusinessObject = _lastSelectedBusinessObject;
        //        return false;
        //    }
        //    return true;
        //}

        //private void SetSelectedBusinessObject()
        //{
        //    TBusinessObject businessObject = CurrentBusinessObject;
        //    _businessObjectControl.BusinessObject = businessObject;
        //    if (businessObject != null)
        //    {
        //        businessObject.PropertyUpdated += PropertyUpdated;
        //    }
        //    UpdateControlEnabledState();
        //    _lastSelectedBusinessObject = businessObject;
        //}

        //private void PropertyUpdated(object sender, BOEventArgs e)
        //{
        //    _cancelButton.Enabled = true;
        //    _saveButton.Enabled = true;
        //    _newButton.Enabled = false;
        //}

        //private void CancelButtonClicked(object sender, EventArgs e)
        //{
        //    IBusinessObject currentBO = CurrentBusinessObject;
        //    CancelChangesToBusinessObject(currentBO, true);
        //    UpdateControlEnabledState();

        //    //Note: Removing the event prevents the flicker event that happens while a grid is being refreshed
        //    //  - perhaps the SelectionChanged event should be temporarily removed inside the RefreshGrid method itself?
        //    _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        //    _readOnlyGridControl.Grid.RefreshGrid();
        //    _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        //}

        //private void CancelChangesToBusinessObject(IBusinessObject currentBO, bool selectLastRowInGrid)
        //{
        //    if (currentBO.Status.IsNew)
        //    {
        //        _lastSelectedBusinessObject = null;
        //        _readOnlyGridControl.Grid.GetBusinessObjectCollection().Remove(currentBO);
        //        if (selectLastRowInGrid) SelectLastRowInGrid();
        //    }
        //    else
        //    {
        //        currentBO.Restore();
        //        _readOnlyGridControl.Grid.RefreshGrid();
        //    }
        //}

        //private void SelectLastRowInGrid()
        //{
        //    int rowCount = _readOnlyGridControl.Grid.Rows.Count - 1;
        //    IBusinessObject lastObjectInGrid = _readOnlyGridControl.Grid.GetBusinessObjectAtRow(rowCount);
        //    _readOnlyGridControl.SelectedBusinessObject = lastObjectInGrid;
        //}

        //private void UpdateControlEnabledState()
        //{
        //    IBusinessObject selectedBusinessObject = CurrentBusinessObject;
        //    bool selectedBusinessObjectNotNull = (selectedBusinessObject != null);
        //    if (selectedBusinessObjectNotNull)
        //    {
        //        _saveButton.Enabled = selectedBusinessObject.Status.IsDirty;
        //        _cancelButton.Enabled = selectedBusinessObject.Status.IsDirty;
        //        _deleteButton.Enabled = !selectedBusinessObject.Status.IsNew;
        //        _newButton.Enabled = !_saveButton.Enabled;
        //    }
        //    else
        //    {
        //        _businessObjectControl.Enabled = false;
        //        _deleteButton.Enabled = false;
        //        _saveButton.Enabled = false;
        //        _cancelButton.Enabled = false;
        //        _newButton.Enabled = _readOnlyGridControl.Grid.GetBusinessObjectCollection() !=null;
        //    }
        //    _businessObjectControl.Enabled = selectedBusinessObjectNotNull;
        //}

        ///// <summary>
        ///// Deliberately adds BO to the grid's collection because current habanero model only
        ///// adds BO to grid when it is saved.  This causes a problem when you call col.CreateBO(), since
        ///// it adds the BO twice and throws a duplicate key exception.
        ///// </summary>
        //private void NewButtonClicked(object sender, EventArgs e)
        //{
        //    IBusinessObject currentBO = CurrentBusinessObject;
        //    if (currentBO != null)
        //    {
        //        if (!currentBO.IsValid())
        //        {
        //            //_businessObjectControl.DisplayErrors();
        //            return;
        //        }
        //        currentBO.Save();
        //    }

        //    IBusinessObjectCollection collection = _readOnlyGridControl.Grid.GetBusinessObjectCollection();
        //    _readOnlyGridControl.Grid.SelectionChanged -= GridSelectionChanged;
        //    IBusinessObject businessObject = collection.CreateBusinessObject();
        //    UpdateControlEnabledState();
        //    _readOnlyGridControl.SelectedBusinessObject = businessObject;
        //    _readOnlyGridControl.Grid.SelectionChanged += GridSelectionChanged;
        //    GridSelectionChanged(null, null);
        //    _businessObjectControl.Focus();
        //    //_businessObjectControl.ClearErrors();
        //    _cancelButton.Enabled = true;
        //}

        //private void DeleteButtonClicked(object sender, EventArgs e)
        //{
        //    IBusinessObject businessObject = CurrentBusinessObject;
        //    businessObject.Delete();
        //    businessObject.Save();

        //    if (CurrentBusinessObject == null && _readOnlyGridControl.Grid.Rows.Count > 0)
        //    {
        //        SelectLastRowInGrid();
        //    }
        //}

        ///// <summary>
        ///// Sets the business object collection to populate the grid.  If the grid
        ///// needs to be cleared, set an empty collection rather than setting to null.
        ///// Until you set a collection, the controls are disabled, since any given
        ///// collection needs to be provided by a suitable context.
        ///// </summary>
        //public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        //{
        //    if (col == null) throw new ArgumentNullException("col");
        //    _readOnlyGridControl.SetBusinessObjectCollection(col);
        //    _newButton.Enabled = true;


        //}

        //public IReadOnlyGridControl ReadOnlyGridControl
        //{
        //    get { return _readOnlyGridControl; }
        //}

        //public IBusinessObjectControl BusinessObjectControl
        //{
        //    get { return _businessObjectControl; }
        //}

        //public IButtonGroupControl Buttons
        //{
        //    get { return _buttonGroupControl; }
        //}

        //IBusinessObject IGridWithPanelControl.CurrentBusinessObject
        //{
        //    get { return CurrentBusinessObject; }
        //}

        //public TBusinessObject CurrentBusinessObject
        //{
        //    get { return (TBusinessObject)_readOnlyGridControl.SelectedBusinessObject; }
        //}

        public ConfirmSave ConfirmSaveDelegate
        {
            get { return _gridWithPanelControlManager.ConfirmSaveDelegate; }
            set { _gridWithPanelControlManager.ConfirmSaveDelegate = value; }
        }

        ///// <summary>
        ///// Displays a message box to the user to check if they want to save
        ///// the selected business object.
        ///// </summary>
        ///// <returns>Returns true if the user does want to delete</returns>
        //public DialogResult CheckUserWantsToSave()
        //{
        //    return _controlFactory.ShowMessageBox(
        //                      "Would you like to save your changes?",
        //                      "Save Changes?",
        //                      MessageBoxButtons.YesNoCancel,
        //                      MessageBoxIcon.Question);
        //}

        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            _gridWithPanelControlManager.SetBusinessObjectCollection(col);
        }

        public IReadOnlyGridControl ReadOnlyGridControl
        {
            get { return _gridWithPanelControlManager.ReadOnlyGridControl; }
        }

        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _gridWithPanelControlManager.BusinessObjectControl; }
        }

        public IButtonGroupControl Buttons
        {
            get { return _gridWithPanelControlManager.Buttons; }
        }

        public TBusinessObject CurrentBusinessObject
        {
            get { return _gridWithPanelControlManager.CurrentBusinessObject; }
        }

        public IGridWithPanelControlStrategy<TBusinessObject> GridWithPanelControlStrategy
        {
            get { return _gridWithPanelControlManager.GridWithPanelControlStrategy; }
            set { _gridWithPanelControlManager.GridWithPanelControlStrategy = value; }
        }

        TBusinessObject IGridWithPanelControl<TBusinessObject>.CurrentBusinessObject
        {
            get { return CurrentBusinessObject; }
        }

    }

    public class BusinessObjectPanelVWG<T> : UserControlVWG, IBusinessObjectPanel where T : class, IBusinessObject
    {
        private IPanelInfo _panelInfo;

        public BusinessObjectPanelVWG(IControlFactory controlFactory, string uiDefName)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            _panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre);
        }

        public IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
            set { _panelInfo = value; }
        }
    }

    public class GridWithPanelControlStrategyVWG<TBusinessObject> : IGridWithPanelControlStrategy<TBusinessObject>
    {
        private IGridWithPanelControl<TBusinessObject> _gridWithPanelControl;

        public GridWithPanelControlStrategyVWG(IGridWithPanelControl<TBusinessObject> gridWithPanelControl)
        {
            _gridWithPanelControl = gridWithPanelControl;
        }

        /// <summary>
        /// Provides custom control state.  Since this is called after the default
        /// implementation, it overrides it.
        /// </summary>
        /// <param name="lastSelectedBusinessObject">The previous selected business
        /// object in the grid - used to revert when a user tries to change a grid
        /// row while an object is dirty or invalid</param>
        public void UpdateControlEnabledState(IBusinessObject lastSelectedBusinessObject)
        {
            IButton cancelButton = _gridWithPanelControl.Buttons["Cancel"];
            IButton deleteButton = _gridWithPanelControl.Buttons["Delete"];
            IButton saveButton = _gridWithPanelControl.Buttons["Save"];
            IButton newButton = _gridWithPanelControl.Buttons["New"];

            if (_gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count == 0)
            {
                cancelButton.Enabled = false;
                deleteButton.Enabled = false;
                saveButton.Enabled = false;
                newButton.Enabled = true;
            }
            else
            {
                cancelButton.Enabled = true;
                deleteButton.Enabled = true;
                saveButton.Enabled = true;
                newButton.Enabled = true;
            }
        }

        /// <summary>
        /// Whether to show the save confirmation dialog when moving away from
        /// a dirty object
        /// </summary>
        public bool ShowConfirmSaveDialog
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates whether PanelInfo.ApplyChangesToBusinessObject needs to be
        /// called to copy control values to the business object.  This will be
        /// the case if the application uses minimal events and does not update
        /// the BO every time a control value changes.
        /// </summary>
        public bool CallApplyChangesToEditBusinessObject
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates whether the grid should be refreshed.  For instance, a VWG
        /// implementation needs regular refreshes due to the lack of synchronisation,
        /// but this behaviour has some adverse affects in the WinForms implementation
        /// </summary>
        public bool RefreshGrid
        {
            get { return true; }
        }
    }
}

