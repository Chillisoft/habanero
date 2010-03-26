// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a collection of controls
    /// </summary>
    public class ControlCollectionVWG :  IControlCollection
    {
        private readonly Control.ControlCollection _col;

        ///<summary>
        /// Constructs the <see cref="ControlCollectionVWG"/>. This is essentially a wrapper class.
        ///</summary>
        ///<param name="col"></param>
        public ControlCollectionVWG(Control.ControlCollection col)
        {
            _col = col;
        }

        /// <summary>
        /// Indicates the Control at the specified indexed location in the collection
        /// </summary>
        public IControlHabanero this[int index]
        {
            get { return (IControlHabanero)_col[index]; }
        }

        /// <summary>
        /// Gets the number of controls in the collection
        /// </summary>
        public int Count
        {
            get { return _col.Count; }
        }

        /// <summary>
        /// Adds a control to the collection
        /// </summary>
        /// <param name="value">The control to add</param>
        /// <returns>Returns the position at which the control was added</returns>
        public void Add(IControlHabanero value)
        {
            _col.Add((Control) value);
        }

        /// <summary>
        /// Provides the index position of the control specified
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        public int IndexOf(IControlHabanero value)
        {
            return _col.IndexOf((Control) value);
        }

        /// <summary>
        /// Insert a control at a specified index position
        /// </summary>
        /// <param name="index">The index position at which to insert</param>
        /// <param name="value">The control to insert</param>
        public void Insert(int index, IControlHabanero value)
        {
            _col.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified control from the collection
        /// </summary>
        /// <param name="value">The control to remove</param>
        public void Remove(IControlHabanero value)
        {
            _col.Remove((Control) value);
        }

        /// <summary>
        /// Indicates whether the collection contains the specified control
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns a boolean indicating whether that control is 
        /// found in the collection</returns>
        public bool Contains(IControlHabanero value)
        {
            return _col.Contains((Control) value);
        }

        /// <summary>
        /// Removes all controls from the collection
        /// </summary>
        public void Clear()
        {
            _col.Clear();
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return _col.GetEnumerator();
        }
    }
}