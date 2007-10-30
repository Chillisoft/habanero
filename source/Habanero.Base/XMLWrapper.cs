//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Xml;
using System.Xml.XPath;
using Habanero.Base.Exceptions;

namespace Habanero.Base
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
        public string ReadXmlValue(IXPathNavigable parentNode, string elementName)
        {
            if (parentNode == null)
            {
                throw new HabaneroArgumentException("parentNode", "The parent node " +
                    "being passed in the method ReadXMLValue() is null.");
            }
            IXPathNavigable node = parentNode.CreateNavigator().SelectSingleNode(elementName);
            if (node != null)
                return node.CreateNavigator().InnerXml;
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
        public void WriteXmlValue(IXPathNavigable parentNode, string elementName,
                                  string newValue)
        {
            if (parentNode == null)
            {
                throw new HabaneroArgumentException("parentNode", "The parent node " +
                    "being passed in the method WriteXMLValue() is null.");
            }
            IXPathNavigable node = parentNode.CreateNavigator().SelectSingleNode(elementName);
            if (node != null)
                node.CreateNavigator().InnerXml = newValue;
            {
                XmlNode newNode = _doc.CreateNode(XmlNodeType.Element,
                                                 elementName, parentNode.CreateNavigator().NamespaceURI);
                newNode.InnerText = newValue;
                parentNode.CreateNavigator().AppendChild(newNode.CreateNavigator());
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