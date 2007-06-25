using System;
using System.Collections;
using Habanero.Base;

namespace Habanero.Util.Comparer
{
    /// <summary>
    /// Compares two business objects on the Guid property specified 
    /// in the constructor
    /// </summary>
    public class GuidComparer : IComparer
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
        public int Compare(object x, object y)
        {
            IBusinessObject boLeft = (IBusinessObject) x;
            IBusinessObject boRight = (IBusinessObject) y;
            Guid left;
            Guid right;
            if (boLeft.GetPropertyValue(_propName) == null)
            {
                left = Guid.Empty;
            }
            else
            {
                left = (Guid) boLeft.GetPropertyValue(_propName);
            }
            if (boRight.GetPropertyValue(_propName) == null)
            {
                right = Guid.Empty;
            }
            else
            {
                right = (Guid) boRight.GetPropertyValue(_propName);
            }
            return left.CompareTo(right);
        }
    }
}