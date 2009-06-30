using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Habanero.Base.Exceptions;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Validates a classdefs xml string against the schema and returns an <see cref="XmlValidationResult"/> which contains
    /// any validation errors.
    /// </summary>
    public class ClassDefsXmlValidator
    {
        /// <summary>
        /// Validates the xml string against the classdefs schema.  
        /// </summary>
        /// <param name="xml">The classdefs xml to validate</param>
        /// <returns>The result of the validation.  See <see cref="XmlValidationResult"/>.</returns>
        public XmlValidationResult ValidateClassDefsXml(string xml)
        {
            List<string> validationMessages = new List<string>();
            bool isValid = true;
            bool schemaValid = true;
            string xsd = Xsds.classes;

            XmlParserContext context = new XmlParserContext(null, null, "", XmlSpace.None);
            XmlTextReader readerXml = new XmlTextReader(xml, XmlNodeType.Element, context);

            XmlTextReader readerSchema = new XmlTextReader(new StringReader(xsd));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            XmlSchema schema = XmlSchema.Read(readerSchema, (sender, args) => schemaValid = false);
            settings.Schemas.Add(schema);
            if (!schemaValid) throw new HabaneroDeveloperException("classes Schema is invalid", "classes Schema is invalid");
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.ValidationEventHandler += (sender, args) =>
                                               {
                                                   validationMessages.Add(args.Message);
                                                   isValid = false;
                                               };
            XmlReader reader = XmlReader.Create(readerXml, settings);

            try
            {
                while (reader.Read()) { }
            }
            catch (XmlException ex)
            {
                validationMessages.Add(ex.Message);
                isValid = false;
            }

            return new XmlValidationResult(isValid, validationMessages);

        }
    }
}