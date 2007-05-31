using System;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can enter or edit a date range with
    /// a start and end date
    /// </summary>
    public class InputBoxDateRange
    {
        private readonly string itsMessage;
        private DateTimePicker itsStartDateTimePicker;
        private DateTimePicker itsEndDateTimePicker;
        private string itsTitle;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title</param>
        public InputBoxDateRange(string message, string title)
        {
            itsMessage = message;
            itsTitle = title;
            itsStartDateTimePicker = ControlFactory.CreateStandardDateTimePicker();
            itsEndDateTimePicker = ControlFactory.CreateStandardDateTimePicker();
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            Panel mainPanel = new Panel();
            BorderLayoutManager manager = new BorderLayoutManager(mainPanel);
            manager.AddControl(ControlFactory.CreateLabel(itsMessage, false), BorderLayoutManager.Position.North);

            Panel messagePanel = new Panel();
            GridLayoutManager messagePanelManager = new GridLayoutManager(messagePanel);
            messagePanelManager.SetGridSize(2, 2);
            messagePanelManager.FixAllRowsBasedOnContents();
            messagePanelManager.FixColumnBasedOnContents(0);
            messagePanelManager.AddControl(ControlFactory.CreateLabel("Start Date", false));
            messagePanelManager.AddControl(itsStartDateTimePicker);
            messagePanelManager.AddControl(ControlFactory.CreateLabel("End Date", false));
            messagePanelManager.AddControl(itsEndDateTimePicker);
            messagePanel.Height = messagePanelManager.GetFixedHeightIncludingGaps();
            messagePanel.Width = Math.Max(250, ControlFactory.CreateLabel(itsMessage, false).PreferredWidth + 10);
            mainPanel.Height = messagePanel.Height + ControlFactory.CreateLabel("Test", true).Height + 5;
            mainPanel.Width = messagePanel.Width;
            manager.AddControl(messagePanel, BorderLayoutManager.Position.Centre);
            return new OKCancelDialog(mainPanel, itsTitle).ShowDialog();
        }

        /// <summary>
        /// Gets and sets the start date in the date range
        /// </summary>
        public DateTime StartDate
        {
            get { return itsStartDateTimePicker.Value; }
            set { itsStartDateTimePicker.Value = value; }
        }

        /// <summary>
        /// Gets and sets the end date in the date range
        /// </summary>
        public DateTime EndDate
        {
            get { return itsEndDateTimePicker.Value; }
            set { itsEndDateTimePicker.Value = value; }
        }
    }
}