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
using System.Data;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Provides a grid on which the user can edit data and add new business objects directly.
    /// Note that this grid does not provide any buttons or menus for users
    /// to save the changes they have made, and all changes will be lost if the form
    /// is closed and changes are not saved programmatically.  Either carry out a dirty check when the
    /// parent form is closed and take appropriate save action using SaveChanges(), or use an
    /// IEditableGridControl, which provides Save and Cancel buttons. 
    /// </summary>
    /// <remarks>
    /// The support for some tailored features done in the Win versions is not available
    /// here, including customised delete key behaviour and combobox clicking.  Potentially,
    /// the option of confirming deletion before deleting a row could be implemented in the future.
    /// </remarks>
    public class EditableGridGiz : GridBaseGiz, IEditableGrid
    {
        public EditableGridGiz()
        {
            this.AllowUserToAddRows = true;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;

        }

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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Checks whether the user wants to delete selected rows.
        /// </summary>
        public CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Indicates what action should be taken when a selection of
        /// cells is selected and the Delete key is pressed.  Note that
        /// this has no correlation to how DataGridView handles the
        /// Delete key when the full row has been selected, and the default delete
        /// behaviour of the DataGridView is not overridden in this case.
        /// </summary>
        public DeleteKeyBehaviours DeleteKeyBehaviour
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Carries out actions when the delete key is called on the grid
        /// </summary>
        public void DeleteKeyHandler()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets whether clicking on a ComboBox cell causes the drop-down to
        /// appear immediately.  Set this to false if the user should click twice
        /// (first to select, then to edit).
        /// </summary>
        public bool ComboBoxClickOnce
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}