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
    public class TestBusinessObjectControl
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

        [Test]
        public void TestConstructor_Generic_NullUIDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl<OrganisationTestBO>(GetControlFactory(), null);

                Assert.Fail("Null uiDefName should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("uiDefName", ex.ParamName);
            }
        }

        [Test]
        public void TestConstructor_Generic_NullControlFactory_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl<OrganisationTestBO>(null, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void TestConstructor_Generic_uiDefDoesNotHaveAUIForm_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl<OrganisationTestBO>(GetControlFactory(), CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {

                string expectedDeveloperMessage = "The 'BusinessObjectControl' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME +
                                                  "' in the classDef '" + def.ClassNameFull + "' does not have a UIForm defined";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }
        [Test]
        public void TestConstructor_NullUIDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl(GetControlFactory(), def, null);

                Assert.Fail("Null uiDefName should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("uiDefName", ex.ParamName);
            }
        }
        [Test]
        public void TestConstructor_NullControlFactory_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl(null, def, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
        }
        [Test]
        public void TestConstructor_NullClassDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl(GetControlFactory(), null, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("classDef", ex.ParamName);
            }
        }
        [Test]
        public void TestConstructor_uiDefDoesNotHaveAUIForm_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BusinessObjectControl(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {

                string expectedDeveloperMessage = "The 'BusinessObjectControl' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME +
                                                  "' in the classDef '" + def.ClassNameFull + "' does not have a UIForm defined";
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
            BusinessObjectControl control = new BusinessObjectControl(def);
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);

        }

        [Test]
        public void Test_setBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectControl control = new BusinessObjectControl(GetCustomClassDef());
            OrganisationTestBO businessObject = OrganisationTestBO.CreateSavedOrganisation();
            //---------------Assert Precondition----------------
            Assert.IsNull(control.BusinessObject);
            // ---------------Execute Test ----------------------
            control.BusinessObject = businessObject;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(businessObject, control.BusinessObject);
        }
        [Test]
        public void Test_SetBusinessObject_Null()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectControl control = new BusinessObjectControl(GetCustomClassDef());
            OrganisationTestBO businessObject = OrganisationTestBO.CreateSavedOrganisation();
            control.BusinessObject = businessObject;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(control.BusinessObject);
            // ---------------Execute Test ----------------------
            control.BusinessObject = null;
            //  ---------------Test Result -----------------------
            Assert.IsNull(control.BusinessObject);
//            Assert.IsFalse(control.Enabled); Brett 2009/02/28 think about this should this control disable itself if bo set to null
        }
        [Test]
        public void Test_SetBusinessObject_Null_DisplayErrors()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectControl control = new BusinessObjectControl(GetCustomClassDef());
            control.BusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(control.BusinessObject);
            // ---------------Execute Test ----------------------
            control.DisplayErrors();
            //  ---------------Test Result -----------------------
            Assert.IsNull(control.BusinessObject);
        }
        [Test]
        public void Test_SetBusinessObject_Null_ClearErrors()
        {
            //   ---------------Set up test pack-------------------
            GlobalUIRegistry.ControlFactory = GetControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectControl control = new BusinessObjectControl(GetCustomClassDef());
            control.BusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(control.BusinessObject);
            // ---------------Execute Test ----------------------
            control.ClearErrors();
            //  ---------------Test Result -----------------------
            Assert.IsNull(control.BusinessObject);
        }
    }
}