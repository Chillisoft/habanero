using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.Comparer
{
    /// <summary>
    /// Compares two business objects on the string property specified 
    /// in the constructor
    /// </summary>
    public class StringComparer<T> : IComparer<T> where T : BusinessObject
    {
        private readonly string _propName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the string property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The string property name on which two
        /// business objects will be compared</param>
        public StringComparer(string propName)
        {
            _propName = propName;
        }

        /// <summary>
        /// Compares two business objects on the string property specified in 
        /// the constructor
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>Returns a negative number, zero or a positive number,
        /// depending on whether the first string is less, equal to or more
        /// than the second</returns>
        public int Compare(T x, T y)
        {
            string left = (string) x.GetPropertyValue(_propName);
            string right = (string) y.GetPropertyValue(_propName);
            if (left == null) left = "";
            if (right == null) right = "";
            return left.CompareTo(right);
        }
    }
}