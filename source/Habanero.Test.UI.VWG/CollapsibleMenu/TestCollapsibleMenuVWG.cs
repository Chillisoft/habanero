using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuVWG : TestCollapsibleMenu
    {
        protected override IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuVWG();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuVWG(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}