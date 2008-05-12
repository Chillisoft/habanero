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
using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a popup box that displays a message to the user
    /// </summary>
    public class OKMessageDialog
    {
        private readonly string _title;
        private readonly int _height;
        private readonly int _width;
        private readonly string _message;
        private Form _form;

        /// <summary>
        /// Constructor to initialise the form with some given details
        /// </summary>
        /// <param name="title">The form title</param>
        /// <param name="message">The message to display</param>
        /// <param name="width">The width of the form</param>
        /// <param name="height">The height of the form</param>
        public OKMessageDialog(string title, string message, int width, int height)
        {
            _message = message;
            _width = width;
            _height = height;
            _title = title;
        }

        /// <summary>
        /// Constructor to initialise the form with a default width and height
        /// </summary>
        /// <param name="title">The form title</param>
        /// <param name="message">The message to display</param>
        public OKMessageDialog(string title, string message) : 
            this(title, message, 250, 150)
        {
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        public void ShowDialog()
        {
            _form = new Form();
            _form.Height = _height;
            _form.Width = _width;
            _form.Text = _title;

            BorderLayoutManager manager = new BorderLayoutManager(_form);
            manager.AddControl(ControlFactory.CreateLabel(_title, false), BorderLayoutManager.Position.North);
            TextBox tb = ControlFactory.CreateTextBox();
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.Text = _message;
            manager.AddControl(tb, BorderLayoutManager.Position.Centre);

            ButtonControl buttons = new ButtonControl();
            buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));
            manager.AddControl(buttons, BorderLayoutManager.Position.South);

            _form.ShowDialog();
        }

        /// <summary>
        /// Handles the event of the OK button being pressed, which closes
        /// the form
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OKButtonClickHandler(object sender, EventArgs e)
        {
            _form.Close();
        }
    }
}