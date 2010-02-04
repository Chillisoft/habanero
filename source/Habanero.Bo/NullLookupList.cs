// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
        public Dictionary<string, string> GetLookupList()
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <param name="connection">A parameter preserved for polymorphism.
        /// This can be set to null.</param>
        /// <returns>Returns an empty lookup-list</returns>
        public Dictionary<string, string> GetLookupList(IDatabaseConnection connection)
        {
            return new Dictionary<string, string>();
        }

        ///<summary>
        /// The property definition that this lookup list is for
        ///</summary>
        public IPropDef PropDef { get; set; }

        ///<summary>
        /// Whether to validate that the property set is in this list.  Eg, if the BOProp's value is set to an
        /// item not in the list and this value is True, a validation error will occur upon save.  If this 
        /// value is set to false no validation will occur.
        ///</summary>
        public bool LimitToList
        {
            get { return false; }
        }

        /// <summary>
        /// The TimeOut the time in Milliseconds before the cache expires. I.e. if the current time + Timeout is
        /// less than now then the lookup list will be reloaded else the currently loaded lookup list will be used. 
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Returns the lookup list contents being held where the list is keyed on the list key 
        ///  either a Guid, int or Business object i.e. the value being stored for the property.
        /// The display value can be looked up.
        /// </summary>
        ///<returns>The Key Value Lookup List</returns>
        public Dictionary<string, string> GetIDValueLookupList()
        {
            return new Dictionary<string, string>();
        }
    }
}