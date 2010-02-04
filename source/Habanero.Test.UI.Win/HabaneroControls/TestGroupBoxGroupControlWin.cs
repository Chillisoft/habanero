using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestGroupBoxGroupControlWin : TestGroupBoxGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.UI.Win.ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}