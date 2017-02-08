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
using System.Globalization;
using Habanero.Base;
using PCLAppConfig;
using PCLAppConfig.Infrastructure;

namespace Habanero.Util
{
    /// <summary>
    /// Stores settings from the application's configuration file
    /// </summary>
    public class ConfigFileSettings : ISettings
    {
        ///<summary>
        /// Returns whether the setting exists or not.
        ///</summary>
        ///<param name="settingName">The name of the setting to look for.</param>
        ///<returns>Returns whether the setting exists or not</returns>
        public bool HasSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName] != null;
        }

        /// <summary>
        /// Returns the configuration for the setting name provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName)
        {
            return GetSettingValue(settingName);
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
        /// Sets a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set it to</param>
        public void SetString(string settingName, string settingValue)
        {
            SetSettingValue(settingName, settingValue);
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
            string settingValue = GetSettingValue(settingName);
            return Convert.ToDecimal(settingValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetDecimal(string settingName, decimal settingValue)
        {
            SetSettingValue(settingName, Convert.ToString(settingValue, CultureInfo.InvariantCulture.NumberFormat));
        }

        /// <summary>
        /// Returns a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a boolean</returns>
        public bool GetBoolean(string settingName)
        {
            string settingValue = GetSettingValue(settingName);
            return Convert.ToBoolean(settingValue);
        }

        /// <summary>
        /// Sets a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="boolValue">The value to set it to</param>
        public void SetBoolean(string settingName, bool boolValue)
        {
            SetSettingValue(settingName, Convert.ToString(boolValue));
        }

        private string GetSettingValue(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];

            //KeyValueConfigurationElement configurationElement = GetSettingConfigurationElement(settingName);
            //if (configurationElement == null)
            //    throw new InvalidOperationException(
            //        string.Format("The key '{0}' does not exist in the appSettings configuration section.", settingName));
            //return configurationElement.Value;
        }

        private void SetSettingValue(string settingName, string settingValue)
        {
            //KeyValueConfigurationElement configurationElement = GetSettingConfigurationElement(settingName);
            //if (configurationElement == null)
            //{
            //    _configuration.AppSettings.Settings.Add(settingName, settingValue);
            //}
            //else
            //{
            //    configurationElement.Value = settingValue;
            //}
            //_configuration.Save(ConfigurationSaveMode.Modified);
            throw new NotImplementedException("Disabled in Xamarin Port!");
        }

    }
}