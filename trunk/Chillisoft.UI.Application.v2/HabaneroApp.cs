using System;
using System.IO;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
//using Chillisoft.UI.Misc.v2;
using Chillisoft.UI.Misc.v2;
using log4net;
using log4net.Config;

namespace Chillisoft.UI.Application.v2
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
    /// </ul>
    /// To set up and launch an application:
    /// <ol>
    /// <li>Instantiate the application with the constructor</li>
    /// <li>Specify any individual settings as required</li>
    /// <li>Call the Startup() method to launch the application</li>
    /// </ol>
    /// </summary>
    public class HabaneroApp
    {
        private readonly string _appName;
        private readonly string _appVersion;
        private static ILog log;
        private ApplicationVersionUpgrader _applicationVersionUpgrader;
        private string _classDefsPath = "";
        private string _classDefsFileName = "ClassDefs.xml";
        private DatabaseConfig _databaseConfig;
        private IExceptionNotifier _exceptionNotifier;
        private SynchronisationController _synchronisationController;
        private ISettingsStorer _settingsStorer;
        private bool _loadClassDefs = true;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroApp(string appName, string appVersion) {
            _appName = appName;
            _appVersion = appVersion;
        }

        /// <summary>
        /// Sets the class definition path.  The class definitions specify
        /// the format and limitations of the data.
        /// </summary>
        public string ClassDefsPath {
            set { _classDefsPath = value; }
        }

        /// <summary>
        /// Sets the class definition file name.  The class definitions specify
        /// the format and limitations of the data.
        /// </summary>
        public string ClassDefsFileName {
            set { _classDefsFileName = value; }
        }

        /// <summary>
        /// Sets the database configuration object, which contains basic 
        /// connection information along with the database vendor name 
        /// (eg. MySQL, Oracle).
        /// </summary>
        public DatabaseConfig DatabaseConfig {
            set { _databaseConfig = value; }
        }

        /// <summary>
        /// Sets the version upgrader, which carries
        /// out upgrades on an installed application to upgrade it to newer
        /// versions.
        /// </summary>
        public ApplicationVersionUpgrader ApplicationVersionUpgrader {
            set { _applicationVersionUpgrader = value; }
        }

        /// <summary>
        /// Sets the exception notifier, which is used to inform the
        /// user of exceptions encountered.
        /// </summary>
        public IExceptionNotifier ExceptionNotifier {
            set { _exceptionNotifier = value; }
        }

        /// <summary>
        /// Sets the synchronisation controller, which implements a
        /// synchronisation strategy for the application.
        /// </summary>
        public SynchronisationController SynchronisationController {
            set { _synchronisationController = value; }
        }

        /// <summary>
        /// Sets the settings storer, which stores database settings
        /// </summary>
        public ISettingsStorer SettingsStorer {
            set { _settingsStorer = value; }
        }

        /// <summary>
        /// Sets the class definitions to the object specified
        /// </summary>
        public bool LoadClassDefs {
            set { _loadClassDefs = value; }
        }

        /// <summary>
        /// Gets the loader for the xml class definitions
        /// </summary>
        /// <returns>Returns the loader</returns>
        private XmlClassDefsLoader GetXmlClassDefsLoader()
        {
            try
            {
                return new XmlClassDefsLoader(new StreamReader(_classDefsFileName).ReadToEnd(), _classDefsPath);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to find Class Definitions file. " +
                      "This file contains all the class definitions that match " +
                      "objects to database tables. Ensure that you have a classdefs.xml file " +
                      "and that the file is being copied to your output directory (eg. bin/debug).", ex);
            }
        }

        /// <summary>
        /// Launches the application, initialising the logger, the database
        /// configuration and connection, the class definitions, the exception
        /// notifier and the synchronisation controller.  This method also
        /// carries out any version upgrades using the 
        /// ApplicationVersionUpgrader, if specified.
        /// </summary>
        /// <returns>Returns true if launched successfully, false if not. A
        /// failed launch will result in error messages being sent to the log
        /// with further information about the failure.</returns>
        public bool Startup() {
            try {
                if (_exceptionNotifier == null) _exceptionNotifier = new FormExceptionNotifier();
                GlobalRegistry.UIExceptionNotifier = _exceptionNotifier;

                if (_synchronisationController == null) _synchronisationController = new NullSynchronisationController();
                GlobalRegistry.SynchronisationController = _synchronisationController;

                GlobalRegistry.ApplicationName = _appName;
                GlobalRegistry.ApplicationVersion = _appVersion;

                try
                {
                    DOMConfigurator.Configure();
                }
                catch (Exception ex)
                {
                    throw new XmlException("There was an error reading the XML configuration file. " +
                        "Check that all custom configurations, such as DatabaseConfig, are well-formed, " +
                        "spelt correctly and have been declared correctly in configSections.  See the " +
                        "Habanero tutorial for example usage or see official " +
                        "documentation on configuration files if the error is not resolved.", ex);
                }
                log = LogManager.GetLogger("HabaneroApp");

                log.Debug("---------------------------------------------------------------------");
                log.Debug(_appName + "v" + _appVersion + " starting");
                log.Debug("---------------------------------------------------------------------");

                if (_databaseConfig == null) _databaseConfig = DatabaseConfig.ReadFromConfigFile();
                DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();

                if (_settingsStorer == null) _settingsStorer = new DatabaseSettingsStorer();
                GlobalRegistry.SettingsStorer = _settingsStorer;

                if (_applicationVersionUpgrader != null) _applicationVersionUpgrader.Upgrade();

                if (_loadClassDefs) ClassDef.LoadClassDefs(GetXmlClassDefsLoader());
            }
            catch (Exception ex) {
                string errorMessage = "There was a problem starting the application.";
                if (log != null && log.Logger.IsEnabledFor(log4net.spi.Level.ERROR))
                {
                    log.Error("---------------------------------------------" +
                        Environment.NewLine + ExceptionUtil.GetCategorizedExceptionString(ex, 0));
                    errorMessage += " Please look at the log file for details of the problem.";
                }
                GlobalRegistry.UIExceptionNotifier.Notify(
                    new UserException(errorMessage, ex),
                    "Problem in Startup:", "Problem in Startup");
                //MessageBox.Show("An error happened on program startup : " + Environment.NewLine + ex.Message + Environment.NewLine + "Please see log file for details.");
                return false;
            }
            return true;
        }
    }
}