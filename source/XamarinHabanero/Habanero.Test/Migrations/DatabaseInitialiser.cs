using System;
using Habanero.DB;

namespace Habanero.Test.Migrations
{
    public class DatabaseInitialiser
    {
        private DatabaseConfig _databaseConfig;

        public DatabaseInitialiser(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public void CreateDatabase()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetCreateDatabaseSql();
            databaseConnection.ExecuteRawSql(sql);
        }

        public void DropDatabase()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetDropDatabaseSql();
            databaseConnection.ExecuteRawSql(sql);
        }

        public bool DatabaseExists()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var databaseConnection = databaseConfig.GetDatabaseConnection();
            var sql = GetDatabaseExistsSql();
            var count = Convert.ToInt64((object) databaseConnection.ExecuteRawSqlScalar(sql));
            return count > 0;
        }

        private string GetDatabaseExistsSql()
        {
            string template;
            switch (_databaseConfig.Vendor.ToUpper())
            {
                case DatabaseConfig.SqlServer:
                    template = "SELECT COUNT(*) FROM sys.databases WHERE name='{0}'";
                    break;
                default:
                    template = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME='{0}'";
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
                case DatabaseConfig.SqlServer:
                    template = "USE MASTER" + Environment.NewLine +
                               "ALTER DATABASE [{0}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE; " + Environment.NewLine +
                               "DROP DATABASE [{0}]";
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
            string masterDatabaseName;
            switch (_databaseConfig.Vendor.ToUpper())
            {
                case DatabaseConfig.MySql:
                    masterDatabaseName = "information_schema";
                    break;
                default:
                    masterDatabaseName = "master";
                    break;
            }
            
            databaseConfig.Database = masterDatabaseName;
            return databaseConfig;
        }
    }
}