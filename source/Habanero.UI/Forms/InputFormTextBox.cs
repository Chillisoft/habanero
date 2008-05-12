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


using System.Windows.Forms;
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can enter text into an input box,
    /// including the option of entering a masked password
    /// </summary>
    public class InputFormTextBox
    {
        private readonly string _message;
        protected TextBox _textBox;
        private bool _isPasswordField = false;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numLines">The number of lines to make available
        /// in the text box (its height in lines)</param>
        public InputFormTextBox(string message, int numLines)
        {
            _message = message;
            _textBox = ControlFactory.CreateTextBox();
            if (numLines > 1)
            {
                _textBox.Multiline = true;
                _textBox.Height = _textBox.Height*numLines;
                _textBox.ScrollBars = ScrollBars.Vertical;
            }
        }

        /// <summary>
        /// Gets and sets whether the input field is a password field, in
        /// which case the characters typed in by the user would be masked
        /// with an asterisk
        /// </summary>
        public bool IsPasswordField
        {
            get { return _isPasswordField; }
            set
            {
                _isPasswordField = value;
                if (value)
                {
                    _textBox.PasswordChar = '*';
                }
                else
                {
                    _textBox.PasswordChar = (char) 0;
                }
            }
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            Panel messagePanel = new Panel();
            FlowLayoutManager messagePanelManager = new FlowLayoutManager(messagePanel);
            messagePanelManager.AddControl(ControlFactory.CreateLabel(_message, false));
            messagePanelManager.AddControl(_textBox);
            messagePanel.Height = _textBox.Height + 40;
            messagePanel.Width = ControlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            _textBox.Width = messagePanel.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the text displayed in the text box
        /// </summary>
        public string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }
    }
}