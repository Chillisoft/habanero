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

using Habanero.Base;

namespace Habanero.UI.Base.FilterControl
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
            if (_leftClause.GetFilterClauseString().Length > 0 && _rightClause.GetFilterClauseString().Length > 0)
            {
                return GetLeftClause() + GetOperatorClause() + GetRightClause();
            }
            else if (_leftClause.GetFilterClauseString().Length > 0)
            {
                return _leftClause.GetFilterClauseString();
            }
            else if (_rightClause.GetFilterClauseString().Length > 0)
            {
                return _rightClause.GetFilterClauseString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the right filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetRightClause()
        {
            return "(" + _rightClause.GetFilterClauseString() + ")";
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

        /// <summary>
        /// Returns the left filter clause surrounded by round brackets
        /// </summary>
        /// <returns>Returns the clause as a string</returns>
        private string GetLeftClause()
        {
            return "(" + _leftClause.GetFilterClauseString() + ")";
        }
    }
}