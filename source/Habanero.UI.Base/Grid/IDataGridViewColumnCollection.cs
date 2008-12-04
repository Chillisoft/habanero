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

using System.Collections.Generic;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a collection of DataGridViewColumn objects in a DataGridView control.
    /// </summary>
    public interface IDataGridViewColumnCollection : IEnumerable<IDataGridViewColumn>
    {
        /// <summary>
        /// Gets the number of columns held in this collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears the collection
        /// </summary>
        void Clear();

        /// <summary>
        /// Adds a column to the collection where the column has been
        /// wrapped using the IDataGridViewColumn pattern
        /// </summary>
        void Add(IDataGridViewColumn dataGridViewColumn);

        /// <summary>
        /// Adds a DataGridViewTextBoxColumn with the given column name and column header text to the collection
        /// </summary>
        /// <returns>The index of the column</returns>
        int Add(string columnName, string headerText);

        /// <summary>
        /// Gets or sets the column at the given index in the collection
        /// </summary>
        IDataGridViewColumn this[int index] { get;}

        /// <summary>
        /// Gets or sets the column of the given name in the collection
        /// </summary>
        IDataGridViewColumn this[string name] { get;}
    }
}
