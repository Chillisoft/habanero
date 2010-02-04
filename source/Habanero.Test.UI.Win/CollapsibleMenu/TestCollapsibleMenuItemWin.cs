using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuItemWin : TestCollapsibleMenuItem
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}