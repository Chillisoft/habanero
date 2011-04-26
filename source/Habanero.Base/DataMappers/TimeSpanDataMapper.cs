using System;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    /// <summary>
    /// Used to implement custom type conversions specifically for DateTime objects.
    /// </summary>
    public class TimeSpanDataMapper : DataMapper
    {
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (valueToParse is TimeSpan)
            {
                returnValue = valueToParse;
                return true;
            }

            object dateTimeOutput;
            var result = DateTimeUtilities.TryParseValue(valueToParse, out dateTimeOutput);
            if (result)
            {
                returnValue = ((DateTime) dateTimeOutput).TimeOfDay;
                return true;
            }
            return false;
        }
    }
}