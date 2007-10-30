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

using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a lookup-list content provider
    /// </summary>
    public interface ILookupList
    {
        /// <summary>
        /// Returns the contents of a lookup-list
        /// </summary>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        Dictionary<string, object> GetLookupList();

        /// <summary>
        /// Returns the contents of a lookup-list using the database 
        /// connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        Dictionary<string, object> GetLookupList(IDatabaseConnection connection);
    }
}