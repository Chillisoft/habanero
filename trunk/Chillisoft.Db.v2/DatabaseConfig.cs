using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Chillisoft.Generic.v2;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Stores database configuration settings and creates connections
    /// using these settings
    /// </summary>
    public class DatabaseConfig : IDatabaseConfig
    {
        public const string MySQL = "MYSQL";
        public const string SQLServer = "SQLSERVER";
        public const string Oracle = "ORACLE";
        public const string Access = "ACCESS";

        protected String mVendor;
        protected String mServer;
        protected String mDatabase;
        protected String mUserName;
        protected String mPassword;
        protected String mPort;

        /// <summary>
        /// A deparameterised constructor
        /// </summary>
        internal DatabaseConfig()
        {
        }

        /// <summary>
        /// A constructor with specific configurations provided
        /// </summary>
        /// <param name="vendor">The database vendor - use the strings
        /// provided (eg. DatabaseConfig.MySQL)</param>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        public DatabaseConfig(string vendor, string server, string database, string userName, string password,
                              string port)
        {
            this.mVendor = vendor;
            this.mServer = server;
            this.mDatabase = database;
            this.mUserName = userName;
            this.mPassword = password;
            this.mPort = port;
        }

        /// <summary>
        /// A constructor as before, but with the configuration settings
        /// passed as an IDictionary object
        /// </summary>
        /// <param name="settings">An IDictionary object containing entries
        /// for "vendor", "server", "database", "username", "password" and
        /// "port"</param>
        public DatabaseConfig(IDictionary settings)
        {
            mVendor = (string) settings["vendor"];
            mServer = (string) settings["server"];
            mDatabase = (string) settings["database"];
            mUserName = (string) settings["username"];
            mPassword = (string) settings["password"];
            mPort = (string) settings["port"];
        }

        /// <summary>
        /// Gets and sets access to the database vendor setting
        /// </summary>
        public virtual string Vendor
        {
            get { return mVendor; }
            set { mVendor = value; }
        }

        /// <summary>
        /// Gets and sets access to the database server setting
        /// </summary>
        public virtual string Server
        {
            get { return mServer; }
            set { mServer = value; }
        }

        /// <summary>
        /// Gets and sets access to the database name setting
        /// </summary>
        public virtual string Database
        {
            get { return mDatabase; }
            set { mDatabase = value; }
        }

        /// <summary>
        /// Gets and sets access to the username setting
        /// </summary>
        public virtual string UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }

        /// <summary>
        /// Gets and sets access to the password setting
        /// </summary>
        public virtual string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        /// <summary>
        /// Gets and sets access to the port setting
        /// </summary>
        public virtual string Port
        {
            get { return mPort; }
            set { mPort = value; }
        }

        /// <summary>
        /// Creates a new configuration object by reading the "DatabaseConfig"
        /// settings from the project's configuration settings
        /// </summary>
        /// <returns>Returns a DatabaseConfig object</returns>
        public static DatabaseConfig ReadFromConfigFile()
        {
            return new DatabaseConfig((IDictionary) ConfigurationSettings.GetConfig("DatabaseConfig"));
        }

        /// <summary>
        /// Returns a connection string tailored for the database vendor,
        /// after appending an alternate assembly name.  Rather use 
        /// GetConnectionString() if no alternate assembly name is needed.
        /// </summary>
        /// <param name="alternateAssemblyName">The alternate assembly name</param>
        /// <returns>Returns a connection string</returns>
        public String GetConnectionString(string alternateAssemblyName)
        {
            ConnectionStringFactory factory =
                ConnectionStringFactory.GetFactory(this.Vendor + "_" + alternateAssemblyName);
            return factory.GetConnectionString(this.Server, this.Database, this.UserName, this.Password, this.Port);
        }

        /// <summary>
        /// Returns a connection string tailored for the database vendor
        /// </summary>
        /// <returns>Returns a connection string</returns>
        public String GetConnectionString()
        {
            ConnectionStringFactory factory = ConnectionStringFactory.GetFactory(this.Vendor);
            return factory.GetConnectionString(this.Server, this.Database, this.UserName, this.Password, this.Port);
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        public IDbConnection GetConnection()
        {
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            return factory.CreateConnection(this).GetConnection();
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        public IDatabaseConnection GetDatabaseConnection()
        {
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            return factory.CreateConnection(this);
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored, along with the assembly name and full
        /// class name provided
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="fullClassName">The full class name</param>
        /// <returns>Returns an IDbConnection object</returns>
        public IDbConnection GetConnection(string assemblyName, string fullClassName)
        {
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            return factory.CreateConnection(this, assemblyName, fullClassName).GetConnection();
        }

        /// <summary>
        /// Checks whether this database configuration is equal to that of
        /// the object provided
        /// </summary>
        /// <param name="obj">The object to compare with, which must be a
        /// type of DatabaseConfig</param>
        /// <returns>Returns true if all configurations are the same</returns>
        public override bool Equals(object obj)
        {
            if (obj is DatabaseConfig)
            {
                DatabaseConfig conf = (DatabaseConfig) obj;
                return (conf.Vendor == this.Vendor &&
                        conf.Server == this.Server &&
                        conf.Database == this.Database &&
                        conf.UserName == this.UserName &&
                        conf.Password == this.Password &&
                        conf.Port == this.Port);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        /// <summary>
        /// Returns the hashcode of all the settings added together
        /// </summary>
        /// <returns>Returns the hashcode</returns>
        public override int GetHashCode()
        {
            return (this.Vendor + this.Server + this.Database + this.UserName + this.Password + this.Port).GetHashCode();
        }

        /// <summary>
        /// Returns a string with all the settings listed
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return
                "Vendor:" + this.Vendor + ";Server:" + this.Server + ";Database:" + this.Database + ";UserName:" +
                this.UserName + ";Password:" + this.Password + ";Port:" + this.Port;
        }
    }
}