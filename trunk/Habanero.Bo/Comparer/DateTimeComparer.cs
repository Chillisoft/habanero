using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.Bo.Comparer
{
    /// <summary>
    /// Compares two business objects on the date-time property specified 
    /// in the constructor
    /// </summary>
    public class DateTimeComparer<T> : IComparer<T> where T : BusinessObject
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the date-time property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The integer property name on which two
        /// business objects will be compared</param>
        public DateTimeComparer(string propName)
        {
            _propName = propName;
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Compares two business objects on the date-time property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first date is less, equal to or more
        /// than the second</returns>
        public int Compare(T x, T y)
        {
            DateTime left;
            DateTime right;
            if (x.GetPropertyValue(_propName) == null)
            {
                left = DateTime.MinValue;
            }
            else
            {
                left = (DateTime) x.GetPropertyValue(_propName);
            }
            if (y.GetPropertyValue(_propName) == null)
            {
                right = DateTime.MinValue;
            }
            else
            {
                right = (DateTime) y.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}