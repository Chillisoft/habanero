#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using Habanero.Base;
using System;
using System.Linq;
using Habanero.BO.Rules;

namespace Habanero.Testability
{
    /// <summary>
    /// This is a base class for a valid value generator of any type.
    /// Generally the <see cref="IPropDef"/>'s <see cref="IPropRule"/>s will 
    /// be taken into consideration when determining the valid value.
    /// E.g. Int, Decimal.
    /// Although this class is public it is primarily intended to be used internally by 
    /// the <see cref="BOTestFactory{TBO}"/> and <see cref="BOTestFactory"/>.
    /// </summary>
    public abstract class ValidValueGenerator
    {
        protected ValidValueGenerator(ISingleValueDef singleValueDef)
        {
            if (singleValueDef == null) throw new ArgumentNullException("singleValueDef");
            this.SingleValueDef = singleValueDef;
        }
        /// <summary>
        /// Generates a valid value taking into account only the <see cref="IPropRule"/>s. I.e. any <see cref="InterPropRule"/>s 
        /// will not be taken into account. The <see cref="IValidValueGeneratorNumeric"/>'s methods are used
        /// by the BOTestFactory to create valid values taking into account InterPropRules
        /// </summary>
        /// <returns></returns>
        public abstract object GenerateValidValue();
        protected TRule GetPropRule<TRule>() where TRule : IPropRule
        {
            return this.SingleValueDef.PropRules.OfType<TRule>().FirstOrDefault();
        }

        public ISingleValueDef SingleValueDef { get; private set; }
    }


}
