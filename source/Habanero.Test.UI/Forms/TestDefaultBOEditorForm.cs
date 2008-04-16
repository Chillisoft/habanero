//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.UI.Forms;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestDefaultBOEditory.
    /// </summary>
    [TestFixture]
    public class TestDefaultBOEditorForm : TestUsingDatabase
    {
        private ClassDef itsClassDefMyBo;
        private BusinessObject itsBo;
        private DefaultBOEditorForm itsEditor;
        private Mock itsDatabaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            itsClassDefMyBo = MyBO.LoadClassDefWithNoLookup();
        }

        [SetUp]
        public void SetupTest()
        {
            itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection conn = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;
            itsBo = itsClassDefMyBo.CreateNewBusinessObject(conn);
            itsEditor = new DefaultBOEditorForm(itsBo);
        }

        [Test]
        public void TestLayout()
        {
            Assert.AreEqual(2, itsEditor.Controls.Count);
            Control boCtl = itsEditor.Controls[0];
            Assert.AreEqual(4, boCtl.Controls.Count);
            Control buttonControl = itsEditor.Controls[1];
            Assert.AreSame(typeof (ButtonControl), buttonControl.GetType());
            Assert.AreEqual(2, buttonControl.Controls.Count);
        }

        [Test]
        public void TestSuccessfulEdit()
        {
            itsEditor.Show();
            itsBo.SetPropertyValue("TestProp", "TestValue");
            itsBo.SetPropertyValue("TestProp2", "TestValue2");
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsEditor.Buttons.ClickButton("OK");
            Assert.IsFalse(itsEditor.Visible);
            Assert.AreEqual(DialogResult.OK, itsEditor.DialogResult);
            Assert.AreEqual("TestValue", itsBo.GetPropertyValue("TestProp"));
            itsEditor.Dispose();
        }

        [Test]
        public void TestUnsuccessfulEdit()
        {
            itsEditor.Show();
            itsBo.SetPropertyValue("TestProp", "TestValue");
            itsBo.SetPropertyValue("TestProp2", "TestValue2");
            itsEditor.Buttons.ClickButton("Cancel");
            Assert.IsFalse(itsEditor.Visible);
            Assert.AreEqual(DialogResult.Cancel, itsEditor.DialogResult);
            object propertyValue = itsBo.GetPropertyValue("TestProp");
            Assert.AreEqual(null, propertyValue, propertyValue != null ? propertyValue.ToString() : null);
            itsEditor.Dispose();
        }
    }
}