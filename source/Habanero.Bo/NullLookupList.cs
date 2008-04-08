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

using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Provides an empty lookup-list
    /// </summary>
    public class NullLookupList : ILookupList
    {
        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <returns>Returns an empty lookup-list</returns>
        public Dictionary<string, object> GetLookupList()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <param name="connection">A parameter preserved for polymorphism.
        /// This can be set to null.</param>
        /// <returns>Returns an empty lookup-list</returns>
        public Dictionary<string, object> GetLookupList(IDatabaseConnection connection)
        {
            return new Dictionary<string, object>();
        }
    }
}