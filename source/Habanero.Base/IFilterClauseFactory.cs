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

using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that creates filter clauses that determine
    /// which rows of data are displayed
    /// </summary>
    public interface IFilterClauseFactory
    {
        /// <summary>
        /// Creates a new filter clause that filters string values
        /// </summary>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateStringFilterClause(string columnName, FilterClauseOperator clauseOperator, string filterValue);

        /// <summary>
        /// Creates a new filter clause that filters integer values
        /// </summary>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateIntegerFilterClause(string columnName, FilterClauseOperator clauseOperator, int filterValue);

        /// <summary>
        /// Creates a new filter clause that filters date values
        /// </summary>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateDateFilterClause(string columnName, FilterClauseOperator clauseOperator, DateTime filterValue);

        /// <summary>
        /// Creates a new composite filter clause combining two given filter
        /// clauses the operator provided
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator, such as
        /// "and" or "or"</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateCompositeFilterClause(IFilterClause leftClause,
                                                 FilterClauseCompositeOperator compositeOperator,
                                                 IFilterClause rightClause);

        /// <summary>
        /// Creates a new null filter clause, which does no filtering
        /// </summary>
        IFilterClause CreateNullFilterClause();
    }
}
