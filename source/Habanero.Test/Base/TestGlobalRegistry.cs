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
using Habanero.Console;
using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestGlobalRegistry
    {
        //private string _appName = GlobalRegistry.ApplicationName;
        //private string _appVersion = GlobalRegistry.ApplicationVersion;
        //private int _dbVersion = GlobalRegistry.DatabaseVersion;
        //private ISettings _settings = GlobalRegistry.Settings;
        //private IExceptionNotifier _exNotifier = GlobalRegistry.UIExceptionNotifier;

        //[SetUp]
        //public void SetupTest()
        //{
        //    GlobalRegistry.ApplicationName = _appName;
        //    GlobalRegistry.ApplicationVersion = _appVersion;
        //    GlobalRegistry.DatabaseVersion = _dbVersion;
        //    GlobalRegistry.Settings = _settings;
        //    GlobalRegistry.UIExceptionNotifier = _exNotifier;
        //}

        //[TearDown]
        //public void RestoreRegistry()
        //{
        //    GlobalRegistry.ApplicationName = _appName;
        //    GlobalRegistry.ApplicationVersion = _appVersion;
        //    GlobalRegistry.DatabaseVersion = _dbVersion;
        //    GlobalRegistry.Settings = _settings;
        //    GlobalRegistry.UIExceptionNotifier = _exNotifier;
        //}

        // This test fails because other tests have already set the global registry
//        [Test]
//        public void TestInitialValues()
//        {
//            Assert.IsNull(GlobalRegistry.ApplicationName);
//            Assert.IsNull(GlobalRegistry.ApplicationVersion);
//            Assert.AreEqual(0,GlobalRegistry.DatabaseVersion);
//            Assert.IsNull(GlobalRegistry.Settings);
//            Assert.IsNull(GlobalRegistry.UIExceptionNotifier);
//        }

        //[Test]
        //public void TestGetsAndSets()
        //{
        //    GlobalRegistry.ApplicationName = "testapp";
        //    GlobalRegistry.ApplicationVersion = "v1";
        //    GlobalRegistry.DatabaseVersion = 1;
        //    GlobalRegistry.Settings = new ConfigFileSettings();
        //    GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();

        //    Assert.AreEqual("testapp", GlobalRegistry.ApplicationName);
        //    Assert.AreEqual("v1", GlobalRegistry.ApplicationVersion);
        //    Assert.AreEqual(1, GlobalRegistry.DatabaseVersion);
        //    Assert.AreEqual(typeof(ConfigFileSettings), GlobalRegistry.Settings.GetType());
        //    Assert.AreEqual(typeof(ConsoleExceptionNotifier), GlobalRegistry.UIExceptionNotifier.GetType());
        //}

        [Test]
        public void Test_UIExceptionNotifier_SetAndGet()
        {
            //---------------Set up test pack-------------------
            ConsoleExceptionNotifier exceptionNotifier = new ConsoleExceptionNotifier();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;
            //---------------Test Result -----------------------
            Assert.AreSame(exceptionNotifier, GlobalRegistry.UIExceptionNotifier);
        }

        [Test]
        public void Test_UIExceptionNotifier_WhenNull_ShouldReturnRethrowingExceptionNotifier()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.UIExceptionNotifier = null;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(RethrowingExceptionNotifier), GlobalRegistry.UIExceptionNotifier);
        }

        [Test]
        public void Test_Settings_SetAndGet()
        {
            //---------------Set up test pack-------------------
            ISettings settings = new ConfigFileSettings();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.Settings = settings;
            //---------------Test Result -----------------------
            Assert.AreSame(settings, GlobalRegistry.Settings);
        }

        [Test]
        public void Test_ApplicationName_SetAndGet()
        {
            //---------------Set up test pack-------------------
            string applicationName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.ApplicationName = applicationName;
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, GlobalRegistry.ApplicationName);
        }

        [Test]
        public void Test_ApplicationVersion_SetAndGet()
        {
            //---------------Set up test pack-------------------
            string applicationVersion = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.ApplicationVersion = applicationVersion;
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationVersion, GlobalRegistry.ApplicationVersion);
        }

        [Test]
        public void Test_DatabaseVersion_SetAndGet()
        {
            //---------------Set up test pack-------------------
            int databaseVersion = TestUtil.GetRandomInt();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.DatabaseVersion = databaseVersion;
            //---------------Test Result -----------------------
            Assert.AreEqual(databaseVersion, GlobalRegistry.DatabaseVersion);
        }


        [Test]
        public void Test_LoggerFactory_SetAndGet()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory habaneroLoggerFactory = MockRepository.GenerateStub<IHabaneroLoggerFactory>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GlobalRegistry.LoggerFactory = habaneroLoggerFactory;
            //---------------Test Result -----------------------
            Assert.AreEqual(habaneroLoggerFactory, GlobalRegistry.LoggerFactory);
        }

    }
}
