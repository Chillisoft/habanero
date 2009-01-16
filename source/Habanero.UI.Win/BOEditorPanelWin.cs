using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{

    public class BOEditorPanelWin : UserControlWin, IBusinessObjectControlWithErrorDisplay, IBOEditorPanel
    {
        private readonly IPanelInfo _panelInfo;

        public BOEditorPanelWin(ClassDef classDef, string uiDefName, IControlFactory controlFactory)
        {
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            UIDef uiDef = classDef.GetUIDef(uiDefName);
            if (uiDef == null) { throw new HabaneroDeveloperException(string.Format("No UI Definition is defined for '{0}' called '{1}'.", classDef.ClassName, uiDefName), "Please check the Class definitions."); }
            UIForm uiForm = uiDef.UIForm;
            _panelInfo = panelBuilder.BuildPanelForForm(uiForm);
            IPanel panel = _panelInfo.Panel;
            BorderLayoutManager borderLayoutManager = controlFactory.CreateBorderLayoutManager(this);
            borderLayoutManager.AddControl(panel, BorderLayoutManager.Position.Centre);
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
}