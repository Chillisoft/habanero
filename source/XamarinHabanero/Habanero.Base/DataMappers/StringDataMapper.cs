#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    ///<summary>
    /// Implements a data mapper for a Guid Property
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="DataMapper"/>.
    ///</summary>
    public  class StringDataMapper : DataMapper
    {
        /// <summary>
        /// This mapper method <see cref="TryParsePropValue"/> will convert any valid Guid string to a valid Guid object 
        ///   (ParsePropValue) and will convert a DBNull to a null value to null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ConvertValueToString(object value)
        {
            if (value == null) return "";
            if (value is Guid)
            {
                Guid guidValue = ((Guid)value);
                //return guidValue == Guid.Empty ? "" : ToUpperInvariant(guidValue);
                return guidValue == Guid.Empty ? "" : guidValue.ToString();
            }
            object parsedPropValue;
            TryParsePropValue(value, out parsedPropValue);
            if (parsedPropValue is Guid)
            {
                return (ToUpperInvariant((Guid)parsedPropValue));
            }
            return parsedPropValue == null ? "" : parsedPropValue.ToString();
        }

        /// <summary>
        /// This mapper method <see cref="ConvertValueToString"/> will convert any valid guid object to an 
        ///   Invariant Guid string of format .ToString("B").ToUpperInvariant().
        /// </summary>
        /// <param name="valueToParse">value to convert.</param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (valueToParse is Guid)
            {
                Guid guid = ((Guid) valueToParse);
                if (guid == Guid.Empty)
                {
                    returnValue = null;
                    return true;
                }
                returnValue = guid.ToString("B").ToUpperInvariant();
                return true;
            }
            if (valueToParse is DateTime)
            {
                returnValue = ((DateTime) valueToParse).ToString(DateTimeUtilities.StandardDateTimeFormat);
                return true;
            }
            returnValue = valueToParse.ToString();
            return true;
        }

        private static string ToUpperInvariant(Guid guidValue)
        {
            return guidValue.ToString("B").ToUpperInvariant();
        }
    }
}