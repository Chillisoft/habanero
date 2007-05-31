using System;
using System.Configuration;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Stores settings from the application's configuration file
    /// </summary>
    public class ConfigFileSettingsStorer : ISettingsStorer
    {
        private AppSettingsReader itsReader;

        /// <summary>
        /// Constructor to initialise a new storer
        /// </summary>
        public ConfigFileSettingsStorer()
        {
            itsReader = new AppSettingsReader();
        }

        /// <summary>
        /// Returns the configuration for the setting name provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName)
        {
            return (string) itsReader.GetValue(settingName, typeof (string));
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        /// TODO ERIC - implement
        public void SetString(string settingName, string settingValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a exception indicating that date settings are not
        /// supported
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown any time this
        /// method is called</exception>
        /// TODO ERIC - is this wrongly named? the error message doesn't
        /// indicate anything to do with decimals.
        /// - need a set as well?
        public decimal GetDecimal(string settingName, DateTime date)
        {
            throw new NotSupportedException("ConfigFileSettingsStorer does not support date ranging settings.");
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