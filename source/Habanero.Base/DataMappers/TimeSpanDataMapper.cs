using System;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    /// <summary>
    /// Used to implement custom type conversions specifically for DateTime objects.
    /// </summary>
    public class TimeSpanDataMapper : DataMapper
    {
		///<summary>
		/// The base date for storage of a TimeStamp as a date time value.
		/// This is the 'zero' date for SQL server and should be supported by all data stores.
		///</summary>
		public readonly static DateTime BaseDate = new DateTime(1900,1,1);

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