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
using System.Windows.Forms;

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