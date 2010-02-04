using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBOEditorControl
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _controlFactory = CreateControlFactory();
        }
        private IControlFactory _controlFactory;
        private const string CUSTOM_UIDEF_NAME = "custom1";

        protected virtual IControlFactory GetControlFactory()
        {
            if (_controlFactory == null)
            {
                _controlFactory = CreateControlFactory();
                GlobalUIRegistry.ControlFactory = _controlFactory;
            }
            return _controlFactory;
        }

        protected abstract IControlFactory CreateControlFactory();
        protected abstract IBOPanelEditorControl CreateEditorControl(IClassDef classDef);

        protected abstract IBOPanelEditorControl CreateEditorControl
            (IControlFactory controlFactory, IClassDef def, string uiDefName);

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

        [Test]
        public virtual void TestConstructor_NullControlFactory_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            IClassDef def = TestBOEditorControl.GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                CreateEditorControl(null, def, TestBOEditorControl.CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public virtual void TestConstructor_NullUIDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            IClassDef def = TestBOEditorControl.GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                CreateEditorControl(GetControlFactory(), def, null);

                Assert.Fail("Null uiDefName should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("uiDefName", ex.ParamName);
            }
        }

        [Test]
        public virtual void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            TestBOEditorControl.GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                CreateEditorControl(GetControlFactory(), null, TestBOEditorControl.CUSTOM_UIDEF_NAME);

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
            IClassDef def = TestBOEditorControl.GetCustomClassDef();

            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                CreateEditorControl(GetControlFactory(), def, TestBOEditorControl.CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'IBOEditorControl";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + TestBOEditorControl.CUSTOM_UIDEF_NAME
                                           + "' in the classDef '" + def.ClassNameFull
                                           + "' does not have a UIForm defined";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Test]
        public void TestConstructor_DefaultUIDef_NoControlFactory()
        {
            //---------------Set up test pack-------------------
            IClassDef def = TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(GlobalUIRegistry.ControlFactory);
            //---------------Execute Test ----------------------
            IBOPanelEditorControl controlWin = CreateEditorControl(def);
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
            IBOPanelEditorControl controlWin = CreateEditorControl(TestBOEditorControl.GetCustomClassDef());
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
            IBOPanelEditorControl controlWin = CreateEditorControl(TestBOEditorControl.GetCustomClassDef());
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
            TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = CreateEditorControl(boInvalid.ClassDef);
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
            TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = CreateEditorControl(boInvalid.ClassDef);
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
            TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO boInvalid = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            IBOPanelEditorControl controlWin = CreateEditorControl(boInvalid.ClassDef);
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
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
            TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.IsDirty);
            TestUtil.AssertStringNotEmpty(person.Surname, "person.Surname");
            //---------------Execute Test ----------------------
            SetSurnameTextBoxToNull(controlWin);
            bool isDirty = controlWin.IsDirty;
            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
            TestUtil.AssertStringEmpty(person.Surname, "person.Surname");
        }

        [Test]
        public virtual void Test_ApplyChangesToBo_WhenControlIsEdited_ShouldUpdateTheBusinessObject()
        {
            //---------------Set up test pack-------------------
            TestBOEditorControl.GetCustomClassDef();
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            IBOPanelEditorControl controlWin = CreateEditorControl(person.ClassDef);
            controlWin.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsTrue(controlWin.BusinessObject.IsValid());
            Assert.IsFalse(controlWin.IsDirty);
            TestUtil.AssertStringNotEmpty(person.Surname, "person.Surname");
            //---------------Execute Test ----------------------
            SetSurnameTextBoxToNull(controlWin);
            controlWin.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            TestUtil.AssertStringEmpty(person.Surname, "person.Surname");
        }

        [Test]
        public virtual void Test_IsDirty_WhenBONull_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            TestBOEditorControl.GetCustomClassDef();
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
                Assert.AreEqual("", fieldInfo.ControlMapper.GetErrorMessage(), "Errors should be cleared");
            }
        }

        protected static IClassDef GetCustomClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            IClassDef classDef = OrganisationTestBO.LoadDefaultClassDef();
            IUIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = ((UIGrid)originalGridDef).Clone();
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, null, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }
    }

  
}