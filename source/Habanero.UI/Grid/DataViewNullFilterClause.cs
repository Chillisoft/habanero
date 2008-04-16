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

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages a null filter clause, which carries out no filtering of data
    /// </summary>
    public class DataViewNullFilterClause : IFilterClause
    {
        /// <summary>
        /// Returns an empty string
        /// </summary>
        /// <returns>Returns a empty string</returns>
        public string GetFilterClauseString()
        {
            return "";
        }
    }
}