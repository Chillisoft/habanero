using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.BO;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides a collection of property definitions.
    /// </summary>
    public class PropDefCol : IEnumerable<PropDef>
    {
        private Dictionary<string, PropDef> _propDefs;

        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public PropDefCol()
        {
            _propDefs = new Dictionary<string, PropDef>();
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the key is not
        /// found. If you are checking for the existence of a key, use the
        /// Contains() method.</exception>
        public PropDef this[string key]
        {
            get
            {
                if (!Contains(key.ToUpper()))
                {
                    throw new ArgumentException(String.Format(
                        "The property name '{0}' does not exist in the " +
                        "collection of property definitions.", key));
                }
                return (_propDefs[key.ToUpper()]);

                //else

                //return new PropDef("","",key,;
                //	Throw (New Exception( obj.PropertyName + " is already in this BOProperty Collection",   "obj", e));
            }
        }

        /// <summary>
        /// Add an existing property definition to the collection
        /// </summary>
        /// <param name="propDef">The existing property definition</param>
        public void Add(PropDef propDef)
        {
            CheckPropNotAlreadyAdded(propDef.PropertyName);
            _propDefs.Add(propDef.PropertyName.ToUpper(), propDef);
        }

        ///// <summary>
        ///// Create a new property definition and add it to the collection
        ///// </summary>
        ///// <param name="propName">The name of the property, e.g. surname</param>
        ///// <param name="propType">The type of the property, e.g. string</param>
        ///// <param name="propRWStatus">Rules for how a property can be
        ///// accessed. See PropReadWriteRule enumeration for more detail.</param>
        ///// <param name="databaseFieldName">The database field name - this
        ///// allows you to have a database field name that is different to the
        ///// property name, which is useful for migrating systems where
        ///// the database has already been set up</param>
        ///// <param name="defaultValue">The default value that a property 
        ///// of a new object will be set to</param>
        ///// <returns>Returns the new definition created, after it has
        ///// been added to the collection</returns>
        internal PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           string databaseFieldName,
                           object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           databaseFieldName, defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        ///// <summary>
        ///// Creates and adds a new property definition as before, but 
        ///// assumes the database field name is the same as the property name.
        ///// </summary>
        internal PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

		/// <summary>
		/// Removes a property definition from the collection
		/// </summary>
		/// <param name="propDef">The Property definition to remove</param>
		protected void Remove(PropDef propDef)
		{
			if (Contains(propDef))
			{
				_propDefs.Remove(propDef.PropertyName.ToUpper());
			}
		}

		/// <summary>
		/// Indicates if the specified property definition exists
		/// in the collection.
		/// </summary>
		/// <param name="propDef">The Property definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		protected bool Contains(PropDef propDef)
		{
			return (_propDefs.ContainsKey(propDef.PropertyName.ToUpper()));
		}

		/// <summary>
		/// Indicates if a property definition with the given key exists
		/// in the collection.
		/// </summary>
		/// <param name="key">The key to match</param>
		/// <returns>Returns true if found, false if not</returns>
		public bool Contains(string key)
		{
			return (_propDefs.ContainsKey(key.ToUpper()));
		}

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="newObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        public BOPropCol CreateBOPropertyCol(bool newObject)
        {
            BOPropCol lBOPropertyCol = new BOPropCol();
            foreach (PropDef lPropDef in this)
            {
                lBOPropertyCol.Add(lPropDef.CreateBOProp(newObject));
                if (lPropDef.AutoIncrementing) {
                    lBOPropertyCol.AutoIncrementingPropertyName = lPropDef.PropertyName;
                }
            }
            return lBOPropertyCol;
        }

		//public IEnumerator GetEnumerator()
		//{
		//    return _propDefs.Values.GetEnumerator();
		//}

        /// <summary>
        /// Checks if a property definition with that name has already been added
        /// and throws an exception if so
        /// </summary>
        /// <param name="propName">The property name</param>
        private void CheckPropNotAlreadyAdded(string propName)
        {
            if (Contains(propName) || Contains(propName.ToUpper()))
            {
                throw new ArgumentException(String.Format(
                    "A property definition with the name '{0}' already " +
                    "exists.", propName));
            }
        }

        public int Count
        {
            get
            {
                return _propDefs.Count;
            }
        }

		#region IEnumerable<PropDef> Members

		IEnumerator<PropDef> IEnumerable<PropDef>.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion
    }
}