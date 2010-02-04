using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.LayoutManager
{
    [TestFixture]
    public class TestColumnLayoutManagerVWG : TestColumnLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
    }
}