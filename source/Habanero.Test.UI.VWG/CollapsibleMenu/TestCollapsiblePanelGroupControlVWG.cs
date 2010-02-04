using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.CollapsibleMenu
{
    /// <summary>
    /// This test class tests the CollapsiblePanelGroupControl for VWG.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelGroupControlVWG : TestCollapsiblePanelGroupControl
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
    }
}