using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleSubMenuItemVWG : TestCollapsibleSubMenuItem
    {
        protected override IMenuItem CreateControl()
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), TestUtil.GetRandomString());
        }
        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), name);
        }
        protected override IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), item);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}