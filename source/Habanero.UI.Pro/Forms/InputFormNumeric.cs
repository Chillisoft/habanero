using System.Windows.Forms;
using Habanero.UI.Base;

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
            Permission.Check(this);
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