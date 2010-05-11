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
using System;
using Habanero.Base;

namespace Habanero.BO.Comparer
{
	/// <summary>
    /// Compares two business objects on the property specified 
    /// in the constructor using the specified generic type
    /// </summary>
    public class PropertyComparer<TBusinessObject, TPropType> : IPropertyComparer<TBusinessObject>
		where TBusinessObject : IBusinessObject
		where TPropType : IComparable
    {
        private  string _propertyName;
	    private  Source _source;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the property
        /// on which two business objects will be compared using the Compare()
		/// method for the specified type
        /// </summary>
        /// <param name="propName">The property name on which two
        /// business objects will be compared</param>
		public PropertyComparer(string propName)
        {
            _propertyName = propName;
            _source = null;
        }

	    /// <summary>
	    /// Gets and sets the name of the property being compared on
	    /// </summary>
	    public string PropertyName
	    {
	        get { return _propertyName; }
            set { _propertyName = value; }
	    }

	    /// <summary>
	    /// Gets and sets the source of data
	    /// </summary>
	    public Source Source
	    {
	        get { return _source; }
            set { _source = value;}
	    }

	    /// <summary>
	    /// Gets and sets the type of the property being compared on
	    /// </summary>
	    public Type PropertyType
	    {
            get { return typeof (TPropType); }
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
        	object left = x.GetPropertyValue(_source, _propertyName);
			object right = y.GetPropertyValue(_source, _propertyName);
        	return CompareValues(left, right);
        }

		private static int CompareValues(object left, object right)
		{
			if (left == null)
			{
			    if (right != null)
				{
					return -1;
				}
			    return 0;
			}
		    if (right == null)
		    {
		        return 1;
		    }
		    TPropType leftValue = (TPropType)left;
			TPropType rightValue = (TPropType)right;
			return leftValue.CompareTo(rightValue);
		}
    }

    
}
