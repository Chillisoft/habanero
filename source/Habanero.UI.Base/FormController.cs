// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Drawing;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a controller for forms that keeps a reference to all open
    /// forms so that a user can switch between them.  The specific advantage
    /// of using a controller is that the state of the window can be maintained and
    /// the form regeneration time is saved.  This controller can
    /// be used to populate a "Window" menu item with open forms.
    /// </summary>
    public abstract class FormController
    {
        private Hashtable _formsbyHeading;
        private Hashtable _formsbyForm;
        private readonly IFormHabanero _parentForm;
        private readonly IControlFactory _controlFactory;
        private float _fontSize;
        
        /// <summary>
        /// Constructor to initialise a new controller
        /// </summary>
        /// <param name="parentForm">The parent form</param>
        /// <param name="controlFactory"></param>
        protected FormController(IFormHabanero parentForm, IControlFactory controlFactory)
        {
            _parentForm = parentForm;
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Sets the default font size for the mdi forms.
        /// Don't set this if you want to get the default font size.
        /// </summary>
        public float FontSize
        {
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
                IFormHabanero frm = (IFormHabanero)_formsbyHeading[heading];
                if (_fontSize != 0.0f) frm.Font = new Font(frm.Font.FontFamily, _fontSize );
                frm.Show();
                frm.Refresh();
                frm.Focus();
                frm.PerformLayout();
                return (IFormControl)frm.Controls[0];
            }
            IFormControl formCtl = GetFormControl(heading);

            IFormHabanero newMdiForm = _controlFactory.CreateForm();
            newMdiForm.Width = 800;
            newMdiForm.Height = 600;
            newMdiForm.MdiParent = _parentForm;
            newMdiForm.WindowState = FormWindowState.Maximized;

            //IControlHabanero ctl = formCtl;
       
            newMdiForm.Text = heading;
            newMdiForm.Controls.Clear();
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(newMdiForm);

            layoutManager.AddControl((IControlHabanero) formCtl, BorderLayoutManager.Position.Centre);
            newMdiForm.Show();
            _formsbyHeading.Add(heading, newMdiForm);
            _formsbyForm.Add(newMdiForm, heading);
            formCtl.SetForm(newMdiForm);
            newMdiForm.Closed += MdiFormClosed;

            return formCtl;
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
        protected IControlHabanero GetControl(string heading)
        {
            IFormHabanero frm = (IFormHabanero)this._formsbyHeading[heading];
            return frm != null ? frm.Controls[0] : null;
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