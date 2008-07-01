using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
	public class HabaneroAppWin: HabaneroAppUI
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
            SetupControlFactory();

        }

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
            if (_privateKey != null) _databaseConfig.SetPrivateKey(_privateKey);
            DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();
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
}
