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
    /// An implementation of WizardStep that does nothing and is drawn as a blank panel.
    /// Can be used as a placeholder step in a Wizard that changes depending on selections made
    /// by users.
    /// </summary>
    public partial class NullWizardStep : UserControl, IWizardStep
    {
        /// <summary>
        /// Constructs the NullWizardStep
        /// </summary>
        public NullWizardStep()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void InitialiseStep()
        {
        }

        /// <summary>
        /// Always allows moving on.
        /// </summary>
        /// <param name="message">Will always be the empty string</param>
        /// <returns>True</returns>
        public bool CanMoveOn(out string message)
        {
            message = "";
            return true;
        }
    }
}
