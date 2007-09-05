using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Habanero.Base.Exceptions;
using log4net;
using log4net.Config;

namespace Habanero.Base
{
    /// <summary>
    /// The base class for a HabaneroAppForm and HabaneroAppConsole, two classes to kick start an application built
    /// using the Habanero libraries.
    /// </summary>
    public abstract class HabaneroApp
    {
        private readonly string _appName;
        private readonly string _appVersion;
        private static ILog log;
        private IApplicationVersionUpgrader _applicationVersionUpgrader;
        private string _classDefsPath = "";
        private string _classDefsFileName = "ClassDefs.xml";
        private IExceptionNotifier _exceptionNotifier;
        private ISettings _settings;
        private bool _loadClassDefs = true;

        //private ISynchronisationController _synchronisationController;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroApp(string appName, string appVersion)
        {
            _appName = appName;
            _appVersion = appVersion;
        }

        /// <summary>
        /// Sets the version upgrader, which carries
        /// out upgrades on an installed application to upgrade it to newer
        /// versions.
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
            try {
                SetupExceptionNotifier();
                SetupApplicationNameAndVersion();
                SetupLogging();

                log.Debug("---------------------------------------------------------------------");
                log.Debug(AppName + "v" + AppVersion + " starting");
                log.Debug("---------------------------------------------------------------------");

                SetupDatabaseConnection();
                SetupSettings();
                Upgrade();
                SetupClassDefs();
            }
            catch (Exception ex) {
                string errorMessage = "There was a problem starting the application.";
                if (log != null && log.Logger.IsEnabledFor(log4net.spi.Level.ERROR)) {
                    log.Error("---------------------------------------------" +
                              Environment.NewLine + ExceptionUtilities.GetExceptionString(ex, 0, true));
                    errorMessage += " Please look at the log file for details of the problem.";
                }
                if (GlobalRegistry.UIExceptionNotifier != null) {
                    GlobalRegistry.UIExceptionNotifier.Notify(
                        new UserException(errorMessage, ex),
                        "Problem in Startup:", "Problem in Startup");
                }
                return false;
            }
            return true;
        }

        private static void SetupLogging() {
            try {
                DOMConfigurator.Configure();
            }
            catch (Exception ex) {
                throw new XmlException("There was an error reading the XML configuration file. " +
                                       "Check that all custom configurations, such as DatabaseConfig, are well-formed, " +
                                       "spelt correctly and have been declared correctly in configSections.  See the " +
                                       "Habanero tutorial for example usage or see official " +
                                       "documentation on configuration files if the error is not resolved.", ex);
            }
            log = LogManager.GetLogger("HabaneroApp");
        }

        private void SetupApplicationNameAndVersion() {
            GlobalRegistry.ApplicationName = _appName;
            GlobalRegistry.ApplicationVersion = _appVersion;
        }

        protected abstract void SetupExceptionNotifier();
        protected abstract void SetupDatabaseConnection();
        protected abstract void SetupSettings();
        protected abstract void SetupClassDefs();

        ///// <summary>
        ///// Sets the synchronisation controller, which implements a
        ///// synchronisation strategy for the application.
        ///// </summary>
        //public ISynchronisationController SynchronisationController {
        //    set { _synchronisationController = value; }
        //}

        //private static void SetupSynchronisationController() {
        //    if (_synchronisationController == null) _synchronisationController = new NullSynchronisationController();
        //    GlobalRegistry.SynchronisationController = _synchronisationController;
        //}

        protected virtual void Upgrade()
        {
            if (_applicationVersionUpgrader != null) {
                _applicationVersionUpgrader.Upgrade();
            }
        }

        /// <summary>
        /// Sets the settings storer, which stores application settings such
        /// as those for the database.  This can be set with an
        /// instantiation of DatabaseSettings (the default) or 
        /// ConfigFileSettings, although the later is read-only.
        /// </summary>
        public ISettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Sets the class definitions to the object specified
        /// </summary>
        public bool LoadClassDefs
        {
            get { return _loadClassDefs; }
            set { _loadClassDefs = value; }
        }

        /// <summary>
        /// The name of the application
        /// </summary>
        public string AppName
        {
            get { return _appName; }
        }

        /// <summary>
        /// The version of the application
        /// </summary>
        public string AppVersion
        {
            get { return _appVersion; }
        }

        /// <summary>
        /// Sets the class definition path.  The class definitions specify
        /// the format and limitations of the data.
        /// </summary>
        public string ClassDefsPath
        {
            set { _classDefsPath = value; }
        }

        /// <summary>
        /// Sets the class definition file name.  The class definitions specify
        /// the format and limitations of the data.
        /// </summary>
        public string ClassDefsFileName
        {
            get { return _classDefsFileName; }
            set { _classDefsFileName = value; }
        }

        /// <summary>
        /// Sets the exception notifier, which is used to inform the
        /// user of exceptions encountered.
        /// </summary>
        public IExceptionNotifier ExceptionNotifier
        {
            get { return _exceptionNotifier; }
            set { _exceptionNotifier = value; }
        }
    }
}