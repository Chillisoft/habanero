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
