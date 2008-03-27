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

using System;
using System.Xml;
using System.Collections;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
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
        private bool _autoIncrementing;
        private int _length;
        private string _description;
        private string _displayName;
        private bool _keepValuePrivate;

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
                   "load a property definition, contained in a 'property' element. " +
                   "Check that you have correctly specified at least one 'property' " +
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
            LoadDisplayName();
            LoadPropertyType();
            LoadReadWriteRule();
            LoadDefaultValue();
            LoadDatabaseFieldName();
            LoadDescription();
            LoadCompulsory();
            LoadAutoIncrementing();
            LoadLength();
            LoadKeepValuePrivate();

            _reader.Read();

			_propDef = _defClassFactory.CreatePropDef(_propertyName, _assemblyName, _typeName, _readWriteRule,
                _databaseFieldName, _defaultValueString, _compulsory, _autoIncrementing, _length, _displayName, _description, _keepValuePrivate);
			//_propDef = new PropDef(_propertyName, _assemblyName, _typeName, 
			//    _readWriteRule, _databaseFieldName, _defaultValueString);

            if (_reader.Name == "rule")
            {
                XmlRuleLoader loader = new XmlRuleLoader(DtdLoader, _defClassFactory);
                loader.LoadRuleIntoProperty(_reader.ReadOuterXml(), _propDef);
                //XmlPropertyRuleLoader.LoadRuleIntoProperty(_reader.ReadOuterXml(), _propDef, DtdLoader, _defClassFactory);
            }
            int len = "lookupList".Length;
            if (_reader.Name.Length >= len &&
                _reader.Name.Substring(_reader.Name.Length - len, len) == "LookupList")
            {
                XmlLookupListLoader.LoadLookupListIntoProperty(_reader.ReadOuterXml(), _propDef,
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
                throw new InvalidXmlDefinitionException("A 'property' element has no 'name' attribute " +
                   "set. Each 'property' element requires a 'name' attribute that " +
                   "specifies the name of the property in the class to map to.");
            }
        }

        /// <summary>
        /// Loads the property display name from the reader
        /// </summary>
        private void LoadDisplayName()
        {
            _displayName = _reader.GetAttribute("displayName");
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
                typeConverter.Add("double", "Double"); 
                typeConverter.Add("object", "Object");
                typeConverter.Add("char", "Char");
                typeConverter.Add("string", "String");
                typeConverter.Add("decimal", "Decimal");
                typeConverter.Add("boolean", "Boolean");
                typeConverter.Add("bool", "Boolean");
                typeConverter.Add("guid", "Guid");
                typeConverter.Add("date", "DateTime");

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
                    "ReadWrite, ReadOnly, WriteOnly, ReadManyWriteNew " +
                    "and WriteOnce.", _propertyName), ex);
            }
        }

        /// <summary>
        /// Loads the default value from the reader
        /// </summary>
        private void LoadDefaultValue()
        {
            _defaultValueString = _reader.GetAttribute("default");

        }

        /// <summary>
        /// Loads the database field name from the reader
        /// </summary>
        private void LoadDatabaseFieldName()
        {
            _databaseFieldName = _reader.GetAttribute("databaseField");
        }

        /// <summary>
        /// Loads the property description from the reader
        /// </summary>
        private void LoadDescription()
        {
            _description = _reader.GetAttribute("description");
        }

        /// <summary>
        /// Loads the attribute that determines whether the property is compulsory or not
        /// </summary>
        private void LoadCompulsory()
        {
            _compulsory = Convert.ToBoolean(_reader.GetAttribute("compulsory"));
        }

        /// <summary>
        /// Loads the attribute that determines whether this property is auto-incrementing
        /// </summary>
        private void LoadAutoIncrementing()
        {
            _autoIncrementing = Convert.ToBoolean(_reader.GetAttribute("autoIncrementing"));
        }

        /// <summary>
        /// Loads the attribute that determines whether the property must keep its value private or not
        /// </summary>
        private void LoadKeepValuePrivate()
        {
            _keepValuePrivate = Convert.ToBoolean(_reader.GetAttribute("keepValuePrivate"));
        }

        /// <summary>
        /// Loads the length attribute used to set a maximum length for a string
        /// </summary>
        private void LoadLength()
        {
            string length = _reader.GetAttribute("length");
            if (length != null)
            {
                if (!Int32.TryParse(length, out _length))
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In the property definition for '{0}', the 'length' " +
                        "was set to an invalid integer value.", _propertyName));
                }
                if (_length < 0)
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In the property definition for '{0}', the 'length' " +
                        "was set to an invalid negative value.", _propertyName));
                }
                else if (_typeName != "String")
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In the property definition for '{0}', a 'length' " +
                        "attribute was provided for a property type that cannot " +
                        "use the attribute.", _propertyName));
                }
            }
            else
            {
                _length = Int32.MaxValue;
            }
        }
    }
}