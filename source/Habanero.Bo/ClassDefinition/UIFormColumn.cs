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

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Holds the property definitions for a column of controls in a user 
    /// interface editing form, as specified in the class definitions
    /// xml file
    /// </summary>
    public class UIFormColumn : ICollection
    {
        private readonly IList _list;
        private int _width;

        /// <summary>
        /// Constructor to initialise a new column definition
        /// </summary>
        public UIFormColumn() : this(-1)
        {
        }

        /// <summary>
        /// Constructor to initialise a new column definition with the
        /// specified width
        /// </summary>
        /// <param name="width">The column width</param>
        public UIFormColumn(int width)
        {
            _width = width;
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a form field to the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public void Add(UIFormField field)
        {
            _list.Add(field);
        }

        /// <summary>
        /// Removes a form field from the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public void Remove(UIFormField field)
        {
            _list.Remove(field);
        }

        /// <summary>
        /// Checks if a form field is in the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public bool Contains(UIFormField field)
        {
            return _list.Contains(field);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public UIFormField this[int index]
        {
            get { return (UIFormField)_list[index]; }
        }


        /// <summary>
        /// Returns the number of property definitions held
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
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
        /// Returns the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the definitions are synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        /// <summary>
        /// Gets and sets the column width
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        //		public UIDefName Name {
        //			get { return _name; }
        //			set { _name = value; }
        //		}

        ///<summary>
        /// Clones the collection.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public UIFormColumn Clone()
        {
            UIFormColumn newPropDefCol = new UIFormColumn();
            foreach (UIFormField def in this)
            {
                newPropDefCol.Add(def);
            }
            return newPropDefCol;
        }

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            UIFormColumn otherFormColumn = obj as UIFormColumn;
            if ((object)otherFormColumn == null)
            {
                return false;
            }
            if (otherFormColumn.Count != this.Count)
            {
                return false;
            }
            if  (otherFormColumn.Width != this.Width) return false;
            foreach (UIFormField field in this)
            {
                bool found = false;
                foreach (UIFormField otherFormField in otherFormColumn)
                {
                    if (otherFormField.Equals(field))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        ///<summary>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIFormColumn a, UIFormColumn b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        ///<summary>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator !=(UIFormColumn a, UIFormColumn b)
        {
            return !(a == b);
        }

    }
}