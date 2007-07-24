using System;
using System.Collections;
using System.Data;
using System.Globalization;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.DB
{
    /// <summary>
    /// Stores database settings as a cache and updates or retrieves settings
    /// either from the cache or from the database if the cache value has
    /// expired
    /// </summary>
    /// TODO ERIC : check caching function (in particular how does it work with dates).
    public class DatabaseSettings : ISettings
    {
        private Hashtable _cachedSettings;
        private string _tableName = "settings";

        /// <summary>
        /// Constructor to initialise an empty store of settings, using the table
        /// name of "settings"
        /// </summary>
        public DatabaseSettings()
        {
            _cachedSettings = new Hashtable();
        }

        /// <summary>
        /// Constructor to initialise an empty store of settings, specifying
        /// the table name to use to store the settings
        /// </summary>
        /// <param name="tableName">The table name in which to store settings</param>
        public DatabaseSettings(string tableName) : this()
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Gets and sets the database table name to use to store the settings.
        /// The default is "settings", but the name can be altered if needed.
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName)
        {
            return Convert.ToString(GetValue(settingName, DateTime.Now));
        }

        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date of the setting to retrieve 
        /// (ie this method will retrieve the setting value as at this date</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName, DateTime date)
        {
            return Convert.ToString(GetValue(settingName, date));
        }

        /// <summary>
        /// Sets a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set it to</param>
        public void SetString(string settingName, string settingValue)
        {
            SetValue(settingName, settingValue);
        }

        /// <summary>
        /// Returns a specified setting as a decimal
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date at which to check the setting</param>
        /// <returns>Returns a decimal</returns>
        public decimal GetDecimal(string settingName, DateTime date)
        {
            return Convert.ToDecimal(GetValue(settingName, date), CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Returns the configuration for the setting name provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public decimal GetDecimal(string settingName)
        {
            return GetDecimal(settingName, DateTime.Now);
        }

        /// <summary>
        /// Sets a setting with a decimal value (using the InvariantCulture).  Uses DateTime.Now for the 
        /// date of the setting to indicate when the setting was changed.
        /// </summary>
        /// <param name="settingName">The name of the setting to set</param>
        /// <param name="settingValue">The value to set the setting to.</param>
        public void SetDecimal(string settingName, decimal settingValue)
        {
            SetValue(settingName, Convert.ToString(settingValue, CultureInfo.InvariantCulture.NumberFormat));
        }

        /// <summary>
        /// Returns a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a boolean</returns>
        public bool GetBoolean(string settingName)
        {
            return Convert.ToBoolean(GetValue(settingName, DateTime.Now));
        }

        /// <summary>
        /// Sets a specified setting as a boolean
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="boolValue">The value to set it to</param>
        public void SetBoolean(string settingName, bool boolValue)
        {
            this.SetValue(settingName, Convert.ToString(boolValue));
        }

        /// <summary>
        /// Updates the named setting in the database with the value
        /// specified
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set to</param>
        private void SetValue(string settingName, string settingValue)
        {
            settingName = settingName.ToUpper();
            bool hasCurrentValue;
            string currentValue;
            try
            {
                currentValue = Convert.ToString(GetValue(settingName, DateTime.Now));
                hasCurrentValue = true;
            }
            catch (UserException)
            {
                hasCurrentValue = false;
            }
            SqlStatement statement;
            if (hasCurrentValue)
            {
                statement = CreateUpdateStatementNoDate(settingName, settingValue);
            }
            else
            {
                statement = CreateInsertStatement(settingName, settingValue);
            }
            DatabaseConnection.CurrentConnection.ExecuteSql(new SqlStatementCollection(statement));
            UpdateCache(settingName, settingValue);
        }

        /// <summary>
        /// Refreshes the cached setting stored in this instance, by
        /// retrieving the updated setting from the database
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The setting value</param>
        /// TODO ERIC - extra parameter not used
        private void UpdateCache(string settingName, string settingValue)
        {
            if (_cachedSettings[settingName] != null)
            {
                _cachedSettings.Remove(settingName);
            }
            GetValue(settingName, DateTime.Now);
        }

        /// <summary>
        /// Retrieves the named setting with the date specified, either
        /// from the cache or from the database if the cached value has
        /// expired
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date</param>
        /// <returns>Returns the value stored in the setting</returns>
        /// <exception cref="UserException">Thrown if the named setting
        /// does not exist</exception>
        private object GetValue(string settingName, DateTime date)
        {
            settingName = settingName.ToUpper();
            Setting setting = (Setting) _cachedSettings[settingName];
            if (setting != null)
            {
                if (!setting.IsExpired())
                {
                    return setting.Value;
                }
                else
                {
                    _cachedSettings.Remove(settingName);
                }
            }
            SqlStatement statement = CreateSelectStatement(settingName, date);
            IDataReader reader = null;
            try
            {
                reader = DatabaseConnection.CurrentConnection.LoadDataReader(statement);

                if (reader.Read())
                {
                    object val = reader.GetValue(0);
                    _cachedSettings.Add(settingName, new Setting(DateTime.Now, val));
                    return val;
                }
                else
                {
                    throw new UserException(String.Format("The setting '{0}' " +
                        "does not exist in the '{1}' table in the database.",
                        settingName, _tableName));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Creates a sql select statement to retrieve the named setting
        /// at the date specified
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date</param>
        /// <returns>Returns a sql statement object</returns>
        private SqlStatement CreateSelectStatement(string settingName, DateTime date)
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("select SettingValue from " + _tableName + " where SettingName = ");
            statement.AddParameterToStatement(settingName);
            statement.Statement.Append(" and (StartDate < ");
            statement.AddParameterToStatement(date);
            statement.Statement.Append(" or StartDate is null) ");
            statement.Statement.Append(" and (EndDate > ");
            statement.AddParameterToStatement(date);
            statement.Statement.Append(" or EndDate is null)");
            return statement;
        }

        /// <summary>
        /// Creates a sql update statement from the name and value provided,
        /// with no date specified
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set to</param>
        /// <returns>Returns a sql statement object</returns>
        private SqlStatement CreateUpdateStatementNoDate(string settingName, string settingValue)
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("update " + _tableName + " set SettingValue = ");
            statement.AddParameterToStatement(settingValue);
            statement.Statement.Append(" where SettingName = ");
            statement.AddParameterToStatement(settingName);
            return statement;
        }

        /// <summary>
        /// Creates a sql insert statement from the name and value provided
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="settingValue">The value to set to</param>
        /// <returns>Returns a sql statement object</returns>
        private SqlStatement CreateInsertStatement(string settingName, string settingValue)
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("insert into " + _tableName + " (SettingName, SettingValue, StartDate, EndDate) ");
            statement.Statement.Append("values (");
            statement.AddParameterToStatement(settingName);
            statement.Statement.Append(", ");
            statement.AddParameterToStatement(settingValue);
            statement.Statement.Append(", ");
            statement.AddParameterToStatement(DateTime.Now);
            statement.Statement.Append(", ");
            statement.AddParameterToStatement(null);
            statement.Statement.Append(") ");
            return statement;
        }

        /// <summary>
        /// Stores some simple settings
        /// </summary>
        private class Setting
        {
            private readonly DateTime _time;
            private readonly object _value;

            /// <summary>
            /// Constructor to initialise the setting
            /// </summary>
            /// <param name="time">The time</param>
            /// <param name="value">The value</param>
            public Setting(DateTime time, object value)
            {
                _time = time;
                _value = value;
            }

            /// <summary>
            /// Returns the time value held
            /// </summary>
            public DateTime Time
            {
                get { return _time; }
            }

            /// <summary>
            /// Returns the setting value held
            /// </summary>
            public object Value
            {
                get { return _value; }
            }

            /// <summary>
            /// Indicates if the stored time is more than 10 minutes old
            /// </summary>
            /// <returns>Returns true if expired, false if not</returns>
            public bool IsExpired()
            {
                return DateTime.Now.Subtract(_time).CompareTo(new TimeSpan(0, 10, 0)) > 0;
            }
        }
    }
}