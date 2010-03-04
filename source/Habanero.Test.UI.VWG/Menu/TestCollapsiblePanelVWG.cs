using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Menu
{
    /// <summary>
    /// This test class tests the CollapsiblePanel for VWG.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelVWG : TestCollapsiblePanel
    {

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
        protected override ICollapsiblePanel CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }
}