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
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can choose a folder name from the file
    /// system
    /// </summary>
    public class InputFormFolderSelect
    {
        private readonly string _message;
        protected TextBox _textBox;
        private string _folder;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputFormFolderSelect(string message)
        {
            _message = message;
            _textBox = ControlFactory.CreateTextBox();
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
            messagePanelManager.AddGlue();
            Button selectButton =
                ControlFactory.CreateButton("Select...", new EventHandler(SelectFolderButtonClickHandler));
            messagePanelManager.AddControl(selectButton);
            messagePanel.Height = _textBox.Height + 40;
            messagePanel.Width = Math.Max(250, ControlFactory.CreateLabel(_message, false).PreferredWidth + 20);
            _textBox.Width = messagePanel.Width - selectButton.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Handles the event of the "Select" button being pressed, which brings
        /// up a FolderBrowserDialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectFolderButtonClickHandler(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (_folder != "") folderDialog.SelectedPath = _folder;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _textBox.Text = folderDialog.SelectedPath;
                _folder = folderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Gets and sets the folder name in the dialog
        /// </summary>
        public string FolderName
        {
            get { return _textBox.Text; }
            set
            {
                _folder = value;
                _textBox.Text = _folder;
            }
        }
    }
}