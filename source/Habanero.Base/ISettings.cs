//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that stores settings.
    /// Very often settings change over time (such as perhaps the VAT rate or the interest rate), or you
    /// would like a setting to change over a weekend (such as New Years).  The Get methods with a date
    /// parameter allow this.  The implementation provided, DatabaseSettings will use the date if its provided to
    /// retrieve the setting as it was on a particular date.  Calling a Set method with a date will create a new
    /// entry with that date as the setting.  Should time not affect your setting at all, simply use the methods
    /// without the date parameter.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        string GetString(string settingName);

        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date of the setting</param>
        /// <returns>Returns a string</returns>
        string GetString(string settingName, DateTime date);

        /// <summary>
        /// Sets a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set it to</param>
        void SetString(string settingName, string settingValue);

        /// <summary>
        /// Returns a specified setting as a decimal
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date of the setting 
        /// (ie this will return the setting value as at the specified date)</param>
        /// <returns>Returns a decimal</returns>
        decimal GetDecimal(string settingName, DateTime date);

        /// <summary>
        /// Returns a specified setting as a decimal, with Now as the specified date
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a decimal</returns>
        decimal GetDecimal(string settingName);

        /// <summary>
        /// Sets the setting with the specified name to the specified value (with a date of Now)
        /// Note: If no setting exists it should be created.
        /// </summary>
        /// <param name="settingName">The name of the setting</param>
        /// <param name="settingValue">The value to set the setting to</param>
        void SetDecimal(string settingName, decimal settingValue);

        /// <summary>
        /// Returns a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a boolean</returns>
        bool GetBoolean(string settingName);

        /// <summary>
        /// Sets a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="boolValue">The value to set it to</param>
        void SetBoolean(string settingName, bool boolValue);
    }
}