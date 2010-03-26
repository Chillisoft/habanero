// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of relationship definitions
    /// </summary>
    public interface IRelationshipDefCol : IEnumerable
    {
        /// <summary>
        /// Add an existing relationship to the collection
        /// </summary>
        /// <param name="relationshipDef">The existing relationship to add</param>
        void Add(IRelationshipDef relationshipDef);

        /// <summary>
        /// Indicates whether the collection contains the relationship
        /// definition specified
        /// </summary>
        /// <param name="keyName">The name of the definition</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(string keyName);

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["marriage"])
        /// </summary>
        /// <param name="relationshipName">The name of the relationship to
        /// access</param>
        /// <returns>Returns the relationship definition that matches the
        /// name provided</returns>
        IRelationshipDef this[string relationshipName] { get; }

        /// <summary>
        /// Gets the count of items in this collection
        /// </summary>
        int Count { get; }
    }
}