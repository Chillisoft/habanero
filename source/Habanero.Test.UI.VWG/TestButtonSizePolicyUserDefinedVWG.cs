using Habanero.Test.UI.Base.ButtonsControl;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG
{
    [TestFixture]
    public class TestButtonSizePolicyUserDefinedVWG : TestButtonSizePolicyUserDefined
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}