//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads primary key information from xml data
    /// </summary>
    public class XmlPrimaryKeyLoader : XmlLoader
    {
        private PrimaryKeyDef _primaryKeyDef;
        private PropDefCol _propDefCol;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlPrimaryKeyLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {

        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPrimaryKeyLoader()
        {
        }

        /// <summary>
        /// Loads a primary key definition from the xml string and property
        /// definition collection provided
        /// </summary>
        /// <param name="xmlPrimaryKeyDef">The xml string</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns the primary key definition</returns>
        internal PrimaryKeyDef LoadPrimaryKey(string xmlPrimaryKeyDef, PropDefCol propDefs)
        {
            return LoadPrimaryKey(this.CreateXmlElement(xmlPrimaryKeyDef), propDefs);
        }

        /// <summary>
        /// Loads a primary key definition from the xml element and property
        /// definition collection provided
        /// </summary>
        /// <param name="primaryKeyElement">The xml element</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns the primary key definition</returns>
        internal PrimaryKeyDef LoadPrimaryKey(XmlElement primaryKeyElement, PropDefCol propDefs)
        {
            _propDefCol = propDefs;
            return (PrimaryKeyDef) Load(primaryKeyElement);
        }

        /// <summary>
        /// Creates the primary key definition from the data loaded
        /// </summary>
        /// <returns>Returns a PrimaryKeyDef object</returns>
        protected override object Create()
        {
            return _primaryKeyDef;
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_primaryKeyDef = _defClassFactory.CreatePrimaryKeyDef();
			//_primaryKeyDef = new PrimaryKeyDef();

            _reader.Read();
            LoadIsObjectID();
            _reader.Read();
            LoadPropertyDefs();
        }

        /// <summary>
        /// Loads the "isObjectID" attribute from the reader
        /// </summary>
        private void LoadIsObjectID()
        {
            if (_reader.GetAttribute("isObjectID") == "true")
            {
                _primaryKeyDef.IsGuidObjectID = true;
            }
            else
            {
                _primaryKeyDef.IsGuidObjectID = false;
            }
        }

        /// <summary>
        /// Loads the property definitions from the reader
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if an
        /// error occurs while loading the data</exception>
        private void LoadPropertyDefs()
        {
            if (_reader.Name != "prop")
            {
                throw new InvalidXmlDefinitionException("A primaryKey node must have one or more prop nodes");
            }
            do
            {
                string propName = _reader.GetAttribute("name");
                if (propName == null || propName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("The 'prop' element " +
                        "must have a 'name' attribute that provides the name of the " +
                        "property definition that serves as the primary key.");
                }
                if (_propDefCol.Contains(propName))
                {
                    _primaryKeyDef.Add(_propDefCol[propName]);
                }
                else
                {
                    throw new InvalidXmlDefinitionException(
                        String.Format("A primary key definition has listed a 'prop' " +
                        "definition for '{0}', which hasn't been defined among " +
                        "the 'property' elements for the class.  Either add a 'property' " +
                        "for '{0}' or correct the spelling or capitalisation of the " +
                        "attribute to match a property that has already been defined.",
                        propName));
                }
                ReadAndIgnoreEndTag();

            } while (_reader.Name == "prop");
        }
    }
}