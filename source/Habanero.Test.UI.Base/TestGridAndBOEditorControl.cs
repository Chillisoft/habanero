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
    public class TestGridAndBOEditorControlWin
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";

        //        [SetUp]
        //        public void SetupTest()
        //        {
        //            //Runs every time that any testmethod is executed
        //            base.SetupTest();
        //        }

        //        [TestFixtureSetUp]
        //        public void TestFixtureSetup()
        //        {
        //            //Code that is executed before any test is run in this class. If multiple tests
        //            // are executed then it will still only be called once.
        //        }

        //        [TearDown]
        //        public void TearDownTest()
        //        {
        //            //runs every time any testmethod is complete
        //            base.TearDownTest();
        //        }

        //         Creates a new UI def by cloning an existing one and adding a cloned column
        //           (easier than creating a whole new BO for this test)


        private static ClassDef GetCustomClassDef()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            ClassDef classDef = OrganisationTestBO.LoadDefaultClassDef();
            UIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = originalGridDef.Clone();
            // UIGridColumn extraColumn = originalGridDef[0].Clone();
            // extraGridDef.Add(extraColumn);
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            // UIGridColumn extraColumn = new UIGridColumn("HABANERO_OBJECTID", "ProjectAssemblyInfoID", typeof(System.Windows.Forms.DataGridViewTextBoxColumn), true, 100, UIGridColumn.PropAlignment.right, null);
            // extraGridDef.Add(extraColumn);
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, null, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private static GridAndBOEditorControlWin<OrganisationTestBO> CreateGridAndBOEditorControlWin()
        {
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            return new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
        }

        //private static GridAndBOEditorControlWin<ProjectAssemblyInfo> CreateGridAndBOEditorControlWin_ProjectAssemblyInfos()
        //{
        //    IBOEditorControl businessObjectControl = new TestComboBox.BusinessObjectControlStub();
        //    return new GridAndBOEditorControlWin<ProjectAssemblyInfo>(GetControlFactory(), businessObjectControl);
        //}

        private static void AssertSelectedBusinessObject
            (OrganisationTestBO businessObjectInfo,
             GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin)
        {
            Assert.AreSame(businessObjectInfo, GridAndBOEditorControlWin.GridControl.SelectedBusinessObject);
            Assert.AreSame
                (businessObjectInfo, GridAndBOEditorControlWin.IBOEditorControl.BusinessObject,
                 "Selected BO in Grid should be loaded in the BoControl");
            Assert.AreSame(businessObjectInfo, GridAndBOEditorControlWin.CurrentBusinessObject);
        }

        private static IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestConstructor_FailsIfBOControlNull()
        {
            // ---------------Set up test pack-------------------
            // ---------------Assert Precondition----------------
            // ---------------Execute Test ----------------------
            try
            {
                new GridAndBOEditorControlWin<OrganisationTestBO>
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
                new GridAndBOEditorControlWin<OrganisationTestBO>(null, iboEditorControl);

                Assert.Fail("Null ControlFactory should be prevented");
            }
                // ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("controlFactory", ex.ParamName);
            }
            //   ---------------Test Result -----------------------
        }

        [Ignore(" Currently working on this")] //Brett 26 Feb 2009:
        [Test]
        public void TestConstructor_NonGeneric()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME));
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME).UIForm);
            //---------------Execute Test ----------------------
            IBOSelectorAndEditor iboSelectorAndEditorWin = new GridAndBOEditorControlWin
                (GetControlFactory(), def, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, iboSelectorAndEditorWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), iboSelectorAndEditorWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), iboSelectorAndEditorWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), iboSelectorAndEditorWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), iboSelectorAndEditorWin.Controls[2]);
            Assert.AreSame(iboEditorControl, iboSelectorAndEditorWin.IBOEditorControl);
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
                new GridAndBOEditorControlWin(GetControlFactory(), (ClassDef) null, CUSTOM_UIDEF_NAME);
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
            ClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new GridAndBOEditorControlWin(null, def, CUSTOM_UIDEF_NAME);
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
        public void TestConstructor_NonGeneric_NoFormDefDefinedForUIDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new GridAndBOEditorControlWin(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
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

        [Test]
        public void TestConstructor_NonGeneric_DefinedUIDefDoesNotExistForDef_ShouldRiaseError()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string uidDefDoesnotexist = "";
            try
            {
                uidDefDoesnotexist = "DoesNotExist";
                new GridAndBOEditorControlWin(GetControlFactory(), def, uidDefDoesnotexist);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, GridAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), GridAndBOEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), GridAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), GridAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), GridAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, GridAndBOEditorControlWin.IBOEditorControl);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, GridAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), GridAndBOEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), GridAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), GridAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), GridAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, GridAndBOEditorControlWin.IBOEditorControl);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = GridAndBOEditorControlWin.GridControl;
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
            ClassDef classDef = GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            organisationTestBOS.ClassDef = classDef;
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();

            // ---------------Assert Precondition----------------
            Assert.IsTrue(classDef.UIDefCol.Count >= 2);
            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, gridAndBOEditorControlWin.GridControl.UiDefName);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IButtonGroupControl buttonGroupControl = GridAndBOEditorControlWin.ButtonGroupControl;
            Assert.IsNotNull(buttonGroupControl);
            Assert.AreEqual(3, buttonGroupControl.Controls.Count);
            Assert.AreEqual("Cancel", buttonGroupControl.Controls[0].Text);
            Assert.AreEqual("Delete", buttonGroupControl.Controls[1].Text);
            Assert.AreEqual("New", buttonGroupControl.Controls[2].Text);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_InitialSelection_NoItems()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            gridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = gridAndBOEditorControlWin.GridControl;
            Assert.AreEqual(businessObjectInfos.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, gridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            // ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], GridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IGridControl readOnlyGridControl = gridAndBOEditorControlWin.GridControl;
            gridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], gridAndBOEditorControlWin);
            // ---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = businessObjectInfos[1];
            //  ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[1], gridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            Assert.AreEqual(businessObjectInfos.Count, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            // ---------------Execute Test ----------------------
            try
            {
                GridAndBOEditorControlWin.BusinessObjectCollection = null;
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            //   ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, GridAndBOEditorControlWin);
            Assert.IsFalse(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(null, GridAndBOEditorControlWin);
            Assert.IsFalse(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestNewButtonDisabledUntilCollectionSet()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            //  ---------------Assert Precondition----------------
            Assert.IsFalse(newButton.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            gridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, gridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(0, businessObjectInfos.Count);
            //  ---------------Execute Test ----------------------
            gridAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(1, gridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(1, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], gridAndBOEditorControlWin);
            Assert.IsTrue(gridAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsTrue(gridAndBOEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_ExistingCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IFormHabanero form = GetControlFactory().CreateForm();
            form.Controls.Add(GridAndBOEditorControlWin);
            form.Show();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(4, organisationTestBOS.Count);
            Assert.IsFalse(GridAndBOEditorControlWin.IBOEditorControl.Focused);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(5, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(5, organisationTestBOS.Count);
            Assert.IsTrue(organisationTestBOS[4].Status.IsNew);
            AssertSelectedBusinessObject(organisationTestBOS[4], GridAndBOEditorControlWin);
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Focused);
            Assert.IsTrue(GridAndBOEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], GridAndBOEditorControlWin);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(null, GridAndBOEditorControlWin);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(1, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, GridAndBOEditorControlWin);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            OrganisationTestBO otherBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, GridAndBOEditorControlWin);
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, GridAndBOEditorControlWin);
            Assert.AreEqual(3, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
#pragma warning disable 168
            OrganisationTestBO currentBO = GridAndBOEditorControlWin.CurrentBusinessObject;
#pragma warning restore 168
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            newButton.PerformClick();
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            // Assert.AreSame(currentBO, GridAndBOEditorControlWin.CurrentBusinessObject);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = businessObjectInfos[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Ignore(" This is currently not doing anything - what should it be doing ")] //Brett  27 Feb 2009:
        [Test]
        public void TestCancelButton_EnabledWhenObjectEdited()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
//            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];

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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            // ---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(5, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
            //   ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsNotNull(GridAndBOEditorControlWin.GridControl.SelectedBusinessObject);
        }

        [Test]
        public void Test_ObjectSavesWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) GridAndBOEditorControlWin.IBOEditorControl;
            GridAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            newButton.PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO) GridAndBOEditorControlWin.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.IsValid());
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreNotSame(currentBO, GridAndBOEditorControlWin.IBOEditorControl.BusinessObject);
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
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) GridAndBOEditorControlWin.IBOEditorControl;
            GridAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            OrganisationTestBO firstBO = organisationTestBOS[0];
            OrganisationTestBO secondBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(secondBO.Status.IsDirty);
            Assert.IsFalse(secondBO.Status.IsNew);
            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, GridAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        //        [Test]
        //        public void Test_CannotChangeGridRowIfCurrentObjectInvalid()
        //        {
        //            ---------------Set up test pack-------------------
        //            BusinessObjectCollection<BusinessObjectInfo> businessObjectInfos = TestUtils.CreateSavedBusinessObjectInfosCollection();
        //            GridAndBOEditorControlWin<BusinessObjectInfo> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
        //            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
        //            BusinessObjectInfo firstBO = businessObjectInfos[0];
        //            firstBO.BusinessObjectName = null;
        //            BusinessObjectInfo secondBO = businessObjectInfos[1];
        //            ---------------Assert Precondition----------------
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsFalse(firstBO.Status.IsNew);
        //            Assert.IsFalse(firstBO.IsValid());
        //            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
        //            ---------------Test Result -----------------------
        //            Assert.AreEqual(0, GridAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, GridAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsTrue(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        //        {
        //            ---------------Set up test pack-------------------
        //            GridAndBOEditorControlWin<BusinessObjectInfo> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
        //            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
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
        //            GridAndBOEditorControlWin<BusinessObjectInfo> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
        //            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.ClearErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsTrue(boControl.ClearErrorsCalled);
        //        }
    }
}