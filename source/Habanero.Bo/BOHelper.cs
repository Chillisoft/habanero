using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Habanero.Base;

namespace Habanero.BO
{
    ///<summary>
    /// A helper class that allows access to internal or hidden details about an <see cref="BusinessObject"/>.
    /// This is currently only used in unit tests so that we can access the rule collection and verify that they are
    /// set up correctly for a particular business object without having to make the method public.
    ///</summary>
    public class BOHelper
    {
        ///<summary>
        /// Returns a read only collection of the rules that have been set up for the <see cref="BusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="BusinessObject"/> for which the rules collection will be returned.</param>
        ///<returns>A read only collection of the rules for the specified <see cref="BusinessObject"/>.</returns>
        public static ReadOnlyCollection<IBusinessObjectRule> GetBusinessObjectRules(BusinessObject businessObject)
        {
            if (businessObject == null) throw new ArgumentNullException("businessObject");
            return new ReadOnlyCollection<IBusinessObjectRule>(businessObject.GetBusinessObjectRules());
        }
    }
}
