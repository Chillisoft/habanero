using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that stores settings
    /// </summary>
    /// TODO ERIC - no set decimal?
    public interface ISettingsStorer
    {
        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        string GetString(string settingName);

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
        /// <param name="date">The date</param>
        /// <returns>Returns a decimal</returns>
        /// TODO ERIC - what is the date for? bypass?
        decimal GetDecimal(string settingName, DateTime date);

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