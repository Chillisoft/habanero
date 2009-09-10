//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
    /// Manages property definitions for a tab in a user interface editing 
    /// form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormTab : IEquatable<IUIFormTab>, IUIFormTab
    {
        private IList _list;
        private string _name;
        private IUIFormGrid _uiFormGrid;
        //private UIDefName _name;

        /// <summary>
        /// Constructor to initialise a new tab definition
        /// </summary>
        public UIFormTab() : this("")
        {
        }

        /// <summary>
        /// Constructor to initialise a new tab definition with a tab name
        /// </summary>
        /// <param name="name">The tab name</param>
        public UIFormTab(string name)
        {
            _name = name;
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a column definition to the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        public void Add(IUIFormColumn column)
        {
            column.UIFormTab = this;
            _list.Add(column);
        }

        /// <summary>
        /// Removes a column definition from the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        public void Remove(IUIFormColumn column)
        {
            _list.Remove(column);
        }

        /// <summary>
        /// Checks if a column definition is in the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        public bool Contains(IUIFormColumn column)
        {
            return _list.Contains(column);
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
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public IUIFormColumn this[int index]
        {
            get { return (UIFormColumn)_list[index]; }
        }
				
        /// <summary>
        /// Returns the number of definitions held
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
        /// Indicates whether the definitions are synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
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
        /// Gets and sets the tab name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets and sets the UIFormGrid definition
        /// </summary>
        public IUIFormGrid UIFormGrid
        {
            set { _uiFormGrid = value; }
            get { return _uiFormGrid; }
        }

        /// <summary>
        /// Returns the <see cref="UIForm"/> that this <see cref="UIFormTab"/> is defined for.
        /// </summary>
        public IUIForm UIForm { get; set; }

        /////<summary>
        /////Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /////</summary>
        /////
        /////<returns>
        /////true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /////</returns>
        /////
        /////<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        //public override bool Equals(object obj)
        //{
        //    UIFormTab otherUiTab = obj as UIFormTab;
        //    if (otherUiTab == null) return false;

        //    if (this.Name != otherUiTab.Name) return false;
        //    if (this.Count != otherUiTab.Count) return false;
        //    foreach (UIFormColumn col in this)
        //    {
        //        bool found = false;
        //        foreach (UIFormColumn otherFormCol in otherUiTab)
        //        {
        //            if (otherFormCol.Equals(col))
        //            {
        //                found = true;
        //            }
        //        }
        //        if (!found)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        ///<summary>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIFormTab a, UIFormTab b)
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
        public static bool operator !=(UIFormTab a, UIFormTab b)
        {
            return !(a == b);
        }
        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public IUIFormTab Clone()
        {
            UIFormTab newUIFormTab = new UIFormTab();
            newUIFormTab.Name = this.Name;
            foreach (UIFormColumn column in this)
            {
                newUIFormTab.Add(column.Clone());
            }
            return newUIFormTab;
        }

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///true if the current object is equal to the other parameter; otherwise, false.
        ///</returns>
        ///
        ///<param name="uiFormTab">An object to compare with this object.</param>
        public bool Equals(IUIFormTab uiFormTab)
        {
            if (uiFormTab == null) return false;
//            if (!Equals(_list, uiFormTab._list)) return false;
            if (!Equals(_name, ((UIFormTab)uiFormTab)._name)) return false;
            if (!Equals(_uiFormGrid, ((UIFormTab)uiFormTab)._uiFormGrid)) return false;
            if (this.Count != uiFormTab.Count) return false;
            foreach (UIFormColumn col in this)
            {
                bool found = false;
                foreach (UIFormColumn otherFormCol in uiFormTab)
                {
                    if (otherFormCol.Equals(col))
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
            return Equals(obj as UIFormTab);
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
            result = 29*result + _name.GetHashCode();
            result = 29*result + (_uiFormGrid != null ? _uiFormGrid.GetHashCode() : 0);
            return result;
        }

        ///<summary>
        /// Get the count of the Maximum number of fields
        ///</summary>
        ///<returns></returns>
        public int GetMaxFieldCount()
        {
            int maxFieldCount = 0;
            foreach (UIFormColumn column in this)
            {
                if (column.Count > maxFieldCount)
                    maxFieldCount = column.Count;
            }
            return maxFieldCount;
        }

        ///<summary>
        /// Get the Max rows In the Columns.
        ///</summary>
        ///<returns></returns>
        public int GetMaxRowsInColumns()
        {
            int maxRowsInColumns = 0;
            for (int colNum = 0; colNum < this.Count; colNum++)
            {
                UIFormColumn column = (UIFormColumn) this[colNum];

                int rowsInColumn = column.GetRowsRequired();

                for (int previousColNum = 0; previousColNum < colNum; previousColNum++)
                {
                    rowsInColumn += ((UIFormColumn )this[previousColNum]).GetRowSpanForColumnToTheRight(colNum-previousColNum);
                }

                    if (rowsInColumn > maxRowsInColumns)
                        maxRowsInColumns = rowsInColumn;
            }

            return maxRowsInColumns;
        }
    }
}