using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Wizard;

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// A basic implementation of WizardStep that can be used for simply displaying a message.  
    /// Should a step be required that is a simple message for the user (such as at the end of a wizard), this step can be used
    /// </summary>
    public partial class MessageWizardStep : UserControl, IWizardStep
    {
        /// <summary>
        /// Constructs the MessageWizardStep
        /// </summary>
        public MessageWizardStep()
        {
            InitializeComponent();
        }


        public void InitialiseStep() { }

        /// <summary>
        /// Always returns true as this wizard step is simply for displaying a message to a user.
        /// </summary>
        /// <param name="message">Out parameter that will always be the empty string</param>
        /// <returns>true</returns>
        public bool CanMoveOn(out string message)
        {
            message = "";
            return true;
        }

        /// <summary>
        /// Sets the message to be displayed to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public void SetMessage(string message)
        {
            _uxMessageLabel.Text = message;
        }
    }
}
