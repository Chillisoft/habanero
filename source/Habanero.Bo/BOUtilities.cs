using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.BO
{
    ///<summary>
    /// Provides utility methods for BusinessObjects.
    ///</summary>
    public static class BOUtilities
    {
        ///<summary>
        /// Returns whether the BusinessObject is Archived or not.
        ///</summary>
        ///<param name="businessObject">The businessObject to check</param>
        ///<returns>the value of the internal Businessobject.IsArchived() method.</returns>
        public static bool IsArchived(BusinessObject businessObject)
        {
            return businessObject.IsArchived();
        }
    }
}
