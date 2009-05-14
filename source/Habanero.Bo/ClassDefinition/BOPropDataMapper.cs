//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    ///<summary>
    /// provides an interface for property data mappers.
    /// The property data mapper conforms to the GOF strategy pattern.
    /// This allows the developer to apply the data mapper that is relevant to the particular situation.
    /// E.g. Code to read data from a database (or other datasource) and convert it to the appropriate type.
    /// This can be customised for a particular situation and can be used to implement custom type conversions
    ///   this is particularly usefull for legacy databases or non standard databases and can be used for 
    ///   parsing text files XML files or any other datasource.
    /// 
    /// The mapper has two methods 
    /// <li>the <see cref="ConvertValueToString"/> method will convert any object of the correct type/format to 
    /// a string value.</li>
    /// <li> the <see cref="TryParsePropValue"/> method will convert any object of the appropriate type or a string 
    ///   with the correct format to a valid object </li>
    ///</summary>
    public abstract class BOPropDataMapper
    {
        /// <summary>
        /// The standard date Time Format to use.
        /// </summary>
        protected string _standardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";

        /// <summary>
        ///  Converts the value of a valid type for this property definition to a string relevant.
        ///  A null value will be oonverted to a zero length string.
        ///  </summary><param name="value">The value to be converted</param><returns>The converted string.</returns>
        public virtual string ConvertValueToString(object value)
        {
            return value == null ? "" : value.ToString();
        }

        /// <summary>
        ///  This method provides a the functionality to convert any object to the appropriate
        ///    type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///    a valid Guid Object.
        ///  </summary><param name="valueToParse">The value to be converted</param><returns>An object of the correct type.</returns>
        ///<param name="returnValue">the parsed value</param>
        public virtual bool TryParsePropValue(object valueToParse, out object returnValue)
        {
            returnValue = null;
            if (valueToParse != null && !(valueToParse is IBusinessObject) && valueToParse.ToString().Length == 0) return true;

            return valueToParse == null || valueToParse == DBNull.Value;
        }
    }
}