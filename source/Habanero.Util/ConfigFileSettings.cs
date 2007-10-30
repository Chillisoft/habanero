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
using System.Configuration;
using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Stores settings from the application's configuration file
    /// </summary>
    public class ConfigFileSettings : ISettings
    {
        private AppSettingsReader _reader;

        /// <summary>
        /// Constructor to initialise a new storer
        /// </summary>
        public ConfigFileSettings()
        {
            _reader = new AppSettingsReader();
        }

        /// <summary>
        /// Returns the configuration for the setting name provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName)
        {
            return (string) _reader.GetValue(settingName, typeof (string));
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetString(string settingName, DateTime date)
        {
            throw new NotSupportedException("ConfigFileSettings does not support date ranging settings.");
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetString(string settingName, string settingValue)
        {
            throw new NotImplementedException("ConfigFileSettings does not support setting settings");
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown any time this
        /// method is called</exception>
        public decimal GetDecimal(string settingName, DateTime date)
        {
            throw new NotSupportedException("ConfigFileSettings does not support date ranging settings.");
        }

        /// <summary>
        /// Returns the configuration for the setting name provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public decimal  GetDecimal(string settingName)
        {
            return (decimal)_reader.GetValue(settingName, typeof(decimal));
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetDecimal(string settingName, decimal settingValue)
        {
            throw new NotImplementedException("ConfigFileSettings does not support setting settings");
        }


        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        public bool GetBoolean(string settingName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="boolValue"></param>
        public void SetBoolean(string settingName, bool boolValue)
        {
            throw new NotImplementedException();
        }
    }
}