using System;
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class WizardControlGiz : UserControl, IWizardControl
    {
        private IControlChilli _currentControl;
        private readonly IButton _nextButton;
        private readonly IButton _previousButton;
        private IWizardController _wizardController;
        private readonly IControlFactory _controlFactory;
        private readonly IPanel _wizardStepPanel;

        public event EventHandler Finished;
        public event Action<string> MessagePosted;


        /// <summary>
        /// Initialises the WizardControl with the IWizardController.  No logic is performed other than storing the wizard controller.
        /// </summary>
        /// <param name="wizardController"></param>
        /// <param name="controlFactory">The control factory that this control will use to create a button</param>
        public WizardControlGiz(IWizardController wizardController, IControlFactory controlFactory)
        {
            _wizardController = wizardController;
            _controlFactory = controlFactory;


            _nextButton = _controlFactory.CreateButton("Next");
            _nextButton.Click += this.uxNextButton_Click;
            _nextButton.Size = new Size(75, 38);
            _nextButton.TabIndex = 1;

            _previousButton = _controlFactory.CreateButton("Previous");
            _previousButton.Click += this.uxPreviousButton_Click;
            _previousButton.Size = new Size(75, 38);
            _previousButton.TabIndex = 0;

            //The layout manager code is not NUnit tested 
            IPanel buttonPanel = controlFactory.CreatePanel();
            FlowLayoutManager layoutManager = new FlowLayoutManager(buttonPanel, _controlFactory);
            layoutManager.Alignment= FlowLayoutManager.Alignments.Right;
            layoutManager.AddControl(_previousButton);
            layoutManager.AddControl(_nextButton);

            _wizardStepPanel = controlFactory.CreatePanel();
            BorderLayoutManagerGiz borderLayoutManager = new BorderLayoutManagerGiz(this, _controlFactory);
            borderLayoutManager.AddControl(_wizardStepPanel, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(buttonPanel, BorderLayoutManager.Position.South);

        }

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        public IControlChilli CurrentControl
        {
            get { return _currentControl;
            }
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

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        /// <summary>
        /// Attempts to go to the next step in the wizard.  If this is disallowed by the wizard controller a MessagePosted event will be fired.
        /// </summary>
        public void Next()
        {
            string message;
            if (_wizardController.CanMoveOn(out message))
            {
                SetStep(_wizardController.GetNextStep());
                if (_wizardController.IsLastStep())
                {
                    _nextButton.Text = "Finish";
                }
                SetPreviousButtonState();
            }
            else
            {
                FireMessagePosted(message);
            }
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
            IControlChilli stepControl = step;
            if (stepControl != null)
            {
                _currentControl = stepControl;
                //The border layout manager clearing panel etc not unit tested
                _wizardStepPanel.Controls.Clear();
                BorderLayoutManagerGiz borderLayoutManager = new BorderLayoutManagerGiz(_wizardStepPanel, _controlFactory);
                borderLayoutManager.AddControl(stepControl);
                step.InitialiseStep();
            }
            else
            {
                throw new WizardStepException("IWizardStep of type " + step.GetType().FullName + " is not a Control");
            }
        }
        private void uxNextButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_wizardController.IsLastStep())
                {
                    Finish();
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
    }
}