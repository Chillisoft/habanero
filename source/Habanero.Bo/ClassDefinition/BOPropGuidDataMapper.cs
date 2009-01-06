using System;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// Implements a data mapper for a Guid Property
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="BOPropDataMapper"/>.
    ///</summary>
    public class BOPropGuidDataMapper : BOPropDataMapper
    {
        /// <summary>
        /// This mapper method will convert any valid guid object to an 
        ///   Invariant Guid string of format .ToString("B").ToUpperInvariant().
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ConvertValueToString(object value)
        {
            if (value == null) return "";
            if (value is Guid)
            {
                Guid guidValue = ((Guid) value);
                return guidValue == Guid.Empty ? "" : ToUpperInvariant(guidValue);
            }
            object parsedPropValue;
            TryParsePropValue(value, out parsedPropValue);
            if (parsedPropValue is Guid)
            {
                return (ToUpperInvariant((Guid) parsedPropValue));
            }
            return parsedPropValue == null ? "" : parsedPropValue.ToString();
        }

        /// <summary>
        /// This mapper method will convert any valid Guid string to a valid Guid object 
        ///   (ParsePropValue) and will convert a DBNull to a null value to null.
        /// </summary>
        /// <param name="valueToParse">value to convert.</param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (!(valueToParse is Guid))
            {
                if (valueToParse is IBusinessObject)
                {
                    IBusinessObject bo = (IBusinessObject) valueToParse;
                    if (bo.ID.IsGuidObjectID)
                    {
                        returnValue = bo.ID.GetAsGuid();
                        return true;
                    }
                    returnValue = null;
                    return false;
                }
                Guid guidValue;
                if (StringUtilities.GuidTryParse(valueToParse.ToString(), out guidValue))
                {
                    if (guidValue == Guid.Empty)
                    {
                        returnValue = null;
                        return true;
                    }
                    returnValue = guidValue;
                    return true;
                }
                returnValue = null;
                return false;
            }
            if (valueToParse is Guid && (Guid) valueToParse == Guid.Empty)
            {
                returnValue = null;
                return true;
            }
            returnValue = valueToParse;
            return true;
        }

        private static string ToUpperInvariant(Guid guidValue)
        {
            return guidValue.ToString("B").ToUpperInvariant();
        }
    }
}