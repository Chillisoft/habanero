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
    /// TODO: This uses ReadOnlyGridControl due to some flaw in ReadOnlyGrid. Look at switching back
    /// to the grid in the future.  What happens when you double-click?
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
    [Obsolete("This has been replaced by IBOSelectorAndEditor : Brett 03 Mar 2009")]
    public class GridWithPanelControlWin<TBusinessObject> : UserControlWin, IGridWithPanelControl<TBusinessObject> 
                where TBusinessObject : class, IBusinessObject, new()
    {
        private readonly GridWithPanelControlManager<TBusinessObject> _gridWithPanelControlManager; 
        
        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlWin{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public GridWithPanelControlWin(IControlFactory controlFactory, string uiDefName)
        {
            IBusinessObjectControl businessObjectControl = new BusinessObjectPanelWin<TBusinessObject>(controlFactory, uiDefName);
            _gridWithPanelControlManager = new GridWithPanelControlManager<TBusinessObject>(this,controlFactory,businessObjectControl,uiDefName);
        }

        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlWin{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="businessObjectControl"></param>
        public GridWithPanelControlWin(IControlFactory controlFactory, IBusinessObjectControl businessObjectControl)
            : this(controlFactory, businessObjectControl, "default")
        {
        }

        ///<summary>
        /// Constructor for <see cref="GridWithPanelControlWin{TBusinessObject}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="businessObjectControl"></param>
        ///<param name="uiDefName"></param>
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

        ///<summary>
        /// Returns the currently selected business object.
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
    [Obsolete("This has been replaced by IBOSelectorAndEditor : Brett 03 Mar 2009")]
    public class BusinessObjectPanelWin<T> : UserControlWin, IBusinessObjectPanel where T : class, IBusinessObject
    {
        private IPanelInfo _panelInfo;

        ///<summary>
        /// Constructor for <see cref="BusinessObjectPanelWin{T}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
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