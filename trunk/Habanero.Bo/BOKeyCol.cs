using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a collection of BOKey objects
    /// </summary>
    public class BOKeyCol 
    {
        private Dictionary<string, BOKey> _boKeys;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        internal BOKeyCol() : base()
        {
            _boKeys = new Dictionary<string, BOKey>();
        }

        /// <summary>
        /// Adds a key to the collection
        /// </summary>
        /// <param name="lBOKey">The BO key</param>
        internal void Add(BOKey lBOKey)
        {
            if (Contains(lBOKey.KeyName))
            {
                throw new InvalidKeyException(String.Format(
                    "A key with the name '{0}' is being added to a key " +
                    "collection but already exists in the collection.",
                    lBOKey.KeyName));
            }
            _boKeys.Add(lBOKey.KeyName, lBOKey);
        }

        /// <summary>
        /// Copies all the keys held in another collection into this collection
        /// </summary>
        /// <param name="keyCol">The other collection</param>
        internal void Add(BOKeyCol keyCol)
        {
            foreach (BOKey key in keyCol)
            {
                this.Add((BOKey)(key));
            }
        }

        //		private bool Contains( string key )  
        //		{
        //			return( Dictionary.Contains( key ) );
        //		}

        /// <summary>
        /// Provides an indexing facility so that this collection can
        /// be accessed with square brackets like an array
        /// </summary>
        /// <param name="boKeyName">The key name</param>
        /// <returns>Returns the BOKey object found with that name, or null
        /// if nothing of that name is matched</returns>
        internal BOKey this[string boKeyName]
        {
            get
            {
                if (!Contains(boKeyName))
                {
                    throw new InvalidKeyException(String.Format(
                        "The key with the name '{0}' does not exist in the " +
                        "collection of keys.", boKeyName));
                }
                return _boKeys[boKeyName];
            }
        }

        /// <summary>
        /// Indicates whether a key with the given name exists in the collection
        /// </summary>
        /// <param name="boKeyName">The key name</param>
        /// <returns>Returns true if so, false if not</returns>
        internal bool Contains(string boKeyName)
        {
            return _boKeys.ContainsKey(boKeyName);
        }

        /// <summary>
        /// Returnst the key collection's enumerator
        /// </summary>
        /// <returns>Returns the enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _boKeys.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets the number of keys in the collection
        /// </summary>
        public int Count
        {
            get
            {
                return _boKeys.Count;
            }
        }
    }
}