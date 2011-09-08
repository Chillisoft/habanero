//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


using Habanero.UI.Wizard;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.UI.Wizard
{
    [TestFixture]
    public class TestWizardController
    {
        private WizardController controller;
        private MockRepository mock = new MockRepository();
        private IWizardStep step1;


        [SetUp]
        public void SetupTest()
        {
            controller = new WizardController();
            step1 = this.mock.CreateMock<IWizardStep>();
            controller.AddStep(step1);
        }

        [TearDown]
        public void TearDown()
    {

    }

        [Test]
        public void TestConstructor()
        {
            controller = new WizardController();
            Assert.That(controller.StepCount == 0);
        }

        [Test]
        public void TestAddStep()
        {
            Assert.AreEqual(1, controller.StepCount);
        }

        [Test]
        public void TestGetNextStep()
        {
            Assert.AreSame(this.step1, controller.GetNextStep());
        }

        [Test, ExpectedException(typeof (WizardStepException), ExpectedMessage = "Invalid Wizard Step: 1")]
        public void TestGetNextStepError()
        {
            controller.GetNextStep();
            controller.GetNextStep();
        }

        [Test]
        public void TestGetPreviousStep()
        {
            IWizardStep step2 = mock.CreateMock<IWizardStep>();
            controller.AddStep(step2);
            controller.GetNextStep();
            controller.GetNextStep();
            Assert.AreSame(step1,controller.GetPreviousStep());
        }

        [Test, ExpectedException(typeof(WizardStepException), ExpectedMessage = "Invalid Wizard Step: -1")]
        public void TestGetPreviousStepError()
        {
            controller.GetNextStep();
            controller.GetPreviousStep();
        }

        [Test]
        public void TestGetFirstStep()
        {
            Assert.AreSame(step1, controller.GetFirstStep());
        }

        [Test]
        public void TestIsLastStep()
        {
            controller.GetNextStep();
            Assert.IsTrue(controller.IsLastStep());
        }

        [Test]
        public void TestIsFirstStep()
        {
            controller.GetNextStep();
            Assert.IsTrue(controller.IsFirstStep());
        }

        [Test, ExpectedException(typeof(WizardStepException), ExpectedMessage = "Invalid call to Finish(), not at last step")]
        public void TestFinishError()
        {
            controller.Finish();
        }

        [Test]
        public void TestFinish()
        {
            controller.GetNextStep();
            controller.Finish();
        }

        [Test]
        public void TestCanMoveOn()
        {
            string message;
            controller.GetNextStep();
            Expect.Call(step1.CanMoveOn(out message)).Return(true);
            mock.ReplayAll();
            Assert.IsTrue(controller.CanMoveOn(out message));
            mock.VerifyAll();
        }

        [Test]
        public void TestGetCurrentStep()
        {
            controller.GetNextStep();
            Assert.AreSame(step1,controller.GetCurrentStep());
        }
    }
}