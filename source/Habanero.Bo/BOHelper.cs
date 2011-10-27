//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
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
