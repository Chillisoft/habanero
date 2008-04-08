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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Facilitates the loading of class definitions from xml data, which define
    /// the terms and properties of an xml structure before it can be
    /// populated with data
    /// </summary>
    public class XmlClassDefsLoader : XmlLoader, IClassDefsLoader
    {
		private ClassDefCol _classDefList;
		//private IList _classDefList;
		private string _xmlClassDefs;

        /// <summary>
        /// Constructor to initialise a new loader. If you create the object
        /// with this constructor, you will need to use methods
        /// that provide the actual class definitions at a later stage.
        /// </summary>
        public XmlClassDefsLoader()
        {
        }

		/// <summary>
		/// Constructor to create a new list of class definitions from the
		/// string provided, using the dtd path provided
		/// </summary>
		/// <param name="xmlClassDefs">The string containing all the
		/// class definitions. If you are loading these from 
		/// a file, you can use 
		/// <code>new StreamReader("filename.xml").ReadToEnd()</code>
		/// to create a continuous string.</param>
		/// <param name="dtdLoader">The dtd loader</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader)
			: this(xmlClassDefs, dtdLoader, null)
		{
		}

        /// <summary>
        /// Constructor to create a new list of class definitions from the
        /// string provided, using the dtd path provided
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
            _xmlClassDefs = xmlClassDefs;
        }


        /// <summary>
        /// Loads class definitions, converting them from a 
        /// string containing these definitions to an IList object.
        /// If the conversion fails, an error message will be sent to the 
        /// console.
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
		/// <returns>Returns an IList object containing the definitions</returns>
		public ClassDefCol LoadClassDefs(string xmlClassDefs)
		///// <returns>Returns an IList object containing the definitions</returns>
		//public IList LoadClassDefs(string xmlClassDefs)
{
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlClassDefs);
            }
            catch (XmlException ex)
            {
                //Console.Out.WriteLine(ExceptionUtil.GetExceptionString(ex, 0, true));
                throw new XmlException("The class definitions XML file has no root " +
                    "element 'classes'.  The document needs a master 'classes' element " +
                    "and individual 'class' elements for each of the classes you are " +
                    "defining.", ex);
            }
            return LoadClassDefs(doc.DocumentElement);
        }

		/// <summary>
		/// As with LoadClassDefs(string), but uses the definition string 
		/// provided on instatiation of the object if you used the
		/// parameterised constructor.
		/// </summary>
		/// <returns>Returns a ClassDefCol containing the definitions</returns>
		public ClassDefCol LoadClassDefs()
		///// <returns>Returns an IList object containing the definitions</returns>
		//public IList LoadClassDefs()
		{
			return LoadClassDefs(_xmlClassDefs);
		}
		

        /// <summary>
        /// As with LoadClassDefs(string), but uses the root element as a
        /// starting reference point.
        /// </summary>
        /// <param name="allClassesElement">The root element</param>
		/// <returns>Returns an IList object containing the definitions</returns>
		public ClassDefCol LoadClassDefs(XmlElement allClassesElement)
		///// <returns>Returns an IList object containing the definitions</returns>
		//public IList LoadClassDefs(XmlElement allClassesElement)
		{
            return (ClassDefCol) this.Load(allClassesElement);
        }

        /// <summary>
        /// Returns the IList object that contains the class definitions
        /// </summary>
        /// <returns>Returns an IList object</returns>
        protected override object Create()
        {
            return _classDefList;
        }

        /// <summary>
        /// Loads the class definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
        	_classDefList = _defClassFactory.CreateClassDefCol();
            //_classDefList = new ArrayList();
            _reader.Read();
            _reader.Read();
            XmlClassLoader classLoader = new XmlClassLoader(DtdLoader, _defClassFactory);
            do
            {
                _classDefList.Add(classLoader.LoadClass(_reader.ReadOuterXml()));
            } while (_reader.Name == "class");
            DoPostLoadChecks();
        }

        private void DoPostLoadChecks()
        {
            CheckRelationships();
        }

        private void CheckRelationships()
        {
            Dictionary<ClassDef, PropDefCol> loadedFullPropertyLists = new Dictionary<ClassDef, PropDefCol>();
            foreach (ClassDef classDef in _classDefList)
            {
                CheckRelationships(loadedFullPropertyLists, classDef);
            }
        }

        private void CheckRelationships(Dictionary<ClassDef, PropDefCol> loadedFullPropertyLists, ClassDef classDef)
        {
            if (classDef == null) return;

            PropDefCol allProps;
            allProps = GetAllClassDefProps(loadedFullPropertyLists, classDef);
            foreach (RelationshipDef relationshipDef in classDef.RelationshipDefCol)
            {
                //// Check Related Object
                //if (!_classDefList.Contains(relationshipDef.RelatedObjectAssemblyName, relationshipDef.RelatedObjectClassName))
                //{
                //    throw new InvalidXmlDefinitionException(String.Format(
                //            "In a 'relatedProperty' element for the '{0}' relationship of " +
                //            "the '{1}' class, the property '{2}' given in the " +
                //            "'property' attribute does not exist for the class or for any of it's superclasses. " +
                //            "Either add the property definition or check the spelling and " +
                //            "capitalisation of the specified property.",
                //            relationshipDef.RelationshipName, classDef.ClassName, ownerPropertyName));
                //}
                
                // Check Relationship Properties
                foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
                {
                    string ownerPropertyName = relPropDef.OwnerPropertyName;
                    if (!allProps.Contains(ownerPropertyName))
                    {
                        throw new InvalidXmlDefinitionException(String.Format(
                            "In a 'relatedProperty' element for the '{0}' relationship of " + 
                            "the '{1}' class, the property '{2}' given in the " +
                            "'property' attribute does not exist for the class or for any of it's superclasses. " +
                            "Either add the property definition or check the spelling and " +
                            "capitalisation of the specified property.", 
                            relationshipDef.RelationshipName, classDef.ClassName, ownerPropertyName));
                    }
                }
            }

            
        }

        private PropDefCol GetAllClassDefProps(Dictionary<ClassDef, PropDefCol> loadedFullPropertyLists, ClassDef classDef)
        {
            PropDefCol allProps;
            if (loadedFullPropertyLists.ContainsKey(classDef))
            {
                allProps = loadedFullPropertyLists[classDef];
            }
            else
            {
                allProps = new PropDefCol();
                ClassDef currentClassDef = classDef;
                while (currentClassDef != null)
                {
                    foreach (PropDef propDef in classDef.PropDefcol)
                    {
                        if (!allProps.Contains(propDef.PropertyName))
                        {
                            allProps.Add(propDef);
                        }
                    }
                    currentClassDef = GetSuperClassClassDef(currentClassDef);
                }
                loadedFullPropertyLists.Add(classDef, allProps);
            }
            return allProps;
        }

        private ClassDef GetSuperClassClassDef(ClassDef currentClassDef)
        {
            SuperClassDef superClassDef = currentClassDef.SuperClassDef;
            if (superClassDef != null)
            {
                return _classDefList[superClassDef.AssemblyName, superClassDef.ClassName];
            }
            return null;
        }
    }
}