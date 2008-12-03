using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using DialogResult=Habanero.UI.Base.DialogResult;
using MessageBoxButtons=Habanero.UI.Base.MessageBoxButtons;
using MessageBoxIcon=Habanero.UI.Base.MessageBoxIcon;

namespace Habanero.UI.Win
{
    /// <summary>
    /// /// <summary>
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
    public class GridWithPanelControlWin<TBusinessObject> : UserControlWin, IGridWithPanelControl<TBusinessObject> 
                where TBusinessObject : class, IBusinessObject, new()
    {
        private GridWithPanelControlManager<TBusinessObject> _gridWithPanelControlManager; 
        
        public GridWithPanelControlWin(IControlFactory controlFactory, string uiDefName)
        {
            IBusinessObjectControl businessObjectControl = new BusinessObjectPanelWin<TBusinessObject>(controlFactory, uiDefName);
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this,controlFactory,businessObjectControl,uiDefName);
        }

        public GridWithPanelControlWin(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        public GridWithPanelControlWin(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl, string uiDefName)
        {
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this, controlFactory, businessObjectControl, uiDefName);
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
    }

    /// <summary>
    /// Represents a panel containing a PanelInfo used to edit a single business object.
    /// </summary>
    public class BusinessObjectPanelWin<T> : UserControlWin, IBusinessObjectPanel where T : class, IBusinessObject
    {
        private IPanelInfo _panelInfo;

        public BusinessObjectPanelWin(IControlFactory controlFactory, string uiDefName)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            _panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre );
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
}