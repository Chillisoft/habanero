using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Habanero.Base;
using Habanero.DB;

namespace Habanero.Test.Migrations
{
    public class Migrator
    {
        private DatabaseConfig _databaseConfig;

        private static readonly Dictionary<string, string> DatabaseProcessorMapping =
            new Dictionary<string, string>
            {
                {DatabaseConfig.SqlServer, "SqlServer2008"},
                {DatabaseConfig.MySql, "MySql"},
            };

        public IMigrationProcessorFactory MigrationProcessorFactory { get; private set; }
        public TextWriterAnnouncer Announcer { get; private set; }
        public Assembly AssemblyContainingMigrations { get; private set; }
        public string NamespaceContainingMigrations { get; private set; }


        public Migrator(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
            NamespaceContainingMigrations = "Habanero.Test.Migrations";
            AssemblyContainingMigrations = Assembly.GetExecutingAssembly();
            MigrationProcessorFactory = GetMigrationProcessorFactory(databaseConfig);
            Announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
        }

        private IMigrationProcessorFactory GetMigrationProcessorFactory(IDatabaseConfig databaseConfig)
        {
            string processorName;
            var vendor = databaseConfig.Vendor;
            if (!DatabaseProcessorMapping.TryGetValue(vendor.ToUpper(), out processorName))
            {
                throw new Exception(string.Format("The vendor '{0}' processor type has not been set up for Habanero Test Migrations.", vendor));
            }
            var migrationProcessorFactoryProvider = new MigrationProcessorFactoryProvider();
            return migrationProcessorFactoryProvider.GetFactory(processorName);
        }

        public class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }

        public void MigrateToLatest()
        {
            var runner = CreateMigrationRunner();
            runner.MigrateUp(true);
        }

        private MigrationRunner CreateMigrationRunner()
        {
            
            var processor = GetMigrationProcessor();
            var migrationContext = new RunnerContext(Announcer)
                                   {
                                       Namespace = NamespaceContainingMigrations,
                                       TransactionPerSession = true,
                                   };


            return new MigrationRunner(AssemblyContainingMigrations, migrationContext, processor);
        }

        private IMigrationProcessor GetMigrationProcessor()
        {
            var connectionString = _databaseConfig.GetConnectionString();
            var options = new MigrationOptions {PreviewOnly = false, Timeout = 60};
            return MigrationProcessorFactory.Create(connectionString, Announcer, options);
        }

        public void EnsureDatabseExists()
        {
            if (DatabaseExists())
            {
                if (TableExists("VersionInfo")) return;
                if (TableExists("another_number_generator"))
                {
                    //This is a pre-generated test DB, so drop it and recreate it
                    DropDatabase();
                }
                else
                {
                    var message = string.Format("It looks like your Habanero test DB ('{0}') needs to be regenerated, " +
                                                "please drop it and rerun the tests.", _databaseConfig.Database);
                    throw new Exception(message);
                }
            }
            CreateDatabase();
        }

        private bool TableExists(string tableName)
        {
            var migrationProcessor = GetMigrationProcessor();
            return migrationProcessor.TableExists(null, tableName);
        }

        private void CreateDatabase()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetCreateDatabaseSql();
            databaseConnection.ExecuteRawSql(sql);
        }

        private void DropDatabase()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetDropDatabaseSql();
            databaseConnection.ExecuteRawSql(sql);
        }

        private bool DatabaseExists()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetDatabaseExistsSql();
            var count = Convert.ToInt64(databaseConnection.ExecuteRawSqlScalar(sql));
            return count > 0;
        }

        private string GetDatabaseExistsSql()
        {
            string template;
            switch (_databaseConfig.Vendor.ToUpper())
            {
                case DatabaseConfig.MySql:
                    template = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME='{0}'";
                    break;
                default:
                    template = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.DATABASES WHERE NAME='{0}'";
                    break;
            }
            return string.Format(template, _databaseConfig.Database);
        }

        private string GetCreateDatabaseSql()
        {
            string template;
            switch (_databaseConfig.Vendor.ToUpper())
            {
                case DatabaseConfig.MySql:
                    template = "CREATE SCHEMA `{0}`";
                    break;
                default:
                    template = "CREATE DATABASE [{0}]";
                    break;
            }
            return string.Format(template, _databaseConfig.Database);
        }

        private string GetDropDatabaseSql()
        {
            string template;
            switch (_databaseConfig.Vendor.ToUpper())
            {
                case DatabaseConfig.MySql:
                    template = "DROP SCHEMA `{0}`";
                    break;
                default:
                    template = "DROP DATABASE [{0}]";
                    break;
            }
            return string.Format(template, _databaseConfig.Database);
        }

        private DatabaseConfig GetMasterDatabaseConfig()
        {
            var databaseConfig = _databaseConfig.Clone();
            databaseConfig.Database = "information_schema";
            return databaseConfig;
        }
    }
}
