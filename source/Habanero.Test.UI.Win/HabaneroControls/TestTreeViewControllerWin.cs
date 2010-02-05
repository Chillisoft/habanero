using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestTreeViewControllerWin : TestTreeViewController
    {

        protected override IControlFactory GetControlFactory()
        {
            IControlFactory controlFactory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = controlFactory;
            return controlFactory;
        }
    }
}