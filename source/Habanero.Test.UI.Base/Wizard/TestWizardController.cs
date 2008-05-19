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
            _step1 = _mock.CreateMock<IWizardStep>();
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
            IWizardStep step2 = _mock.CreateMock<IWizardStep>();
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
        public void TestFinish()
        {
            _wizardController.GetNextStep();
            _wizardController.Finish();
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
    }
}