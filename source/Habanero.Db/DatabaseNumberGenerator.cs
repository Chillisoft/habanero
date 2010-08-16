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
    public class DatabaseNumberGenerator : IDBNumberGenerator
    {
        private readonly string _settingName;
        private readonly string _tableName;
        private readonly int _seedValue;
        private int _number;

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
        {
            _settingName = settingName;
            _tableName = tableName;
            _seedValue = seedValue;

            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection,
                                 "select SettingValue from " + _tableName + " where SettingName = ");
            statement.AddParameterToStatement(_settingName);
            IDataReader reader = null;
            bool hasNumber = false;
            try
            {
                using (reader = DatabaseConnection.CurrentConnection.LoadDataReader(statement))
                {
                    if (reader.Read())
                    {
                        hasNumber = true;
                        _number = Convert.ToInt32(reader.GetValue(0));
                    }
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            if (!hasNumber)
            {
                _number = _seedValue;
                DatabaseConnection.CurrentConnection.ExecuteRawSql(
                    string.Format("insert into {0} (SettingName, SettingValue) values ('{1}', {2})",
                                  _tableName, _settingName, _seedValue));
            }
        }

        /// <summary>
        /// Returns the next unique number, which is incremented from the
        /// last generated number
        /// </summary>
        /// <returns>Returns an integer</returns>
        public int GetNextNumberInt()
        {
            return ++_number;
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
        public ITransactional GetUpdateTransaction()
        {
            return new NumberUpdate(_number, _settingName, _tableName);
        }

        /// <summary>
        /// Creates an ITransactional object to update the number in the database when the number is updated.
        /// </summary>
        private class NumberUpdate : ITransactionalDB
        {
            private readonly int _newNumber;
            private readonly string _settingName;
            private readonly string _tableName;

            /// <summary>
            /// Constructor to initialise a new update
            /// </summary>
            /// <param name="newNumber">The new number to store</param>
            /// <param name="settingName">The database setting name</param>
            /// <param name="tableName">The database table name</param>
            public NumberUpdate(int newNumber, string settingName, string tableName)
            {
                _newNumber = newNumber;
                _settingName = settingName;
                _tableName = tableName;
            }


            #region ITransaction Members

            /// <summary>
            /// Returns the sql statement to update the number in the
            /// database
            /// </summary>
            /// <returns>Returns an ISqlStatementCollection containing
            /// the statement</returns>
            public ISqlStatementCollection GetPersistSql()
            {
                SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection,
                                                          " update " + _tableName + " set SettingValue = ");
                statement.AddParameterToStatement(_newNumber.ToString());
                statement.Statement.Append(" where SettingName = ");
                statement.AddParameterToStatement(_settingName);

                return new SqlStatementCollection(statement);
            }

            #endregion

            ///<summary>
            ///</summary>
            ///<returns>The ID that uniquelty identifies this item of the transaction. In the case of business objects the object Id.
            /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
            /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
            /// is not added twice in error.</returns>
            public string TransactionID()
            {
                return _settingName;
            }

            ///<summary>
            /// Updates the business object as committed
            ///</summary>
            public void UpdateStateAsCommitted()
            {
            }

            ///<summary>
            /// updates the object as rolled back
            ///</summary>
            public void UpdateAsRolledBack()
            {
            }
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        public void UpdateStateAsCommitted()
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        public void UpdateAsRolledBack()
        {
            throw new NotImplementedException();
        }
    }
}
