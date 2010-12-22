using System;
using System.Xml;
using log4net.Config;

namespace Habanero.Base
{
    /// <summary>
    /// Logger factory for creating a Log4NetLogger
    /// </summary>
    public class Log4NetLoggerFactory : IHabaneroLoggerFactory
    {
        public Log4NetLoggerFactory()
        {
/*            try
            {*/
                XmlConfigurator.Configure();
/*            }
            catch (Exception ex)
            {
                throw new XmlException("There was an error reading the XML configuration file. " +
                                       "Check that all custom configurations, such as DatabaseConfig, are well-formed, " +
                                       "spelt correctly and have been declared correctly in configSections.  See the " +
                                       "Habanero tutorial for example usage or see official " +
                                       "documentation on configuration files if the error is not resolved.", ex);
            }*/
        }

        public IHabaneroLogger GetLogger(string contextName)
        {
            return new Log4NetLogger(contextName);
        }
    }
}