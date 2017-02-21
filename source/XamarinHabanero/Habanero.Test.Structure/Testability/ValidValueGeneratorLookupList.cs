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

    public class ValidValueGeneratorLookupList : ValidValueGenerator
    {
        public ValidValueGeneratorLookupList(ISingleValueDef propDef)
            : base(propDef)
        {
        }

        public override object GenerateValidValue()
        {
            var lookupList = base.SingleValueDef.LookupList;
            var generateValidValue = GetLookupListValue(lookupList);
            if (generateValidValue == null && lookupList is BusinessObjectLookupList)
            {
                BusinessObjectLookupList boLList = (BusinessObjectLookupList)lookupList;
                var boTestFactory = BOTestFactoryRegistry.Instance.Resolve(boLList.BoType);
                IBusinessObject businessObject = boTestFactory.CreateSavedBusinessObject();
                generateValidValue = businessObject.ID.GetAsValue();
            }
            object value;
            IPropDef propDef = this.SingleValueDef as IPropDef;
            if (propDef == null) return generateValidValue;
            var tryParsePropValue = propDef.TryParsePropValue(generateValidValue, out value);
            return tryParsePropValue ? value : generateValidValue;
        }

        private static object GetLookupListValue(ILookupList lookupList)
        {
            return (((lookupList == null) || (lookupList is NullLookupList)) ? null : RandomValueGen.GetRandomLookupListValue(lookupList.GetLookupList()));
        }
    }
}
