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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using Habanero.Base;

namespace Habanero.DB
{
	/// <summary>
    /// Stores database configuration settings and creates connections
    /// using these settings
    /// </summary>
    public partial class DatabaseConfig : IDatabaseConfig
    {
        public const string MySql = "MYSQL";
        public const string SqlServer = "SQLSERVER";
        public const string Oracle = "ORACLE";
		public const string Access = "ACCESS";
        public const string PostgreSql = "POSTGRESQL";
        public const string SQLite = "SQLITE";

        private String _vendor;
        private String _server;
        private String _database;
        private String _userName;
        private String _password;
        private String _port;
        private ICrypter _passwordCrypter = new NullCrypter();

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
        /// provided (eg. DatabaseConfig.MySql)</param>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        public DatabaseConfig(string vendor, string server, string database, string userName, string password,
                              string port)
        {
            this._vendor = vendor;
            this._server = server;
            this._database = database;
            this._userName = userName;
            this._password = password;
            this._port = port;
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
            if (settings != null)
            {
                _vendor = (string) settings["vendor"];
                _server = (string) settings["server"];
                _database = (string) settings["database"];
                _userName = (string) settings["username"];
                _password = (string) settings["password"];
                _port = (string) settings["port"];

                if (_vendor == null || _vendor.Length == 0)
                {
                    throw new ArgumentException("Missing database settings for the database configuration " +
                                         "in the application configuration file. " +
                                         "Ensure that you have a setting for 'vendor' " +
                                         "- see documentation for possible options on the database " +
                                         "vendor setting.");
                }
            }
            else
            {
                throw new ArgumentException("The database configuration could not be read. " +
                                         "Check that your application configuration file exists (eg. app.config), " +
                                         "that you have DatabaseConfig in the configSections, and that you have " +
                                         "a section of settings in the DatabaseConfig category.");
            }
        }

        /// <summary>
        /// Gets and sets access to the database vendor setting
        /// </summary>
        public virtual string Vendor
        {
            get { return _vendor; }
            set { _vendor = value; }
        }

        /// <summary>
        /// Gets and sets access to the database server setting
        /// </summary>
        public virtual string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        /// <summary>
        /// Gets and sets access to the database name setting
        /// </summary>
        public virtual string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        /// <summary>
        /// Gets and sets access to the username setting
        /// </summary>
        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Gets and sets access to the password setting
        /// </summary>
        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets and sets access to the port setting
        /// </summary>
        public virtual string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        internal string DecryptedPassword
        {
            get { return _passwordCrypter.DecryptString(Password);  }
        }

        /// <summary>
        /// Sets the private key to use to decrypt the password.  The private key is in xml format.   
        /// </summary>
        /// <param name="xmlPrivateKey">The xml format of the RSA key (RSA.ToXmlString(true))</param>
        public void SetPrivateKey(string xmlPrivateKey)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(xmlPrivateKey);
            SetPrivateKey(rsa);

        }

        /// <summary>
        /// Sets the private key to use to decrypt password. The private key is an RSA object.
        /// </summary>
        /// <param name="privateKey">The RSA object which has the private key</param>
        public void SetPrivateKey(RSA privateKey)
        {
            _passwordCrypter = new RSAPasswordCrypter(privateKey);
        }

        /// <summary>
        /// Creates a new configuration object by reading the "DatabaseConfig"
        /// settings from the project's configuration settings
        /// </summary>
        /// <returns>Returns a DatabaseConfig object</returns>
        public static DatabaseConfig ReadFromConfigFile()
        {
            return ReadFromConfigFile("DatabaseConfig");
        }
                
	    ///<summary>
        /// Creates a new configuration object by reading the "DatabaseConfig"
        /// settings from the project's configuration settings
	    ///</summary>
	    ///<param name="configSectionName">The name of the Config Setting Section where the database connection settings are stored.</param>
        ///<returns>Returns a DatabaseConfig object</returns>
	    public static DatabaseConfig ReadFromConfigFile(string configSectionName)
	    {
	        return new DatabaseConfig((IDictionary) ConfigurationSettings.GetConfig(configSectionName));
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
            return factory.GetConnectionString(this.Server, this.Database, this.UserName, this.DecryptedPassword, this.Port);
        }

        /// <summary>
        /// Returns a connection string tailored for the database vendor
        /// </summary>
        /// <returns>Returns a connection string</returns>
        public String GetConnectionString()
        {
            ConnectionStringFactory factory = ConnectionStringFactory.GetFactory(this.Vendor);
            return factory.GetConnectionString(this.Server, this.Database, this.UserName, this.DecryptedPassword, this.Port);
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        public IDbConnection GetConnection()
        {
            return DatabaseConnectionFactory.CreateConnection(this).GetConnection();
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        public IDatabaseConnection GetDatabaseConnection()
        {
            return DatabaseConnectionFactory.CreateConnection(this);
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
            return DatabaseConnectionFactory.CreateConnection(this, assemblyName, fullClassName).GetConnection();
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