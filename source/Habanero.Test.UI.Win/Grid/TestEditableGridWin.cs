using System;
using Habanero.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestEditableGridWin : TestEditableGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override void LoadMyBoDefaultClassDef()
        {
            MyBO.LoadDefaultClassDef();
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
        public void TestConfirmDeletion_NoDeletionWhenAllowUserToDeleteRowsIsFalse()
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
            Assert.IsInstanceOf(typeof(DBNull), editableGrid.Rows[0].Cells[0].Value);
            Assert.IsInstanceOf(typeof(DBNull), editableGrid.Rows[1].Cells[0].Value);
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
            ((EditableGridWin)editableGrid).CellClick -= ((EditableGridWin)editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();

            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin)editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof(System.Windows.Forms.DataGridViewComboBoxColumn), column);
            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin)editableGrid).CheckIfComboBoxShouldSetToEditMode(1, 0);
            //---------------Test Result -----------------------
            Assert.IsTrue(setToEditMode);
        }

        [Test]
        public void TestComboBoxClick_DoesNotAffectOtherColumnType()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IEditableGrid editableGrid = GetGridWith_5_Rows(out col);
            ((EditableGridWin)editableGrid).CellClick -= ((EditableGridWin)editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            //---------------Assert Precondition----------------
            Assert.IsNotInstanceOf(typeof(IDataGridViewComboBoxColumn), editableGrid.Columns[0]);
            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin)editableGrid).CheckIfComboBoxShouldSetToEditMode(0, 0);
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
            ((EditableGridWin)editableGrid).CellClick -= ((EditableGridWin)editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];

            //---------------Assert Precondition----------------
            Assert.IsFalse(editableGrid.ComboBoxClickOnce);
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin)editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof(System.Windows.Forms.DataGridViewComboBoxColumn), column);

            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin)editableGrid).CheckIfComboBoxShouldSetToEditMode(1, 0);

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
            ((EditableGridWin)editableGrid).CellClick -= ((EditableGridWin)editableGrid).CellClickHandler;
            IFormHabanero formWin = AddControlToForm(editableGrid);
            formWin.Show();
            editableGrid.CurrentCell = editableGrid.Rows[0].Cells[1];

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, editableGrid.CurrentCell.RowIndex);
            Assert.AreEqual(1, editableGrid.CurrentCell.ColumnIndex);
            System.Windows.Forms.DataGridViewColumn column =
                ((DataGridViewColumnWin)editableGrid.Columns[2]).DataGridViewColumn;
            Assert.IsInstanceOf(typeof(System.Windows.Forms.DataGridViewComboBoxColumn), column);

            //---------------Execute Test ----------------------
            bool setToEditMode = ((EditableGridWin)editableGrid).CheckIfComboBoxShouldSetToEditMode(-1, -1);

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
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(System.Windows.Forms.DataGridViewTextBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(System.Windows.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnWin dataGridViewColumnWin = (DataGridViewColumnWin)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(System.Windows.Forms.DataGridViewComboBoxColumn), dataGridViewColumnWin.DataGridViewColumn);
        }

        protected override void AssertCorrectSelectionMode(IGridBase dataGridView)
        {
            System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView)dataGridView;
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