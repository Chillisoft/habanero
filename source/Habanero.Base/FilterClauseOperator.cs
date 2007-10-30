//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.Base
{
    /// <summary>
    /// An enumeration that provides an operator for filter clauses, which
    /// allow only rows of data to be shown that meet the requirements set by the
    /// filter.  Note that some types of operators may not be appropriate for
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
        OpLessThanOrEqualTo
    }
}
