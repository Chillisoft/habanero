//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a collection of sql statements
    /// </summary>
    public interface ISqlStatementCollection : IEnumerable<ISqlStatement>
    {
        /// <summary>
        /// Returns the number of statements in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to check</param>
        /// <returns>Returns the sql statement at the index position
        /// chosen</returns>
        ISqlStatement this[int index] { get; }

    	/// <summary>
    	/// Adds a sql statement object to the collection
    	/// </summary>
    	/// <param name="statement">The sql statement object</param>
    	void Add(ISqlStatement statement);

    	/// <summary>
    	/// Adds the contents of another sql statement collection into this
    	/// collection
    	/// </summary>
    	/// <param name="statementCollection">The other collection</param>
    	void Add(ISqlStatementCollection statementCollection);

    	/// <summary>
    	/// Inserts a sql statement object at the position specified
    	/// </summary>
    	/// <param name="index">The position to insert at</param>
    	/// <param name="sql">The sql statement object to add</param>
    	void Insert(int index, ISqlStatement sql);
    }
}