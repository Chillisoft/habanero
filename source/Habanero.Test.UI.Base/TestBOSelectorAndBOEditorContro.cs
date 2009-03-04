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
    public class TestBOSelectorAndBOEditorControlWin
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";
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
            // extraGridDef.Add(extraColum
            UIForm originalformDef = classDef.UIDefCol["default"].UIForm;
            UIForm formDef = originalformDef.Clone();
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, formDef, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private static BOSelectorAndBOEditorControlWin<TBusinessObject> CreateGridAndBOEditorControlWinCP<TBusinessObject>() where TBusinessObject : class, IBusinessObject
        {
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            return new BOSelectorAndBOEditorControlWin<TBusinessObject>(GetControlFactory(), iboEditorControl);
        }
        private static BOSelectorAndBOEditorControlWin<OrganisationTestBO> CreateGridAndBOEditorControlWin()
        {
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            return new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
        }

        //private static BOSelectorAndBOEditorControlWin<ProjectAssemblyInfo> CreateGridAndBOEditorControlWin_ProjectAssemblyInfos()
        //{
        //    IBOEditorControl businessObjectControl = new TestComboBox.BusinessObjectControlStub();
        //    return new BOSelectorAndBOEditorControlWin<ProjectAssemblyInfo>(GetControlFactory(), businessObjectControl);
        //}

        private static void AssertSelectedBusinessObject
            (OrganisationTestBO businessObjectInfo,
             BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin)
        {
            Assert.AreSame(businessObjectInfo, boSelectorAndBOEditorControlWin.GridControl.SelectedBusinessObject);
            Assert.AreSame
                (businessObjectInfo, boSelectorAndBOEditorControlWin.IBOEditorControl.BusinessObject,
                 "Selected BO in Grid should be loaded in the BoControl");
            Assert.AreSame(businessObjectInfo, boSelectorAndBOEditorControlWin.CurrentBusinessObject);
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
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>
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
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(null, iboEditorControl);

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
        [Ignore("Brett is working on this")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            IBOEditorControl iboEditorControl = new TestComboBox.BusinessObjectControlStub();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME));
            Assert.IsNotNull(def.GetUIDef(CUSTOM_UIDEF_NAME).UIForm);
            //---------------Execute Test ----------------------
            IBOSelectorAndEditor iboSelectorAndEditorWin = new BOSelectorAndBOEditorControlWin
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
                new BOSelectorAndBOEditorControlWin(GetControlFactory(), (ClassDef) null, CUSTOM_UIDEF_NAME);
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
                new BOSelectorAndBOEditorControlWin(null, def, CUSTOM_UIDEF_NAME);
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
        [Ignore("Brett is working on this")] //TODO Brett 03 Mar 2009: Brett is working on this
        public void TestConstructor_NonGeneric_NoFormDefDefinedForUIDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ClassDef def = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOSelectorAndBOEditorControlWin(GetControlFactory(), def, CUSTOM_UIDEF_NAME);
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
                new BOSelectorAndBOEditorControlWin(GetControlFactory(), def, uidDefDoesnotexist);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, boSelectorAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), boSelectorAndBOEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), boSelectorAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), boSelectorAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), boSelectorAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, boSelectorAndBOEditorControlWin.IBOEditorControl);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, boSelectorAndBOEditorControlWin.Controls.Count);
            Assert.IsInstanceOfType(typeof (IUserControlHabanero), boSelectorAndBOEditorControlWin);
            Assert.IsInstanceOfType
                (typeof (IBOEditorControl), boSelectorAndBOEditorControlWin.Controls[0]);
            Assert.IsInstanceOfType(typeof (IReadOnlyGridControl), boSelectorAndBOEditorControlWin.Controls[1]);
            Assert.IsInstanceOfType(typeof (IButtonGroupControl), boSelectorAndBOEditorControlWin.Controls[2]);
            Assert.AreSame(iboEditorControl, boSelectorAndBOEditorControlWin.IBOEditorControl);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = boSelectorAndBOEditorControlWin.GridControl;
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>
                    (GetControlFactory(), iboEditorControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, boSelectorAndBOEditorControlWin.GridControl.UiDefName);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //  ---------------Test Result -----------------------
            IButtonGroupControl buttonGroupControl = boSelectorAndBOEditorControlWin.ButtonGroupControl;
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            IGridControl readOnlyGridControl = boSelectorAndBOEditorControlWin.GridControl;
            Assert.AreEqual(businessObjectInfos.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, boSelectorAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            // ---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GetCustomClassDef();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            // ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], boSelectorAndBOEditorControlWin);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //   ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IGridControl readOnlyGridControl = boSelectorAndBOEditorControlWin.GridControl;
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], boSelectorAndBOEditorControlWin);
            // ---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = businessObjectInfos[1];
            //  ---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[1], boSelectorAndBOEditorControlWin);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            Assert.AreEqual(businessObjectInfos.Count, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            // ---------------Execute Test ----------------------
            try
            {
                boSelectorAndBOEditorControlWin.BusinessObjectCollection = null;
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            //   ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, boSelectorAndBOEditorControlWin);
            Assert.IsFalse(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(4, businessObjectInfos.Count);
            AssertSelectedBusinessObject(null, boSelectorAndBOEditorControlWin);
            Assert.IsFalse(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestNewButtonDisabledUntilCollectionSet()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
            //  ---------------Assert Precondition----------------
            Assert.IsFalse(newButton.Enabled);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //  ---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos =
                new BusinessObjectCollection<OrganisationTestBO>();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Assert Precondition----------------
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(0, businessObjectInfos.Count);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(1, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(1, businessObjectInfos.Count);
            AssertSelectedBusinessObject(businessObjectInfos[0], boSelectorAndBOEditorControlWin);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_ExistingCollection()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IFormHabanero form = GetControlFactory().CreateForm();
            form.Controls.Add(boSelectorAndBOEditorControlWin);
            form.Show();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            //   ---------------Assert Precondition----------------
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(4, organisationTestBOS.Count);
            Assert.IsFalse(boSelectorAndBOEditorControlWin.IBOEditorControl.Focused);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreEqual(5, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.AreEqual(5, organisationTestBOS.Count);
            Assert.IsTrue(organisationTestBOS[4].Status.IsNew);
            AssertSelectedBusinessObject(organisationTestBOS[4], boSelectorAndBOEditorControlWin);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Focused);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"].Enabled);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin =
                new BOSelectorAndBOEditorControlWin<OrganisationTestBO>(GetControlFactory(), iboEditorControl);
            //---------------Test Result -----------------------
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(businessObjectInfos[0], boSelectorAndBOEditorControlWin);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            // ---------------Test Result -----------------------
            AssertSelectedBusinessObject(null, boSelectorAndBOEditorControlWin);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(1, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            // ---------------Assert Precondition----------------
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //  ---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, boSelectorAndBOEditorControlWin);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            OrganisationTestBO currentBO = organisationTestBOS[0];
            OrganisationTestBO otherBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, boSelectorAndBOEditorControlWin);
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, boSelectorAndBOEditorControlWin);
            Assert.AreEqual(3, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
            IButton deleteButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Delete"];
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
#pragma warning disable 168
            OrganisationTestBO currentBO = boSelectorAndBOEditorControlWin.CurrentBusinessObject;
#pragma warning restore 168
            //---------------Assert Precondition----------------
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            organisationTestBOS.SaveAll();
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            newButton.PerformClick();
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            // Assert.AreSame(currentBO, BOSelectorAndBOEditorControlWin.CurrentBusinessObject);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //  ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> businessObjectInfos = CreateSavedOrganisationTestBOSCollection
                ();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
//            IButton cancelButton = BOSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = businessObjectInfos;
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];

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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            BusinessObjectCollection<OrganisationTestBO> collection = new BusinessObjectCollection<OrganisationTestBO>();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = collection;
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, collection.Count, "The new item should be removed from the collection");
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
        } 
        
        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid_CompositionRelationship()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef organisationClassDef = OrganisationTestBO.LoadDefaultClassDef();
            IRelationshipDef cpRelationship = organisationClassDef.RelationshipDefCol["ContactPeople"];
            cpRelationship.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();

            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<ContactPersonTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWinCP<ContactPersonTestBO>();
            BusinessObjectCollection<ContactPersonTestBO> people = new OrganisationTestBO().ContactPeople;
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = people;
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, people.Count, "The cancelled item should be removed from the collection");
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsFalse(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            // ---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection
                ();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            IButton cancelButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Cancel"];
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];

            newButton.PerformClick();
            // ---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(5, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            //   ---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //  ---------------Test Result -----------------------
            Assert.AreEqual(4, boSelectorAndBOEditorControlWin.GridControl.Grid.Rows.Count);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.IBOEditorControl.Enabled);
            Assert.IsNotNull(boSelectorAndBOEditorControlWin.GridControl.SelectedBusinessObject);
        }

        [Test]
        public void Test_ObjectSavesWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) boSelectorAndBOEditorControlWin.IBOEditorControl;
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton newButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
            newButton.PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO) boSelectorAndBOEditorControlWin.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.IsValid());
            //  ---------------Execute Test ----------------------
            newButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreNotSame(currentBO, boSelectorAndBOEditorControlWin.IBOEditorControl.BusinessObject);
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
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub) boSelectorAndBOEditorControlWin.IBOEditorControl;
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = organisationTestBOS;
            OrganisationTestBO firstBO = organisationTestBOS[0];
            OrganisationTestBO secondBO = organisationTestBOS[1];
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(secondBO.Status.IsDirty);
            Assert.IsFalse(secondBO.Status.IsNew);
            Assert.AreEqual(0, boSelectorAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, boSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            boSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boSelectorAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, boSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsFalse(boControl.DisplayErrorsCalled);
        }

        [Test]
        public void Test_ObjectSavesWhenSaveButtonClicked()
        {
            //---------------Set up test pack-------------------
            GetCustomClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BOSelectorAndBOEditorControlWin<OrganisationTestBO> boSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
            TestComboBox.BusinessObjectControlStub boControl =
                (TestComboBox.BusinessObjectControlStub)boSelectorAndBOEditorControlWin.IBOEditorControl;
            boSelectorAndBOEditorControlWin.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            IButton saveButton = boSelectorAndBOEditorControlWin.ButtonGroupControl["Save"];
            boSelectorAndBOEditorControlWin.ButtonGroupControl["New"].PerformClick();
            OrganisationTestBO currentBO =
                (OrganisationTestBO)boSelectorAndBOEditorControlWin.IBOEditorControl.BusinessObject;

            //---------------Assert Precondition----------------
            Assert.IsNotNull(currentBO);
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.IsValid());
            //  ---------------Execute Test ----------------------
            saveButton.PerformClick();
            // ---------------Test Result -----------------------
            Assert.AreSame(currentBO, boSelectorAndBOEditorControlWin.IBOEditorControl.BusinessObject);
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
        //            BOSelectorAndBOEditorControlWin<BusinessObjectInfo> BOSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOSelectorAndBOEditorControlWin.BusinessObjectControl;
        //            BOSelectorAndBOEditorControlWin.SetBusinessObjectCollection(businessObjectInfos);
        //            BusinessObjectInfo firstBO = businessObjectInfos[0];
        //            firstBO.BusinessObjectName = null;
        //            BusinessObjectInfo secondBO = businessObjectInfos[1];
        //            ---------------Assert Precondition----------------
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsFalse(firstBO.Status.IsNew);
        //            Assert.IsFalse(firstBO.IsValid());
        //            Assert.AreEqual(0, BOSelectorAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsFalse(boControl.DisplayErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            BOSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject = secondBO;
        //            ---------------Test Result -----------------------
        //            Assert.AreEqual(0, BOSelectorAndBOEditorControlWin.GridControl.Grid.SelectedRows[0].Index);
        //            Assert.AreSame(firstBO, BOSelectorAndBOEditorControlWin.GridControl.Grid.SelectedBusinessObject);
        //            Assert.IsTrue(firstBO.Status.IsDirty);
        //            Assert.IsTrue(boControl.DisplayErrorsCalled);
        //        }

        //        [Test]
        //        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        //        {
        //            ---------------Set up test pack-------------------
        //            BOSelectorAndBOEditorControlWin<BusinessObjectInfo> BOSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOSelectorAndBOEditorControlWin.BusinessObjectControl;
        //            BOSelectorAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
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
        //            BOSelectorAndBOEditorControlWin<BusinessObjectInfo> BOSelectorAndBOEditorControlWin = CreateGridAndBOEditorControlWin();
        //           TestComboBox.BusinessObjectControlStub boControl = (BusinessObjectControlStub)BOSelectorAndBOEditorControlWin.BusinessObjectControl;
        //            BOSelectorAndBOEditorControlWin.SetBusinessObjectCollection(new BusinessObjectCollection<BusinessObjectInfo>());
        //            IButton newButton = BOSelectorAndBOEditorControlWin.ButtonGroupControl["New"];
        //            ---------------Assert Precondition----------------
        //            Assert.IsFalse(boControl.ClearErrorsCalled);
        //            ---------------Execute Test ----------------------
        //            newButton.PerformClick();
        //            ---------------Test Result -----------------------
        //            Assert.IsTrue(boControl.ClearErrorsCalled);
        //        }
    }
}