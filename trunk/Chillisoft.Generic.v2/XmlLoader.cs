using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Provides a super-class for different types of xml loaders.
    /// A sub-class must implement LoadFromReader(), which loads the data
    /// from the reader, and Create(), which creates the object that the
    /// xml represents.
    /// </summary>
    /// (from before) Using Form Template Method (Refactoring p345) to make 
    /// sure all XmlLoaders implement dtd validation. 
    public abstract class XmlLoader
    {
        protected readonly string itsDtdPath;
        protected XmlValidatingReader itsReader;
        private bool itsDocumentValid = true;
        private ValidationEventArgs itsInvalidDocumentArgs;
        private XmlElement itsElement;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlLoader(string dtdPath)
        {
            itsDtdPath = dtdPath;
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlLoader() : this("")
        {
        }

        /// <summary>
        /// Loads the specified xml element
        /// </summary>
        /// <param name="el">The xml element</param>
        /// <returns>Returns the loaded object</returns>
        protected object Load(XmlElement el)
        {
            itsElement = el;
            CreateValidatingReader(el);
            LoadFromReader();
            CheckDocumentValidity();
            return Create();
        }

        /// <summary>
        /// Creates the object using the data that has been read in using
        /// LoadFromReader(). This method needs to be implemented by the
        /// sub-class.
        /// </summary>
        /// <returns>Returns the object created</returns>
        protected abstract object Create();

        /// <summary>
        /// Loads all the data out of the reader, assuming the document is 
        /// well-formed, otherwise the error must be caught and thrown.
        /// By the end of this method the reader must be finished reading.
        /// This method needs to be implemented by the sub-class.
        /// </summary>
        protected abstract void LoadFromReader();

        /// <summary>
        /// Checks that the xml document is valid and well-formed
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if the
        /// xml document is not valid</exception>
        private void CheckDocumentValidity()
        {
            if (!itsDocumentValid)
            {
                throw new InvalidXmlDefinitionException("The " + itsElement.Name + " node does not conform to its dtd." +
                                                        itsInvalidDocumentArgs.Message);
            }
        }

        /// <summary>
        /// Creates a reader to read and validate the xml data for the
        /// property element specified
        /// </summary>
        /// <param name="propertyElement">The xml property element</param>
        private void CreateValidatingReader(XmlElement propertyElement)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(propertyElement.OuterXml);
            doc.InsertBefore(
                doc.CreateDocumentType(doc.DocumentElement.Name, null, null, GetDTD(doc.DocumentElement.Name)),
                doc.DocumentElement);
            itsReader = new XmlValidatingReader(new XmlTextReader(new StringReader(doc.OuterXml)));
            itsReader.ValidationType = ValidationType.DTD;
            itsReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            itsReader.Read();
        }

        /// <summary>
        /// Returns the dtd for the root element name provided
        /// </summary>
        /// <param name="rootElementName">The root element name</param>
        /// <returns>Returns a string</returns>
        private string GetDTD(string rootElementName)
        {
            string dtdFileName = itsDtdPath + rootElementName + ".dtd";
            if (!File.Exists(dtdFileName))
            {
                throw new FileNotFoundException("The dtd for " + rootElementName + " was not found in '" + itsDtdPath + "'.");
            }
            return new DtdLoader(itsDtdPath).LoadDtd(dtdFileName);
            //return new StreamReader(dtdFileName).ReadToEnd();
        }

        /// <summary>
        /// Handles the event of an xml document being invalid
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            itsDocumentValid = false;
            itsInvalidDocumentArgs = args;
        }

        /// <summary>
        /// Creates and returns an xml element for the element name provided
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>Returns an XmlElement object</returns>
        protected XmlElement CreateXmlElement(string element)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(element);
            return doc.DocumentElement;
        }
    }
}