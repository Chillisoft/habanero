using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads property definitions from xml data
    /// </summary>
    public class XmlPropertyLoader : XmlLoader
    {
        private Type itsPropertyType;
        private cbsPropReadWriteRule itsReadWriteRule;
        private String itsPropertyName;
        private object itsDefaultValue;
        private string itsDatabaseFieldName;
        private PropDef itsPropDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyLoader()
        {
        }

        /// <summary>
        /// Loads the property definition from the xml string provided
        /// </summary>
        /// <param name="xmlPropDef">The xml string</param>
        /// <returns>Returns a property definition</returns>
        public PropDef LoadProperty(string xmlPropDef)
        {
            if (xmlPropDef == null || xmlPropDef.Length == 0)
            {
                throw new XmlException("An error has occurred while attempting to " +
                   "load a property definition, contained in a 'propertyDef' element. " +
                   "Check that you have correctly specified at least one 'propertyDef' " +
                   "element, which defines a property that is to be mapped from a " +
                   "database field to a property in a class.");
            }
            return LoadProperty(CreateXmlElement(xmlPropDef));
        }

        /// <summary>
        /// Loads the property definition from the xml element provided
        /// </summary>
        /// <param name="propertyElement">The xml property element</param>
        /// <returns>Returns a PropDef object</returns>
        public PropDef LoadProperty(XmlElement propertyElement)
        {
            return (PropDef) Load(propertyElement);
        }

        /// <summary>
        /// Creates the property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a PropDef object</returns>
        protected override object Create()
        {
            return itsPropDef;
        }

        /// <summary>
        /// Loads the property definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            LoadPropertyName();
            LoadPropertyType();
            LoadReadWriteRule();
            LoadDefaultValue();
            LoadDatabaseFieldName();

            itsReader.Read();

            if (itsDatabaseFieldName != null)
            {
                itsPropDef =
                    new PropDef(itsPropertyName, itsPropertyType, itsReadWriteRule, itsDatabaseFieldName,
                                itsDefaultValue);
            }
            else
            {
                itsPropDef = new PropDef(itsPropertyName, itsPropertyType, itsReadWriteRule, itsDefaultValue);
            }

            if (itsReader.Name.Length >= 12 && itsReader.Name.Substring(0, 12) == "propertyRule")
            {
                XmlPropertyRuleLoader.LoadRuleIntoProperty(itsReader.ReadOuterXml(), itsPropDef, itsDtdPath);
            }
            int len = "lookupListSource".Length;
            if (itsReader.Name.Length >= len &&
                itsReader.Name.Substring(itsReader.Name.Length - len, len) == "LookupListSource")
            {
                XmlLookupListSourceLoader.LoadLookupListSourceIntoProperty(itsReader.ReadOuterXml(), itsPropDef,
                                                                           itsDtdPath);
            }
        }

        /// <summary>
        /// Loads the property name attribute from the reader
        /// </summary>
        private void LoadPropertyName()
        {
            itsPropertyName = itsReader.GetAttribute("name");
            if (itsPropertyName == null || itsPropertyName.Length == 0)
            {
                throw new XmlException("A 'propertyDef' element has no 'name' attribute " +
                   "set. Each 'propertyDef' element requires a 'name' attribute that " +
                   "specifies the name of the property in the class to map to.");
            }
        }

        /// <summary>
        /// Loads the property type from the reader (the "assembly" attribute)
        /// </summary>
        private void LoadPropertyType()
        {
            string assemblyName = itsReader.GetAttribute("assembly");
            string typeName = itsReader.GetAttribute("type");
            try
            {
                itsPropertyType = TypeLoader.LoadType(assemblyName, typeName);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException("Unable to load the property type while " +
                       "attempting to load a property definition, given the 'assembly' as: '" +
                       assemblyName + "', and the 'type' as: '" + typeName +
                       "'. Check that the type exists in the given assembly name and " +
                       "that spelling and capitalisation are correct.", ex);
            }
        }

        /// <summary>
        /// Loads the read-write rule from the reader
        /// </summary>
        private void LoadReadWriteRule()
        {
            itsReadWriteRule =
                (cbsPropReadWriteRule)
                Enum.Parse(typeof (cbsPropReadWriteRule), itsReader.GetAttribute("readWriteRule"));
        }

        /// <summary>
        /// Loads the default value from the reader
        /// </summary>
        private void LoadDefaultValue()
        {
            string strDefValue = itsReader.GetAttribute("defaultValue");
            if (strDefValue != null)
            {
                if (itsPropertyType == typeof (Guid))
                {
                    itsDefaultValue = new Guid(strDefValue);
                }
                else
                {
                    itsDefaultValue = Convert.ChangeType(strDefValue, itsPropertyType);
                }
            }
            else
            {
                itsDefaultValue = null;
            }
        }

        /// <summary>
        /// Loads the database field name from the reader
        /// </summary>
        private void LoadDatabaseFieldName()
        {
            itsDatabaseFieldName = itsReader.GetAttribute("databaseFieldName");
        }
    }
}