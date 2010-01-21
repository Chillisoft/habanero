// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Data;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that manages a database connection 
    /// and executes sql commands
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Gets and sets the database connection string
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Returns a database connection string with the password removed
        /// for data security purposes
        /// </summary>
        /// <returns>Returns a string</returns>
        string ErrorSafeConnectString();

        /// <summary>
        /// Returns a database connection 
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Loads a data reader
        /// </summary>
        /// <param name="selectSql">The sql statement object</param>
        /// <returns>Returns an IDataReader object</returns>
        IDataReader LoadDataReader(ISqlStatement selectSql);

        /// <summary>
        /// Loads a data reader and specifies an order-by clause
        /// </summary>
        /// <param name="selectSql">The sql statement object</param>
        /// <param name="strOrderByCriteria">A sql order-by clause</param>
        /// <returns>Returns an IDataReader object</returns>
        IDataReader LoadDataReader(ISqlStatement selectSql, string strOrderByCriteria);

        /// <summary>
        /// Loads a data reader with the given raw sql select statement
        /// </summary>
        /// <param name="selectSql">The sql statement as a string</param>
        /// <returns>Returns an IDataReader object with the results of the query</returns>
        IDataReader LoadDataReader(string selectSql);

        /// <summary>
        /// Loads a data reader with the given raw sql select statement for the specified transaction
        /// </summary>
        /// <param name="selectSql">The sql statement as a string</param>
        /// <param name="transaction">Thransaction that gives the context within which the sql statement should be executed</param>
        /// <returns>Returns an IDataReader object with the results of the query</returns>
        /// <exception>DataBaseReadException Thrown when an error
        /// occurred while setting up the data reader.  Also sends error
        /// output to the log.</exception>        
        IDataReader LoadDataReader(string selectSql, IDbTransaction transaction);

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
        DataTable LoadDataTable(ISqlStatement selectSql, string strSearchCriteria, string strOrderByCriteria);

        /// <summary>
        /// Executes a sql command using the sql string provided
        /// </summary>
        /// <param name="sql">The sql statement as a string</param>
        /// <returns>Returns the number of rows affected</returns>
        int ExecuteRawSql(string sql);

        /// <summary>
        /// Executes a sql command that returns no result set and takes no 
        /// parameters, using the provided connection
        /// </summary>
        /// <param name="sql">The sql statement as a string</param>
        /// <param name="transaction">A valid transaction object in which the 
        /// sql must be executed, or null</param>
        /// <returns>Returns the number of rows affected</returns>
        int ExecuteRawSql(string sql, IDbTransaction transaction);
        
        /// <summary>
        /// Executes a collection of sql statements
        /// </summary>
        /// <param name="sql">The collection of sql statements</param>
        /// <returns>Returns the number of rows affected</returns>
        int ExecuteSql(ISqlStatementCollection sql);

        /// <summary>
        /// Executes a collection of sql statements
        /// </summary>
        /// <param name="sql">The collection of sql statements</param>
        /// <param name="transaction">A valid transaction object in which the 
        /// sql must be executed, or null</param>
        /// <returns>Returns the number of rows affected</returns>
        int ExecuteSql(ISqlStatementCollection sql, IDbTransaction transaction);

        /// <summary>
        /// Executes a single sql statement object
        /// </summary>
        /// <param name="sql">The sql statement object</param>
        /// <returns>Returns the number of rows affected</returns>
        int ExecuteSql(ISqlStatement sql);
        
        /// <summary>
        /// Returns the left field delimiter appropriate to the database
        /// vendor
        /// </summary>
        string LeftFieldDelimiter { get; }

        /// <summary>
        /// Returns the right field delimiter appropriate to the database
        /// vendor
        /// </summary>
        string RightFieldDelimiter { get; }

        ///<summary>
        /// Creates a SQL formatter for the specified database.
        ///</summary>
        ISqlFormatter SqlFormatter { get; }

        /// <summary>
        /// Gets the IsolationLevel to use for this connection
        /// </summary>
        IsolationLevel IsolationLevel { get; }

        /// <summary>
        /// Set the time-out period in seconds
        /// </summary>
        /// <param name="timeoutSeconds">The time-out period in seconds</param>
        void SetTimeoutPeriod(int timeoutSeconds);

        /// <summary>
        /// Creates an <see cref="IParameterNameGenerator"/> for this database connection.  This is used to create names for parameters
        /// added to an <see cref="ISqlStatement"/> because each database uses a different naming convention for their parameters.
        /// </summary>
        /// <returns>The <see cref="IParameterNameGenerator"/> valid for this <see cref="IDatabaseConnection"/></returns>
        IParameterNameGenerator CreateParameterNameGenerator();

        /// <summary>
        /// Creates a transaction on the given connection.
        /// </summary>
        /// <param name="openConnection"></param>
        /// <returns></returns>
        IDbTransaction BeginTransaction(IDbConnection openConnection);
    }
}