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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.Util;
using log4net;
using DataGridViewSelectionMode=System.Windows.Forms.DataGridViewSelectionMode;
using MessageBoxButtons=System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon=System.Windows.Forms.MessageBoxIcon;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a grid on which the user can edit data and add new business objects directly.
    /// <br/>
    /// IMPORTANT: This grid does not provide any buttons or menus for users
    /// to save the changes they have made, and all changes will be lost if the form
    /// is closed and changes are not saved programmatically.  Either carry out a dirty check when the
    /// parent form is closed and take appropriate save action using SaveChanges(), or use an
    /// IEditableGridControl, which provides Save and Cancel buttons. 
    /// </summary>
    public class EditableGridWin : GridBaseWin, IEditableGrid
    {
        private static readonly ILog _log = LogManager.GetLogger("Habanero.UI.Win.EditableGridWin");

        private DeleteKeyBehaviours _deleteKeyBehaviour;

        /// <summary>
        /// Gets or sets whether clicking on a ComboBox cell causes the drop-down to
        /// appear immediately.  Set this to false if the user should click twice
        /// (first to select, then to edit), which is the default behaviour.
        /// </summary>
        public bool ComboBoxClickOnce { get; set; }

        ///<summary>
        /// Constructs the <see cref="EditableGridWin"/>
        ///</summary>
        public EditableGridWin()
        {
            ConfirmDeletion = false;
            AllowUserToAddRows = true;
            _deleteKeyBehaviour = DeleteKeyBehaviours.DeleteRow;
//            SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ComboBoxClickOnce = true;
            UserDeletingRow += ConfirmRowDeletion;
            CheckUserConfirmsDeletionDelegate += CheckUserWantsToDelete;
            this.UserDeletedRow+=((sender, e) => ResetBOCollection());
            CellClick += CellClickHandler;
        }



        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a read only datasetprovider, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns></returns>
        public override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            return new EditableDataSetProvider(col);
        }

        /// <summary>
        /// Restore the objects in the grid to their last saved state
        /// </summary>
        public void RejectChanges()
        {
            try
            {
                if (this.BusinessObjectCollection != null)
                {
                    //EditableDataSetProvider editableDataSetProvider = (EditableDataSetProvider)this.DataSetProvider;
                    //editableDataSetProvider.DeregisterForEvents();
                    
                    this.BusinessObjectCollection.CancelEdits();
                    ResetBOCollection();
                    //editableDataSetProvider.RegisterForEvents();//This is not necessary as the SetBOCol ultimately
                    //registers for all events 
                }
                else if (this.DataSource is DataView)
                {
                    ((DataView)this.DataSource).Table.RejectChanges();
                }
 
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void ResetBOCollection()
        {
            IBusinessObjectCollection col = this.BusinessObjectCollection;
            IBusinessObject bo = this.SelectedBusinessObject;
            if (this.DataSource != null)
            {
                ((DataView) this.DataSource).Table.RejectChanges();

            }
            SetBusinessObjectCollection(null);
            SetBusinessObjectCollection(col);
            SelectedBusinessObject = bo;
        }

        /// <summary>
        /// Saves the changes made to the data in the grid
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                if (this.BusinessObjectCollection != null)
                {
                    this.BusinessObjectCollection.SaveAll();
                } else if (this.DataSource is DataView)
                {
                    ((DataView) this.DataSource).Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        public bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Gets or sets the delegate that checks whether the user wants to delete selected rows
        /// </summary>
        public CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate { get; set; }

        /// <summary>
        /// Indicates what action should be taken when a selection of
        /// cells is selected and the Delete key is pressed.
        /// This has no correlation to how DataGridView handles the
        /// Delete key when the full row has been selected.
        /// </summary>
        public DeleteKeyBehaviours DeleteKeyBehaviour
        {
            get { return _deleteKeyBehaviour; }
            set { _deleteKeyBehaviour = value; }
        }

        /// <summary>
        /// If deletion is to be confirmed, checks deletion with the user before continuing.
        /// This applies only to the default delete behaviour where a full row is selected
        /// by clicking on the column.
        /// </summary>
        private void ConfirmRowDeletion(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (ConfirmDeletion && !CheckUserConfirmsDeletionDelegate())
                {
                    e.Cancel = true;
                    return;
                }
                IBusinessObject businessObject = this.DataSetProvider.Find(e.Row.Index);
                if (businessObject == null)
                {
                    //this.RefreshGrid();
                    //GlobalUIRegistry.ControlFactory.ShowMessageBox(
                    //    "There was a problem deleting the selected item please try again");

                    _log.Debug("ConfirmRowDeletion - Row Index :" + e.Row.Index + " - No business object found");
                    e.Cancel = true;
                    return;
                }
                string message;
                if (!businessObject.IsDeletable(out message))
                {
                    e.Cancel = true;
                    throw new BusObjDeleteException(businessObject, message);
                }
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        /// <summary>
        /// A Microsoft-suggested override to catch key presses, since KeyPress does
        /// not work correctly on DataGridView
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Delete)
            {
                DeleteKeyHandler();
                return this.ProcessDeleteKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// A Microsoft-suggested override to catch key presses, since KeyPress does
        /// not work correctly on DataGridView
        /// </summary>
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteKeyHandler();
                return this.ProcessDeleteKey(e.KeyData);
            }
            return base.ProcessDataGridViewKey(e);
        }

        /// <summary>
        /// Carries out actions when the delete key on the keyboard is pressed
        /// </summary>
        public void DeleteKeyHandler()
        {
            if (CurrentCell == null || CurrentCell.IsInEditMode || SelectedRows.Count != 0) return;
            if (_deleteKeyBehaviour == DeleteKeyBehaviours.DeleteRow && AllowUserToDeleteRows)
            {
                if (!ConfirmDeletion || (ConfirmDeletion && CheckUserConfirmsDeletionDelegate()))
                {
                    ArrayList rowIndexes = new ArrayList();
                    foreach (IDataGridViewCell cell in SelectedCells)
                    {
                        if (!rowIndexes.Contains(cell.RowIndex) && cell.RowIndex != NewRowIndex)
                        {
                            rowIndexes.Add(cell.RowIndex);
                        }
                    }
                    rowIndexes.Sort();

                    for (int row = rowIndexes.Count - 1; row >= 0; row--)
                    {
                        Rows.RemoveAt((int) rowIndexes[row]);
                    }
                }
            }
            else if (_deleteKeyBehaviour == DeleteKeyBehaviours.ClearContents)
            {
                foreach (IDataGridViewCell cell in SelectedCells)
                {
                    cell.Value = null;
                }
            }
        }
#pragma warning disable 168
        /// <summary>
        /// Carries out additional actions when a cell is clicked.  Specifically, if
        /// a combobox cell is clicked, the cell goes into edit mode immediately.
        /// </summary>
        public void CellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            bool setToEditMode = CheckIfComboBoxShouldSetToEditMode(e.ColumnIndex, e.RowIndex);
            if (!setToEditMode) return;
            DataGridViewColumn dataGridViewColumn =
                ((DataGridViewColumnWin) Columns[e.ColumnIndex]).DataGridViewColumn;
            ControlsHelper.SafeGui(this, () => BeginEdit(true));
            if (EditingControl is DataGridViewComboBoxEditingControl)
            {
                ((DataGridViewComboBoxEditingControl) EditingControl).DroppedDown = true;
            }
        }
#pragma warning restore 168

        /// <summary>
        /// Displays a message box to the user to check if they want to proceed with
        /// deleting the selected rows.
        /// </summary>
        /// <returns>Returns true if the user does want to delete</returns>
        public static bool CheckUserWantsToDelete()
        {
            return
                MessageBox.Show
                    ("Are you sure you want to delete the selected row(s)?", "Delete?", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// Checks whether this is a comboboxcolumn and whether it should
        /// begin edit immediately (to circumvent the pain of having to click
        /// a cell multiple times to edit the value).  This method is typically
        /// called by the cell click handler.
        /// </summary>
        /// <remarks>
        /// This method was extracted from the handler in order to make testing
        /// possible, since calling BeginEdit at testing time causes an STA thread
        /// error.
        /// </remarks>
        public bool CheckIfComboBoxShouldSetToEditMode(int columnIndex, int rowIndex)
        {
            if (columnIndex > -1 && rowIndex > -1 && !CurrentCell.IsInEditMode && this.ComboBoxClickOnce)
            {
                DataGridViewColumn dataGridViewColumn =
                    ((DataGridViewColumnWin) Columns[columnIndex]).DataGridViewColumn;
                if (dataGridViewColumn is DataGridViewComboBoxColumn)
                {
                    return true;
                }
            }
            return false;
        }
    }
}