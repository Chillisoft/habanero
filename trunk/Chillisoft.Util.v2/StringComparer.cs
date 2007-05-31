using System.Collections;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Compares two business objects on the string property specified 
    /// in the constructor
    /// </summary>
    public class StringComparer : IComparer
    {
        private readonly string itsPropName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the string property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The string property name on which two
        /// business objects will be compared</param>
        public StringComparer(string propName)
        {
            itsPropName = propName;
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
        public int Compare(object x, object y)
        {
            BusinessObject boLeft = (BusinessObject) x;
            BusinessObject boRight = (BusinessObject) y;
            string left = (string) boLeft.GetPropertyValue(itsPropName);
            string right = (string) boRight.GetPropertyValue(itsPropName);
            if (left == null) left = "";
            if (right == null) right = "";
            return left.CompareTo(right);
        }
    }
}