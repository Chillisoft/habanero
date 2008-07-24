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

using System.Collections;
using System.Data;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class EditableGridWin : GridBaseWin, IEditableGrid
    {
        private bool _confirmDeletion;
        private CheckUserConfirmsDeletion _checkUserConfirmsDeletionDelegate;
        private DeleteKeyBehaviours _deleteKeyBehaviour;

        public EditableGridWin()
        {
            this._confirmDeletion = false;
            this.AllowUserToAddRows = true;
            this._deleteKeyBehaviour = DeleteKeyBehaviours.DeleteRow;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
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
        /// Restore the grid to its previous saved state.
        /// </summary>
        
        public void RejectChanges()
        {
            if (this.DataSource is DataView)
            {
                ((DataView)this.DataSource).Table.RejectChanges();
            }
        }

        /// <summary>
        /// Saves the changes made to the data in the grid.
        /// </summary>
        public void SaveChanges()
        {
            if (this.DataSource is DataView)
            {
                ((DataView)this.DataSource).Table.AcceptChanges();
            }
        }

        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        public bool ConfirmDeletion
        {
            get { return _confirmDeletion; }
            set { _confirmDeletion = value; }
        }

        /// <summary>
        /// Checks whether the user wants to delete selected rows.
        /// </summary>
        public CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate
        {
            get { return _checkUserConfirmsDeletionDelegate; }
            set { _checkUserConfirmsDeletionDelegate = value; }
        }

        /// <summary>
        /// Indicates what action should be taken when a selection of
        /// cells is selected and the Delete key is pressed.  Note that
        /// this has no correlation to how DataGridView handles the
        /// Delete key when the full row has been selected.
        /// </summary>
        public DeleteKeyBehaviours DeleteKeyBehaviour
        {
            get { return _deleteKeyBehaviour; }
            set { _deleteKeyBehaviour = value; }
        }

        /// <summary>
        /// Carries out actions when the delete key is called on the grid
        /// </summary>
        public void DeleteKeyHandler()
        {
            
            if ( !CurrentCell.IsInEditMode)
            {
                if (_deleteKeyBehaviour == DeleteKeyBehaviours.DeleteRow && AllowUserToDeleteRows)
                {
                    if (ConfirmDeletion && CheckUserConfirmsDeletionDelegate())
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
            }
        }
    }
}