//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util.File;
using log4net;
// Limiting the number of records for a Select
// -------------------------------------------
// SQL Server: SELECT TOP 10 * FROM [TABLE]
// DB2: SELECT * FROM [TABLE] FETCH FIRST 10 ROWS ONLY
// PostgreSQL: SELECT * FROM [TABLE] LIMIT 10
// Oracle: SELECT * FROM [TABLE] WHERE ROWNUM <= 10
// Sybase: SET ROWCOUNT 10 SELECT * FROM [TABLE]
// Firebird: SELECT FIRST 10 * FROM [TABLE]
// MySQL: SELECT * FROM [TABLE] LIMIT 10


namespace Habanero.DB
{
    /// <summary>
    /// A super-class to manage a database connection and execute sql commands
    /// </summary>
    /// "See registry (480) think typesafe as well."
    public abstract class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _assemblyName;
        private readonly string _className;
        private string _connectString;
        private IList _connections;
        //protected IDbConnection _currentDbConnection;
        private static IDatabaseConnection _currentDatabaseConnection;
        private static readonly ILog log = LogManager.GetLogger("Habanero.DB.DatabaseConnection");
        private int _timeoutPeriod = 30;

        ///// <summary>
        ///// A class constructor that creates a new connection to a
        ///// SqlServer database.
        ///// </summary>
        ///// TODO ERIC - hmm? only SqlServer?
        //static DatabaseConnection()
        //{
        //    new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection");
        //}

        /// <summary>
        /// Constructor that initialises a new set of null connections
        /// </summary>
        private DatabaseConnection()
        {
            _connections = new ArrayList(5);
        }

        //private IDbConnection sampleCon;

        /// <summary>
        /// Constructor that allows an assembly name and class name to
        /// be specified
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The database class name</param>
        protected DatabaseConnection(string assemblyName, string className) : this()
        {
            _assemblyName = assemblyName;
            _className = className;
            //this.CreateDatabaseConnection(assemblyName, className);
        }

        /// <summary>
        /// Constructor to initialise the connection with the assembly name,
        /// class name and connection string provided
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string. This can be
        /// generated by the various GetConnectionString() methods, tailored
        /// for the appropriate database vendors.</param>
        protected DatabaseConnection(string assemblyName, string className, string connectString)
            : this(assemblyName, className)
        {
            this.ConnectionString = connectString;
            //_currentDbConnection = GetNewConnection();
        }

        /// <summary>
        /// Creates a database connection using the assembly name and class
        /// name provided.
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        private IDbConnection CreateDatabaseConnection()
        {
            //Assembly dbAssembly;
            //dbAssembly = Assembly.LoadWithPartialName(_assemblyName);
            //if (dbAssembly == null)
            //{
            //    dbAssembly = Assembly.LoadFrom(_assemblyName + ".dll");
            //}
            //Type connectionType = dbAssembly.GetType(_className);
            try
            {
                Type connectionType = TypeLoader.LoadType(_assemblyName, _className);
                return (IDbConnection) Activator.CreateInstance(connectionType);
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(String.Format(
                    "An error occurred while attempting to connect to the " +
                    "database.  The assembly, '{0}', required for the " +
                    "database vendor specified in the configuration (eg. " +
                    "app.config) could not be loaded.  Please check that it " +
                    "has been included in the dependencies or references, " +
                    "or that it has been copied to the output/execution " +
                    "folder for this application.", _assemblyName),
                    ex);
            }
        }

        /// <summary>
        /// Creates a new connection object and assigns to it the
        /// connection string that is stored in this instance
        /// </summary>
        /// <returns>Returns the new IDbConnection object</returns>
        protected IDbConnection NewConnection
        {
            get
            {
                IDbConnection con = this.CreateDatabaseConnection();
                //IDbConnection) Activator.CreateInstance(sampleCon.GetType()); // new MySqlConnection(ConnectionString));
                try {
                    con.ConnectionString = this.ConnectionString;
                }
                catch (Exception ex) {
                    throw new DatabaseConnectionException("There was an error " +
                                                          "connecting to the database. The connection information was " +
                                                          "rejected by the database - connection information is either " +
                                                          "missing or incorrect.  In your configuration (eg. in app." +
                                                          "config), you require settings for vendor, server and " +
                                                          "database.  Depending on your setup, you may also need " +
                                                          "username, password and port. Consult the documentation for " +
                                                          "more detail on available options for these settings.", ex);
                }
				//Mark - I have no Idea what the point of the next 3 lines are but I have commented them out because they are pointless :)
				//if (con.State != ConnectionState.Open && this._className == "System.Data.OleDb.OleDbConnection") {
				//    con.Open();
				//}
//			if (con.State != ConnectionState.Open) 
//			{
//				con.Open() ;
//			}
                return con;
            }
        }

        /// <summary>
        /// Gets or sets the current database connection
        /// </summary>
        public static IDatabaseConnection CurrentConnection
        {
            get { return _currentDatabaseConnection; }
            set { _currentDatabaseConnection = value; }
        }

        /// <summary>
        /// Gets and sets the connection string used to connect with the
        /// database.  Closes and disposes existing connections before
        /// assigning a new connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectString; }
            set
            {
                foreach (IDbConnection dbConnection in _connections)
                {
                    try
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    } catch (Exception ex)
                    {
                        log.Warn("Error closing and disposing connection", ex);
                    }
                }
                _connections = new ArrayList(5);
                _connectString = value;
            }
        }

        /// <summary>
        /// Creates a connection and assigns this instance's connection string
        /// </summary>
        /// <returns>Returns a new IDbConnection object</returns>
        internal IDbConnection TestConnection
        {
            get
            {
                IDbConnection con = this.CreateDatabaseConnection();
                con.ConnectionString = this.ConnectionString;
                return con;
            }
        }

        /// <summary>
        /// Returns a connection string with the password removed.  This method
        /// serves as a secure way of displaying an error message in the case 
        /// of a connection error, without compromising confidentiality.
        /// </summary>
        /// <returns>Returns a connect string with no password</returns>
        public string ErrorSafeConnectString()
        {
            string connectString = ConnectionString;
            int pwdStartPos =
                connectString.ToUpper(CultureInfo.InvariantCulture).IndexOf("pwd=".ToUpper(CultureInfo.InvariantCulture));
            if (pwdStartPos <= 0)
            {
                pwdStartPos =
                    connectString.ToUpper(CultureInfo.InvariantCulture).IndexOf(
                        "password=".ToUpper(CultureInfo.InvariantCulture));
            }
            if (pwdStartPos >= 0)
            {
                int pwdEndPos = connectString.IndexOf(";", pwdStartPos);
                if (pwdEndPos < pwdStartPos)
                {
                    pwdEndPos = connectString.Length;
                }
                int numChars = pwdEndPos - pwdStartPos;
                connectString = connectString.Remove(pwdStartPos, numChars);
            }
            return connectString;
        }

        /// <summary>
        /// Either finds a closed connection and opens and returns it,
        /// or creates a new connection and returns that.  Throws an 
        /// exception and adds a message to the log if there is an 
        /// error opening a connection.
        /// </summary>
        /// <returns>Returns a new IDbConnection object</returns>
        private IDbConnection GetOpenConnectionForReading()
        {
            try
            {
                // looks for closed connections for reading because open 
                // connections could have readers still associated with them.
                foreach (IDbConnection dbConnection in _connections)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                    {
                        //} || dbConnection.State == ConnectionState.Broken) {
                        dbConnection.Open();
                        return dbConnection;
                    }
                }
                IDbConnection newDbConnection = this.NewConnection;
                //newDbConnection.Open() ;
                _connections.Add(newDbConnection);
                return newDbConnection;
            }
            catch (Exception ex)
            {
                log.Error("Error opening connection to db : " + ex.GetType().Name + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 8, true));
                throw;
            }
        }

        /// <summary>
        /// Returns the first closed connection available or returns a
        /// new connection object.  Throws an exception and adds a message 
        /// to the log if there is an error opening a connection.
        /// </summary>
        /// <returns>Returns a new IDbConnection object</returns>
        public IDbConnection GetConnection()
        {
            try
            {
                foreach (IDbConnection dbConnection in _connections)
                {
                    //if (dbConnection.State == ConnectionState.Open) {
                    //	return dbConnection;
                    //} else
                    if (dbConnection.State == ConnectionState.Closed)
                    {
                        //dbConnection.Open();
                        return dbConnection;
                    }
                }
                IDbConnection newDbConnection = this.NewConnection;
                _connections.Add(newDbConnection);
                return newDbConnection;

                //				if (_currentDbConnection == null || (_currentDbConnection.State == ConnectionState.Broken) ||
                //					(_currentDbConnection.State == ConnectionState.Closed) ||
                //					(_currentDbConnection.State == ConnectionState.Executing)) 
                //				{
                //					_currentDbConnection = GetNewConnection();
                //					_currentDbConnection.Open();
                //				}
                //				else 
                //				{
                //					//if the provided connection is not open, we will open it
                //					if (_currentDbConnection.State != ConnectionState.Open) 
                //					{
                //						_currentDbConnection.Open();
                //					}
                //				}
            }
            catch (Exception ex)
            {
                log.Error("Error opening connection to db : " + ex.GetType().Name + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 8, true));
                //throw ex;
                throw new DatabaseConnectionException("An error occurred while attempting " +
                    "to connect to the database.", ex);
            }
        }

        /// <summary>
        /// Creates and returns an open connection
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        protected IDbConnection OpenConnection
        {
            get
            {
                IDbConnection connection = this.GetConnection();
                connection.Open();
                return connection;
            }
        }

//		public IDbConnection GetConnection() {
//			if (_currentDbConnection == null) {
//				_currentDbConnection = GetNewConnection();
//				_connections.Add(_currentDbConnection);
//			}
//			return _currentDbConnection;
//		}

        //		public IDataReader LoadDataReader(SqlStatement selectSql,
        //		                                  string strSearchCriteria,
        //		                                  string strOrderByCriteria) {
        //			selectSql.AppendCriteria(strSearchCriteria) ;
        //			selectSql.AppendOrderBy(strOrderByCriteria) ;
        //			return LoadDataReader(selectSql);
        //		}

        /// <summary>
        /// Loads a data reader and specifies an order-by clause
        /// </summary>
        /// <param name="selectSql">The sql statement object</param>
        /// <param name="strOrderByCriteria">A sql order-by clause</param>
        /// <returns>Returns an IDataReader object</returns>
        public IDataReader LoadDataReader(ISqlStatement selectSql, string strOrderByCriteria)
        {
            if (selectSql == null) throw new ArgumentNullException("selectSql");
            //selectSql.AppendCriteria(strSearchCriteria) ;
            selectSql.AppendOrderBy(strOrderByCriteria);
            return LoadDataReader(selectSql);
        }

        /// <summary>
        /// Loads a data reader with the given raw sql select statement
        /// </summary>
        /// <param name="selectSql">The sql statement as a string</param>
        /// <returns>Returns an IDataReader object with the results of the query</returns>
        /// <exception cref="DatabaseReadException">Thrown when an error
        /// occurred while setting up the data reader.  Also sends error
        /// output to the log.</exception>        
        public IDataReader LoadDataReader(string selectSql) {
            if (selectSql == null) throw new ArgumentNullException("selectSql");
            IDbConnection con = null;
            try
            {
                con = GetOpenConnectionForReading();
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = selectSql;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                log.Error("Error reading from database : " + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 10, true));
                log.Error("Sql: " + selectSql);
                throw new DatabaseReadException(
                    "There was an error reading the database. Please contact your system administrator.",
                    "The DataReader could not be filled with", ex, selectSql, ErrorSafeConnectString());
            }
        }

        /// <summary>
        /// Loads a data reader
        /// </summary>
        /// <param name="selectSql">The sql statement object</param>
        /// <returns>Returns an IDataReader object</returns>
        /// <exception cref="DatabaseReadException">Thrown when an error
        /// occurred while setting up the data reader.  Also sends error
        /// output to the log.</exception>
        public IDataReader LoadDataReader(ISqlStatement selectSql)
        {
            if (selectSql == null)
            {
                throw new DatabaseConnectionException("The sql statement object " +
                    "that has been passed to LoadDataReader() is null.");
            }
            IDbConnection con = null;
            try
            {
                con = GetOpenConnectionForReading();
                IDbCommand cmd = con.CreateCommand();
                selectSql.SetupCommand(cmd);
                //log.Debug("LoadDataReader with sql statement: " + selectSql.ToString() ) ;
                //cmd.CommandText = selectSql;
                //_currentDbConnection = null;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                log.Error("Error reading from database : " + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 10, true));
                log.Error("Sql: " + selectSql.ToString());
                //				if (con != null && con.State != ConnectionState.Closed) 
                //				{
                //					con.Close();
                //				}
                Console.Out.WriteLine("Error reading from database : " + Environment.NewLine +
                                      ExceptionUtilities.GetExceptionString(ex, 10, true));
                Console.Out.WriteLine("Sql: " + selectSql.ToString());
                throw new DatabaseReadException(
                    "There was an error reading the database. Please contact your system administrator.",
                    "The DataReader could not be filled with", ex, selectSql.ToString(), ErrorSafeConnectString());
            }
        }

        /// <summary>
        /// Executes a sql command that returns no result set and takes no 
        /// parameters, using the provided connection
        /// </summary>
        /// <param name="sql">A valid sql statement (typically "insert",
        /// "update" or "delete"). Note that this assumes that the
        /// sqlCommand is not a stored procedure.</param>
        /// <returns>Returns the number of rows affected</returns>
        /// <future>
        /// In future override this method with others that allow you to 
        /// pass in stored procedures and parameters.
        /// </future>
        public int ExecuteSql(ISqlStatementCollection sql)
        {
            return ExecuteSql(sql, null);
        }

        /// <summary>
        /// Executes a sql command as before, but with the full sql string
        /// provided, rather than with a sql statement object
        /// </summary>
        /// <param name="sql">The sql statement as a string</param>
        /// <returns>Returns the number of rows affected</returns>
        public int ExecuteRawSql(string sql)
        {
            return ExecuteRawSql(sql, null);
        }


        /// <summary>
        /// Executes a sql command that returns no result set and takes no 
        /// parameters, using the provided connection.
        /// This method can be used effectively where the database vendor
        /// supports the execution of several sql statements in one
        /// ExecuteNonQuery.  However, for database vendors like Microsoft
        /// Access and MySql, the sql statements will need to be split up
        /// and executed as separate transactions.
        /// </summary>
        /// <param name="sql">A valid sql statement (typically "insert",
        /// "update" or "delete") as a string. Note that this assumes that the
        /// sqlCommand is not a stored procedure.</param>
        /// <param name="transaction">A valid transaction object in which the 
        /// sql must be executed, or null</param>
        /// <returns>Returns the number of rows affected</returns>
        /// <exception cref="DatabaseWriteException">Thrown if there is an
        /// error writing to the database.  Also outputs error messages to the log.
        /// </exception>
        /// <future>
        /// In future override this method with methods that allow you to 
        /// pass in stored procedures and parameters.
        /// </future>
        public virtual int ExecuteRawSql(string sql, IDbTransaction transaction)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(sql, "sql");
            IDbConnection con = null;
            try
            {
                IDbCommand cmd;
                if (transaction != null)
                {
                    con = transaction.Connection;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    cmd = con.CreateCommand();
                    cmd.Transaction = transaction;
                }
                else
                {
                    con = OpenConnection;
                    cmd = con.CreateCommand();
                } try
                {
                    cmd.CommandTimeout = _timeoutPeriod;
                } catch (NotSupportedException  )
                {
                }
                cmd.CommandText = sql;
                //log.Debug("ExecuteRawSql with sql statement: " + sql.ToString());
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error writing to database : " + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 10, true));
                log.Error("Sql: " + sql.ToString());
                Console.WriteLine("Error writing to database : " + Environment.NewLine +
                                  ExceptionUtilities.GetExceptionString(ex, 10, true));
                Console.WriteLine("Connect string: " + this.ErrorSafeConnectString());
                throw new DatabaseWriteException(
                    "There was an error writing to the database. Please contact your system administrator.",
                    "The command executeNonQuery could not be completed.", ex, sql.ToString(), ErrorSafeConnectString());
            }
            finally
            {
                if (transaction == null)
                {
                    if (con != null && con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
        }


        /// <summary>
        /// Executes a collection of sql commands that returns no result set 
        /// and takes no parameters, using the provided connection.
        /// This method can be used effectively where the database vendor
        /// supports the execution of several sql statements in one
        /// ExecuteNonQuery.  However, for database vendors like Microsoft
        /// Access and MySql, the sql statements will need to be split up
        /// and executed as separate transactions.
        /// </summary>
        /// <param name="sql">A valid sql statement object (typically "insert",
        /// "update" or "delete"). Note that this assumes that the
        /// sqlCommand is not a stored procedure.</param>
        /// <param name="transaction">A valid transaction object in which the 
        /// sql must be executed, or null</param>
        /// <returns>Returns the number of rows affected</returns>
        /// <exception cref="DatabaseWriteException">Thrown if there is an
        /// error writing to the database.  Also outputs error messages to the log.
        /// </exception>
        /// <future>
        /// In future override this method with methods that allow you to 
        /// pass in stored procedures and parameters.
        /// </future>
        public virtual int ExecuteSql(ISqlStatementCollection sql, IDbTransaction transaction)
        {
            bool inTransaction = false;
            ArgumentValidationHelper.CheckArgumentNotNull(sql, "sql");
            IDbConnection con = null;
            try
            {
                IDbCommand cmd;
                if (transaction != null)
                {
                    inTransaction = true;
                    con = transaction.Connection;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    cmd = con.CreateCommand();
                    cmd.Transaction = transaction;
                }
                else
                {
                    con = OpenConnection;
                    cmd = con.CreateCommand();
                    transaction = con.BeginTransaction();
                    cmd.Transaction = transaction;
                }
                int totalRowsAffected = 0;
                //log.Debug("ExecuteSql with sql statement collection: " + sql.ToString());
                foreach (SqlStatement statement in sql)
                {
                    statement.SetupCommand(cmd);
                    //cmd.CommandText = sql;
                    totalRowsAffected += cmd.ExecuteNonQuery();
                    statement.DoAfterExecute(this, transaction, cmd);
                    //statement.UpdateAutoIncrement(this);
                }
                if (!inTransaction)
                {
                    transaction.Commit();
                }
                return totalRowsAffected;
            }
            catch (Exception ex)
            {
                log.Error("Error writing to database : " + Environment.NewLine +
                          ExceptionUtilities.GetExceptionString(ex, 10, true));
                log.Error("Sql: " + sql.ToString());
                if (!inTransaction && transaction != null)
                {
                    transaction.Rollback();
                }
                throw new DatabaseWriteException(
                    "There was an error writing to the database. Please contact your system administrator.",
                    "The command executeNonQuery could not be completed.", ex, sql.ToString(), ErrorSafeConnectString());
            }
            finally
            {
                if (!inTransaction)
                {
                    if (con != null && con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Executes a single sql statement object
        /// </summary>
        /// <param name="sql">The sql statement object</param>
        /// <returns>Returns the number of rows affected</returns>
        public int ExecuteSql(ISqlStatement sql) {
            if (sql == null) throw new ArgumentNullException("sql");
            return ExecuteSql(new SqlStatementCollection(sql));
        }

        /// <summary>
        /// Returns a left square bracket
        /// </summary>
        public virtual string LeftFieldDelimiter
        {
            get { return "["; }
        }

        /// <summary>
        /// Returns a right square bracket
        /// </summary>
        public virtual string RightFieldDelimiter
        {
            get { return "]"; }
        }

        /// <summary>
        /// Returns a limit clause with the limit specified, with the format
        /// as " TOP [limit] " (eg. " TOP 4 ")
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Returns a string</returns>
        public virtual string GetLimitClauseForBeginning(int limit)
        {
            return " TOP " + limit + " ";
        }

        /// <summary>
        /// Returns an empty string in this implementation
        /// </summary>
        /// <param name="limit">The limit - has no relevance in this 
        /// implementation</param>
        /// <returns>Returns an empty string in this implementation</returns>
        public virtual string GetLimitClauseForEnd(int limit)
        {
            return "";
        }

        /// <summary>
        /// Set the time-out period in seconds, after which the connection
        /// attempt will fail
        /// </summary>
        /// <param name="timeoutSeconds">The time-out period in seconds</param>
        public void SetTimeoutPeriod(int timeoutSeconds)
        {
            _timeoutPeriod = timeoutSeconds;
        }

        /// <summary>
        /// Loads data from the database into a DataTable object, using the
        /// sql statement object provided
        /// </summary>
        /// <param name="selectSql">The sql statement object</param>
        /// <param name="strSearchCriteria">The search criteria as a string
        /// to append</param>
        /// <param name="strOrderByCriteria">The order by criteria as a string
        /// to append</param>
        /// <returns>Returns a DataTable object</returns>
        /// <exception cref="DatabaseReadException">Thrown if there is an
        /// error reading the database.  Also outputs error messages to the log.
        /// </exception>
        public DataTable LoadDataTable(ISqlStatement selectSql, string strSearchCriteria, string strOrderByCriteria)
        {
            if (selectSql == null) throw new ArgumentNullException("selectSql");
            IDbConnection con = null;
            try
            {
                con = GetOpenConnectionForReading();
                IDbCommand cmd = con.CreateCommand();
                selectSql.SetupCommand(cmd);
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    //CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add();
                    }
                    do
                    {
                        DataRow row = dt.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.GetValue(i);
                        }
                        dt.Rows.Add(row);
                    } while (reader.Read());
                }
                reader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                log.Error("Error in LoadDataTable:" + Environment.NewLine + ExceptionUtilities.GetExceptionString(ex, 8, true));
                log.Error("Sql string: " + selectSql.ToString());
                //				if (con != null && con.State != ConnectionState.Closed) {
                //					con.Close();
                //				}
                throw new DatabaseReadException(
                    "There was an error reading the database. Please contact your system administrator.",
                    "The DataReader could not be filled with", ex, selectSql.ToString(), ErrorSafeConnectString());
            }
        }

        /// <summary>
        /// Gets the value of the last auto-incrementing number.  This called after doing an insert statement so that
        /// the inserted auto-number can be retrieved.  The table name, current IDbTransaction and IDbCommand are passed
        /// in so that they can be used if necessary.  Note, this must be overridden in subclasses to include support
        /// for this feature in different databases - otherwise a NotImplementedException will be thrown.
        /// </summary>
        /// <param name="tableName">The name of the table inserted into</param>
        /// <param name="tran">The current transaction, the one the insert was done in</param>
        /// <param name="command">The Command the did the insert statement</param>
        /// <returns></returns>
        public virtual long GetLastAutoIncrementingID(string tableName, IDbTransaction tran, IDbCommand command)
        {
            throw new NotImplementedException("GetLastAutoIncrementingID is not implemented on DatabaseConnection of type " + _className +
                                              " in assembly " + _assemblyName);
        }
    }
}