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

namespace Habanero.BO
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