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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestGlobalRegistry
    {
        private string appName = GlobalRegistry.ApplicationName;
        private string appVersion = GlobalRegistry.ApplicationVersion;
        private int dbVersion = GlobalRegistry.DatabaseVersion;
        private ISettings settings = GlobalRegistry.Settings;
        private IExceptionNotifier exNotifier = GlobalRegistry.UIExceptionNotifier;

        [SetUp]
        public void ResetRegistry()
        {
            GlobalRegistry.ApplicationName = appName;
            GlobalRegistry.ApplicationVersion = appVersion;
            GlobalRegistry.DatabaseVersion = dbVersion;
            GlobalRegistry.Settings = settings;
            GlobalRegistry.UIExceptionNotifier = exNotifier;
        }

        [TearDown]
        public void RestoreRegistry()
        {
            GlobalRegistry.ApplicationName = appName;
            GlobalRegistry.ApplicationVersion = appVersion;
            GlobalRegistry.DatabaseVersion = dbVersion;
            GlobalRegistry.Settings = settings;
            GlobalRegistry.UIExceptionNotifier = exNotifier;
        }

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

        [Test]
        public void TestGetsAndSets()
        {
            GlobalRegistry.ApplicationName = "testapp";
            GlobalRegistry.ApplicationVersion = "v1";
            GlobalRegistry.DatabaseVersion = 1;
            GlobalRegistry.Settings = new ConfigFileSettings();
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();

            Assert.AreEqual("testapp", GlobalRegistry.ApplicationName);
            Assert.AreEqual("v1", GlobalRegistry.ApplicationVersion);
            Assert.AreEqual(1, GlobalRegistry.DatabaseVersion);
            Assert.AreEqual(typeof(ConfigFileSettings), GlobalRegistry.Settings.GetType());
            Assert.AreEqual(typeof(ConsoleExceptionNotifier), GlobalRegistry.UIExceptionNotifier.GetType());
        }
    }
}
