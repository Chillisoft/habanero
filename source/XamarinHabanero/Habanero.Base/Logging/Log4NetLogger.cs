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
using Habanero.Base.Exceptions;
using log4net;

namespace Habanero.Base.Logging
{
	///<summary>
	/// The Log4Net adapter.
	///</summary>
	public class Log4NetLogger : IHabaneroLogger
	{
		private readonly string _contextName;
        /// <summary>
        /// The underlying log4net logger.
        /// </summary>
		protected ILog _log;

		///<summary>
		/// Constructs the Logger with the appropriate context
		///</summary>
		///<param name="contextName"></param>
		public Log4NetLogger(string contextName)
		{
            _contextName = contextName;
            _log = LogManager.GetLogger(contextName);
        }

		///<summary>
		/// Constructs the Logger with the appropriate contextType
		///</summary>
		///<param name="contextType">The Type of object this context is for.</param>
		public Log4NetLogger(Type contextType)
		{
			_log = LogManager.GetLogger(contextType);
			_contextName = _log.Logger.Name;
		}

		///<summary>
		/// The context that the Logger is currently running e.g. ComponentName.ClassName.MethodName
		///</summary>
		public string ContextName
		{
			get { return _contextName; }
		}

		///<summary>
		/// Logs the message to the configured loLog4Netgger with the 
		/// appropriate <see cref="LogCategory"/>
		///</summary>
		///<param name="message"></param>
		///<param name="logCategory"></param>
		public void Log(string message, LogCategory logCategory)
		{
			if (!IsLogging(logCategory)) return;
			switch (logCategory)
			{
				case LogCategory.Fatal:
					_log.Fatal(message);
					break;
				case LogCategory.Exception:
					_log.Error(message);
					break;
				case LogCategory.Debug:
					_log.Debug(message);
					break;
				case LogCategory.Warn:
					_log.Warn(message);
					break;
                case LogCategory.Error:
                    _log.Error(message);
                    break;
				default:
					_log.Info(message);
					break;
			}
		}

		/// <summary>
		/// Creates a single log entry for with the message with the appropriate message.
		/// The HabaneroLoggerLog4Net logs an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
		/// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
		/// </summary>
		/// <param name="exception"></param>
		public void Log(Exception exception)
		{
			this.Log("", exception);
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
			if (exception is UserException)
			{
				if (_log.IsInfoEnabled) _log.Info(message, exception);
				return;
			}
			if (_log.IsErrorEnabled) _log.Error(message, exception);           
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
			switch (logCategory)
			{
				case LogCategory.Fatal:
					_log.Fatal(message, exception);
					break;
				case LogCategory.Exception:
					_log.Error(message, exception);
					break;
				case LogCategory.Debug:
					_log.Debug(message, exception);
					break;
				case LogCategory.Warn:
					_log.Warn(message, exception);
					break;
				default:
					_log.Info(message, exception);
					break;
			}       
		}

	    ///<summary>
	    /// Checks the logger to see if it has been enabled to log messages of the specified <see cref="LogCategory"/> type.
	    ///</summary>
	    ///<param name="logCategory">The <see cref="LogCategory"/> for which to check if logging is enabled or not.</param>
	    ///<returns>true if the specified <see cref="LogCategory"/> messages will be logged, otherwise false.</returns>
	    public bool IsLogging(LogCategory logCategory)
		{
			switch (logCategory)
			{
				case LogCategory.Fatal:
					return _log.IsFatalEnabled;
				case LogCategory.Exception:
					return _log.IsErrorEnabled;
				case LogCategory.Debug:
					return _log.IsDebugEnabled;
				case LogCategory.Warn:
					return _log.IsWarnEnabled;
				default:
					return _log.IsInfoEnabled;
			}       
		}
	}
}