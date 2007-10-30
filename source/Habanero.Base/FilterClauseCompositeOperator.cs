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
    /// An enumeration that provides an operator used by a composite filter
    /// clauses to connect individual filter clauses
    /// </summary>
    public enum FilterClauseCompositeOperator
    {
        /// <summary>
        /// An "and" operator to connect filter clauses
        /// </summary>
        OpAnd,
        /// <summary>
        /// An "or" operator to connect filter clauses
        /// </summary>
        OpOr
    }
}
