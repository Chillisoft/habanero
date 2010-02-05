using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    public class TestTreeViewControllerVWG : TestTreeViewController
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory controlFactory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = controlFactory;
            return controlFactory;
        }
    }
}