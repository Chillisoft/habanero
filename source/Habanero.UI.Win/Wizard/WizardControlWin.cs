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
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides the controls for a wizard, which guides users through a process one
    /// step at a time.
    /// </summary>
    public class WizardControlWin : UserControlWin, IWizardControl
    {
        private IControlHabanero _currentControl;
        private IButton _nextButton;
        private IButton _previousButton;
        private IWizardController _wizardController;
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _wizardStepPanel;
        private IButton _cancelButton;

        /// <summary>
        /// Raised when the wizard is complete to notify the containing control or controlling object.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Raised when a message is communicated so the controlling object can display or log the message.
        ///  uses an <see cref="Action{T}"/> which is merely a predifined delegate that takes one parameter of Type T and
        /// returns a void.
        /// </summary>
        public event Action<string> MessagePosted;

        /// <summary>
        /// Raised when the wizard step changes. The new step is passed through as an event argument.
        /// </summary>
        public event Action<IWizardStep> StepChanged;


        /// <summary>
        /// The panel that the controls are physically being placed on.
        /// </summary>
        public IPanel WizardStepPanel
        {
            get { return _wizardStepPanel; }
        }

        /// <summary>
        /// Gets the Cancel Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton CancelButton
        {
            get { return _cancelButton; }
        }

        ///// <summary>
        ///// The label that is displayed at the top of the wizard control for each step.
        ///// </summary>
        //public ILabel HeadingLabel
        //{
        //    get { return _headingLabel; }
        //}

        /// <summary>
        /// Initialises the WizardControl with the IWizardController.  No logic is performed other than storing the wizard controller.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <param name="controlFactory">The control factory that this control will use to create a button</param>
        public WizardControlWin(IWizardController wizardController, IControlFactory controlFactory)
        {
            _wizardController = wizardController;
            _controlFactory = controlFactory;

            IPanel buttonPanel = CreateButtonPanel();

            _wizardStepPanel = _controlFactory.CreatePanel();
            
  
            //IGroupBox headerLabelGroupBox = CreateHeaderLabel(controlFactory);

            BorderLayoutManagerWin borderLayoutManager = new BorderLayoutManagerWin(this, _controlFactory);
            //borderLayoutManager.AddControl(headerLabelGroupBox, BorderLayoutManager.Position.North);
            borderLayoutManager.AddControl(_wizardStepPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(buttonPanel, BorderLayoutManager.Position.South);

        }

        //private IGroupBox CreateHeaderLabel(IControlFactory controlFactory)
        //{
        //    IGroupBox headerLabelGroupBox = controlFactory.CreateGroupBox();

        //    _headingLabel = controlFactory.CreateLabel();
        //    _headingLabel.Font = new Font(_headingLabel.Font, FontStyle.Bold);
        //    _headingLabel.Height = 15;
        //    _headingLabel.Visible = true;
        //    headerLabelGroupBox.Height = 30;
        //    FlowLayoutManager flowManager = new FlowLayoutManager(headerLabelGroupBox, controlFactory);
        //    flowManager.Alignment = FlowLayoutManager.Alignments.Left;
        //    flowManager.AddControl(_headingLabel);

        //    return headerLabelGroupBox;
        //}

        private IPanel CreateButtonPanel()
        {
            IPanel buttonPanel = _controlFactory.CreatePanel();
            FlowLayoutManager layoutManager = new FlowLayoutManager(buttonPanel, _controlFactory);
            layoutManager.Alignment= FlowLayoutManager.Alignments.Right;

            _cancelButton = _controlFactory.CreateButton("Cancel");
            _cancelButton.Click += this.uxCancelButton_Click;
            _cancelButton.Size = new Size(75, 38);
            _cancelButton.TabIndex = 0;
            layoutManager.AddControl(_cancelButton);

            _nextButton = _controlFactory.CreateButton("Next");
            _nextButton.Click += this.uxNextButton_Click;
            _nextButton.Size = new Size(75, 38);
            _nextButton.TabIndex = 1;
            layoutManager.AddControl(_nextButton);

            _previousButton = _controlFactory.CreateButton("Previous");
            _previousButton.Click += this.uxPreviousButton_Click;
            _previousButton.Size = new Size(75, 38);
            _previousButton.TabIndex = 0;
            layoutManager.AddControl(_previousButton);


            return buttonPanel;
        }

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        public IControlHabanero CurrentControl
        {
            get { return _currentControl; }
        }

        /// <summary>
        /// Gets the Next Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton NextButton
        {
            get { return _nextButton; }
        }

        /// <summary>
        /// Gets the Previous Button so that it can be programmatically interacted with.
        /// </summary>
        public IButton PreviousButton
        {
            get { return _previousButton; }
        }

        /// <summary>
        /// Gets or sets the WizardController.  Upon setting the controller, the Start() method is called to begin the wizard.
        /// </summary>
        public IWizardController WizardController
        {
            get { return _wizardController; }
            set
            {
                _wizardController = value;
                if (_wizardController != null) Start();
            }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Attempts to go to the next step in the wizard.  If this is disallowed by the wizard controller a MessagePosted event will be fired.
        /// </summary>
        public void Next()
        {
            DoIfCanMoveOn(delegate
            {
                SetStep(_wizardController.GetNextStep());
                if (_wizardController.IsLastStep())
                {
                    _nextButton.Text = "Finish";
                }
                SetPreviousButtonState();
            });
        }

        /// <summary>
        /// Attempts to go to the previous step in the wizard.
        ///  </summary>
        /// <exception cref="WizardStepException">If the wizard is on the first step this exception will be thrown.</exception>
        public void Previous()
        {
            SetStep(_wizardController.GetPreviousStep());
            _nextButton.Text = "Next";
            SetPreviousButtonState();
        }

        /// <summary>
        /// Starts the wizard by moving to the first step.
        /// </summary>
        public void Start()
        {
            SetStep(_wizardController.GetFirstStep());
            SetPreviousButtonState();
        }

        private void SetStep(IWizardStep step)
        {
            IControlHabanero stepControl = step;
            if (stepControl != null)
            {
                _currentControl = stepControl;
                FireStepChanged(step);
                //TODO: The border layout manager clearing panel etc not unit tested
                _wizardStepPanel.Controls.Clear();
                stepControl.Top = WizardControl.PADDING;
                stepControl.Left = WizardControl.PADDING;
                stepControl.Width = _wizardStepPanel.Width - WizardControl.PADDING*2;
                stepControl.Height = _wizardStepPanel.Height - WizardControl.PADDING*2;
                _wizardStepPanel.Controls.Add(stepControl);
                //BorderLayoutManagerGiz borderLayoutManager = new BorderLayoutManagerGiz(_wizardStepPanel, _controlFactory);
                //borderLayoutManager.AddControl(stepControl, BorderLayoutManager.Position.Centre);

                step.InitialiseStep();
            }
            else
            {
                throw new WizardStepException("IWizardStep of type " + step.GetType().FullName + " is not a Control");
            }
        }

        private delegate void Operation();

        private void DoIfCanMoveOn(Operation operation)
        {
            string message;
            if (_wizardController.CanMoveOn(out message))
            {
                operation();
            }
            else
            {
                FireMessagePosted(message);
            }
        }


        private void uxCancelButton_Click(object sender, EventArgs e)
        {
            _wizardController.CancelWizard();
        }

        private void uxNextButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_wizardController.IsLastStep())
                {
                    DoIfCanMoveOn(delegate { Finish(); });
                }
                else
                {
                    Next();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Cannot complete this wizard step due to an error:",
                                                          "Wizard Step Error");
            }
        }

        private void uxPreviousButton_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void SetPreviousButtonState()
        {
            _previousButton.Enabled = !_wizardController.IsFirstStep();
            if (_previousButton.Enabled)
            {
                _previousButton.Enabled = _wizardController.GetCurrentStep().CanMoveBack();
            }
        }

        /// <summary>
        /// Calls the finish method on the controller to being the completion process.  If this is successful the Finished event is fired.
        /// </summary>
        public void Finish()
        {
            _wizardController.Finish();
            FireFinished();
        }

        private void FireFinished()
        {
            if (Finished != null)
            {
                Finished(this,new EventArgs());
            }
        }
        private void FireMessagePosted(string message)
        {
            if (MessagePosted != null)
            {
                MessagePosted(message);
            }
        }
        private void FireStepChanged(IWizardStep wizardStep)
        {
            if (StepChanged != null)
            {
                StepChanged(wizardStep);
            }
        }
    }
}