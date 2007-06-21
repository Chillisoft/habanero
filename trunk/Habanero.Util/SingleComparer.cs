using System;
using System.Collections;
using Habanero.Generic;

namespace Habanero.Util
{
    /// <summary>
    /// Compares two business objects on the Single property specified 
    /// in the constructor
    /// </summary>
    public class SingleComparer : IComparer
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the Single property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The Single property name on which two
        /// business objects will be compared</param>
        public SingleComparer(string propName)
        {
            _propName = propName;
        }

        /// <summary>
        /// Compares two business objects on the Single property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first Single is less, equal to or more
        /// than the second</returns>
        public int Compare(object x, object y)
        {
            IBusinessObject boLeft = (IBusinessObject) x;
            IBusinessObject boRight = (IBusinessObject) y;
            Single left;
            Single right;
            if (boLeft.GetPropertyValue(_propName) == null)
            {
                left = 0;
            }
            else
            {
                left = (Single) boLeft.GetPropertyValue(_propName);
            }
            if (boRight.GetPropertyValue(_propName) == null)
            {
                right = 0;
            }
            else
            {
                right = (Single) boRight.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}