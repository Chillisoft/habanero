#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.Base.Logging;

namespace Habanero.Base
{
    /// <summary>
    /// The base class for a HabaneroAppUI and HabaneroAppConsole, two classes to kick start an application built
    /// using the Habanero libraries.
    /// </summary>
    public abstract class HabaneroApp
    {
        /// <summary>
        /// The application version upgrader. Override this if you want to perform
        /// upgrade maneuvres on startup.
        /// </summary>
        protected IApplicationVersionUpgrader _applicationVersionUpgrader;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroApp(string appName, string appVersion)
        {
            ClassDefsFileName = "ClassDefs.xml";
            LoadClassDefs = true;
            AppName = appName;
            AppVersion = appVersion;
        }

        /// <summary>
        /// Sets the version upgrader, which carries
        /// out upgrades on an installed application to upgrade it to newer
        /// versions. Typically this will consist of running scripts against a Db to 
        /// add/remove columns or migrate DB to new structure.
        /// </summary>
        public IApplicationVersionUpgrader ApplicationVersionUpgrader
        {
            set { _applicationVersionUpgrader = value; }
        }

        /// <summary>
        /// Launches the application, initialising the logger, the database
        /// configuration and connection, the class definitions, the exception
        /// notifier and the synchronisation controller.  This method also
        /// carries out any version upgrades using the 
        /// IApplicationVersionUpgrader, if specified.
        /// </summary>
        /// <returns>Returns true if launched successfully, false if not. A
        /// failed launch will result in error messages being sent to the log
        /// with further information about the failure.</returns>
        public virtual bool Startup()
        {
            IHabaneroLogger log = null;
            try {
                SetupApplicationNameAndVersion();
                SetupLogging();
                SetupExceptionNotifier();

                log = GlobalRegistry.LoggerFactory.GetLogger("HabaneroApp");
                LogAppStartingInfo(log);

                SetupDatabaseConnection();
                SetupSettings();
                SetupClassDefs();
                Upgrade();
            }
            catch (Exception ex) {
                string errorMessage = "There was a problem starting the application.";
                if (log != null)
                {
                    log.Log("---------------------------------------------" +
                              Environment.NewLine + ExceptionUtilities.GetExceptionString(ex, 0, true), LogCategory.Exception);
                    errorMessage += " Please look at the log file for details of the problem.";
                }
                if (GlobalRegistry.UIExceptionNotifier != null)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(
                        new UserException(errorMessage, ex),
                        "Problem in Startup:", "Problem in Startup");
                }
                return false;
            }
            return true;
        }

        private void LogAppStartingInfo(IHabaneroLogger log)
        {
            log.Log("---------------------------------------------------------------------", LogCategory.Debug);
            log.Log(string.Format("{0} v{1} starting", AppName, AppVersion), LogCategory.Debug);
            log.Log("---------------------------------------------------------------------", LogCategory.Debug);
        }

        /// <summary>
        /// Sets up logging using Log4net. If you want to use your own logging component override this.
        /// </summary>
        /// <exception cref="XmlException"></exception>
        protected virtual void SetupLogging()
        {
            try
            {
                GlobalRegistry.LoggerFactory = new CrossSimpleLoggerFactory();
            }
            catch (Exception ex) {
                throw new XmlException("There was an error reading the XML configuration file. " +
                                       "Check that all custom configurations, such as DatabaseConfig, are well-formed, " +
                                       "spelt correctly and have been declared correctly in configSections.  See the " +
                                       "Habanero tutorial for example usage or see official " +
                                       "documentation on configuration files if the error is not resolved.", ex);
            }
        }


        /// <summary>
        /// Sets the GlobalRegistry ApplicationName and ApplicationVersion from AppName and AppVersion.
        /// </summary>
        protected void SetupApplicationNameAndVersion()
        {
            GlobalRegistry.ApplicationName = AppName;
            GlobalRegistry.ApplicationVersion = AppVersion;
        }

        /// <summary>
        /// Sets up the exception notifier used to display
        /// exceptions to the final user.  If not specified,
        /// assumes the FormExceptionNotifier.
        /// </summary>
        protected abstract void SetupExceptionNotifier();

        /// <summary>
        /// Sets up the database connection.  If not provided, then
        /// reads the connection from the config file.
        /// </summary>
        protected abstract void SetupDatabaseConnection();

        /// <summary>
        /// Initialises the settings.  If not provided, DatabaseSettings
        /// is assumed.
        /// </summary>
        protected abstract void SetupSettings();

        /// <summary>
        /// Loads the class definitions
        /// </summary>
        protected abstract void SetupClassDefs();

        /// <summary>
        /// Upgrades an application's database where an application
        /// upgrader has been provided.  See <see cref="IApplicationVersionUpgrader"/>.
        /// </summary>
        protected virtual void Upgrade()
        {
            if (_applicationVersionUpgrader != null)
            {
                _applicationVersionUpgrader.Upgrade();
            }
        }

        /// <summary>
        /// Gets and sets the settings storer, which stores application settings such
        /// as those for the database.  This can be set with an
        /// instantiation of DatabaseSettings (the default) or 
        /// ConfigFileSettings, although the later is read-only.
        /// </summary>
        public ISettings Settings { get; set; }

        /// <summary>
        /// Gets and sets the value indicating whether to load the class definitions
        /// </summary>
        public bool LoadClassDefs { get; set; }

        /// <summary>
        /// Gets the name of the application
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// Gets the version of the application
        /// </summary>
        public string AppVersion { get; private set; }

        /// <summary>
        /// Gets and sets the class definition file name. See <see cref="IClassDef"/>.
        /// </summary>
        public string ClassDefsFileName { get; set; }

        /// <summary>
        /// Gets and sets the class definition Xml. You can load the xml any way and set it here.
        /// </summary>
        public string ClassDefsXml { get; set; }

        /// <summary>
        /// Gets and sets the exception notifier, which is used to inform the
        /// user of exceptions encountered.
        /// </summary>
        public IExceptionNotifier ExceptionNotifier { get; set; }
    }

}