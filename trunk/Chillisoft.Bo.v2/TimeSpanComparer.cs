using System;
using System.Collections;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Compares timespan properties
    /// </summary>
    /// TODO ERIC - no error checking to make sure property is a timespan
    public class TimeSpanComparer : IComparer
    {
        private readonly string itsPropName;

        /// <summary>
        /// Constructor to initialise a new comparer
        /// </summary>
        /// <param name="propName">The name of the property to compare on</param>
        public TimeSpanComparer(string propName)
        {
            itsPropName = propName;
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Compares two given objects on the property specified in the
        /// constructor
        /// </summary>
        /// <param name="x">The first object to compare (note that comparison
        /// is done on the property name specified in the constructor, not
        /// the object itself)</param>
        /// <param name="y">The second object to compare (note that comparison
        /// is done on the property name specified in the constructor, not
        /// the object itself)</param>
        /// <returns>Returns a negative number, zero or positive number,
        /// depending on whether x's timespan property is less, equal to or 
        /// greater than y's</returns>
        public int Compare(object x, object y)
        {
            BusinessObjectBase boLeft = (BusinessObjectBase) x;
            BusinessObjectBase boRight = (BusinessObjectBase) y;
            TimeSpan left;
            TimeSpan right;
            if (boLeft.GetPropertyValue(itsPropName) == null)
            {
                left = TimeSpan.MinValue;
            }
            else
            {
                left = (TimeSpan) boLeft.GetPropertyValue(itsPropName);
            }
            if (boRight.GetPropertyValue(itsPropName) == null)
            {
                right = TimeSpan.MinValue;
            }
            else
            {
                right = (TimeSpan) boRight.GetPropertyValue(itsPropName);
            }
            return left.CompareTo(right);
        }
    }
}