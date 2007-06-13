using System.Collections;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    /// <summary>
    /// Summary description for TestWizard.
    /// </summary>
    [TestFixture]
    public class TestWizard
    {
        private Wizard myWizard;
        private string errMsg;

        public TestWizard()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            myWizard = new MockWizard();
        }

        [Test]
        public void TestStartAtStep1()
        {
            Assert.AreEqual(1, myWizard.CurrentStepNumber, "Starting step should be step 1");
        }

        [Test]
        public void TestNextToNextStepNumber()
        {
            Assert.IsTrue(myWizard.NextStep(ref errMsg), "NextStep should return true");
            Assert.AreEqual(2, myWizard.CurrentStepNumber, "After calling next step current num should be 2");
            myWizard.NextStep(ref errMsg);
            Assert.AreEqual(3, myWizard.CurrentStepNumber, "After calling next step again current num should be 3");
        }

        [Test]
        public void TestPreviousGoesToPrevious()
        {
            myWizard.NextStep(ref errMsg);
            myWizard.previousStep();
            Assert.AreEqual(1, myWizard.CurrentStepNumber, "IT should be back at 1");
        }

        [Test]
        public void TestPreviousDoesntWorkAtStepOne()
        {
            myWizard.previousStep();
            Assert.AreEqual(1, myWizard.CurrentStepNumber, "It shouldn't work if current step is at 1");
        }

        [Test]
        public void TestMaxNumberOfSteps()
        {
            myWizard.MaxNumberOfSteps = 3;
            Assert.AreEqual(3, myWizard.MaxNumberOfSteps, "MaxNumberOfSteps doesn't work.");
        }

        [Test]
        public void TestNextButtonDoesntGoPastMaxNumSteps()
        {
            myWizard.MaxNumberOfSteps = 3;
            myWizard.NextStep(ref errMsg);
            myWizard.NextStep(ref errMsg);
            myWizard.NextStep(ref errMsg);
            Assert.AreEqual(3, myWizard.CurrentStepNumber, "Shouldnt go past max number of steps");
        }

        [Test, ExpectedException(typeof (Wizard.FinishNotAvailableException), "Finish is not available.")]
        public void testFinish()
        {
            myWizard.Finish(ref errMsg);
        }

        [Test]
        public void TestFinishWhenFinishIsAvailable()
        {
            myWizard.MaxNumberOfSteps = 3;
            myWizard.NextStep(ref errMsg);
            myWizard.NextStep(ref errMsg);
            Assert.IsTrue(myWizard.Finish(ref errMsg), "Finish should return true.");
        }

        [Test]
        public void TestAvailableButtonsAtStart()
        {
            Assert.AreEqual(1, myWizard.AvailableOperations().Count,
                            "There should only be one operation (next) available when the wizard starts.");
            Assert.AreEqual("Next", myWizard.AvailableOperations()[0], "Next should be the only op available at start.");
        }

        [Test]
        public void TestAvailableButtonsAtStep2Outof3()
        {
            myWizard.MaxNumberOfSteps = 3;
            myWizard.NextStep(ref errMsg);
            Assert.AreEqual(2, myWizard.AvailableOperations().Count,
                            "There should be two ops available at step 2 out of 3");
            Assert.AreEqual("Next", myWizard.AvailableOperations()[0], "Next should be the only op available at start.");
            Assert.AreEqual("Previous", myWizard.AvailableOperations()[1],
                            "Next and Previous should be the only ops available at start.");
        }

        [Test]
        public void TestAvailableButtonsAtStep3Outof3()
        {
            myWizard.MaxNumberOfSteps = 3;
            myWizard.NextStep(ref errMsg);
            myWizard.NextStep(ref errMsg);
            Assert.AreEqual(2, myWizard.AvailableOperations().Count,
                            "There should be two ops available at step 3 out of 3");
            Assert.AreEqual("Previous", myWizard.AvailableOperations()[0],
                            "Next and Previous should be the only ops available at start.");
            Assert.AreEqual("Finish", myWizard.AvailableOperations()[1],
                            "Finish and Previous should be the only ops available at start.");
        }

        [Test]
        public void TestSetStep()
        {
            myWizard.setWizardStep(1, new MockWizardStep(true, ""));
            myWizard.NextStep(ref errMsg);
            Assert.AreEqual(2, myWizard.CurrentStepNumber, "Should go to step 2 and pass validation.");
        }

        [Test]
        public void TestGetStep()
        {
            Assert.IsNull(myWizard.getWizardStep(1), "Step 1 should be null because it hasn't been set.");
            MockWizardStep mockStep = new MockWizardStep(true, "");
            myWizard.setWizardStep(1, mockStep);
            Assert.AreSame(mockStep, myWizard.getWizardStep(1), "Step 1 should be mockstep because it was set.");
        }

        [Test]
        public void TestStepFailing()
        {
            myWizard.setWizardStep(2, new MockWizardStep(false, "MockFail"));
            myWizard.NextStep(ref errMsg);
            Assert.IsFalse(myWizard.NextStep(ref errMsg), "Should not pass step 2.");
            Assert.AreEqual(2, myWizard.CurrentStepNumber, "Should not pass step 2.");
            Assert.AreEqual("MockFail", errMsg, "Error message should be 'MockFail'");
        }

        [Test]
        public void TestPreviousCancelsWizardStep()
        {
            MockWizardStep mock = new MockWizardStep(true, "");
            myWizard.setWizardStep(2, mock);
            myWizard.NextStep(ref errMsg);
            myWizard.previousStep();
            Assert.AreEqual("Cancelled", mock.CancelStatus, "PreviousStep didn't cancel wizard step");
        }

        [Test]
        public void TestFinishFailing()
        {
            myWizard.MaxNumberOfSteps = 2;
            myWizard.setWizardStep(2, new MockWizardStep(false, "MockFail"));
            myWizard.NextStep(ref errMsg);
            Assert.IsFalse(myWizard.Finish(ref errMsg), "Shouldn't be able to finish.");
            Assert.AreEqual(2, myWizard.CurrentStepNumber, "Should not pass step 2.");
            Assert.AreEqual("MockFail", errMsg, "Error message should be 'MockFail'");
        }

        [Test]
        public void TestGetCurrentStep()
        {
            MockWizardStep mock = new MockWizardStep(true, "");
            myWizard.setWizardStep(2, mock);
            myWizard.NextStep(ref errMsg);
            Assert.AreSame(mock, myWizard.GetCurrentWizardStep(),
                           "GetCurrentStep is not returning the correct wizardstep");
        }

        [Test]
        public void TestGetPreviousStep()
        {
            MockWizardStep mock1 = new MockWizardStep(true, "");
            MockWizardStep mock2 = new MockWizardStep(true, "");
            myWizard.setWizardStep(1, mock1);
            myWizard.setWizardStep(2, mock2);
            myWizard.NextStep(ref errMsg);
            Assert.AreSame(mock1, myWizard.GetPreviousWizardStep(),
                           "GetPreviousWizardStep is not returning the correct wizardstep");
            myWizard.previousStep();
            Assert.IsNull(myWizard.GetPreviousWizardStep(),
                          "GetPreviousWizardStep should return nothing when step 1 is current.");
        }

        [Test]
        public void TestStepHeading()
        {
            myWizard.setWizardStep(1, new MockWizardStep(true, ""));
            myWizard.MaxNumberOfSteps = 3;
            Assert.AreEqual("        Step 1 of 3 - Test Heading", myWizard.GetHeading(), "Heading is incorrect");
        }


        private class MockWizard : Wizard
        {
            protected override void AfterNextStep()
            {
            }

            protected override bool Persist(ref string errMsg)
            {
                return true;
            }
        }

        private class MockWizardStep : IWizardStep
        {
            private bool mPass;
            private string mMsg;
            public string CancelStatus;

            public MockWizardStep(bool pass, string msg)
            {
                mPass = pass;
                mMsg = msg;
            }

            public event WizardStepEnabledUpdatedHandler WizardStepEnabledUpdated;

            public bool Validate(ref string errMsg)
            {
                errMsg = mMsg;
                return mPass;
            }

            public void CancelChanges()
            {
                CancelStatus = "Cancelled";
            }

            public Panel GetPanel()
            {
                return null;
            }

            public string GetHeading()
            {
                return "Test Heading";
            }

            public IList GetObjectsToPersist()
            {
                return new ArrayList();
            }

            public void Activate()
            {
            }

            public bool Enabled()
            {
                return true;
            }

            private void FireWizardStepEnabledUpdated(bool enabled)
            {
                if (this.WizardStepEnabledUpdated != null)
                {
                    WizardStepEnabledUpdated(this, new WizardStepEventArgs(enabled));
                }
            }
        }
    }
}