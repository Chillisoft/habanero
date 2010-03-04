using Habanero.Test.UI.Base.Wizard;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Wizard
{
    [TestFixture]
    public class TestNullWizardStepWin : TestNullWizardStep
    {
        protected override IWizardStep CreateWizardStep()
        {
            return new NullWizardStepWin();
        }
    }
}