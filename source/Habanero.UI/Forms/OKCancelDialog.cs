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
using System.Drawing;
using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form that contains a specified control and "OK" and
    /// "Cancel" buttons
    /// </summary>
    public class OKCancelDialog
    {
        private readonly Control _controlToNest;
        private readonly string _title;
        private readonly Point _position;
        private Form _form;

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        public OKCancelDialog(Control controlToNest) : this(controlToNest, "")
        {
        }

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        /// <param name="title">The dialog title</param>
        public OKCancelDialog(Control controlToNest, string title) : this(controlToNest, title, new Point(0, 0))
        {
        }

        /// <summary>
        /// Constructor to initialise the dialog with a control to display
        /// </summary>
        /// <param name="controlToNest">The control to add to the dialog</param>
        /// <param name="title">The dialog title</param>
        /// <param name="position">The position at which to display the
        /// dialog</param>
        public OKCancelDialog(Control controlToNest, string title, Point position)
        {
            _controlToNest = controlToNest;
            _title = title;
            _position = position;
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user.  Calls the
        /// CreateDialog() method.
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            return CreateDialog().ShowDialog();
        }

        /// <summary>
        /// Sets up the form
        /// </summary>
        /// <returns>Returns the Form object created</returns>
        private Form CreateDialog()
        {
            _form = new Form();
            _form.Padding = new Padding(5,5,5,5);
            ButtonControl buttons = new ButtonControl();
            Button cancelButton = buttons.AddButton("Cancel", new EventHandler(CancelButtonClickHandler));
            Button okButton = buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

            ResizeForm(buttons, okButton);
            BorderLayoutManager borderLayoutManager = new BorderLayoutManager(_form);
            borderLayoutManager.BorderSize = 5;
            //GridLayoutManager manager = new GridLayoutManager(_form);
            //manager.SetGridSize(2, 1);
            //manager.FixAllRowsBasedOnContents();
            //manager.AddControl(_controlToNest);
            //manager.AddControl(buttons);
            borderLayoutManager.AddControl(_controlToNest, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(buttons, BorderLayoutManager.Position.South);

            _form.Text = _title;
            if (_position.X != 0 && _position.Y != 0)
            {
                _form.StartPosition = FormStartPosition.Manual;
                _form.Location = _position;
            }
            else
            {
                _form.StartPosition = FormStartPosition.CenterScreen;
            }
            _form.AcceptButton = okButton;
            okButton.NotifyDefault(true);
            _form.CancelButton = cancelButton;
            bool formResizing = false;
            EventHandler resizeEventHandler = delegate(object sender, EventArgs e)
            {
                if (!formResizing)
                {
                    formResizing = true;
                    ResizeForm(buttons, okButton);
                    formResizing = false;
                }
            };
            EventHandler formCloseEventHandler = null;
            formCloseEventHandler = delegate(object sender, EventArgs e)
            {
                _controlToNest.Resize -= resizeEventHandler;
                _form.Closed -= formCloseEventHandler;
            };
            _controlToNest.Resize += resizeEventHandler;
            _form.Closed += formCloseEventHandler;
            return _form;
        }

        private void ResizeForm(ButtonControl buttons, Button okButton)
        {
            _form.Width = Math.Max(_controlToNest.Width, okButton.Width * 2 + 30) + 8 + 10;
            _form.Height = _controlToNest.Height + buttons.Height + 40 + 10;
        }


        /// <summary>
        /// Handles the event of the Cancel button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CancelButtonClickHandler(object sender, EventArgs e)
        {
            _form.DialogResult = DialogResult.Cancel;
            _form.Close();
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which
        /// closes the dialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            _form.DialogResult = DialogResult.OK;
            _form.Close();
        }
    }
}