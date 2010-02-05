using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base.Wizard;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Wizard
{
    [TestFixture]
    public class TestMessageWizardStepWin : TestMessageWizardStep
    {
        protected override IMessageWizardStep CreateWizardStep()
        {
            return new MessageWizardStepWin();
        }
    }
}