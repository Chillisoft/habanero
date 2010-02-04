// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Drawing;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.UI.Base.Wizard
{
    [TestFixture]
    public class TestWizardController
    {
        private WizardController _wizardController;
        private readonly MockRepository _mock = new MockRepository();
        private IWizardStep _step1;

        [SetUp]
        public void SetupTest()
        {
            _wizardController = new WizardController();
            _step1 = _mock.StrictMock<IWizardStep>();
            _wizardController.AddStep(_step1);
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestConstructor()
        {
            _wizardController = new WizardController();
            Assert.That(_wizardController.StepCount == 0);
        }

        [Test]
        public void TestAddStep()
        {
            Assert.AreEqual(1, _wizardController.StepCount);
        }

        [Test]
        public void TestGetNextStep()
        {
            Assert.AreSame(this._step1, _wizardController.GetNextStep());
        }



        [Test, ExpectedException(typeof (WizardStepException), ExpectedMessage = "Invalid Wizard Step: 1")]
        public void TestGetNextStepError()
        {
            _wizardController.GetNextStep();
            _wizardController.GetNextStep();
        }

        [Test]
        public void TestGetPreviousStep()
        {
            IWizardStep step2 = _mock.StrictMock<IWizardStep>();
            _wizardController.AddStep(step2);
            _wizardController.GetNextStep();
            _wizardController.GetNextStep();
            Assert.AreSame(_step1,_wizardController.GetPreviousStep());
        }

        [Test, ExpectedException(typeof(WizardStepException), ExpectedMessage = "Invalid Wizard Step: -1")]
        public void TestGetPreviousStepError()
        {
            _wizardController.GetNextStep();
            _wizardController.GetPreviousStep();
        }

        [Test]
        public void TestGetFirstStep()
        {
            Assert.AreSame(_step1, _wizardController.GetFirstStep());
        }
        [Test]
        public void TestGetFirstStep_WhenNoFirstStepSetup()
        {
            
        }

        [Test]
        public void Test_GetFirstStep_WhenNoFirstStepSetup()
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                wizardController.GetFirstStep();
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains("There was an Error when trying to access the first step of the wizard Controller", ex.Message);
                StringAssert.Contains("The wizard controller has not been set up with steps", ex.Message);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestIsLastStep()
        {
            _wizardController.GetNextStep();
            Assert.IsTrue(_wizardController.IsLastStep());
        }

        [Test]
        public void TestIsFirstStep()
        {
            _wizardController.GetNextStep();
            Assert.IsTrue(_wizardController.IsFirstStep());
        }

        [Test, ExpectedException(typeof(WizardStepException), ExpectedMessage = "Invalid call to Finish(), not at last step")]
        public void TestFinishError()
        {
            _wizardController.Finish();
        }

        [Test]
        public void TestFinish_DoesNotRiaseError()
        {
            _wizardController.GetNextStep();
            _wizardController.Finish();
        }

        [Test]
        public void TestFinish_FiresEvent()
        {
            //-----------------------Setup TestPack----------------------
            WizardController wizardController = new WizardController();
            bool wizardFinishedFires = false;
            wizardController.WizardFinished += delegate { wizardFinishedFires = true; };
            IWizardStep step1 = _mock.StrictMock<IWizardStep>();
            wizardController.AddStep(step1);

            wizardController.GetNextStep();
            //------------------------Execute----------------------------
            wizardController.Finish();
            //------------------------Verify Result ---------------------
            Assert.IsTrue(wizardFinishedFires);
        }
        [Test]
        public void Test_Finish_ShouldCallCurrentStepMoveOn()
        {
            //-----------------------Setup TestPack----------------------
            WizardController wizardController = new WizardController();

            IWizardStep step1 = MockRepository.GenerateMock<IWizardStep>();
            wizardController.AddStep(step1);

            wizardController.GetFirstStep();
            //------------------------Assert Precondition----------------
            Assert.AreSame(step1, wizardController.GetCurrentStep());
            step1.AssertWasNotCalled(step => step.MoveOn());
            //------------------------Execute----------------------------
            wizardController.Finish();
            //------------------------Verify Result ---------------------
            step1.AssertWasCalled(step => step.MoveOn());
        }

        [Test]
        public void TestCanMoveOn()
        {
            string message;
            _wizardController.GetNextStep();
            Expect.Call(_step1.CanMoveOn(out message)).Return(true);
            _mock.ReplayAll();
            Assert.IsTrue(_wizardController.CanMoveOn(out message));
            _mock.VerifyAll();
        }

        [Test]
        public void Test_CompleteCurrentStep_ShouldCallStepMoveOn()
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            var step1 = MockRepository.GenerateMock<IWizardStep>();
            wizardController.AddStep(step1);
            wizardController.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, wizardController.StepCount);
            Assert.AreSame(step1, wizardController.GetCurrentStep());
            step1.AssertWasNotCalled(step => step.MoveOn());
            // ---------------Execute Test ----------------------
            wizardController.CompleteCurrentStep();
            //---------------Test Result -----------------------
            step1.AssertWasCalled(wizardStep => wizardStep.MoveOn());
        }
        [Test]
        public void Test_UndoCurrentStep_ShouldCallStepMoveBack()
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            var step1 = MockRepository.GenerateMock<IWizardStep>();
            wizardController.AddStep(step1);
            wizardController.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, wizardController.StepCount);
            step1.AssertWasNotCalled(step => step.UndoMoveOn());
            Assert.AreSame(step1, wizardController.GetCurrentStep());
            //---------------Execute Test ----------------------
            wizardController.UndoCompleteCurrentStep();
            //---------------Test Result -----------------------
            step1.AssertWasCalled(wizardStep => wizardStep.UndoMoveOn());
        }

        [Test]
        public void Test_CanMoveBack_ShouldCallStepCanMoveBack()
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            var step1 = MockRepository.GenerateMock<IWizardStep>();
            wizardController.AddStep(step1);
            wizardController.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, wizardController.StepCount);
            step1.AssertWasNotCalled(step => step.CanMoveBack());
            Assert.AreSame(step1, wizardController.GetCurrentStep());
            //---------------Execute Test ----------------------
            var canMoveBack = wizardController.CanMoveBack();
            //---------------Test Result -----------------------
            step1.AssertWasCalled(wizardStep => wizardStep.CanMoveBack());
            Assert.AreEqual(step1.CanMoveBack(), canMoveBack);
            Assert.IsFalse(canMoveBack);
        }

        [Test]
        public void Test_CanMoveBack_WhenStepTrue_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            var step1 = MockRepository.GenerateMock<IWizardStep>();
            step1.Stub(wizardStep1 => wizardStep1.CanMoveBack()).Return(true);
            wizardController.AddStep(step1);
            wizardController.GetFirstStep();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, wizardController.StepCount);
            step1.AssertWasNotCalled(step => step.CanMoveBack());
            Assert.AreSame(step1, wizardController.GetCurrentStep());
            //---------------Execute Test ----------------------
            var canMoveBack = wizardController.CanMoveBack();
            //---------------Test Result -----------------------
            step1.AssertWasCalled(wizardStep => wizardStep.CanMoveBack());
            Assert.AreEqual(step1.CanMoveBack(), canMoveBack);
            Assert.IsTrue(canMoveBack);
        }

        [Test]
        public void TestGetCurrentStep()
        {
            _wizardController.GetNextStep();
            Assert.AreSame(_step1, _wizardController.GetCurrentStep());
        }

        [Test]
        public void TestGetCurrentStep_BeforeFirstStepCalled()
        {
            Assert.AreSame(null, _wizardController.GetCurrentStep());
        }

        [Test]
        public void Test_WizardControllerCancelsWizardSteps_Win()
        {
            Test_WizardControllerCancelsWizardSteps(new Habanero.UI.Win.ControlFactoryWin());
        }

        [Test]
        public void Test_WizardControllerCancelsWizardSteps_VWG()
        {
            Test_WizardControllerCancelsWizardSteps(new Habanero.UI.VWG.ControlFactoryVWG());
        }
        [Test]
        public void Test_WizardControllerCancelsWizardSteps(IControlFactory controlFactory)
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            IWizardStep wzrdStep = MockRepository.GenerateMock<IWizardStep>();
            IWizardControl wizardControl = controlFactory.CreateWizardControl(wizardController);
            wizardController.AddStep(wzrdStep);
            //--------------Assert PreConditions----------------            
            wzrdStep.AssertWasNotCalled(step => step.CancelStep());
            //---------------Execute Test ----------------------
            wizardControl.CancelButton.PerformClick();
            //---------------Test Result -----------------------
            wzrdStep.AssertWasCalled(step => step.CancelStep());

        }
    }
}