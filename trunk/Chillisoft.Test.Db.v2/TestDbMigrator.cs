using System;
using System.Collections.Generic;
using System.Text;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.Db.v2
{
    [TestFixture]
    public class TestDbMigrator
    {
        DbMigrator itsDbMigrator;
        IDatabaseConnection itsConn;
        Mock itsConnMock;
        ISettingsStorer itsSettingsStorer;
        Mock itsSettingsStorerMock;
        
        [SetUp]
        public void SetupFixture() {
            itsConnMock = new DynamicMock(typeof(IDatabaseConnection));
            itsConn = (IDatabaseConnection)itsConnMock.MockInstance;
            itsDbMigrator = new DbMigrator(itsConn);
            itsDbMigrator.AddMigration(1, "migration1;");
            itsDbMigrator.AddMigration(2, "migration2;");
            itsDbMigrator.AddMigration(3, "migration3;");

            itsSettingsStorerMock = new DynamicMock(typeof(ISettingsStorer));
            itsSettingsStorer = (ISettingsStorer)itsSettingsStorerMock.MockInstance;            
       
        }
        
        [Test]
        public void TestAddMigration() {
            Assert.AreEqual(3, itsDbMigrator.MigrationCount);
        }
        
        [Test]
        public void TestGetMigration() {
            Assert.AreEqual(new SqlStatement(itsConn.GetConnection(), "migration2;"), itsDbMigrator.GetMigration(2));
        }

        [Test]
        public void TestGetMigrateSql()
        {
            SqlStatementCollection  sqlCol = itsDbMigrator.GetMigrationSql(0, 3);
            Assert.AreEqual(3, sqlCol.Count);
            Assert.AreEqual(new SqlStatement(itsConn.GetConnection(), "migration1;"), sqlCol[0]);
        }
        
        [Test]
        public void TestGetMigrateSqlBoundaries() {
            SqlStatementCollection sqlCol = itsDbMigrator.GetMigrationSql(1, 2);
            Assert.AreEqual(1, sqlCol.Count);
            Assert.AreEqual(new SqlStatement(itsConn.GetConnection(), "migration2;"), sqlCol[0]);
        }
        
        [Test]
        public void TestMigrate() {
            itsDbMigrator.SetSettingsStorer(itsSettingsStorer);
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { new SqlStatementCollection(new SqlStatement(itsConn.GetConnection(), "migration2;")) });
            itsSettingsStorerMock.ExpectAndReturn("SetString", null, new object[] { DbMigrator.DATABASE_VERSION_SETTING, "2" });
            itsDbMigrator.Migrate(1, 2);
            itsConnMock.Verify();
            itsSettingsStorerMock.Verify();
        }
        
        [Test]
        public void TestGetCurrentVersion() {
            itsDbMigrator.SetSettingsStorer(itsSettingsStorer);
            itsSettingsStorerMock.ExpectAndReturn("GetString", "2", new object[] { DbMigrator.DATABASE_VERSION_SETTING });
            Assert.AreEqual(2, itsDbMigrator.GetCurrentVersion());
            itsSettingsStorerMock.Verify();
        }
        

        [Test, ExpectedException(typeof(ArgumentNullException ))]
        public void TestGetCurrentVersionFailure() {
            Assert.AreEqual(2, itsDbMigrator.GetCurrentVersion());
        }

        [Test]
        public void TestGetCurrentVersionGlobalSettings()
        {
            itsSettingsStorerMock.ExpectAndReturn("GetString", "2", new object[] { DbMigrator.DATABASE_VERSION_SETTING });
            GlobalRegistry.SettingsStorer = itsSettingsStorer;            
            Assert.AreEqual(2, itsDbMigrator.GetCurrentVersion());
            GlobalRegistry.SettingsStorer = null;
            itsSettingsStorerMock.Verify();
        }        
        
        
        [Test]
        public void TestMigrateTo() {
            itsDbMigrator.SetSettingsStorer(itsSettingsStorer);
            itsSettingsStorerMock.ExpectAndReturn("GetString", "1", new object[] { DbMigrator.DATABASE_VERSION_SETTING });
            itsSettingsStorerMock.ExpectAndReturn("SetString", null, new object[] {DbMigrator.DATABASE_VERSION_SETTING, "3"});
            SqlStatementCollection col = new SqlStatementCollection();
            col.Add(new SqlStatement(itsConn.GetConnection(), "migration2;"));
            col.Add(new SqlStatement(itsConn.GetConnection(), "migration3;"));
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { col  });
          
            itsDbMigrator.MigrateTo(3);
            
            itsConnMock.Verify();
            itsSettingsStorerMock.Verify();
        }
        
        [Test]
        public void TestGetLatestVersion() {
            Assert.AreEqual(3, itsDbMigrator.GetLatestVersion());
        }

        [Test]
        public void TestAddSqlStatement() {
            SqlStatement statement = new SqlStatement(itsConn.GetConnection(), "test");
            itsDbMigrator.AddMigration(4, statement);
            Assert.AreEqual(statement, itsDbMigrator.GetMigration(4));
            
        }
    
        
        
    }
}
