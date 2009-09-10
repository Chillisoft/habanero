//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            IDbCommand cmd = connection.CreateCommand();
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
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}