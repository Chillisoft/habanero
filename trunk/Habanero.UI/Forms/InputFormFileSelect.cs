using System;
using System.Windows.Forms;
using Habanero.Ui.Base;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Provides a form in which a user can choose a file name
    /// </summary>
    public class InputFormFileSelect
    {
        private readonly string _message;
        protected TextBox _textBox;
        private string _fileName;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputFormFileSelect(string message)
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
            messagePanelManager.AddControl(
                ControlFactory.CreateButton("Select...", new EventHandler(SelectFolderButtonClickHandler)));
            messagePanel.Height = _textBox.Height + 40;
            messagePanel.Width = ControlFactory.CreateLabel(_message, false).PreferredWidth + 20;
            _textBox.Width = messagePanel.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Handles the event of the "Select" button being pressed, which
        /// brings up a new OpenFileDialog
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectFolderButtonClickHandler(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (_fileName != "") fileDialog.FileName = _fileName;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                _textBox.Text = fileDialog.FileName;
                _fileName = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Gets and sets the file name in the dialog
        /// </summary>
        public string FileName
        {
            get { return _textBox.Text; }
            set
            {
                _fileName = value;
                _textBox.Text = _fileName;
            }
        }
    }
}