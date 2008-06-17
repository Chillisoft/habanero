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


using System;
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
        private ClassDef _classDefMyBo;
        private IBusinessObject _bo;
        private DefaultBOEditorForm _defaultBOEditorForm;
        private Mock _databaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            _classDefMyBo = MyBO.LoadClassDefWithNoLookup();
        }

        [SetUp]
        public void SetupTest()
        {
            _databaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection conn = (IDatabaseConnection) _databaseConnectionMockControl.MockInstance;
            _bo = _classDefMyBo.CreateNewBusinessObject(conn);
            _defaultBOEditorForm = new DefaultBOEditorForm((BusinessObject) _bo);
        }

        #region Utility Methods

        private void PrepareMockForSave()
        {
            _databaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                           DatabaseConnection.CurrentConnection.GetConnection());
            _databaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                           DatabaseConnection.CurrentConnection.GetConnection());
            _databaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                           DatabaseConnection.CurrentConnection.GetConnection());
            _databaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] { null, null });
        }

        #endregion //Utility Methods

        [Test]
        public void TestLayout()
        {
            Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
            Control boCtl = _defaultBOEditorForm.Controls[0];
            Assert.AreEqual(4, boCtl.Controls.Count);
            Control buttonControl = _defaultBOEditorForm.Controls[1];
            Assert.AreSame(typeof (ButtonControl), buttonControl.GetType());
            Assert.AreEqual(2, buttonControl.Controls.Count);
        }

        [Test]
        public void TestSuccessfulEdit()
        {
            //Setup
            _defaultBOEditorForm.Show();
            _bo.SetPropertyValue("TestProp", "TestValue");
            _bo.SetPropertyValue("TestProp2", "TestValue2");
            PrepareMockForSave();
            //Fixture
            _defaultBOEditorForm.Buttons.ClickButton("OK");
            //Assert
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
            Assert.IsNull(_defaultBOEditorForm._panelFactoryInfo.ControlMappers.BusinessObject);
            //TearDown
            _defaultBOEditorForm.Dispose();
        }

        [Test]
        public void TestUnsuccessfulEdit()
        {
            _defaultBOEditorForm.Show();
            _bo.SetPropertyValue("TestProp", "TestValue");
            _bo.SetPropertyValue("TestProp2", "TestValue2");
            _defaultBOEditorForm.Buttons.ClickButton("Cancel");
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
            object propertyValue = _bo.GetPropertyValue("TestProp");
            Assert.AreEqual(null, propertyValue, propertyValue != null ? propertyValue.ToString() : null);
            _defaultBOEditorForm.Dispose();
        }
        
    }
}