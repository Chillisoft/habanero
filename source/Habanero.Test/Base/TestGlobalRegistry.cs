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
using Habanero.Base;
using Habanero.Base.Logging;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestGlobalRegistry
    {
        [Test]
        public void Settings()
        {
            //---------------Set up test pack-------------------
            ISettings settings = new ConfigFileSettings();
            //---------------Execute Test ----------------------
            GlobalRegistry.Settings = settings;
            //---------------Test Result -----------------------
            Assert.AreSame(settings, GlobalRegistry.Settings);
        }

        [Test]
        public void Settings_ShouldDefaultToConfigFileSettings()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.Settings = null;
            //---------------Execute Test ----------------------
            var settings = GlobalRegistry.Settings;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ConfigFileSettings>(settings);
        }


        [Test]
        public void UIExceptionNotifier_ShouldDefaultToRethrowingExceptionNotifier()
        {
            GlobalRegistry.UIExceptionNotifier = null;
            //---------------Execute Test ----------------------
            var exceptionNotifier = GlobalRegistry.UIExceptionNotifier;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<RethrowingExceptionNotifier>(exceptionNotifier);
        }

        [Test]
        public void ApplicationName()
        {
            //---------------Set up test pack-------------------
            string applicationName = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            GlobalRegistry.ApplicationName = applicationName;
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, GlobalRegistry.ApplicationName);
        }

        [Test]
        public void ApplicationVersion()
        {
            //---------------Set up test pack-------------------
            string applicationVersion = TestUtil.GetRandomString();
            //---------------Execute Test ----------------------
            GlobalRegistry.ApplicationVersion = applicationVersion;
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationVersion, GlobalRegistry.ApplicationVersion);
        }

        [Test]
        public void DatabaseVersion()
        {
            //---------------Set up test pack-------------------
            int databaseVersion = TestUtil.GetRandomInt();
            //---------------Execute Test ----------------------
            GlobalRegistry.DatabaseVersion = databaseVersion;
            //---------------Test Result -----------------------
            Assert.AreEqual(databaseVersion, GlobalRegistry.DatabaseVersion);
        }

        [Test]
        public void SecurityController()
        {
            //---------------Set up test pack-------------------
            ISecurityController securityController = new NullSecurityController();
            //---------------Execute Test ----------------------
            GlobalRegistry.SecurityController = securityController;
            //---------------Test Result -----------------------
            Assert.AreSame(securityController, GlobalRegistry.SecurityController);
        }

        [Test]
        public void SecurityController_ShouldDefaultToNullSecurityController()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.SecurityController = null;
            //---------------Execute Test ----------------------
            var securityController = GlobalRegistry.SecurityController;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<NullSecurityController>(securityController);
        }

        [Test]
        public void PasswordCrypter()
        {
            //---------------Set up test pack-------------------
            ICrypter crypter = new NullCrypter();
            //---------------Execute Test ----------------------
            GlobalRegistry.PasswordCrypter = crypter;
            //---------------Test Result -----------------------
            Assert.AreSame(crypter, GlobalRegistry.PasswordCrypter);
        }

        [Test]
        public void PasswordCrypter_ShouldDefaultToNullCrypter()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.PasswordCrypter = null;
            //---------------Execute Test ----------------------
            var crypter = GlobalRegistry.PasswordCrypter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<NullCrypter>(crypter);
        }

        [Test]
        public void PasswordHasher()
        {
            //---------------Set up test pack-------------------
            IHasher hasher = new Base64Sha1Hasher();
            //---------------Execute Test ----------------------
            GlobalRegistry.PasswordHasher = hasher;
            //---------------Test Result -----------------------
            Assert.AreSame(hasher, GlobalRegistry.PasswordHasher);
        }

        [Test]
        public void PasswordHasher_ShouldDefaultToUtf8Sha1Hasher()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.PasswordHasher = null;
            //---------------Execute Test ----------------------
            var hasher = GlobalRegistry.PasswordHasher;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<Utf8Sha1Hasher>(hasher);
        }

        [Test]
        public void LoggerFactory()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new ConsoleLoggerFactory();
            //---------------Execute Test ----------------------
            GlobalRegistry.LoggerFactory = loggerFactory;
            //---------------Test Result -----------------------
            Assert.AreSame(loggerFactory, GlobalRegistry.LoggerFactory);
        }

        [Test]
        public void LoggerFactory_ShouldDefaultToConsoleLoggerFactory()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.LoggerFactory = null;
            //---------------Execute Test ----------------------
            var loggerFactory = GlobalRegistry.LoggerFactory;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ConsoleLoggerFactory>(loggerFactory);
        }

     
    }
}
