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
    public class TestReadOnlyGridControlSelectorWin : TestReadOnlyGridControlSelector
    {

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IReadOnlyGridControl readOnlyGridControl = GetControlFactory().CreateReadOnlyGridControl();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)readOnlyGridControl);
            return readOnlyGridControl;
        }
    }
}