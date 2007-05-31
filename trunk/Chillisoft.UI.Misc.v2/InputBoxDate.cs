using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can enter or edit a date
    /// </summary>
    public class InputBoxDate
    {
        private readonly string itsMessage;
        protected DateTimePicker itsDateTimePicker;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public InputBoxDate(string message)
        {
            itsMessage = message;
            itsDateTimePicker = ControlFactory.CreateStandardDateTimePicker();
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
            messagePanelManager.AddControl(itsDateTimePicker);
            messagePanel.Height = itsDateTimePicker.Height*2 + 20;
            messagePanel.Width = ControlFactory.CreateLabel(itsMessage, false).PreferredWidth + 20;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the date-time value held in the date-time picker
        /// on the form
        /// </summary>
        public DateTime Value
        {
            get { return itsDateTimePicker.Value; }
            set { itsDateTimePicker.Value = value; }
        }
    }
}