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

using Habanero.DB;
using Habanero.Test;
using Habanero.Test.General;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for TestUsingDatabase.
    /// </summary>
    public class TestUsingDatabase : ArchitectureTest
    {
        public void SetupDBConnection()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof (DatabaseConnectionMySql))
            {
                return;
            }
            else
            {
                DatabaseConnection.CurrentConnection =
                    new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
                DatabaseConnection.CurrentConnection.ConnectionString =
                    MyDBConnection.GetDatabaseConfig().GetConnectionString();
                DatabaseConnection.CurrentConnection.GetConnection();
            }
        }

        public void SetupDBOracleConnection()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionOracle))
            {
                return;
            }
            else
            {
                DatabaseConnection.CurrentConnection =
                    new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
                ConnectionStringOracleFactory oracleConnectionString = new ConnectionStringOracleFactory();
                string connStr = oracleConnectionString.GetConnectionString("core1", "XE", "system", "system", "1521");
                DatabaseConnection.CurrentConnection.ConnectionString = connStr;
                DatabaseConnection.CurrentConnection.GetConnection();
            }
        }
    }
}