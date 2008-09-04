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
using Habanero.UI.Base;
using DialogResult=System.Windows.Forms.DialogResult;
using FormStartPosition=System.Windows.Forms.FormStartPosition;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Represents a form containing a wizard control that guides users
    /// through a process step by step.
    /// This form simply wraps the WizardControl in a form and handles communication with the user.
    /// </summary>
    public partial class WizardFormWin : FormWin, IWizardForm
    {
        private readonly IWizardController _wizardController;
        private readonly WizardControlWin _uxWizardControl;
        private readonly IControlFactory _controlFactory;
        private string _wizardText;

        /// <summary>
        /// Initialises the WizardForm, sets the controller and starts the wizard.
        /// </summary>
        /// <param name="controller">the wizrd controller that controls moving the wizard steps and the </param>
        public WizardFormWin(IWizardController controller) : this(controller, GlobalUIRegistry.ControlFactory)
        {
        }

        /// <summary>
        /// Initialises the WizardForm, sets the controller and starts the wizard.
        /// </summary>
        /// <param name="controller">the wizrd controller that controls moving the wizard steps and the </param>
        /// <param name="controlFactory">The control factory to use for creating any controls</param>
        public WizardFormWin(IWizardController controller, IControlFactory controlFactory)
        {
            _wizardController = controller;

            _controlFactory = controlFactory;
//            _uxWizardControl = new WizardControlGiz(controller, _controlFactory);
            _uxWizardControl = (WizardControlWin)controlFactory.CreateWizardControl(controller);
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

        /// <summary>
        /// Gets and sets the text to dispaly
        /// </summary>
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

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Forces the form to invalidate its client area and
        /// immediately redraw itself and any child controls
        /// </summary>
        public void Refresh()
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the current multiple document interface (MDI) parent form of this form
        /// </summary>
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
            WizardFormWin form = new WizardFormWin(wizardController, controlFactory);
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            if (showDialog)
            {
                return form.ShowDialog() == (DialogResult)Base.DialogResult.OK;
            }
            else
            {
                form.Show();
                return true;
            }
        }
    }
}