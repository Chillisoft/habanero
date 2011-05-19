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

    	/// <summary>
    	///  This method provides the functionality to convert any object to the appropriate
    	///    type for this mapper. The default behaviour only handles null values, empty
    	///    strings and DBNull.Value and parses all three to null. This method should be
    	///    overridden in subtypes to parse values to the type you want.
    	///  </summary>
    	/// <param name="valueToParse">The value to be attempted to parse</param>
    	/// <param name="returnValue">the parsed value, if parsing was successful</param>
    	/// <returns>True if the parsing was succesful, false if this mapper was unable to 
    	/// parse the valueToParse.</returns>
    	public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (valueToParse is TimeSpan)
            {
                returnValue = valueToParse;
                return true;
            }

			if (valueToParse is string)
			{
				TimeSpan timeSpan;
				if (TimeSpan.TryParse((string)valueToParse, out timeSpan))
				{
					returnValue = timeSpan;
					return true;
				}
			}

    		object dateTimeOutput;
            var result = DateTimeUtilities.TryParseValue(valueToParse, out dateTimeOutput);
            if (result)
            {
				returnValue = ((DateTime)dateTimeOutput).Subtract(BaseDate);
                return true;
            }
            return false;
        }
    }
}