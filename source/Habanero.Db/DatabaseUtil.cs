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
using System.Drawing;
using System.Globalization;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.DB
{
    /// <summary>
    /// Provides database utilities such as methods to reformat objects
    /// into a format appropriate for sql
    /// </summary>
    public class DatabaseUtil
    {
        private DatabaseUtil() {}

        /// <summary>
        /// Returns a DateTime object as a string in the format of:<br/>
        /// "yyyy-MM-dd hh:mm:ss"
        /// </summary>
        /// <param name="date">The DateTime object</param>
        /// <returns>Returns a string</returns>
        public static string FormatDatabaseDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// Prepares the value to be converted to a format appropriate for sql
        /// </summary>
        /// <param name="objValue">The value to prepare</param>
        /// <returns>Returns the reformatted object</returns>
        internal static object PrepareValue(object objValue)
        {
            if (objValue is Guid)
            {
                if (Guid.Empty.Equals(objValue))
                {
                    return DBNull.Value;
                }
                return ((Guid) objValue).ToString("B").ToUpper(CultureInfo.InvariantCulture);
            }
            if (objValue is bool)
            {
                return (bool) objValue ? 1 : 0;
            }
            if (objValue is Image)
            {
                return SerialisationUtilities.ObjectToByteArray(objValue);
            }
            if (objValue is CustomProperty)
            {
                return ((CustomProperty) objValue).GetPersistValue();
            }
            if (objValue is TimeSpan)
            {
                TimeSpan time = (TimeSpan) objValue;
                return new DateTime(1900, 1, 1, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
            }
            return objValue;
        }

        /// <summary>
        /// Converts the value to a string format appropriate for sql
        /// </summary>
        /// <param name="objValue">The value to convert</param>
        /// <returns>Returns a string</returns>
        public static string ConvertValueToStandardSql(object objValue)
        {
            object preparedValue = PrepareValue(objValue);
            string stringValue;
            if (preparedValue == null || DBNull.Value.Equals(preparedValue))
            {
                return "null";
            }
            if (preparedValue is DateTime)
            {
                stringValue = FormatDatabaseDateTime((DateTime) preparedValue);
            }
            else if (preparedValue is String)
            {
                Decimal decimalValue;
                if (Decimal.TryParse((string)preparedValue, out decimalValue))
                {
                    stringValue = (string) preparedValue;
                } else 
                {
                    stringValue = string.Format("'{0}'", preparedValue);
                }
            }
            else
            {
                stringValue = preparedValue.ToString();
            }
            return stringValue;
        }
    }
}