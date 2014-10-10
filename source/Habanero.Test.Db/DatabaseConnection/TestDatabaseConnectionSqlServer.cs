#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
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
	public class TestDatabaseConnectionSqlServer
	{// ReSharper disable InconsistentNaming

	    private IDatabaseConnection _originalConnection;

	    [TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			_originalConnection = DatabaseConnection.CurrentConnection;
			SetupSQLServerConnection();
		}

	    private static void SetupSQLServerConnection()
	    {
	        var databaseConnection = TestUsingDatabase.CreateDatabaseConnection(DatabaseConfig.SqlServer);
	        DatabaseConnection.CurrentConnection = databaseConnection;
	    }

	    [TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			DatabaseConnection.CurrentConnection = _originalConnection;
		}

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
            const string sql = "Select LookupValue + ', ' + LookupValue from database_lookup_int";
			var sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection, sql);

			//---------------Execute Test ----------------------
			var dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");

			//---------------Test Result -----------------------
			Assert.AreEqual(1, dataTable.Columns.Count);

			//---------------Tear Down -------------------------     
		}

	    [Test]
		public void PersistSQLparamaterValue_WhenByteArrayNull_WhenSQLServer_ShouldNotExist_FIXBUG1741()
		{
			//---------------Set up test pack-------------------
			var loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
			var classDef = loader.LoadClass(@"
					<class name=""MyBO"" assembly=""Habanero.Test"">
						<property  name=""MyBoID"" type=""Guid"" />
						<property  name=""ByteArrayProp"" type=""Byte[]"" />
						<primaryKey>
							<prop name=""MyBoID"" />
						</primaryKey>
					</class>
				");
            ClassDef.ClassDefs.Clear();
			ClassDef.ClassDefs.Add(classDef);
			var bo = classDef.CreateNewBusinessObject();
			bo.SetPropertyValue("ByteArrayProp", null);
			//---------------Assert Precondition----------------

			//---------------Execute Test ----------------------
			var sqlCol = new TransactionalBusinessObjectDB(bo, DatabaseConnection.CurrentConnection).GetPersistSql();
			var sqlStatement = sqlCol.First();
			//IList parameters = sqlStatement.Parameters;

			DatabaseConnection.CurrentConnection.ExecuteSql(sqlStatement);
			//---------------Test Result -----------------------
			Assert.Pass("If it got here then it is OK");
		}
	}
}