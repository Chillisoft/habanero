using System;
using System.Xml;
using log4net.Config;

namespace Habanero.Base.Logging
{
    /// <summary>
    /// Logger factory for creating a <see cref="Log4NetLogger"/>
    /// </summary>
    public class Log4NetLoggerFactory : IHabaneroLoggerFactory
    {
        ///<summary>
        /// Constructs the Log4net factory
        ///</summary>
        public Log4NetLoggerFactory()
        {
            try
            {
                XmlConfigurator.Configure();
            }
            catch (Exception ex)
            {
                throw new XmlException("There was an error reading the XML configuration file. " +
                                       "Log4Net could not load its configuration file, " +
                                       "if you are using Log4Net as your logger then please .  See the " +
                                       "Habanero tutorial for example usage or see official " +
                                       "documentation on configuration files if the error is not resolved.", ex);
            }
        }

        public IHabaneroLogger GetLogger(string contextName)
        {

            return new Log4NetLogger(contextName);
        }

        public IHabaneroLogger GetLogger(Type contextType)
        {
            //   throw new NotImplementedException();
            return new Log4NetLogger(contextType);
        }
    }

}