using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    ///<summary>
    /// data mapper that is used for  reading and writing byte[] (Byte array) from a database
    ///</summary>
    public class ByteArrayDataMapper : DataMapper
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