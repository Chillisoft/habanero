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
    public class DatabaseNumberGenerator : INumberGenerator
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
        public DatabaseNumberGenerator(string settingName) : this(settingName, "numbers")
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
        public DatabaseNumberGenerator(string settingName, string tableName) : this(settingName, tableName, 0)
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
                new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection(),
                                 "select SettingValue from " + _tableName + " where SettingName = ");
            statement.AddParameterToStatement(_settingName);
            IDataReader reader = null;
            try
            {
                reader = DatabaseConnection.CurrentConnection.LoadDataReader(statement);
                if (reader.Read())
                {
                    _number = Convert.ToInt32(reader.GetValue(0));
                }
                else
                {
                    _number = _seedValue;
                    DatabaseConnection.CurrentConnection.ExecuteRawSql("insert into " + _tableName +
                                                                         " (SettingName, SettingValue) values ('" +
                                                                         _settingName + "', " + _seedValue + ")");
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
        /// <returns>Returns an ITransaction object</returns>
        public ITransaction GetUpdateTransaction()
        {
            return new NumberUpdate(_number, _settingName, _tableName);
        }

        /// <summary>
        /// Creates a transaction to update the number in the database
        /// </summary>
        private class NumberUpdate : ITransaction
        {
            private int _newNumber;
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
            /// Returns the number as a string
            /// </summary>
            /// <returns>Returns a string</returns>
            public string StrID()
            {
                return _newNumber.ToString();
            }

            /// <summary>
            /// Returns int.MinValue
            /// </summary>
            /// <returns>Returns int.MinValue</returns>
            public int TransactionRanking()
            {
                return int.MinValue;
            }

			/// <summary>
			/// Does nothing
			/// </summary>
        	public bool AddingToTransaction(ITransactionCommitter transaction)
        	{
        		return true;
        	}


            ///// <summary>
			///// Does nothing
			///// </summary>
			//public void CheckPersistRules()
			//{
			//}

            /// <summary>
            /// Does nothing
            /// </summary>
            public void BeforeCommit(ITransactionCommitter transactionCommitter)
            {
            }

			/// <summary>
			/// Returns the sql statement to update the number in the
			/// database
			/// </summary>
			/// <returns>Returns an ISqlStatementCollection containing
			/// the statement</returns>
			public ISqlStatementCollection GetPersistSql()
			{
				SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection(),
														  " update " + _tableName + " set SettingValue = ");
				statement.AddParameterToStatement(_newNumber.ToString());
				statement.Statement.Append(" where SettingName = ");
				statement.AddParameterToStatement(_settingName);

				return new SqlStatementCollection(statement);
			}

            /// <summary>
            /// Does nothing
            /// </summary>
            public void AfterCommit()
            {
            }

             /// <summary>
            /// Does nothing
            /// </summary>
            public void TransactionCommitted()
            {
            }

			/// <summary>
			/// Does nothing
			/// </summary>
			public void TransactionRolledBack()
			{
			}

			/// <summary>
			/// Does nothing
			/// </summary>
			public void TransactionCancelEdits()
			{
			}

       	#endregion

        }
    }
}