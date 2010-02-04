using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Selectors
{
    /// <summary>
    /// This test class tests the GridSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorGridWin : TestBOSelectorGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            TestGridBase.GridBaseWinStub gridBase = new TestGridBase.GridBaseWinStub();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(gridBase);
            SetupGridColumnsForMyBo(gridBase);
            return gridBase;
        }
    }
}