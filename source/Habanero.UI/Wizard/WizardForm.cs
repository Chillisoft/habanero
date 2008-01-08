using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Habanero.Base;

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// A form that displays a wizard.  This form simply wraps the WizardControl in a form and handles communication with the user.
    /// </summary>
    public partial class WizardForm : Form
    {
        private readonly IWizardController _wizardController;

        /// <summary>
        /// Initialises the WizardForm
        /// </summary>
        public WizardForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises the WizardForm, sets the controller and starts the wizard.
        /// </summary>
        /// <param name="controller"></param>
        public WizardForm(IWizardController controller)
        {
            InitializeComponent();

            _wizardController = controller;
            WizardControl.WizardController = _wizardController;
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Gets the WizardControl
        /// </summary>
        public WizardControl WizardControl
        {
            get
            {
                return _uxWizardControl;
            }
        }

        private void _uxWizardControl_Finished()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _uxWizardControl_MessagePosted(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// A convenience method to show a wizard using a particular title and wizard controller
        /// </summary>
        /// <param name="title">The title of the wizard, displayed in the title bar of the form</param>
        /// <param name="wizardController">The wizard controller</param>
        public static void Show(string title, IWizardController wizardController)
        {
            Show(title, wizardController, false);
        }

        /// <summary>
        /// A convenience method to show a wizard using a particular title and wizard controller in a dialog
        /// </summary>
        /// <param name="title">The title of the wizard, displayed in the title bar of the form</param>
        /// <param name="wizardController">The wizard controller</param>
        public static bool ShowDialog(string title, IWizardController wizardController)
        {
            return Show(title, wizardController, true);
        }

        private static bool Show(string title, IWizardController wizardController, bool showDialog)
        {
            WizardForm form = new WizardForm(wizardController);
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            if (showDialog)
            {
                return form.ShowDialog() == DialogResult.OK;
            } else
            {
                form.Show();
                return true;
            }
        }
    }
}