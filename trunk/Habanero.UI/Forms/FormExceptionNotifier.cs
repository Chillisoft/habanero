using System;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;

namespace Habanero.Ui.Forms
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
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            //new ExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
            new CollapsibleExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
        }

        /// <summary>
        /// Provides a form to display the exception message using two tabs,
        /// one for the basic message and one for the detailed error information
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
                //detailsTextBox.Text = ExceptionUtil.GetExceptionString(ex, 0);
                detailsTextBox.Text = ExceptionUtil.GetCategorizedExceptionString(ex, 0);
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

        /// <summary>
        /// Provides a form to display the exception message, using a "More Detail"
        /// button that collapses or uncollapses the error detail panel
        /// </summary>
        private class CollapsibleExceptionNotifyForm : Form
        {
            private Exception _exception;
            private Panel _summary;
            private Panel _fullDetail;
            private Button _moreDetailButton;
            private TextBox _errorDetails;
            private CheckBox _showStackTrace;
            private const int SUMMARY_HEIGHT = 150;
            private const int FULL_DETAIL_HEIGHT = 300;
            private const int BUTTONS_HEIGHT = 50;

            /// <summary>
            /// Constructor that sets up the error message form
            /// </summary>
            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                _exception = ex;

                _summary = new Panel();
                _summary.Text = title;
                _summary.Height = SUMMARY_HEIGHT;
                TextBox messageTextBox = GetSimpleMessage(ex.Message);
                Label messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager summaryManager = new BorderLayoutManager(_summary);
                summaryManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                summaryManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                ButtonControl buttonsOK = new ButtonControl();
                ButtonControl buttonsDetail = new ButtonControl();
                buttonsOK.AddButton("&OK", new EventHandler(OKButtonClickHandler));
                _moreDetailButton = buttonsDetail.AddButton("&More Detail »", new EventHandler(MoreDetailClickHandler));
                buttonsOK.Height = BUTTONS_HEIGHT;
                buttonsDetail.Height = BUTTONS_HEIGHT;
                buttonsDetail.Width = _moreDetailButton.Width + 9;

                SetFullDetailsPanel();

                BorderLayoutManager manager = new BorderLayoutManager(this);
                manager.AddControl(_summary, BorderLayoutManager.Position.North);
                manager.AddControl(buttonsDetail, BorderLayoutManager.Position.West);
                manager.AddControl(buttonsOK, BorderLayoutManager.Position.East);
                manager.AddControl(_fullDetail, BorderLayoutManager.Position.South);

                this.Text = title;
                this.Width = 600;
                this.Height = SUMMARY_HEIGHT + BUTTONS_HEIGHT + 16;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(50, 50);
                this.Resize += new EventHandler(ResizeForm);
            }

            /// <summary>
            /// Creates the red error label that appears at the top
            /// </summary>
            private static Label GetErrorLabel(string message)
            {
                Label messageLabel = ControlFactory.CreateLabel(" " + message, true);
                messageLabel.TextAlign = ContentAlignment.BottomLeft;
                messageLabel.BackColor = Color.Red;
                messageLabel.ForeColor = Color.White;
                messageLabel.Font = new Font(messageLabel.Font.FontFamily, 10);
                messageLabel.Height = 18;
                return messageLabel;
            }

            /// <summary>
            /// Creates the text box that shows the error summary at the top
            /// </summary>
            private static TextBox GetSimpleMessage(string message)
            {
                TextBox messageTextBox = ControlFactory.CreateTextBox();
                messageTextBox.Text = message;
                messageTextBox.Multiline = true;
                messageTextBox.ScrollBars = ScrollBars.Both;
                messageTextBox.ReadOnly = true;
                messageTextBox.Font = new Font(messageTextBox.Font.FontFamily, 10);
                return messageTextBox;
            }

            /// <summary>
            /// Sets up the panel that shows the error details
            /// </summary>
            private void SetFullDetailsPanel()
            {
                _fullDetail = new Panel();
                _fullDetail.Text = "Error Detail";
                _fullDetail.Height = FULL_DETAIL_HEIGHT;
                _fullDetail.Visible = false;
                _errorDetails = ControlFactory.CreateTextBox();
                _errorDetails.Text = ExceptionUtil.GetExceptionString(_exception, 0, false);
                _errorDetails.Multiline = true;
                _errorDetails.ScrollBars = ScrollBars.Both;
                _showStackTrace = new CheckBox();
                _showStackTrace.Text = "&Show stack trace";
                _showStackTrace.CheckedChanged += new EventHandler(ShowStackTraceClicked);
                BorderLayoutManager detailsManager = new BorderLayoutManager(_fullDetail);
                detailsManager.AddControl(_errorDetails, BorderLayoutManager.Position.Centre);
                detailsManager.AddControl(_showStackTrace, BorderLayoutManager.Position.South);
            }

            /// <summary>
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                this.Close();
            }

            /// <summary>
            /// Expands the form when the "More Details" button is clicked
            /// </summary>
            private void MoreDetailClickHandler(object sender, EventArgs e)
            {
                if (!_fullDetail.Visible)
                {
                    Height = _summary.Height + BUTTONS_HEIGHT + 16 + FULL_DETAIL_HEIGHT;
                    Width = 750;
                    _fullDetail.Visible = true;
                    _moreDetailButton.Text = "« &Less Detail";
                }
                else
                {
                    Height = _summary.Height + BUTTONS_HEIGHT + 16;
                    _fullDetail.Visible = false;
                    _moreDetailButton.Text = "&More Detail »";
                }
            }

            /// <summary>
            /// Toggles the showing of the stack trace in the error details
            /// </summary>
            private void ShowStackTraceClicked(object sender, EventArgs e)
            {
                _errorDetails.Text = ExceptionUtil.GetExceptionString(_exception, 0, _showStackTrace.Checked);
            }

            /// <summary>
            /// Scales the components when the form is resized
            /// </summary>
            private void ResizeForm(object sender, EventArgs e)
            {
                int sdHeight = Height - BUTTONS_HEIGHT - 16;
                if (sdHeight > SUMMARY_HEIGHT)
                {
                    sdHeight = SUMMARY_HEIGHT;
                }
                _summary.Height = sdHeight;
                int heightRemaining = Height - BUTTONS_HEIGHT - sdHeight - 16;
                if (heightRemaining > 0)
                {
                    _fullDetail.Height = heightRemaining;
                }
                else
                {
                    _fullDetail.Height = 0;
                }
            }
        }
    }
}