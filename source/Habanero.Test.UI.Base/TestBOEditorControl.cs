using System;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestBOEditorControl
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";

        private static ClassDef GetCustomClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            ClassDef classDef = OrganisationTestBO.LoadDefaultClassDef();
            UIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = originalGridDef.Clone();
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, null, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private static IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        private static void AssertControlsAreEnabled(IBusinessObjectPanel controlWin)
        {
            Assert.IsTrue
                (controlWin.Enabled, "IBusinessObjectPanel should be disabled at construction since no BO is set");
            Assert.IsTrue
                (controlWin.PanelInfo.Panel.Enabled,
                 "IBusinessObjectPanel's BOPanel s should be disabled at construction since no BO is set");
        }

        private static void AssertControlsAreDisabled(IBusinessObjectPanel controlWin)
        {
            Assert.IsFalse
                (controlWin.Enabled, "IBusinessObjectPanel should be disabled at construction since no BO is set");
            Assert.IsFalse
                (controlWin.PanelInfo.Panel.Enabled,
                 "IBusinessObjectPanel's BOPanel should be disabled at construction since no BO is set");
        }

        protected virtual IBOPanelEditorControl CreateEditorControl(ClassDef classDef)
        {
            return new BOEditorControlWin(classDef);
        }

        [Test]
        public virtual void TestConstructor_NullUIDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin(GetControlFactory(), def, null);

                Assert.Fail("Null uiDefName should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("uiDefName", ex.ParamName);
            }
        }

        [Test]
        public virtual void TestConstructor_NullControlFactory_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin(null, def, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public virtual void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin(GetControlFactory(), null, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("classDef", ex.ParamName);
            }
        }

        [Test]
        public virtual void TestConstructor_uiDefDoesNotHaveAUIForm_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();

            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'BOEditorControlWin";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME
                                           + "' in the classDef '" + def.ClassNameFull
                                           + "' does not have a UIForm defined";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Test]
        public void TestConstructor_DefaultUIDef_NoControlFactory()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(GlobalUIRegistry.ControlFactory);
            //---------------Execute Test ----------------------
            IBOPanelEditorControl controlWin = new BOEditorControlWin(def);
            //---------------Test Result -----------------------
            Assert.IsNotNull(controlWin);
            AssertControlsAreDisabled(controlWin);
        }

        [Test]
        public void Test_setBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBOPanelEditorControl controlWin = new BOEditorControlWin(GetCustomClassDef());
            OrganisationTestBO businessObject = OrganisationTestBO.CreateSavedOrganisation();
            //---------------Assert Precondition----------------
            Assert.IsNull(controlWin.BusinessObject);
            // ---------------Execute Test ----------------------
            controlWin.BusinessObject = businessObject;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(businessObject, controlWin.BusinessObject);
            Assert.AreSame(businessObject, controlWin.PanelInfo.BusinessObject);
            AssertControlsAreEnabled(controlWin);
        }

        [Test]
        public void Test_SetBusinessObject_Null()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBOPanelEditorControl controlWin = new BOEditorControlWin(GetCustomClassDef());
            OrganisationTestBO businessObject = OrganisationTestBO.CreateSavedOrganisation();
            controlWin.BusinessObject = businessObject;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(controlWin.BusinessObject);
            // ---------------Execute Test ----------------------
            controlWin.BusinessObject = null;
            //  ---------------Test Result -----------------------
            Assert.IsNull(controlWin.BusinessObject);
            AssertControlsAreDisabled(controlWin);
            controlWin.PanelInfo.ClearErrorProviders();
            AssertErrorProvidersHaveBeenCleared(controlWin);
        }

        [Test]
        public void Test_WhenSetBOInInvalidState_SetsErrorProviders()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = new BOEditorControlWin(boInvalid.ClassDef);
            //---------------Assert Precondition----------------
            Assert.IsNull(controlWin.BusinessObject);
            string errMessage;
            Assert.IsFalse(boInvalid.Status.IsValid(out errMessage));
            AssertErrorProvidersHaveBeenCleared(controlWin);
            StringAssert.Contains("Surname'", errMessage);
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = boInvalid;
            //---------------Test Result -----------------------
            AssertErrorProviderHasErrors(controlWin, "Surname");
        }

        [Test]
        public void Test_IfInvalidState_WhenSetBOToValidBo_ShouldClearErrorProviders()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = new BOEditorControlWin(boInvalid.ClassDef);
            controlWin.BusinessObject = boInvalid;
            //---------------Assert Precondition----------------
            Assert.IsFalse(boInvalid.Status.IsValid());
            AssertErrorProviderHasErrors(controlWin, "Surname");
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Test Result -----------------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            AssertErrorProvidersHaveBeenCleared(controlWin);
        }
        [Test]
        public void Test_IfInvalidState_WhenSetBOToNull_ShouldClearErrorProviders()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = new BOEditorControlWin(boInvalid.ClassDef);
            controlWin.BusinessObject = boInvalid;
            //---------------Assert Precondition----------------
            Assert.IsFalse(boInvalid.Status.IsValid());
            AssertErrorProviderHasErrors(controlWin, "Surname");
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(controlWin.BusinessObject);
            AssertErrorProvidersHaveBeenCleared(controlWin);
        }

        [Test]
        public virtual void Test_IfValidState_WhenSetControlValueToInvalidValue_ShouldUpdatesErrorProviders()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            AssertErrorProvidersHaveBeenCleared(controlWin);
            //---------------Execute Test ----------------------
            SetSurnameTextBoxToNull(controlWin);
            //---------------Test Result -----------------------
            AssertErrorProviderHasErrors(controlWin, "Surname");
        }

        [Test]
        public virtual void Test_HasErrors_WhenBOValid_ButCompulsorytFieldSetToNull_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.HasErrors);
            //---------------Execute Test ----------------------
            SetSurnameTextBoxToNull(controlWin);
            bool hasErrors = controlWin.HasErrors;
            //---------------Test Result -----------------------
            Assert.IsTrue(hasErrors);
        }

        [Test]
        public virtual void Test_HasErrors_WhenBOValid_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            //---------------Execute Test ----------------------
            bool hasErrors = controlWin.HasErrors;
            //---------------Test Result -----------------------
            Assert.IsFalse(hasErrors);
        }

        [Test]
        public virtual void Test_HasErrors_WhenBONull_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.HasErrors);
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = null;
            bool hasErrors = controlWin.HasErrors;
            //---------------Test Result -----------------------
            Assert.IsFalse(hasErrors);
        }

        [Test]
        public virtual void Test_HasErrors_WhenBOInvalid_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsFalse(controlWin.BusinessObject.IsValid());
            AssertErrorProviderHasErrors(controlWin, "Surname");
            //---------------Execute Test ----------------------
            bool hasErrors = controlWin.HasErrors;
            //---------------Test Result -----------------------
            Assert.IsTrue(hasErrors);
        }

        [Test]
        public virtual void Test_IsDirty_WhenNoEditsDone_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            
            //---------------Assert Precondition----------------
            Assert.IsTrue(person.Status.IsValid());
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = person;
            bool dirty = controlWin.IsDirty;
            //---------------Test Result -----------------------
            Assert.IsFalse(dirty);
        }
        [Test]
        public virtual void Test_IsDirty_WhenControlIsEdited_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.IsDirty);
            //---------------Execute Test ----------------------
            SetSurnameTextBoxToNull(controlWin);
            bool isDirty = controlWin.IsDirty;
            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }
        [Test]
        public virtual void Test_IsDirty_WhenBONull_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsFalse(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.IsDirty);
            //---------------Execute Test ----------------------
            controlWin.BusinessObject = null;
            bool isDirty = controlWin.IsDirty;
            //---------------Test Result -----------------------
            Assert.IsFalse(isDirty);
        }


        private static void SetSurnameTextBoxToNull(IBusinessObjectPanel controlWin)
        {
            ITextBox surnameTextBox = (ITextBox) controlWin.PanelInfo.FieldInfos["Surname"].InputControl;
            surnameTextBox.Text = "";
        }

        private static void AssertErrorProviderHasErrors(IBusinessObjectPanel controlWin, string propertyName)
        {
            IPanelInfo panelInfo = controlWin.PanelInfo;
            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos[propertyName];
            string error = fieldInfo.ControlMapper.ErrorProvider.GetError(fieldInfo.InputControl);
            Assert.IsFalse(string.IsNullOrEmpty(error), "string '" + error + "' should not be null");
        }

        private static void AssertErrorProvidersHaveBeenCleared(IBusinessObjectPanel controlWin)
        {
            IPanelInfo panelInfo = controlWin.PanelInfo;
            foreach (PanelInfo.FieldInfo fieldInfo in panelInfo.FieldInfos)
            {
                Assert.AreEqual("", fieldInfo.ControlMapper.GetErrorMessage(),"Errors should be cleared");
            }
        }

//        [Test]
//        public void Test_SetBusinessObject_Null_DisplayErrors()
//        {
//            //   ---------------Set up test pack-------------------
//            GlobalUIRegistry.ControlFactory = GetControlFactory();
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            BOEditorControlWin controlWin = new BOEditorControlWin(GetCustomClassDef());
//            controlWin.BusinessObject = null;
//            //---------------Assert Precondition----------------
//            Assert.IsNull(controlWin.BusinessObject);
//            // ---------------Execute Test ----------------------
//            controlWin.DisplayErrors();
//            //  ---------------Test Result -----------------------
//            Assert.IsNull(controlWin.BusinessObject);
//        }
//        [Test]
//        public void Test_SetBusinessObject_Null_ClearErrors()
//        {
//            //   ---------------Set up test pack-------------------
//            GlobalUIRegistry.ControlFactory = GetControlFactory();
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            BOEditorControlWin controlWin = new BOEditorControlWin(GetCustomClassDef());
//            controlWin.BusinessObject = null;
//            //---------------Assert Precondition----------------
//            Assert.IsNull(controlWin.BusinessObject);
//            // ---------------Execute Test ----------------------
//            controlWin.ClearErrors();
//            //  ---------------Test Result -----------------------
//            Assert.IsNull(controlWin.BusinessObject);
//        }
    }
}