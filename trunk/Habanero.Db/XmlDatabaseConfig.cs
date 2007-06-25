using System.Xml;
using Habanero.Base;

namespace Habanero.Db
{
    /// <summary>
    /// Stores database configuration information in an XML format
    /// </summary>
    /// TODO ERIC - review
    public class XmlDatabaseConfig : DatabaseConfig
    {
        private XmlNode _node;
        private XmlWrapper _xmlWrapper;

        /// <summary>
        /// Constructor to initialise a new configuration
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="wrapper">The xml wrapper</param>
        public XmlDatabaseConfig(XmlNode node, XmlWrapper wrapper) : base()
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