//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System;
using System.Drawing;
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
            IWizardStep step1 = _mock.CreateMock<IWizardStep>();
            wizardController.AddStep(step1);

            wizardController.GetNextStep();

            //------------------------Execute----------------------------
            wizardController.Finish();

            //------------------------Verify Result ---------------------
            Assert.IsTrue(wizardFinishedFires);
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

        [Test]
        public void Test_WizardControllerCancelsWizardSteps_Win()
        {
            //TODO:Implement
        }

        [Test]
        public void Test_WizardControllerCancelsWizardSteps_VWG()
        {
            Test_WizardControllerCancelsWizardSteps(new Habanero.UI.VWG.ControlFactoryVWG());
        }
        
        public void Test_WizardControllerCancelsWizardSteps(IControlFactory controlFactory)
        {
            //---------------Set up test pack-------------------
            WizardController wizardController = new WizardController();
            WizardStepStub wzrdStep = new WizardStepStub();
            IWizardControl wizardControl = controlFactory.CreateWizardControl(wizardController);
            wizardController.AddStep(wzrdStep);
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(wzrdStep.Cancelled);

            //---------------Execute Test ----------------------
            wizardControl.CancelButton.PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(wzrdStep.Cancelled);

        }
        //WizardController catches WizardControl Cancelevent
    }

    internal class WizardStepStub : IWizardStep
    {
        private bool _cancelled = false;

        public bool Cancelled
        {
            get { return _cancelled; }
        }
        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        public void InitialiseStep()
        {

        }
        /// <summary>
        /// Verifies whether this step can be passed.
        /// </summary>
        /// <param name="message">Error message should moving on be disallowed. This message will be displayed to the user by the WizardControl.</param>
        /// <returns></returns>
        public bool CanMoveOn(out string message)
        {
            message = "";
            return true;
        }

        /// <summary>
        /// Verifies whether the user can move back from this step.
        /// </summary>
        /// <returns></returns>
        public bool CanMoveBack()
        {
            return false;
        }

        /// <summary>
        /// The text that you want displayed at the top of the wizard control when this step is active.
        /// </summary>
        public string HeaderText
        {
            get { return ""; }
        }

        /// <summary>
        /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
        /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
        /// its Cancel method is called.
        /// </summary>
        public void CancelStep()
        {
            _cancelled = true;
        }

        #region IControlInterface

        /// <summary>
        /// Occurs on clicking the button etc.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Occurs when the control is double clicked.
        /// </summary>
        public event EventHandler DoubleClick;
        public event EventHandler Resize;
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Gets/Sets the width position
        /// </summary>
        public int Width
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IControlCollection Controls
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the control visability.  
        /// </summary>
        public bool Visible
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// The order in which tabbing through the form will tab to this control
        /// </summary>
        public int TabIndex
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>Gets a value indicating whether the control has input focus.</summary>
        /// <returns>true if the control has focus; otherwise, false.</returns>
        public bool Focused
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets/Sets the height position
        /// </summary>
        public int Height
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets/Sets the top position
        /// </summary>
        public int Top
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets/Sets the bottom position
        /// </summary>
        public int Bottom
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets/Sets the left position
        /// </summary>
        public int Left
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets/Sets the right position
        /// </summary>
        public int Right
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.  
        /// </summary>
        public string Text
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the name associated with this control.  
        /// </summary>
        public string Name
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the control enabled state.  
        /// </summary>
        public bool Enabled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Color ForeColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Color BackColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tab stop is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if tab stop is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool TabStop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value></value>
        public Size Size
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Activates the control.  
        /// </summary>
        public void Select()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this control has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this control has children; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildren
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>Gets or sets the size that is the upper limit that can specify.</summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.</returns>
        /// <filterpriority>1</filterpriority>
        public Size MaximumSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>Gets or sets the size that is the lower limit that can specify.</summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.</returns>
        /// <filterpriority>1</filterpriority>
        public Size MinimumSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        public Font Font
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public void SuspendLayout()
        {
            throw new NotImplementedException();
        }

        public void ResumeLayout(bool performLayout)
        {
            throw new NotImplementedException();
        }

        public void Invalidate()
        {
            throw new NotImplementedException();
        }

        public Point Location
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DockStyle Dock
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler TextChanged;

        #endregion
    }
}