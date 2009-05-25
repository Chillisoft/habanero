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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads super class information from xml data
    /// </summary>
    public class XmlSuperClassLoader : XmlLoader
    {
        private ORMapping _orMapping;
        private string _className;
    	private string _assemblyName;
        private string _discriminator;
        private string _id;


        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlSuperClassLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlSuperClassLoader()
        {
        }

        /// <summary>
        /// Loads super class information from the given xml string
        /// </summary>
        /// <param name="xmlSuperClassDesc">The xml string containing the
        /// super class definition</param>
        /// <returns>Returns the property rule object</returns>
        public SuperClassDef LoadSuperClassDesc(string xmlSuperClassDesc)
        {
            return this.LoadSuperClassDesc(this.CreateXmlElement(xmlSuperClassDesc));
        }

        /// <summary>
        /// Loads super class information from the given xml element
        /// </summary>
        /// <param name="xmlSuperClassDesc">The xml element containing the
        /// super class definition</param>
        /// <returns>Returns the property rule object</returns>
        public SuperClassDef LoadSuperClassDesc(XmlElement xmlSuperClassDesc)
        {
            return (SuperClassDef) this.Load(xmlSuperClassDesc);
        }

        /// <summary>
        /// Creates a new SuperClassDef object using the data that
        /// has been loaded for the object
        /// </summary>
        /// <returns>Returns a SuperClassDef object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateSuperClassDef(_assemblyName, _className, _orMapping, _id, _discriminator);
			//return new SuperClassDef(_assemblyName, _className, _orMapping);
		}

        /// <summary>
        /// Load the class data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");
			string orMappingType = _reader.GetAttribute("orMapping");
			try
            {
                _orMapping = (ORMapping)Enum.Parse(typeof(ORMapping), orMappingType);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "The specified ORMapping type, '{0}', is not a valid inheritance " +
                    "type.  The valid options are ClassTableInheritance (the default), " +
                    "SingleTableInheritance and ConcreteTableInheritance.", orMappingType), ex);
            }

            _id = _reader.GetAttribute("id");
            if (!String.IsNullOrEmpty(_id) && _orMapping != ORMapping.ClassTableInheritance)
            {
                throw new InvalidXmlDefinitionException("In a superClass definition, an 'id' " +
                    "attribute has been specified for an OR-mapping type other than " +
                    "ClassTableInheritance.");
            }

            _discriminator = _reader.GetAttribute("discriminator");
            if (!String.IsNullOrEmpty(_discriminator) && _orMapping != ORMapping.SingleTableInheritance)
            {
                throw new InvalidXmlDefinitionException("In a superClass definition, a 'discriminator' " +
                    "attribute has been specified for an OR-mapping type other than " +
                    "SingleTableInheritance.");
            }
            else if (_discriminator == null && _orMapping == ORMapping.SingleTableInheritance)
            {
                throw new InvalidXmlDefinitionException("In a superClass definition, a 'discriminator' " +
                    "attribute is missing where the SingleTableInheritance OR-mapping type has been " +
                    "specified.");
            }
        }
    }
}