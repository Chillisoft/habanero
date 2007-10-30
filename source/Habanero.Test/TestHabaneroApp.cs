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

using Habanero.UI.Forms;
using NUnit.Framework;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.Util;
using Habanero.DB;
using Habanero;

namespace Habanero.Test
{
    [TestFixture]
    public class TestHabaneroApp
    {
        [Test]
        public void TestDefaultValues()
        {
            HabaneroAppConsole app = new HabaneroAppConsole("testapp", "v1");
            //Assert.AreEqual(app.ApplicationVersionUpgrader);
            Assert.AreEqual("testapp", app.AppName);
            Assert.AreEqual("v1", app.AppVersion);
            Assert.AreEqual("ClassDefs.xml", app.ClassDefsFileName);
            //Assert.AreEqual(app.ClassDefsPath);
            //Assert.AreEqual(app.DatabaseConfig);
            //Assert.AreEqual(app.DefClassFactory);
            Assert.IsNull(app.ExceptionNotifier);
            Assert.IsTrue(app.LoadClassDefs);
            Assert.IsNull(app.Settings);
        }

        [Test]
        public void TestManualSettings()
        {
            HabaneroApp app = new HabaneroAppConsole("testapp", "v1");
            app.ApplicationVersionUpgrader = new TestApplicationVersionUpgrader();
            //app.AppName = "testappchange";
            //app.AppVersion = "v2";
            app.ClassDefsFileName = "newdefs.xml";
            app.ClassDefsPath = "testfolder";
            app.ExceptionNotifier = new ConsoleExceptionNotifier();
            app.LoadClassDefs = false;
            app.Settings = new ConfigFileSettings();

            //Assert.AreEqual(typeof(TestApplicationVersionUpgrader), app.ApplicationVersionUpgrader.GetType());
            Assert.AreEqual("testapp", app.AppName);
            Assert.AreEqual("v1", app.AppVersion);
            Assert.AreEqual("newdefs.xml", app.ClassDefsFileName);
            //Assert.AreEqual("testfolder", app.ClassDefsPath);
            Assert.AreEqual(typeof(ConsoleExceptionNotifier), app.ExceptionNotifier.GetType());
            Assert.IsFalse(app.LoadClassDefs);
            Assert.AreEqual(typeof(ConfigFileSettings), app.Settings.GetType());
        }

        [Test]
        public void TestConsoleSpecificSettings()
        {
            HabaneroAppConsole app = new HabaneroAppConsole("testapp", "v1");
            app.DatabaseConfig = new DatabaseConfig("", "", "", "", "", "");
            //app.DefClassFactory = new DefClassFactory();

            //Assert.AreEqual(typeof(DatabaseConfig), app.DatabaseConfig.GetType());
            //Assert.AreEqual(app.DefClassFactory);
        }

        [Test]
        public void TestFormSpecificSettings()
        {
            HabaneroAppForm app = new HabaneroAppForm("testapp", "v1");
            app.DatabaseConfig = new DatabaseConfig("", "", "", "", "", "");
            //app.DefClassFactory = new DefClassFactory();

            //Assert.AreEqual(typeof(DatabaseConfig), app.DatabaseConfig.GetType());
            //Assert.AreEqual(app.DefClassFactory);
        }

        [Test]
        public void TestConsoleStartup()
        {
            HabaneroAppConsole app = new HabaneroAppConsole("testapp", "v1");
            app.LoadClassDefs = false;
            app.Startup();

            //Assert.AreEqual(typeof(TestApplicationVersionUpgrader), app.ApplicationVersionUpgrader.GetType());
            Assert.AreEqual("testapp", app.AppName);
            Assert.AreEqual("v1", app.AppVersion);
            Assert.AreEqual("ClassDefs.xml", app.ClassDefsFileName);
            //Assert.AreEqual("testfolder", app.ClassDefsPath);
            Assert.AreEqual(typeof(ConsoleExceptionNotifier), app.ExceptionNotifier.GetType());
            Assert.IsFalse(app.LoadClassDefs);
            //Assert.AreEqual(typeof(DatabaseSettings), app.Settings.GetType());
            Assert.IsNull(app.Settings);

            //Assert.AreEqual(typeof(DatabaseConfig), app.DatabaseConfig.GetType());
            //Assert.AreEqual(app.DefClassFactory);
        }

        [Test]
        public void TestFormStartup()
        {
            HabaneroAppForm app = new HabaneroAppForm("testapp", "v1");
            app.LoadClassDefs = false;
            app.DatabaseConfig = new DatabaseConfig("", "", "", "", "", "");
            app.Startup();

            //Assert.AreEqual(typeof(TestApplicationVersionUpgrader), app.ApplicationVersionUpgrader.GetType());
            Assert.AreEqual("testapp", app.AppName);
            Assert.AreEqual("v1", app.AppVersion);
            Assert.AreEqual("ClassDefs.xml", app.ClassDefsFileName);
            //Assert.AreEqual("testfolder", app.ClassDefsPath);
            Assert.AreEqual(typeof(FormExceptionNotifier), app.ExceptionNotifier.GetType());
            Assert.IsFalse(app.LoadClassDefs);
            Assert.AreEqual(typeof(DatabaseSettings), app.Settings.GetType());
            
            //Assert.AreEqual(typeof(DatabaseConfig), app.DatabaseConfig.GetType());
            //Assert.AreEqual(app.DefClassFactory);
        }



        private class TestApplicationVersionUpgrader : IApplicationVersionUpgrader
        {
            public void Upgrade()
            {
                //upgrades done here
            }
        }
    }
}
