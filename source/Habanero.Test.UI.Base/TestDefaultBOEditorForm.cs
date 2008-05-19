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


using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.UI.Base;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestDefaultBOEditory.
    /// </summary>
    public abstract class TestDefaultBOEditorForm : TestUsingDatabase
    {
        protected abstract IControlFactory GetControlFactory();
        private IDatabaseConnection _conn;
        //[TestFixture]
        //public class TestDefaultBOEditorFormWin : TestDefaultBOEditorForm
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new Habanero.UI.Win.ControlFactoryWin();
        //    }
        //Create a duplicate for win
        //[Test]
        //public void TestLayout()
        //{
        //    Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
        //    IControlChilli boCtl = _defaultBOEditorForm.Controls[0];
        //    Assert.AreEqual(4, boCtl.Controls.Count);
        //    IControlChilli buttonControl = _defaultBOEditorForm.Controls[1];
        //    Assert.IsTrue(buttonControl.GetType().IsInstanceOfType(IButtonGroupControl);
        //    Assert.AreEqual(2, buttonControl.Controls.Count);
        //}
        //}
        [TestFixture]
        public class TestDefaultBOEditorFormGiz : TestDefaultBOEditorForm
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            //Create a duplicate for win
            [Test]
            public void TestLayout()
            {
                Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
                IControlChilli boCtl = _defaultBOEditorForm.Controls[0];
                Assert.AreEqual(4, boCtl.Controls.Count);
                IControlChilli buttonControl = _defaultBOEditorForm.Controls[1];
                Assert.IsTrue(buttonControl is Habanero.UI.WebGUI.ButtonGroupControlGiz);
                Assert.AreEqual(2, buttonControl.Controls.Count);
            }
        }

        private ClassDef _classDefMyBo;
        private BusinessObject _bo;
        private IDefaultBOEditorForm _defaultBOEditorForm;
        private Mock _databaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            if (GetControlFactory() is Habanero.UI.WebGUI.ControlFactoryGizmox)
            {
                _classDefMyBo = MyBO.LoadDefaultClassDefGizmox();
            }
            else
            {
                _classDefMyBo = MyBO.LoadClassDefWithNoLookup();
            }
        }

        [SetUp]
        public void SetupTest()
        {
            _databaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            
            _conn = (IDatabaseConnection) _databaseConnectionMockControl.MockInstance;
            _bo = _classDefMyBo.CreateNewBusinessObject(_conn);
            _defaultBOEditorForm = GetControlFactory().CreateBOEditorForm(_bo);
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
            _databaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
        }

        #endregion //Utility Methods

        [Test, Ignore("get object ref not set")]
        public void TestSuccessfulEdit()
        {
            //Setup-------------------------------
            _defaultBOEditorForm.Show();
            _bo.SetPropertyValue("TestProp", "TestValue");
            _bo.SetPropertyValue("TestProp2", "TestValue2");
            PrepareMockForSave();
            //Execute --------------------------
            _defaultBOEditorForm.Buttons["OK"].PerformClick();
            //Assert----------------------------
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            //TODO_Port: Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
            //TODO_Port: Assert.IsNull(_defaultBOEditorForm._panelFactoryInfo.ControlMappers.BusinessObject);
            //TearDown--------------------------
            _defaultBOEditorForm.Dispose();
        }

        [Test, Ignore("get object ref not set")]
        public void TestUnsuccessfulEdit()
        {
            _defaultBOEditorForm.Show();
            _bo.SetPropertyValue("TestProp", "TestValue");
            _bo.SetPropertyValue("TestProp2", "TestValue2");
            _defaultBOEditorForm.Buttons["Cancel"].PerformClick();
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            //Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
            object propertyValue = _bo.GetPropertyValue("TestProp");
            Assert.AreEqual(null, propertyValue, propertyValue != null ? propertyValue.ToString() : null);
            _defaultBOEditorForm.Dispose();
        }

        [Test, Ignore("get reference not set")]
        public void TestSuccessFullEditCallsDelegate()
        {
            //---------------Set up test pack-------------------
            BusinessObject bo = _classDefMyBo.CreateNewBusinessObject(_conn);
            bool delegateCalled = false;
            IDefaultBOEditorForm boEditorForm =
                GetControlFactory().CreateBOEditorForm(bo, "default",
                                                       delegate { delegateCalled = true; });
            //--------------Assert PreConditions----------------            
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            boEditorForm.Buttons["OK"].PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            //---------------Tear Down -------------------------          
        }
    }
}