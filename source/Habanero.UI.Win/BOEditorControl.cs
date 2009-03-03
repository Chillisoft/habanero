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
    public class BOEditorControl : UserControlWin, IBOEditorControl
    {
        private readonly IPanelInfo _panelInfo;

        ///<summary>
        /// The Constructor for the <see cref="BOEditorControl"/> which passes in the
        /// <paramref name="classDef"/> for the <see cref="IBusinessObject"/> and the <see cref="uiDefName"/> that 
        ///  is used to defined the User Interface for the <see cref="IBusinessObject"/>      
        ///</summary>
        ///<param name="controlFactory">The control factory which is used to create the Controls on this form.</param>
        ///<param name="classDef">The <see cref="IClassDef"/> for the  <see cref="IBusinessObject"/> that will be edited by this control</param>
        ///<param name="uiDefName">The user interface defined in the <see cref="IClassDef"/> that will be used to Build this control</param>
        public BOEditorControl(IControlFactory controlFactory,IClassDef classDef, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (classDef == null) throw new ArgumentNullException("classDef");
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");

            _panelInfo = BusinessObjectControlUtils.CreatePanelInfo(controlFactory, classDef, uiDefName, this);
        }

        ///<summary>
        /// The Constructor for the <see cref="BOEditorControl"/> which passes in the
        /// <paramref name="classDef"/> for the <see cref="IBusinessObject"/> and
        ///  this control will be built using the default <see cref="UIDef"/> and the <see cref="IControlFactory"/> 
        ///  from the <see cref="GlobalUIRegistry.ControlFactory"/>
        ///  is used to defined the User Interface for the <see cref="IBusinessObject"/>      
        ///</summary>
        ///<param name="classDef">The <see cref="IClassDef"/> for the  <see cref="IBusinessObject"/> that will be edited by this control</param>
        public BOEditorControl(IClassDef classDef): this(GlobalUIRegistry.ControlFactory,classDef, "default")
        {
        }

        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
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

    ///<summary>
    /// A Control for Editing/Viewing an <see cref="IBusinessObject"/> of type <typeparam name="T"/>.
    ///</summary>
    public class BusinessObjectControlWin<T> : UserControlWin, IBOEditorControl
        where T : class, IBusinessObject
    {
        private readonly IPanelInfo _panelInfo;

        ///<summary>
        /// The Constructor for <see cref="BusinessObjectControlWin{T}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName">the user interface that identifies the <see cref="UIDef"/> that will be used
        /// for building the <see cref="IBusinessObject"/>'s Controls. </param>
        public BusinessObjectControlWin(IControlFactory controlFactory, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");
            ClassDef classDef = ClassDef.Get<T>();
            _panelInfo = BusinessObjectControlUtils.CreatePanelInfo(controlFactory, classDef, uiDefName, this);
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
    /// <summary>
    /// A Utility Class used by <see cref="BOEditorControl"/> and <see cref="BusinessObjectControlWin{T}"/> providing common functionality.
    /// </summary>
    internal static class BusinessObjectControlUtils
    {
        private static UIForm GetUiForm(IClassDef classDef, string uiDefName)
        {
            UIForm uiForm;
            try
            {
                uiForm = ((ClassDef)classDef).UIDefCol[uiDefName].UIForm;
            }
            catch (HabaneroDeveloperException ex)
            {
                string developerMessage = "The 'BusinessObjectControlWin' could not be created since the the uiDef '" + uiDefName +
                                          "' does not exist in the classDef for '" + classDef.ClassNameFull + "'";
                throw new HabaneroDeveloperException(developerMessage, developerMessage, ex);
            }
            if (uiForm == null)
            {
                string developerMessage = "The 'BusinessObjectControlWin' could not be created since the the uiDef '" + uiDefName +
                                          "' in the classDef '" + classDef.ClassNameFull + "' does not have a UIForm defined";
                throw new HabaneroDeveloperException(developerMessage, developerMessage);
            }
            return uiForm;
        }
        internal static IPanelInfo CreatePanelInfo(IControlFactory controlFactory, IClassDef classDef, string uiDefName, IBOEditorControl iboEditorControl)
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