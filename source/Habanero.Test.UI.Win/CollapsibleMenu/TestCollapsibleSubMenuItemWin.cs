using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleSubMenuItemWin : TestCollapsibleSubMenuItem
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }
        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(), name);
        }
        protected override IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(), item);
        }
        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}