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
using System.Text;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of BOProp objectsn (See <see cref="IBOProp"/>)
    /// Typically this collection is created when the <see cref="IBusinessObject"/>
    ///  is created. The collection is created based on Property Definition (<see cref="IPropDef"/>)
    ///  for the Class definition (<see cref="IClassDef"/>) that defines the Business Object (<see cref="IBusinessObject"/>).
    /// This collection in should thus not be used by the application developer.
    /// This collection is typically controlled by an <see cref="IBusinessObject"/>
    /// </summary>
    public class BOPropCol 
    {
        private readonly Dictionary<string, IBOProp> _boProps;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public BOPropCol()
        {
            _boProps = new Dictionary<string, IBOProp>();
        }

        /// <summary>
        /// Adds a property to the collection
        /// </summary>
        /// <param name="prop">The property to add</param>
        public void Add(IBOProp prop)
        {
            if (Contains(prop.PropertyName.ToUpper()))
            {
                throw new InvalidPropertyException(String.Format(
                    "The BOProp with the name '{0}' is being added to the " +
                    "prop collection, but already exists in the collection.",
                    prop.PropertyName));
            }
            _boProps.Add(prop.PropertyName.ToUpper(), prop);
        }



        /// <summary>
        /// Copies the properties from another collection into this one
        /// </summary>
        /// <param name="propCol">A collection of properties</param>
        public void Add(BOPropCol propCol)
        {
            foreach (IBOProp prop in propCol.Values)
            {
                this.Add(prop);
            }
        }

        /// <summary>
        /// Remove a specified property from the collection
        /// </summary>
        /// <param name="propName">The property name</param>
        public void Remove(string propName)
        {
            _boProps.Remove(propName.ToUpper());
        }

        /// <summary>
        /// Indicates whether the collection contains the property specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if found</returns>
        public bool Contains(string propName)
        {
            return _boProps.ContainsKey(propName.ToUpper());
        }

        /// <summary>
        /// Provides an indexing facility so the contents of the collection
        /// can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The name of the property to access</param>
        /// <returns>Returns the property if found, or null if not</returns>
        public IBOProp this[string propName]
        {
            get
            {
                if (!Contains(propName.ToUpper()))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "A BOProp with the name '{0}' does not exist in the " +
                        "prop collection.", propName));
                }
                return _boProps[propName.ToUpper()];
            }
        }

        /// <summary>
        /// Returns an xml string containing the properties whose values
        /// have changed, along with their old and new values
        /// </summary>
        public string DirtyXml
        {
            get
            {
                string dirtlyXml = "<Properties>";
                foreach (IBOProp prop in this.SortedValues )
                {
                    if (prop.IsDirty)
                    {
                        dirtlyXml += prop.DirtyXml;
                    }
                }
                dirtlyXml += "</Properties>";
                return dirtlyXml;
            }
        }

        /// <summary>
        /// Restores each of the property values to their PersistedValue
        /// </summary>
        public void RestorePropertyValues()
        {
            foreach (IBOProp prop in this)
            {
                prop.RestorePropValue();
            }
        }

        /// <summary>
        /// Copies across each of the properties' current values to their
        /// persisted values
        /// </summary>
        public void BackupPropertyValues()
        {
            foreach (IBOProp prop in this)
            {
                prop.BackupPropValue();
            }
        }

        /// <summary>
        /// Indicates whether all of the held property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to alter if one or more
        /// property values are invalid</param>
        /// <returns>Returns true if all the property values are valid, false
        /// if any one is invalid</returns>
        public bool IsValid(out string invalidReason)
        {
            bool propsValid = true;
            StringBuilder reason = new StringBuilder();
            foreach (IBOProp prop in this)
            {
                prop.Validate();
                if (!prop.IsValid)
                {
                    reason.Append(prop.InvalidReason + Environment.NewLine);
                    propsValid = false;
                }
            }
            invalidReason = reason.ToString();
            return propsValid;
        }

        /// <summary>
        /// Returns a collection containing all the values being held
        /// </summary>
        public ICollection  Values
        {
            get { return _boProps.Values; }
        }

        /// <summary>
        /// Returns the collection of property values as a SortedList
        /// </summary>
        public IEnumerable SortedValues
        {
            get { return new SortedList(_boProps).Values; }
        }

        ///<summary>
        /// returns an enumerator for a collection of <see cref="IBOProp"/>s
        ///</summary>
        ///<returns></returns>
        public IEnumerator GetEnumerator()
        {
            return SortedValues.GetEnumerator();
        }

        ///<summary>
        /// Returns a count of the number of properties <see cref="IBOProp"/> in the properties collection.
        ///</summary>
        public int Count
        {
            get
            {
                return _boProps.Count;
            }
        }

        /// <summary>
        /// Indicates whether any of the properties in this collection are defined as autoincrementing fields.
        /// An auto incrementing field is a field that relies on the database auto incrementing a number.
        /// E.g. every time a new row is inserted into a table the value of the auto incrementing field is 
        /// incremented. To update the business object accordingly this value needs to be updated to the 
        /// matching property
        /// </summary>
        public bool HasAutoIncrementingField
        {
            get
            {
                foreach (KeyValuePair<string, IBOProp> keyValuePair in _boProps)
                {
                    if (keyValuePair.Value.PropDef.AutoIncrementing) return true;
                }
                return false;
               
            }
        }
    }
}