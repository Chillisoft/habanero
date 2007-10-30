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
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// A basic lookup-list content provider that stores a collection of
    /// string-Guid pairs as provided in the constructor.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// </summary>
    public class SimpleLookupList : ILookupList
    {
        private Dictionary<string, object> _lookupList;

        /// <summary>
        /// Constructor to initialise the provider with a specified
        /// collection of string-Guid pairs
        /// </summary>
        /// <param name="collection">The string-Guid pair collection</param>
        public SimpleLookupList(Dictionary<string, object> collection)
        {
            _lookupList = collection;
        }

        /// <summary>
        /// Returns the lookup list contents being held
        /// </summary>
        /// <returns>Returns a collection of display-value pairs</returns>
        public Dictionary<string, object> GetLookupList()
        {
            return _lookupList;
        }

        /// <summary>
        /// Returns the lookup list contents being held
        /// </summary>
        /// <param name="connection">Ignored for this lookup list type.</param>
        /// <returns>Returns a collection of display-value pairs</returns>
        public Dictionary<string, object> GetLookupList(IDatabaseConnection connection)
        {
            return _lookupList;
        }

    }
}