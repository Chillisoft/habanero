using System;
using System.Drawing;
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
            private Panel _fullDetail;
            private Button _moreDetailButton;

            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                Panel simpleDetail = new Panel();
                simpleDetail.Text = title;
                simpleDetail.Height = 150;
                TextBox messageTextBox = GetSimpleMessage(ex.Message);
                Label messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager messageTabPageManager = new BorderLayoutManager(simpleDetail);
                messageTabPageManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                messageTabPageManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                ButtonControl buttonsOK = new ButtonControl();
                ButtonControl buttonsDetail = new ButtonControl();
                buttonsOK.AddButton("&OK", new EventHandler(OKButtonClickHandler));
                _moreDetailButton = buttonsDetail.AddButton("&More Detail »", new EventHandler(MoreDetailClickHandler));
                buttonsOK.Height = 50;
                buttonsDetail.Height = 50;
                buttonsDetail.Width = _moreDetailButton.Width + 9;

                _fullDetail = new Panel();
                _fullDetail.Text = "Error Detail";
                _fullDetail.Height = 400;
                _fullDetail.Visible = false;
                TextBox detailsTextBox = ControlFactory.CreateTextBox();
                //detailsTextBox.Text = ExceptionUtil.GetExceptionString(ex, 0);
                detailsTextBox.Text = ExceptionUtil.GetCategorizedExceptionString(ex, 0);
                detailsTextBox.Multiline = true;
                detailsTextBox.ScrollBars = ScrollBars.Both;
                BorderLayoutManager detailsTabPageManager = new BorderLayoutManager(_fullDetail);
                detailsTabPageManager.AddControl(detailsTextBox, BorderLayoutManager.Position.Centre);

                BorderLayoutManager manager = new BorderLayoutManager(this);
                manager.AddControl(simpleDetail, BorderLayoutManager.Position.North);
                manager.AddControl(buttonsDetail, BorderLayoutManager.Position.West);
                manager.AddControl(buttonsOK, BorderLayoutManager.Position.East);
                manager.AddControl(_fullDetail, BorderLayoutManager.Position.South);

                this.Text = title;
                this.Width = 600;
                this.Height = 216;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(50, 50);
                
            }

            private static Label GetErrorLabel(string message)
            {
                Label messageLabel = ControlFactory.CreateLabel(" " + message, true);
                messageLabel.TextAlign = ContentAlignment.BottomLeft;
                //messageLabel.BorderStyle = BorderStyle.FixedSingle;
                messageLabel.BackColor = Color.Red;
                messageLabel.ForeColor = Color.White;
                messageLabel.Font = new Font(messageLabel.Font.FontFamily, 10);
                messageLabel.Height = 18;
                return messageLabel;
            }

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
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            /// <param name="sender">The object that notified of the event</param>
            /// <param name="e">Attached arguments regarding the event</param>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                this.Close();
            }

            /// <summary>
            /// Handles the event of the More Detail button being pressed on the
            /// exception form, which expands the form
            /// </summary>
            /// <param name="sender">The object that notified of the event</param>
            /// <param name="e">Attached arguments regarding the event</param>
            private void MoreDetailClickHandler(object sender, EventArgs e)
            {
                if (!_fullDetail.Visible)
                {
                    this.Height = 616;
                    this.Width = 750;
                    _fullDetail.Visible = true;
                    _moreDetailButton.Text = "« &Less Detail";
                }
                else
                {
                    this.Height = 216;
                    _fullDetail.Visible = false;
                    _moreDetailButton.Text = "&More Detail »";
                }
            }
        }
    }
}