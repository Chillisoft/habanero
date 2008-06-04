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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Provides an email sender
    /// </summary>
    public class EmailSender
    {
        private readonly string _fromAddress;
        //private readonly string _attachmentPath;
        private readonly string _content;
        private readonly string _subject;
        private readonly List<string> _toAddresses = new List<string>(1);
        private readonly List<string> _ccAddresses = new List<string>(1);
        private readonly List<string> _bccAddresses = new List<string>(1);
        private readonly List<string> _attachmentPaths = new List<string>(1);
        private string _smtpServerHost;
        private int _smtpServerPort;
        private bool _enableSSL;

        #region Constructors

        /// <summary>
        /// Constructor to initialise a new sender
        /// </summary>
        /// <param name="emailAddresses">The email addresses to send to</param>
        /// <param name="fromAddress">The "from" address</param>
        /// <param name="subject">The email subject</param>
        /// <param name="content">The email content</param>
        /// <param name="attachmentPath">The attachment path</param>
        public EmailSender(IList<string> emailAddresses, string fromAddress, string subject, string content,
                           string attachmentPath)
            : this(emailAddresses, fromAddress, subject, content, CreateListWithItem(attachmentPath) )
        {
        }
               

        /// <summary>
        /// Constructor to initialise a new sender
        /// </summary>
        /// <param name="emailAddresses">The email addresses to send to</param>
        /// <param name="fromAddress">The "from" address</param>
        /// <param name="subject">The email subject</param>
        /// <param name="content">The email content</param>
        /// <param name="attachmentPaths">The attachment paths</param>
        public EmailSender(IList<string> emailAddresses, string fromAddress, string subject, string content,
                           IList<string> attachmentPaths)
            : this(emailAddresses, new List<string>(), new List<string>(), fromAddress, 
                    subject, content, attachmentPaths)
        {
        }

        /// <summary>
        /// Constructor to initialise a new sender
        /// </summary>
        /// <param name="toAddresses">The email addresses to send to</param>
        /// <param name="ccAddresses">The email addresses to carbon copy</param>
        /// <param name="bccAddresses">The email addresses to blind carbon copy</param>
        /// <param name="fromAddress">The "from" address</param>
        /// <param name="subject">The email subject</param>
        /// <param name="content">The email content</param>
        /// <param name="attachmentPaths">The attachment paths</param>
        public EmailSender(IList<string> toAddresses, IList<string> ccAddresses, IList<string> bccAddresses, 
            string fromAddress, string subject, string content, IList<string> attachmentPaths)
        {
            _toAddresses = new List<string>(toAddresses);
            _ccAddresses = new List<string>(ccAddresses);
            _bccAddresses = new List<string>(bccAddresses);
            _subject = subject;
            _content = content;
            _attachmentPaths = new List<string>(attachmentPaths);
            //_attachmentPath = attachmentPath;
            
            _fromAddress = fromAddress;
            if (GlobalRegistry.Settings != null)
            {
                try
                {
                    _smtpServerHost = GlobalRegistry.Settings.GetString("SMTP_SERVER");
                } catch
                {
                }
            }
            _smtpServerPort = 25;
            _enableSSL = false;
        }


        private static List<string> CreateListWithItem(string item)
        {
            List<string> list = new List<string>();
            if (!String.IsNullOrEmpty(item))
            {
                list.Add(item);

            }
            return list;
        }

        #endregion //Constructors

        #region Properties

        ///<summary>
        /// Gets the List of Email Addresses used for SMTP Transactions.
        ///</summary>
        public List<string> ToAddresses
        {
            get { return _toAddresses; }
        }
        ///<summary>
        /// Gets the List of Subject associated with the Email.
        ///</summary>
        public string Subject
        {
            get { return _subject; }
        }

        ///<summary>
        /// Gets the File Attachment Path used for SMTP Transactions.
        ///</summary>
        public List<string> Attachment
        {
            get
            {
                return _attachmentPaths;
            }
        }

        ///<summary>
        /// Gets or sets the name or IP address of the host used for SMTP transactions.
        ///</summary>
        public string SmtpServerHost
        {
            get { return _smtpServerHost; }
            set { _smtpServerHost = value; }
        }

        ///<summary>
        /// Gets or sets the port used for SMTP transactions.
        ///</summary>
        public int SmtpServerPort
        {
            get { return _smtpServerPort; }
            set { _smtpServerPort = value; }
        }

        ///<summary>
        /// Gets or sets whether the SMTP server uses SSL or not.
        ///</summary>
        public bool EnableSSL
        {
            get { return _enableSSL; }
            set { _enableSSL = value; }
        }

        #endregion //Properties

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration
        /// </summary>
        public void Send()
        {
            DoSend(null, null, null);
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration and authenticates using the provided username and password
        /// </summary>
        /// <param name="username">The username used for authentification</param>
        /// <param name="password">The password used for authentification</param>
        public void SendAuthenticated(string username, string password)
        {
            DoSend(username, password, null);
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration and authenticates using the provided username and password
        /// </summary>
        /// <param name="username">The username used for authentification</param>
        /// <param name="password">The password used for authentification</param>
        /// <param name="domain">The domain used for authentification</param>
        public void SendAuthenticated(string username, string password, string domain)
        {
            DoSend(username, password, domain);
        }

        private void DoSend(string authUsername, string authPassword, string authDomain)
        {
            if (_smtpServerHost == null)
            {
                throw new Exception("Please specify the SMTP Host Name before attempting to send.");
            }
            if (_toAddresses.Count == 0)
            {
                throw new Exception("No Email Addresses were provoided");
            }
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_fromAddress);
            addAddresses(message.To, _toAddresses);
            addAddresses(message.CC, _ccAddresses);
            addAddresses(message.Bcc, _bccAddresses);
            message.Subject = _subject;
            message.Body = _content;
            AttachmentCollection attachments = message.Attachments;
            foreach (string attachmentPath in _attachmentPaths)
            {
                attachments.Add(new Attachment(attachmentPath));
            }
            SmtpClient smtpServer = new SmtpClient();
            smtpServer.Host = _smtpServerHost;
            smtpServer.Port = _smtpServerPort;
            smtpServer.EnableSsl = _enableSSL;
            if (authUsername != null && authPassword != null)
            {
                NetworkCredential auth = new NetworkCredential(authUsername, authPassword);
                if (authDomain != null)
                {
                    auth.Domain = authDomain;
                }
                smtpServer.Credentials = auth;
            }
            else
            {
                smtpServer.UseDefaultCredentials = true;
            }
            smtpServer.Send(message);
        }

        private static void addAddresses(MailAddressCollection addToCol, List<string> addressList)
        {
            foreach (string address in addressList)
            {
                if (address != null)
                {
                    addToCol.Add(new MailAddress(address.ToString()));
                }
            }
        }
    }
}