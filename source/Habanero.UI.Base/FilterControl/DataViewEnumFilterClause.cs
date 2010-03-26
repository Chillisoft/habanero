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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a filter clause that filters which data to
    /// display in a DataView, according to some criteria set on an integer column
    /// </summary>
    public class DataViewEnumFilterClause : DataViewFilterClause
    {
        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to compare to</param>
        internal DataViewEnumFilterClause(string filterColumn, FilterClauseOperator clauseOperator, object filterValue)
            : base(filterColumn, clauseOperator, filterValue)
        {
            if (_clauseOperator == FilterClauseOperator.OpLike)
            {
                throw new HabaneroArgumentException("clauseOperator",
                                                    "Operator Like is not supported for non string operands");
            }
        }

        /// <summary>
        /// Returns the value part of the clause
        /// </summary>
        /// <returns>Returns a string</returns>
        protected override string CreateValueClause(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            return "'" + _filterValue.ToString() + "'";
        }
    }
}