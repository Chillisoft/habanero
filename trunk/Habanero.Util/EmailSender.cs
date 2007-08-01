using System.Collections;
using System.Net;
using Habanero.Base;
using System.Net.Mail;

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
        private readonly IList _toAddresses = new ArrayList(1);
        private readonly IList _ccAddresses = new ArrayList(1);
        private readonly IList _bccAddresses = new ArrayList(1);
        private readonly IList _attachmentPaths = new ArrayList(1);
        private string _smtpServerHost;
        private int _smtpServerPort;

        /// <summary>
        /// Constructor to initialise a new sender
        /// </summary>
        /// <param name="emailAddresses">The email addresses to send to</param>
        /// <param name="fromAddress">The "from" address</param>
        /// <param name="subject">The email subject</param>
        /// <param name="content">The email content</param>
        /// <param name="attachmentPath">The attachment path</param>
        /// TODO ERIC - cater for multiple attachments using a list
        public EmailSender(IList emailAddresses, string fromAddress, string subject, string content,
                           string attachmentPath)
        {
            _toAddresses = emailAddresses;
            _subject = subject;
            _content = content;
            //_attachmentPath = attachmentPath;
            if (attachmentPath != null && attachmentPath.Length > 0)
                _attachmentPaths.Add(attachmentPath);
            _fromAddress = fromAddress;
            if (GlobalRegistry.Settings != null)
                _smtpServerHost = GlobalRegistry.Settings.GetString("SmtpServer");
            _smtpServerPort = 25;
        }

        ///<summary>
        /// Gets or sets the name or IP address of the host used for SMTP transactions.
        ///</summary>
        public string SmtpServerHost
        {
            get { return _smtpServerHost; }
            set{ _smtpServerHost = value;}
        }

        ///<summary>
        /// Gets or sets the port used for SMTP transactions.
        ///</summary>
        public int SmtpServerPort
        {
            get { return _smtpServerPort; }
            set { _smtpServerPort = value; }
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration
        /// </summary>
        public void Send()
        {
            DoSend(null, null,null);
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration and authenticates using the provided username and password
        /// </summary>
        /// <param name="username">The username used for authentification</param>
        /// <param name="password">The password used for authentification</param>
        public void SendAuthenticated(string username, string password)
        {
            DoSend(username, password,null);
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
            //EmailMessage message =
            //    new EmailMessage((string) itsEmailAddresses[0], _fromAddress, _subject, _content,
            //                     BodyPartFormat.Plain);
            //for (int i = 1; i < itsEmailAddresses.Count; i++)
            //{
            //    message.Recipients.Add((string) itsEmailAddresses[i]);
            //}
            //if (_attachmentPath != null && _attachmentPath.Length > 0)
            //{
            //    message.Attachments.Add(_attachmentPath);
            //}
            //SMTP server = new SMTP(GlobalRegistry.Settings.GetString("SmtpServer"));
            //server.Send(message);

            if (_smtpServerHost == null)
            {
                throw new System.Exception("Please specify the SMTP Host Name before attempting to send.");
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
            if (authUsername != null && authPassword != null)
            {
                NetworkCredential auth = new NetworkCredential(authUsername,authPassword);
                if (authDomain != null)
                {
                    auth.Domain = authDomain;
                }
                smtpServer.Credentials = auth;
            } else
            {
                smtpServer.UseDefaultCredentials = true;
            }
            smtpServer.Send(message);
        }

        private static void addAddresses(MailAddressCollection addToCol, IList addressList)
        {
            foreach (object toAddress in addressList)
            {
                addToCol.Add(new MailAddress(toAddress.ToString()));
            }
        }
    }
}