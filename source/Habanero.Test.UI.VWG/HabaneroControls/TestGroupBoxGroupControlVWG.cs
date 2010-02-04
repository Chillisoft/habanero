using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    /// <summary>
    /// This test class tests the GroupBoxGroupControl class.
    /// </summary>
    [TestFixture]
    public class TestGroupBoxGroupControlVWG : TestGroupBoxGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            Habanero.UI.VWG.ControlFactoryVWG factory = new Habanero.UI.VWG.ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}