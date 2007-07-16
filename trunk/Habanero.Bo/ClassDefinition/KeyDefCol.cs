using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Bo;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Maintains a collection of key definitions (KeyDef objects)
    /// </summary>
    public class KeyDefCol
    {
        private Dictionary<string, KeyDef> _keyDefs;

        /// <summary>
        /// A basic constructor that sets up an empty collection
        /// </summary>
        public KeyDefCol() : base()
        {
            _keyDefs = new Dictionary<string, KeyDef>();
        }

        /// <summary>
        /// Adds a key definition to the collection
        /// </summary>
        /// <param name="keyDef"></param>
        public void Add(KeyDef keyDef)
        {
            if (Contains(keyDef))
            {
                throw new ArgumentException(String.Format(
                    "A key definition with the name '{0}' already " +
                    "exists.", keyDef.KeyName));
            }
            _keyDefs.Add(keyDef.KeyName, keyDef);
        }

		/// <summary>
		/// Removes a key definition from the collection
		/// </summary>
		/// <param name="keyDef">The Key Definition to remove</param>
		protected void Remove(KeyDef keyDef)
		{
			if (Contains(keyDef))
			{
                _keyDefs.Remove(keyDef.KeyName);
			}
		}

		/// <summary>
		/// Indicates if the specified Key Definition exists
		/// in the collection.
		/// </summary>
		/// <param name="keyDef">The Key Definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		protected bool Contains(KeyDef keyDef)
		{
		    return (_keyDefs.ContainsValue(keyDef));
		}

        /// <summary>
        /// Indicates whether the collection contains the key definition specified
        /// </summary>
        /// <param name="keyName">The name of the key definition</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(string keyName)
        {
            return _keyDefs.ContainsKey(keyName);
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="keyName">The name of the key definition</param>
        /// <returns>Returns the definition matching the name
        /// provided or null if none are found</returns>
        protected internal KeyDef this[string keyName]
        {
            get
            {
                if (!Contains(keyName))
                {
                    throw new ArgumentException(String.Format(
                        "The key name '{0}' does not exist in the " +
                        "collection of key definitions.", keyName));
                }
                return _keyDefs[keyName];
            }
        }

        /// <summary>
        /// Creates a new collection of business object keys (BOKey)
        /// using the key definitions in this collection.
        /// </summary>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns>Returns a new BOKey collection object containing a mirror
        /// of this key definition collection</returns>
        public BOKeyCol CreateBOKeyCol(BOPropCol lBOPropCol)
        {
            BOKeyCol lBOKeyCol = new BOKeyCol();
            foreach (KeyDef lKeyDef in this)
            {
                lBOKeyCol.Add(lKeyDef.CreateBOKey(lBOPropCol));
            }
            return lBOKeyCol;
        }

        public IEnumerator GetEnumerator()
        {
            return _keyDefs.Values.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return _keyDefs.Count;
            }
        }

    }
}