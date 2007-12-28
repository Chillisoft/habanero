using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Util
{
    ///<summary>
    /// General Utilities
    ///</summary>
    public static class Utilities
    {
        ///<summary>
        /// This method tests the reference passed in to see if it is null or not.
        /// It inspects the actual memory location of the object's pointer to see if it is null or not.
        /// This is useful in the case where you need to test for null without using the == operator.
        ///</summary>
        ///<param name="obj">The object to be tested it it is null or not.</param>
        ///<returns>True if the object is null, or false if not.</returns>
        public static bool IsNull(object obj)
        {
            WeakReference testNull = new WeakReference(obj);
            return !testNull.IsAlive;
        }
    }
}
