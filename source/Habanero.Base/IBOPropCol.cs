//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of IBOProp objects (See <see cref="IBOProp"/>)
    /// Typically this collection is created when the <see cref="IBusinessObject"/>
    ///  is created. The collection is created based on Property Definition (<see cref="IPropDef"/>)
    ///  for the Class definition (<see cref="IClassDef"/>) that defines the Business Object (<see cref="IBusinessObject"/>).
    /// This collection in should thus not be used by the application developer.
    /// This collection is typically controlled by an <see cref="IBusinessObject"/>
    /// </summary>
    public interface IBOPropCol: IEnumerable<IBOProp>
    {
        /// <summary>
        /// Adds a property to the collection
        /// </summary>
        /// <param name="boProp">The property to add</param>
        void Add(IBOProp boProp);

        /// <summary>
        /// Copies the properties from another collection into this one
        /// </summary>
        /// <param name="propCol">A collection of properties</param>
        void Add(IBOPropCol propCol);

        /// <summary>
        /// Remove a specified property from the collection
        /// </summary>
        /// <param name="propName">The property name</param>
        void Remove(string propName);

        /// <summary>
        /// Indicates whether the collection contains the property specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if found</returns>
        bool Contains(string propName);

        /// <summary>
        /// Provides an indexing facility so the contents of the collection
        /// can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The name of the property to access</param>
        /// <returns>Returns the property if found, or null if not</returns>
        IBOProp this[string propName] { get; }

        /// <summary>
        /// Returns an xml string containing the properties whose values
        /// have changed, along with their old and new values
        /// </summary>
        string DirtyXml { get; }

        /// <summary>
        /// Returns a collection containing all the values being held
        /// </summary>
        ICollection Values { get; }

        /// <summary>
        /// Returns the collection of property values as a SortedList
        /// </summary>
        IEnumerable SortedValues { get; }

        ///<summary>
        /// Returns a count of the number of properties <see cref="IBOProp"/> in the properties collection.
        ///</summary>
        int Count { get; }

        /// <summary>
        /// Indicates whether any of the properties in this collection are defined as autoincrementing fields.
        /// An auto incrementing field is a field that relies on the database auto incrementing a number.
        /// E.g. every time a new row is inserted into a table the value of the auto incrementing field is 
        /// incremented. To update the business object accordingly this value needs to be updated to the 
        /// matching property
        /// </summary>
        bool HasAutoIncrementingField { get; }

        /// <summary>
        /// Returns an xml string containing the properties whose values
        /// have changed, along with their old and new values
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Restores each of the property values to their PersistedValue
        /// </summary>
        void RestorePropertyValues();

        /// <summary>
        /// Copies across each of the properties' current values to their
        /// persisted values
        /// </summary>
        void BackupPropertyValues();

        /// <summary>
        /// Indicates whether all of the held property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to alter if one or more
        /// property values are invalid</param>
        /// <returns>Returns true if all the property values are valid, false
        /// if any one is invalid</returns>
        bool IsValid(out string invalidReason);

        /// <summary>
        /// Indicates whether all of the held property values are valid
        /// </summary>
        /// <param name="errors">A list of <see cref="IBOError"/> describing all the errors in this IBOPropCol</param>
        /// <returns>Returns true if all the property values are valid, false
        /// if any one is invalid</returns>
        bool IsValid(out IList<IBOError> errors);
    }
}