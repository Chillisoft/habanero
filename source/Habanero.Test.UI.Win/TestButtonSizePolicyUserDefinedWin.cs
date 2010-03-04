using Habanero.Test.UI.Base.ButtonsControl;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win
{
    [TestFixture]
    public class TestButtonSizePolicyUserDefinedWin : TestButtonSizePolicyUserDefined
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}