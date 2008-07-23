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
using System.Collections.Generic;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestEditableGrid : TestUsingDatabase
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
            base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn);
        //protected abstract void AssertIsDateTimeColumnType(IDataGridViewColumn dataGridViewColumn);
        protected abstract IFormChilli AddControlToForm(IGridBase cntrl);
        protected abstract IEditableGrid CreateEditableGrid();


        [TestFixture]
        public class TestEditableGridWin : TestEditableGrid
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
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

            private void SimulateDeleteKeyPress(IEditableGrid editableGrid)
            {
                // This four lines are the preferable approach (create an actual Delete key press)
                //   using Nunit's testing framework (see TestControlMapperStrategyWin for a working example)
                //   but there is some deep lying bug in Nunit (and there is no GridTester or equivalent)

                //formWin.Show();
                //FormTester box = new FormTester();
                //KeyEventArgs eveArgsDelete = new KeyEventArgs(Keys.Delete);
                //box.FireEvent("KeyUp", eveArgsDelete);

                // Circumvent the above using this (which means that some code will go untested)
                editableGrid.DeleteKeyHandler();
            }

            [Test]
            public void TestConfirmDeletionFalse_NoMessageBox()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };

                IFormChilli formWin = AddControlToForm(editableGrid);

                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);
                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }

            [Test]
            public void TestConfirmDeletionTrue_ShowsMessageBox()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };

                IFormChilli formWin = AddControlToForm(editableGrid);

                //---------------Assert Precondition----------------
                Assert.IsTrue(editableGrid.ConfirmDeletion);
                Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
            }

            [Test]
            public void TestConfirmDeletion_NoDeletionWhenAllowUserToDeleteRowsIsFalse()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                editableGrid.ConfirmDeletion = true;
                editableGrid.AllowUserToDeleteRows = false;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };

                IFormChilli formWin = AddControlToForm(editableGrid);

                //---------------Assert Precondition----------------
                Assert.IsTrue(editableGrid.ConfirmDeletion);
                Assert.IsFalse(editableGrid.AllowUserToDeleteRows);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }

            [Test]
            public void TestConfirmDeletion_NoDeletionWhenDeleteKeyBehaviourIsClearContents()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };
                editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.ClearContents; 
                IFormChilli formWin = AddControlToForm(editableGrid);

                //---------------Assert Precondition----------------
                Assert.AreEqual(DeleteKeyBehaviours.ClearContents, editableGrid.DeleteKeyBehaviour);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);
                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }


            [Test]
            public void TestConfirmDeletion_NoDeletionWhenDeleteKeyBehaviourIsNone()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };
                editableGrid.DeleteKeyBehaviour = DeleteKeyBehaviours.None;
                IFormChilli formWin = AddControlToForm(editableGrid);

                //---------------Assert Precondition----------------
                Assert.AreEqual(DeleteKeyBehaviours.None, editableGrid.DeleteKeyBehaviour);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);
                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }

            [Test]
            public void TestConfirmDeletion_DeletionWhenDeleteKeyBehaviourIsDeleteRow()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return false;
                };
               
                IFormChilli formWin = AddControlToForm(editableGrid);
                editableGrid.Rows[0].Cells[0].Selected = true;
                //---------------Assert Precondition----------------
                Assert.AreEqual(DeleteKeyBehaviours.DeleteRow, editableGrid.DeleteKeyBehaviour);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);
                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
            }

            [Test]
            public void TestDeleteKeyBehaviours_IsDeleteRowByDefault()
            {
                //---------------Set up test pack-------------------
                IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
                
                //---------------Assert Precondition----------------
              
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.AreEqual(DeleteKeyBehaviours.DeleteRow,editableGrid.DeleteKeyBehaviour);
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

            [Test]
            public void TestDeleteKeyBehaviours_DeletesSelectedCells_OneRow_WhenUserConfirmsDeletion()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                //---------------Assert Precondition----------------
                Assert.AreEqual(5, editableGrid.Rows.Count);
                //---------------Execute Test ----------------------
                editableGrid.Rows[0].Cells[0].Selected = true;
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
                Assert.AreEqual(4, editableGrid.Rows.Count);
            }

            [Test]
            public void TestDeleteKeyBehaviours_DeletesSelectedCells_ThreeRows_WhenUserConfirmsDeletion()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                //---------------Assert Precondition----------------
                Assert.AreEqual(5, editableGrid.Rows.Count);
                //---------------Execute Test ----------------------
                editableGrid.Rows[0].Cells[0].Selected = true;
                editableGrid.Rows[1].Cells[0].Selected = true;
                editableGrid.Rows[2].Cells[0].Selected = true;
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
                Assert.AreEqual(2, editableGrid.Rows.Count);
            }



            [Test]
            public void TestDeleteKeyBehaviours_DeletesNoCells_WhenNoneSelected_WhenUserConfirmsDeletion()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                //---------------Assert Precondition----------------
                Assert.AreEqual(5, editableGrid.Rows.Count);
                Assert.AreEqual(0, editableGrid.SelectedCells.Count);
                //---------------Execute Test ----------------------
                
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsTrue(delegateCalled);
                Assert.AreEqual(5, editableGrid.Rows.Count);
            }

            [Test, Ignore("The thing that makes this work is breaking the other tests")]
            public void TestDeleteKeyBehaviours_NoDeleteWhen_IsInEditMode()
            {
                  //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                editableGrid.CurrentCell = editableGrid.Rows[0].Cells[0];
                editableGrid.BeginEdit(true);
                //---------------Assert Precondition----------------
                Assert.IsTrue(editableGrid.CurrentCell.IsInEditMode);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
               
            }

            [Test, Ignore("Selecting row not working")]
            public void TestDeleteKeyBehaviours_NoDeleteWhen_SelectedRowsNotZero()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                editableGrid.Rows[1].Selected=true;
                //---------------Assert Precondition----------------
                Assert.AreEqual(1, editableGrid.SelectedRows.Count);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }

            [Test, Ignore("Revisit this")]
            public void TestDeleteKeyBehaviours_NoDeleteWhen_CurrentRowNotSet()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
                editableGrid.ConfirmDeletion = true;
                bool delegateCalled = false;
                editableGrid.CheckUserConfirmsDeletionDelegate += delegate
                {
                    delegateCalled = true;
                    return true;
                };
                //---------------Assert Precondition----------------
                Assert.IsNull(editableGrid.CurrentRow);
                //---------------Execute Test ----------------------
                SimulateDeleteKeyPress(editableGrid);

                //---------------Test Result -----------------------
                Assert.IsFalse(delegateCalled);
            }




            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseWinStub gridBase = new GridBaseWinStub();
            //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                         IGridBase gridBase)
            {
                System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView)gridBase;
                System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            protected override IFormChilli AddControlToForm(IGridBase gridBase)
            {
                IFormChilli frm = GetControlFactory().CreateForm();
                frm.Controls.Add((IControlChilli)gridBase);
                return frm;
            }

            protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(System.Windows.Forms.DataGridViewTextBoxColumn),
                    dataGridViewColumnWin.DataGridViewColumn);
            }

            protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(System.Windows.Forms.DataGridViewCheckBoxColumn),
                    dataGridViewColumnWin.DataGridViewColumn);
            }

            protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(System.Windows.Forms.DataGridViewComboBoxColumn),
                    dataGridViewColumnWin.DataGridViewColumn);
            }

            protected override IEditableGrid CreateEditableGrid()
            {
                EditableGridWin editableGridWin = new EditableGridWin();
                System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
                frm.Controls.Add(editableGridWin);
                return editableGridWin;
            }
        }

        [TestFixture]
        public class TestEditableGridGiz : TestEditableGrid
        {
            
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }


            protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz) dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn), 
                    dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz)dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(Gizmox.WebGUI.Forms.DataGridViewCheckBoxColumn),
                    dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
            {
                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz)dataGridViewColumn;
                Assert.IsInstanceOfType(typeof(Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn),
                    dataGridViewColumnGiz.DataGridViewColumn);
            }

            protected override IEditableGrid CreateEditableGrid()
            {
                EditableGridGiz editableGridGiz = new EditableGridGiz();
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(editableGridGiz);
                return editableGridGiz;
            }

//            protected override void AssertIsDateTimeColumnType(IDataGridViewColumn dataGridViewColumn)
//            {
//                DataGridViewColumnGiz dataGridViewColumnGiz = (DataGridViewColumnGiz) dataGridViewColumn;
//                Assert.IsInstanceOfType(typeof(Gizmox.WebGUI.Forms.DataGridView,
//                    dataGridViewColumnGiz.DataGridViewColumn);
//            }
            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseGizStub gridBase = new GridBaseGizStub();
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            protected override IFormChilli AddControlToForm(IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) gridBase);
                return null;
            }

            private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                        IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView) gridBase;
                Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            private object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
            {
                Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
                return cell.Value;
            }

            //public void TestCreateGridBaseGiz()
            //{
            //    //---------------Set up test pack-------------------
            //    //---------------Execute Test ----------------------
            //    IControlChilli grid = GetControlFactory().CreateEditableGridControl();
            //    ReadOnlyGridGiz readOnlyGrid = (EditableGridControl)grid;
            //    ////---------------Test Result -----------------------
            //    Assert.IsTrue(readOnlyGrid.ReadOnly);
            //    Assert.IsFalse(readOnlyGrid.AllowUserToAddRows);
            //    Assert.IsFalse(readOnlyGrid.AllowUserToDeleteRows);
            //    Assert.IsTrue(readOnlyGrid.SelectionMode == Gizmox.WebGUI.Forms.DataGridViewSelectionMode.FullRowSelect);
            //    //---------------Tear Down -------------------------   
            //}
        }


        [Test]
        public void TestConstructGrid()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IControlChilli grid = GetControlFactory().CreateEditableGrid();
            //---------------Test Result -----------------------
            IEditableGrid editableGrid = (IEditableGrid) grid;
            Assert.IsNotNull(editableGrid);
            Assert.IsFalse(editableGrid.ReadOnly);
            Assert.IsTrue(editableGrid.AllowUserToAddRows);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //TODO: Should we test selection mode
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestRejectChanges()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.SetBusinessObjectCollection(col);

            //---------------Assert Precondition----------------
            Assert.AreEqual(5,editableGrid.Rows.Count);
            Assert.AreEqual("b", editableGrid.Rows[0].Cells[0].Value);
            //---------------Execute Test ----------------------
            editableGrid.Rows[0].Cells[0].Value = "test";
            //---------------Assert Precondition----------------
            Assert.AreEqual("test", editableGrid.Rows[0].Cells[0].Value);
            //---------------Execute Test ----------------------
            editableGrid.RejectChanges();
            //---------------Test Result -----------------------
            Assert.AreEqual("b", editableGrid.Rows[0].Cells[0].Value);
        }

        [Test]
        public void TestSaveChanges()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //---------------Clean from previous tests----------
            string originalText = "testsavechanges";
            string newText = "testsavechanges_edited";
            MyBO oldBO1 = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + originalText + "'");
            if (oldBO1 != null)
            {
                oldBO1.Delete();
                oldBO1.Save();
            }
            MyBO oldBO2 = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + newText + "'");
            if (oldBO2 != null)
            {
                oldBO2.Delete();
                oldBO2.Save();
            }
            
            MyBO bo = new MyBO();
            bo.TestProp = originalText;
            bo.Save();

            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(bo);

            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            editableGrid.SetBusinessObjectCollection(col);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, editableGrid.Rows.Count);
            Assert.AreEqual(originalText, editableGrid.Rows[0].Cells[0].Value);
            MyBO nullBO = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + newText + "'");
            Assert.IsNull(nullBO);
            //---------------Execute Test ----------------------
            editableGrid.Rows[0].Cells[0].Value = newText;
            //---------------Assert Precondition----------------
            Assert.AreEqual(newText, editableGrid.Rows[0].Cells[0].Value);
            //---------------Execute Test ----------------------
            editableGrid.SaveChanges();
            //---------------Test Result -----------------------
            Assert.AreEqual(newText, editableGrid.Rows[0].Cells[0].Value);
            MyBO savedBO = BOLoader.Instance.GetBusinessObject<MyBO>("TestProp='" + newText + "'");
            Assert.IsNotNull(savedBO);
            //---------------Tear Down--------------------------
            savedBO.Delete();
            savedBO.Save();
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
            editableGrid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(col.Count + 1, editableGrid.Rows.Count, "should be 4 item 1 adding item");
            //---------------Tear Down -------------------------    
        }




        [Test]
        public void TestSetupColumnAsTextBoxType()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1TextboxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_2Items(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            SetupGridColumnsForMyBo(grid);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            UIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1,uiGridDef.Count);

            //---------------Execute Test ----------------------
            grid.SetBusinessObjectCollection(colBOs);
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsTextBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetupColumnAsCheckBoxType()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1CheckBoxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_1CheckboxItem(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            IDataGridViewColumn dataGridViewColumnSetup = GetControlFactory().CreateDataGridViewCheckBoxColumn();
            SetupGridColumnsForMyBo(grid,dataGridViewColumnSetup);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            UIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);

            //---------------Execute Test ----------------------
            grid.SetBusinessObjectCollection(colBOs);
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsCheckBoxColumnType(dataGridViewColumn);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestSetupComboBoxType()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1ComboBoxColumn();
            IBusinessObjectCollection colBOs = GetCol_BO_1ComboBoxItem(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            IDataGridViewColumn dataGridViewColumnSetup = GetControlFactory().CreateDataGridViewComboBoxColumn();
            SetupGridColumnsForMyBo(grid, dataGridViewColumnSetup);
            
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Columns.Count);
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            UIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1, uiGridDef.Count);
            //---------------Execute Test ----------------------
            grid.SetBusinessObjectCollection(colBOs);
            //---------------Test Result -----------------------
            IDataGridViewColumn dataGridViewColumn = grid.Columns[0];
            AssertIsComboBoxColumnType(dataGridViewColumn);    
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

        private static IBusinessObjectCollection GetCol_BO_1ComboBoxItem(ClassDef classDef)
        {
            IBusinessObjectCollection col = new BusinessObjectCollection<BusinessObject>(classDef);
            IBusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("RelatedID",Guid.NewGuid());
            col.Add(bo1);
            return col;
        }


        private static IBusinessObjectCollection GetCol_BO_1CheckboxItem(ClassDef classDef)
        {
            IBusinessObjectCollection col = new BusinessObjectCollection<BusinessObject>(classDef);
            IBusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", true);
            col.Add(bo1);
            return col;

        }

        private static IBusinessObjectCollection GetCol_BO_2Items(ClassDef classDef)
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

        private IEditableGrid GetGridWith_5_Rows(out BusinessObjectCollection<MyBO> col)
        {
            LoadMyBoDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IEditableGrid grid = CreateEditableGrid();
            SetupGridColumnsForMyBo(grid);
            grid.SetBusinessObjectCollection(col);
            return grid;
        }

        private static void SetupGridColumnsForMyBo(IEditableGrid editableGrid, IDataGridViewColumn dataGridViewColumn)
        {
            editableGrid.Columns.Add(dataGridViewColumn);
        }

        private static void SetupGridColumnsForMyBo(IEditableGrid editableGrid)
        {
            editableGrid.Columns.Add("TestProp","TestProp");
        }
    }
}