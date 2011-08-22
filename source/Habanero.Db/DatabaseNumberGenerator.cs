// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Generates unique numbers by storing the last number generated in
    /// the database and then supplying a newly incremented number in the
    /// next request.  A database update transaction can also be generated
    /// to increment the number stored in the database.
    /// </summary>
    /// <remarks>
    /// This number generator does not implement concurrency control of any type.
    /// It is susceptible to both lost number and to multiple users generating the 
    /// same number. If either of these scenarious exist in your application then
    /// please use one of the sub types of <see cref="INumberGenerator"/>. 
    /// </remarks>
    public class DatabaseNumberGenerator : IDBNumberGenerator
    {
/*        private readonly string _settingName;
        private readonly string _tableName;
        private readonly int _seedValue;
        private int _number;*/
        private readonly NumberUpdate _numberUpdater;

        /// <summary>
        /// Constructor to initialise a new generator. The table name is
        /// initialised to "numbers".
        /// </summary>
        /// <param name="settingName">The database setting name that
        /// stores the number</param>
        public DatabaseNumberGenerator(string settingName)
            : this(settingName, "numbers")
        {
        }

        /// <summary>
        /// Constructor to initialise a new generator, supplying a specific
        /// table name
        /// </summary>
        /// <param name="settingName">The database setting name that
        /// stores the number</param>
        /// <param name="tableName">The database table name that
        /// stores the number</param>
        public DatabaseNumberGenerator(string settingName, string tableName)
            : this(settingName, tableName, 0)
        {
        }

        /// <summary>
        /// Constructor to initialise a new generator, supplying a specific
        /// table name and starting seed value
        /// </summary>
        /// <param name="settingName">The database setting name that
        /// stores the number</param>
        /// <param name="tableName">The database table name that
        /// stores the number</param>
        /// <param name="seedValue">The seed value to begin incrementing
        /// from</param>
        public DatabaseNumberGenerator(string settingName, string tableName, int seedValue)
            : this(settingName, tableName, seedValue, "SettingName", "SettingValue")
        {
        }

        /// <summary>
        /// Constructor to initialise a new generator, supplying a specific
        /// table name and starting seed value
        /// </summary>
        /// <param name="settingName">The database setting name that
        /// stores the number</param>
        /// <param name="tableName">The database table name that
        /// stores the number</param>
        /// <param name="seedValue">The seed value to begin incrementing
        /// from</param>
        /// <param name="settingNameFieldName"></param>
        /// <param name="settingValueFieldName"></param>
        public DatabaseNumberGenerator(string settingName, string tableName, int seedValue, string settingNameFieldName, string settingValueFieldName)
        {
            var statement =
                new SqlStatement(DatabaseConnection.CurrentConnection,
                                 string.Format("select {0} from ", settingValueFieldName) + tableName + string.Format(" where {0} = ", settingNameFieldName));
            statement.AddParameterToStatement(settingName);
            IDataReader reader = null;
            var hasNumber = false;
            var number = 0;
            try
            {
                using (reader = DatabaseConnection.CurrentConnection.LoadDataReader(statement))
                {
                    if (reader.Read())
                    {
                        hasNumber = true;

                        number = Convert.ToInt32(reader.GetValue(0));
                    }
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            if (!hasNumber)
            {
                number = seedValue;
                DatabaseConnection.CurrentConnection.ExecuteRawSql(
                    string.Format("insert into {0} ({1}, {2}) values ('{3}', {4})",
                                  tableName, settingNameFieldName, settingValueFieldName, settingName, seedValue));
            }
            _numberUpdater = new NumberUpdate(number, settingName, tableName, settingNameFieldName, settingValueFieldName);
        }

        /// <summary>
        /// Returns the next unique number, which is incremented from the
        /// last generated number
        /// </summary>
        /// <returns>Returns an integer</returns>
        public int GetNextNumberInt()
        {
            return _numberUpdater.GetNextNumberInt();
        }

        /// <summary>
        /// Creates a database transaction that updates the database to the
        /// last number dispensed, so the next number dispensed will be a
        /// fresh increment
        /// </summary>
        /// <returns>Returns an ITransactional object</returns>
        ITransactional IDBNumberGenerator.GetUpdateTransaction()
        {
            return GetUpdateTransaction();
        }

        /// <summary>
        /// Creates a database transaction that updates the database to the
        /// last number dispensed, so the next number dispensed will be a
        /// fresh increment
        /// </summary>
        /// <returns>Returns an ITransaction object</returns>
        private ITransactional GetUpdateTransaction()
        {
            
            return _numberUpdater;
        }

        /// <summary>
        /// Creates an ITransactional object to update the number in the database when the number is updated.
        /// </summary>
        private class NumberUpdate : ITransactionalDB
        {
            private int _currentNumber;
            private readonly string _setting;
            private readonly string _tableName;
            private readonly string _settingFieldName;
            private readonly string _settingValueFieldName;
            private int _originalNumber;

            /// <summary>
            /// Constructor to initialise a new update
            /// </summary>
            /// <param name="currentNumber">The new number to store</param>
            /// <param name="setting">The database setting name</param>
            /// <param name="tableName">The database table name</param>
            /// <param name="settingFieldName"></param>
            /// <param name="settingValueFieldName"></param>
            public NumberUpdate(int currentNumber, string setting, string tableName, string settingFieldName, string settingValueFieldName)
            {
                _currentNumber = currentNumber;
                _originalNumber = currentNumber;
                _setting = setting;
                _tableName = tableName;
                _settingFieldName = settingFieldName;
                _settingValueFieldName = settingValueFieldName;
            }

            #region ITransaction Members

            /// <summary>
            /// Returns the sql statement to update the number in the
            /// database
            /// </summary>
            /// <returns>Returns an ISqlStatementCollection containing
            /// the statement</returns>
            public IEnumerable<ISqlStatement> GetPersistSql()
            {
                var sqlStatement = string.Format(" update {0} set {1} = ", _tableName, _settingValueFieldName);
                var statement = new SqlStatement(DatabaseConnection.CurrentConnection,
                                                          sqlStatement);
                statement.AddParameterToStatement(_currentNumber.ToString());
                statement.Statement.Append(string.Format(" where {0} = ", _settingFieldName));
                statement.AddParameterToStatement(_setting);

                return new [] {statement};
            }

            #endregion

            ///<summary>
            /// This is the ID that uniquely identifies this item of the transaction.
            /// This must be the same for the lifetime of the transaction object. 
            /// This assumption is relied upon for certain optimisations in the Transaction Comitter.
            ///</summary>
            ///<returns>The ID that uniquely identifies this item of the transaction. In the case of business objects the object Id.
            /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
            /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
            /// is not added twice in error.</returns>
            public string TransactionID()
            {
                return _setting;
            }

            ///<summary>
            /// Updates the business object as committed
            ///</summary>
            public void UpdateStateAsCommitted()
            {
                _originalNumber = _currentNumber;
            }

            ///<summary>
            /// updates the object as rolled back
            ///</summary>
            public void UpdateAsRolledBack()
            {
                _currentNumber = _originalNumber;
            }

            public int GetNextNumberInt()
            {
                return ++_currentNumber;
            }
        }

    }
}
