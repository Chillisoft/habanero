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

using System.Data;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
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
    }
}