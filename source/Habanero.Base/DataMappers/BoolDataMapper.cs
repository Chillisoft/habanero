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
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.Base.DataMappers
{
    /// <summary>
    /// Data mapper for boolean types
    /// </summary>
    public class BoolDataMapper : DataMapper
    {
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

            bool result;
            bool success;
            try
            {
                success = StringUtilities.BoolTryParse(valueToParse, out result);
            }
            catch (HabaneroDeveloperException)
            {
                returnValue = null;
                return false;
            }
            returnValue = result;
            return success;
        }
    }
}