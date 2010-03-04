using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Menu
{
    [TestFixture]
    public class TestCollapsiblePanelGroupControlWin : TestCollapsiblePanelGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }
    }
}