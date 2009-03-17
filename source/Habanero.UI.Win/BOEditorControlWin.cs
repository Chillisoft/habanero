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
                this.PanelInfo.ApplyChangesToBusinessObject();
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
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");
            ClassDef classDef = ClassDef.Get<T>();
            _panelInfo = BOEditorControlUtils.CreatePanelInfo(controlFactory, classDef, uiDefName, this);
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
            _panelInfo.ApplyChangesToBusinessObject();
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
                this.PanelInfo.ApplyChangesToBusinessObject();
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

    /// <summary>
    /// A Utility Class used by <see cref="BOEditorControlWin"/> and <see cref="BOEditorControlWin{T}"/> providing common functionality.
    /// </summary>
    internal static class BOEditorControlUtils
    {
        private static UIForm GetUiForm(IClassDef classDef, string uiDefName)
        {
            UIForm uiForm;
            try
            {
                uiForm = ((ClassDef) classDef).UIDefCol[uiDefName].UIForm;
            }
            catch (HabaneroDeveloperException ex)
            {
                string developerMessage = "The 'BOEditorControlWin' could not be created since the the uiDef '"
                                          + uiDefName + "' does not exist in the classDef for '"
                                          + classDef.ClassNameFull + "'";
                throw new HabaneroDeveloperException(developerMessage, developerMessage, ex);
            }
            if (uiForm == null)
            {
                string developerMessage = "The 'BOEditorControlWin' could not be created since the the uiDef '"
                                          + uiDefName + "' in the classDef '" + classDef.ClassNameFull
                                          + "' does not have a UIForm defined";
                throw new HabaneroDeveloperException(developerMessage, developerMessage);
            }
            return uiForm;
        }

        internal static IPanelInfo CreatePanelInfo
            (IControlFactory controlFactory, IClassDef classDef, string uiDefName, IBOEditorControl iboEditorControl)
        {
            UIForm uiForm = GetUiForm(classDef, uiDefName);
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            IPanelInfo panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(iboEditorControl);
            layoutManager.AddControl(panelInfo.Panel, BorderLayoutManager.Position.Centre);
            return panelInfo;
        }
    }
}