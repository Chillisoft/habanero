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
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
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
    public class HabaneroAppVWG : HabaneroAppUI
    {
        private DatabaseConfig _databaseConfig;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroAppVWG(string appName, string appVersion)
            : base(appName, appVersion)
        {
        }

        /// <summary>
        /// Sets the control factory used to create controls
        /// </summary>
        protected override void SetupControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryVWG();
        }

        /// <summary>
        /// Initialises the settings.  If not provided, DatabaseSettings
        /// is assumed.
        /// </summary>
        protected override void SetupSettings()
        {
            if (Settings == null) Settings = new DatabaseSettings(DatabaseConnection.CurrentConnection);
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
    //public class HabaneroAppInMemoryVWG : HabaneroAppVWG
    //{
    //    ///<summary>
    //    /// Creates a windows application that runs using an in memory database. I.e. no database connection is set up
    //    /// and the DataAccessor is set to be InMemory.
    //    ///</summary>
    //    ///<param name="appName"></param>
    //    ///<param name="appVersion"></param>
    //    public HabaneroAppInMemoryVWG(string appName, string appVersion)
    //        : base(appName, appVersion)
    //    {
    //    }
    //    /// <summary>
    //    /// Sets up the dataaccessor to be an in Memory  DataAccessor
    //    /// </summary>
    //    protected override void SetupDatabaseConnection()
    //    {
    //        BORegistry.DataAccessor = new DataAccessorInMemory();
    //    }
    //}
}
