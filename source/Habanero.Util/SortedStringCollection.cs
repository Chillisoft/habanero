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

namespace Habanero.Util
{
    /// <summary>
    /// Manages a collection of strings that is always alphabetically sorted
    /// </summary>
    public class SortedStringCollection : ICollection
    {
        private readonly IList _list;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public SortedStringCollection()
        {
            _list = new ArrayList();
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        /// Returns the number of items held in the collection
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Returns the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        /// <summary>
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to search at</param>
        /// <returns>Returns the string found at the specified position</returns>
        public string this[int index]
        {
            get { return (string) _list[index]; }
        }

        /// <summary>
        /// Adds a string to the collection, inserting it at the appropriate
        /// sorted position
        /// </summary>
        /// <param name="s">The string to add</param>
        public void Add(string s)
        {
            if (_list.Count == 0)
            {
                _list.Add(s);
            }
            else
            {
                int i = 0;
                while (i < _list.Count && String.Compare(this[i], s) < 0)
                {
                    i++;
                }
                if (i == _list.Count)
                {
                    if (String.Compare(this[i - 1], s) > 0)
                    {
                        _list.Insert(i - 1, s);
                    }
                    else
                    {
                        _list.Add(s);
                    }
                }
                else
                {
                    _list.Insert(i, s);
                }
            }
        }
    }
}