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
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.DB
{
    /// <summary>
    /// Manages database migrations that are needed to ensure that working
    /// copies of a project have the up-to-date database structure.<br/>
    /// This migrator requires access to an ISettings service that can read and write a
    /// a DATABASE_VERSION setting.  Since ConfigFileSettings does not have write
    /// support, you will either need to use DatabaseSettings or reimplement
    /// a settings operator.  The setting will need to be in existence before this
    /// operation will execute correctly.<br/>
    /// This class is commonly used inside an implementation of
    /// IApplicationVersionUpgrader.  See the tutorials for usage examples.
    /// </summary>
    public class DBMigrator
    {
        public const string DatabaseVersionSetting = "DATABASE_VERSION";
        private readonly IDatabaseConnection _connection;
        private SortedDictionary<int, SqlStatement> _migrations;
        private ISettings _settings;

        /// <summary>
        /// Constructor to initialise the migrator with the connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        public DBMigrator(IDatabaseConnection connection) {
            _connection = connection;
            _migrations = new SortedDictionary<int, SqlStatement>();
        }

        /// <summary>
        /// Returns the migration count
        /// </summary>
        public int MigrationCount { get { return _migrations.Count; } }

        /// <summary>
        /// Adds a sql migration that can be performed
        /// </summary>
        /// <param name="number">The migration number</param>
        /// <param name="sql">The sql statement string to add</param>
        public void AddMigration(int number, string sql) {
            AddMigration(number, new SqlStatement(_connection, sql));
        }
        
        /// <summary>
        /// Adds a sql migration that can be performed
        /// </summary>
        /// <param name="number">The migration number</param>
        /// <param name="sql">The sql statement object to add</param>
        public void AddMigration(int number, SqlStatement sql) {
            if (_migrations.ContainsKey(number))
            {
                throw new HabaneroApplicationException(String.Format(
                    "While processing a database migration, a duplicate migration " +
                    "number '{0}' was encountered. Each number must be unique.",
                    number));
            }
            _migrations.Add(number, sql);
        }

        /// <summary>
        /// Returns a set of sql statements between two specified version numbers,
        /// excluding the start version and including the end version.
        /// </summary>
        /// <param name="startAfterVersion">The start version number (exclusive)</param>
        /// <param name="endVersion">The end version number (inclusive)</param>
        /// <returns>Returns a collection of sql objects</returns>
        public SqlStatementCollection  GetMigrationSql(int startAfterVersion, int endVersion) {
            SqlStatementCollection migrationSql = new SqlStatementCollection();
            foreach (KeyValuePair<int, SqlStatement> migration in _migrations)
            {
                if (migration.Key > startAfterVersion && migration.Key <= endVersion)
                {
                    migrationSql.Add(migration.Value);
                }
            }
            return migrationSql;
            
        }

        /// <summary>
        /// Returns the sql migration statement with the version number specified
        /// </summary>
        /// <param name="number">The version number</param>
        /// <returns>Returns a sql statement object, or null if not found</returns>
        public SqlStatement GetMigration(int number)
        {
            return _migrations[number];
        }

        /// <summary>
        /// Performs the migrations from after the start version up to and
        /// including the end version.  Updates the stored version
        /// number to the end version number specified.
        /// </summary>
        /// <param name="startAfterVersion">The start version (exclusive)</param>
        /// <param name="endVersion">The end version (inclusive)</param>
        public void Migrate(int startAfterVersion, int endVersion) {
            //Each migration should be done separately because changes to DDL does not support rollback.
            for (int i = startAfterVersion + 1; i <= endVersion; i++)
            {
                _connection.ExecuteSql(GetMigrationSql(i - 1, i));
                SetCurrentVersion(i);
            }
            //_connection.ExecuteSql(GetMigrationSql(startAfterVersion, endVersion));
            SetCurrentVersion(endVersion);
        }

        /// <summary>
        /// Sets this instance's settings storer to that specified
        /// </summary>
        /// <param name="storer">The settings storer</param>
        public void SetSettingsStorer(ISettings storer) {
            _settings = storer;
        }

        /// <summary>
        /// Sets the current version number to that specified
        /// </summary>
        /// <param name="version">The version number to set to</param>
        public void SetCurrentVersion(int version) {
            _settings.SetString(DatabaseVersionSetting, version.ToString( ));
        }

        /// <summary>
        /// Returns the current version number
        /// </summary>
        /// <returns>Returns the version number, or 0 if unsuccessful</returns>
        /// <exception cref="ArgumentNullException">Thrown if the
        /// settings storer has not been assigned</exception>
        public int CurrentVersion() {
            if (this._settings == null && GlobalRegistry.Settings == null)
            {
                throw new ArgumentNullException("SettingsStorer",
                                                "Please set the setting storer before using CurrentVersion as it uses the SettingsStorer to read the current version (VERSION setting)");
            }
            try {
                if (_settings == null) return Convert.ToInt32(GlobalRegistry.Settings.GetString(DatabaseVersionSetting));

                return Convert.ToInt32(_settings.GetString(DatabaseVersionSetting));
            } catch (UserException ) {
                return 0;
            }
        }

        /// <summary>
        /// Carries out all migrations from the current version to the version
        /// specified. Note: The DBMigrator currently only supports forward 
        /// migrations.
        /// </summary>
        /// <param name="version">The version number to migrate to (inclusive)</param>
        public void MigrateTo(int version) {

            Migrate(CurrentVersion(), version);
        }

        /// <summary>
        /// Returns the most recent migration version number available
        /// </summary>
        /// <returns>Returns an integer</returns>
        public int LatestVersion() {
            int latestVersion = 0;
            foreach (KeyValuePair<int, SqlStatement > migration in _migrations) {
                latestVersion = migration.Key;
            }
            return latestVersion;
           
        }
        
        /// <summary>
        /// Performs all migrations available from the current version number
        /// to the most recent version available.  This is the common-case
        /// method used to carry out a migration, unless you require more
        /// specific control.
        /// </summary>
        public void MigrateToLatestVersion() {
            if (CurrentVersion() < LatestVersion()) {
                MigrateTo(LatestVersion() );
            }
        }

        //public class Migration
        //{
        //    public Migration(Database) {}
        //}
    }
}
