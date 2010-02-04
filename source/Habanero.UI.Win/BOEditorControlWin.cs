// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    ///<summary>
    /// A Control for Editing/Viewing an <see cref="IBusinessObject"/>.
    ///</summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    public class BOEditorControlWin : UserControlWin, IBOPanelEditorControl

    {
        private readonly IPanelInfo _panelInfo;

        ///<summary>
        /// The Constructor for the <see cref="BOEditorControlWin"/> which passes in the
        /// <paramref name="classDef"/> for the <see cref="IBusinessObject"/> and the <paramref name="uiDefName"/> that 
        ///  is used to defined the User Interface for the <see cref="IBusinessObject"/>      
        ///</summary>
        ///<param name="controlFactory">The control factory which is used to create the Controls on this form.</param>
        ///<param name="classDef">The <see cref="IClassDef"/> for the  <see cref="IBusinessObject"/> that will be edited by this control</param>
        ///<param name="uiDefName">The user interface defined in the <see cref="IClassDef"/> that will be used to Build this control</param>
        public BOEditorControlWin(IControlFactory controlFactory, IClassDef classDef, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (classDef == null) throw new ArgumentNullException("classDef");
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");

            _panelInfo = BOEditorControlUtils.CreatePanelInfo(controlFactory, classDef, uiDefName, this);
            SetEnableState();
        }

        ///<summary>
        /// The Constructor for the <see cref="BOEditorControlWin"/> which passes in the
        /// <paramref name="classDef"/> for the <see cref="IBusinessObject"/> and
        ///  this control will be built using the default <see cref="UIDef"/> and the <see cref="IControlFactory"/> 
        ///  from the <see cref="GlobalUIRegistry.ControlFactory"/>
        ///  is used to defined the User Interface for the <see cref="IBusinessObject"/>      
        ///</summary>
        ///<param name="classDef">The <see cref="IClassDef"/> for the  <see cref="IBusinessObject"/> that will be edited by this control</param>
        public BOEditorControlWin(IClassDef classDef) : this(GlobalUIRegistry.ControlFactory, classDef, "default")
        {
        }

        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        public virtual IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set
            {
                _panelInfo.BusinessObject = value;
                //_panelInfo.UpdateErrorProvidersErrorMessages();
                SetEnableState();
            }
        }
        /// <summary>
        /// Sets the forms enabled state.
        /// </summary>
        protected virtual void SetEnableState()
        {
            this.Enabled = BusinessObject != null;
            this.PanelInfo.Panel.Enabled = this.Enabled;
            this.PanelInfo.ControlsEnabled = this.Enabled;
        }

        ///<summary>
        /// The PanelInfo for the <see cref="BOEditorControlWin"/>.
        ///</summary>
        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
        }

//        /// <summary>
//        /// Displays any errors or invalid fields associated with the BusinessObjectInfo
//        /// being edited.  A typical use would involve activating the ErrorProviders
//        /// on a BO panel.
//        /// </summary>
//        public void DisplayErrors()
//        {
//            _panelInfo.ApplyChangesToBusinessObject();
//        }
//
//        /// <summary>
//        /// Hides all the error providers.  Typically used where a new object has just
//        /// been added and the interface is being cleaned up.
//        /// </summary>
//        public void ClearErrors()
//        {
//            _panelInfo.ClearErrorProviders();
//        }

        #region Implementation of IBOEditorControl

        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            _panelInfo.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        public bool HasErrors
        {
            get { return this.BusinessObject == null? false: !this.BusinessObject.IsValid(); }
        }

        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        public bool HasWarning
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        public ErrorList Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        public bool IsDirty
        {
            get
            {
                ApplyChangesToBusinessObject();
                return this.BusinessObject == null ? false : this.BusinessObject.Status.IsDirty;
            }
        }
// 
        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        public ErrorList Warnings
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }

    ///<summary>
    /// A Control for Editing/Viewing an <see cref="IBusinessObject"/> of type T.
    ///</summary>
    public class BOEditorControlWin<T> : UserControlWin, IBOPanelEditorControl where T : class, IBusinessObject
    {
        private readonly IPanelInfo _panelInfo;

        ///<summary>
        /// The Constructor for <see cref="BOEditorControlWin{T}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName">the user interface that identifies the <see cref="UIDef"/> that will be used
        /// for building the <see cref="IBusinessObject"/>'s Controls. </param>
        public BOEditorControlWin(IControlFactory controlFactory, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (string.IsNullOrEmpty(uiDefName)) throw new ArgumentNullException("uiDefName");
            IClassDef classDef = ClassDef.Get<T>();
            _panelInfo = BOEditorControlUtils.CreatePanelInfo(controlFactory, classDef, uiDefName, this);
            SetEnableState();
        }
        ///<summary>
        /// The Constructor for the <see cref="BOEditorControlWin"/> 
        ///  this control will be built using the default <see cref="UIDef"/> and the <see cref="IControlFactory"/> 
        ///  from the <see cref="GlobalUIRegistry.ControlFactory"/>
        ///  is used to defined the User Interface for the <see cref="IBusinessObject"/>      
        ///</summary>
        public BOEditorControlWin()
            : this(GlobalUIRegistry.ControlFactory, "default")
        {
        }
        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        public virtual IBusinessObject BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set
            {
                _panelInfo.BusinessObject = value;
                _panelInfo.UpdateErrorProvidersErrorMessages();
                SetEnableState();
            }
        }
        /// <summary>
        /// Sets the forms enabled state.
        /// </summary>
        protected virtual void SetEnableState()
        {
            this.Enabled = BusinessObject != null;
            this.PanelInfo.Panel.Enabled = this.Enabled;
            this.PanelInfo.ControlsEnabled = this.Enabled;
        }
        ///<summary>
        /// The PanelInfo for the <see cref="BOEditorControlWin"/>.
        ///</summary>
        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
        }

        /// <summary>
        /// Displays any errors or invalid fields associated with the BusinessObjectInfo
        /// being edited.  A typical use would involve activating the ErrorProviders
        /// on a BO panel.
        /// </summary>
        public void DisplayErrors()
        {
            ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Hides all the error providers.  Typically used where a new object has just
        /// been added and the interface is being cleaned up.
        /// </summary>
        public void ClearErrors()
        {
            _panelInfo.ClearErrorProviders();
        }

        #region Implementation of IBOEditorControl

        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {
            _panelInfo.ApplyChangesToBusinessObject();
        }

        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        public bool HasErrors
        {
            get { return this.BusinessObject == null ? false : !this.BusinessObject.IsValid(); }
        }

        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        public bool HasWarning
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        public ErrorList Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        public bool IsDirty
        {
            get
            {
                ApplyChangesToBusinessObject();
                return this.BusinessObject == null ? false : this.BusinessObject.Status.IsDirty;
            }
        }

        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        public ErrorList Warnings
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }

    // ReSharper restore ClassWithVirtualMembersNeverInherited.Global
    /// <summary>
    /// A Utility Class used by <see cref="BOEditorControlWin"/> and <see cref="BOEditorControlWin{T}"/> providing common functionality.
    /// </summary>
    internal static class BOEditorControlUtils
    {
        private static IUIForm GetUiForm(IClassDef classDef, string uiDefName)
        {
            IUIForm uiForm;
            try
            {
                uiForm = ((ClassDef) classDef).UIDefCol[uiDefName].UIForm;
            }
            catch (HabaneroDeveloperException ex)
            {
                string developerMessage = "The 'IBOEditorControl' could not be created since the the uiDef '"
                                          + uiDefName + "' does not exist in the classDef for '"
                                          + classDef.ClassNameFull + "'";
                throw new HabaneroDeveloperException(developerMessage, developerMessage, ex);
            }
            if (uiForm == null)
            {
                string developerMessage = "The 'IBOEditorControl' could not be created since the the uiDef '"
                                          + uiDefName + "' in the classDef '" + classDef.ClassNameFull
                                          + "' does not have a UIForm defined";
                throw new HabaneroDeveloperException(developerMessage, developerMessage);
            }
            return uiForm;
        }

        internal static IPanelInfo CreatePanelInfo
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName, IBOEditorControl iboEditorControl)
        {
            IUIForm uiForm = GetUiForm(classDef, uiDefName);
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(iboEditorControl);
            layoutManager.AddControl(panelInfo.Panel, BorderLayoutManager.Position.Centre);
            return panelInfo;
        }
    }
}
