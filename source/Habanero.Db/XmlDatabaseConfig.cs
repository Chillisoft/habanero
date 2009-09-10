// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Xml.XPath;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Stores database configuration information in an XML format
    /// </summary>
    public class XmlDatabaseConfig : DatabaseConfig
    {
        private readonly IXPathNavigable _node;
        private readonly XmlWrapper _xmlWrapper;

        /// <summary>
        /// Constructor to initialise a new configuration
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="wrapper">The xml wrapper</param>
        public XmlDatabaseConfig(IXPathNavigable node, XmlWrapper wrapper)
        {
            _node = node;
            _xmlWrapper = wrapper;
        }

        /// <summary>
        /// Gets and sets the database vendor setting
        /// </summary>
        public override string Vendor
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Vendor"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Vendor", value); }
        }

        /// <summary>
        /// Gets and sets the database server setting
        /// </summary>
        public override string Server
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Server"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Server", value); }
        }

        /// <summary>
        /// Gets and sets the database name setting
        /// </summary>
        public override string Database
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Database"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Database", value); }
        }

        /// <summary>
        /// Gets and sets the username setting
        /// </summary>
        public override string UserName
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "UserName"); }
            set { _xmlWrapper.WriteXmlValue(_node, "UserName", value); }
        }

        /// <summary>
        /// Gets and sets the password setting
        /// </summary>
        public override string Password
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Password"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Password", value); }
        }

        /// <summary>
        /// Gets and sets the port setting
        /// </summary>
        public override string Port
        {
            get { return _xmlWrapper.ReadXmlValue(_node, "Port"); }
            set { _xmlWrapper.WriteXmlValue(_node, "Port", value); }
        }
    }
}