// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a collection of cells in a DataGridViewRow
    /// </summary>
    public interface IDataGridViewCellCollection
    {
        /// <summary>Adds a cell to the collection.</summary>
        /// <returns>The position in which to insert the new element.</returns>
        /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to add to the collection.</param>
        /// <exception cref="T:System.InvalidOperationException">The row that owns this 
        /// <see cref="IDataGridViewCellCollection"></see> already belongs to a DataGridView control.-or-
        /// dataGridViewCell already belongs to a DataGridViewRow>.</exception>
        /// <filterpriority>1</filterpriority>
        int Add(IDataGridViewCell dataGridViewCell);

        /// <summary>Clears all cells from the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The row that owns this 
        /// <see cref="IDataGridViewCellCollection"></see> already belongs to a DataGridView control.</exception>
        /// <filterpriority>1</filterpriority>
        void Clear();

        /// <summary>Determines whether the specified cell is contained in the collection.</summary>
        /// <returns>true if dataGridViewCell is in the collection; otherwise, false.</returns>
        /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to locate in the collection.</param>
        /// <filterpriority>1</filterpriority>
        bool Contains(IDataGridViewCell dataGridViewCell);

        ///// <summary>Copies the entire collection of cells into an array at a specified location within the array.</summary>
        ///// <param name="array">The destination array to which the contents will be copied.</param>
        ///// <param name="index">The index of the element in array at which to start copying.</param>
        ///// <filterpriority>1</filterpriority>
        //void CopyTo(IDataGridViewCell[] array, int index);

        /// <summary>Returns the index of the specified cell.</summary>
        /// <returns>The zero-based index of the value of dataGridViewCell parameter, if it is found in the collection; otherwise, -1.</returns>
        /// <param name="dataGridViewCell">The cell to locate in the collection.</param>
        /// <filterpriority>1</filterpriority>
        int IndexOf(IDataGridViewCell dataGridViewCell);

        ///// <summary>Inserts a cell into the collection at the specified index. </summary>
        ///// <param name="dataGridViewCell">The <see cref="IDataGridViewCell"></see> to insert.</param>
        ///// <param name="index">The zero-based index at which to place dataGridViewCell.</param>
        ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridViewRow"></see>.</exception>
        ///// <filterpriority>1</filterpriority>
        //void Insert(int index, IDataGridViewCell dataGridViewCell);

        ///// <summary>Removes the specified cell from the collection.</summary>
        ///// <param name="cell">The <see cref="IDataGridViewCell"></see> to remove from the collection.</param>
        ///// <exception cref="T:System.ArgumentException">cell could not be found in the collection.</exception>
        ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.</exception>
        ///// <filterpriority>1</filterpriority>
        //void Remove(IDataGridViewCell cell);

        ///// <summary>Removes the cell at the specified index.</summary>
        ///// <param name="index">The zero-based index of the <see cref="IDataGridViewCell"></see> to be removed.</param>
        ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="T:Gizmox.WebGUI.Forms.DataGridView"></see> control.</exception>
        ///// <filterpriority>1</filterpriority>
        //void RemoveAt(int index);

        /// <summary>Gets or sets the cell at the provided index location. In C#, this property is 
        /// the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
        /// <returns>The <see cref="IDataGridViewCell"></see> stored at the given index.</returns>
        /// <param name="index">The zero-based index of the cell to get or set.</param>
        /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this 
        /// property already belongs to a DataGridView control.-or-The specified cell when setting this 
        /// property already belongs to a DataGridViewRow.</exception>
        /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
        /// <filterpriority>1</filterpriority>
        IDataGridViewCell this[int index] { get;}// set; }

        /// <summary>Gets or sets the cell in the column with the provided name. In C#, this property is 
        /// the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
        /// <returns>The <see cref="IDataGridViewCell"></see> stored in the column with the given name.</returns>
        /// <param name="columnName">The name of the column in which to get or set the cell.</param>
        /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this 
        /// property already belongs to a DataGridView control.-or-The specified cell when setting this 
        /// property already belongs to a DataGridViewRow".</exception>
        /// <exception cref="T:System.ArgumentException">columnName does not match the name of any columns in the control.</exception>
        /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
        /// <filterpriority>1</filterpriority>
        IDataGridViewCell this[string columnName] { get; }//set; }
    }
}