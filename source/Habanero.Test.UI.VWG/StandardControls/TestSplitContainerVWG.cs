using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    /// <summary>
    /// This test class tests the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestSplitContainerVWG : TestSplitContainer
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}