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
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionSqlServer
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionSqlServer("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual("@", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }
        
        [Test]
        public void Test_NoColumnName_DoesntError_SqlServer()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection originalConnection = DatabaseConnection.CurrentConnection;
            DatabaseConfig databaseConfig = new DatabaseConfig("SqlServer", "localhost", "habanero_test_branch_2_3_1", "sa", "sa", null);
            DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
            //DatabaseConnection.CurrentConnection = new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection","server=localhost;database=habanero_test_branch_2_3_1;user=sa;password=sa");
            const string sql = "Select FirstName + ', ' + Surname from tbPersonTable";
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection, sql);

            //---------------Execute Test ----------------------
            DataTable dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataTable.Columns.Count);

            //---------------Tear Down -------------------------     
            DatabaseConnection.CurrentConnection = originalConnection;
        }
    }
}