using System;
using System.Collections;
using System.Text;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Db.v2;
using Chillisoft.Util.v2;
using NUnit.Framework;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Contains the details of the key constraints for the particular
    /// business object.  It is essentially a collection of BOProp objects
    /// that behave together in some way (e.g. for a composite alternate
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public class BOKey : DictionaryBase
    {
        protected KeyDef _keyDef;

        /// <summary>
        /// Constructor to initialise a new instance
        /// </summary>
        /// <param name="lKeyDef">The key definition</param>
        internal BOKey(KeyDef lKeyDef)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(lKeyDef, "lKeyDef");
            _keyDef = lKeyDef;
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
                //if (this.Contains(key)) //TODO_Err: If this does not exist
                return ((BOProp) Dictionary[propName]);
            }
        }

        /// <summary>
        /// Adds a BOProp object to the key
        /// </summary>
        /// <param name="boProp">The BOProp to add</param>
        internal virtual void Add(BOProp boProp)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(boProp, "bOProp");
            //TODO_Err: If this already exist
            Dictionary.Add(boProp.PropertyName, boProp);
        }

        /// <summary>
        /// Returns true if a property with this name is part of this key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if it is contained</returns>
        public bool Contains(string propName)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(propName, "propName");
            return (Dictionary.Contains(propName));
        }

        /// <summary>
        /// Returns a copy of the collection of properties in the key
        /// </summary>
        /// <returns>Returns a new BOProp collection</returns>
        internal BOPropCol GetBOPropCol()
        {
            BOPropCol col = new BOPropCol();
            foreach (BOProp boProp in this.Dictionary.Values)
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
        protected bool IgnoreNulls
        {
            get { return _keyDef.IgnoreNulls; }
        }

        /// <summary>
        /// Indicates whether to check for duplicates.
        /// This will be false when the IgnoreNulls is true and one or more 
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
            if (!IgnoreNulls)
            {
                return true;
            }
            // check each property to determine whether
            // any of them are null if any are null then do not check sincd
            // Ignore nulls is true.
            foreach (DictionaryEntry item in this)
            {
                BOProp lBOProp = (BOProp) item.Value;
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
                foreach (DictionaryEntry item in this)
                {
                    BOProp lBOProp = (BOProp) item.Value;
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
                foreach (DictionaryEntry item in this)
                {
                    BOProp lBOProp = (BOProp) item.Value;
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
            StringBuilder propString = new StringBuilder(this.Count*30);
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
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
            StringBuilder whereClause = new StringBuilder(this.Count*30);
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
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
        /// TODO ERIC - rename to DatabaseWhereClause
        protected internal virtual string DataBaseWhereClause(SqlStatement sql)
        {
            StringBuilder whereClause = new StringBuilder(this.Count*30);
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
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
                if (lhs.Count != rhs.Count)
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
                throw e;
            }
            foreach (DictionaryEntry item in lhs)
            {
                BOProp prop = (BOProp) item.Value;
                if (rhs.Contains(prop.PropertyName))
                {
                    BOProp rhsProp = (BOProp) rhs.Dictionary[prop.PropertyName];
                    if (prop.PropertyValue != rhsProp.PropertyValue)
                    {
                        return false;
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

    #region Tests

    [TestFixture]
    public class BOKeyTester
    {
        private BOPropCol mBOPropCol1 = new BOPropCol();
        private KeyDef mKeyDef1 = new KeyDef();

        private BOPropCol mBOPropCol2 = new BOPropCol();
        private KeyDef mKeyDef2 = new KeyDef();

        [SetUp]
        public void init()
        {
            //Props for KeyDef 1
            PropDef lPropDef = new PropDef("PropName", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            mBOPropCol1.Add(lPropDef.CreateBOProp(false));
            mKeyDef1.Add(lPropDef);

            lPropDef = new PropDef("PropName1", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            mBOPropCol1.Add(lPropDef.CreateBOProp(false));
            mKeyDef1.Add(lPropDef);

            //Props for KeyDef 2

            lPropDef = new PropDef("PropName1", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            mBOPropCol2.Add(lPropDef.CreateBOProp(false));
            mKeyDef2.Add(lPropDef);

            lPropDef = new PropDef("PropName", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            mBOPropCol2.Add(lPropDef.CreateBOProp(false));
            mKeyDef2.Add(lPropDef);
        }

        [Test]
        public void TestBOKeyEqual()
        {
            //Set values for Key1
            BOKey lBOKey1 = mKeyDef1.CreateBOKey(mBOPropCol1);
            BOProp lProp = mBOPropCol1["PropName"];
            lProp.PropertyValue = "Prop Value";

            lProp = mBOPropCol1["PropName1"];
            lProp.PropertyValue = "Value 2";

            //Set values for Key2
            BOKey lBOKey2 = mKeyDef2.CreateBOKey(mBOPropCol2);
            lProp = mBOPropCol2["PropName"];
            lProp.PropertyValue = "Prop Value";

            lProp = mBOPropCol2["PropName1"];
            lProp.PropertyValue = "Value 2";

            Assert.AreEqual(lBOKey1, lBOKey2);
            Assert.IsTrue(lBOKey1 == lBOKey2);

            Assert.AreEqual(lBOKey1.GetHashCode(), lBOKey2.GetHashCode());
        }
    }

    #endregion //Tests
}