// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.DB
{
    /// <summary>
    /// Stores database configuration settings and creates connections
    /// using these settings
    /// </summary>
    public class DatabaseConfig : IDatabaseConfig
    {
        /// <summary>
        /// MySql - the MySql .NET data provider will be used
        /// </summary>
        public const string MySql = "MYSQL";

        /// <summary>
        /// DB4O - the DB4O .NET data provider will be used
        /// </summary>
        public const string DB4O = "DB4O";

        /// <summary>
        /// Microsoft Sql Server - the built in SqlClient data provider will be used
        /// </summary>
        public const string SqlServer = "SQLSERVER";

        /// <summary>
        /// Microsoft Sql Server Compact Edition - the built in SqlClient data provider will be used
        /// </summary>
        public const string SqlServerCe = "SQLSERVERCE";

        /// <summary>
        /// Oracle - the built in Oracle data provider will be used
        /// </summary>
        public const string Oracle = "ORACLE";

        /// <summary>
        /// Access - the built in OleDb data provider will be used
        /// </summary>
        public const string Access = "ACCESS";

        /// <summary>
        /// PostGreSQL - the PostGreSQL data provider will be used
        /// </summary>
        public const string PostgreSql = "POSTGRESQL";

        /// <summary>
        /// SQLite - the SQLite data provider will be used
        /// </summary>
        public const string SQLite = "SQLITE";

        /// <summary>
        /// Firebird - the Firebird data provider will be used
        /// </summary>
        public const string Firebird = "FIREBIRD";

        /// <summary>
        /// Firebird embedded - the Firebird embedded data provider will be used
        /// </summary>
        public const string FirebirdEmbedded = "FIREBIRDEMBEDDED";

        /// <summary>
        /// MSAccess 2007 - accdb format
        /// </summary>
        public const string Access2007 = "ACCESS2007";

        /// <summary>
        /// We need to map the database vendor name to the right <see cref="ConnectionStringFactory"/> type.
        /// </summary>
        private static readonly Dictionary<string, string>
            VendorToConnectionStringFactoryNameMap =
                new Dictionary<string, string>
                    {
                        {MySql, "MySql"},
                        {DB4O, "DB4O"},
                        {SqlServer, "SqlServer"},
                        {SqlServerCe, "SqlServerCe"},
                        {Oracle, "Oracle"},
                        {Access, "Access"},
                        {PostgreSql, "PostgreSql"},
                        {SQLite, "SQLite"},
                        {Firebird, "Firebird"},
                        {FirebirdEmbedded, "FirebirdEmbedded"},
                        {Access2007, "Access2007"}
                    };

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

                if (string.IsNullOrEmpty(_vendor))
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

        /// <summary>
        /// The name of the Assembly to use - the assembly that contains the IDbConnection class for this database type.
        /// This does not need to be specified normally, but if you want to use a custom data provider you will need to 
        /// set this property before using the <see cref="IDatabaseConnectionFactory"/> to create the <see cref="IDatabaseConnection"/>.
        /// This must be the full name of the assembly if you are to be sure to get the right assembly.  Alternately if the dll is
        /// placed in the same folder as the application you can just specify the name of the file (without the .dll extension).
        /// </summary>
        public virtual string AssemblyName { get; set; }

        /// <summary>
        /// The fully qualified name of the type to use when creating the IDbConnection.
        /// This does not need to be specified normally, but if you want to use a custom data provider you will need to 
        /// set this property before using the <see cref="IDatabaseConnectionFactory"/> to create the <see cref="IDatabaseConnection"/>.
        /// This class must exist withing the assembly specified in the <see cref="AssemblyName"/> property, and be fully qualified
        /// i.e. it must include the namespace.
        /// </summary>
        public virtual string FullClassName { get; set; }

        /// <summary>
        /// The full assembly name of the assembly containing <see cref="ConnectionStringFactory"/> to use.
        /// This does not need to be specified if you are using one of the standard Habanero database types.
        /// </summary>
        public virtual string ConnectionStringFactoryAssemblyName { get; set; }

        /// <summary>
        /// The fully qualified class name of the <see cref="ConnectionStringFactory"/> to use.
        /// This does not need to be specified if you are using one of the standard Habanero database types.
        /// </summary>
        public virtual string ConnectionStringFactoryClassName { get; set; }

        /// <summary>
        /// Returns the decrypted password.  This will be the same as <see cref="Password"/> if the private key has not been
        /// set (via <see cref="SetPrivateKey(string)"/> or <see cref="SetPrivateKey(System.Security.Cryptography.RSA)"/>.
        /// </summary>
        public string DecryptedPassword
        {
            get { return _passwordCrypter.DecryptString(Password); }
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
            return new DatabaseConfig((IDictionary) ConfigurationManager.GetSection(configSectionName));
        }

        /// <summary>
        /// Returns a connection string tailored for the database vendor
        /// </summary>
        /// <returns>Returns a connection string</returns>
        public String GetConnectionString()
        {
            string factoryClassName = this.ConnectionStringFactoryClassName;
            string factoryAssembly = this.ConnectionStringFactoryAssemblyName;
            if (String.IsNullOrEmpty(factoryClassName))
                factoryClassName = "ConnectionString" + VendorToConnectionStringFactoryNameMap[this.Vendor.ToUpper()] +
                                   "Factory";
            if (String.IsNullOrEmpty(factoryAssembly)) factoryAssembly = "Habanero.DB";

            Type factoryType = TypeLoader.LoadType(factoryAssembly, factoryClassName);
            ConnectionStringFactory factory = (ConnectionStringFactory) Activator.CreateInstance(factoryType);
            return factory.GetConnectionString(this.Server, this.Database, this.UserName, this.DecryptedPassword,
                                               this.Port);
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        public IDbConnection GetConnection()
        {
            return new DatabaseConnectionFactory().CreateConnection(this).GetConnection();
        }

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        public IDatabaseConnection GetDatabaseConnection()
        {
            return new DatabaseConnectionFactory().CreateConnection(this);
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
            return base.Equals(obj);
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
        /// <summary>
        /// Returns true or false for whether this DatabaseConfig points at an In Memory DB or not.
        /// </summary>
        public bool IsInMemoryDB
        {
            get
            {
                var vendor = this.Vendor;
                return string.IsNullOrEmpty(vendor) || vendor.ToLower().Contains("memory");
            }
        }
    }
}