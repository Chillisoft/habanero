using System;
using System.Windows.Forms;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Gets a date from the user using a dialog box
    /// </summary>
    /// TODO ERIC - not sure what this does, the name is not intuitive
    public class MessageBoxDateProvider : IDateProvider
    {
        private readonly string itsMessage;

        /// <summary>
        /// Constructor to initialise the provider with the message provided
        /// </summary>
        /// <param name="message">The message to display</param>
        public MessageBoxDateProvider(string message)
        {
            itsMessage = message;
        }

        /// <summary>
        /// Gets a date from the user, using the DateInputForm dialog
        /// </summary>
        /// <returns>Returns the date given by the user</returns>
        public DateTime GetDate()
        {
            DateInputForm frm = new DateInputForm(itsMessage);
            frm.ShowDialog();
            return frm.DateTime;
        }

        /// <summary>
        /// A dialog to obtain a date from the user
        /// </summary>
        private class DateInputForm : Form
        {
            private DateTimePicker itsDateTimePicker;

            /// <summary>
            /// Constructor to initialise the form with a message provided
            /// </summary>
            /// <param name="message">The message to display</param>
            public DateInputForm(string message)
            {
                itsDateTimePicker = new DateTimePicker();
                Panel pnl = new Panel();
                GridLayoutManager manager = new GridLayoutManager(pnl);
                manager.SetGridSize(1, 2);
                manager.AddControl(ControlFactory.CreateLabel(message, false));
                manager.AddControl(itsDateTimePicker);

                ButtonControl buttons = new ButtonControl();
                buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));
                this.Height = itsDateTimePicker.Height + 10 + buttons.Height;

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
                get { return itsDateTimePicker.Value; }
            }
        }
    }
}