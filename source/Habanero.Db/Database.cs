using System;
using System.Data;

namespace Habanero.DB
{
    /// <summary>
    /// Executes sql statements
    /// </summary>
    public class Database
    {
        private Database() {}

        /// <summary>
        /// Executes the given sql statement using the database connection 
        /// provided
        /// </summary>
        /// <param name="statement">The sql statement</param>
        /// <param name="connection">The database connection</param>
        public static void ExecuteSqlStatement(SqlStatement statement, IDbConnection connection)
        {
            if (statement == null) throw new ArgumentNullException("statement");
            if (connection == null) throw new ArgumentNullException("connection");
            IDbCommand cmd;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            cmd = connection.CreateCommand();
            statement.SetupCommand(cmd);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes the sql given as a raw string, using the database
        /// connection provided.  It is generally preferable to use the
        /// ExecuteSqlStatement() method, since this provides error
        /// checking for the components of the sql statement that you build up.
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <param name="connection">The database connection</param>
        public static void ExecuteRawSql(string sql, IDbConnection connection)
        {
            IDbCommand cmd;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}