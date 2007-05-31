using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can enter text into an input box,
    /// including the option of entering a masked password
    /// </summary>
    public class InputBoxTextBox
    {
        private readonly string itsMessage;
        protected TextBox itsTextBox;
        private bool itsIsPasswordField = false;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numLines">The number of lines to make available
        /// in the text box (its height in lines)</param>
        public InputBoxTextBox(string message, int numLines)
        {
            itsMessage = message;
            itsTextBox = ControlFactory.CreateTextBox();
            if (numLines > 1)
            {
                itsTextBox.Multiline = true;
                itsTextBox.Height = itsTextBox.Height*numLines;
                itsTextBox.ScrollBars = ScrollBars.Vertical;
            }
        }

        /// <summary>
        /// Gets and sets whether the input field is a password field, in
        /// which case the characters typed in by the user would be masked
        /// with an asterisk
        /// </summary>
        public bool IsPasswordField
        {
            get { return itsIsPasswordField; }
            set
            {
                itsIsPasswordField = value;
                if (value)
                {
                    itsTextBox.PasswordChar = '*';
                }
                else
                {
                    itsTextBox.PasswordChar = (char) 0;
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
            messagePanelManager.AddControl(ControlFactory.CreateLabel(itsMessage, false));
            messagePanelManager.AddControl(itsTextBox);
            messagePanel.Height = itsTextBox.Height + 40;
            messagePanel.Width = ControlFactory.CreateLabel(itsMessage, true).PreferredWidth + 20;
            itsTextBox.Width = messagePanel.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the text displayed in the text box
        /// </summary>
        public string Text
        {
            get { return itsTextBox.Text; }
            set { itsTextBox.Text = value; }
        }
    }
}