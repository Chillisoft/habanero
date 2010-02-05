using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemWin : TestCollapsibleMenuItem
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleMenuItemWin(name);
        }

        protected override IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemWin(habaneroMenuItem);
        }

        protected override IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemWin(controlFactory, habaneroMenuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}