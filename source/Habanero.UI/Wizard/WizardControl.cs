using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.UI.Wizard
{

    /// <summary>
    /// A control that displays a Wizard defined by an IWizardController.
    /// </summary>
    /// 
    public partial class WizardControl : UserControl
    {
        /// <summary>
        /// A delegate that is used when the wizard is finished.
        /// </summary>
        public delegate void FinishedDelegate();

        /// <summary>
        /// A delegate that is used when a message is posted.
        /// </summary>
        /// <param name="message"></param>
        public delegate void MessagePostedDelegate(string message);

        /// <summary>
        /// Raised when the wizard is complete to notify the containing control or controlling object.
        /// </summary>
        public event FinishedDelegate Finished;

        /// <summary>
        /// Raised when a message is communicated so the controlling object can display or log the message.
        /// </summary>
        public event MessagePostedDelegate MessagePosted;

        private IWizardController _wizardController;

        internal WizardControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises the WizardControl with the IWizardController.  No logic is performed other than storing the wizard controller.
        /// </summary>
        /// <param name="wizardController"></param>
        public WizardControl(IWizardController wizardController)
        {
            _wizardController = wizardController;
            InitializeComponent();
        }

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        public Control CurrentControl
        {
            get { return this.splitContainer1.Panel1.Controls[0]; }
        }

        /// <summary>
        /// Gets the Next Button so that it can be programmatically interacted with.
        /// </summary>
        public Button NextButton
        {
            get { return uxNextButton; }
        }

        /// <summary>
        /// Gets the Previous Button so that it can be programmatically interacted with.
        /// </summary>
        public Button PreviousButton
        {
            get { return uxPreviousButton; }
        }

        /// <summary>
        /// Gets or sets the WizardController.  Upon setting the controller, the Start() method is called to begin the wizard.
        /// </summary>
        public IWizardController WizardController
        {
            get { return _wizardController;  }
            set { _wizardController = value;
                if (_wizardController != null) Start();
            }
        }

        /// <summary>
        /// Attempts to go to the next step in the wizard.  If this is disallowed by the wizard controller a MessagePosted event will be fired.
        /// </summary>
        public void Next()
        {
            string message;
            if (_wizardController.CanMoveOn(out message)) {
                SetStep(_wizardController.GetNextStep());
                if (_wizardController.IsLastStep()) {
                    uxNextButton.Text = "Finish";
                }
                SetPreviousButtonState();
            } else {
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
            uxNextButton.Text = "Next";
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
            Control stepControl = step as Control;
            if (stepControl != null) {
                splitContainer1.Panel1.Controls.Clear();
                stepControl.Dock = DockStyle.Fill;
                splitContainer1.Panel1.Controls.Add(stepControl);
                step.InitialiseStep();
            } else {
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
            } catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "Cannot complete this wizard step due to an error:", "Wizard Step Error");
            }
        }

        private void uxPreviousButton_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void SetPreviousButtonState()
        {
            uxPreviousButton.Enabled = !_wizardController.IsFirstStep();
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
            if (Finished != null) {
                Finished();
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