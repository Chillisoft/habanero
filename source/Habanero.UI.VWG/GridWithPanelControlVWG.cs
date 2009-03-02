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

using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a control to edit a collection of business objects.  A grid
    /// lists the objects as specified by SetBusinessObjectCollection and a control
    /// below the grid allows the selected business object to be edited.  Default
    /// buttons are provided: Save, New, Delete and Cancel.
    /// <br/>
    /// The editing control is
    /// specified here as a IBusinessObjectControl, allowing the developer to pass
    /// in a custom control, but the default instantiation uses a IBusinessObjectPanel,
    /// which is more suited to displaying errors.  If the developer provides a custom
    /// control, they are responsible for updating the business object status
    /// and displaying useful feedback to the user (by
    /// catching appropriate events on the business object or the controls).
    /// <br/>
    /// Some customisation is provided through the GridWithPanelControlStrategy,
    /// including how controls should be enabled for the appropriate environment.
    /// </summary>
    /// TODO: This uses ReadOnlyGridControl due to some flaw in ReadOnlyGrid. Look at switching back
    /// to the grid in the future.  What happens when you double-click?
    public class GridWithPanelControlVWG<TBusinessObject> : UserControlVWG, IGridWithPanelControl<TBusinessObject>
        where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly GridWithPanelControlManager<TBusinessObject> _gridWithPanelControlManager;
        
        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlVWG{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public GridWithPanelControlVWG(IControlFactory controlFactory, string uiDefName)
        {
            IBusinessObjectControl businessObjectControl = new BusinessObjectPanelVWG<TBusinessObject>(controlFactory, uiDefName);
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, controlFactory, businessObjectControl, uiDefName);
            //_gridWithPanelControlManager.SetupControl();
            //SetupControl(controlFactory, businessObjectControl, uiDefName);
            _gridWithPanelControlManager.GridWithPanelControlStrategy = new GridWithPanelControlStrategyVWG<TBusinessObject>(this);
        }
        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlVWG{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="businessObjectControl"></param>
        public GridWithPanelControlVWG(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlVWG{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="businessObjectControl"></param>
        ///<param name="uiDefName"></param>
        public GridWithPanelControlVWG(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        {
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, controlFactory, businessObjectControl, uiDefName);
            //_gridWithPanelControlManager.SetupControl();
            _gridWithPanelControlManager.GridWithPanelControlStrategy = new GridWithPanelControlStrategyVWG<TBusinessObject>(this);
        }

        /// <summary>
        /// Called when the user attempts to move away from a dirty business object
        /// and needs to indicate Yes/No/Cancel to the option of saving.  This delegate
        /// facility is provided primarily to facilitate testing.
        /// </summary>
        public ConfirmSave ConfirmSaveDelegate
        {
            get { return _gridWithPanelControlManager.ConfirmSaveDelegate; }
            set { _gridWithPanelControlManager.ConfirmSaveDelegate = value; }
        }

        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            _gridWithPanelControlManager.SetBusinessObjectCollection(col);
        }

        /// <summary>
        /// Gets the grid control
        /// </summary>
        public IReadOnlyGridControl ReadOnlyGridControl
        {
            get { return _gridWithPanelControlManager.ReadOnlyGridControl; }
        }

        /// <summary>
        /// Gets the control used to edit the selected business object
        /// </summary>
        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _gridWithPanelControlManager.BusinessObjectControl; }
        }

        /// <summary>
        /// Gets the control holding the buttons
        /// </summary>
        public IButtonGroupControl Buttons
        {
            get { return _gridWithPanelControlManager.Buttons; }
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObject"/> of type TBusinessObject.
        ///</summary>
        public TBusinessObject CurrentBusinessObject
        {
            get { return _gridWithPanelControlManager.CurrentBusinessObject; }
        }

        /// <summary>
        /// Gets the strategy used to provide custom behaviour in the control
        /// </summary>
        public IGridWithPanelControlStrategy<TBusinessObject> GridWithPanelControlStrategy
        {
            get { return _gridWithPanelControlManager.GridWithPanelControlStrategy; }
            set { _gridWithPanelControlManager.GridWithPanelControlStrategy = value; }
        }

        /// <summary>
        /// Gets the business object currently selected in the grid
        /// </summary>
        TBusinessObject IGridWithPanelControl<TBusinessObject>.CurrentBusinessObject
        {
            get { return CurrentBusinessObject; }
        }
    }

    /// <summary>
    /// Represents a panel containing a PanelInfo used to edit a single business object.
    /// </summary>
    public class BusinessObjectPanelVWG<T> : UserControlVWG, IBusinessObjectPanel where T : class, IBusinessObject
    {
        private IPanelInfo _panelInfo;

        ///<summary>
        /// Constructor for <see cref="BusinessObjectPanelVWG{T}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public BusinessObjectPanelVWG(IControlFactory controlFactory, string uiDefName)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            _panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre);
        }

        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
            set { _panelInfo = value; }
        }
    }

    /// <summary>
    /// Provides a strategy to add custom behaviour to a GridWithPanelControl
    /// </summary>
    public class GridWithPanelControlStrategyVWG<TBusinessObject> : IGridWithPanelControlStrategy<TBusinessObject>
    {
        private readonly IGridWithPanelControl<TBusinessObject> _gridWithPanelControl;

        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlStrategyVWG{TBusinessObject}"/>
        ///</summary>
        ///<param name="gridWithPanelControl"></param>
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
        public void UpdateControlEnabledState(TBusinessObject lastSelectedBusinessObject)
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
        /// the BO every time a control value changes. This is typically the case with
        /// Web applications e.g. Visual Web Gui but could be used for Windows apps if required.
        /// </summary>
        public bool CallApplyChangesToEditBusinessObject
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates whether the grid should be refreshed.  For instance, a VWG
        /// implementation needs regular refreshes due to the lack of synchronisation. I.e.
        /// the model changes on the server are not pushed to the user interface in an 
        /// individual browser. In a windows forms or other rich client application this is 
        /// not necessary.
        /// In windows Refreshing the grid regularly will therefore 
        /// actually deteriorate the application performance since changes to the business object
        /// model propogate themselves through the application.
        /// </summary>
        public bool RefreshGrid
        {
            get { return true; }
        }
    }
}

