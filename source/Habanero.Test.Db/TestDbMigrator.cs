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
using System;
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
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
            var sqlCol = itsDbMigrator.GetMigrationSql(0, 3);
            var sqlStatements = sqlCol.ToList();
            Assert.AreEqual(3, sqlStatements.Count);
            Assert.AreEqual(new SqlStatement(itsConn, "migration1;"), sqlStatements[0]);
        }
        
        [Test]
        public void TestGetMigrateSqlBoundaries() {
            var sqlCol = itsDbMigrator.GetMigrationSql(1, 2);
            var sqlStatements = sqlCol.ToList();
            Assert.AreEqual(1, sqlStatements.Count);
            Assert.AreEqual(new SqlStatement(itsConn, "migration2;"), sqlStatements[0]);
        }
        
        [Test]
        public void TestMigrate()
        {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { new[] { new SqlStatement(itsConn, "migration2;") } });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            itsDbMigrator.Migrate(1, 2);
            itsConnMock.Verify();
            itsSettingsMock.Verify();
        }

        [Test]
        public void Event_WhenMigrationStartsAndMigrationIsRequired_OnMigrationStartedEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "1", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            DBMigrator eventMigrator = null;
            DBMigratorEventArgs eventArgs = null;
            itsDbMigrator.OnDbMigrationStarted += (s, e) =>
                {
                    eventMigrator = s as DBMigrator;
                    eventArgs = e;
                };
            
            //---------------Assert Precondition----------------
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);

            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(eventMigrator);
            Assert.AreEqual(itsDbMigrator, eventMigrator);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(2, eventArgs.CurrentStep);
        }

        [Test]
        public void Event_WhenMigrationStartsAndNoMigrationRequired_OnMigrationStartedEventNotRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "2", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            DBMigrator eventMigrator = null;
            DBMigratorEventArgs eventArgs = null;
            var onDbMigrationStartedEventCalled = false;
            itsDbMigrator.OnDbMigrationStarted += (s, e) =>
                {
                    onDbMigrationStartedEventCalled = true;
                    eventMigrator = s as DBMigrator;
                    eventArgs = e;
                };
            
            //---------------Assert Precondition----------------
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);
            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(2);

            //---------------Test Result -----------------------
            Assert.IsFalse(onDbMigrationStartedEventCalled);
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);
        }
        
        [Test]
        public void Event_WhenMigrationCompletesAndMigrationIsRequired_OnMigrationCompletedEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "1", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            DBMigrator eventMigrator = null;
            DBMigratorEventArgs eventArgs = null;
            itsDbMigrator.OnDbMigrationCompleted += (s, e) =>
                {
                    eventMigrator = s as DBMigrator;
                    eventArgs = e;
                };
            
            //---------------Assert Precondition----------------
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);

            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(2);

            //---------------Test Result -----------------------
            Assert.IsNotNull(eventMigrator);
            Assert.AreEqual(itsDbMigrator, eventMigrator);
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(2, eventArgs.CurrentStep);
        }

        [Test]
        public void Event_WhenMigrationCompletesAndNoMigrationRequired_OnMigrationCompletedEventNotRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "2", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            DBMigrator eventMigrator = null;
            DBMigratorEventArgs eventArgs = null;
            var onDbMigrationCompletedCalled = false;
            itsDbMigrator.OnDbMigrationCompleted += (s, e) =>
                {
                    onDbMigrationCompletedCalled = true;
                    eventMigrator = s as DBMigrator;
                    eventArgs = e;
                };
            
            //---------------Assert Precondition----------------
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);
            Assert.IsFalse(onDbMigrationCompletedCalled);
            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(2);

            //---------------Test Result -----------------------
            Assert.IsFalse(onDbMigrationCompletedCalled);
            Assert.IsNull(eventMigrator);
            Assert.IsNull(eventArgs);
        }

        [Test]
        public void Event_WhenMigratorRunsStep_OnMigrationProgressEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "1", new object[] { DBMigrator.DatabaseVersionSetting });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "2" });
            itsSettingsMock.ExpectAndReturn("SetString", null, new object[] { DBMigrator.DatabaseVersionSetting, "3" });

            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            statements = new [] {new SqlStatement(itsConn, "migration3;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });

            var progressValues = new List<decimal>();
            itsDbMigrator.OnDbMigrationProgress += (s, e) =>
                {
                    progressValues.Add(e.PercentageComplete);
                };
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, progressValues.Count());
            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(3);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, progressValues.Count());
            Assert.AreEqual(50, progressValues[0]);
            Assert.AreEqual(100, progressValues[1]);
        }
        
        [Test]
        public void TestGetCurrentVersion() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            itsSettingsMock.ExpectAndReturn("GetString", "2", new object[] { DBMigrator.DatabaseVersionSetting });
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
            itsSettingsMock.Verify();
        }
        
        [Test]
        public void TestGetCurrentVersionFailure() {
            //---------------Set up test pack-------------------
            ISettings oldSettings = GlobalRegistry.Settings;
            GlobalRegistry.Settings = null;
            itsDbMigrator.SetSettingsStorer(null);
            //---------------Execute Test ----------------------
            try
            {
                itsDbMigrator.CurrentVersion();
                Assert.Fail("Expected to throw an InvalidOperationException");
            //---------------Test Result -----------------------
            } catch (InvalidOperationException ex)
            {
               StringAssert.Contains("does not exist", ex.Message);
            }
            //---------------Tear Down -------------------------          
            finally
            {
                itsDbMigrator.SetSettingsStorer(_itsSettings);
                GlobalRegistry.Settings = oldSettings;
            }
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
            var statements = new [] {new SqlStatement(itsConn, "migration2;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
            statements = new [] {new SqlStatement(itsConn, "migration3;")};
            itsConnMock.ExpectAndReturn("ExecuteSql", 0, new object[] { statements  });
          
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
