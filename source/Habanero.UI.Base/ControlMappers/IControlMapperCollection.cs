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
using System;
using System.Collections;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a collection of mappers that are sub-types of ControlMapper
    /// </summary>
    public interface IControlMapperCollection
    {
        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        void CopyTo(Array array, int index);

        /// <summary>
        /// Returns the number of objects in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the collection's synchronisation root
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        bool IsSynchronized { get; }

        /// <summary>
        /// Provides an enumerator of the collection
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Provides an indexing facility so that the collection can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to read</param>
        /// <returns>Returns the mapper at the position specified</returns>
        IControlMapper this[int index] { get; }

        /// <summary>
        /// Provides an indexing facility as before, but allows the objects
        /// to be referenced using their property names instead of their
        /// numerical position
        /// </summary>
        /// <param name="propertyName">The property name of the object</param>
        /// <returns>Returns the mapper if found, or null if not</returns>
        IControlMapper this[string propertyName] { get; }

        /// <summary>
        /// Adds a mapper object to the collection
        /// </summary>
        /// <param name="mapper">The object to add, which must be a type or
        /// sub-type of ControlMapper</param>
        void Add(IControlMapper mapper);

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Updates the business object for 
        /// every control mapper in this collection.
        /// </summary>
        BusinessObject BusinessObject { get; set; }

        /// <summary>
        /// Enables or disables all the controls managed in this control mapper collection.
        /// </summary>
        bool ControlsEnabled { set; }

        /// <summary>
        /// Updates the business object properties being mapped by the mappers in this collection
        /// </summary>
        void ApplyChangesToBusinessObject();
    }
}