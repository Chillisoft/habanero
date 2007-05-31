using System;
using System.Collections;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Compares two business objects on the Guid property specified 
    /// in the constructor
    /// </summary>
    public class GuidComparer : IComparer
    {
        private readonly string itsPropName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the Guid property on
        /// which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The Guid property name on which two
        /// business objects will be compared</param>
        public GuidComparer(string propName)
        {
            itsPropName = propName;
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
            BusinessObject boLeft = (BusinessObject) x;
            BusinessObject boRight = (BusinessObject) y;
            Guid left;
            Guid right;
            if (boLeft.GetPropertyValue(itsPropName) == null)
            {
                left = Guid.Empty;
            }
            else
            {
                left = (Guid) boLeft.GetPropertyValue(itsPropName);
            }
            if (boRight.GetPropertyValue(itsPropName) == null)
            {
                right = Guid.Empty;
            }
            else
            {
                right = (Guid) boRight.GetPropertyValue(itsPropName);
            }
            return left.CompareTo(right);
        }
    }
}