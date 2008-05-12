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
using Habanero.Base;


namespace Habanero.UI
{
    /// <summary>
    /// Creates filter clauses that determine which rows of data are displayed
    /// </summary>
    public class DataViewFilterClauseFactory : IFilterClauseFactory
    {
        /// <summary>
        /// Creates a new filter clause that filters string values
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        public IFilterClause CreateStringFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                      string filterValue)
        {
            return new DataViewStringFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Creates a new filter clause that filters integer values
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        public IFilterClause CreateIntegerFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                       int filterValue)
        {
            //BusinessObject b = new BusinessObject();
            //b.Props["test"].PropertyValueString
            return new DataViewIntegerFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Creates a new filter clause that filters integer values
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        public IFilterClause CreateDateFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                    DateTime filterValue)
        {
            //BusinessObject b = new BusinessObject();
            //b.Props["test"].PropertyValueString
            return new DataViewDateFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Creates a new composite filter clause combining two given filter
        /// clauses the operator provided
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator, such as
        /// "and" or "or"</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        public IFilterClause CreateCompositeFilterClause(IFilterClause leftClause,
                                                         FilterClauseCompositeOperator compositeOperator,
                                                         IFilterClause rightClause)
        {
            return new DataViewCompositeFilterClause(leftClause, compositeOperator, rightClause);
        }

        /// <summary>
        /// Creates a new null filter clause, which does no filtering
        /// </summary>
        public IFilterClause CreateNullFilterClause()
        {
            return new DataViewNullFilterClause();
        }
    }
}