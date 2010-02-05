using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuWin : TestCollapsibleMenu
    {

        protected override IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuWin();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuWin(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}