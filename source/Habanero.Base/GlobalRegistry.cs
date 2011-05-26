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
using log4net;

namespace Habanero.Base
{
    /// <summary>
    /// Stores key global settings for an application
    /// </summary>
    public class GlobalRegistry
    {
        private static IExceptionNotifier _exceptionNotifier;
        //private static ISynchronisationController _synchronisationController;

        /// <summary>
        /// Gets and sets the application's settings storer, which stores
        /// database settings
        /// </summary>
        public static ISettings Settings { get; set; }

        /// <summary>
        /// Gets and sets the application's exception notifier, which
        /// provides a means to communicate exceptions to the user
        /// </summary>
        public static IExceptionNotifier UIExceptionNotifier
        {
            get
            {
                if (_exceptionNotifier == null) return new RethrowingExceptionNotifier();
                return _exceptionNotifier;
            }
            set { _exceptionNotifier = value; }
        }

//        /// <summary>
//        /// //TODO sherwin 29 Oct 2010: 
//        /// </summary>
//        public static ILoggerFacade GetLogger(string contextName)
//        {
//           
//            if (_logger == null) return LogManager.GetLogger(contextName); ;
//            return _logger;
//        }

        ///// <summary>
        ///// Gets and sets the application's synchronisation controller,
        ///// which implements a synchronisation strategy for the application
        ///// </summary>
        //public static ISynchronisationController SynchronisationController
        //{
        //    get
        //    {
        //        if (_synchronisationController == null)
        //        {
        //            _synchronisationController = new NullSynchronisationController();
        //        }
        //        return _synchronisationController;
        //    }
        //    set { _synchronisationController = value; }
        //}

        /// <summary>
        /// Gets and sets the application name
        /// </summary>
        public static string ApplicationName { get; set; }

        /// <summary>
        /// Gets and sets the application version as a string
        /// </summary>
        public static string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets and sets the database version as an integer
        /// </summary>
        public static int DatabaseVersion { get; set; }

        ///<summary>
        /// Gets and sets the security controller for the application
        ///</summary>
        public static ISecurityController SecurityController { get; set; }

        private static IHabaneroLoggerFactory _loggerFactory;

        ///<summary>
        /// Gets and sets the <see cref="IHabaneroLoggerFactory"/> that is used to create the <see cref="IHabaneroLogger"/>
        /// for this application.
        ///</summary>
        public static IHabaneroLoggerFactory LoggerFactory
        {
            get
            {
                if (_loggerFactory == null) return new Log4NetLoggerFactory();
                return _loggerFactory;
            }
            set { _loggerFactory = value; }
        }
    }
}
