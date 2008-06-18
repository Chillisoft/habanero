//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Uses a variety of xml loaders to load all the facets that make
    /// up a business object class definition
    /// </summary>
    public class XmlClassLoader : XmlLoader
    {
        private PropDefCol _propDefCol;
        private PrimaryKeyDef _primaryKeyDef;
        private KeyDefCol _keyDefCol;
        private RelationshipDefCol _relationshipDefCol;
        private string _className;
        private string _assemblyName;
        private SuperClassDef _superClassDef;
        private UIDefCol _uiDefCol;
        private string _tableName;
        private string _displayName;
        //private bool _SupportsSynchronising;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlClassLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlClassLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads the class data from the xml string provided
        /// </summary>
        /// <param name="xmlClassDef">The xml class definition string.
        /// You can use <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to read a continuous string from a file.</param>
        /// <returns>Returns the class definition loaded</returns>
        public ClassDef LoadClass(string xmlClassDef)
        {
            if (xmlClassDef == null || xmlClassDef.Length == 0)
            {
                throw new ArgumentException("The application is unable to read the " +
                    "'classes' element from the XML class definitions file.  " +
                    "The definitions need one 'classes' root element and a 'class' " +
                    "element for each class that you are mapping.  XML elements are " +
                    "case-sensitive.");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlClassDef);
            return LoadClass(doc.DocumentElement);
        }

        /// <summary>
        /// Loads the class data using the xml element provided
        /// </summary>
        /// <param name="classElement">The xml class element</param>
        /// <returns>Returns the class definition loaded</returns>
        public ClassDef LoadClass(XmlElement classElement)
        {
            return (ClassDef) this.Load(classElement);
        }

        /// <summary>
        /// Creates a class definition using the data loaded from the reader
        /// </summary>
        /// <returns>Returns a class definition</returns>
        protected override object Create()
        {
            ClassDef classDef = _defClassFactory.CreateClassDef(_assemblyName, _className, _displayName, _primaryKeyDef, _propDefCol, 
							 _keyDefCol, _relationshipDefCol, _uiDefCol);
			//ClassDef def =
			//    new ClassDef(_assemblyName,_className, _primaryKeyDef, _propDefCol, 
			//                 _keyDefCol, _relationshipDefCol, _uiDefCol);
			if (_superClassDef != null)
            {
                classDef.SuperClassDef = _superClassDef;
            }
            if (_tableName != null && _tableName.Length > 0)
            {
                classDef.TableName = _tableName;
            }
            foreach (PropDef propDef in classDef.PropDefcol)
            {
                propDef.ClassDef = classDef;
            }
            //def.SupportsSynchronising = _SupportsSynchronising;
            return classDef;
        }

        /// <summary>
        /// Loads all relevant data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _superClassDef = null;
            _reader.Read();
            LoadClassInfo();
            LoadTableName();
            LoadDisplayName();
            //LoadSupportsSynchronisation();

            _reader.Read();

            List<string> keyDefXmls = new List<string>();
            List<string> propDefXmls = new List<string>();
            List<string> relationshipDefXmls = new List<string>();
            List<string> uiDefXmls = new List<string>();
            string superclassDescXML = null;
            string primaryKeDefXML = null;
            while (_reader.Name != "class")
            {
				switch (_reader.Name)
				{
					case "superClass":
						superclassDescXML = _reader.ReadOuterXml();
						break;
					case "property":
						propDefXmls.Add(_reader.ReadOuterXml());
						break;
					case "key":
						keyDefXmls.Add(_reader.ReadOuterXml());
						break;
					case "primaryKey":
						primaryKeDefXML = _reader.ReadOuterXml();
						break;
					case "relationship":
						relationshipDefXmls.Add(_reader.ReadOuterXml());
						break;
					case "ui":
						uiDefXmls.Add(_reader.ReadOuterXml());
						break;
					default:
						throw new InvalidXmlDefinitionException("The element '" +
							_reader.Name + "' is not a recognised class " +
							"definition element.  Ensure that you have the correct " +
							"spelling and capitalisation, or see the documentation " +
							"for available options.");
						break;
				}
            }

            LoadSuperClassDesc(superclassDescXML);
            LoadPropDefs(propDefXmls);
            LoadKeyDefs(keyDefXmls);
            LoadPrimaryKeyDef(primaryKeDefXML);
            LoadRelationshipDefs(relationshipDefXmls);
            LoadUIDefs(uiDefXmls);
        }

        /// <summary>
        /// Load the super-class data
        /// </summary>
        private void LoadSuperClassDesc(string xmlDef)
        {
            if (xmlDef != null)
            {
                XmlSuperClassLoader superClassLoader = new XmlSuperClassLoader(DtdLoader, _defClassFactory);
                _superClassDef = superClassLoader.LoadSuperClassDesc(xmlDef);
            }
        }

        /// <summary>
        /// Loads the relationship data
        /// </summary>
        private void LoadRelationshipDefs(List<string> xmlDefs)
        {
			_relationshipDefCol = _defClassFactory.CreateRelationshipDefCol();
			//_relationshipDefCol = new RelationshipDefCol();
            foreach (string relDefXml in xmlDefs)
            {
                XmlRelationshipLoader relationshipLoader = new XmlRelationshipLoader(DtdLoader, _defClassFactory);
                _relationshipDefCol.Add(relationshipLoader.LoadRelationship(relDefXml, _propDefCol));
            }
        }

        /// <summary>
        /// Loads the UIDef data
        /// </summary>
        private void LoadUIDefs(List<string> xmlDefs)
        {
			_uiDefCol = _defClassFactory.CreateUIDefCol();
			//_uiDefCol = new UIDefCol();
            foreach (string uiDefXml in xmlDefs)
            {
                XmlUILoader loader = new XmlUILoader(DtdLoader, _defClassFactory);
                _uiDefCol.Add(loader.LoadUIDef(uiDefXml));
            }
        }

        /// <summary>
        /// Loads the key definition data
        /// </summary>
        private void LoadKeyDefs(List<string> xmlDefs)
        {
			_keyDefCol = _defClassFactory.CreateKeyDefCol();
			//_keyDefCol = new KeyDefCol();
            foreach (string keyDefXml in xmlDefs)
            {
                XmlKeyLoader loader = new XmlKeyLoader(DtdLoader, _defClassFactory);
                _keyDefCol.Add(loader.LoadKey(keyDefXml, _propDefCol));
            }
        }

        /// <summary>
        /// Loads the primary key definition data
        /// </summary>
        private void LoadPrimaryKeyDef(string xmlDef)
        {
            if (xmlDef == null && _superClassDef == null)
            {
                throw new InvalidXmlDefinitionException(String.Format("Could not find a " +
                    "'primaryKey' element in the class definition for the class '{0}'. " +
                    "Each class definition requires a primary key " +
                    "definition, which is composed of one or more property definitions, " +
                    "implying that you will need at least one 'prop' element as " +
                    "well.", _className));
            }
            if (xmlDef != null)
            {
                //_primaryKeyDef = new PrimaryKeyDef();
                XmlPrimaryKeyLoader primaryKeyLoader = new XmlPrimaryKeyLoader(DtdLoader, _defClassFactory);
                _primaryKeyDef = primaryKeyLoader.LoadPrimaryKey(xmlDef, _propDefCol);
                if (_primaryKeyDef == null)
                {
                    throw new InvalidXmlDefinitionException(String.Format("There was an error loading " +
                        "the 'primaryKey' element in the class definition for the class '{0}. '" +
                        "Each class definition requires a primary key " +
                        "definition, which is composed of one or more property definitions, " +
                        "implying that you will need at least one 'prop' element as " +
                        "well.", _className));
                }
            }
        }

        /// <summary>
        /// Loads the property definition data
        /// </summary>
        private void LoadPropDefs(List<string> xmlDefs)
        {
            if (xmlDefs.Count == 0 && _superClassDef == null)
            {
                throw new InvalidXmlDefinitionException(String.Format("No property " +
                    "definitions have been specified for the class definition of '{0}'. " +
                    "Each class requires at least one 'property' and 'primaryKey' " +
                    "element which define the mapping from the database table fields to " +
                    "properties in the class that is being mapped to.", _className));
            }
			_propDefCol = _defClassFactory.CreatePropDefCol();
            foreach (string propDefXml in xmlDefs)
            {
                XmlPropertyLoader propLoader = new XmlPropertyLoader(DtdLoader, _defClassFactory);
                PropDef propDef = propLoader.LoadProperty(propDefXml);
                _propDefCol.Add(propDef);
            }
        }

        /// <summary>
        /// Loads the table name attribute
        /// </summary>
        private void LoadTableName()
        {
            _tableName = _reader.GetAttribute("table");
        }

        /// <summary>
        /// Loads the display name attribute
        /// </summary>
        private void LoadDisplayName()
        {
            _displayName = _reader.GetAttribute("displayName");
        }

        /// <summary>
        /// Loads the class type info data
        /// </summary>
        private void LoadClassInfo()
        {
            _className = _reader.GetAttribute("name");
            _assemblyName = _reader.GetAttribute("assembly");

            if (_assemblyName == null || _assemblyName.Length == 0)
            {
                string errorMessage = "No assembly name has been specified for the " +
                                      "class definition";
                if (_className != null && _className.Length > 0)
                {
                    errorMessage += " of '" + _className + "'";
                }
                errorMessage += ". Within each 'class' element you need to set " +
                                "the 'assembly' attribute, which refers to the project or assembly " +
                                "that contains the class which is being mapped to.";
                throw new XmlException(errorMessage);
            }
            if (_className == null || _className.Length == 0)
            {
                throw new XmlException("No 'name' attribute has been specified for a " +
                   "'class' element.  The 'name' attribute indicates the name of the " +
                   "class to which a database table will be mapped.");
            }
        }

        ///// <summary>
        ///// Loads the attribute that indicates whether synchronisation is
        ///// supported
        ///// </summary>
        //private void LoadSupportsSynchronisation()
        //{
        //    string supportsSynch = _reader.GetAttribute("supportsSynchronising");
        //    try
        //    {
        //        _SupportsSynchronising = Convert.ToBoolean(supportsSynch);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidXmlDefinitionException(String.Format(
        //            "In the class definition for '{0}', the value provided for " +
        //            "the 'supportsSynchronising' attribute is not valid. The value " +
        //            "needs to be 'true' or 'false'.", _className));
        //    }
        //}
    }
}
