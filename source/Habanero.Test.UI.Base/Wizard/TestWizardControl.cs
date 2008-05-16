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


using System.Collections.Generic;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Wizard
{
    [TestFixture]
    public abstract class TestWizardControl
    {
        protected abstract IControlFactory GetControlFactory();

        //[TestFixture]
        //public class TestWizardControlWin : TestWizardControl
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new Habanero.UI.Win.ControlFactoryWin();
        //    }
        //}

        [TestFixture]
        public class TestWizardControlGiz : TestWizardControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }
        }

        private MyWizardController _controller;
        private IWizardControl _wizardControl;

        private string _message;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _controller = new MyWizardController();
            _wizardControl = GetControlFactory().CreateWizardControl(_controller);// new WizardControl(_controller);

            
        }
        [SetUp]
        public void SetupTest()
        {
            _message = "";
            _wizardControl.Start();
            
        }
        //TODO: Tests for layout management?
        [TearDown]
        public void TearDownTest()
        {
        }
        //[Test]
        //public void TestInitialise()
        //{
        //    //Setup
        //    MyWizardController wizardController = new MyWizardController();
        //    IWizardControl wizardControl = GetControlFactory().CreateWizardControl(_controller);
        //    //Execute
        //    wizardControl.i();
        //    //Assert
        //    Assert.AreSame(wizardController.ControlForStep1, wizardControl.CurrentControl);
        //}

        [Test]
        public void TestStart()
        {
            //Setup -----------------------------------------------------
            MyWizardController  wizardController = new MyWizardController();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(_controller);
            //Execute ---------------------------------------------------
            wizardControl.Start();
            //Assert Results --------------------------------------------
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            Assert.AreEqual(wizardController.ControlForStep1.Name, wizardControl.CurrentControl.Name);
            Assert.AreEqual(0, wizardControl.PreviousButton.TabIndex);
            Assert.AreEqual(1, wizardControl.NextButton.TabIndex);
        }
        [Test]
        public void Test_SetWizardController_CallsStart()
        {
            //Setup ----------------------------------------------------
            MyWizardController wizardController = new MyWizardController();
            IWizardControl wizardControl = GetControlFactory().CreateWizardControl(_controller);
            //Execute ---------------------------------------------------
            wizardControl.WizardController = wizardController;
            //Assert Results --------------------------------------------
            Assert.AreEqual("ControlForStep1", wizardControl.CurrentControl.Name);
            Assert.AreEqual(wizardController.ControlForStep1.Name, wizardControl.CurrentControl.Name);
            Assert.AreEqual(0, wizardControl.PreviousButton.TabIndex);
            Assert.AreEqual(1, wizardControl.NextButton.TabIndex);
        }
        [Test]
        public void TestNext()
        {
            _wizardControl.Next();
            Assert.AreSame(_controller.Control2, _wizardControl.CurrentControl);
        }

        [Test]
        public void TestPrevious()
        {
            _wizardControl.Next();
            _wizardControl.Previous();
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
        }

        [Test, ExpectedException(typeof(WizardStepException))]
        public void TestNextWithNoNextStep()
        {
            _wizardControl.Next();
            _wizardControl.Next();
        }

        [Test, ExpectedException(typeof(WizardStepException))]
        public void TestPreviousWithNoNextStep()
        {
            _wizardControl.Previous();
        }

        [Test]
        public void Test_Click_NextButton()
        {
            _wizardControl.NextButton.PerformClick();
            Assert.AreSame(_controller.Control2, _wizardControl.CurrentControl);
        }

        [Test]
        public void Test_ClickPreviousButton()
        {
            _wizardControl.Next();
            _wizardControl.PreviousButton.PerformClick();
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
        }

        [Test]
        public void TestNextButtonText()
        {
            _wizardControl.Next();
            Assert.AreEqual("Finish", _wizardControl.NextButton.Text);
            _wizardControl.Previous();
            Assert.AreEqual("Next", _wizardControl.NextButton.Text);
        }

        [Test]
        public void TestPreviousButtonDisabledAtStart()
        {
            Assert.IsFalse(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void TestPreviousButtonEnabledAfterStart()
        {
            _wizardControl.Next();
            Assert.IsTrue(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void TestPreviousButtonDisabledAtFirstStep()
        {
            _wizardControl.Next();
            _wizardControl.Previous();
            Assert.IsFalse(_wizardControl.PreviousButton.Enabled);
        }

        [Test]
        public void Test_CallFinisheEvent()
        {
            //---------------Set up test pack-------------------
            bool finished = false;
            _wizardControl.Finished += delegate { finished = true; };
            //---------------Execute Test ----------------------
            _wizardControl.Next();
            _wizardControl.Finish();
            //---------------Test Result -----------------------
            Assert.IsTrue(_controller.FinishCalled);
            Assert.IsTrue(finished);
        }

        [Test, ExpectedException(typeof(WizardStepException))]
        public void TestFinishAtNonFinishStep()
        {
            _wizardControl.Finish();
        }

        [Test]
        public void TestNextClickAtLastStep()
        {
            Assert.IsFalse(_controller.FinishCalled);
            _wizardControl.Next();
            _wizardControl.NextButton.PerformClick();
            Assert.IsTrue(_controller.FinishCalled);
        }

        [Test]
        public void TestNextWhen_CanMoveOn_False()
        {
            _wizardControl.MessagePosted += delegate(string message) { _message = message; };
            _controller.ControlForStep1.AllowMoveOn = false;
            _wizardControl.Next();
            Assert.AreSame(_controller.ControlForStep1, _wizardControl.CurrentControl);
            Assert.AreEqual("Sorry, can't move on", _message);
            _controller.ControlForStep1.AllowMoveOn = true;
        }


 

        internal class MyWizardController : IWizardController
        {
            public MyWizardStep ControlForStep1 = new MyWizardStep();
            
            public MyWizardStep Control2 = new MyWizardStep();
            public bool FinishCalled = false;
            private readonly List<IWizardStep> _wizardSteps;
            private int _currentStep = -1;
            public MyWizardController()
            {
                _wizardSteps = new List<IWizardStep>();
                ControlForStep1.Name = "ControlForStep1";
                Control2.Name = "Control2";
                _wizardSteps.Add(ControlForStep1);
                _wizardSteps.Add(Control2);
     
            }

            public IWizardStep GetNextStep()
            {
                if (_currentStep < _wizardSteps.Count - 1)
                    return _wizardSteps[++_currentStep];
                else
                    throw new WizardStepException("Invalid Wizard Step: " + (_currentStep + 1));
            }

            public IWizardStep GetPreviousStep()
            {
                if (_currentStep > 0)
                    return _wizardSteps[--_currentStep];
                else throw new WizardStepException("Invalid Wizard Step: " + (_currentStep - 1));
            }


            public IWizardStep GetFirstStep()
            {
                FinishCalled = false;
                return _wizardSteps[_currentStep = 0];
            }

            public bool IsLastStep()
            {
                return (_currentStep == _wizardSteps.Count - 1);
            }

            public bool IsFirstStep()
            {
                return (_currentStep == 0);
            }

            public void Finish()
            {
                if (IsLastStep())
                    FinishCalled = true;
                else throw new WizardStepException("Invalid call to Finish(), not at last step");
            }

            public bool CanMoveOn(out string message)
            {
                return _wizardSteps[_currentStep].CanMoveOn(out message);
            }

            public int StepCount
            {
                get { return _wizardSteps.Count; }
            }

            public IWizardStep GetCurrentStep()
            {
                return _wizardSteps[_currentStep];
            }
        }

        internal class MyWizardStep : Gizmox.WebGUI.Forms.Control, IWizardStep
        {
            private bool _allowMoveOn = true;

            #region IWizardStep Members

            public void InitialiseStep()
            {}

            public bool CanMoveOn(out string message)
            {
               message = "";
               if (!AllowMoveOn) message = "Sorry, can't move on";
                return AllowMoveOn;
            }

            #endregion

            public bool AllowMoveOn
            {
                get { return _allowMoveOn; }
                set { _allowMoveOn = value; }
            }

            IControlCollection IControlChilli.Controls
            {
                get
                {
                    return null;

                }
            }

            ///<summary>
            ///Returns a <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any. This method should not be overridden.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.String"></see> containing the name of the <see cref="T:System.ComponentModel.Component"></see>, if any, or null if the <see cref="T:System.ComponentModel.Component"></see> is unnamed.
            ///</returns>
            ///
            public override string ToString()
            {
                return Name;
            }
        }
    }
}
