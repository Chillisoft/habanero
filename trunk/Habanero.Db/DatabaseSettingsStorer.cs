using System;
using System.Collections;
using System.Data;
using Habanero.Base;

namespace Habanero.Db
{
    /// <summary>
    /// Stores database settings as a cache and updates or retrieves settings
    /// either from the cache or from the database if the cache value has
    /// expired
    /// </summary>
    /// TODO ERIC
    /// - more clarity on how the date works (eg. 10 minute expiration)
    /// - is the 10-minute rule standard or should the user be able to amend
    /// - no set decimal?
    public class DatabaseSettingsStorer : ISettingsStorer
    {
        private Hashtable _cachedSettings;

        /// <summary>
        /// Constructor to initialise an empty store of settings
        /// </summary>
        public DatabaseSettingsStorer()
        {
            _cachedSettings = new Hashtable();
        }

        /// <summary>
        /// Returns a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <returns>Returns a string</returns>
        public string GetString(string settingName)
        {
            return Convert.ToString(GetValue(settingName, DateTime.Now.Date));
        }

        /// <summary>
        /// Sets a specified setting as a string
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="val">The value to set it to</param>
        public void SetString(string settingName, string val)
        {
            SetValue(settingName, val);
        }

        /// <summary>
        /// Returns a specified setting as a decimal
        /// </summary>
        /// <param name="settingName">The setting name</param>
        /// <param name="date">The date at which to check the setting</param>
        /// <returns>Returns a decimal</returns>
        /// TODO ERIC - what is the date for? can that be passed?
        public decimal GetDecimal(string settingName, DateTime date)
        {
            return Convert.ToDecimal(GetValue(settingName, date));
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
            if (_cachedSettings[settingName] != null)
            {
                Setting setting = (Setting) _cachedSettings[settingName];
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
                    throw new UserException("Setting " + settingName +
                                            " doesn't exist in the tbsetting table in the database. ");
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
        private static SqlStatement CreateSelectStatement(string settingName, DateTime date)
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("select SettingValue from tbsetting where SettingName = ");
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
        private static SqlStatement CreateUpdateStatementNoDate(string settingName, string settingValue)
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            statement.Statement.Append("update tbsetting set SettingValue = ");
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
            statement.Statement.Append("insert into tbsetting (SettingName, SettingValue, StartDate, EndDate) ");
            statement.Statement.Append("values (");
            statement.AddParameterToStatement(settingName);
            statement.Statement.Append(", ");
            statement.AddParameterToStatement(settingValue);
            statement.Statement.Append(", ");
            statement.AddParameterToStatement(DateTime.Now.Date.AddDays(-1));
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