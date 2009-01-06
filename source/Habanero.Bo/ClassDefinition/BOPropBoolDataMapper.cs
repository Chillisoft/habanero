using System;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    internal class BOPropBoolDataMapper : BOPropDataMapper
    {
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            bool result;
            bool success;
            try
            {
                success = StringUtilities.BoolTryParse(valueToParse, out result);
            }
            catch (HabaneroDeveloperException)
            {
                returnValue = null;
                return false;
            }
            returnValue = result;
            return success;
        }
    }
}