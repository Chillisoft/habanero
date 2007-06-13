using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.UI.Misc.v2;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Provides a super-class for form controllers
    /// </summary>
    public abstract class FormController
    {
        private Hashtable _formsbyHeading;
        private Hashtable _formsbyForm;
        private Form _parentForm;
        
        /// <summary>
        /// Constructor to initialise a new controller
        /// </summary>
        /// <param name="parentForm">The parent form</param>
        public FormController(Form parentForm)
        {
            _parentForm = parentForm;
        }

        /// <summary>
        /// Sets the current control to the one with the specified heading
        /// </summary>
        /// <param name="heading">The heading</param>
        /// <returns>Returns the relevant FormControl object</returns>
        public FormControl SetCurrentControl(String heading)
        {
            if (_formsbyHeading == null)
            {
                _formsbyHeading = new Hashtable();
                _formsbyForm = new Hashtable();
            }
            if (_formsbyHeading.Contains(heading))
            {
                Form frm = (Form)_formsbyHeading[heading];
                frm.Show();
                frm.Refresh();
                frm.Focus();
                frm.PerformLayout();
                return (FormControl)frm.Controls[0];
            }
            else
            {
                FormControl formCtl = GetFormControl(heading);

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
        /// <returns>Returns the FormControl object if found</returns>
        protected abstract FormControl GetFormControl(string heading);

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
