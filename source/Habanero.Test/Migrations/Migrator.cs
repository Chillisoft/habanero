using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Versioning;
using FluentMigrator.VersionTableInfo;
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

        private readonly DatabaseInitialiser _databaseInitialiser;

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
            _databaseInitialiser = new DatabaseInitialiser(databaseConfig);
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
            var options = new Migrator.MigrationOptions {PreviewOnly = false, Timeout = 60};
            return MigrationProcessorFactory.Create(connectionString, Announcer, options);
        }

        public void EnsureDatabseExists()
        {
            if (_databaseInitialiser.DatabaseExists())
            {
                if (TableExists("VersionInfo")) return;
                if (TableExists("mybo"))
                {
                    //This is a pre-generated test DB, so drop it and recreate it
                    _databaseInitialiser.DropDatabase();
                }
                else
                {
                    var message = string.Format("It looks like your Habanero test DB ('{0}') needs to be regenerated, " +
                                                "please drop it and rerun the tests.", _databaseConfig.Database);
                    throw new Exception(message);
                }
            }
            _databaseInitialiser.CreateDatabase();
        }

        private bool TableExists(string tableName)
        {
            var migrationProcessor = GetMigrationProcessor();
            return migrationProcessor.TableExists(null, tableName);
        }

        public void DirectMigrateUp(IMigration migration)
        {
            var runner = CreateMigrationRunner();
            runner.VersionLoader = new NullVersionLoader();
            runner.ApplyMigrationUp(new MigrationInfo(0, TransactionBehavior.Default, migration), true);
        }

        public class NullVersionLoader : IVersionLoader
        {
            public NullVersionLoader()
            {
                VersionInfo = new VersionInfo();
            }

            public void DeleteVersion(long version)
            {
                
            }

            public IVersionTableMetaData GetVersionTableMetaData()
            {
                return new DefaultVersionTableMetaData();
            }

            public void LoadVersionInfo()
            {
            }

            public void RemoveVersionTable()
            {
            }

            public void UpdateVersionInfo(long version)
            {
            }

            public void UpdateVersionInfo(long version, string description)
            {
            }

            public bool AlreadyCreatedVersionSchema { get; private set; }
            public bool AlreadyCreatedVersionTable { get; private set; }
            public IMigrationRunner Runner { get; set; }
            public IVersionInfo VersionInfo { get; set; }
            public IVersionTableMetaData VersionTableMetaData { get; private set; }
        }
    }

    
}
