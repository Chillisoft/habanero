using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuItemVWG : TestCollapsibleMenuItem
    {
        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}