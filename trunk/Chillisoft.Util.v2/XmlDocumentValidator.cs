using System.IO;
using System.Xml;
using System.Xml.Schema;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Validates an xml document
    /// </summary>
    public class XmlDocumentValidator
    {
        private bool itsDocumentValid = true;
        private ValidationEventArgs itsInvalidDocumentArgs;
        private XmlDocument itsXmlDocument;

        /// <summary>
        /// Handles a validation failure
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="args">Attached arguments regarding the event</param>
        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            itsDocumentValid = false;
            itsInvalidDocumentArgs = args;
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
            itsXmlDocument = new XmlDocument();
            itsXmlDocument.LoadXml(xmlDocument);
            itsXmlDocument.InsertBefore(itsXmlDocument.CreateDocumentType(rootElementName, null, null, dtd),
                                        itsXmlDocument.DocumentElement);
            ValidateCurrentDocument();
        }

        /// <summary>
        /// Validates the xml document
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if there
        /// is a validation failure</exception>
        private void ValidateCurrentDocument()
        {
            XmlValidatingReader validatingReader =
                new XmlValidatingReader(new XmlTextReader(new StringReader(itsXmlDocument.OuterXml)));
            validatingReader.ValidationType = ValidationType.DTD;
            validatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            while (validatingReader.Read())
            {
                ;
            }
            if (!itsDocumentValid)
            {
                throw new InvalidXmlDefinitionException("The relationship node does not conform to the dtd." +
                                                        itsInvalidDocumentArgs.Message);
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
            itsXmlDocument = xmlDocument;
            itsXmlDocument.InsertBefore(
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
            itsXmlDocument = new XmlDocument();
            itsXmlDocument.LoadXml(xmlElement.OuterXml);
            ValidateDocument(itsXmlDocument);
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
            if (!File.Exists(dtdFileName))
            {
                throw new FileNotFoundException("The dtd for " + rootElementName + " was not found.");
            }
            return new StreamReader(dtdFileName).ReadToEnd();
        }
    }
}