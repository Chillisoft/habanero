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
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// A form that displays a wizard.  This form simply wraps the WizardControl in a form and handles communication with the user.
    /// </summary>
    public partial class WizardFormGiz : FormGiz, IFormChilli
    {
        private readonly IWizardController _wizardController;
        private readonly WizardControlGiz _uxWizardControl;
        private readonly IControlFactory _controlFactory;
        private string _wizardText;

        /// <summary>
        /// Initialises the WizardForm, sets the controller and starts the wizard.
        /// </summary>
        /// <param name="controller">the wizrd controller that controls moving the wizard steps and the </param>
        /// <param name="controlFactory">The control factory to use for creating any controls</param>
        public WizardFormGiz(IWizardController controller, IControlFactory controlFactory)
        {
            _wizardController = controller;

            _controlFactory = controlFactory;
//            _uxWizardControl = new WizardControlGiz(controller, _controlFactory);
            _uxWizardControl = (WizardControlGiz) controlFactory.CreateWizardControl(controller);
            this._uxWizardControl.MessagePosted += _uxWizardControl_MessagePosted;
            this._uxWizardControl.Finished += this._uxWizardControl_Finished;
            this._uxWizardControl.StepChanged += this._uxWizardControl_StepChanged;
            this._uxWizardControl.CancelButton.Click += CancelButton_OnClick;
            InitializeComponent();
            WizardControl.WizardController = _wizardController;
            DialogResult = DialogResult.Cancel;
            this.Closing += WizardFormGiz_Closing;
        }

        void WizardFormGiz_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: should follow pattern of checking dirty status and if dirty ask user
            this._wizardController.CancelWizard();
        }

        private void CancelButton_OnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _uxWizardControl_StepChanged(string headingText)
        {
            this.Text = this.WizardText + " - " + headingText;
        }

        public string WizardText
        {
            get { return _wizardText; }
            set
            {
                _wizardText = value;
                Text = _wizardText;
            }
        }

        /// <summary>
        /// Gets the WizardControl
        /// </summary>
        public IWizardControl WizardControl
        {
            get { return _uxWizardControl; }
        }
        
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        public void Refresh()
        {
            // do nothing
        }

        IFormChilli IFormChilli.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form)value; }
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

        private static bool Show(string title, IWizardController wizardController, bool showDialog,
                                 IControlFactory controlFactory)
        {
            WizardFormGiz form = new WizardFormGiz(wizardController, controlFactory);
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            if (showDialog)
            {
                return form.ShowDialog() == DialogResult.OK;
            }
            else
            {
                form.Show();
                return true;
            }
        }
    }
}