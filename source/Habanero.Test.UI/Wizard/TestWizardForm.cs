using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Habanero.UI.Wizard;

namespace Habanero.Test.UI.Wizard
{
    [TestFixture]
    public class TestWizardForm
    {
        [Test]
        public void TestFinish()
        {
            TestWizardControl.MyWizardController _wizardController = new TestWizardControl.MyWizardController();
            WizardForm wizardForm = new WizardForm(_wizardController);
            wizardForm.Show();
            Assert.IsTrue(_wizardController.IsFirstStep());
            wizardForm.WizardControl.Next();
            Assert.IsTrue(_wizardController.IsLastStep());
            wizardForm.WizardControl.Finish();
            Assert.IsFalse(wizardForm.Visible);
        }
    }
}
