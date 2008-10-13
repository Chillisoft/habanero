//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using Habanero.Util;
using DialogResult=System.Windows.Forms.DialogResult;
using FormStartPosition=System.Windows.Forms.FormStartPosition;
using MessageBoxButtons=System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon=System.Windows.Forms.MessageBoxIcon;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a form that displays an exception to the user
    /// </summary>
    public class FormExceptionNotifier : IExceptionNotifier
    {
        private static readonly IControlFactory _controlFactory = GlobalUIRegistry.ControlFactory;
        /// <summary>
        /// Displays a dialog with exception information to the user
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Additional error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            //new ExceptionNotifyForm(ex, furtherMessage, title).ShowDialog();
            if (ex is UserException)
            {
                string message = ex.Message;
                if (!String.IsNullOrEmpty(furtherMessage))
                {
                    furtherMessage = furtherMessage.TrimEnd('.', ':');
                    message = furtherMessage + ":" + Environment.NewLine + message;
                }
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CollapsibleExceptionNotifyForm form = new CollapsibleExceptionNotifyForm(ex, furtherMessage, title);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Provides a form to display the exception message, using a "More Detail"
        /// button that collapses or uncollapses the error detail panel
        /// </summary>
        private class CollapsibleExceptionNotifyForm : FormWin
        {
            private readonly Exception _exception;
            private readonly IPanel _summary;
            private IPanel _fullDetail;
            private readonly IButton _moreDetailButton;
            private ITextBox _errorDetails;
            private ICheckBox _showStackTrace;
            private const int SUMMARY_HEIGHT = 150;
            private const int FULL_DETAIL_HEIGHT = 300;
            private const int BUTTONS_HEIGHT = 50;

            /// <summary>
            /// Constructor that sets up the error message form
            /// </summary>
            public CollapsibleExceptionNotifyForm(Exception ex, string furtherMessage, string title)
            {
                _exception = ex;

                _summary = _controlFactory.CreatePanel();
                _summary.Text = title;
                _summary.Height = SUMMARY_HEIGHT;
                ITextBox messageTextBox = GetSimpleMessage(ex.Message);
                ILabel messageLabel = GetErrorLabel(furtherMessage);
                BorderLayoutManager summaryManager = _controlFactory.CreateBorderLayoutManager(_summary);
                summaryManager.AddControl(messageLabel, BorderLayoutManager.Position.North);
                summaryManager.AddControl(messageTextBox, BorderLayoutManager.Position.Centre);

                IButtonGroupControl buttonsOK = _controlFactory.CreateButtonGroupControl();
                //IButtonGroupControl buttonsDetail = _controlFactory.CreateButtonGroupControl();
                buttonsOK.AddButton("&OK", new EventHandler(OKButtonClickHandler));

                //_moreDetailButton = buttonsDetail.AddButton("&More Detail »", new EventHandler(MoreDetailClickHandler));
                //buttonsOK.Height = BUTTONS_HEIGHT;
                //buttonsDetail.Height = BUTTONS_HEIGHT;
                //buttonsDetail.Width = _moreDetailButton.Width + 9;

                IButtonGroupControl buttonsDetail = _controlFactory.CreateButtonGroupControl();
                buttonsDetail.AddButton("Email Error", EmailErrorClickHandler);
                _moreDetailButton = buttonsDetail.AddButton("More Detail »", MoreDetailClickHandler);
                buttonsDetail.Height = BUTTONS_HEIGHT;
                buttonsDetail.Width = 2 * (_moreDetailButton.Width + 9);

                SetFullDetailsPanel();

                BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(this);
                manager.AddControl(_summary, BorderLayoutManager.Position.North);
                manager.AddControl(buttonsDetail, BorderLayoutManager.Position.West);
                manager.AddControl(buttonsOK, BorderLayoutManager.Position.East);
                manager.AddControl(_fullDetail, BorderLayoutManager.Position.South);

                this.Text = title;
                this.Width = 600;
                this.Height = SUMMARY_HEIGHT + BUTTONS_HEIGHT + 16;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(50, 50);
                this.Resize += ResizeForm;
            }

            private void EmailErrorClickHandler(object sender, EventArgs e)
            {
                try
                {
                    string userDescription = "";
                    ErrorDescriptionForm errorDescriptionForm = new ErrorDescriptionForm();
                    errorDescriptionForm.Closing +=
                        delegate { userDescription = errorDescriptionForm.ErrorDescriptionTextBox.Text; };
                    errorDescriptionForm.ShowDialog(this);

                    IDictionary dictionary = GetEmailErrorSettings();
                    string exceptionString = ExceptionUtilities.GetExceptionString(_exception, 0, true);
                    if (!string.IsNullOrEmpty(userDescription))
                    {
                        exceptionString = "User Description : " + Environment.NewLine + userDescription +
                                          Environment.NewLine + "  -  Exception : " + exceptionString;
                    }

                    if (dictionary != null)
                    {
                        try
                        {
                            SendErrorMessage(dictionary, exceptionString);
                            return;
                        }
                        catch (Exception ex)
                        {
                            exceptionString += Environment.NewLine + "  -  Error sending mail via smtp: " +
                                               Environment.NewLine + ex.Message;
                        }
                    }
                    System.Diagnostics.Process.Start("mailto:?subject=" + _exception.Source + "&body=" + exceptionString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The error message was not sent due to the following error : " + Environment.NewLine +
                                    ex.Message);
                }
            }

            private void SendErrorMessage(IDictionary dictionary, string emailContent)
            {
                string smtpServer = (string)dictionary["smtp_server"];
                string emailTo = (string)dictionary["email_to"];
                string[] emailAddresses = emailTo.Split(new char[]{';'});

                string emailFrom = (string)dictionary["email_from"];

                //string emailContent = ExceptionUtilities.GetExceptionString(_exception, 0, true);

                EmailSender emailSender = new EmailSender(emailAddresses, emailFrom, _exception.Source, emailContent, "");
                ////Todo : check Send Authenticated for security purposes?
                
                emailSender.SmtpServerHost = smtpServer;
                string port = (string)dictionary["smtp_port"];
                if (!String.IsNullOrEmpty(port))
                {
                    emailSender.SmtpServerPort = Convert.ToInt32(port);
                }
                bool enableSSL = Convert.ToBoolean(dictionary["smtp_enable_ssl"]); 
                emailSender.EnableSSL = enableSSL;
                emailSender.Send();
            }

            /// <summary>
            /// Creates the red error label that appears at the top
            /// </summary>
            private static ILabel GetErrorLabel(string message)
            {
                ILabel messageLabel = _controlFactory.CreateLabel(" " + message, true);
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
                ITextBox messageTextBox = _controlFactory.CreateTextBox();
                messageTextBox.Text = message;
                messageTextBox.Multiline = true;
                //messageTextBox.ScrollBars = ScrollBars.Both;
                //messageTextBox.ReadOnly = true;
                messageTextBox.Font = new Font(messageTextBox.Font.FontFamily, 10);
                return messageTextBox;
            }

            /// <summary>
            /// Sets up the panel that shows the error details
            /// </summary>
            private void SetFullDetailsPanel()
            {
                _fullDetail = _controlFactory.CreatePanel();
                _fullDetail.Text = "Error Detail";
                _fullDetail.Height = FULL_DETAIL_HEIGHT;
                _fullDetail.Visible = false;
                _errorDetails = _controlFactory.CreateTextBox();
                _errorDetails.Text = ExceptionUtilities.GetExceptionString(_exception, 0, false);
                _errorDetails.Multiline = true;
//                _errorDetails.ScrollBars = ScrollBars.Both;
                _showStackTrace = _controlFactory.CreateCheckBox();
                _showStackTrace.Text = "&Show stack trace";
                _showStackTrace.CheckedChanged += new EventHandler(ShowStackTraceClicked);//TODO: Fix
                BorderLayoutManager detailsManager = _controlFactory.CreateBorderLayoutManager(_fullDetail);
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

            private  IDictionary GetEmailErrorSettings()
            {
                IDictionary dictionary = ((IDictionary)ConfigurationSettings.GetConfig("EmailErrorConfig"));
                return dictionary;
            }
        }
    }
}