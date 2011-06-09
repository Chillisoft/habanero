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
using Habanero.Base.Logging;
using Habanero.Util;

namespace Habanero.Base
{
    /// <summary>
    /// Stores key global settings for an application
    /// </summary>
    public class GlobalRegistry
    {
        private static IExceptionNotifier _exceptionNotifier;
        private static IHabaneroLoggerFactory _loggerFactory;
        private static ICrypter _passwordCrypter;
        private static IHasher _passwordHasher;
        private static ISettings _settings;
        private static ISecurityController _securityController;

        /// <summary>
        /// Gets and sets the application's settings storer, which stores settings
        /// This defaults to the <see cref="ConfigFileSettings"/>
        /// </summary>
        public static ISettings Settings
        {
            get { return _settings ?? (_settings = new ConfigFileSettings()); }
            set { _settings = value; }
        }

        /// <summary>
        /// Gets and sets the application's exception notifier, which
        /// provides a means to communicate exceptions to the user
        /// The default is the <see cref="RethrowingExceptionNotifier"/>
        /// </summary>
        public static IExceptionNotifier UIExceptionNotifier
        {
            get { return _exceptionNotifier ?? (_exceptionNotifier = new RethrowingExceptionNotifier()); }
            set { _exceptionNotifier = value; }
        }

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
        public static ISecurityController SecurityController
        {
            get { return _securityController ?? (_securityController = new NullSecurityController()); }
            set { _securityController = value; }
        }

        /// <summary>
        /// Gets and sets the crypter used to encrypt and decrypt short strings. This is typically used for encrypting and decrypting database passwords.
        /// This defaults to the <see cref="NullCrypter"/> which does no encryption and returns the value passed in.
        /// </summary>
        public static ICrypter PasswordCrypter
        {
            get { return _passwordCrypter ?? (_passwordCrypter = new NullCrypter()); }
            set { _passwordCrypter = value; }
        }

        /// <summary>
        /// Gets and sets the hash function used to hash passwords.
        /// This defaults to the <see cref="Utf8Sha1Hasher"/>
        /// </summary>
        public static IHasher PasswordHasher
        {
            get { return _passwordHasher ?? (_passwordHasher = new Utf8Sha1Hasher()); }
            set { _passwordHasher = value; }
        }

        ///<summary>
        /// Gets and sets the <see cref="IHabaneroLoggerFactory"/> that is used to create the <see cref="IHabaneroLogger"/>
        /// for this application.
        /// This defaults to the <see cref="ConsoleLoggerFactory"/>
        ///</summary>
        public static IHabaneroLoggerFactory LoggerFactory
        {
            get { return _loggerFactory ?? (_loggerFactory = new ConsoleLoggerFactory()); }
            set { _loggerFactory = value; }
        }
    }
}
