using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can choose a folder name from the file
    /// system
    /// </summary>
    public class InputBoxFolderSelect
    {
        private readonly string _message;
        protected TextBox _textBox;
        private string _folder;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputBoxFolderSelect(string message)
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
            Button itsSelectButton =
                ControlFactory.CreateButton("Select...", new EventHandler(SelectFolderButtonClickHandler));
            messagePanelManager.AddControl(itsSelectButton);
            messagePanel.Height = _textBox.Height + 40;
            messagePanel.Width = Math.Max(250, ControlFactory.CreateLabel(_message, false).PreferredWidth + 20);
            _textBox.Width = messagePanel.Width - itsSelectButton.Width - 30;
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