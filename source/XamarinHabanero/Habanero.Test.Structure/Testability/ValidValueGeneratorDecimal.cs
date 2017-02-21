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

    public class ValidValueGeneratorDecimal : ValidValueGenerator, IValidValueGeneratorNumeric
    {
        public ValidValueGeneratorDecimal(IPropDef propDef) : base(propDef)
        {
        }

        public override object GenerateValidValue()
        {
            PropRuleDecimal propRule = base.GetPropRule<PropRuleDecimal>();
            return ((propRule == null) ? RandomValueGen.GetRandomDecimal() : RandomValueGen.GetRandomDecimal(propRule.MinValue, propRule.MaxValue));
        }

        private decimal GenerateValidValue(decimal? overridingMinValue, decimal? overridingMaxValue)
        {
            PropRuleDecimal propRule = base.GetPropRule<PropRuleDecimal>();
            decimal minValue = GetMinValue(propRule, overridingMinValue);
            decimal maxValue = GetMaxValue(propRule, overridingMaxValue);
            return RandomValueGen.GetRandomDecimal(minValue, maxValue);
        }

        public object GenerateValidValueGreaterThan(object minValue)
        {
            return this.GenerateValidValue((decimal?)minValue, null);
        }

        public object GenerateValidValueLessThan(object maxValue)
        {
            return this.GenerateValidValue(null, (decimal?)maxValue);
        }

        private static decimal GetMaxValue(IPropRuleComparable<decimal> propRule, decimal? overridingMaxValue)
        {
            return RandomValueGen.GetMaxValue(propRule, overridingMaxValue);
        }

        private static decimal GetMinValue(IPropRuleComparable<decimal> propRule, decimal? overridingMinValue)
        {
            return RandomValueGen.GetMinValue(propRule, overridingMinValue);
        }
    }
}