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
using NSubstitute;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDBMigrator
    {
        DBMigrator itsDbMigrator;
        IDatabaseConnection itsConn;
        ISettings _itsSettings;

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

            itsConn = Substitute.For<IDatabaseConnection>();
            itsDbMigrator = new DBMigrator(itsConn);
            itsDbMigrator.AddMigration(1, "migration1;");
            itsDbMigrator.AddMigration(2, "migration2;");
            itsDbMigrator.AddMigration(3, "migration3;");

            _itsSettings = Substitute.For<ISettings>();
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
            itsDbMigrator.Migrate(1, 2);
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "2");
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration2;"));
        }

        [Test]
        public void Event_WhenMigrationStartsAndMigrationIsRequired_OnMigrationStartedEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("1");
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
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "2");
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration2;"));
        }


        [Test]
        public void Event_WhenMigrationStartsAndNoMigrationRequired_OnMigrationStartedEventNotRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("2");
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
            _itsSettings.DidNotReceive().SetString(DBMigrator.DatabaseVersionSetting, Arg.Any<string>());
        }
        
        [Test]
        public void Event_WhenMigrationCompletesAndMigrationIsRequired_OnMigrationCompletedEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("1");
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
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "2");
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration2;"));
        }

        [Test]
        public void Event_WhenMigrationCompletesAndNoMigrationRequired_OnMigrationCompletedEventNotRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("2");
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
            _itsSettings.DidNotReceive().SetString(DBMigrator.DatabaseVersionSetting, Arg.Any<string>());
        }

        [Test]
        public void Event_WhenMigratorRunsStep_OnMigrationProgressEventRaised()
        {
            //---------------Set up test pack-------------------
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("1");

            var progressValues = new List<decimal>();
            itsDbMigrator.OnDbMigrationProgress += (s, e) => progressValues.Add(e.PercentageComplete);
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, progressValues.Count());
            //---------------Execute Test ----------------------
            itsDbMigrator.MigrateTo(3);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, progressValues.Count());
            Assert.AreEqual(50, progressValues[0]);
            Assert.AreEqual(100, progressValues[1]);
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "2");
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "3");
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration2;"));
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration3;"));
        }
        
        [Test]
        public void TestGetCurrentVersion() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("2");
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
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
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("2");
            GlobalRegistry.Settings = _itsSettings;            
            Assert.AreEqual(2, itsDbMigrator.CurrentVersion());
            GlobalRegistry.Settings = null;
        }        
        
        [Test]
        public void TestMigrateTo() {
            itsDbMigrator.SetSettingsStorer(_itsSettings);
            _itsSettings.GetString(DBMigrator.DatabaseVersionSetting).Returns("1");
          
            itsDbMigrator.MigrateTo(3);
            
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "2");
            _itsSettings.Received().SetString(DBMigrator.DatabaseVersionSetting, "3");
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration2;"));
            itsConn.Received().ExecuteSql(new SqlStatement(itsConn, "migration3;"));
        }
        
        [Test]
        public void TestGetLatestVersion() {
            Assert.AreEqual(3, itsDbMigrator.LatestVersion());
        }

        [Test]
        public void TestAddSqlStatement() {
            var statement = new SqlStatement(itsConn, "test");
            itsDbMigrator.AddMigration(4, statement);
            Assert.AreEqual(statement, itsDbMigrator.GetMigration(4));
            
        }
    
        
        
    }
}
