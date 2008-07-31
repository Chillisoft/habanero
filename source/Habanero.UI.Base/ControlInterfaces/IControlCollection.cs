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

using System.Collections;

namespace Habanero.UI.Base
{
    public interface IControlCollection : IEnumerable
    {
        /// <summary>
        /// An indexing facility for the collection so that it can be
        /// accessed like an array with square brackets
        /// </summary>
        /// <param name="index">The numerical index position</param>
        /// <returns>Returns the control at the position specified</returns>
        IControlChilli this[int index] { get;  }

        int Count { get; }

        /// <summary>
        /// Adds a control to the collection
        /// </summary>
        /// <param name="value">The control to add</param>
        /// <returns>Returns the position at which the control was added</returns>
        void Add(IControlChilli value);

        /// <summary>
        /// Provides the index position of the control specified
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        int IndexOf(IControlChilli value);

        /// <summary>
        /// Insert a control at a specified index position
        /// </summary>
        /// <param name="index">The index position at which to insert</param>
        /// <param name="value">The control to insert</param>
        void Insert(int index, IControlChilli value);

        /// <summary>
        /// Removes the specified control from the collection
        /// </summary>
        /// <param name="value">The control to remove</param>
        void Remove(IControlChilli value);

        /// <summary>
        /// Indicates whether the collection contains the specified control
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns a boolean indicating whether that control is 
        /// found in the collection</returns>
        bool Contains(IControlChilli value);

        void Clear();
    }
}