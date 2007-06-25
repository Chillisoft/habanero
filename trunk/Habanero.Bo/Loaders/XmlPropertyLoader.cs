using System;
using System.Xml;
using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads property definitions from xml data
    /// </summary>
    public class XmlPropertyLoader : XmlLoader
    {
        private string _assemblyName;
    	private string _typeName;
        private PropReadWriteRule _readWriteRule;
        private string _propertyName;
        private string _defaultValueString;
        private string _databaseFieldName;
        private PropDef _propDef;
        private bool _compulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlPropertyLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
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
                throw new InvalidXmlDefinitionException("An error has occurred while attempting to " +
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
            return _propDef;
        }

        /// <summary>
        /// Loads the property definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadPropertyName();
            LoadPropertyType();
            LoadReadWriteRule();
            LoadDefaultValue();
            LoadDatabaseFieldName();
            LoadCompulsory();

            _reader.Read();

			_propDef = _defClassFactory.CreatePropDef(_propertyName, _assemblyName, _typeName, 
				_readWriteRule, _databaseFieldName, _defaultValueString, _compulsory);
			//_propDef = new PropDef(_propertyName, _assemblyName, _typeName, 
			//    _readWriteRule, _databaseFieldName, _defaultValueString);

            if (_reader.Name == "rule")
            {
                XmlRuleLoader loader = new XmlRuleLoader(DtdLoader, _defClassFactory);
                loader.LoadRuleIntoProperty(_reader.ReadOuterXml(), _propDef);
                //XmlPropertyRuleLoader.LoadRuleIntoProperty(_reader.ReadOuterXml(), _propDef, DtdLoader, _defClassFactory);
            }
            int len = "lookupListSource".Length;
            if (_reader.Name.Length >= len &&
                _reader.Name.Substring(_reader.Name.Length - len, len) == "LookupListSource")
            {
                XmlLookupListSourceLoader.LoadLookupListSourceIntoProperty(_reader.ReadOuterXml(), _propDef,
                                                                           DtdLoader, _defClassFactory);
            }
        }


        /// <summary>
        /// Loads the property name attribute from the reader
        /// </summary>
        private void LoadPropertyName()
        {
            _propertyName = _reader.GetAttribute("name");
            if (_propertyName == null || _propertyName.Length == 0)
            {
                throw new InvalidXmlDefinitionException("A 'propertyDef' element has no 'name' attribute " +
                   "set. Each 'propertyDef' element requires a 'name' attribute that " +
                   "specifies the name of the property in the class to map to.");
            }
        }

        /// <summary>
        /// Loads the property type from the reader (the "assembly" attribute)
        /// </summary>
        private void LoadPropertyType()
        {
            _assemblyName = _reader.GetAttribute("assembly");
            _typeName = _reader.GetAttribute("type");

            if (_assemblyName == "System")
            {
                Hashtable typeConverter = new Hashtable();
                typeConverter.Add("byte", "Byte");
                typeConverter.Add("sbyte", "Sbyte");
                typeConverter.Add("short", "Int16");
                typeConverter.Add("int", "Int32");
                typeConverter.Add("long", "Int64");
                typeConverter.Add("ushort", "UInt16");
                typeConverter.Add("uint", "UInt32");
                typeConverter.Add("ulong", "UInt64");
                typeConverter.Add("float", "Single");
                typeConverter.Add("object", "Object");
                typeConverter.Add("char", "Char");
                typeConverter.Add("string", "String");
                typeConverter.Add("decimal", "Decimal");
                typeConverter.Add("boolean", "Boolean");
                typeConverter.Add("bool", "Boolean");
                typeConverter.Add("guid", "Guid");

                if (typeConverter.ContainsKey(_typeName))
                {
                    _typeName = (string)typeConverter[_typeName];
                }
            }
        }

        /// <summary>
        /// Loads the read-write rule from the reader
        /// </summary>
        private void LoadReadWriteRule()
        {
            try
            {
                _readWriteRule =
                    (PropReadWriteRule)
                    Enum.Parse(typeof (PropReadWriteRule), _reader.GetAttribute("readWriteRule"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "In the property definition for '{0}', the 'readWriteRule' " +
                    "was set to an invalid value. The valid options are " +
                    "ReadManyWriteMany, ReadOnly, WriteOnly, ReadManyWriteNew " +
                    "and ReadManyWriteOnce.", _propertyName), ex);
            }
        }

        /// <summary>
        /// Loads the default value from the reader
        /// </summary>
        private void LoadDefaultValue()
        {
            _defaultValueString = _reader.GetAttribute("defaultValue");

        }

        /// <summary>
        /// Loads the database field name from the reader
        /// </summary>
        private void LoadDatabaseFieldName()
        {
            _databaseFieldName = _reader.GetAttribute("databaseFieldName");
        }


        private void LoadCompulsory()
        {
            _compulsory = Convert.ToBoolean(_reader.GetAttribute("compulsory"));
        }
    }
}