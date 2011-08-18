// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
	[TestFixture]
	public class TestDatabaseConnectionSqlServerCe
	{
		private IDatabaseConnection _originalConnection;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			_originalConnection = DatabaseConnection.CurrentConnection;
			SetupSQLServerCeConnection();
		}
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			DatabaseConnection.CurrentConnection = _originalConnection;
		}
// ReSharper disable InconsistentNaming
		[Test]
		public void TestCreateParameterNameGenerator()
		{
			//---------------Set up test pack-------------------
			var databaseConnection = new DatabaseConnectionSqlServer("", "");
			//---------------Assert PreConditions---------------            
			//---------------Execute Test ----------------------
			var generator = databaseConnection.CreateParameterNameGenerator();
			//---------------Test Result -----------------------
			Assert.AreEqual("@", generator.PrefixCharacter);
			//---------------Tear Down -------------------------          
		}
		
		[Test]
		public void Test_NoColumnName_DoesntError_SqlServer()
		{
			//---------------Set up test pack-------------------
			const string sql = "Select FirstName + ', ' + Surname from tbPersonTable";
			var sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection, sql);

			//---------------Execute Test ----------------------
			var dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");

			//---------------Test Result -----------------------
			Assert.AreEqual(1, dataTable.Columns.Count);

			//---------------Tear Down -------------------------     
		}

		private static void SetupSQLServerCeConnection()
		{
			var databaseConfig = new DatabaseConfig(DatabaseConfig.SqlServerCe, "", "sqlserverce-testdb.sdf", "", "", null);
			DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
		}

		[Test]
		public void Test_IsolationLevel_SQLServerCe()
		{
			//---------------Execute Test ----------------------
			IDatabaseConnection conn = DatabaseConnection.CurrentConnection;
			//---------------Test Result -----------------------
			Assert.AreEqual(IsolationLevel.ReadCommitted, conn.IsolationLevel);
		}

		[Test]
		public void Test_CreateSqlFormatter_SQLServerCe()
		{
			//---------------Set up test pack-------------------
			IDatabaseConnection dbConn = DatabaseConnection.CurrentConnection;
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
			SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
			Assert.IsNotNull(sqlFormatter);
			Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
			Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
			Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
			Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
		}
	}
}