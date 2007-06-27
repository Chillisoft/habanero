using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Ui.Base;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Gets a date from the user using a dialog box
    /// </summary>
    /// TODO ERIC - not sure what this does, the name is not intuitive
    public class MessageBoxDateProvider : IDateProvider
    {
        private readonly string _message;

        /// <summary>
        /// Constructor to initialise the provider with the message provided
        /// </summary>
        /// <param name="message">The message to display</param>
        public MessageBoxDateProvider(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Gets a date from the user, using the DateInputForm dialog
        /// </summary>
        /// <returns>Returns the date given by the user</returns>
        public DateTime GetDate()
        {
            DateInputForm frm = new DateInputForm(_message);
            frm.ShowDialog();
            return frm.DateTime;
        }

        /// <summary>
        /// A dialog to obtain a date from the user
        /// </summary>
        private class DateInputForm : Form
        {
            private DateTimePicker _dateTimePicker;

            /// <summary>
            /// Constructor to initialise the form with a message provided
            /// </summary>
            /// <param name="message">The message to display</param>
            public DateInputForm(string message)
            {
                _dateTimePicker = new DateTimePicker();
                Panel pnl = new Panel();
                GridLayoutManager manager = new GridLayoutManager(pnl);
                manager.SetGridSize(1, 2);
                manager.AddControl(ControlFactory.CreateLabel(message, false));
                manager.AddControl(_dateTimePicker);

                ButtonControl buttons = new ButtonControl();
                buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));
                this.Height = _dateTimePicker.Height + 10 + buttons.Height;

                BorderLayoutManager mainManager = new BorderLayoutManager(this);
                mainManager.AddControl(buttons, BorderLayoutManager.Position.South);
                mainManager.AddControl(pnl, BorderLayoutManager.Position.Centre);
            }

            /// <summary>
            /// A handler for the event of the OK button being pressed
            /// </summary>
            /// <param name="sender">The object that notified of the event</param>
            /// <param name="e">Attached arguments regarding the event</param>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                this.Close();
            }

            /// <summary>
            /// Returns the forms DateTime object
            /// </summary>
            public DateTime DateTime
            {
                get { return _dateTimePicker.Value; }
            }
        }
    }
}