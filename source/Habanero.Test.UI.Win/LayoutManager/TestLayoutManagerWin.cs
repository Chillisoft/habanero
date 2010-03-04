using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.LayoutManager
{
    [TestFixture]
    public class TestLayoutManagerWin : TestLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}