//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
