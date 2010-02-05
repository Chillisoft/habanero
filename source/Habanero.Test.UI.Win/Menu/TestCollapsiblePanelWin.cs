using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Menu
{
    /// <summary>
    /// This test class tests the CollapsiblePanel for Win.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelWin : TestCollapsiblePanel
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override ICollapsiblePanel CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }
}