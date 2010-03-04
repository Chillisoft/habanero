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
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{   
    /// <summary>
    /// TODO:
    ///  - create code that assigns an event handler to show the confirmation message box
    /// 
    /// UNTESTED (IN WIN):
    ///  - true delete key press on win version of grids (ie. the overwriting of the key
    ///       press events)
    ///  - confirming deletion before the default deletion (selecting a full row and pressing
    ///       delete)
    ///  - that the delegate that displays a message box actually does so
    ///  - that begin edit actually happens on comboboxclick (was causing STA thread errors)
    ///  - that combo box click doesn't edit when already editing(as we cannot call beginEdit)
    ///  - that the combo box drops down on click as this requires being in edit mode
    /// </summary>
    [TestFixture]
    public abstract class TestEditableGrid
    {
        protected const string _HABANERO_OBJECTID = "HABANERO_OBJECTID";

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            //base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertCorrectSelectionMode(IGridBase dataGridView);
        protected abstract IEditableGrid CreateEditableGrid();
        protected abstract IFormHabanero AddControlToForm(IGridBase gridBase);

        [Test]
        public void TestConstructGrid()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateEditableGrid();
            //---------------Test Result -----------------------
            IEditableGrid editableGrid = (IEditableGrid) grid;
            Assert.IsNotNull(editableGrid);
            Assert.IsFalse(editableGrid.ReadOnly);
            Assert.IsTrue(editableGrid.AllowUserToAddRows);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //Should we test selection mode
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetAllowUsersToAddRowsToFalse()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.AllowUserToAddRows);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //---------------Execute Test ----------------------
            editableGrid.AllowUserToAddRows = false;
            editableGrid.AllowUserToDeleteRows = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(editableGrid.AllowUserToAddRows);
            Assert.IsFalse(editableGrid.AllowUserToDeleteRows);
        }

        [Test]
        public void TestConfirmDeletion_FalseByDefault()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(editableGrid.ConfirmDeletion);
        }

        [Test]
        public void TestConfirmDeletion_SetToTrue()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();

            //---------------Assert Precondition----------------
            Assert.IsFalse(editableGrid.ConfirmDeletion);
            //---------------Execute Test ----------------------
            editableGrid.ConfirmDeletion = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(editableGrid.ConfirmDeletion);
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            //---------------Execute Test ----------------------
            editableGrid.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void TestSetupColumnAsTextBoxType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1TextboxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_2Items(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            SetupGridColumnsForMyBo(grid);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            IUIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            grid.BusinessObjectCollection = colBOs;
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsTextBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetupColumnAsCheckBoxType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1CheckBoxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_1CheckboxItem(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            IDataGridViewColumn dataGridViewColumnSetup = GetControlFactory().CreateDataGridViewCheckBoxColumn();
            SetupGridColumnsForMyBo(grid, dataGridViewColumnSetup);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            IUIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
#pragma warning disable 618,612// maintained test for backward compatibility testing
            grid.SetBusinessObjectCollection(colBOs);
#pragma warning restore 618,612
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsCheckBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void Test_Set_BusinessObjectCollection_SetupColumnAsCheckBoxType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1CheckBoxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_1CheckboxItem(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            IDataGridViewColumn dataGridViewColumnSetup = GetControlFactory().CreateDataGridViewCheckBoxColumn();
            SetupGridColumnsForMyBo(grid, dataGridViewColumnSetup);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            IUIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            grid.BusinessObjectCollection = colBOs;
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsCheckBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestSetupComboBoxType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_1ComboBoxItem(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            IDataGridViewColumn dataGridViewColumnSetup = GetControlFactory().CreateDataGridViewComboBoxColumn();
            SetupGridColumnsForMyBo(grid, dataGridViewColumnSetup);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            IUIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);
            //---------------Execute Test ----------------------
            grid.BusinessObjectCollection = colBOs;
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsComboBoxColumnType(dataGridViewColumn);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestBasicSettings()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            //---------------Test Result -----------------------
            AssertCorrectSelectionMode(grid);
        }

        [Test]
        public void Test_AutoSelectsFirstItem()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            //---------------Execute Test ----------------------
            editableGrid.BusinessObjectCollection = col;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            Assert.IsNotNull(editableGrid.SelectedBusinessObject);
        }

        [Test]
        public void Test_SelectItem_SetsSelectedBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGrid.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGrid.SelectedBusinessObject);
        }

        [Test]
        public void Test_SelectIndex_SetsSelectedBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGrid.Rows[2].Selected = true;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGrid.SelectedBusinessObject);
        }

        [Ignore(
            " Do not seem to be able to get the Businesss Object selected event to fire from any sort of manipulation of the grid we do not have a NUnitForms type tester for grids so not sure what next"
            )] //TODO Brett 04 Mar 2009:
        [Test]
        public void Test_BusinessObjectSelectEvent()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            IBusinessObject boFromEvent = null;
            bool eventFired = false;
            editableGrid.BusinessObjectSelected += delegate(object sender, BOEventArgs e)
                                                       {
                                                           eventFired = true;
                                                           boFromEvent = e.BusinessObject;
                                                       };
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGrid.CurrentCell = editableGrid.Rows[2].Cells[1];
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
//            Assert.AreSame(myBO, editableGrid.SelectedBusinessObject);
            Assert.IsTrue(eventFired);
            Assert.AreEqual(myBO, boFromEvent);
        }

        [Test]
        public void Test_SetBusinessObject_BusinessObjectSelectEvent_FiresAndReturnsAValidBO()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            IBusinessObject boFromEvent = null;
            bool eventFired = false;
            editableGrid.BusinessObjectSelected += delegate(object sender, BOEventArgs e)
                                                       {
                                                           eventFired = true;
                                                           boFromEvent = e.BusinessObject;
                                                       };
            MyBO myBO = col[2];
            //---------------Execute Test ----------------------
            editableGrid.SelectedBusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            Assert.AreSame(myBO, editableGrid.SelectedBusinessObject);
            Assert.IsTrue(eventFired);
            Assert.AreEqual(myBO, boFromEvent);
        }

        [Test]
        public void Test_GetBusinessObjectAtRow()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IBusinessObject bo = editableGrid.GetBusinessObjectAtRow(0);
            //---------------Test Result -----------------------
            Assert.AreSame(col[0], bo);
        }

        [Test]
        public void Test_GetBusinessObjectAtRow_NewRow()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.BusinessObjectCollection = col;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IBusinessObject bo = editableGrid.GetBusinessObjectAtRow(4);
            //---------------Test Result -----------------------
            Assert.IsNull(bo);
        }

        [Test]
        public void Test_GetBusinessObjectAtRow_WhenBOColNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            //---------------Assert Precondition----------------
            Assert.IsNull(editableGrid.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            IBusinessObject bo = editableGrid.GetBusinessObjectAtRow(0);
            //---------------Test Result -----------------------
            Assert.IsNull(bo);
        }

        private static IBusinessObjectCollection GetCol_BO_1ComboBoxItem(IClassDef classDef)
        {
            IBusinessObjectCollection col = new BusinessObjectCollection<BusinessObject>(classDef);
            IBusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("RelatedID", Guid.NewGuid());
            col.Add(bo1);
            return col;
        }

        private static IBusinessObjectCollection GetCol_BO_1CheckboxItem(IClassDef classDef)
        {
            IBusinessObjectCollection col = new BusinessObjectCollection<BusinessObject>(classDef);
            IBusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", true);
            col.Add(bo1);
            return col;
        }

        private static IBusinessObjectCollection GetCol_BO_2Items(IClassDef classDef)
        {
            IBusinessObjectCollection col = new BusinessObjectCollection<BusinessObject>(classDef);
            IBusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            IBusinessObject bo2 = classDef.CreateNewBusinessObject();
            bo2.SetPropertyValue("TestProp", "2Value1");
            bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(bo1);
            col.Add(bo2);
            return col;
        }

        protected static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO();
            cp.TestProp = "b";
            cp.Save();
            MyBO cp2 = new MyBO();
            cp2.TestProp = "d";
            cp2.Save();
            MyBO cp3 = new MyBO();
            cp3.TestProp = "c";
            cp3.Save();
            MyBO cp4 = new MyBO();
            cp4.TestProp = "a";
            cp4.Save();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(cp, cp2, cp3, cp4);
            return col;
        }

        protected static void SetupGridColumnsForMyBo(IDataGridView editableGrid, IDataGridViewColumn dataGridViewColumn)
        {
            editableGrid.Columns.Add(dataGridViewColumn);
        }

        protected static void SetupGridColumnsForMyBo(IDataGridView editableGrid)
        {
            editableGrid.Columns.Add(_HABANERO_OBJECTID, _HABANERO_OBJECTID);
            editableGrid.Columns.Add("TestProp", "TestProp");
        }

        protected abstract void LoadMyBoDefaultClassDef();

        protected IEditableGrid GetGridWith_5_Rows(out BusinessObjectCollection<MyBO> col)
        {
            LoadMyBoDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IEditableGrid grid = CreateEditableGrid();
            SetupGridColumnsForMyBo(grid);
            grid.BusinessObjectCollection = col;
            return grid;
        }

        protected void AddComboBoxColumnWithValues(IEditableGrid editableGrid)
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("str");

            table.LoadDataRow(new object[] {"", ""}, true);
            table.LoadDataRow(new object[] {"asdfsdf", "A"}, true);
            table.LoadDataRow(new object[] {"shasdfg", "B"}, true);

            IDataGridViewComboBoxColumn column = GetControlFactory().CreateDataGridViewComboBoxColumn();
            column.DataSource = table;

            SetupGridColumnsForMyBo(editableGrid, column);
        }
    }



}
