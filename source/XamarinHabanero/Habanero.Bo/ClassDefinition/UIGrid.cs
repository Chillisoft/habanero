#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages property definitions for a user interface grid, as specified
    /// in the class definitions xml file
    /// </summary>
    public class UIGrid : IUIGrid
    {
        private readonly ArrayList _list;

        /// <summary>
        /// Constructor to initialise a new collection of definitions
        /// </summary>
        public UIGrid()
        {
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        public void Add(IUIGridColumn prop)
        {
            _list.Add(prop);
            prop.UIGrid = this;
        }

        /// <summary>
        /// Removes a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        public void Remove(IUIGridColumn prop)
        {
            _list.Remove(prop);
        }

        /// <summary>
        /// Checks if a grid property definition is in the Grid definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        public bool Contains(IUIGridColumn prop)
        {
            return _list.Contains(prop);
        }
		
        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public IUIGridColumn this[int index]
        {
            get { return (UIGridColumn)_list[index]; }
        }		
        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public IUIGridColumn this[string propName]
        {
            get
            {
                foreach (UIGridColumn column in _list)
                {
                    if (column.PropertyName == propName)
                    {
                        return column;
                    }
                }
                return null;
            }
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
        /// The column on which rows are ordered initially.
        /// Indicate the direction by adding " asc" or " desc"
        /// after the column name (" asc" is assumed if left out).  If this
        /// property is not specified, rows will be listed in the order
        /// they were added to the database.
        /// </summary>
        public string SortColumn { get; set; }

        ///<summary>
        /// The definition of the filter that will be used for this grid.
        ///</summary>
        public IFilterDef FilterDef { get; set; }

        ///<summary>
        /// The UI Def that this UIForm is related to.
        ///</summary>
        public IUIDef UIDef { get; set; }

        /// <summary>
        /// The <see cref="IClassDef"/> for this UIGrid.
        /// </summary>
        public IClassDef ClassDef
        {
            get
            {
                return this.UIDef == null ? null : this.UIDef.ClassDef;
            }
        }

        IEnumerator<IUIGridColumn> IEnumerable<IUIGridColumn>.GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return (IUIGridColumn) item;
            }
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
            if (obj == null) return false;
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(this, obj)) return true;

            UIGrid otherUiGrid = obj as UIGrid;
            if (otherUiGrid == null) return false;
            if (otherUiGrid.SortColumn != this.SortColumn) return false;

            if (this.Count != otherUiGrid.Count) return false;
            foreach (UIGridColumn tab in this)
            {
                bool found = false;
                foreach (UIGridColumn otherTab in otherUiGrid)
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
        /// overloads the operator == 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIGrid a, UIGrid b)
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
        public static bool operator !=(UIGrid a, UIGrid b)
        {
            return !(a == b);
        }

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        public UIGrid Clone()
        {
            UIGrid newUIGrid = new UIGrid();
            newUIGrid.SortColumn = this.SortColumn;
            foreach (UIGridColumn column in this)
            {
                newUIGrid.Add(column.Clone());
            }
            return newUIGrid;
        }
        /// <summary>
        /// Determines whether this object is equal to obj.
        /// </summary>
        /// <param name="obj">The object being compared to</param>
        /// <returns></returns>
        public bool Equals(IUIGrid obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return this.Equals((object) obj);
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_list != null ? _list.GetHashCode() : 0);
                result = (result * 397) ^ (SortColumn != null ? SortColumn.GetHashCode() : 0);
                result = (result * 397) ^ (FilterDef != null ? FilterDef.GetHashCode() : 0);
                return result;
            }
        }
    }
}