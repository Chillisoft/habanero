//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------
#pragma warning disable DoNotCallOverridableMethodsInConstructor
using System;
using System.Drawing;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
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
            new CollapsibleExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
        }

        /// <summary>
        /// Provides a form to display the exception message, using a "More Detail"
        /// button that collapses or uncollapses the error detail panel
        /// </summary>
        private class CollapsibleExceptionNotifyForm : Form, IControlChilli
        {
            private Exception _exception;
            private PanelGiz _summary;
            private PanelGiz _fullDetail;
            private IButton _moreDetailButton;
            private TextBoxGiz _errorDetails;
            private CheckBoxGiz _showStackTrace;
            private const int SUMMARY_HEIGHT = 150;
            private const int FULL_DETAIL_HEIGHT = 300;
            private const int BUTTONS_HEIGHT = 50;

            /// <summary>
            /// Constructor that sets up the error message form
            /// </summary>
            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                _exception = ex;

                _summary = new PanelGiz();
                _summary.Text = title;
                _summary.Height = SUMMARY_HEIGHT;
                ITextBox messageTextBox = GetSimpleMessage(ex.Message);
                ILabel messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager summaryManager = new BorderLayoutManagerGiz(_summary, GlobalUIRegistry.ControlFactory);
                summaryManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                summaryManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                IButtonGroupControl buttonsOK = new ButtonGroupControlGiz(GlobalUIRegistry.ControlFactory);
                IButtonGroupControl buttonsDetail = new ButtonGroupControlGiz(GlobalUIRegistry.ControlFactory);
                buttonsOK.AddButton("&OK", OKButtonClickHandler);
                _moreDetailButton = buttonsDetail.AddButton("More Detail...", MoreDetailClickHandler);
                buttonsOK.Height = BUTTONS_HEIGHT;
                buttonsDetail.Height = BUTTONS_HEIGHT;
                buttonsDetail.Width = _moreDetailButton.Width + 9;

                SetFullDetailsPanel();

                BorderLayoutManager manager = new BorderLayoutManagerGiz(this, GlobalUIRegistry.ControlFactory);
                manager.AddControl(_summary, BorderLayoutManager.Position.North);
                manager.AddControl(buttonsDetail, BorderLayoutManager.Position.West);
                manager.AddControl(buttonsOK, BorderLayoutManager.Position.East);
                manager.AddControl(_fullDetail, BorderLayoutManager.Position.South);


                Text = title;
                Width = 600;
                Height = SUMMARY_HEIGHT + BUTTONS_HEIGHT + 16;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(50, 50);
                Resize += ResizeForm;
            }

            /// <summary>
            /// Creates the red error label that appears at the top
            /// </summary>
            private static ILabel GetErrorLabel(string message)
            {
                ILabel messageLabel = GlobalUIRegistry.ControlFactory.CreateLabel(" " + message, true);
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
            private static ITextBox GetSimpleMessage(string message)
            {
                TextBoxGiz messageTextBox = new TextBoxGiz();
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
                _fullDetail = new PanelGiz();
                _fullDetail.Text = "Error Detail";
                _fullDetail.Height = FULL_DETAIL_HEIGHT;
                _fullDetail.Visible = false;
                _errorDetails = new TextBoxGiz();
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, false);
                _errorDetails.Multiline = true;
                _errorDetails.ScrollBars = ScrollBars.Both;
                _showStackTrace = new CheckBoxGiz();
                _showStackTrace.Text = "&Show stack trace";
                _showStackTrace.CheckedChanged += ShowStackTraceClicked;
                BorderLayoutManager detailsManager = new BorderLayoutManagerGiz(_fullDetail, GlobalUIRegistry.ControlFactory);
                detailsManager.AddControl(_errorDetails, BorderLayoutManager.Position.Centre);
                detailsManager.AddControl(_showStackTrace, BorderLayoutManager.Position.South);
            }

            /// <summary>
            /// Handles the event of the OK button being pressed on the
            /// exception form, which closes the form
            /// </summary>
            private void OKButtonClickHandler(object sender, EventArgs e)
            {
                Close();
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
                    _moreDetailButton.Text = "� &Less Detail";
                }
                else
                {
                    Height = _summary.Height + BUTTONS_HEIGHT + 16;
                    _fullDetail.Visible = false;
                    _moreDetailButton.Text = "&More Detail �";
                }
            }

            /// <summary>
            /// Toggles the showing of the stack trace in the error details
            /// </summary>
            private void ShowStackTraceClicked(object sender, EventArgs e)
            {
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, _showStackTrace.Checked);
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


            IControlCollection IControlChilli.Controls
            {
                get { return new ControlCollectionGiz(base.Controls); }
            }

            
        }
    }
}