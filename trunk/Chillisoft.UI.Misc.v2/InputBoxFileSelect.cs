using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can choose a file name
    /// </summary>
    public class InputBoxFileSelect
    {
        private readonly string itsMessage;
        protected TextBox itsTextBox;
        private string itsFileName;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputBoxFileSelect(string message)
        {
            itsMessage = message;
            itsTextBox = ControlFactory.CreateTextBox();
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
            messagePanelManager.AddControl(ControlFactory.CreateLabel(itsMessage, false));
            messagePanelManager.AddControl(itsTextBox);
            messagePanelManager.AddGlue();
            messagePanelManager.AddControl(
                ControlFactory.CreateButton("Select...", new EventHandler(SelectFolderButtonClickHandler)));
            messagePanel.Height = itsTextBox.Height + 40;
            messagePanel.Width = ControlFactory.CreateLabel(itsMessage, false).PreferredWidth + 20;
            itsTextBox.Width = messagePanel.Width - 30;
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
            if (itsFileName != "") fileDialog.FileName = itsFileName;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                itsTextBox.Text = fileDialog.FileName;
                itsFileName = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Gets and sets the file name in the dialog
        /// </summary>
        public string FileName
        {
            get { return itsTextBox.Text; }
            set
            {
                itsFileName = value;
                itsTextBox.Text = itsFileName;
            }
        }
    }
}