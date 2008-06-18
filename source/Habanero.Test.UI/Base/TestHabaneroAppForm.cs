using System;
using System.Collections.Generic;
using System.Text;
using Habanero.DB;
using Habanero.UI;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestHabaneroAppForm 
    {

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
    }
}
