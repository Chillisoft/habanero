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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Console;
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
        //private IDatabaseConnection _conn;

        [TestFixture]
        public class TestDefaultBOEditorFormWin : TestDefaultBOEditorForm
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test]
            public void TestLayout()
            {
                //---------------Test Result -----------------------
                Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
                IControlHabanero boCtl = _defaultBOEditorForm.Controls[0];
                Assert.AreEqual(6, boCtl.Controls.Count);
                IControlHabanero buttonControl = _defaultBOEditorForm.Controls[1];
                Assert.IsTrue(buttonControl is Habanero.UI.Win.ButtonGroupControlWin);
                Assert.AreEqual(2, buttonControl.Controls.Count);
            }
            
            [Test]
            public void TestSuccessfulEdit()
            {
                //---------------Set up test pack-------------------
                _defaultBOEditorForm.Show();
                _bo.SetPropertyValue("TestProp", "TestValue");
                _bo.SetPropertyValue("TestProp2", "TestValue2");
                PrepareMockForSave();
                ///---------------Execute Test ----------------------
                _defaultBOEditorForm.Buttons["OK"].PerformClick();
                //---------------Test Result -----------------------
                Assert.IsFalse(_defaultBOEditorForm.Visible);
                Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
                Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
                Assert.IsNull(_defaultBOEditorForm.PanelInfo.BusinessObject);
                //TearDown--------------------------
                _defaultBOEditorForm.Dispose();
            }

            [Test]
            public void TestUnsuccessfulEdit()
            {
                //---------------Set up test pack-------------------
                _defaultBOEditorForm.Show();
                _bo.SetPropertyValue("TestProp", "TestValue");
                _bo.SetPropertyValue("TestProp2", "TestValue2");
                //---------------Execute Test ----------------------
                _defaultBOEditorForm.Buttons["Cancel"].PerformClick();
                //---------------Test Result -----------------------
                Assert.IsFalse(_defaultBOEditorForm.Visible);
                Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
                object propertyValue = _bo.GetPropertyValue("TestProp");
                Assert.AreEqual(null, propertyValue, propertyValue != null ? propertyValue.ToString() : null);
                _defaultBOEditorForm.Dispose();
            }

            [Test]
            public void TestSuccessfulEditCallsDelegate()
            {
                //---------------Set up test pack-------------------
                //IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject(_conn);
                IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();

                bool delegateCalled = false;
                bool cancelledValue = true;
                IBusinessObject boInDelegate = null;
                IDefaultBOEditorForm boEditorForm = GetControlFactory()
                    .CreateBOEditorForm((BusinessObject)bo, "default", 
                    delegate(IBusinessObject bo1, bool cancelled)
                    {
                        delegateCalled = true;
                        cancelledValue = cancelled;
                        boInDelegate = bo1;
                    });
                boEditorForm.Show();
                bo.SetPropertyValue("TestProp", "TestValue");
                bo.SetPropertyValue("TestProp2", "TestValue2");
                PrepareMockForSave();
                //--------------Assert PreConditions----------------            
                Assert.IsFalse(delegateCalled);
                //---------------Execute Test ----------------------
                boEditorForm.Buttons["OK"].PerformClick();
                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
                Assert.IsFalse(cancelledValue);
                Assert.AreSame(bo, boInDelegate);
                //---------------Tear Down -------------------------          
            }

            [Test]
            public void TestUnsuccessfulEditCallsDelegate()
            {
                //---------------Set up test pack-------------------
                //IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject(_conn);
                IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();

                bool delegateCalled = false;
                bool cancelledValue = false;
                IBusinessObject boInDelegate = null;
                IDefaultBOEditorForm boEditorForm =
                    GetControlFactory().CreateBOEditorForm((BusinessObject)bo, "default",
                    delegate(IBusinessObject bo1, bool cancelled)
                    {
                        delegateCalled = true;
                        cancelledValue = cancelled;
                        boInDelegate = bo1;
                    });
                boEditorForm.Show();
                bo.SetPropertyValue("TestProp", "TestValue");
                bo.SetPropertyValue("TestProp2", "TestValue2");
                PrepareMockForSave();
                //--------------Assert PreConditions----------------            
                Assert.IsFalse(delegateCalled);
                //---------------Execute Test ----------------------
                boEditorForm.Buttons["Cancel"].PerformClick();
                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
                Assert.IsTrue(cancelledValue);
                Assert.AreSame(bo, boInDelegate);
                //---------------Tear Down -------------------------          
            }

        }

        [TestFixture]
        public class TestDefaultBOEditorFormVWG : TestDefaultBOEditorForm
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.VWG.ControlFactoryVWG();
            }

            //Create a duplicate for win
            [Test]
            public void TestLayout()
            {
                //---------------Test Result -----------------------
                Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
                IControlHabanero boCtl = _defaultBOEditorForm.Controls[0];
                Assert.AreEqual(6, boCtl.Controls.Count);
                IControlHabanero buttonControl = _defaultBOEditorForm.Controls[1];
                Assert.IsTrue(buttonControl is Habanero.UI.VWG.ButtonGroupControlVWG);
                Assert.AreEqual(2, buttonControl.Controls.Count);
            }
        }

        private ClassDef _classDefMyBo;
        private IBusinessObject _bo;
        private IDefaultBOEditorForm _defaultBOEditorForm;
        private Mock _databaseConnectionMockControl;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            this.SetupDBConnection();
            ClassDef.ClassDefs.Clear();
            if (GetControlFactory() is Habanero.UI.VWG.ControlFactoryVWG)
            {
                _classDefMyBo = MyBO.LoadDefaultClassDefVWG();
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

            //_conn = (IDatabaseConnection) _databaseConnectionMockControl.MockInstance;
            //_bo = _classDefMyBo.CreateNewBusinessObject(_conn);
            BusinessObjectManager.Instance.ClearLoadedObjects();
            _bo = _classDefMyBo.CreateNewBusinessObject();
            _defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject) _bo);
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

        //[Test, Ignore("get object ref not set")]
        //public void TestSuccessfulEdit()
        //{
        //    //Setup-------------------------------
        //    _defaultBOEditorForm.Show();
        //    _bo.SetPropertyValue("TestProp", "TestValue");
        //    _bo.SetPropertyValue("TestProp2", "TestValue2");
        //    PrepareMockForSave();
        //    //Execute --------------------------
        //    _defaultBOEditorForm.Buttons["OK"].PerformClick();
        //    //Assert----------------------------
        //    Assert.IsFalse(_defaultBOEditorForm.Visible);
        //    //TODO_Port: Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
        //    Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
        //    //TODO_Port: Assert.IsNull(_defaultBOEditorForm._panelInfo.ControlMappers.BusinessObject);
        //    //TearDown--------------------------
        //    _defaultBOEditorForm.Dispose();
        //}

        //[Test, Ignore("get object ref not set")]
        //public void TestUnsuccessfulEdit()
        //{
        //    _defaultBOEditorForm.Show();
        //    _bo.SetPropertyValue("TestProp", "TestValue");
        //    _bo.SetPropertyValue("TestProp2", "TestValue2");
        //    _defaultBOEditorForm.Buttons["Cancel"].PerformClick();
        //    Assert.IsFalse(_defaultBOEditorForm.Visible);
        //    //Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
        //    object propertyValue = _bo.GetPropertyValue("TestProp");
        //    Assert.AreEqual(null, propertyValue, propertyValue != null ? propertyValue.ToString() : null);
        //    _defaultBOEditorForm.Dispose();
        //}

        //[Test, Ignore("get reference not set")]
        //public void TestSuccessFullEditCallsDelegate()
        //{
        //    //---------------Set up test pack-------------------
        //    IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject(_conn);
        //    bool delegateCalled = false;
        //    IDefaultBOEditorForm boEditorForm =
        //        GetControlFactory().CreateBOEditorForm((BusinessObject) bo, "default",
        //            delegate { delegateCalled = true; });
        //    //--------------Assert PreConditions----------------            
        //    Assert.IsFalse(delegateCalled);
        //    //---------------Execute Test ----------------------
        //    boEditorForm.Buttons["OK"].PerformClick();
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(delegateCalled);
        //    //---------------Tear Down -------------------------          
        //}
    }
}