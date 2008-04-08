//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Contains the details of the key constraints for the particular
    /// business object.  It is essentially a collection of BOProp objects
    /// that behave together in some way (e.g. for a composite alternate
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public class BOKey
    {
        private Dictionary<string, BOProp> _props;
        private KeyDef _keyDef;

        /// <summary>
        /// Indicates that the value held by one of the properties in the
        /// key has been changed
        /// </summary>
        public event EventHandler<BOKeyEventArgs> Updated;

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

        /// <summary>
        /// Gets the key definition for this key
        /// </summary>
        public KeyDef KeyDef
        {
            get { return _keyDef; }
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
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="index">The index position of the item to retrieve</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        public BOProp this[int index]
        {
            get
            {
                if (_props.Count < index + 1)
                {
                    throw new IndexOutOfRangeException(String.Format(
                        "A collection of keys is being accessed with the index " +
                        "position '{0}', but the collection does not contain that " +
                        "many items.", index));
                }

                string propName = "";
                int count = 0;
                foreach (KeyValuePair<string, BOProp> pair in _props)
                {
                    if (count == index)
                    {
                        propName = pair.Key;
                        break;
                    }
                    count++;
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
            boProp.Updated += delegate { FireValueUpdated(); };
        }

        /// <summary>
        /// Fires an Updated event
        /// </summary>
        private void FireValueUpdated()
        {
            if (this.Updated != null)
            {
                Updated(this, new BOKeyEventArgs(this));
            }
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
        /// Gets the ignore-if-null setting.  If this is true, then the uniqueness
        /// check on a key is ignored if one of the properties that make up the
        /// key are null.
        /// </summary>
        protected bool IgnoreIfNull
        {
            get { return _keyDef.IgnoreIfNull; }
        }

        /// <summary>
        /// Indicates whether to check for duplicates.
        /// This will be false when IgnoreIfNull is true and one or more 
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

            //If the relevant props are dirty and ignore if null is false
            // then you must always check for duplicates
            if (!IgnoreIfNull)
            {
                return true;
            }
            // check each property to determine whether
            // any of them are null if any are null then do not check sincd
            // Ignore if null is true.
            foreach (BOProp lBOProp in _props.Values)
            {
               
                if (lBOProp.Value == null)
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
                    // Eric: looks faulty
                    //else
                    //{
                    //    return false;
                    //}
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
        /// Returns a sorted list of the property values
        /// </summary>
        public List<BOProp> SortedValues
        {
            get
            {
                List<BOProp> props = new List<BOProp>();
                foreach (KeyValuePair<string, BOProp> prop in _props) {
                    props.Add(prop.Value);
                }
                props.Sort(delegate(BOProp x, BOProp y) { return String.Compare(x.PropertyName, y.PropertyName); });
                return props;
            }
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
                propString.Append(prop.PropertyName + "=" + prop.Value);
            }
            return propString.ToString();
        }

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values at last persistence rather than any dirty values
        /// </summary>
        /// <returns>Returns a string</returns>
        public string PersistedValueString()
        {
            StringBuilder propString = new StringBuilder(_props.Count * 30);
            foreach (BOProp prop in _props.Values)
            {
                if (propString.Length > 0)
                {
                    propString.Append(" AND ");
                }
                propString.Append(prop.PropertyName + "=" + prop.PersistedPropertyValue);
            }
            return propString.ToString();
        }

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values held before the last time they were edited.  This
        /// method differs from PersistedValueString in that the properties may have
        /// been edited several times since their last persistence.
        /// </summary>
        public string PropertyValueStringBeforeLastEdit()
        {
            StringBuilder propString = new StringBuilder(_props.Count * 30);
            foreach (BOProp prop in _props.Values)
            {
                if (propString.Length > 0)
                {
                    propString.Append(" AND ");
                }
                propString.Append(prop.PropertyName + "=" + prop.ValueBeforeLastEdit);
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
            foreach (BOProp prop in SortedValues) {
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
            bool lhsIsNull = Utilities.IsNull(lhs);
            bool rhsIsNull = Utilities.IsNull(rhs);
            if (rhsIsNull && lhsIsNull)
            {
                return true;
            } 
            else if (rhsIsNull || lhsIsNull)
            {
                return false;
            }
            // Niether of the operands are null
            int lhsPropsCount = lhs._props.Count;
            int rhsPropsCount = rhs._props.Count;
            if (lhsPropsCount != rhsPropsCount)
            {
                return false;
            }
            foreach (BOProp prop in lhs._props.Values)
            {
                if (rhs.Contains(prop.PropertyName))
                {
                    BOProp rhsProp = rhs[prop.PropertyName];
                    if (prop.Value != rhsProp.Value)
                    {
                        if (prop.Value != null && rhsProp.Value != null)
                        {
                            if (!prop.Value.Equals(rhsProp.Value)) return false;
                        } 
                        else 
                        {
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