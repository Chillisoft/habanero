using System;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Core;

namespace Habanero.Base
{
    ///<summary>
    ///</summary>
    public class Log4NetLogger : IHabaneroLogger
    {
        private readonly string _contextName;
        protected ILog _log;
        public Log4NetLogger(string contextName)
        {
            _contextName = contextName;



            _log = LogManager.GetLogger(contextName);
        }

        ///<summary>
        /// The context that the Logger is currently running e.g. ComponentName.ClassName.MethodName
        ///</summary>
        public string ContextName
        {
            get { return _contextName; }
        }

        ///<summary>
        /// Logs the message to the predifined Logger
        ///</summary>
        ///<param name="message"></param>
        public void Log(string message)
        {
            if(_log.IsErrorEnabled) _log.Error(message);
        }

        ///<summary>
        /// Logs the message to the predifined logger with the 
        /// appropriate <see cref="LogCategory"/>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="logCategory"></param>
        public void Log(string message, LogCategory logCategory)
        {
            switch (logCategory)
            {
                case LogCategory.Debug:
                    if (_log.IsDebugEnabled) _log.Debug(message);
                    break;

                case LogCategory.Exception:
                    Log(message);
                    break;

                case LogCategory.Warn:
                    if (_log.IsWarnEnabled) _log.Warn(message);
                    break;

                default:
                    if (_log.IsInfoEnabled) _log.Info(message);
                    break;
            }
        }

    }
}