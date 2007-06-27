using System;
using System.Windows.Forms;
using Habanero.Ui.Base;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Provides a form in which a user can enter or edit a date
    /// </summary>
    public class InputFormDate
    {
        private readonly string _message;
        protected DateTimePicker _dateTimePicker;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputFormDate(string message)
        {
            _message = message;
            _dateTimePicker = ControlFactory.CreateStandardDateTimePicker();
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
            messagePanelManager.AddControl(_dateTimePicker);
            messagePanel.Height = _dateTimePicker.Height*2 + 20;
            messagePanel.Width = ControlFactory.CreateLabel(_message, false).PreferredWidth + 20;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the date-time value held in the date-time picker
        /// on the form
        /// </summary>
        public DateTime Value
        {
            get { return _dateTimePicker.Value; }
            set { _dateTimePicker.Value = value; }
        }
    }
}