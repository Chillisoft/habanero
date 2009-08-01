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
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a collection of property definitions for a user interface
    /// editing form, as specified in the class definitions xml file
    /// </summary>
    public class UIForm : IEquatable<UIForm>, IUIForm
    {
        private readonly IList _list;
        private int _width;
        private int _height;
        private string _title;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        public UIForm()
        {
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a tab to the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        public void Add(IUIFormTab tab)
        {
            tab.UIForm = this;
            _list.Add(tab);
        }

        /// <summary>
        /// Removes a tab from the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        public void Remove(IUIFormTab tab)
        {
            _list.Remove(tab);
        }

        /// <summary>
        /// Checks if the form contains the specified tab
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        public bool Contains(IUIFormTab tab)
        {
            return _list.Contains(tab);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public IUIFormTab this[int index]
        {
            get { return (UIFormTab)_list[index]; }
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
        /// Returns the number of definitions held
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
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
        /// Gets and sets the width
        /// </summary>
        public int Width
        {
            set { _width = value; }
            get { return _width; }
        }

        /// <summary>
        /// Gets and sets the height
        /// </summary>
        public int Height
        {
            set { _height = value; }
            get { return _height; }
        }

        /// <summary>
        /// Gets and sets the heading
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }

        ///<summary>
        /// The UI Def that this UIForm is related to.
        ///</summary>
        public IUIDef UIDef { get; set; }

        ///<summary>
        /// overloads the operator == 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIForm a, UIForm b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
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
        /// overloads the operator != 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator !=(UIForm a, UIForm b)
        {
            return !(a == b);
        }

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public IUIForm Clone()
        {
            UIForm newUIForm = new UIForm();
            newUIForm.Title = this.Title;
            newUIForm.Height = this.Height;
            newUIForm.Width = this.Width;
            foreach (IUIFormTab tab in this)
            {
                newUIForm.Add(((UIFormTab)tab).Clone());
            }
            return newUIForm;
        }

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///true if the current object is equal to the other parameter; otherwise, false.
        ///</returns>
        ///
        ///<param name="otherUIForm">An object to compare with this object.</param>
        public bool Equals(UIForm otherUIForm)
        {
            if (otherUIForm == null) return false;
            if (_width != otherUIForm._width) return false;
            if (_height != otherUIForm._height) return false;
            if (!Equals(_title, otherUIForm._title)) return false;

            if (this.Count != otherUIForm.Count) return false;
            foreach (IUIFormTab tab in this)
            {
                bool found = false;
                foreach (IUIFormTab otherTab in otherUIForm)
                {
                    if (otherTab.Equals(tab))
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
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as UIForm);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            int result = _list.GetHashCode();
            result = 29*result + _width;
            result = 29*result + _height;
            result = 29*result + _title.GetHashCode();
            return result;
        }
    }
}