using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Wizard
{
    [TestFixture]
    public class TestWizardControllerVWG : TestWizardController
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}