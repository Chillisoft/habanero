using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.StandardControls
{
    /// <summary>
    /// This test class tests the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestSplitContainerWin : TestSplitContainer
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}