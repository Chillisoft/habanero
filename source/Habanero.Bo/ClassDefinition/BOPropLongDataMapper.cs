using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// Implements a data mapper for a long Property
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="BOPropDataMapper"/>.
    ///</summary>
    public class BOPropLongDataMapper : BOPropDataMapper
    {
        /// <summary>
        /// This mapper method will convert any valid guid object to an 
        ///   Invariant long string of format .ToString("B").ToUpperInvariant().
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ConvertValueToString(object value)
        {
            if (value == null) return "";
            if (value is long)
            {
                long longValue     = ((long) value);
                return longValue.ToString();
            }
            object parsedPropValue;
            TryParsePropValue(value, out parsedPropValue);
            if (parsedPropValue is long)
            {
                return (parsedPropValue.ToString());
            }
            return parsedPropValue == null ? "" : parsedPropValue.ToString();
        }

        /// <summary>
        /// This mapper method will convert any valid long string to a valid long object 
        ///   (ParsePropValue) and will convert a DBNull to a null value to null.
        /// </summary>
        /// <param name="valueToParse">value to convert.</param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (!(valueToParse is long))
            {
                if (valueToParse is IBusinessObject)
                {
                    IBusinessObject bo = (IBusinessObject) valueToParse;
                    if (bo.ID.GetAsValue() is long)
                    {
                        returnValue = bo.ID.GetAsValue();
                        return true;
                    }
                    returnValue = null;
                    return false;
                }
                long longValue;
                if (long.TryParse(Convert.ToString(valueToParse), out longValue))
                {
                    returnValue = longValue;
                    return true;
                }
                if (valueToParse is decimal)
                {
                    decimal decimalValue = (decimal)valueToParse;
                    if (decimalValue >= long.MinValue && decimalValue <= long.MaxValue)
                    {
                        returnValue = Convert.ToInt64(valueToParse);
                        return true;
                    }
                }
                returnValue = null;
                return false;
            }

            returnValue = valueToParse;
            return true;
        }

    }
}