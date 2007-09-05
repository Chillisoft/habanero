using System;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    /// <summary>
    /// This class does a call on the methods but does not actually send anything
    /// </summary>
    [TestFixture]
    public class TestEmailSender
    {
        [Test]
        public void TestServerSettings()
        {
            string[] recipients = {"to"};
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");

            sender.SmtpServerHost = "testhost";
            sender.SmtpServerPort = 100;
            Assert.AreEqual("testhost", sender.SmtpServerHost);
            Assert.AreEqual(100, sender.SmtpServerPort);
        }

        [Test, ExpectedException(typeof(Exception))]
        public void TestInvalidEmail()
        {
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.Send();
        }

        [Test, ExpectedException(typeof(Exception))]
        public void TestAuthenticatedNoHost()
        {
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.SendAuthenticated("username", "password");
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void TestAuthenticatedNoPort()
        {
            string[] recipients = { "to", "to2" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.SmtpServerHost = "testhost";
            sender.SendAuthenticated("username", "password");
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void TestAuthenticatedWithDomain()
        {
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.SmtpServerHost = "testhost";
            sender.SendAuthenticated("username", "password", "domain");
        }
    }
}
