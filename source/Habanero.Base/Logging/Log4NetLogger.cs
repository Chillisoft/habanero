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
            switch (logCategory)
            {
                case LogCategory.Fatal:
                    if (_log.IsFatalEnabled) _log.Fatal(message);
                    //else Log(message, LogCategory.Exception);
                    break;
                case LogCategory.Exception:
                    if (_log.IsErrorEnabled) _log.Error(message);
                    //else Log(message, LogCategory.Debug);
                    break;
                case LogCategory.Debug:
                    if (_log.IsDebugEnabled) _log.Debug(message);
                    //else Log(message, LogCategory.Warn);
                    break;
                case LogCategory.Warn:
                    if (_log.IsWarnEnabled) _log.Warn(message);
                    //else Log(message, LogCategory.Info);
                    break;
                default:
                    if (_log.IsInfoEnabled) _log.Info(message);
                    break;
            }
        }

        /// <summary>
        /// Creates a single log entry for with the message with the appropriate message.
        /// The Log4NetLogger logs an entry with <see cref="LogCategory.Exception"/> unless the exception inherits from
        /// <see cref="UserException"/> in which case we log this with the <see cref="LogCategory.Info"/>
        /// </summary>
        /// <param name="exception"></param>
        public void Log(Exception exception)
        {
            this.Log("", exception);
        }

        public void Log(string message, Exception exception)
        {
            if (exception is UserException)
            {
                if (_log.IsInfoEnabled) _log.Info(message, exception);
                return;
            }
            if (_log.IsErrorEnabled) _log.Error(message, exception);           
        }

        public void Log(string message, Exception exception, LogCategory logCategory)
        {
            switch (logCategory)
            {
                case LogCategory.Fatal:
                    if (_log.IsFatalEnabled) _log.Fatal(message, exception);
                    //else Log(message, LogCategory.Exception);
                    break;
                case LogCategory.Exception:
                    if (_log.IsErrorEnabled) _log.Error(message, exception);
                    //else Log(message, LogCategory.Debug);
                    break;
                case LogCategory.Debug:
                    if (_log.IsDebugEnabled) _log.Debug(message, exception);
                    //else Log(message, LogCategory.Warn);
                    break;
                case LogCategory.Warn:
                    if (_log.IsWarnEnabled) _log.Warn(message, exception);
                    //else Log(message, LogCategory.Info);
                    break;
                default:
                    if (_log.IsInfoEnabled) _log.Info(message, exception);
                    break;
            }       
        }
    }
}