using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    internal class BOPropDateTimeDataMapper : BOPropDataMapper
    {
        public override string ConvertValueToString(object value)
        {
            if (value == null) return "";
            object parsedPropValue;
            TryParsePropValue(value, out parsedPropValue);
            if (parsedPropValue is DateTime) return ((DateTime) parsedPropValue).ToString(_standardDateTimeFormat);

            return parsedPropValue == null ? "" : parsedPropValue.ToString();
        }

        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (!(valueToParse is DateTime))
            {
                if (valueToParse is DateTimeToday || valueToParse is DateTimeNow)
                {
                    returnValue = valueToParse;
                    return true;
                }
                if (valueToParse is String)
                {
                    string stringValueToConvert = (string)valueToParse;
                    if (stringValueToConvert.ToUpper() == "TODAY")
                    {
                        returnValue = new DateTimeToday();
                        return true;
                    }
                    if (stringValueToConvert.ToUpper() == "NOW")
                    {
                        returnValue = new DateTimeNow();
                        return true;
                    }
                }
                DateTime dateTimeOut;
                if (DateTime.TryParse(valueToParse.ToString(), out dateTimeOut))
                {
                    returnValue = dateTimeOut;
                    return true;
                }
                returnValue = null;
                return false;
            }
            returnValue = valueToParse;
            return true;
        }
    }
}