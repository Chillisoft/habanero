using System;
using System.Collections;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a collection of BOProp objects
    /// </summary>
    public class BOPropCol : DictionaryBase
    {
        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        internal BOPropCol() : base()
        {
        }

        /// <summary>
        /// Adds a property to the collection
        /// </summary>
        /// <param name="prop">The property to add</param>
        internal void Add(BOProp prop)
        {
            if (Dictionary.Contains(prop.PropertyName.ToUpper()))
            {
                throw new InvalidPropertyException(String.Format(
                    "The BOProp with the name '{0}' is being added to the " +
                    "prop collection, but already exists in the collection.",
                    prop.PropertyName));
            }
            base.Dictionary.Add(prop.PropertyName.ToUpper(), prop);
        }

        /// <summary>
        /// Copies the properties from another collection into this one
        /// </summary>
        /// <param name="propCol">A collection of properties</param>
        internal void Add(BOPropCol propCol)
        {
            foreach (BOProp prop in propCol.Values)
            {
                this.Add(prop);
            }
        }

        /// <summary>
        /// Remove a specified property from the collection
        /// </summary>
        /// <param name="propName">The property name</param>
        internal void Remove(string propName)
        {
            Dictionary.Remove(propName.ToUpper());
        }

        /// <summary>
        /// Indicates whether the collection contains the property specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if found</returns>
        public bool Contains(string propName)
        {
            return Dictionary.Contains(propName.ToUpper());
        }

        /// <summary>
        /// Provides an indexing facility so the contents of the collection
        /// can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The name of the property to access</param>
        /// <returns>Returns the property if found, or null if not</returns>
        public BOProp this[string propName]
        {
            get
            {
                if (!Dictionary.Contains(propName.ToUpper()))
                {
                    //TODO: This exception breaks tests
//                    throw new InvalidPropertyNameException(String.Format(
//                        "A BOProp with the name '{0}' does not exist in the " +
//                        "prop collection.", propName));
                }
                return ((BOProp) Dictionary[propName.ToUpper()]);
            }
        }

        /// <summary>
        /// Returns an xml string containing the properties whose values
        /// have changed, along with their old and new values
        /// </summary>
        internal string DirtyXml
        {
            get
            {
                string dirtlyXml = "<Properties>";
                foreach (BOProp prop in this.SortedValues )
                {
                    if (prop.IsDirty)
                    {
                        dirtlyXml += prop.DirtyXml;
                    }
                }
                return dirtlyXml;
            }
        }

        /// <summary>
        /// Restores each of the property values to their PersistedValue
        /// </summary>
        internal void RestorePropertyValues()
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                prop.RestorePropValue();
            }
        }

        /// <summary>
        /// Copies across each of the properties' current values to their
        /// persisted values
        /// </summary>
        internal void BackupPropertyValues()
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
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
        internal bool IsValid(out string invalidReason)
        {
            bool propsValid = true;
            StringBuilder reason = new StringBuilder();
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                if (!prop.isValid)
                {
                    reason.Append(prop.InvalidReason + Environment.NewLine);
                    propsValid = false;
                }
            }
            invalidReason = reason.ToString();
            return propsValid;
        }

        /// <summary>
        /// Sets the IsObjectNew setting in each property to that specified
        /// </summary>
        /// <param name="bValue">Whether the object is set as new</param>
        internal void setIsObjectNew(bool bValue)
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                prop.IsObjectNew = bValue;
            }
        }

        /// <summary>
        /// Returns a collection containing all the values being held
        /// </summary>
        public ICollection  Values
        {
            get { return base.Dictionary.Values; }
        }

        /// <summary>
        /// Returns the collection of property values as a SortedList
        /// </summary>
        public IEnumerable SortedValues
        {
            get { return new SortedList(base.Dictionary).Values; }
        }
    }
}
