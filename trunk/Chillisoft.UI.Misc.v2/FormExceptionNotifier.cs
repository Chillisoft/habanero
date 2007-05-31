using System;
using System.Windows.Forms;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form that displays an exception to the user
    /// </summary>
    public class FormExceptionNotifier : IExceptionNotifier
    {
        /// <summary>
        /// Displays a dialog with exception information to the user
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Additional error messages</param>
        /// <param name="title">The title</param>
        public void Notify(BaseApplicationException ex, string furtherMessage, string title)
        {
            new ExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
        }

        /// <summary>
        /// Provides a form to display the exception message
        /// </summary>
        private class ExceptionNotifyForm : Form
        {
            /// <summary>
            /// Constructor to initialise the form
            /// </summary>
            /// <param name="ex">The exception</param>
            /// <param name="furtherMessage">Extra error messages</param>
            /// <param name="title">The title</param>
            public ExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                TabControl tabControl = new TabControl();
                ButtonControl buttons = new ButtonControl();
                buttons.AddButton("OK", new EventHandler(OKButtonClickHandler));

                BorderLayoutManager manager = new BorderLayoutManager(this);
                manager.AddControl(tabControl, BorderLayoutManager.Position.Centre);
                manager.AddControl(buttons, BorderLayoutManager.Position.South);

                TabPage messageTabPage = ControlFactory.CreateTabPage("Message");
                Label messageLabel = ControlFactory.CreateLabel(furtherMessage, false);
                TextBox messageTextBox = ControlFactory.CreateTextBox();
                messageTextBox.Text = ex.Message;
                messageTextBox.Multiline = true;
                messageTextBox.ScrollBars = ScrollBars.Both;
                BorderLayoutManager messageTabPageManager = new BorderLayoutManager(messageTabPage);
                messageTabPageManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                messageTabPageManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                TabPage detailsTabPage = ControlFactory.CreateTabPage("Details");
                Label detailsLabel = ControlFactory.CreateLabel("Below are further details for the error.", false);
                TextBox detailsTextBox = ControlFactory.CreateTextBox();
                detailsTextBox.Text = ExceptionUtil.GetExceptionString(ex, 0);
                detailsTextBox.Multiline = true;
                detailsTextBox.ScrollBars = ScrollBars.Both;
                BorderLayoutManager detailsTabPageManager = new BorderLayoutManager(detailsTabPage);
                detailsTabPageManager.AddControl(detailsLabel, BorderLayoutManager.Position.North);
                detailsTabPageManager.AddControl(detailsTextBox, BorderLayoutManager.Position.Centre);

                tabControl.TabPages.Add(messageTabPage);
                tabControl.TabPages.Add(detailsTabPage);

                this.Text = title;
            }

            /// <summary>
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            /// <param name="sender">The object that notified of the event</param>
            /// <param name="e">Attached arguments regarding the event</param>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                this.Close();
            }
        }
    }
}