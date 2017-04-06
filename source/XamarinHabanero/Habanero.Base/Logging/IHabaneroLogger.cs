﻿#region Licensing Header
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
using Habanero.Base.Exceptions;

namespace Habanero.Base.Logging
{
    /// <summary>
    /// This is an interface for a logger adapter.
    /// It is expected that you can create a specific Adapter 
    /// for the Logger that you use. 
    /// In Habanero we use the <see cref="CrossSimpleLogger"/> by default.
    /// You can change this in the BootStrapping of your application by registering the appropriate
    /// Logger Factor see <see cref="IHabaneroLoggerFactory"/> for more details
    /// </summary>
    public interface IHabaneroLogger
    {
        ///<summary>
        /// The Context of the Log message that is being logged.
        /// This is typically the FullClassName e.g. namespace.ClassName
        /// but can be anything else.
        ///</summary>
        string ContextName { get; }

        /// <summary>
        /// Creates a single log entry for with the message with the appropriate message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logCategory"></param>
        void Log(string message, LogCategory logCategory);

        /// <summary>
        /// Creates a single log entry for with the appropriate exception message.
        /// Although this is an interface and you can implement it as you wish for <see cref="CrossSimpleLogger"/>
        /// We log an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
        /// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
        /// </summary>
        /// <param name="exception"></param>
        void Log(Exception exception);

        /// <summary>
        /// Creates a single log entry for with the appropriate exception message and message.
        /// Although this is an interface and you can implement it as you wish for <see cref="CrossSimpleLogger"/>
        /// We log an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
        /// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
        /// </summary>
        /// <param name="message">The additional log message to be logged with the exception</param>
        /// <param name="exception"></param>
        void Log(string message, Exception exception);

        /// <summary>
        /// Creates a single log entry for with appropriate exception message and message with the specified LogCategory.
        /// </summary>
        /// <param name="message">The additional log message to be logged with the exception</param>
        /// <param name="exception">The exception being logged</param>
        /// <param name="logCategory">The specified LogCategory</param>
        void Log(string message, Exception exception, LogCategory logCategory);

    	///<summary>
    	/// Checks the logger to see if it has been enabled to log messages of the specified <see cref="LogCategory"/> type.
    	///</summary>
    	///<param name="logCategory">The <see cref="LogCategory"/> for which to check if logging is enabled or not.</param>
    	///<returns>true if the specified <see cref="LogCategory"/> messages will be logged, otherwise false.</returns>
    	bool IsLogging(LogCategory logCategory);
    }
}