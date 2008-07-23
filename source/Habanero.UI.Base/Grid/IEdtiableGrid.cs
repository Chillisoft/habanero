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
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{

    /// <summary>
    /// Checks whether the user wants to delete selected rows.
    /// </summary>
    public delegate bool CheckUserConfirmsDeletion();


    /// <summary>
    /// Indicates what action should be taken when a selection of
    /// cells is selected and the Delete key is pressed.  Note that
    /// this has no correlation to how DataGridView handles the
    /// Delete key when the full row has been selected.
    /// </summary>
    public enum DeleteKeyBehaviours
    {
        /// <summary>Nothing is done</summary>
        None,
        /// <summary>Each row containing part of the selection is deleted</summary>
        DeleteRow,
        /// <summary>Clears the contents of the selected cells</summary>
        ClearContents
    }

    public interface IEditableGrid:IGridBase
    {
        //IDataGridViewColumnCollection Columns { get; }

        //bool ReadOnly { get; }

        //bool AllowUserToAddRows { get; }

        //bool AllowUserToDeleteRows { get; }
        
        /// <summary>
        /// Restore the grid to its previous saved state.
        /// </summary>
        void RejectChanges();

        /// <summary>
        /// Saves the changes made to the data in the grid.
        /// </summary>
        void SaveChanges();
        
        /// <summary>
        /// Gets or sets the boolean value that determines whether to confirm
        /// deletion with the user when they have chosen to delete a row
        /// </summary>
        bool ConfirmDeletion { get; set; }

        /// <summary>
        /// Checks whether the user wants to delete selected rows.
        /// </summary>
        CheckUserConfirmsDeletion CheckUserConfirmsDeletionDelegate{ get; set;}

        /// <summary>
        /// Indicates what action should be taken when a selection of
        /// cells is selected and the Delete key is pressed.  Note that
        /// this has no correlation to how DataGridView handles the
        /// Delete key when the full row has been selected.
        /// </summary>
        DeleteKeyBehaviours DeleteKeyBehaviour { get; set; }

        /// <summary>
        /// Carries out actions when the delete key is called on the grid
        /// </summary>
        void DeleteKeyHandler();

        
    }
}
