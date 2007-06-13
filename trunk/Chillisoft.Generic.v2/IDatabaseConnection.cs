using System.Data;

namespace Chillisoft.Generic.v2
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

        //IDataReader LoadDataReader(SqlStatement selectSQL, string strSearchCriteria, string strOrderByCriteria);

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
        /// Loads data from the database into a DataTable object, using the
        /// sql statement object provided
        /// </summary>
        /// <param name="selectSQL">The sql statement object</param>
        /// <param name="strSearchCriteria">The search criteria as a string
        /// to append</param>
        /// <param name="strOrderByCriteria">The order by criteria as a string
        /// to append</param>
        /// <returns>Returns a DataTable object</returns>
        DataTable LoadDataTable(ISqlStatement selectSQL, string strSearchCriteria, string strOrderByCriteria);

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
        
        /// <summary>
        /// Returns the beginning limit clause with the limit specified
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Returns a string</returns>
        string GetLimitClauseForBeginning(int limit);

        /// <summary>
        /// Returns the ending limit clause with the limit specified
        /// </summary>
        /// <param name="limit">The limit</param>
        /// <returns>Returns a string</returns>
        string GetLimitClauseForEnd(int limit);

        /// <summary>
        /// Set the time-out period in seconds
        /// </summary>
        /// <param name="timeoutSeconds">The time-out period in seconds</param>
        void SetTimeoutPeriod(int timeoutSeconds);
        
        //string RightDateDelimiter { get; }
        //string LeftDateDelimiter { get; }
    }
}