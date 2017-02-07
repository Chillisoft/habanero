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

namespace Habanero.Base.DataMappers
{
    ///<summary>
    /// Provides a base class for property data mappers.
    /// The property data mapper conforms to the GOF strategy pattern.
    /// This allows the developer to apply the data mapper that is relevant to the particular situation.
    /// E.g. Code to read data from a database (or other datasource) and convert it to the appropriate type.
    /// This can be customised for a particular situation and can be used to implement custom type conversions
    ///   this is particularly usefull for legacy databases or non standard databases and can be used for 
    ///   parsing text files XML files or any other datasource.
    /// 
    /// The mapper has two methods 
    /// <li>the <see cref="ConvertValueToString"/> method will convert any object of the correct type/format to 
    /// a string value. The default implementation just uses the ToString() method on the value (or returns the 
    /// empty string if the value is null)</li>
    /// <li> the <see cref="TryParsePropValue"/> method will try to convert any object to an object of the output type.
    /// The output type is determined by the mapper - the default in this base class if object.</li>
    ///</summary>
    public abstract class DataMapper : IDataMapper
    {
        /// <summary>
        /// This value represents how the data mapper will treat an empty string.
        /// If this is set to true (default) then empty string values will be set to null, and will be found to be equivalent to null.
        /// </summary>
        protected bool _convertEmptyStringToNull = true;

        /// <summary>
        /// Compares two values as the appropriate type, applying the conversion conventions that this <see cref="IDataMapper"/> includes.
        /// These values are not parsed as part of this comparison operation.
        /// This is meant to be a quick check of two possibly recognisable types.
        /// </summary>
        /// <param name="compareToValue">The value to compare to. This usually an already parsed value.</param>
        /// <param name="value">The value that is being compared. This may be an unparsed value.</param>
        /// <returns>The result of the comparison. True if the values are equal, False if not.</returns>
        public bool CompareValues(object compareToValue, object value)
        { 
            if (compareToValue == value) return true;
            if (compareToValue != null) return compareToValue.Equals(value);
            if (value == null) return true;
            return _convertEmptyStringToNull && (string.IsNullOrEmpty(Convert.ToString(value)));
        }

        /// <summary>
        ///  Converts the value of a valid type for this property definition to a string relevant.
        ///  A null value will be oonverted to a zero length string.
        ///  </summary><param name="value">The value to be converted</param><returns>The converted string.</returns>
        public virtual string ConvertValueToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

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
        public virtual bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            returnValue = null;
            if (_convertEmptyStringToNull && valueToParse != null && valueToParse is string && ((string)valueToParse).Length == 0) return true;

            return valueToParse == null || valueToParse == DBNull.Value;
        }

    }
}