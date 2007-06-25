using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.Db
{
    /// <summary>
    /// Manages database migrations that are needed to ensure that working
    /// copies of a project have the up-to-date database structure
    /// </summary>
    /// TODO ERIC - potential confusion over inclusivity of version number
    /// range - see GetMigrationSql() and Migrate()
    public class DbMigrator
    {
        public const string DATABASE_VERSION_SETTING = "DATABASE_VERSION";
        private readonly IDatabaseConnection _connection;
        private SortedDictionary<int, SqlStatement> _migrations;
        private ISettingsStorer _settingsStorer;

        /// <summary>
        /// Constructor to initialise the migrator with the connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        public DbMigrator(IDatabaseConnection connection) {
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
            _migrations.Add(number, new SqlStatement(_connection.GetConnection() , sql));
        }
        
        /// <summary>
        /// Adds a sql migration that can be performed
        /// </summary>
        /// <param name="number">The migration number</param>
        /// <param name="sql">The sql statement object to add</param>
        public void AddMigration(int number, SqlStatement sql) {
            _migrations.Add(number, sql);
        }

        /// <summary>
        /// Returns a set of sql statements between two specified version numbers
        /// </summary>
        /// <param name="startVersion">The start version number (exclusive)</param>
        /// <param name="endVersion">The end version number (inclusive)</param>
        /// <returns>Returns a collection of sql objects</returns>
        /// TODO ERIC - start version should be inclusive (as in "start from
        /// that version", not "after that version").  Also see Migrate()
        /// - or rename the parameter
        public SqlStatementCollection  GetMigrationSql(int startVersion, int endVersion) {
            SqlStatementCollection migrationSql = new SqlStatementCollection();
            foreach (KeyValuePair<int, SqlStatement> migration in _migrations)
            {
                if (migration.Key > startVersion && migration.Key <= endVersion)
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
        /// Performs the migrations from the start version (exclusive) to the
        /// end version (inclusive) specified.  Updates the stored version
        /// number to the end version number specified
        /// </summary>
        /// <param name="startVersion">The start version (exclusive)</param>
        /// <param name="endVersion">The end version (inclusive)</param>
        /// TODO ERIC - rename startversion as currentversion
        public void Migrate(int startVersion, int endVersion) {
            //for (int i = startVersion; i <= endVersion; i++)
            //{ 
            //    _connection.ExecuteSql(GetMigrationSql(i, i));
            //    SetCurrentVersion(endVersion);

            //}
            _connection.ExecuteSql(GetMigrationSql(startVersion, endVersion));
            SetCurrentVersion(endVersion);
        }

        /// <summary>
        /// Sets this instance's settings storer to that specified
        /// </summary>
        /// <param name="storer">The settings storer</param>
        public void SetSettingsStorer(ISettingsStorer storer) {
            _settingsStorer = storer;
        }

        /// <summary>
        /// Sets the current version number to that specified
        /// </summary>
        /// <param name="version">The version number to set to</param>
        public void SetCurrentVersion(int version) {
            _settingsStorer.SetString(DATABASE_VERSION_SETTING, version.ToString( ));
        }

        /// <summary>
        /// Returns the current version number
        /// </summary>
        /// <returns>Returns the version number, or 0 if unsuccessful</returns>
        /// <exception cref="ArgumentNullException">Thrown if the
        /// settings storer has not been assigned</exception>
        public int GetCurrentVersion() {
            if (this._settingsStorer == null && GlobalRegistry.SettingsStorer == null)
            {
                throw new ArgumentNullException("SettingsStorer",
                                                "Please set the setting storer before using GetCurrentVersion as it uses the SettingsStorer to read the current version (VERSION setting)");
            }
            try {
                if (_settingsStorer == null) return Convert.ToInt32(GlobalRegistry.SettingsStorer.GetString(DATABASE_VERSION_SETTING));

                return Convert.ToInt32(_settingsStorer.GetString(DATABASE_VERSION_SETTING));
            } catch (UserException ) {
                return 0;
            }
        }

        /// <summary>
        /// Carries out all migrations from the current version to the version
        /// specified
        /// Note: The DbMigrator only supports forward migrations at the moment.
        /// </summary>
        /// <param name="version">The version number to migrate to (inclusive)</param>
        public void MigrateTo(int version) {

            Migrate(GetCurrentVersion(), version);
        }

        /// <summary>
        /// Returns the most recent migration version number available
        /// </summary>
        /// <returns>Returns an integer</returns>
        public int GetLatestVersion() {
            int latestVersion = 0;
            foreach (KeyValuePair<int, SqlStatement > migration in _migrations) {
                latestVersion = migration.Key;
            }
            return latestVersion;
           
        }
        
        /// <summary>
        /// Performs all migrations available from the current version number
        /// to the most recent version available
        /// </summary>
        public void MigrateToLatestVersion() {
            if (GetCurrentVersion() < GetLatestVersion()) {
                MigrateTo(GetLatestVersion() );
            }
        }

        //public class Migration
        //{
        //    public Migration(Database) {}
        //}
    }
}
