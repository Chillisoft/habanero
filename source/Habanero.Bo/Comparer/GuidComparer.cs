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
using System.Collections.Generic;

namespace Habanero.BO.Comparer
{
    /// <summary>
    /// Compares two business objects on the Guid property specified 
    /// in the constructor
    /// </summary>
    public class GuidComparer<T> : IComparer<T> where T: BusinessObject
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the Guid property on
        /// which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The Guid property name on which two
        /// business objects will be compared</param>
        public GuidComparer(string propName)
        {
            _propName = propName;
        }

        /// <summary>
        /// Compares two business objects on the Guid property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first Guid is less, equal to or more
        /// than the second</returns>
        public int Compare(T x, T y)
        {
            Guid left;
            Guid right;
            if (x.GetPropertyValue(_propName) == null)
            {
                left = Guid.Empty;
            }
            else
            {
                left = (Guid) x.GetPropertyValue(_propName);
            }
            if (y.GetPropertyValue(_propName) == null)
            {
                right = Guid.Empty;
            }
            else
            {
                right = (Guid) y.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}