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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a collection of controls
    /// </summary>
    public class ControlCollection : CollectionBase
    {
        /// <summary>
        /// An indexing facility for the collection so that it can be
        /// accessed like an array with square brackets
        /// </summary>
        /// <param name="index">The numerical index position</param>
        /// <returns>Returns the control at the position specified</returns>
        public IControlChilli this[int index]
        {
            get { return ((IControlChilli) List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds a control to the collection
        /// </summary>
        /// <param name="value">The control to add</param>
        /// <returns>Returns the position at which the control was added</returns>
        public int Add(IControlChilli value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Provides the index position of the control specified
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        public int IndexOf(IControlChilli value)
        {
            return (List.IndexOf(value));
        }

        /// <summary>
        /// Insert a control at a specified index position
        /// </summary>
        /// <param name="index">The index position at which to insert</param>
        /// <param name="value">The control to insert</param>
        public void Insert(int index, IControlChilli value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified control from the collection
        /// </summary>
        /// <param name="value">The control to remove</param>
        public void Remove(IControlChilli value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Indicates whether the collection contains the specified control
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns a boolean indicating whether that control is 
        /// found in the collection</returns>
        public bool Contains(IControlChilli value)
        {
            return (List.Contains(value));
        }

        /// <summary>
        /// Carries out additional checks and preparation before inserting
        /// a new control
        /// </summary>
        /// <param name="index">The position at which to insert</param>
        /// <param name="value">The control to insert</param>
        protected override void OnInsert(int index, Object value)
        {
            if (!(value == null) && !(value is IControlChilli))
            {
                throw new ArgumentException("value must be of type IControlChilli.", "value");
            }
        }

        /// <summary>
        /// Carries out additional checks and preparation when removing 
        /// a control
        /// </summary>
        /// <param name="index">The index position at which the control 
        /// may be found</param>
        /// <param name="value">The control to remove</param>
        protected override void OnRemove(int index, Object value)
        {
            if (!(value is IControlChilli))
            {
                throw new ArgumentException("value must be of type IControlChilli.", "value");
            }
        }

        /// <summary>
        /// Carries out additional checks and preparation before allowing
        /// a control in the collection to be reassigned
        /// </summary>
        /// <param name="index">The index position at which the control
        /// may be found</param>
        /// <param name="oldValue">The previous control</param>
        /// <param name="newValue">The new control to replace it</param>
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (!(newValue is IControlChilli))
            {
                throw new ArgumentException("newValue must be of type IControlChilli.", "newValue");
            }
        }

        /// <summary>
        /// Checks that the specified object is a type of Control and not null
        /// </summary>
        /// <param name="value">The control to validate</param>
        /// <exception cref="ArgumentException">Thrown if validation fails
        /// </exception>
        protected override void OnValidate(Object value)
        {
            if (!(value == null) && !(value is IControlChilli))
            {
                throw new ArgumentException("value must be of type IControlChilli.");
            }
        }
    }
}