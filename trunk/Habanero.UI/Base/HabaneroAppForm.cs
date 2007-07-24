using System;
using System.IO;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.DB;
using Habanero.Base;
//using Habanero.UI.Misc;
using Habanero.UI.Base;
using Habanero.UI.Forms;
using Habanero.Util;
using log4net;
using log4net.Config;

namespace Habanero.UI.Base
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
    public class HabaneroAppForm : HabaneroApp
    {
        private IDefClassFactory _defClassFactory;
        private DatabaseConfig _databaseConfig;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroAppForm(string appName, string appVersion) : base(appName, appVersion) {}



        /// <summary>
        /// Sets the definition class factory.
        /// </summary>
        public IDefClassFactory DefClassFactory
        {
            set { _defClassFactory = value; }
        }

        /// <summary>
        /// Sets the database configuration object, which contains basic 
        /// connection information along with the database vendor name 
        /// (eg. MySql, Oracle).
        /// </summary>
        public DatabaseConfig DatabaseConfig {
            set { _databaseConfig = value; }
        }

        /// <summary>
        /// Gets the loader for the xml class definitions
        /// </summary>
        /// <returns>Returns the loader</returns>
        private XmlClassDefsLoader GetXmlClassDefsLoader()
        {
            try
            {
                if (_defClassFactory != null)
                {
                    return new XmlClassDefsLoader(new StreamReader(ClassDefsFileName).ReadToEnd(), new DtdLoader(), _defClassFactory);
                } else {
                    return new XmlClassDefsLoader(new StreamReader(ClassDefsFileName).ReadToEnd(), new DtdLoader());
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to find Class Definitions file. " +
                                                "This file contains all the class definitions that match " +
                                                "objects to database tables. Ensure that you have a classdefs.xml file " +
                                                "and that the file is being copied to your output directory (eg. bin/debug).", ex);
            }
        }

        protected override void SetupClassDefs()
        {
            if (LoadClassDefs) ClassDef.LoadClassDefs(GetXmlClassDefsLoader());
        }

        protected override void SetupSettings()
        {
            if (Settings == null) Settings = new DatabaseSettings();
            GlobalRegistry.Settings = Settings;
        }

         protected  override void  SetupDatabaseConnection()
                {
            if (_databaseConfig == null) _databaseConfig = DatabaseConfig.ReadFromConfigFile();
            DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();
        }

        protected  override void SetupExceptionNotifier() {
            if (ExceptionNotifier == null) ExceptionNotifier = new FormExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = ExceptionNotifier;
        }
    }
}