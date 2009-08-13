//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Provides a collection of property definitions.
    /// </summary>
    public interface IPropDefCol : IEnumerable
    {
        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the propertyName is not
        /// found. If you are checking for the existence of a propertyName, use the
        /// Contains() method.</exception>
        IPropDef this[string propertyName] { get; }

        /// <summary>
        /// Gets the number of definitions in this collection
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="isNewObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        IBOPropCol CreateBOPropertyCol(bool isNewObject);

        /// <summary>
        /// Add an existing property definition to the collection
        /// </summary>
        /// <param name="propDef">The existing property definition</param>
        void Add(IPropDef propDef);


        ///<summary>
        /// Clones the propdefcol.  The new propdefcol has the same propdefs in it.
        ///</summary>
        ///<returns></returns>
        IPropDefCol Clone();
        /// <summary>
        /// Clones the propdefcol. This method was created so that you could control the depth of the copy. The reason is so that you can limit the
        ///   extra memory used in cases where the propdef does not need to be copied.
        /// </summary>
        /// <param name="clonePropDefs">If true then makes a full copy of the propdefs else only makes a copy of the propdefcol.</param>
        /// <returns></returns>
        IPropDefCol Clone(bool clonePropDefs);
        /// <summary>
        /// Indicates if the specified property definition exists
        /// in the collection.
        /// </summary>
        /// <param name="propDef">The Property definition to search for</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(IPropDef propDef);

        /// <summary>
        /// Indicates if a property definition with the given key exists
        /// in the collection.
        /// </summary>
        /// <param name="propertyName">The propertyName to match</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(string propertyName);
        /// <summary>
        /// Removes a property definition from the collection
        /// </summary>
        /// <param name="propDef">The Property definition to remove</param>
        void Remove(IPropDef propDef);

        /// <summary>
        /// Adds all the property definitions from the given collection
        /// into this one.
        /// </summary>
        /// <param name="propDefCol">The collection of property definitions</param>
        void Add(IPropDefCol propDefCol);
    }
}