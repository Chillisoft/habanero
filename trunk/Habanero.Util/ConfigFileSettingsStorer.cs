using System;
using System.Configuration;
using Habanero.Base;

namespace Habanero.Util
{
    /// <summary>
    /// Stores settings from the application's configuration file
    /// </summary>
    public class ConfigFileSettingsStorer : ISettingsStorer
    {
        private AppSettingsReader _reader;

        /// <summary>
        /// Constructor to initialise a new storer
        /// </summary>
        public ConfigFileSettingsStorer()
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
            throw new NotSupportedException("ConfigFileSettingsStorer does not support date ranging settings.");
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        public void SetString(string settingName, string settingValue)
        {
            throw new NotImplementedException("ConfigFileSettingsStorer does not support setting settings");
        }

        /// <summary>
        /// Not supported by ConfigFileSettings
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown any time this
        /// method is called</exception>
        public decimal GetDecimal(string settingName, DateTime date)
        {
            throw new NotSupportedException("ConfigFileSettingsStorer does not support date ranging settings.");
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
            throw new NotImplementedException("ConfigFileSettingsStorer does not support setting settings");
        }


        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="settingName"></param>
        /// TODO ERIC - implement
        public bool GetBoolean(string settingName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="boolValue"></param>
        /// TODO ERIC - implement
        public void SetBoolean(string settingName, bool boolValue)
        {
            throw new NotImplementedException();
        }
    }
}