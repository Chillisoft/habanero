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
using System;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestCloseBOEditorDialog
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        protected virtual ControlFactoryWin CreateControlFactoryWin()
        {
            return new ControlFactoryWin();
        }

        protected virtual CloseBOEditorDialogWin CreateDialogBox(IControlFactory factory, IBusinessObject businessObject)
        {
            return new CloseBOEditorDialogWin(factory, businessObject);
        }
        protected virtual CloseBOEditorDialogWin CreateDialogBoxWithDisplayName(IControlFactory factory)
        {
            return new CloseBOEditorDialogWin(factory, "fdsafasd", true, true);
        }

        protected virtual ICloseBOEditorDialog CreateDialogBox()
        {
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO();
            return CreateDialogBox(factory, businessObject);
        }

        private static IBusinessObject CreateMockBO()
        {
            return CreateMockBO(true, true, true);
        }

        private static IBusinessObject CreateMockBO(bool isNew, bool isDirty, bool isValid)
        {
            var businessObject = MockRepository.GenerateMock<IBusinessObject>();
            var boStatus = MockRepository.GenerateMock<IBOStatus>();
            boStatus.Stub(status => status.IsNew).Return(isNew);
            boStatus.Stub(status => status.IsDirty).Return(isDirty);
            boStatus.Stub(status => status.IsValid()).Return(isValid);
            businessObject.Stub(bo => bo.Status).Return(boStatus);
            var classDef = MockRepository.GenerateMock<IClassDef>();
            businessObject.Stub(bo => bo.ClassDef).Return(classDef);
            return businessObject;
        }

        [Test]
        public void Test_Construct_ShouldConstructButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox(factory, CreateMockBO());
            //---------------Test Result -----------------------
            Assert.IsNotNull(dialogWin);
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
        }
        [Test]
        public void Test_ConstructWithDisplayname_ShouldConstructButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBoxWithDisplayName(factory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dialogWin);
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
        }

        [Test]
        public void Test_Construct_WhenNullBO_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = MockRepository.GenerateMock<IControlFactory>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateDialogBox(factory, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("businessObject", ex.ParamName);
            }
        }
        [Test]
        public void Test_Construct_WhenNullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateDialogBox(null, CreateMockBO());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_ClickSaveAndClose_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();

            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.Show();
            dialogWin.SaveAndCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.SaveAndClose, dialogWin.BOEditorDialogResult);
        }


        [Test]
        public void Test_ClickSaveAndClose_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.SaveAndCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.Show();
            dialogWin.SaveAndCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }

        [Test]
        public void Test_CloseWithoutSaving_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            //---------------Execute Test ----------------------
            dialogWin.Show();
            dialogWin.CloseWithoutSavingBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CloseWithoutSaving, dialogWin.BOEditorDialogResult);
        }

        [Test]
        public void Test_CloseWithoutSaving_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CloseWithoutSavingBtn);
            //---------------Execute Test ----------------------
            dialogWin.Show();
            dialogWin.CloseWithoutSavingBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }

        [Test]
        public void Test_CancelCloseBtn_ShouldSetDialogResult()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.CancelCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CancelClose, dialogWin.BOEditorDialogResult);
        }

        [Test]
        public void Test_CancelCloseBtn_ShouldCloseForm()
        {
            //---------------Set up test pack-------------------
            ICloseBOEditorDialog dialogWin = CreateDialogBox();
            EventHandler closedEventHandler = MockRepository.GenerateStub<EventHandler>();
            dialogWin.Closed += closedEventHandler;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(dialogWin.CancelCloseBtn);
            //---------------Execute Test ----------------------
            dialogWin.Show();
            dialogWin.CancelCloseBtn.PerformClick();
            //---------------Test Result -----------------------
            closedEventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Anything, Arg<EventArgs>.Is.Anything));
        }


        [Test]
        public void Test_Construct_WithDirtyValidBO_ShouldEnableAllButtons()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, true, true);
            //---------------Assert Precondition----------------
            Assert.IsTrue(businessObject.Status.IsDirty);
            Assert.IsTrue(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            CloseBOEditorDialogWin dialogWin = CreateDialogBox(factory, businessObject);
            //---------------Test Result -----------------------
            Assert.IsTrue(dialogWin.SaveAndCloseBtn.Enabled);
            Assert.IsTrue(dialogWin.CloseWithoutSavingBtn.Enabled);
            Assert.IsTrue(dialogWin.CancelCloseBtn.Enabled);
        }

        [Test]
        public void Test_Construct_WithDirtyInvalidBO_ShouldDisableSaveAndCloseBtn()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, true, false);
            //---------------Assert Precondition----------------
            Assert.IsTrue(businessObject.Status.IsDirty);
            Assert.IsFalse(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            CloseBOEditorDialogWin dialogWin = new CloseBOEditorDialogWin(factory, businessObject);
            //---------------Test Result -----------------------
            Assert.IsFalse(dialogWin.SaveAndCloseBtn.Enabled, "Should be disabled");
            Assert.IsTrue(dialogWin.CloseWithoutSavingBtn.Enabled);
            Assert.IsTrue(dialogWin.CancelCloseBtn.Enabled);
        }
        [Test]
        public void Test_Construct_WithNotDirtyNotValidBO_ShouldCloseFormAndReturnCloseWithoutSaving()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = CreateControlFactoryWin();
            var businessObject = CreateMockBO(false, false, true);
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObject.Status.IsDirty);
            Assert.IsTrue(businessObject.Status.IsValid());
            //---------------Execute Test ----------------------
            CloseBOEditorDialogWin dialogWin = new CloseBOEditorDialogWin(factory, businessObject);
            //---------------Test Result -----------------------
            Assert.AreEqual(CloseBOEditorDialogResult.CloseWithoutSaving, dialogWin.BOEditorDialogResult);
        }

    }
}