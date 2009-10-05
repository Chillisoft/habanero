// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.IO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

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
    public abstract class HabaneroAppUI : HabaneroApp
    {
        /// <summary>
        /// The application's private key
        /// </summary>
        protected string _privateKey;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        protected HabaneroAppUI(string appName, string appVersion)
            : base(appName, appVersion)
        {
            SetupHabaneroAppUI();
        }

        private void SetupHabaneroAppUI()
        {
            SetupUISettings();
            SetupDateDisplaySettings();
            SetupControlFactory();
        }

        /// <summary>
        /// Sets the control factory used to create controls
        /// </summary>
        protected abstract void SetupControlFactory();

        /// <summary>
        /// Sets the private key used to decrypt the database password. If your database password as supplied is
        /// in plaintext then this is not necessary. If you supply the DatabaseConfig object you can also set the
        /// private key on that instead.
        /// </summary>
        /// <param name="xmlPrivateKey">The private key (RSA) in xml format</param>
        public void SetPrivateKey(string xmlPrivateKey)
        {
            _privateKey = xmlPrivateKey;
        }

        /// <summary>
        /// Gets the loader for the xml class definitions
        /// </summary>
        /// <returns>Returns the loader</returns>
        private XmlClassDefsLoader GetXmlClassDefsLoader()
        {
            try
            {
                string classDefsXml;
                if (String.IsNullOrEmpty(ClassDefsXml))
                    classDefsXml = new StreamReader(ClassDefsFileName).ReadToEnd();
                else
                    classDefsXml = ClassDefsXml;

                return new XmlClassDefsLoader(classDefsXml, new DtdLoader(), new DefClassFactory());
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to find Class Definitions file. " +
                                                "This file contains all the class definitions that match " +
                                                "objects to database tables. Ensure that you have a classdefs.xml file " +
                                                "and that the file is being copied to your output directory (eg. bin/debug).",
                                                ex);
            }
        }

        /// <summary>
        /// Loads the class definitions
        /// </summary>
        protected override void SetupClassDefs()
        {
            if (LoadClassDefs) ClassDef.ClassDefs.Add(GetXmlClassDefsLoader().LoadClassDefs());
        }

        /// <summary>
        /// Sets up the class that stores the user interface
        /// settings
        /// </summary>
        protected virtual void SetupUISettings()
        {
            GlobalUIRegistry.UISettings = new UISettings();
        }

        /// <summary>
        /// Sets up the class that stores the date display
        /// settings
        /// </summary>
        protected virtual void SetupDateDisplaySettings()
        {
            GlobalUIRegistry.DateDisplaySettings = new DateDisplaySettings();
        }
    }
}