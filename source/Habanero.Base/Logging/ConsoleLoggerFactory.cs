using System;
using System.Collections.Generic;
using System.Linq;

namespace Habanero.Base.Logging
{
    public class ConsoleLoggerFactory : IHabaneroLoggerFactory
    {
        readonly IDictionary<string, IHabaneroLogger> _loggers = new Dictionary<string, IHabaneroLogger>();
        private IDictionary<LogCategory, bool> _isLogging = new Dictionary<LogCategory, bool>();

        public ConsoleLoggerFactory()
        {
            _isLogging.Add(LogCategory.Debug, false);
            _isLogging.Add(LogCategory.Info, false);
            _isLogging.Add(LogCategory.Warn, true);
            _isLogging.Add(LogCategory.Exception, true);
            _isLogging.Add(LogCategory.Fatal, true);
        }

        public IHabaneroLogger GetLogger(string contextName)
        {
            if (_loggers.ContainsKey(contextName)) return _loggers[contextName];
            var consoleLogger = new ConsoleLogger(contextName,  new Dictionary<LogCategory, bool>(_isLogging));
            _loggers.Add(contextName, consoleLogger);
            return consoleLogger;
        }

        public IHabaneroLogger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        public void SetDefaultIsLogging(LogCategory logCategory, bool isLogging)
        {
            _isLogging[logCategory] = isLogging;
        }
    }

    public class ConsoleLogger : IHabaneroLogger
    {
        private readonly string _contextName;
        private readonly IDictionary<LogCategory, bool> _isLogging;


        public ConsoleLogger(string contextName, IDictionary<LogCategory, bool> isLogging)
        {
            _contextName = contextName;
            _isLogging = isLogging;
        }

        public string ContextName
        {
            get { return _contextName; }
        }

        public void Log(string message, LogCategory logCategory)
        {
            if (!IsLogging(logCategory)) return;
            Console.Out.WriteLine("{0} {1} {2} {3} {4}", DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToShortTimeString(), _contextName,
                                  Enum.GetName(typeof (LogCategory), logCategory), message);
        }

        public void Log(Exception exception)
        {
            if (!IsLogging(LogCategory.Exception)) return;
            Log(exception.GetType().Name + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace, LogCategory.Exception);
        }

        public void Log(string message, Exception exception)
        {
            if (!IsLogging(LogCategory.Exception)) return;
            Log(message + Environment.NewLine + 
                exception.GetType().Name + Environment.NewLine + 
                exception.Message + Environment.NewLine + 
                exception.StackTrace, LogCategory.Exception);
        }

        public void Log(string message, Exception exception, LogCategory logCategory)
        {
            if (!IsLogging(logCategory)) return;
            Log(message + Environment.NewLine +
                 exception.GetType().Name + Environment.NewLine +
                 exception.Message + Environment.NewLine +
                 exception.StackTrace, logCategory);
        }

        public bool IsLogging(LogCategory logCategory)
        {
            return _isLogging[logCategory];
        }

        public void SetIsLogging(LogCategory logCategory, bool isLogging)
        {
            _isLogging[logCategory] = isLogging;
        }
    }
}