using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Contains the details of the key constraints for the particular
    /// business object.  It is essentially a collection of BOProp objects
    /// that behave together in some way (e.g. for a composite alternate
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public class BOKey// : DictionaryBase
    {
        private Dictionary<string, BOProp> _props;
        private KeyDef _keyDef;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="lKeyDef">The key definition</param>
        internal BOKey(KeyDef lKeyDef)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(lKeyDef, "lKeyDef");
            _keyDef = lKeyDef;
            _props = new Dictionary<string, BOProp>();
        }

        protected KeyDef KeyDef
        {
            get
            {
                return _keyDef;
            }
        }

        /// <summary>
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        public BOProp this[string propName]
        {
            get
            {
                if (!_props.ContainsKey(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "The given property name '{0}' does not exist in the " +
                        "key collection for this class.",
                        propName));
                }
                return _props[propName];
            }
        }

        /// <summary>
        /// Adds a BOProp object to the key
        /// </summary>
        /// <param name="boProp">The BOProp to add</param>
        internal virtual void Add(BOProp boProp)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(boProp, "bOProp");
            if (_props.ContainsKey(boProp.PropertyName))
            {
                throw new InvalidPropertyException(String.Format(
                    "The property with the name '{0}' that is being added already " +
                    "exists in the key collection.", boProp.PropertyName));
            }
            _props.Add(boProp.PropertyName, boProp);
        }

        /// <summary>
        /// Returns true if a property with this name is part of this key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if it is contained</returns>
        public bool Contains(string propName)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(propName, "propName");
            return (_props.ContainsKey(propName));
        }

        /// <summary>
        /// Returns the number of BOProps in this key.
        /// </summary>
        public int Count
        {
            get
            {
                return _props.Count;
            }
        }

        /// <summary>
        /// Returns a copy of the collection of properties in the key
        /// </summary>
        /// <returns>Returns a new BOProp collection</returns>
        internal BOPropCol GetBOPropCol()
        {
            BOPropCol col = new BOPropCol();
            foreach (BOProp boProp in _props.Values)
            {
                col.Add(boProp);
            }
            return col;
        }

        //      /// <summary>
        //      /// is valid if more the BOKey contains 1 or more properties.
        //      /// </summary>
        //      /// <returns></returns>
        //		internal bool IsValid
        //		{
        //			get{return (Count > 0);}
        //		}
        
        /// <summary>
        /// Returns the ignore-nulls setting, which is used to determine
        /// whether to check for duplicate keys (if this is set to false,
        /// then duplicates will always be checked for)
        /// </summary>
        protected bool IgnoreIfNull
        {
            get { return _keyDef.IgnoreIfNull; }
        }

        /// <summary>
        /// Indicates whether to check for duplicates.
        /// This will be false when the IgnoreIfNull is true and one or more 
        /// of the BOProperties is null.
        /// </summary>
        /// <returns>Returns true if duplicates need to be checked for</returns>
        internal virtual bool MustCheckKey()
        {
            // if the properties have not been edited then ignore them since
            // they could not now cause a duplicate.

            if (!IsDirty && !IsObjectNew)
            {
                return false;
            }

            //If the relevant props are dirty and ignore nulls is false
            // then you must always check for duplicates
            if (!IgnoreIfNull)
            {
                return true;
            }
            // check each property to determine whether
            // any of them are null if any are null then do not check sincd
            // Ignore nulls is true.
            foreach (BOProp lBOProp in _props.Values)
            {
               
                if (lBOProp.PropertyValue == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Indicates whether the properties have been altered
        /// </summary>
        protected internal bool IsDirty
        {
            get
            {
                foreach (BOProp lBOProp in _props.Values)
                {
                    if (lBOProp.IsDirty)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Indicates whether any of the properties are new
        /// </summary>
        protected virtual bool IsObjectNew
        {
            get
            {
                foreach (BOProp lBOProp in _props.Values)
                {
                    if (lBOProp.IsObjectNew)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Returns the key name
        /// </summary>
        protected internal string KeyName
        {
            get { return _keyDef.KeyName; }
        }

        /// <summary>
        /// Returns a string containing all the properties and their values
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            StringBuilder propString = new StringBuilder(_props.Count*30);
            foreach (BOProp prop in _props.Values)
            {
                if (propString.Length > 0)
                {
                    propString.Append(" AND ");
                }
                propString.Append(prop.PropertyName + "=" + prop.PropertyValue);
            }
            return propString.ToString();
        }

        /// <summary>
        /// Creates a "where" clause from the persisted properties held
        /// </summary>
        /// <param name="sql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        protected internal virtual string PersistedDatabaseWhereClause(SqlStatement sql)
        {
            StringBuilder whereClause = new StringBuilder(_props.Count*30);
            foreach (BOProp prop in _props.Values)
            {
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" AND ");
                }
                if (prop.PersistedPropertyValue == null)
                {
                    whereClause.Append(prop.DatabaseNameFieldNameValuePair(sql));
                }
                else
                {
                    whereClause.Append(prop.PersistedDatabaseNameFieldNameValuePair(sql));
                }
            }
            return whereClause.ToString();
        }

        /// <summary>
        /// Creates a "where" clause from the properties held
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <returns>Returns a string</returns>
        protected internal virtual string DatabaseWhereClause(SqlStatement sql)
        {
            StringBuilder whereClause = new StringBuilder(_props.Count*30);
            foreach (BOProp prop in _props.Values)
            {
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" AND ");
                }
                whereClause.Append(prop.DatabaseNameFieldNameValuePair(sql));
            }
            return whereClause.ToString();
        }

        /// <summary>
        /// Indicates whether the two keys provided are equal in content
        /// </summary>
        /// <param name="lhs">The first key to compare</param>
        /// <param name="rhs">The second key to compare</param>
        /// <returns>Returns true if equal</returns>
        public static bool operator ==(BOKey lhs, BOKey rhs)
        {
            try
            {
                if (lhs._props.Count != rhs._props.Count)
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
            foreach (BOProp prop in lhs._props.Values)
            {
                if (rhs.Contains(prop.PropertyName))
                {
                    BOProp rhsProp = rhs[prop.PropertyName];
                    if (prop.PropertyValue != rhsProp.PropertyValue)
                    {
                        if (prop.PropertyValue != null && rhsProp.PropertyValue != null)
                        {
                            if (!prop.PropertyValue.Equals(rhsProp.PropertyValue)) return false;
                        }
                        else {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Indicates whether the two keys provided are different in content
        /// </summary>
        /// <param name="lhs">The first key to compare</param>
        /// <param name="rhs">The second key to compare</param>
        /// <returns>Returns true if the keys differ at some point</returns>
        public static bool operator !=(BOKey lhs, BOKey rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Indicates whether the key provided is equal to this key
        /// </summary>
        /// <param name="obj">The key to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(Object obj)
        {
            if (obj is BOKey)
            {
                return (this == (BOKey) obj);
            }

            return false;
        }

        /// <summary>
        /// Returns a hashcode of the properties string
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return PersistedDatabaseWhereClause(null).GetHashCode();
        }
    }
}