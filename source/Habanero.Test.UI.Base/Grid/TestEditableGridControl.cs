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
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestEditableGridControl
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            // base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlChilli cntrl);
        protected abstract void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract IEditableGridControl CreateEditableGridControl();

        //[TestFixture]
        //public class TestEditableGridControlWin : TestEditableGridControl
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }

        //    //protected override IGridBase CreateGridBaseStub()
        //    //{
        //    //    GridBaseWinStub gridBase = new GridBaseWinStub();
        //    //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    //    frm.Controls.Add(gridBase);
        //    //    return gridBase;
        //    //}

        //    //private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
        //    //                                                             IGridBase gridBase)
        //    //{
        //    //    System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
        //    //    System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
        //    //    return row.Cells[propName];
        //    //}

        //    //protected override void AddControlToForm(IGridBase gridBase)
        //    //{
        //    //    throw new NotImplementedException();
        //    //}
        //    protected override void AddControlToForm(IControlChilli cntrl)
        //    {
        //        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //        frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        //    }
        //}

        [TestFixture]
        public class TestEditableGridControlGiz : TestEditableGridControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseGizStub gridBase = new GridBaseGizStub();
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            //private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
            //                                                IGridBase gridBase)
            //{
            //    Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView)gridBase;
            //    Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
            //    return row.Cells[propName];
            //}

            //private object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
            //{
            //    Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
            //    return cell.Value;
            //}
            [Test]
            protected override void AddControlToForm(IControlChilli cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
            }


            protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz) dataGridViewColumn;
                Assert.IsInstanceOfType
                    (typeof (Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn), dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz) dataGridViewColumn;
                Assert.IsInstanceOfType
                    (typeof (Gizmox.WebGUI.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz) dataGridViewColumn;
                Assert.IsInstanceOfType
                    (typeof (Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn), dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override IEditableGridControl CreateEditableGridControl()
            {
                EditableGridControlGiz editableGridControlGiz = new EditableGridControlGiz(GetControlFactory());
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(editableGridControlGiz);
                return editableGridControlGiz;
            }

            [Test]
            public void TestGizInitialise_SelectionEditMode()
            {
                //---------------Set up test pack-------------------
                IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
                MyBO.LoadDefaultClassDef();
                ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
                //---------------Execute Test ----------------------
                gridControl.Initialise(def);
                //---------------Test Result -----------------------
                Assert.AreEqual
                    (Gizmox.WebGUI.Forms.DataGridViewSelectionMode.CellSelect,
                     ((Gizmox.WebGUI.Forms.DataGridView) gridControl.Grid).SelectionMode);
                Assert.AreEqual
                    (Gizmox.WebGUI.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2,
                     ((Gizmox.WebGUI.Forms.DataGridView) gridControl.Grid).EditMode);
                //---------------Tear Down -------------------------
            }

            [Test, Ignore("Currently working on this")]
            public void TestGiz_CheckBoxUIGridDef_Creates_CheckBoxColumn()
            {
                //---------------Set up test pack-------------------
                IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
                MyBO.LoadClassDefWithBoolean();
                ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
                //--------------Assert PreConditions----------------            

                //---------------Execute Test ----------------------
                gridControl.Initialise(def);
                //---------------Test Result -----------------------
                IDataGridViewColumn column = gridControl.Grid.Columns["TestBoolean"];
                Assert.IsNotNull(column);
                Assert.IsInstanceOfType(typeof (DataGridViewCheckBoxColumnGiz), column);
                //---------------Tear Down -------------------------          
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            gridControl.Width = 200;
            gridControl.Height = 133;
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl);
            Assert.IsNotNull(gridControl.Grid);
            Assert.AreSame(gridControl.Grid, gridControl.Grid);
            Assert.AreEqual(1, gridControl.Controls.Count);
            Assert.AreEqual(gridControl.Width, gridControl.Grid.Width);
            Assert.AreEqual(gridControl.Height, gridControl.Grid.Height);


            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestInitialise()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            //---------------Execute Test ----------------------
            gridControl.Initialise(def);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridControl.Grid.Columns.Count);
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_SetCollection()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);

            Assert.AreEqual(1, gridControl.Grid.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Using_EditableDataSetProvider()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (EditableDataSetProvider), gridControl.Grid.DataSetProvider);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events")]
        public void Test_EditInTextbox_FirstRow()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            grid.Grid.SetBusinessObjectCollection(col);
            string testvalue = "testvalue";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
//            grid.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count);
            MyBO newBo = col.CreatedBusinessObjects[0];
            Assert.AreEqual(testvalue, newBo.TestProp);
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events")]
        public void Test_EditInTextbox_ExistingObject()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            AddControlToForm(grid);
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof (MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            bo.TestProp = "testPropValue";
            col.Add(bo);
            grid.Grid.SetBusinessObjectCollection(col);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, grid.Grid.Rows.Count, "Editable auto adds adding row");

            //---------------Execute Test ----------------------


            string testvalue = "new test value";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
            grid.Grid.Rows[1].Selected = true;
//            grid.ApplyChangesToBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(testvalue, bo.TestProp);
        }


        [Test]
        public void TestSetupComboBoxFromClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            string uiDefName = "default";
            UIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have comboBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsComboBoxColumnType(dataGridViewColumn);
        }

        //TODO: Combo Fill with items as per classdef.

        //TODO: Working on this
        //        [Test]
        //        public void TestSetupComboBoxFromClassDef_SetsItemsInComboBox()
        //        {
        //            //---------------Set up test pack-------------------
        //            ClassDef classDef = ContactPersonTestBO.LoadClassDefOrganisationRelationship();
        //            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

        ////            OrganisationTestBO.LoadDefaultClassDef();
        ////            CreateSavedOrganisation();
        ////            CreateSavedOrganisation();

        //            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
        //            col.LoadAll();

        //            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
        //            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

        //            //--------------Assert PreConditions----------------            
        //            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
        //            Assert.AreEqual(1, classDef.UIDefCol.Count);
        //            string uiDefName = "default";
        //            UIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
        //            Assert.IsNotNull(uiGridDef);
        //            Assert.AreEqual(1, uiGridDef.Count);

        //            Assert.AreEqual(1,col.Count);

        //            //---------------Execute Test ----------------------
        //            gridInitialiser.InitialiseGrid(classDef, uiDefName);
        //            gridControl.
        //            //---------------Test Result -----------------------
        //            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have comboBoxColumn");
        //            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
        //            AssertIsComboBoxColumnType(dataGridViewColumn);
        //        }


        //[Test]
        //public void TestSetCollection_IncorrectClassDef()
        //{
        //    //---------------Set up test pack-------------------
        //    LoadMyBoDefaultClassDef();
        //    BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
        //    IEditableGridControl editableGridControl = CreateEditableGridControl();

        //    //---------------Execute Test ----------------------
        //    editableGridControl.Initialise(Sample.CreateClassDefGiz());
        //    try
        //    {
        //        editableGridControl.SetBusinessObjectCollection(col);
        //        Assert.Fail(
        //            "You cannot call set collection for a collection that has a different class def than is initialised");
        //        ////---------------Test Result -----------------------
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        StringAssert.Contains(
        //            "You cannot call set collection for a collection that has a different class def than is initialised",
        //            ex.Message);
        //    }
        //}

        //[Test]
        //public void TestSetCollection_Null_ClearsTheGrid()
        //{
        //    //---------------Set up test pack-------------------
        //    LoadMyBoDefaultClassDef();
        //    BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
        //    IEditableGridControl editableGridControl = CreateEditableGridControl();
        //    AddControlToForm(editableGridControl);
        //    editableGridControl.SetBusinessObjectCollection(col);
        //    //----------------Assert Preconditions --------------

        //    Assert.IsTrue(editableGridControl.Grid.Rows.Count > 0, "There should be items in teh grid b4 clearing");
        //    //---------------Execute Test ----------------------
        //    editableGridControl.SetBusinessObjectCollection(null);
        //    //---------------Verify Result ---------------------
        //    Assert.AreEqual(0, editableGridControl.Grid.Rows.Count,
        //                    "There should be no items in the grid  after setting to null");
        //    Assert.IsFalse(editableGridControl.Buttons.Enabled);
        //    Assert.IsFalse(editableGridControl.FilterControl.Enabled);
        //}


        //[Test]
        //public void TestSetCollection_InitialisesGridIfNotPreviouslyInitialised()
        //{
        //    //---------------Set up test pack-------------------
        //    LoadMyBoDefaultClassDef();
        //    BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
        //    IEditableGridControl editableGridControl = CreateEditableGridControl();

        //    //---------------Execute Test ----------------------
        //    editableGridControl.SetBusinessObjectCollection(col);
        //    ////---------------Test Result -----------------------
        //    Assert.AreEqual("default", editableGridControl.UiDefName);
        //    Assert.AreEqual(col.ClassDef, editableGridControl.ClassDef);
        //}

        //[Test]
        //public void TestSetCollection_NotInitialiseGrid_IfPreviouslyInitialised()
        //{
        //    //Verify that setting the collection for a grid that is already initialised
        //    //does not cause it to be reinitialised.
        //    //---------------Set up test pack-------------------
        //    ClassDef.ClassDefs.Clear();
        //    ClassDef classDef = LoadMyBoDefaultClassDef();
        //    BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
        //    string alternateUIDefName = "Alternate";
        //    IEditableGridControl grid = CreateEditableGridControl();

        //    grid.Initialise(classDef, alternateUIDefName);
        //    //---------------Execute Test ----------------------
        //    grid.SetBusinessObjectCollection(col);
        //    ////---------------Test Result -----------------------
        //    Assert.AreEqual(alternateUIDefName, grid.UiDefName);
        //}

        [Test]
        public void Test_RaiseErrorIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                EditableGridControlGiz editableGridControlGiz = new EditableGridControlGiz(null);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains
                    ("Cannot create an editable grid control if the control factory is null", ex.Message);
            }
        }

        [Test]
        public void TestSetCollection_NumberOfGridRows_Correct()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGridControl editableGridControl = CreateEditableGridControl();

            AddControlToForm(editableGridControl);
            IEditableGrid editableGrid = (IEditableGrid) editableGridControl.Grid;
            editableGrid.Columns.Add("TestProp", "TestProp");

            //--------------Assert PreConditions----------------   
            Assert.AreEqual(4, col.Count);

            //---------------Execute Test ----------------------
            editableGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "The number of items in the grid plus the null item");
        }


        //private void CreateSavedOrganisation()
        //{
        //    OrganisationTestBO organisation1 = new OrganisationTestBO();
        //    organisation1.Save();
        //}

        [Test]
        public void TestSetupColumnAsTextBoxType_FromClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1TextboxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            string uiDefName = "default";
            UIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have textBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsTextBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestSetupColumnAsCheckBoxType_FromClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1CheckBoxColumn();
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            GridInitialiser gridInitialiser = new GridInitialiser(gridControl, GetControlFactory());

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(0, gridControl.Grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            string uiDefName = "default";
            UIGrid uiGridDef = classDef.UIDefCol[uiDefName].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            gridInitialiser.InitialiseGrid(classDef, uiDefName);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridControl.Grid.Columns.Count, "Should have ID column and should have checkBoxColumn");
            IDataGridViewColumn dataGridViewColumn = gridControl.Grid.Columns[1];
            AssertIsCheckBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        private ClassDef LoadMyBoDefaultClassDef()
        {
            ClassDef classDef;
            if (GetControlFactory() is ControlFactoryGizmox)
            {
                classDef = MyBO.LoadDefaultClassDefGizmox();
            }
            else
            {
                classDef = MyBO.LoadDefaultClassDef();
            }
            return classDef;
        }

        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO();
            cp.TestProp = "b";
            MyBO cp2 = new MyBO();
            cp2.TestProp = "d";
            MyBO cp3 = new MyBO();
            cp3.TestProp = "c";
            MyBO cp4 = new MyBO();
            cp4.TestProp = "a";
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(cp, cp2, cp3, cp4);
            return col;
        }
    }
}