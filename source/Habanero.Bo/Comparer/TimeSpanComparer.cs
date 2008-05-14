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
    /// Compares timespan properties
    /// </summary>
    /// TODO ERIC - no error checking to make sure property is a timespan
    public class TimeSpanComparer<T> : IComparer<T> where T : BusinessObject
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a new comparer
        /// </summary>
        /// <param name="propName">The name of the property to compare on</param>
        public TimeSpanComparer(string propName)
        {
            _propName = propName;
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Compares two given objects on the property specified in the
        /// constructor
        /// </summary>
        /// <param name="x">The first object to compare (note that comparison
        /// is done on the property name specified in the constructor, not
        /// the object itself)</param>
        /// <param name="y">The second object to compare (note that comparison
        /// is done on the property name specified in the constructor, not
        /// the object itself)</param>
        /// <returns>Returns a negative number, zero or positive number,
        /// depending on whether x's timespan property is less, equal to or 
        /// greater than y's</returns>
        public int Compare(T x, T y)
        {
            TimeSpan left;
            TimeSpan right;
            if (x.GetPropertyValue(_propName) == null)
            {
                left = TimeSpan.MinValue;
            }
            else
            {
                left = (TimeSpan) x.GetPropertyValue(_propName);
            }
            if (y.GetPropertyValue(_propName) == null)
            {
                right = TimeSpan.MinValue;
            }
            else
            {
                right = (TimeSpan) y.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}