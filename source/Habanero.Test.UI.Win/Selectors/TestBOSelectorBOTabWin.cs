using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Selectors
{
    /// <summary>
    /// This test class tests the BOTabControlSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorBOTabWin : TestBOSelectorBOTab
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IBOColTabControl control = GetControlFactory().CreateBOColTabControl();
            control.BusinessObjectControl = this.GetBusinessObjectControlStub();
            return control;
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new TestBOColTabControl.BusinessObjectControlWinStub();
        }
    }
}