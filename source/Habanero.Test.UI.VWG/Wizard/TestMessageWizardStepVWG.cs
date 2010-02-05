using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base.Wizard;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Wizard
{
    [TestFixture]
    public class TestMessageWizardStepVWG : TestMessageWizardStep
    {
        protected override IMessageWizardStep CreateWizardStep()
        {
            return new MessageWizardStepVWG();
        }
    }
}