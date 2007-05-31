using System;
using System.Collections;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Compares two business objects on the date-time property specified 
    /// in the constructor
    /// </summary>
    public class DateTimeComparer : IComparer
    {
        private readonly string itsPropName;

        /// <summary>
        /// Constructor to initialise a comparer, specifying the date-time property
        /// on which two business objects will be compared using the Compare()
        /// method
        /// </summary>
        /// <param name="propName">The integer property name on which two
        /// business objects will be compared</param>
        public DateTimeComparer(string propName)
        {
            itsPropName = propName;
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
        public int Compare(object x, object y)
        {
            BusinessObject boLeft = (BusinessObject) x;
            BusinessObject boRight = (BusinessObject) y;
            DateTime left;
            DateTime right;
            if (boLeft.GetPropertyValue(itsPropName) == null)
            {
                left = DateTime.MinValue;
            }
            else
            {
                left = (DateTime) boLeft.GetPropertyValue(itsPropName);
            }
            if (boRight.GetPropertyValue(itsPropName) == null)
            {
                right = DateTime.MinValue;
            }
            else
            {
                right = (DateTime) boRight.GetPropertyValue(itsPropName);
            }
            return left.CompareTo(right);
        }
    }
}