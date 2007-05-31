using System.Xml;
using Chillisoft.Xml.v2;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Stores database configuration information in an XML format
    /// </summary>
    /// TODO ERIC - review
    public class XmlDatabaseConfig : DatabaseConfig
    {
        private XmlNode itsNode;
        private XmlWrapper itsXmlWrapper;

        /// <summary>
        /// Constructor to initialise a new configuration
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="wrapper">The xml wrapper</param>
        public XmlDatabaseConfig(XmlNode node, XmlWrapper wrapper) : base()
        {
            itsNode = node;
            itsXmlWrapper = wrapper;
        }

        /// <summary>
        /// Gets and sets the database vendor setting
        /// </summary>
        public override string Vendor
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "Vendor"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "Vendor", value); }
        }

        /// <summary>
        /// Gets and sets the database server setting
        /// </summary>
        public override string Server
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "Server"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "Server", value); }
        }

        /// <summary>
        /// Gets and sets the database name setting
        /// </summary>
        public override string Database
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "Database"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "Database", value); }
        }

        /// <summary>
        /// Gets and sets the username setting
        /// </summary>
        public override string UserName
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "UserName"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "UserName", value); }
        }

        /// <summary>
        /// Gets and sets the password setting
        /// </summary>
        public override string Password
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "Password"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "Password", value); }
        }

        /// <summary>
        /// Gets and sets the port setting
        /// </summary>
        public override string Port
        {
            get { return itsXmlWrapper.ReadXmlValue(itsNode, "Port"); }
            set { itsXmlWrapper.WriteXmlValue(itsNode, "Port", value); }
        }
    }
}