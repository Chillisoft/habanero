// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
            return new Log4NetLogger(contextType);
        }
    }
}