using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuItemCollectionVWG : TestCollapsibleMenuItemCollection
    {
        protected override IMenuItemCollection CreateControl()
        {
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            return new CollapsibleMenuItemCollectionVWG(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}