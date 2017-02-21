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

    public class ValidValueGeneratorDouble : ValidValueGenerator, IValidValueGeneratorNumeric
    {
        public ValidValueGeneratorDouble(IPropDef propDef) : base(propDef)
        {
        }

        public override object GenerateValidValue()
        {
            PropRuleDouble propRule = base.GetPropRule<PropRuleDouble>();
            return ((propRule == null) ? RandomValueGen.GetRandomDouble() : RandomValueGen.GetRandomDouble(propRule.MinValue, propRule.MaxValue));
        }

        private double GenerateValidValue(double? overridingMinValue, double? overridingMaxValue)
        {
            PropRuleDouble propRule = base.GetPropRule<PropRuleDouble>();
            double minValue = GetMinValue(propRule, overridingMinValue);
            double maxValue = GetMaxValue(propRule, overridingMaxValue);
            return RandomValueGen.GetRandomDouble(minValue, maxValue);
        }

        public object GenerateValidValueGreaterThan(object minValue)
        {
            return this.GenerateValidValue((double?)minValue, null);
        }

        public object GenerateValidValueLessThan(object maxValue)
        {
            return this.GenerateValidValue(null, (double?)maxValue);
        }

        private static double GetMaxValue(IPropRuleComparable<double> propRule, double? overridingMaxValue)
        {
            return RandomValueGen.GetMaxValue(propRule, overridingMaxValue);
        }

        private static double GetMinValue(IPropRuleComparable<double> propRule, double? overridingMinValue)
        {
            return RandomValueGen.GetMinValue(propRule, overridingMinValue);
        }
    }
}
