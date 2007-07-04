
using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.Bo.Comparer
{
    /// <summary>
    /// Compares two business objects on the integer property specified 
    /// in the constructor
    /// </summary>
    public class IntComparer<T> : IComparer<T> where T: BusinessObject
    {
        private readonly string _propName;
        

        /// <summary>
        /// Constructor to initialise a comparer, specifying the integer property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The integer property name on which two
        /// business objects will be compared</param>
        public IntComparer(string propName)
        {
            _propName = propName;
        }

        /// <summary>
        /// Compares two business objects on the integer property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first integer is less, equal to or more
        /// than the second</returns>
        public int Compare(T x, T y)
        {
            int left;
            int right;
            if (x.GetPropertyValue(_propName) == null)
            {
                left = 0;
            }
            else
            {
                left = (int) x.GetPropertyValue(_propName);
            }
            if (y.GetPropertyValue(_propName) == null)
            {
                right = 0;
            }
            else
            {
                right = (int) y.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}