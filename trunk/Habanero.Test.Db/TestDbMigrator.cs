using System;
using System.Collections.Generic;
using System.Text;
using Habanero.DB;
using Habanero.Base;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDBMigrator
    {
        DBMigrator itsDbMigrator;
        IDatabaseConnection itsConn;
        Mock itsConnMock;
        ISettings _itsSettings;
        Mock itsSettingsMock;
        
        [SetUp]
        public void SetupFixture() {
            itsConnMock = new DynamicMock(typeof(IDatabaseConnection));
            itsConn = (IDatabaseConnection)itsConnMock.MockInstance;
            itsDbMigrator = new DBMigrator(itsConn);
            itsDbMigrator.AddMigration(1, "migration1;");
            itsDbMigrator.AddMigration(2, "migration2;");
            itsDbMigrator.AddMigration(3, "migration3;");

            itsSettingsMock = new DynamicMock(typeof(ISettings));
            _itsSettings = (ISettings)itsSettingsMock.MockInstance;            
       
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
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { new SqlStatementCollection(new SqlStatement(itsConn.GetConnection(), "migration2;")) });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            itsDbMigrator.Migrate(1, 2);
            itsConnMock.Verify();
            itsSettingsMock.Verify();
        }
        
        [Test]
        public void TestGetCurrentVersion() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "2", new object[] { DBMigrator.DatabaseVersionSetting });
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
            itsSettingsMock.Verify();
        }
        

        [Test, ExpectedException(typeof(ArgumentNullException ))]
        public void TestGetCurrentVersionFailure() {
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
        }

        [Test]
        public void TestGetCurrentVersionGlobalSettings()
        {
            itsSettingsMock.ExpectAndReturn("GetString", "2", new object[] { DBMigrator.DatabaseVersionSetting });
            GlobalRegistry.Settings = _itsSettings;            
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
            GlobalRegistry.Settings = null;
            itsSettingsMock.Verify();
        }        
        
        
        [Test]
        public void TestMigrateTo() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "1", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] {DBMigrator.DatabaseVersionSetting, "3"});
            SqlStatementCollection col = new SqlStatementCollection();
            col.Add(new SqlStatement(itsConn.GetConnection(), "migration2;"));
            col.Add(new SqlStatement(itsConn.GetConnection(), "migration3;"));
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { col  });
          
            itsDbMigrator.MigrateTo(3);
            
            itsConnMock.Verify();
            itsSettingsMock.Verify();
        }
        
        [Test]
        public void TestGetLatestVersion() {
            Assert.AreEqual(3, itsDbMigrator.LatestVersion());
        }

        [Test]
        public void TestAddSqlStatement() {
            SqlStatement statement = new SqlStatement(itsConn.GetConnection(), "test");
            itsDbMigrator.AddMigration(4, statement);
            Assert.AreEqual(statement, itsDbMigrator.GetMigration(4));
            
        }
    
        
        
    }
}
