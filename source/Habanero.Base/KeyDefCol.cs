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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Habanero.Base
{
    /// <summary>
    /// Maintains a collection of key definitions (KeyDef objects)
    /// </summary>
    public class KeyDefCol : IEnumerable<IKeyDef>
    {
        private readonly Dictionary<string, IKeyDef> _keyDefs;

        /// <summary>
        /// A basic constructor that sets up an empty collection
        /// </summary>
        public KeyDefCol() : base()
        {
            _keyDefs = new Dictionary<string, IKeyDef>();
        }

        /// <summary>
        /// Adds a key definition to the collection
        /// </summary>
        /// <param name="keyDef"></param>
        public void Add(IKeyDef keyDef)
        {
            if (Contains(keyDef))
            {
                throw new ArgumentException
                    (String.Format("A key definition with the name '{0}' already " + "exists.", keyDef.KeyName));
            }
            _keyDefs.Add(keyDef.KeyName, keyDef);
        }

        /// <summary>
        /// Adds multiple <see cref="IKeyDef"></see> objects to the collection
        /// </summary>
        /// <param name="keyDefs"></param>
        public void AddRange(IEnumerable<IKeyDef> keyDefs)
        {
            if (keyDefs == null) throw new ArgumentNullException("keyDefs");
            foreach (var keyDef in keyDefs.Where(keyDef => !this.Contains(keyDef.KeyName)))
            {
                this.Add(keyDef);
            }
        }


        /// <summary>
        /// Removes a key definition from the collection
        /// </summary>
        /// <param name="keyDef">The Key Definition to remove</param>
        protected void Remove(IKeyDef keyDef)
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
        protected bool Contains(IKeyDef keyDef)
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
        public IKeyDef this[string keyName]
        {
            get
            {
                if (!Contains(keyName))
                {
                    throw new ArgumentException
                        (String.Format
                             ("The key name '{0}' does not exist in the " + "collection of key definitions.", keyName));
                }
                return _keyDefs[keyName];
            }
        }

        /// <summary>
        /// returns the key def at the index/
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IKeyDef GetKeyDefAtIndex(int index)
        {
            int counter = 0;
            foreach (IKeyDef def in this)
            {
                if (counter == index)
                {
                    return def;
                }
                counter++;
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Creates a new collection of business object keys (BOKey)
        /// using the key definitions in this collection.
        /// </summary>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns>Returns a new BOKey collection object containing a mirror
        /// of this key definition collection</returns>
        public BOKeyCol CreateBOKeyCol(IBOPropCol lBOPropCol)
        {
            BOKeyCol lBOKeyCol = new BOKeyCol();
            foreach (IKeyDef lKeyDef in this)
            {
                lBOKeyCol.Add(lKeyDef.CreateBOKey(lBOPropCol));
            }
            return lBOKeyCol;
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return _keyDefs.Values.GetEnumerator();
        //}

        /// <summary>
        /// Gets the count of items in this collection
        /// </summary>
        public int Count
        {
            get { return _keyDefs.Count; }
        }

        #region IEnumerable<IKeyDef> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<IKeyDef> IEnumerable<IKeyDef>.GetEnumerator()
        {
            return _keyDefs.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _keyDefs.Values.GetEnumerator();
        }

        #endregion

        ///<summary>
        /// The ClassDef that these KeyDefs belong to.
        ///</summary>
        public IClassDef ClassDef { get; set; }


    }
}