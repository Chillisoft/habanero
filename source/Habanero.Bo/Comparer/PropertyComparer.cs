//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.BO.Comparer
{
	/// <summary>
    /// Compares two business objects on the property specified 
    /// in the constructor using the specified generic type
    /// </summary>
    public class PropertyComparer<TBusinessObject, TPropType> : IComparer<TBusinessObject> 
		where TBusinessObject : BusinessObject
		where TPropType : IComparable
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the property
        /// on which two business objects will be compared using the Compare()
		/// method for the specified type
        /// </summary>
        /// <param name="propName">The property name on which two
        /// business objects will be compared</param>
		public PropertyComparer(string propName)
        {
            _propName = propName;
        }

        /// <summary>
        /// Compares two business objects on the property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first business object to compare</param>
        /// <param name="y">The second business object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first property value is less, equal to or more
        /// than the second</returns>
        public int Compare(TBusinessObject x, TBusinessObject y)
        {
        	object left = x.GetPropertyValue(_propName);
			object right = y.GetPropertyValue(_propName);
        	return CompareValues(left, right);
        }

		private static int CompareValues(object left, object right)
		{
			if (left == null)
			{
				if (right != null)
				{
					return -1;
				} else
				{
					return 0;
				}
			} else if (right == null)
			{
				return 1;
			}
			TPropType leftValue = (TPropType)left;
			TPropType rightValue = (TPropType)right;
			return leftValue.CompareTo(rightValue);
		}
    }
}