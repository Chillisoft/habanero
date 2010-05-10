// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// Implements a data mapper for a int Property
    /// The property data mapper conforms to the GOF strategy pattern <seealso cref="BOPropDataMapper"/>.
    ///</summary>
    public class BOPropIntDataMapper : BOPropDataMapper
    {
        /// <summary>
        /// This mapper method will convert any valid guid object to an 
        ///   Invariant int string of format .ToString("B").ToUpperInvariant().
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ConvertValueToString(object value)
        {
            if (value == null) return "";
            if (value is int)
            {
                int intValue     = ((int) value);
                return intValue.ToString();
            }
            object parsedPropValue;
            TryParsePropValue(value, out parsedPropValue);
            if (parsedPropValue is int)
            {
                return (parsedPropValue.ToString());
            }
            return parsedPropValue == null ? "" : parsedPropValue.ToString();
        }

        /// <summary>
        /// This mapper method will convert any valid int string to a valid int object 
        ///   (ParsePropValue) and will convert a DBNull to a null value to null.
        /// </summary>
        /// <param name="valueToParse">value to convert.</param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public override bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            if (base.TryParsePropValue(valueToParse, out returnValue)) return true;

            if (!(valueToParse is int))
            {
                if (valueToParse is IBusinessObject)
                {
                    IBusinessObject bo = (IBusinessObject) valueToParse;
                    if (bo.ID.GetAsValue() is int)
                    {
                        returnValue = bo.ID.GetAsValue();
                        return true;
                    }
                    returnValue = null;
                    return false;
                }
                int intValue;
                if (int.TryParse(Convert.ToString(valueToParse), out intValue))
                {
                    returnValue = intValue;
                    return true;
                }
                if (valueToParse is decimal)
                {
                    decimal decimalValue = (decimal)valueToParse;
                    if (decimalValue >= int.MinValue && decimalValue <= int.MaxValue)
                    {
                        returnValue = Convert.ToInt32(valueToParse);
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