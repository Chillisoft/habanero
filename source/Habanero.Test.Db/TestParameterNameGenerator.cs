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

using System.Data;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestParameterNameGenerator : TestUsingDatabase
    {
        //	private ParameterNameGenerator gen;


        [Test]
		public void TestNameGenerationSqlServer()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
            Assert.AreEqual("@Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
        }

        [Test]
        public void TestNameGenerationMySql()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual("?Param0", gen.GetNextParameterName());
            Assert.AreEqual("?Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual("?Param0", gen.GetNextParameterName());
        }

		[Test]

		public void TestNameGenerationOracle()
		{
			DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
			IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
			ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
			Assert.AreEqual(":Param0", gen.GetNextParameterName());
			Assert.AreEqual(":Param1", gen.GetNextParameterName());
			gen.Reset();
			Assert.AreEqual(":Param0", gen.GetNextParameterName());
		}

        [Test]
        public void TestNameGenerationAccess()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
			Assert.AreEqual("@Param0", gen.GetNextParameterName());
			Assert.AreEqual("@Param1", gen.GetNextParameterName());
            gen.Reset();
			Assert.AreEqual("@Param0", gen.GetNextParameterName());
        }

		[Test]
		public void TestNameGenerationPostgreSql()
		{
			DatabaseConfig config = new DatabaseConfig(DatabaseConfig.PostgreSql, "test", "test", "test", "test", "1000");
			IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
			ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
			Assert.AreEqual(":Param0", gen.GetNextParameterName());
			Assert.AreEqual(":Param1", gen.GetNextParameterName());
			gen.Reset();
			Assert.AreEqual(":Param0", gen.GetNextParameterName());
		}

        [Test, Ignore("Issue with SQLite 64-bit driver")]
        public void TestNameGenerationSQLite()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLite, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
            Assert.AreEqual(":Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
        }

    }
}