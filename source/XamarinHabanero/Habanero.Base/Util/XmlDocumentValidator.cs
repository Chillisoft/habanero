#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Validates an xml document
    /// </summary>
    public class XmlDocumentValidator
    {
        private bool _documentValid = true;
        private ValidationEventArgs _invalidDocumentArgs;
        private XmlDocument _xmlDocument;

        /// <summary>
        /// Handles a validation failure
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="args">Attached arguments regarding the event</param>
        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            _documentValid = false;
            _invalidDocumentArgs = args;
        }

        /// <summary>
        /// Validates the given xml document, throwing an exception if there
        /// is a validation failure
        /// </summary>
        /// <param name="xmlDocument">The xml document in a continuous 
        /// string</param>
        /// <param name="rootElementName">The root element name</param>
        /// <param name="dtd">The dtd path</param>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if there
        /// is a validation failure</exception>
        public void ValidateDocument(string xmlDocument, string rootElementName, string dtd)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.LoadXml(xmlDocument);
            _xmlDocument.InsertBefore(_xmlDocument.CreateDocumentType(rootElementName, null, null, dtd),
                                        _xmlDocument.DocumentElement);
            ValidateCurrentDocument();
        }

        /// <summary>
        /// Validates the xml document
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if there
        /// is a validation failure</exception>
        private void ValidateCurrentDocument()
        {
             XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.DTD;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.ValidationEventHandler += ValidationHandler;
            XmlReader validatingReader = XmlReader.Create(new XmlTextReader(new StringReader(_xmlDocument.OuterXml)), settings);
               // new XmlValidatingReader(new XmlTextReader(new StringReader(_xmlDocument.OuterXml)));
            //validatingReader.ValidationType = ValidationType.DTD;
            //validatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            while (validatingReader.Read())
            {
                ;
            }
            if (!_documentValid)
            {
                throw new InvalidXmlDefinitionException("The relationship node does not conform to the dtd." +
                                                        _invalidDocumentArgs.Message);
            }
        }

        /// <summary>
        /// Validats the given xml document
        /// </summary>
        /// <param name="xmlDocument">The xml document object</param>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if there
        /// is a validation failure</exception>
        public void ValidateDocument(XmlDocument xmlDocument)
        {
            _xmlDocument = xmlDocument;
            _xmlDocument.InsertBefore(
                xmlDocument.CreateDocumentType(xmlDocument.DocumentElement.Name, null, null,
                                               GetDTD(xmlDocument.DocumentElement.Name)), xmlDocument.DocumentElement);
            ValidateCurrentDocument();
        }

        /// <summary>
        /// Validates an xml element
        /// </summary>
        /// <param name="xmlElement">The xml element object</param>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if there
        /// is a validation failure</exception>
        public void ValidateElement(XmlElement xmlElement)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.LoadXml(xmlElement.OuterXml);
            ValidateDocument(_xmlDocument);
        }

        /// <summary>
        /// Returns the dtd for the given root element name
        /// </summary>
        /// <param name="rootElementName">The root element name</param>
        /// <returns>Returns a string</returns>
        /// <exception cref="FileNotFoundException">Thrown if the dtd
        /// was not found</exception>
        private string GetDTD(string rootElementName)
        {
            string dtdFileName = rootElementName + ".dtd";
            if (!System.IO.File.Exists(dtdFileName))
            {
                throw new FileNotFoundException("The dtd for " + rootElementName + " was not found.");
            }
            return new StreamReader(dtdFileName).ReadToEnd();
        }
    }
}