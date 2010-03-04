using Habanero.Test.UI.Base;
using Habanero.Test.UI.VWG.Grid;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Selectors
{
    /// <summary>
    /// This test class tests the GridSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorGridVWG : TestBOSelectorGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
        protected override IBOColSelectorControl CreateSelector()
        {
            TestGridBaseVWG.GridBaseVWGStub gridBase = new TestGridBaseVWG.GridBaseVWGStub();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(gridBase);
            SetupGridColumnsForMyBo(gridBase);
            return gridBase;
        }
    }
}