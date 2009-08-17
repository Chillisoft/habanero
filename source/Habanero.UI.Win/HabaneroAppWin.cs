//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a template for a standard Habanero application, including
    /// standard fields and initialisations.  Specific details covered are:
    /// <ul>
    /// <li>The class definitions that define how the data is represented
    /// and limited</li>
    /// <li>The database configuration, connection and settings</li>
    /// <li>A logger to record debugging and error messages</li>
    /// <li>An exception notifier to communicate exceptions to the user</li>
    /// <li>Automatic version upgrades when an application is out-of-date</li>
    /// <li>A synchronisation controller</li>
    /// <li>A control factory to create controls</li>
    /// <li>A data accessor that specifies what type of data source is used (DB by default)</li>
    /// </ul>
    /// To set up and launch an application:
    /// <ol>
    /// <li>Instantiate the application with the constructor</li>
    /// <li>Specify any individual settings as required</li>
    /// <li>Call the Startup() method to launch the application</li>
    /// </ol>
    /// </summary>
    public class HabaneroAppWin : HabaneroAppUI
    {
        private DatabaseConfig _databaseConfig;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroAppWin(string appName, string appVersion)
            : base(appName, appVersion)
        {
        }

        /// <summary>
        /// Sets the control factory used to create controls
        /// </summary>
        protected override void SetupControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
        }

        /// <summary>
        /// Initialises the settings.  If not provided, DatabaseSettings
        /// is assumed.
        /// </summary>
        protected override void SetupSettings()
        {
            if (Settings == null) Settings = new DatabaseSettings();
            GlobalRegistry.Settings = Settings;
        }

        /// <summary>
        /// Sets up the database connection.  If not provided, then
        /// reads the connection from the config file.
        /// </summary>
        protected override void SetupDatabaseConnection()
        {
            if (DatabaseConnection.CurrentConnection != null) return;
            if (_databaseConfig == null) _databaseConfig = DatabaseConfig.ReadFromConfigFile();
            string vendor = _databaseConfig.Vendor;
            if (string.IsNullOrEmpty(vendor) || vendor.ToLower().Contains("memory"))
            {
                BORegistry.DataAccessor = new DataAccessorInMemory();
            }
            else
            {
                if (_privateKey != null) _databaseConfig.SetPrivateKey(_privateKey);
                DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();
                BORegistry.DataAccessor = new DataAccessorDB();
            }
        }

        /// <summary>
        /// Sets the database configuration object, which contains basic 
        /// connection information along with the database vendor name 
        /// (eg. MySql, Oracle).
        /// </summary>
        public DatabaseConfig DatabaseConfig
        {
            set { _databaseConfig = value; }
            get { return _databaseConfig; }
        }

        /// <summary>
        /// Sets up the exception notifier used to display
        /// exceptions to the final user.  If not specified,
        /// assumes the FormExceptionNotifier.
        /// </summary>
        protected override void SetupExceptionNotifier()
        {
            if (ExceptionNotifier == null) ExceptionNotifier = new FormExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = ExceptionNotifier;
        }
    }

    // 2009-08-17: Habanero now figures out from the config file if this is inmemory or not
    /////<summary>
    ///// Provides a template for an InMemory Habanero application, including
    ///// standard fields and initialisations.  Specific details covered are:
    ///// <ul>
    ///// <li>The class definitions that define how the data is represented
    ///// and limited</li>
    ///// <li>A logger to record debugging and error messages</li>
    ///// <li>An exception notifier to communicate exceptions to the user</li>
    ///// <li>Automatic version upgrades when an application is out-of-date</li>
    ///// <li>A synchronisation controller</li>
    ///// <li>A control factory to create controls</li>
    ///// <li>A data accessor that specifies what type of data source is used (InMemory)</li>
    ///// </ul>
    ///// To set up and launch an application:
    ///// <ol>
    ///// <li>Instantiate the application with the constructor</li>
    ///// <li>Specify any individual settings as required</li>
    ///// <li>Call the Startup() method to launch the application</li>
    ///// </ol>
    /////</summary>
    //public class HabaneroAppInMemoryWin : HabaneroAppWin
    //{
    //    ///<summary>
    //    /// Creates a windows application that runs using an in memory database. I.e. no database connection is set up
    //    /// and the DataAccessor is set to be InMemory.
    //    ///</summary>
    //    ///<param name="appName"></param>
    //    ///<param name="appVersion"></param>
    //    public HabaneroAppInMemoryWin(string appName, string appVersion) : base(appName, appVersion)
    //    {
    //    }

    //    /// <summary>
    //    /// Sets up the database connection.  If not provided, then
    //    /// reads the connection from the config file.
    //    /// </summary>
    //    protected override void SetupDatabaseConnection()
    //    {
    //        BORegistry.DataAccessor = new DataAccessorInMemory();
    //    }
    //}

    /////<summary>
    ///// Provides a template for an InMemory Habanero application, including
    ///// standard fields and initialisations.  Specific details covered are:
    ///// <ul>
    ///// <li>The class definitions that define how the data is represented
    ///// and limited</li>
    ///// <li>A logger to record debugging and error messages</li>
    ///// <li>An exception notifier to communicate exceptions to the user</li>
    ///// <li>Automatic version upgrades when an application is out-of-date</li>
    ///// <li>A synchronisation controller</li>
    ///// <li>A control factory to create controls</li>
    ///// <li>A data accessor that specifies what type of data source is used (InMemory)</li>
    ///// </ul>
    ///// To set up and launch an application:
    ///// <ol>
    ///// <li>Instantiate the application with the constructor</li>
    ///// <li>Specify any individual settings as required</li>
    ///// <li>Call the Startup() method to launch the application</li>
    ///// </ol>
    /////</summary>
    //public class HabaneroAppDB4OWin : HabaneroAppWin
    //{
    //    private string _vendor;
    //    private string _server;
    //    private string _database;
    //    private string _userName;
    //    private string _password;
    //    private string _port;
    //    private int _portNumber;

    //    ///<summary>
    //    /// Creates a windows application that runs using an in memory database. I.e. no database connection is set up
    //    /// and the DataAccessor is set to be InMemory.
    //    ///</summary>
    //    ///<param name="appName"></param>
    //    ///<param name="appVersion"></param>
    //    public HabaneroAppDB4OWin(string appName, string appVersion) : base(appName, appVersion)
    //    {
    //    }
    //    /// <summary>
    //    /// Sets up the database connection.  If not provided, then
    //    /// reads the connection from the config file.
    //    /// </summary>
    //    protected override void SetupDatabaseConnection()
    //    {
    //        SetupDB4OConfiguration();
    //        DB4ORegistry.CreateDB4OServerConfiguration(_server, _portNumber,_database);
    //    }

    //    private void SetupDB4OConfiguration()
    //    {
    //        IDictionary configuration = (IDictionary) ConfigurationManager.GetSection("DatabaseConfig");
    //        if (configuration != null)
    //        {
    //            _vendor = (string)configuration["vendor"];
    //            _server = (string)configuration["server"];
    //            _database = (string)configuration["database"];
    //            _userName = (string)configuration["username"];
    //            _password = (string)configuration["password"];
    //            _port = (string)configuration["port"];

    //            if (string.IsNullOrEmpty(_vendor) || !_vendor.Equals("DB4O"))
    //            {
    //                throw new ArgumentException("Missing database settings for the database configuration " +
    //                                     "in the application configuration file. " +
    //                                     "Ensure that you have a setting for 'vendor' " +
    //                                     "- see documentation for possible options on the database " +
    //                                     "vendor setting.");
    //            }

    //            if(!Int32.TryParse(_port,out _portNumber))
    //            {
    //                throw new ArgumentException("The value given for 'Port' in the database configuration is an invald Int32 ("+_port+").");
    //            }
    //        }
    //        else
    //        {
    //            throw new ArgumentException("The database configuration could not be read. " +
    //                                     "Check that your application configuration file exists (eg. app.config), " +
    //                                     "that you have DatabaseConfig in the configSections, and that you have " +
    //                                     "a section of settings in the DatabaseConfig category.");
    //        }
    //        return;
    //    }
    //}
}