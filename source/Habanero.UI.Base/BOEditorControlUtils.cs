using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A Utility Class used by <see cref="IBOEditorControl"/> providing common functionality for windows and VWG.
    /// </summary>
    public static class BOEditorControlUtils
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
        /// <summary>
        /// Creates an IPanelInfo for the business Object with the classDef and uiDef.
        /// The Panel Info is added to the IBOEditorControl.
        /// </summary>
        /// <param name="controlFactory"></param>
        /// <param name="classDef"></param>
        /// <param name="uiDefName"></param>
        /// <param name="iboEditorControl"></param>
        /// <returns></returns>
        public static IPanelInfo CreatePanelInfo
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
