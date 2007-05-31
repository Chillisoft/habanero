using System.Collections;
using System.Net;
using Chillisoft.Generic.v2;
using System.Net.Mail;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Provides an email sender
    /// </summary>
    public class EmailSender
    {
        private readonly string itsFromAddress;
        //private readonly string itsAttachmentPath;
        private readonly string itsContent;
        private readonly string itsSubject;
        private readonly IList itsToAddresses = new ArrayList(1);
        private readonly IList itsCcAddresses = new ArrayList(1);
        private readonly IList itsBccAddresses = new ArrayList(1);
        private readonly IList itsAttachmentPaths = new ArrayList(1);
        private string itsSmtpServerHost;
        private int itsSmtpServerPort;

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
            itsToAddresses = emailAddresses;
            itsSubject = subject;
            itsContent = content;
            //itsAttachmentPath = attachmentPath;
            if (attachmentPath != null && attachmentPath.Length > 0)
                itsAttachmentPaths.Add(attachmentPath);
            itsFromAddress = fromAddress;
            if (GlobalRegistry.SettingsStorer != null)
                itsSmtpServerHost = GlobalRegistry.SettingsStorer.GetString("SmtpServer");
            itsSmtpServerPort = 25;
        }

        ///<summary>
        /// Gets or sets the name or IP address of the host used for SMTP transactions.
        ///</summary>
        public string SmtpServerHost
        {
            get { return itsSmtpServerHost; }
            set{ itsSmtpServerHost = value;}
        }

        ///<summary>
        /// Gets or sets the port used for SMTP transactions.
        ///</summary>
        public int SmtpServerPort
        {
            get { return itsSmtpServerPort; }
            set { itsSmtpServerPort = value; }
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration
        /// </summary>
        public void Send()
        {
            doSend(null, null,null);
        }

        /// <summary>
        /// Sends the email message using the "SmtpServer" setting in the
        /// configuration and authenticates using the provided username and password
        /// </summary>
        /// <param name="username">The username used for authentification</param>
        /// <param name="password">The password used for authentification</param>
        public void SendAuthenticated(string username, string password)
        {
            doSend(username, password,null);
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
            doSend(username, password, domain);
        }

        private void doSend(string authUsername, string authPassword, string authDomain)
        {
            //EmailMessage message =
            //    new EmailMessage((string) itsEmailAddresses[0], itsFromAddress, itsSubject, itsContent,
            //                     BodyPartFormat.Plain);
            //for (int i = 1; i < itsEmailAddresses.Count; i++)
            //{
            //    message.Recipients.Add((string) itsEmailAddresses[i]);
            //}
            //if (itsAttachmentPath != null && itsAttachmentPath.Length > 0)
            //{
            //    message.Attachments.Add(itsAttachmentPath);
            //}
            //SMTP server = new SMTP(GlobalRegistry.SettingsStorer.GetString("SmtpServer"));
            //server.Send(message);

            if (itsSmtpServerHost == null)
            {
                throw new System.Exception("Please specify the SMTP Host Name before attempting to send.");
            }
            MailMessage message = new MailMessage();
            message.From = new MailAddress(itsFromAddress);
            addAddresses(message.To, itsToAddresses);
            addAddresses(message.CC, itsCcAddresses);
            addAddresses(message.Bcc, itsBccAddresses);
            message.Subject = itsSubject;
            message.Body = itsContent;
            AttachmentCollection attachments = message.Attachments;
            foreach (string attachmentPath in itsAttachmentPaths)
            {
                attachments.Add(new Attachment(attachmentPath));
            }
            SmtpClient smtpServer = new SmtpClient();
            smtpServer.Host = itsSmtpServerHost;
            smtpServer.Port = itsSmtpServerPort;
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