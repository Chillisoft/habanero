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
using System.Collections.Generic;
using System.Linq;
using Habanero.Base.Exceptions;

namespace Habanero.Base.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleLoggerFactory : IHabaneroLoggerFactory
    {
        readonly IDictionary<string, IHabaneroLogger> _loggers = new Dictionary<string, IHabaneroLogger>();
        private IDictionary<LogCategory, bool> _isLogging = new Dictionary<LogCategory, bool>();

        /// <summary>
        /// Constructor, by default only Warn, Exception and Fatal levels are enabled.
        /// </summary>
        public ConsoleLoggerFactory()
        {
            _isLogging.Add(LogCategory.Debug, false);
            _isLogging.Add(LogCategory.Info, false);
            _isLogging.Add(LogCategory.Warn, true);
            _isLogging.Add(LogCategory.Exception, true);
            _isLogging.Add(LogCategory.Fatal, true);
        }

        /// <summary>
        /// Get the logger for the given context.
        /// </summary>
        /// <param name="contextName">The context, to allow for various lgger names</param>
        /// <returns>A logger with the given context name</returns>
        public IHabaneroLogger GetLogger(string contextName)
        {
            if (_loggers.ContainsKey(contextName)) return _loggers[contextName];
            var consoleLogger = new ConsoleLogger(contextName,  new Dictionary<LogCategory, bool>(_isLogging));
            _loggers.Add(contextName, consoleLogger);
            return consoleLogger;
        }

        /// <summary>
        /// Returns a logger set up with the context of the type given
        /// </summary>
        /// <param name="type">The type the logger is for</param>
        /// <returns>A logger configured with the type as its context</returns>
        public IHabaneroLogger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        /// <summary>
        /// Sets up the default isLogging value for the LogCategory.
        /// </summary>
        /// <param name="logCategory">The <see cref="LogCategory"/> to configure</param>
        /// <param name="isLogging">Whether to log that LogCategory by default</param>
        public void SetDefaultIsLogging(LogCategory logCategory, bool isLogging)
        {
            _isLogging[logCategory] = isLogging;
        }
    }

    /// <summary>
    /// A logger that just logs to the console
    /// </summary>
    public class ConsoleLogger : IHabaneroLogger
    {
        private readonly string _contextName;
        private readonly IDictionary<LogCategory, bool> _isLogging;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextName">The name of the logger</param>
        /// <param name="isLogging">Whether each <see cref="LogCategory"/> should be logged</param>
        public ConsoleLogger(string contextName, IDictionary<LogCategory, bool> isLogging)
        {
            _contextName = contextName;
            _isLogging = isLogging;
        }

        /// <summary>
        /// The name of the logger
        /// </summary>
        public string ContextName
        {
            get { return _contextName; }
        }

        /// <summary>
        /// Creates a single log entry for with the message with the appropriate message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logCategory"></param>
        public void Log(string message, LogCategory logCategory)
        {
            if (!IsLogging(logCategory)) return;
            Console.Out.WriteLine("{0} {1} {2} {3} {4}", DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToShortTimeString(), _contextName,
                                  Enum.GetName(typeof (LogCategory), logCategory), message);
        }

        /// <summary>
        /// Creates a single log entry for with the appropriate exception message.
        /// Although this is an interface and you can implement it as you wish for <see cref="Log4NetLogger"/>
        /// We log an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
        /// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
        /// </summary>
        /// <param name="exception"></param>
        public void Log(Exception exception)
        {
            if (!IsLogging(LogCategory.Exception)) return;
            Log(exception.GetType().Name + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace, LogCategory.Exception);
        }

        /// <summary>
        /// Creates a single log entry for with the appropriate exception message and message.
        /// Although this is an interface and you can implement it as you wish for <see cref="Log4NetLogger"/>
        /// We log an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
        /// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
        /// </summary>
        /// <param name="message">The additional log message to be logged with the exception</param>
        /// <param name="exception"></param>
        public void Log(string message, Exception exception)
        {
            if (!IsLogging(LogCategory.Exception)) return;
            Log(message + Environment.NewLine + 
                exception.GetType().Name + Environment.NewLine + 
                exception.Message + Environment.NewLine + 
                exception.StackTrace, LogCategory.Exception);
        }

        /// <summary>
        /// Creates a single log entry for with appropriate exception message and message with the specified LogCategory.
        /// </summary>
        /// <param name="message">The additional log message to be logged with the exception</param>
        /// <param name="exception">The exception being logged</param>
        /// <param name="logCategory">The specified LogCategory</param>
        public void Log(string message, Exception exception, LogCategory logCategory)
        {
            if (!IsLogging(logCategory)) return;
            Log(message + Environment.NewLine +
                 exception.GetType().Name + Environment.NewLine +
                 exception.Message + Environment.NewLine +
                 exception.StackTrace, logCategory);
        }

        ///<summary>
        /// Checks the logger to see if it has been enabled to log messages of the specified <see cref="LogCategory"/> type.
        ///</summary>
        ///<param name="logCategory">The <see cref="LogCategory"/> for which to check if logging is enabled or not.</param>
        ///<returns>true if the specified <see cref="LogCategory"/> messages will be logged, otherwise false.</returns>
        public bool IsLogging(LogCategory logCategory)
        {
            return _isLogging[logCategory];
        }

        /// <summary>
        /// Set up the <see cref="LogCategory"/> for logging
        /// </summary>
        /// <param name="logCategory"></param>
        /// <param name="isLogging"></param>
        public void SetIsLogging(LogCategory logCategory, bool isLogging)
        {
            _isLogging[logCategory] = isLogging;
        }
    }
}