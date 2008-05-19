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


using System;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// A form that displays a wizard.  This form simply wraps the WizardControl in a form and handles communication with the user.
    /// </summary>
    public partial class WizardFormGiz : Form, IFormChilli
    {
        private readonly IWizardController _wizardController;
        private readonly WizardControlGiz _uxWizardControl;
        private IControlFactory _controlFactory;
        ///// <summary>
        ///// Initialises the WizardForm
        ///// </summary>
        //public WizardFormGiz()
        //{
        //    InitializeComponent();
        //}

        /// <summary>
        /// Initialises the WizardForm, sets the controller and starts the wizard.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="controlFactory"></param>
        public WizardFormGiz(IWizardController controller, IControlFactory controlFactory)
        {
            _wizardController = controller;

            _controlFactory = controlFactory;
            _uxWizardControl = new WizardControlGiz(controller, _controlFactory);
            InitializeComponent();
            WizardControl.WizardController = _wizardController;
            DialogResult = DialogResult.Cancel;
            this._uxWizardControl.MessagePosted += _uxWizardControl_MessagePosted;
            this._uxWizardControl.Finished += this._uxWizardControl_Finished;
        }

        /// <summary>
        /// Gets the WizardControl
        /// </summary>
        public IWizardControl WizardControl
        {
            get
            {
                return _uxWizardControl;
            }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        private void _uxWizardControl_Finished(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private static void _uxWizardControl_MessagePosted(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// A convenience method to show a wizard using a particular title and wizard controller
        /// </summary>
        /// <param name="title">The title of the wizard, displayed in the title bar of the form</param>
        /// <param name="wizardController">The wizard controller</param>
        /// <param name="controlFactory">the factory used to create the controls on the wizard</param>
        public static void Show(string title, IWizardController wizardController, IControlFactory controlFactory)
        {
            Show(title, wizardController, false, controlFactory);
        }

        /// <summary>
        /// A convenience method to show a wizard using a particular title and wizard controller in a dialog
        /// </summary>
        /// <param name="title">The title of the wizard, displayed in the title bar of the form</param>
        /// <param name="wizardController">The wizard controller</param>
        /// <param name="controlFactory">the factory used to create the controls on the wizard</param>
        public static bool ShowDialog(string title, IWizardController wizardController, IControlFactory controlFactory)
        {
            return Show(title, wizardController, true, controlFactory);
        }

        private static bool Show(string title, IWizardController wizardController, bool showDialog, IControlFactory controlFactory)
        {
            WizardFormGiz form = new WizardFormGiz(wizardController, controlFactory);
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