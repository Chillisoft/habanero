using System;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Core;

namespace Habanero.Base
{
    public class Log4NetLogger : IHabaneroLogger
    {
        private readonly string _contextName;
        protected ILog _log;
        public Log4NetLogger(string contextName)
        {
            _contextName = contextName;



            _log = LogManager.GetLogger(contextName);
        }

        public string ContextName
        {
            get { return _contextName; }
        }

        public void Log(string message)
        {
            if(_log.IsErrorEnabled) _log.Error(message);
        }

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