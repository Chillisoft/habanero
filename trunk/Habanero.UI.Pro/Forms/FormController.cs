using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Ui.Misc;

namespace Habanero.Ui.Application
{
    /// <summary>
    /// Provides a super-class for form controllers
    /// </summary>
    public abstract class FormController
    {
        private Hashtable _formsbyHeading;
        private Hashtable _formsbyForm;
        private Form _parentForm;
        private float _fontSize = 0.0f;
        
        /// <summary>
        /// Constructor to initialise a new controller
        /// </summary>
        /// <param name="parentForm">The parent form</param>
        public FormController(Form parentForm)
        {
            _parentForm = parentForm;
        }

        /// <summary>
        /// Sets the default font size for the mdi forms.  Don't set this to get the default font size.
        /// </summary>
        public float FontSize {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// Sets the current control to the one with the specified heading
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <returns>Returns the relevant IFormControl object</returns>
        public IFormControl SetCurrentControl(String heading)
        {
            if (_formsbyHeading == null)
            {
                _formsbyHeading = new Hashtable();
                _formsbyForm = new Hashtable();
            }
            if (_formsbyHeading.Contains(heading))
            {
                Form frm = (Form)_formsbyHeading[heading];
                if (_fontSize != 0.0f) frm.Font = new Font(frm.Font.FontFamily, _fontSize );
                frm.Show();
                frm.Refresh();
                frm.Focus();
                frm.PerformLayout();
                return (IFormControl)frm.Controls[0];
            }
            else
            {
                IFormControl formCtl = GetFormControl(heading);

                Form newMdiForm = new Form();
                newMdiForm.Width = 800;
                newMdiForm.Height = 600;
                newMdiForm.MdiParent = _parentForm;

                newMdiForm.WindowState = FormWindowState.Maximized;

                Control ctl = (Control)formCtl;
                ctl.Dock = DockStyle.Fill;
                newMdiForm.Text = heading;
                newMdiForm.Controls.Clear();
                newMdiForm.Controls.Add(ctl);
                newMdiForm.Show();
                _formsbyHeading.Add(heading, newMdiForm);
                _formsbyForm.Add(newMdiForm, heading);
                formCtl.SetForm(newMdiForm);
                newMdiForm.Closed += new EventHandler(MdiFormClosed);

                return formCtl;
            }
        }

        /// <summary>
        /// Returns the form control with the heading specified
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <returns>Returns the IFormControl object if found</returns>
        protected abstract IFormControl GetFormControl(string heading);

        /// <summary>
        /// Returns the control with the heading specified
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <returns>Returns the control if found</returns>
        protected Control GetControl(string heading) {
            Form frm = (Form)this._formsbyHeading[heading];
            if (frm != null)
            {
                return frm.Controls[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Handles the event of a form being closed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void MdiFormClosed(object sender, EventArgs e)
        {
            string heading = (string)_formsbyForm[sender];
            _formsbyHeading.Remove(heading);
            _formsbyForm.Remove(sender);
        }


    }
}
