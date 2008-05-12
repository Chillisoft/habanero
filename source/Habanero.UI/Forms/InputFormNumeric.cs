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
    /// Provides a super-class for a form in which a user can edit a numeric value
    /// </summary>
    public abstract class InputFormNumeric
    {
        private readonly string _message;
        protected NumericUpDown _numericUpDown;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputFormNumeric(string message)
        {
            _message = message;
            _numericUpDown = CreateNumericUpDown();
        }

        /// <summary>
        /// Creates a numeric up-down control for the form
        /// </summary>
        /// <returns>Returns the NumericUpDown control created</returns>
        protected abstract NumericUpDown CreateNumericUpDown();

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
            messagePanelManager.AddControl(_numericUpDown);
            messagePanel.Height = _numericUpDown.Height*2 + 20;
            messagePanel.Width = ControlFactory.CreateLabel(_message, false).PreferredWidth + 20;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }
    }
}