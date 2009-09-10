//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a composite filter clause, which is composed of individual
    /// filter clauses that filter which data to display in a DataView, 
    /// according to some criteria
    /// </summary>
    public class DataViewCompositeFilterClause : IFilterClause
    {
        private readonly IFilterClause _rightClause;
        private readonly FilterClauseCompositeOperator _compositeOperator;
        private readonly IFilterClause _leftClause;

        /// <summary>
        /// Constructor to initialise a new composite filter clause
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The operator to connect the
        /// clauses</param>
        /// <param name="rightClause">The right filter clause</param>
        public DataViewCompositeFilterClause(IFilterClause leftClause, FilterClauseCompositeOperator compositeOperator,
                                             IFilterClause rightClause)
        {
            _leftClause = leftClause;
            _compositeOperator = compositeOperator;
            _rightClause = rightClause;
        }

        /// <summary>
        /// Adds the clauses together to return a complete filter clause string
        /// </summary>
        /// <returns>The completed string</returns>
        public string GetFilterClauseString()
        {
            return GetFilterClauseString("*", "#");
        }

        /// <summary>
        /// Returns the filter clause as a string. The filter clause is a clause used for filtering
        /// a ADO.Net <see cref="DataView"/>
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetFilterClauseString(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            string leftFilterClauseString = _leftClause.GetFilterClauseString(stringLikeDelimiter, dateTimeDelimiter);
            string rightFilterClauseString = _rightClause.GetFilterClauseString(stringLikeDelimiter, dateTimeDelimiter);

            if (leftFilterClauseString.Length > 0 && rightFilterClauseString.Length > 0)
            {
                return GetLeftClause(stringLikeDelimiter, dateTimeDelimiter) + GetOperatorClause() + GetRightClause(stringLikeDelimiter, dateTimeDelimiter);
            }
            if (leftFilterClauseString.Length > 0)
            {
                return leftFilterClauseString;
            }
            if (rightFilterClauseString.Length > 0)
            {
                return rightFilterClauseString;
            }
            return "";
        }
        /// <summary>
        /// Returns the right filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetRightClause(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            return "(" + _rightClause.GetFilterClauseString(stringLikeDelimiter, dateTimeDelimiter) + ")";
        }

        /// <summary>
        /// Returns the left filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetLeftClause(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            return "(" + _leftClause.GetFilterClauseString(stringLikeDelimiter, dateTimeDelimiter) + ")";
        }

        /// <summary>
        /// Returns the operator
        /// </summary>
        /// <returns>Returns the operator</returns>
        private string GetOperatorClause()
        {
            switch (this._compositeOperator)
            {
                case FilterClauseCompositeOperator.OpAnd:
                    return " and ";
                case FilterClauseCompositeOperator.OpOr:
                    return " or ";
                default:
                    return " <unsupported composite operator> ";
            }
        }
    }
}