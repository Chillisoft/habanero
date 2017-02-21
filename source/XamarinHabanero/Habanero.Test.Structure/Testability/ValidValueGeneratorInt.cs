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

using Habanero.Test.Structure;

namespace Habanero.Testability
{
    using Habanero.Base;
    using Habanero.BO;
    using System;

    /// <summary>
    /// Generates a Valid Value for an Integer (int or Int32) based on the min or max value in the PropDef
    /// </summary>
    public class ValidValueGeneratorInt : ValidValueGenerator, IValidValueGeneratorNumeric
    {
        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="propDef"></param>
        public ValidValueGeneratorInt(IPropDef propDef) : base(propDef)
        {
        }

        public override object GenerateValidValue()
        {
            return this.GenerateValidValue(null, null);
        }

        private int GenerateValidValue(int? overridingMinValue, int? overridingMaxValue)
        {
            var propRule = base.GetPropRule<PropRuleInteger>();
            var intMinValue = GetMinValue(propRule, overridingMinValue);
            var intMaxValue = GetMaxValue(propRule, overridingMaxValue);
            return RandomValueGen.GetRandomInt(intMinValue, intMaxValue);
        }

        public object GenerateValidValueGreaterThan(object minValue)
        {
            return this.GenerateValidValue((int?)minValue, null);
        }

        public object GenerateValidValueLessThan(object maxValue)
        {
            return this.GenerateValidValue(null, (int?)maxValue);
        }

        private static int GetMaxValue(IPropRuleComparable<int> propRule, int? overridingMaxValue)
        {
            return RandomValueGen.GetMaxValue(propRule, overridingMaxValue);
        }

        private static int GetMinValue(IPropRuleComparable<int> propRule, int? overridingMinValue)
        {
            return RandomValueGen.GetMinValue(propRule, overridingMinValue);
        }
    }
}
