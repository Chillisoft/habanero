using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Wizard
{
    [TestFixture]
    public class TestWizardControllerWin : TestWizardController
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}