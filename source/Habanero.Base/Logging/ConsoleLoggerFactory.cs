using System;

namespace Habanero.Base.Logging
{
    public class ConsoleLoggerFactory : IHabaneroLoggerFactory
    {
        public IHabaneroLogger GetLogger(string contextName)
        {
            return new ConsoleLogger(contextName);
        }

        public IHabaneroLogger GetLogger(Type type)
        {
            return new ConsoleLogger(type.FullName);
        }
    }

    public class ConsoleLogger : IHabaneroLogger
    {
        private readonly string _contextName;

        public ConsoleLogger(string contextName)
        {
            _contextName = contextName;
        }

        public string ContextName
        {
            get { return _contextName; }
        }

        public void Log(string message, LogCategory logCategory)
        {
            Console.Out.WriteLine("{0} {1} {2} {3} {4}", DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToShortTimeString(), _contextName,
                                  Enum.GetName(typeof (LogCategory), logCategory), message);
        }

        public void Log(Exception exception)
        {
            Log(exception.GetType().Name + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace, LogCategory.Exception);
        }

        public void Log(string message, Exception exception)
        {
            Log(message + Environment.NewLine + 
                exception.GetType().Name + Environment.NewLine + 
                exception.Message + Environment.NewLine + 
                exception.StackTrace, LogCategory.Exception);
        }

        public void Log(string message, Exception exception, LogCategory logCategory)
        {
            Log(message + Environment.NewLine +
                 exception.GetType().Name + Environment.NewLine +
                 exception.Message + Environment.NewLine +
                 exception.StackTrace, logCategory);
        }

        public bool IsLogging(LogCategory logCategory)
        {
            return true;
        }
    }
}