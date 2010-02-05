using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Wizard
{
    [TestFixture]
    public class TestNullWizardStepVWG : TestNullWizardStep
    {
        protected override IWizardStep CreateWizardStep()
        {
            return new NullWizardStepVWG();
        }
    }
}