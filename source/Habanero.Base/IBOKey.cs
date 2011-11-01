#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Contains the details of the key constraints for the particular
    /// business object. The Key constraint can be a primary or Alternate Key. 
    /// The primaryKey  or alternate key can both be either 
    /// composite/compound (more than one property) or not (only one property). 
    /// The property can also be a meaningFull Key e.g. Surname or a meaningLess Key e.g PersonID
    /// It is essentially a collection of Business Object Properties <see cref="IBOProp"/>
    ///  objects e.g. FirstName and Surname 
    ///  that behave together in some way (e.g. for a composite alternate
    ///  key, the combination of properties is required to be unique).
    /// </summary>
    public interface IBOKey : IEnumerable<IBOProp>
    {
        /// <summary>
        /// Indicates that the value held by one of the properties in the
        /// key has been changed
        /// </summary>
        event EventHandler<BOKeyEventArgs> Updated;

        /// <summary>
        /// Returns true if a property with this name is part of this key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if it is contained</returns>
        bool Contains(string propName);

        /// <summary>
        /// Returns the number of BOProps in this key.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the key definition for this key
        /// </summary>
        IKeyDef KeyDef
        {
            get;
        }

        /// <summary>
        /// Indicates whether any of the properties of this key are auto incrementing.
        /// </summary>
        bool HasAutoIncrementingProperty { get; }

        /// <summary>
        /// Returns the key name
        /// </summary>
        string KeyName { get; }

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values at last persistence rather than any dirty values
        /// </summary>
        /// <returns>Returns a string</returns>
        string AsString_LastPersistedValue();

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values held before the last time they were edited.  This
        /// method differs from AsString_LastPersistedValue in that the properties may have
        /// been edited several times since their last persistence.
        /// </summary>
        string AsString_PreviousValue();

        /// <summary>
        /// Returns a string containing all the properties and their values, using the current
        /// values of the properties.
        /// </summary>
        string AsString_CurrentValue();

        /// <summary>
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        IBOProp this[string propName] { get;  }

        
        /// <summary>
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="index">The index position of the item to retrieve</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        IBOProp this[int index] { get;  }

        /// <summary>
        /// Returns a copy of the collection of properties in the key
        /// </summary>
        /// <returns>Returns a new BOProp collection</returns>
        IBOPropCol GetBOPropCol();

        /// <summary>
        /// Adds a <see cref="IBOProp"/> to the key
        /// </summary>
        /// <param name="boProp">The BOProp to add</param>
        void Add(IBOProp boProp);
    }
}