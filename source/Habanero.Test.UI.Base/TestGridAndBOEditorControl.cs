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
using Habanero.Base;
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
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            return new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl);
        }

        //private static GridAndBOEditorControlWin<ProjectAssemblyInfo> CreateGridAndBOEditorControlWin_ProjectAssemblyInfos()
        //{
        //    IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
        //    return new GridAndBOEditorControlWin<ProjectAssemblyInfo>(GetControlFactory(), businessObjectControl);
        //}

        private static void AssertSelectedBusinessObject(OrganisationTestBO businessObjectInfo, GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin)
        {
            Assert.AreSame(businessObjectInfo, GridAndBOEditorControlWin.ReadOnlyGridControl.SelectedBusinessObject);
            Assert.AreSame(businessObjectInfo, GridAndBOEditorControlWin.BusinessObjectControl.BusinessObject, "Selected BO in Grid should be loaded in the BoControl");
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
                GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                    new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), (IBusinessObjectControlWithErrorDisplay)((IBusinessObjectControlWithErrorDisplay)null));

                Assert.Fail("Null BOControl should be prevented");
            }
            //    ---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("businessObjectControl", ex.ParamName);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestConstructor_FailsIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                    new GridAndBOEditorControlWin<OrganisationTestBO>((IControlFactory)((IControlFactory)null), businessObjectControl);

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
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, GridAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof(IUserControlHabanero), GridAndBOEditorControlWin);
            Assert.IsInstanceOfType(typeof(IBusinessObjectControlWithErrorDisplay), GridAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof(IReadOnlyGridControl), GridAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof(IButtonGroupControl), GridAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(businessObjectControl, GridAndBOEditorControlWin.BusinessObjectControl);
            Assert.IsFalse(businessObjectControl.Enabled);
        }

        [Test]
        public void TestConstructor_UsingCustomUIDefName()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, GridAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof(IUserControlHabanero), GridAndBOEditorControlWin);
            Assert.IsInstanceOfType(typeof(IBusinessObjectControlWithErrorDisplay), GridAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof(IReadOnlyGridControl), GridAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof(IButtonGroupControl), GridAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(businessObjectControl, GridAndBOEditorControlWin.BusinessObjectControl);
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
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            // ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl);
            //  ---------------Test Result -----------------------
            IReadOnlyGridControl readOnlyGridControl = GridAndBOEditorControlWin.ReadOnlyGridControl;
            Assert.IsNotNull(readOnlyGridControl);
            Assert.IsFalse(readOnlyGridControl.Buttons.Visible);
            Assert.IsFalse(readOnlyGridControl.FilterControl.Visible);
            Assert.IsNull(readOnlyGridControl.Grid.GetBusinessObjectCollection());
            int expectedWidth = GetGridWidthToFitColumns(readOnlyGridControl.Grid) + 2;
            Assert.AreEqual(expectedWidth, readOnlyGridControl.Width);
        }

        [Test]
        public void TestGridWithCustomClassDef()
        {
            //  ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef classDef = GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            organisationTestBOS.ClassDef = classDef;
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();

            // ---------------Assert Precondition----------------
            Assert.IsTrue(classDef.UIDefCol.Count >= 2);
            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, gridAndBOEditorControlWin.ReadOnlyGridControl.UiDefName);
        }


        private BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOSCollection()
        {
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = new BusinessObjectCollection<OrganisationTestBO>();
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
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin =
                new GridAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), businessObjectControl);
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
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = new BusinessObjectCollection<OrganisationTestBO>();
            GridAndBOEditorControlWin<OrganisationTestBO> gridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            gridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            // ---------------Test Result -----------------------
            IReadOnlyGridControl readOnlyGridControl = gridAndBOEditorControlWin.ReadOnlyGridControl;
            Assert.AreEqual(businessObjectInfos.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, gridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            // ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], GridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
               CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IReadOnlyGridControl readOnlyGridControl = GridAndBOEditorControlWin.ReadOnlyGridControl;
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], GridAndBOEditorControlWin);
            // ---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = businessObjectInfos[1];
            //  ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[1], GridAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //---------------Test Result -----------------------
            Assert.AreEqual(businessObjectInfos.Count, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            // ---------------Execute Test ----------------------
            try
            {
                GridAndBOEditorControlWin.SetBusinessObjectCollection(null);
                //   ---------------Test Result -----------------------
                Assert.Fail("Error should have been thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("col", ex.ParamName);
            }
        }

        [Test]
        public void TestBOControlDisabledWhenGridIsCleared()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            //   ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            //  ---------------Test Result -----------------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, GridAndBOEditorControlWin);
            Assert.IsFalse(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(null, GridAndBOEditorControlWin);
            Assert.IsFalse(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //  ---------------Test Result -----------------------
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
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
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
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
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = new BusinessObjectCollection<OrganisationTestBO>();
            gridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, gridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(0, businessObjectInfos.Count);
            //  ---------------Execute Test ----------------------
            gridAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(1, gridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(1, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], gridAndBOEditorControlWin);
            Assert.IsTrue(gridAndBOEditorControlWin.BusinessObjectControl.Enabled);
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
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(4, organisationTestBOS.Count);
            Assert.IsFalse(GridAndBOEditorControlWin.BusinessObjectControl.Focused);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(5, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(5, organisationTestBOS.Count);
            Assert.IsTrue(organisationTestBOS[4].Status.IsNew);
            AssertSelectedBusinessObject(organisationTestBOS[4], GridAndBOEditorControlWin);
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Focused);
            Assert.IsTrue(GridAndBOEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledAtConstruction()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IBusinessObjectControlWithErrorDisplay businessObjectControl = new BusinessObjectControlStub();
            //  ---------------Assert Precondition----------------

            // ---------------Execute Test ----------------------
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = new GridAndBOEditorControlWin<OrganisationTestBO>(
                GetControlFactory(), businessObjectControl);
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
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
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
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
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
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(1, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenOldObjectSelected()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDeletesCurrentBO()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
               CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
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
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            OrganisationTestBO otherBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, GridAndBOEditorControlWin);
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, GridAndBOEditorControlWin);
            Assert.AreEqual(3, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
        }

        //         Tests a unique set of circumstances
        [Test]
        public void TestDeleteSelectsPreviousRow_NewTypeNewCancelDelete()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            IButton deleteButton = GridAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = GridAndBOEditorControlWin.CurrentBusinessObject;
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            newButton.PerformClick();
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            // Assert.AreSame(currentBO, GridAndBOEditorControlWin.CurrentBusinessObject);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
               CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            OrganisationTestBO currentBO = businessObjectInfos[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_EnabledWhenObjectEdited()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
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
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
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
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS =
                CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            IButton cancelButton = GridAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            // ---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(5, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
            //   ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(GridAndBOEditorControlWin.BusinessObjectControl.Enabled);
            Assert.IsNotNull(GridAndBOEditorControlWin.ReadOnlyGridControl.SelectedBusinessObject);
        }

        [Test]
        public void Test_ObjectSavesWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
            GridAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<OrganisationTestBO>());
            IButton newButton = GridAndBOEditorControlWin.ButtonGroupControl["New"];
            newButton.PerformClick();
            OrganisationTestBO currentBO = (OrganisationTestBO)GridAndBOEditorControlWin.BusinessObjectControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.IsValid());
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreNotSame(currentBO, GridAndBOEditorControlWin.BusinessObjectControl.BusinessObject);
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
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection();
            GridAndBOEditorControlWin<OrganisationTestBO> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
            GridAndBOEditorControlWin.SetBusinessObjectCollection(organisationTestBOS);
            OrganisationTestBO firstBO = organisationTestBOS[0];
            OrganisationTestBO secondBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(secondBO.Status.IsDirty);
            Assert.IsFalse(secondBO.Status.IsNew);
            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        //        [Test]
        //        public void Test_CannotChangeGridRowIfCurrentObjectInvalid()
        //        {
        //            ---------------Set up test pack-------------------
        //            BusinessObjectCollection<BusinessObjectInfo> businessObjectInfos = TestUtils.CreateSavedBusinessObjectInfosCollection();
        //            GridAndBOEditorControlWin<BusinessObjectInfo> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //            BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
        //            GridAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
        //            BusinessObjectInfo firstBO = businessObjectInfos[0];
        //            firstBO.BusinessObjectName = null;
        //            BusinessObjectInfo secondBO = businessObjectInfos[1];
        //            ---------------Assert Precondition----------------
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsFalse(firstBO.Status.IsNew);
        //            Assert.IsFalse(firstBO.IsValid());
        //            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject);
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
        //            ---------------Test Result -----------------------
        //            Assert.AreEqual(0, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, GridAndBOEditorControlWin.ReadOnlyGridControl.Grid.SelectedBusinessObject);
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsTrue(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        //        {
        //            ---------------Set up test pack-------------------
        //            GridAndBOEditorControlWin<BusinessObjectInfo> GridAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //            BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
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
        //            BusinessObjectControlStub boControl = (BusinessObjectControlStub)GridAndBOEditorControlWin.BusinessObjectControl;
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