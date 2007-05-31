using System;
using System.Data;
using Chillisoft.Generic.v2;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Generates unique numbers by storing the last number generated in
    /// the database and then supplying a newly incremented number in the
    /// next request.  A database update transaction can also be generated
    /// to increment the number stored in the database.
    /// </summary>
    public class DatabaseNumberGenerator : INumberGenerator
    {
        private readonly string itsSettingName;
        private readonly string itsTableName;
        private readonly int itsSeedValue;
        private int itsNumber;

        /// <summary>
        /// Constructor to initialise a new generator. The table name is
        /// initialised to "tbnumbers".
        /// </summary>
        /// <param name="settingName">The database setting name that
        /// stores the number</param>
        public DatabaseNumberGenerator(string settingName) : this(settingName, "tbnumbers")
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
            itsSettingName = settingName;
            itsTableName = tableName;
            itsSeedValue = seedValue;

            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection(),
                                 "select SettingValue from " + itsTableName + " where SettingName = ");
            statement.AddParameterToStatement(itsSettingName);
            IDataReader reader = null;
            try
            {
                reader = DatabaseConnection.CurrentConnection.LoadDataReader(statement);
                if (reader.Read())
                {
                    itsNumber = Convert.ToInt32(reader.GetValue(0));
                }
                else
                {
                    itsNumber = itsSeedValue;
                    DatabaseConnection.CurrentConnection.ExecutePlainSql("insert into " + itsTableName +
                                                                         " (SettingName, SettingValue) values ('" +
                                                                         itsSettingName + "', " + itsSeedValue + ")");
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
            return ++itsNumber;
        }

        /// <summary>
        /// Creates a database transaction that updates the database to the
        /// last number dispensed, so the next number dispensed will be a
        /// fresh increment
        /// </summary>
        /// <returns>Returns an ITransaction object</returns>
        public ITransaction GetUpdateTransaction()
        {
            return new NumberUpdate(itsNumber, itsSettingName, itsTableName);
        }

        /// <summary>
        /// Creates a transaction to update the number in the database
        /// </summary>
        private class NumberUpdate : ITransaction
        {
            private int itsNewNumber;
            private readonly string itsSettingName;
            private readonly string itsTableName;

            /// <summary>
            /// Constructor to initialise a new update
            /// </summary>
            /// <param name="newNumber">The new number to store</param>
            /// <param name="settingName">The database setting name</param>
            /// <param name="tableName">The database table name</param>
            public NumberUpdate(int newNumber, string settingName, string tableName)
            {
                itsNewNumber = newNumber;
                itsSettingName = settingName;
                itsTableName = tableName;
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            public void TransactionCommited()
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
                                                          " update " + itsTableName + " set SettingValue = ");
                statement.AddParameterToStatement(itsNewNumber.ToString());
                statement.Statement.Append(" where SettingName = ");
                statement.AddParameterToStatement(itsSettingName);

                return new SqlStatementCollection(statement);
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            public void CheckPersistRules()
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

            /// <summary>
            /// Returns int.MinValue
            /// </summary>
            /// <returns>Returns int.MinValue</returns>
            public int TransactionRanking()
            {
                return int.MinValue;
            }

            /// <summary>
            /// Returns the number as a string
            /// </summary>
            /// <returns>Returns a string</returns>
            public string StrID()
            {
                return itsNewNumber.ToString();
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            public void BeforeCommit()
            {
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            public void AfterCommit()
            {
            }
        }
    }
}