using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    public class ImageDataMapper : DataMapper
    {
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;
            if (valueToParse is Image)
            {
                returnValue = valueToParse;
                return true;
            } 
            if (valueToParse is byte[])
            {
                returnValue = SerialisationUtilities.ByteArrayToObject((byte[])valueToParse);
                return true;
            }
            if (valueToParse is String)
            {
                var bytes = Convert.FromBase64String((string) valueToParse);
                returnValue = new Bitmap(new MemoryStream(bytes));
                return true;
            }
            returnValue = null;
            return false;
        }

        public override string ConvertValueToString(object value)
        {
            if (value is Image)
            {
                var image = (Image) value;
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                var bytes = stream.ToArray();
                return Convert.ToBase64String(bytes);
            }
            return Convert.ToString(value);
        }
    }
}