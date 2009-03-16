using System;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestBOEditorControl_Generic : TestBOEditorControl
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

        protected override IBOPanelEditorControl CreateEditorControl(ClassDef classDef)
        {
            return new BOEditorControlWin<ContactPersonTestBO>();
        }

        [Test]
        public override void TestConstructor_NullUIDef_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin<OrganisationTestBO>(GetControlFactory(), null);

                Assert.Fail("Null uiDefName should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("uiDefName", ex.ParamName);
            }
        }

        [Test]
        public override void TestConstructor_NullControlFactory_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin<OrganisationTestBO>(null, CUSTOM_UIDEF_NAME);

                Assert.Fail("Null controlFactory should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public override void TestConstructor_uiDefDoesNotHaveAUIForm_ShouldRaiseError()
        {
            // ---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOEditorControlWin<OrganisationTestBO>(GetControlFactory(), CUSTOM_UIDEF_NAME);
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
    }
}