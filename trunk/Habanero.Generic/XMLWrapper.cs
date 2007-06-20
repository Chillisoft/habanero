using System.Xml;

namespace Habanero.Generic
{
    /// <summary>
    /// This class is used to retrieve and update data in an XML document.
    /// It serves as a wrapper for common functionality used by the report 
    /// definition classes, but is in no way specific to these classes.
    /// </summary>
    public class XmlWrapper
    {
        private XmlDocument _doc;
        private string _filename;

        /// <summary>
        /// Constructor that initialises the new object with an XML document
        /// that has already been instantiated as a System.Xml.XmlDocument
        /// object.
        /// </summary>
        /// <param name="doc">The xml document object to wrap</param>
        public XmlWrapper(XmlDocument doc)
        {
            this._doc = doc;
        }


        /// <summary>
        /// Constructor that initialises the new object using a specified
        /// XML file path
        /// </summary>
        /// <param name="xmlFilename">The path to the XML file to wrap</param>
        public XmlWrapper(string xmlFilename)
        {
            _filename = xmlFilename;
            _doc = new XmlDocument();
            _doc.Load(xmlFilename);
        }

        /// <summary>
        /// The XmlDocument object that is being wrapped
        /// </summary>
        public XmlDocument XmlDocument
        {
            get { return _doc; }
        }


        /// <summary>
        /// Returns the value from the node that matches the parent node and
        /// element name parameters provided.
        /// </summary>
        /// <param name="parentNode">The parent node object</param>
        /// <param name="elementName">The element name</param>
        /// <returns>Returns the value if found or an empty string if not</returns>
        /// TODO: Error-checking on arguments
        public string ReadXmlValue(XmlNode parentNode, string elementName)
        {
            XmlNode node = parentNode.SelectSingleNode(elementName);
            if (node != null)
                return node.InnerText;
            else
                return string.Empty;
        }
        
        /// <summary>
        /// Writes the given value to the xml node found using the
        /// arguments provided.  If the element specified does not exist,
        /// a new one will be created.
        /// </summary>
        /// <param name="parentNode">The parent of the node to be edited</param>
        /// <param name="elementName">The element name for the value</param>
        /// <param name="newValue">The new value to be applied</param>
        /// TODO: Error checking on arguments
        public void WriteXmlValue(XmlNode parentNode, string elementName,
                                  string newValue)
        {
            XmlNode node = parentNode.SelectSingleNode(elementName);
            if (node != null)
                node.InnerText = newValue;
            {
                XmlNode newNode = _doc.CreateNode(XmlNodeType.Element,
                                                 elementName, parentNode.NamespaceURI);
                newNode.InnerText = newValue;
                parentNode.AppendChild(newNode);
            }
        }

        /// <summary>
        /// Updates the XML document file with changes made to the XML
        /// structure.<br/>
        /// NOTE: This method will only execute if the object was originally
        /// created with the constructor that specifies a _filename.  Alternatively,
        /// use the variant of this method that takes a file name as a parameter.
        /// </summary>
        /// TODO: No testing has been done on this
        public void WriteXmlDocToFile()
        {
            if (_filename != string.Empty)
            {
                XmlTextWriter writer = new XmlTextWriter(_filename,
                                                         System.Text.Encoding.UTF8);
                _doc.WriteTo(writer);
                writer.Close();
            }
        }

        /// <summary>
        /// Updates the specified XML document file with changes made to the XML
        /// structure.
        /// </summary>
        /// <param name="filename">The name and path of the file to be updated</param>
        /// TODO: No testing has been done on this
        public void WriteXmlDocToFile(string filename)
        {
            this._filename = filename;
            WriteXmlDocToFile();
        }
    }
}