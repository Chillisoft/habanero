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

namespace Habanero.BO.Comparer
{
	/// <summary>
    /// Compares two business objects on the Double property specified 
    /// in the constructor
    /// </summary>
	public class DoubleComparer<T> : PropertyComparer<T, Double>
		where T : BusinessObject
	{

		/// <summary>
		/// Constructor to initialise a comparer, specifying the property
		/// on which two business objects will be compared using the Compare()
		/// method for the specified type
		/// </summary>
		/// <param name="propName">The property name on which two
		/// business objects will be compared</param>
		public DoubleComparer(string propName)
			: base(propName)
		{
		}
	}
}
