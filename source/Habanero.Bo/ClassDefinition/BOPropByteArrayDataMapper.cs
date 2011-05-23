using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    public class BOPropByteArrayDataMapper : BOPropDataMapper
    {
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;
            if (valueToParse is byte[])
            {
                returnValue = valueToParse;
                return true;
            } 
            if (valueToParse is String)
            {
                var bytes = Convert.FromBase64String((string) valueToParse);
                returnValue = bytes;
                return true;
            }
            returnValue = null;
            return false;
        }

        public override string ConvertValueToString(object value)
        {
            if (value is byte[])
            {
                return Convert.ToBase64String((byte[]) value);
            }
            return Convert.ToString(value);
        }
    }
}