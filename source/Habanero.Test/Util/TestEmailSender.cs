// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    /// <summary>
    /// This class does a call on the methods but does not actually send anything
    /// </summary>
    [TestFixture]
    public class TestEmailSender
    {
        private ISettings _settings = GlobalRegistry.Settings;

        [SetUp]
        public void SetupFixture()
        {
            _settings = GlobalRegistry.Settings;
            GlobalRegistry.Settings = null;
        }

        [TearDown]
        public void RestoreSettings()
        {
            GlobalRegistry.Settings = _settings;
        }

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

        [Test]
        public void TestInvalidEmail()
        {
            //---------------Set up test pack-------------------
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");

            //---------------Execute Test ----------------------
            try
            {
                sender.Send();
                Assert.Fail("Expected to throw an Exception");
            }
                //---------------Test Result -----------------------
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestAuthenticatedNoHost()
        {
            //---------------Set up test pack-------------------
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            //---------------Execute Test ----------------------
            try
            {
                sender.SendAuthenticated("username", "password");
                Assert.Fail("Expected to throw an Exception");
            }
                //---------------Test Result -----------------------
            catch (Exception ex)
            {
                StringAssert.Contains("Please specify the SMTP Host Name before attempting to send", ex.Message);
            }
        }

        [Test]
        public void TestAuthenticatedNoPort()
        {
            //---------------Set up test pack-------------------
            string[] recipients = { "to", "to2" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.SmtpServerHost = "testhost";
            //---------------Execute Test ----------------------
            try
            {
                sender.SendAuthenticated("username", "password");
                Assert.Fail("Expected to throw an FormatException");
            }
                //---------------Test Result -----------------------
            catch (FormatException ex)
            {
                StringAssert.Contains("The specified string is not in the form required for an e-mail address", ex.Message);
            }
        }

        [Test]
        public void TestAuthenticatedWithDomain()
        {
            //---------------Set up test pack-------------------
            string[] recipients = { "to" };
            EmailSender sender = new EmailSender(recipients, "from", "subject", "content", "attach");
            sender.SmtpServerHost = "testhost";
            //---------------Execute Test ----------------------
            try
            {
                sender.SendAuthenticated("username", "password", "domain");
                Assert.Fail("Expected to throw an FormatException");
            }
                //---------------Test Result -----------------------
            catch (FormatException ex)
            {
                StringAssert.Contains("The specified string is not in the form required for an e-mail address", ex.Message);
            }
        }
    }
}
