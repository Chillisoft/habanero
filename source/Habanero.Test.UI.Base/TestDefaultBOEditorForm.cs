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
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestDefaultBOEditorForm.
    /// </summary>
    [TestFixture]
    public class TestDefaultBOEditorForm// : TestUsingDatabase
    {
        private IClassDef _classDefMyBo;
        private IBusinessObject _bo;
        private IDefaultBOEditorForm _defaultBOEditorForm;

        protected virtual IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual IDefaultBOEditorForm CreateDefaultBOEditorForm(IBusinessObject businessObject)
        {
            return GetControlFactory().CreateBOEditorForm((BusinessObject)businessObject);
        }

        protected virtual void ShowFormIfNecessary(IFormHabanero form)
        {
            form.Show();
        }

        [TestFixture]
        public class TestDefaultBOEditorFormWin : TestDefaultBOEditorForm
        {

        }

        [TestFixture]
        public class TestDefaultBOEditorFormVWG : TestDefaultBOEditorForm
        {
            //private static MyForm _myForm;
            //private readonly Thread _thread;

            //public TestDefaultBOEditorFormVWG()
            //{
            //    _thread = new Thread(() => Gizmox.WebGUI.Client.Application.Run(typeof (MyForm)));
            //    _thread.Start();
            //}

            //~TestDefaultBOEditorFormVWG()
            //{
            //    _thread.Abort();
            //}

            //public class MyForm : FormVWG
            //{
            //    public MyForm()
            //    {
            //        _myForm = this;
            //    }
            //}

            protected override void ShowFormIfNecessary(IFormHabanero form)
            {
                // Do not show the form for VWG
                //Gizmox.WebGUI.Common.Interfaces.IForm formVWG = (FormVWG) form;
                //formVWG.SetContext(_myForm.Context);
                //form.Show();
            }

            protected override IControlFactory GetControlFactory()
            {
                ControlFactoryVWG factory = new Habanero.UI.VWG.ControlFactoryVWG();
                GlobalUIRegistry.ControlFactory =factory;
                return factory;
            }

            [Test]
            [Ignore("This cannot be tested for VWG because you cannot show a form to close it")]
            public override void Test_CloseForm_ShouldCallDelegateWithCorrectInformation()
            {
            }
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            _bo = _classDefMyBo.CreateNewBusinessObject();
            _defaultBOEditorForm = CreateDefaultBOEditorForm(_bo);
        }

        [Test]
        public void Test_Layout()
        {
            //---------------Test Result -----------------------
            Assert.AreEqual(2, _defaultBOEditorForm.Controls.Count);
            IControlHabanero boCtl = _defaultBOEditorForm.Controls[0];
            Assert.AreEqual(6, boCtl.Controls.Count);
            IControlHabanero buttonControl = _defaultBOEditorForm.Controls[1];
            Assert.IsInstanceOfType(typeof(IButtonGroupControl), buttonControl);
            Assert.AreEqual(2, buttonControl.Controls.Count);
            Assert.AreEqual(FormStartPosition.CenterScreen, _defaultBOEditorForm.StartPosition);
        }

        [Test]
        public void Test_Construct_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_AlternateConstruct_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default");
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_AlternateConstruct_2_ShouldConstructWithDefaultConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default", delegate {  });
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
        }
        [Test]
        public void Test_Constructor_WithGroupCreator()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _classDefMyBo.CreateNewBusinessObject();
            GroupControlCreator groupControl = GetControlFactory().CreateCollapsiblePanelGroupControl;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDefaultBOEditorForm defaultBOEditorForm = GetControlFactory().CreateBOEditorForm((BusinessObject)bo,"default",  groupControl);
            //---------------Test Result -----------------------
            Assert.IsNotNull(defaultBOEditorForm.PanelInfo);
            Assert.IsNotNull(defaultBOEditorForm.GroupControlCreator);
            Assert.AreSame(groupControl, defaultBOEditorForm.GroupControlCreator);
        }


        [Test]
        public void Test_ClickOK_ShouldCommitEdits()
        {
            //---------------Set up test pack-------------------
            ShowFormIfNecessary(_defaultBOEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton okButton = _defaultBOEditorForm.Buttons["OK"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(okButton);
            //---------------Execute Test ----------------------
            okButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.OK, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual("TestValue", _bo.GetPropertyValue("TestProp"));
            Assert.AreEqual("TestValue2", _bo.GetPropertyValue("TestProp2"));
            Assert.IsFalse(_bo.Status.IsDirty);
            Assert.IsNull(_defaultBOEditorForm.PanelInfo.BusinessObject);
            //TearDown--------------------------
            //_defaultBOEditorForm.Dispose();
        }

        [Test]
        public void Test_ClickCancel_ShouldCancelEdits()
        {
            //---------------Set up test pack-------------------
            ShowFormIfNecessary(_defaultBOEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton cancelButton = _defaultBOEditorForm.Buttons["Cancel"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(cancelButton);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(_defaultBOEditorForm.Visible);
            Assert.AreEqual(DialogResult.Cancel, _defaultBOEditorForm.DialogResult);
            Assert.AreEqual(null, _bo.GetPropertyValue("TestProp"));
            Assert.AreEqual(null, _bo.GetPropertyValue("TestProp2"));
            Assert.IsFalse(_bo.Status.IsDirty);
            Assert.IsNull(_defaultBOEditorForm.PanelInfo.BusinessObject);
            //TearDown--------------------------
            //_defaultBOEditorForm.Dispose();
        }

        [Test]
        public void Test_ClickOK_ShouldCallDelegateWithCorrectInformation()
        {
            //---------------Set up test pack-------------------
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
            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton okButton = boEditorForm.Buttons["OK"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(okButton);
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            okButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsFalse(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        [Test]
        public void Test_ClickCancel_ShouldCallDelegateWithCorrectInformation()
        {
            //---------------Set up test pack-------------------
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
            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            IButton cancelButton = boEditorForm.Buttons["Cancel"];
            //--------------Assert PreConditions----------------
            Assert.IsNotNull(cancelButton);
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsTrue(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        [Test]
        public virtual void Test_CloseForm_ShouldCallDelegateWithCorrectInformation()
        {
            //---------------Set up test pack-------------------
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
            ShowFormIfNecessary(boEditorForm);
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp", "TestValue");
            EditControlValueOnForm(_defaultBOEditorForm, "TestProp2", "TestValue2");
            //--------------Assert PreConditions----------------
            Assert.IsFalse(delegateCalled);
            //---------------Execute Test ----------------------
            boEditorForm.Close();
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateCalled);
            Assert.IsTrue(cancelledValue);
            Assert.AreSame(bo, boInDelegate);
        }

        private static void EditControlValueOnForm(IDefaultBOEditorForm defaultBOEditorForm, string propertyName, string value)
        {
            defaultBOEditorForm.PanelInfo.FieldInfos[propertyName].ControlMapper.Control.Text = value;
        }
    }
}

//namespace VWGFormsTestExample
//{
//    using Gizmox.WebGUI.Forms;
//    using Gizmox.WebGUI.Common;
//    using NUnit.Framework;
//    using System.Threading;

//    [TestFixture]
//    public class TestFormVWG
//    {
//        private readonly Thread _thread;

//        public TestFormVWG()
//        {
//            _thread = new Thread(() => Gizmox.WebGUI.Client.Application.Run(typeof(TempForm)));
//            _thread.Start();
//            while (Global.Context == null) Thread.Sleep(100);
//        }

//        ~TestFormVWG()
//        {
//            _thread.Abort();
//        }

//        public class TempForm : Form
//        {
//            public TempForm()
//            {
//                Global.Context = this.Context;
//            }
//        }

//        [Test]
//        public void Test_ShowForm()
//        {
//            //---------------Set up test pack-------------------
//            Form form = new Form();
//            bool isLoaded = false;
//            form.Load += delegate { isLoaded = true; };
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(isLoaded);
//            Assert.IsNotNull(form.Context);
//            Assert.IsNotNull(form.Context.Config); // Fails here
//            //---------------Execute Test ----------------------
//            form.Show();
//            //---------------Test Result -----------------------
//            Assert.IsTrue(isLoaded);
//        }
//    }
//}