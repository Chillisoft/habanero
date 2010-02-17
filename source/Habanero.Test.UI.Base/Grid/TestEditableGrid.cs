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
using Habanero.UI.VWG;
using Habanero.UI.Win;
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
    public class TestEditableGridVWG
    {
        private const string _HABANERO_OBJECTID = "HABANERO_OBJECTID";

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

//        protected abstract IControlFactory GetControlFactory();
//        protected abstract void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn);
//        protected abstract void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn);
//        protected abstract void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn);
//        //protected abstract void AssertIsDateTimeColumnType(IDataGridViewColumn dataGridViewColumn);
//        protected abstract void AssertCorrectSelectionMode(IGridBase dataGridView);
//        protected abstract IFormHabanero AddControlToForm(IGridBase cntrl);
//        protected abstract IEditableGrid CreateEditableGrid();


        protected virtual IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }


        protected virtual void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected virtual void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (Gizmox.WebGUI.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected virtual void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected virtual void AssertCorrectSelectionMode(IGridBase dataGridView)
        {
            Gizmox.WebGUI.Forms.DataGridView grid = (Gizmox.WebGUI.Forms.DataGridView) dataGridView;
            Assert.AreEqual(Gizmox.WebGUI.Forms.DataGridViewSelectionMode.RowHeaderSelect, grid.SelectionMode);
        }

        protected virtual IEditableGrid CreateEditableGrid()
        {
            EditableGridVWG editableGridVWG = new EditableGridVWG();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(editableGridVWG);
            return editableGridVWG;
        }

        protected virtual IFormHabanero AddControlToForm(IGridBase gridBase)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control) gridBase);
            return null;
        }

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

        private void LoadMyBoDefaultClassDef()
        {
            if (GetControlFactory() is ControlFactoryVWG)
            {
                MyBO.LoadDefaultClassDefVWG();
            }
            else
            {
                MyBO.LoadDefaultClassDef();
            }
            return;
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

        private static void SetupGridColumnsForMyBo(IDataGridView editableGrid, IDataGridViewColumn dataGridViewColumn)
        {
            editableGrid.Columns.Add(dataGridViewColumn);
        }

        protected static void SetupGridColumnsForMyBo(IDataGridView editableGrid)
        {
            editableGrid.Columns.Add(_HABANERO_OBJECTID, _HABANERO_OBJECTID);
            editableGrid.Columns.Add("TestProp", "TestProp");
        }
    }

    [TestFixture]
    public class TestEditableGridWin : TestEditableGridVWG
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }



        private static void SimulateDeleteKeyPress(IEditableGrid editableGrid)
        {
            // These four lines are the preferable approach (create an actual Delete key press)
            //   using Nunit's testing framework (see TestControlMapperStrategyWin for a working example)
            //   but there is some deep lying bug_ in Nunit (and there is no GridTester or equivalent)

            //formWin.Show();
            //FormTester box = new FormTester();
            //KeyEventArgs eveArgsDelete = new KeyEventArgs(Keys.Delete);
            //box.FireEvent("KeyUp", eveArgsDelete);

            // Circumvent the above using this (which means that some code will go untested)
            editableGrid.DeleteKeyHandler();
        }

        [Test]
        public void TestConfirmDeletion_False_NoMessageBox()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };

            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);
            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestConfirmDeletion_True_ShowsMessageBox()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };

            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();

            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.ConfirmDeletion);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsTrue(confirmationDelegateCalled);
        }

        [Test]
        public  void TestConfirmDeletion_NoDeletionWhenAllowUserToDeleteRowsIsFalse()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;
            editableGrid.AllowUserToDeleteRows = false;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };

            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.ConfirmDeletion);
            Assert.IsFalse(editableGrid.AllowUserToDeleteRows);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Test]
        public void TestConfirmDeletion_NoDeletionWhenDeleteKeyBehaviourIsClearContents()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };
            editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.ClearContents;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.AreEqual(DeleteKeyBehaviours.ClearContents, editableGrid.DeleteKeyBehaviour);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);
            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }


        [Test]
        public void TestConfirmDeletion_NoDeletionWhenDeleteKeyBehaviourIsNone()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };
            editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.None;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.AreEqual(DeleteKeyBehaviours.None, editableGrid.DeleteKeyBehaviour);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);
            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestConfirmDeletion_DeletionWhenDeleteKeyBehaviourIsDeleteRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return false;
                                                              };

            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.Rows[0].Cells[0].Selected = true;
            //---------------Assert Precondition----------------
            Assert.AreEqual(DeleteKeyBehaviours.DeleteRow, editableGrid.DeleteKeyBehaviour);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);
            //---------------Test Result -----------------------
            Assert.IsTrue(confirmationDelegateCalled);
        }

        [Test]
        public void TestDeleteKeyBehaviours_IsDeleteRowByDefault()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteKeyBehaviours.DeleteRow, editableGrid.DeleteKeyBehaviour);
        }

        [Test]
        public void TestDeleteKeyBehaviours_ChangesValue()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();

            //---------------Assert Precondition----------------
            Assert.AreEqual(DeleteKeyBehaviours.DeleteRow, editableGrid.DeleteKeyBehaviour);
            //---------------Execute Test ----------------------
            editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.ClearContents;
            //---------------Test Result -----------------------
            Assert.AreEqual(DeleteKeyBehaviours.ClearContents, editableGrid.DeleteKeyBehaviour);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestDeleteKeyBehaviours_DeletesSelectedCells_OneRow_WithoutConfirmsDeletion()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = false;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, editableGrid.Rows.Count);
            //---------------Execute Test ----------------------
            editableGrid.Rows[0].Cells[0].Selected = true;
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
            Assert.AreEqual(4, editableGrid.Rows.Count);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestDeleteKeyBehaviours_DeletesSelectedCells_OneRow_WhenUserConfirmsDeletion()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, editableGrid.Rows.Count);
            //---------------Execute Test ----------------------
            editableGrid.Rows[0].Cells[0].Selected = true;
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsTrue(confirmationDelegateCalled);
            Assert.AreEqual(4, editableGrid.Rows.Count);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestDeleteKeyBehaviours_DeletesSelectedCells_ThreeRows_WhenUserConfirmsDeletion()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, editableGrid.Rows.Count);
            //---------------Execute Test ----------------------
            editableGrid.Rows[0].Cells[0].Selected = true;
            editableGrid.Rows[1].Cells[0].Selected = true;
            editableGrid.Rows[2].Cells[0].Selected = true;
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsTrue(confirmationDelegateCalled);
            Assert.AreEqual(2, editableGrid.Rows.Count);
        }

        [Test]
        public void TestDeleteKeyBehaviours_DeletesNoCells_WhenNoneSelected_WhenUserConfirmsDeletion()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            foreach (IDataGridViewCell Cell in editableGrid.SelectedCells)
            {
                Cell.Selected = false;
            }
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, editableGrid.Rows.Count);
            Assert.AreEqual(0, editableGrid.SelectedCells.Count);
            //---------------Execute Test ----------------------

            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsTrue(confirmationDelegateCalled);
            Assert.AreEqual(5, editableGrid.Rows.Count);
        }

        [Test]
        public void TestDeleteKeyBehaviours_NoDeleteWhen_IsInEditMode()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[0];
            editableGrid.BeginEdit(true);
            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.CurrentCell.IsInEditMode);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Test, Ignore("Cannot programmatically select a grid row")]
        public void TestDeleteKeyBehaviours_NoDeleteWhen_SelectedRowsNotZero()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.Rows[1].Selected = true;

            //editableGrid.MultiSelect = false;
            //((EditableGridWin)editableGrid).SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //editableGrid.CurrentCell = editableGrid.Rows[0].Cells[0];

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, editableGrid.SelectedRows.Count);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Test]
        public void TestDeleteKeyBehaviours_NoDeleteWhen_CurrentRowNotSet()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            //---------------Assert Precondition----------------
            Assert.IsNull(editableGrid.CurrentRow);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestDeleteKeyBehaviours_ClearsContentsSuccessfully()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            editableGrid.ConfirmDeletion = true;
            editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.ClearContents;

            bool confirmationDelegateCalled = false;
            editableGrid.CheckUserConfirmsDeletionDelegate -= EditableGridWin.CheckUserWantsToDelete;
            editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                                                              {
                                                                  confirmationDelegateCalled = true;
                                                                  return true;
                                                              };
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();

            editableGrid.Rows[0].Cells[0].Selected = true;
            editableGrid.Rows[1].Cells[0].Selected = true;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, editableGrid.SelectedCells.Count);
            Assert.AreEqual("b", editableGrid.Rows[0].Cells[0].Value);
            Assert.AreEqual("d", editableGrid.Rows[1].Cells[0].Value);
            Assert.AreEqual("c", editableGrid.Rows[2].Cells[0].Value);
            //---------------Execute Test ----------------------
            SimulateDeleteKeyPress(editableGrid);

            //---------------Test Result -----------------------
            Assert.IsFalse(confirmationDelegateCalled);
            Assert.IsInstanceOf(typeof (DBNull), editableGrid.Rows[0].Cells[0].Value);
            Assert.IsInstanceOf(typeof (DBNull), editableGrid.Rows[1].Cells[0].Value);
            Assert.AreEqual("c", editableGrid.Rows[2].Cells[0].Value);
        }
        
        [Test]
        public void TestDeleteKeyBehaviours_MessageBoxDelegateAssignedByDefault()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual("CheckUserWantsToDelete", editableGrid.CheckUserConfirmsDeletionDelegate.Method.Name);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestComboBoxClick_DefaultAndCanSet()
        {
            //---------------Set up test pack-------------------
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            //---------------Assert Precondition----------------
            Assert.IsTrue(editableGrid.ComboBoxClickOnce);
            //---------------Execute Test ----------------------
            editableGrid.ComboBoxClickOnce = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(editableGrid.ComboBoxClickOnce);
            //---------------Tear Down -------------------------
        }

        [Ignore("Changing how this works 04 March 2009 bbb")]
        [Test]
        public void TestComboBoxClick_SetsCellInEditModeOnOneClick()
        {
            //---------------Set up test pack-------------------
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            AddComboBoxColumnWithValues(editableGrid);
            ((EditableGridWin) editableGrid).CellClick -= ((EditableGridWin) editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();

            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin) editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof (System.Windows.Forms.DataGridViewComboBoxColumn), column);
            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin) editableGrid).CheckIfComboBoxShouldSetToEditMode(1, 0);
            //---------------Test Result -----------------------
            Assert.IsTrue(setToEditMode);
        }

        [Test]
        public void TestComboBoxClick_DoesNotAffectOtherColumnType()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            ((EditableGridWin) editableGrid).CellClick -= ((EditableGridWin) editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.IsNotInstanceOf(typeof (IDataGridViewComboBoxColumn), editableGrid.Columns[0]);
            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin) editableGrid).CheckIfComboBoxShouldSetToEditMode(0, 0);
            //---------------Test Result -----------------------
            Assert.IsFalse(setToEditMode);
        }

        [Test]
        public void TestComboBoxClick_DontEditWhencomboBoxClickOnceIsFalse()
        {
            //---------------Set up test pack-------------------
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            AddComboBoxColumnWithValues(editableGrid);
            editableGrid.ComboBoxClickOnce = false;
            ((EditableGridWin) editableGrid).CellClick -= ((EditableGridWin) editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];

            //---------------Assert Precondition----------------
            Assert.IsFalse(editableGrid.ComboBoxClickOnce);
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin) editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof (System.Windows.Forms.DataGridViewComboBoxColumn), column);

            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin) editableGrid).CheckIfComboBoxShouldSetToEditMode(1, 0);

            //---------------Test Result -----------------------
            Assert.IsFalse(setToEditMode);
        }

        [Test]
        public void TestComboBoxClick_DoesNotEditWhenIndicesAreNegetive()
        {
            //---------------Set up test pack-------------------
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            AddComboBoxColumnWithValues(editableGrid);
            ((EditableGridWin) editableGrid).CellClick -= ((EditableGridWin) editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin) editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof (System.Windows.Forms.DataGridViewComboBoxColumn), column);

            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin) editableGrid).CheckIfComboBoxShouldSetToEditMode(-1, -1);

            //---------------Test Result -----------------------
            Assert.IsFalse(setToEditMode);
        }


        //protected override IGridBase CreateGridBaseStub()
        //{
        //    GridBaseWinStub gridBase = new GridBaseWinStub();
        //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    frm.Controls.Add(gridBase);
        //    return gridBase;
        //}

        //            private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
        //                                                                         IGridBase gridBase)
        //            {
        //                System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView)gridBase;
        //                System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
        //                return row.Cells[propName];
        //            }

        protected override IFormHabanero AddControlToForm(IGridBase gridBase)
        {
            IFormHabanero frm = GetControlFactory().CreateForm();
            frm.Controls.Add(gridBase);
            return frm;
        }

        protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (System.Windows.Forms.DataGridViewTextBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (System.Windows.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin) dataGridViewColumn;
            Assert.IsInstanceOf
                (typeof (System.Windows.Forms.DataGridViewComboBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertCorrectSelectionMode(IGridBase dataGridView)
        {
            System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView) dataGridView;
            Assert.AreEqual(System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect, grid.SelectionMode);
        }

        protected override IEditableGrid CreateEditableGrid()
        {
            EditableGridWin editableGridWin = new EditableGridWin();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(editableGridWin);
            return editableGridWin;
        }
    }
}
