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

using System;
using Habanero.Base;
using Habanero.DB;
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

        private string appName = GlobalRegistry.ApplicationName;
        private string appVersion = GlobalRegistry.ApplicationVersion;
        private int dbVersion = GlobalRegistry.DatabaseVersion;
        private ISettings settings = GlobalRegistry.Settings;
        private IExceptionNotifier exNotifier = GlobalRegistry.UIExceptionNotifier;

        [SetUp]
        public void SetupFixture()
        {
            GlobalRegistry.ApplicationName = appName;
            GlobalRegistry.ApplicationVersion = appVersion;
            GlobalRegistry.DatabaseVersion = dbVersion;
            GlobalRegistry.Settings = settings;
            GlobalRegistry.UIExceptionNotifier = exNotifier;

            itsConnMock = new DynamicMock(typeof(IDatabaseConnection));
            itsConn = (IDatabaseConnection)itsConnMock.MockInstance;
            itsDbMigrator = new DBMigrator(itsConn);
            itsDbMigrator.AddMigration(1, "migration1;");
            itsDbMigrator.AddMigration(2, "migration2;");
            itsDbMigrator.AddMigration(3, "migration3;");

            itsSettingsMock = new DynamicMock(typeof(ISettings));
            _itsSettings = (ISettings)itsSettingsMock.MockInstance;
        }

        [TearDown]
        public void RestoreRegistry()
        {
            GlobalRegistry.ApplicationName = appName;
            GlobalRegistry.ApplicationVersion = appVersion;
            GlobalRegistry.DatabaseVersion = dbVersion;
            GlobalRegistry.Settings = settings;
            GlobalRegistry.UIExceptionNotifier = exNotifier;
        }
        
        [Test]
        public void TestAddMigration() {
            Assert.AreEqual(3, itsDbMigrator.MigrationCount);
        }
        
        [Test]
        public void TestGetMigration() {
            Assert.AreEqual(new SqlStatement(itsConn, "migration2;"), itsDbMigrator.GetMigration(2));
        }

        [Test]
        public void TestGetMigrateSql()
        {
            SqlStatementCollection  sqlCol = itsDbMigrator.GetMigrationSql(0, 3);
            Assert.AreEqual(3, sqlCol.Count);
            Assert.AreEqual(new SqlStatement(itsConn, "migration1;"), sqlCol[0]);
        }
        
        [Test]
        public void TestGetMigrateSqlBoundaries() {
            SqlStatementCollection sqlCol = itsDbMigrator.GetMigrationSql(1, 2);
            Assert.AreEqual(1, sqlCol.Count);
            Assert.AreEqual(new SqlStatement(itsConn, "migration2;"), sqlCol[0]);
        }
        
        [Test]
        public void TestMigrate() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { new SqlStatementCollection(new SqlStatement(itsConn, "migration2;")) });
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
        

        [Test, Ignore("This fails through the resharper tester."), ExpectedException(typeof(ArgumentNullException ))]
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
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "3" });
            SqlStatementCollection sqlStatementCollection;
            sqlStatementCollection = new SqlStatementCollection();
            sqlStatementCollection.Add(new SqlStatement(itsConn, "migration2;"));
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { sqlStatementCollection  });
            sqlStatementCollection = new SqlStatementCollection();
            sqlStatementCollection.Add(new SqlStatement(itsConn, "migration3;"));
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { sqlStatementCollection  });
          
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
            SqlStatement statement = new SqlStatement(itsConn, "test");
            itsDbMigrator.AddMigration(4, statement);
            Assert.AreEqual(statement, itsDbMigrator.GetMigration(4));
            
        }
    
        
        
    }
}
