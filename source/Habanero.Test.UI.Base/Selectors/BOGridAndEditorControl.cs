//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
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
    public class TestBOGridAndEditorControlWin
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";
        private static IClassDef GetCustomClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            IClassDef classDef = OrganisationTestBO.LoadDefaultClassDef();
            IUIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = ((UIGrid)originalGridDef).Clone();
            // UIGridColumn extraColumn = originalGridDef[0].Clone();
            // extraGridDef.Add(extraColumn);
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            // UIGridColumn extraColumn = new UIGridColumn("HABANERO_OBJECTID", "ProjectAssemblyInfoID", typeof(System.Windows.Forms.DataGridViewTextBoxColumn), true, 100, UIGridColumn.PropAlignment.right, null);
            // extraGridDef.Add(extraColum
            IUIForm originalformDef = classDef.UIDefCol["default"].UIForm;
            IUIForm formDef = ((UIForm)originalformDef).Clone();
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, formDef, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private static BOGridAndEditorControlWin<TBusinessObject> CreateGridAndBOEditorControlWinCP<TBusinessObject>() where TBusinessObject : class, IBusinessObject
        {
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            return new BOGridAndEditorControlWin<TBusinessObject>(GetControlFactory(), iboEditorControl);
        }
        private static BOGridAndEditorControlWin<OrganisationTestBO> CreateGridAndBOEditorControlWin()
        {
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            return new BOGridAndEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
        }

        //private static BOGridAndEditorControlWin<ProjectAssemblyInfo> CreateGridAndBOEditorControlWin_ProjectAssemblyInfos()
        //{
        //    IBOEditorControl businessObjectControl = new TestComboBox.BusinessObjectControlStub();
        //    return new BOGridAndEditorControlWin<ProjectAssemblyInfo>(GetControlFactory(), businessObjectControl);
        //}

        private static void AssertSelectedBusinessObject
            (OrganisationTestBO businessObjectInfo,
             BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin)
        {
            Assert.AreSame(businessObjectInfo, andBOGridAndEditorControlWin.GridControl.SelectedBusinessObject);
            Assert.AreSame
                (businessObjectInfo, andBOGridAndEditorControlWin.IBOEditorControl.BusinessObject,
                 "Selected BO in Grid should be loaded in the BoControl");
            Assert.AreSame(businessObjectInfo, andBOGridAndEditorControlWin.CurrentBusinessObject);
        }

        private static IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestConstructor_FailsIfBOControlNull()
        {
            // ---------------Set up test pack-------------------
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), (IBOEditorControl) null);

                Assert.Fail("Null BOControl should be prevented");
            }
                //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("iboEditorControl", ex.ParamName);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestConstructor_FailsIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlWin<OrganisationTestBO>(null, iboEditorControl);

                Assert.Fail("Null ControlFactory should be prevented");
            }
                // ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
            //   ---------------Test Result -----------------------
        }

        [Test]
        [Ignore("Brett is working on this : Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric()
        {
            //---------------Set up test pack-------------------
            ClassDef def = (ClassDef) GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME));
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME).UIForm);
            //---------------Execute Test ----------------------
            IBOGridAndEditorControl iboGridAndEditorControlWin = new BOGridAndEditorControlWin
                (GetControlFactory(), def, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, iboGridAndEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), iboGridAndEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), iboGridAndEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), iboGridAndEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), iboGridAndEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, iboGridAndEditorControlWin.IBOEditorControl);
            Assert.IsFalse(iboEditorControl.Enabled);
        }

        [Test]
        public void TestConstructor_NonGeneric_NullClassDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlWin(GetControlFactory(), (ClassDef) null, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("classDef", ex.ParamName);
            }
        }

        [Test]
        public void TestConstructor_NonGeneric_NullControlFactory_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlWin(null, def, CUSTOM_UIDEF_NAME);
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
        [Ignore("Brett is working on this : Brett 03 Mar 2009:")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric_NoFormDefDefinedForUIDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOGridAndEditorControlWin(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'BusinessObjectControl";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME
                                           + "' in the classDef '" + def.ClassNameFull
                                           + "' does not have a UIForm defined";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
//
//                string expectedDeveloperMessage = "The 'BusinessObjectControl' could not be created since the the uiDef '" + CUSTOM_UIDEF_NAME +
//                                        "' in the classDef '" + def.ClassNameFull + "' does not have a UIForm defined";
//                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Ignore(" In the process of removing this class")] //TODO  15 Mar 2009:
        [Test]
        public void TestConstructor_NonGeneric_DefinedUIDefDoesNotExistForDef_ShouldRiaseError()
        {
            //---------------Set up test pack-------------------
            IClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string uidDefDoesnotexist = "";
            try
            {
                uidDefDoesnotexist = "DoesNotExist";
                new BOGridAndEditorControlWin(GetControlFactory(), def, uidDefDoesnotexist);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string expectedDeveloperMessage = "The 'BusinessObjectControl";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
                expectedDeveloperMessage = "' could not be created since the the uiDef '" + uidDefDoesnotexist
                                           + "' does not exist in the classDef for '" + def.ClassNameFull + "'";
                StringAssert.Contains(expectedDeveloperMessage, ex.Message);
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, andBOGridAndEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), andBOGridAndEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), andBOGridAndEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), andBOGridAndEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), andBOGridAndEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, andBOGridAndEditorControlWin.IBOEditorControl);
            Assert.IsFalse(iboEditorControl.Enabled);
        }

        [Test]
        public void TestConstructor_UsingCustomUIDefName()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, andBOGridAndEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), andBOGridAndEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), andBOGridAndEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), andBOGridAndEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), andBOGridAndEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, andBOGridAndEditorControlWin.IBOEditorControl);
        }

        public static int GetGridWidthToFitColumns(IGridBase grid)
        {
            int width = 0;
            if (grid.RowHeadersVisible)
            {
                width = grid.RowHeadersWidth;
            }
            foreach (IDataGridViewColumn column in grid.Columns)
            {
                if (column.Visible) width += column.Width;
            }
            return width;
        }

        [Test]
        public void TestGridConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            // ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = andBOGridAndEditorControlWin.GridControl;
            Assert.IsNotNull(readOnlyGridControl);
            Assert.IsFalse(readOnlyGridControl.Buttons.Visible);
            Assert.IsFalse(readOnlyGridControl.FilterControl.Visible);
            Assert.IsNull(readOnlyGridControl.Grid.BusinessObjectCollection);
            int expectedWidth = GetGridWidthToFitColumns(readOnlyGridControl.Grid) + 2;
            Assert.AreEqual(expectedWidth, readOnlyGridControl.Width);
        }

        [Test]
        public void TestGridWithCustomClassDef()
        {
            //  ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IClassDef classDef = GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            organisationTestBOS.ClassDef = classDef;
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();

            // ---------------Assert Precondition----------------
            Assert.IsTrue(classDef.UIDefCol.Count >= 2);
            // ---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, andBOGridAndEditorControlWin.GridControl.UiDefName);
        }


        private static BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOSCollection()
        {
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                new BusinessObjectCollection<OrganisationTestBO>();
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            return organisationTestBOS;
        }

        [Test]
        public void TestButtonControlConstruction()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //  ---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IButtonGroupControl buttonGroupControl = andBOGridAndEditorControlWin.ButtonGroupControl;
            Assert.IsNotNull(buttonGroupControl);
            Assert.AreEqual(4, buttonGroupControl.Controls.Count);
            Assert.AreEqual("Cancel", buttonGroupControl.Controls[0].Text);
            Assert.AreEqual("Save", buttonGroupControl.Controls[1].Text);
            Assert.AreEqual("Delete", buttonGroupControl.Controls[2].Text);
            Assert.AreEqual("New", buttonGroupControl.Controls[3].Text);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_InitialSelection_NoItems()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = andBOGridAndEditorControlWin.GridControl;
            Assert.AreEqual(businessObjectInfos.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            // ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlWin);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            IGridControl readOnlyGridControl = andBOGridAndEditorControlWin.GridControl;
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlWin);
            // ---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = businessObjectInfos[1];
            //  ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[1], andBOGridAndEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            Assert.AreEqual(businessObjectInfos.Count, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            // ---------------Execute Test ----------------------
            try
            {
                andBOGridAndEditorControlWin.BusinessObjectCollection = null;
                //   ---------------Test Result -----------------------
                Assert.Fail("Error should have been thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("value", ex.ParamName);
            }
        }

        [Test]
        public void TestBOControlDisabledWhenGridIsCleared()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            //   ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlWin);
            Assert.IsFalse(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlWin);
            Assert.IsFalse(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestNewButtonDisabledUntilCollectionSet()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];
            //  ---------------Assert Precondition----------------
            Assert.IsFalse(newButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(0, businessObjectInfos.Count);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(1, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlWin);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsTrue(andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_ExistingCollection()
        {
            //---------------Set up test pack-------------------
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            IFormHabanero form = GetControlFactory().CreateForm();
            form.Controls.Add(andBOGridAndEditorControlWin);
            form.Show();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(4, organisationTestBOS.Count);
            Assert.IsFalse(andBOGridAndEditorControlWin.IBOEditorControl.Focused);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.ButtonGroupControl["New"].PerformClick();

            // ---------------Test Result -----------------------
            Assert.AreEqual(5, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(5, organisationTestBOS.Count);
            Assert.IsTrue(organisationTestBOS[4].Status.IsNew);
            AssertSelectedBusinessObject(organisationTestBOS[4], andBOGridAndEditorControlWin);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            // TODO: this line passes on PC's, but not when run on the server
            //Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Focused);
            //Assert.IsTrue(andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledAtConstruction()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //  ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin =
                new BOGridAndEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenBOSelected()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], andBOGridAndEditorControlWin);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenControlHasNoBO()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(null, andBOGridAndEditorControlWin);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenOldObjectSelected()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDeletesCurrentBO()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, andBOGridAndEditorControlWin);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.AreEqual(4, organisationTestBOS.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(currentBO.Status.IsDeleted);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.AreEqual(3, organisationTestBOS.Count);
            Assert.IsFalse(organisationTestBOS.Contains(currentBO));
        }

        [Test]
        public void TestDeleteButton_ControlsUpdated()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            OrganisationTestBO otherBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, andBOGridAndEditorControlWin);
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, andBOGridAndEditorControlWin);
            Assert.AreEqual(3, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
        }

        //         Tests a unique set of circumstances
        [Test]
        public void TestDeleteSelectsPreviousRow_NewTypeNewCancelDelete()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];
            IButton deleteButton = andBOGridAndEditorControlWin.ButtonGroupControl["Delete"];
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
#pragma warning disable 168
            OrganisationTestBO currentBO = andBOGridAndEditorControlWin.CurrentBusinessObject;
#pragma warning restore 168
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            organisationTestBOS.SaveAll();
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            newButton.PerformClick();
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            // Assert.AreSame(currentBO, BOGridAndEditorControlWin.CurrentBusinessObject);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = businessObjectInfos[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Ignore(" This is currently not doing anything - what should it be doing: Brett 03 Mar 2009:")] //Brett  27 Feb 2009:
        [Test]
        public void TestCancelButton_EnabledWhenObjectEdited()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
//            IButton cancelButton = BOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            // ---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            // ---------------Execute Test ----------------------
            //currentBO.BusinessObjectName = TestUtils.GetRandomString();
            // ---------------Test Result -----------------------
            //Assert.IsTrue(currentBO.Status.IsDirty);
            //            Assert.IsTrue(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRestoresSavedObject()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];

            OrganisationTestBO currentBO = businessObjectInfos[0];

            //  ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> collection = new BusinessObjectCollection<OrganisationTestBO>();
            andBOGridAndEditorControlWin.BusinessObjectCollection = collection;
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, collection.Count, "The new item should be removed from the collection");
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
        } 
        
        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid_CompositionRelationship()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef organisationClassDef = OrganisationTestBO.LoadDefaultClassDef();
            IRelationshipDef cpRelationship = organisationClassDef.RelationshipDefCol["ContactPeople"];
            cpRelationship.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<ContactPersonTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWinCP<ContactPersonTestBO>();
            BusinessObjectCollection<ContactPersonTestBO> people = new OrganisationTestBO().ContactPeople;
            andBOGridAndEditorControlWin.BusinessObjectCollection = people;
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, people.Count, "The cancelled item should be removed from the collection");
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton cancelButton = andBOGridAndEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            // ---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(5, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            //   ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, andBOGridAndEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(andBOGridAndEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsNotNull(andBOGridAndEditorControlWin.GridControl.SelectedBusinessObject);
        }

        [Test]
        public void Test_ObjectSavesWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) andBOGridAndEditorControlWin.IBOEditorControl;
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton newButton = andBOGridAndEditorControlWin.ButtonGroupControl["New"];
            newButton.PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO) andBOGridAndEditorControlWin.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.Status.IsValid());
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreNotSame(currentBO, andBOGridAndEditorControlWin.IBOEditorControl.BusinessObject);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(currentBO.Status.IsNew);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        [Test]
        public void Test_ObjectSavesWhenGridRowChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) andBOGridAndEditorControlWin.IBOEditorControl;
            andBOGridAndEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            OrganisationTestBO firstBO = organisationTestBOS[0];
            OrganisationTestBO secondBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(secondBO.Status.IsDirty);
            Assert.IsFalse(secondBO.Status.IsNew);
            Assert.AreEqual(0, andBOGridAndEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, andBOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            andBOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, andBOGridAndEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, andBOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        [Test]
        public void Test_ObjectSavesWhenSaveButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOGridAndEditorControlWin<OrganisationTestBO> andBOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub)andBOGridAndEditorControlWin.IBOEditorControl;
            andBOGridAndEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton saveButton = andBOGridAndEditorControlWin.ButtonGroupControl["Save"];
            andBOGridAndEditorControlWin.ButtonGroupControl["New"].PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO)andBOGridAndEditorControlWin.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsNotNull(currentBO);
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.Status.IsValid());
            //  ---------------Execute Test ----------------------
            saveButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreSame(currentBO, andBOGridAndEditorControlWin.IBOEditorControl.BusinessObject);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(currentBO.Status.IsNew);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        //        [Test]
        //        public void Test_CannotChangeGridRowIfCurrentObjectInvalid()
        //        {
        //            ---------------Set up test pack-------------------
        //            BusinessObjectCollection<BusinessObjectInfo> businessObjectInfos = TestUtils.CreateSavedBusinessObjectInfosCollection();
        //            BOGridAndEditorControlWin<BusinessObjectInfo> BOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOGridAndEditorControlWin.BusinessObjectControl;
        //            BOGridAndEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
        //            BusinessObjectInfo firstBO = businessObjectInfos[0];
        //            firstBO.BusinessObjectName = null;
        //            BusinessObjectInfo secondBO = businessObjectInfos[1];
        //            ---------------Assert Precondition----------------
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsFalse(firstBO.Status.IsNew);
        //            Assert.IsFalse(firstBO.IsValid());
        //            Assert.AreEqual(0, BOGridAndEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            BOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
        //            ---------------Test Result -----------------------
        //            Assert.AreEqual(0, BOGridAndEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOGridAndEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsTrue(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        //        {
        //            ---------------Set up test pack-------------------
        //            BOGridAndEditorControlWin<BusinessObjectInfo> BOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOGridAndEditorControlWin.BusinessObjectControl;
        //            BOGridAndEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOGridAndEditorControlWin.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_ClearErrorsWhenNewObjectAdded()
        //        {
        //            ---------------Set up test pack-------------------
        //            BOGridAndEditorControlWin<BusinessObjectInfo> BOGridAndEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOGridAndEditorControlWin.BusinessObjectControl;
        //            BOGridAndEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOGridAndEditorControlWin.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.ClearErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsTrue(boControl.ClearErrorsCalled);
        //        }
    }
}