using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemVWG : TestCollapsibleMenuItem
    {
        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleMenuItemVWG(name);
        }

        protected override IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(habaneroMenuItem);
        }

        protected override IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(controlFactory, habaneroMenuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}