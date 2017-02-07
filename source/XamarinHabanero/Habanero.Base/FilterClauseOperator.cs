#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
namespace Habanero.Base
{
    /// <summary>
    /// An enumeration that provides an operator for filter clauses, which
    /// allow only rows of data to be shown that meet the requirements set by the
    /// filter.  Note_ that some types of operators may not be appropriate for
    /// certain data types, such as "OpLike" for an integer.
    /// </summary>
    public enum FilterClauseOperator
    {
        /// <summary>
        /// The data matches the filter value exactly
        /// </summary>
        OpEquals,
        /// <summary>
        /// The data contains the filter value
        /// </summary>
        OpLike,
        /// <summary>
        /// The data is greater than or equal to the filter value
        /// </summary>
        OpGreaterThanOrEqualTo,
        /// <summary>
        /// The data is less than or equal to the filter value
        /// </summary>
        OpLessThanOrEqualTo,
        /// <summary>
        /// The data is greater than the filter value
        /// </summary>
        OpGreaterThan,
        /// <summary>
        /// The data is less than the filter value
        /// </summary>
        OpLessThan,
        /// <summary>
        /// E.g. Is Null and Is Not Null.
        /// </summary>
        Is,
        /// <summary>
        /// NotEqual i.e. != .
        /// </summary>
        OpNotEqual

    }
}
