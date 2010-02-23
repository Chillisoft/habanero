// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads key definitions from xml data
    /// </summary>
    public class XmlKeyLoader : XmlLoader
    {
        private IKeyDef _keyDef;
        private IPropDefCol _propDefCol;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlKeyLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a key definition from the xml string and property definition
        /// collection provided
        /// </summary>
        /// <param name="xmlKeyDef">The xml key definition string</param>
        /// <param name="propDefs">The collection of property definitions</param>
        /// <returns>Returns the key definition</returns>
        public IKeyDef LoadKey(string xmlKeyDef, IPropDefCol propDefs)
        {
            return LoadKey(this.CreateXmlElement(xmlKeyDef), propDefs);
        }

        /// <summary>
        /// Loads a key definition from the xml element and property definition
        /// collection provided
        /// </summary>
        /// <param name="keyElement">The xml key definition element</param>
        /// <param name="propDefs">The collection of property definitions</param>
        /// <returns>Returns the key definition</returns>
        public IKeyDef LoadKey(XmlElement keyElement, IPropDefCol propDefs)
        {
            _propDefCol = propDefs;
            return (IKeyDef) Load(keyElement);
        }

        /// <summary>
        /// Creates the key definition object from the data read in
        /// </summary>
        /// <returns>Returns a KeyDef object</returns>
        protected override object Create()
        {
            return _keyDef;
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadKeyName();

            LoadKeyIgnoreIfNull();
            LoadMessage();

            _reader.Read();
            LoadKeyProperties();
        }

        /// <summary>
        /// Loads the key name
        /// </summary>
        private void LoadKeyName()
        {
            string name = _reader.GetAttribute("name");
			_keyDef = _defClassFactory.CreateKeyDef(name);
			//if (name != null)
			//{
			//    _keyDef = new KeyDef(name);
			//}
			//else
			//{
			//    _keyDef = new KeyDef();
			//}
        }

        /// <summary>
        /// Loads the message to display to the user
        /// </summary>
        private void LoadMessage()
        {
            _keyDef.Message = _reader.GetAttribute("message");
        }

        /// <summary>
        /// Loads the 'ignoreIfNull' attribute
        /// </summary>
        private void LoadKeyIgnoreIfNull()
        {
            try
            {
                _keyDef.IgnoreIfNull = Convert.ToBoolean(_reader.GetAttribute("ignoreIfNull"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'key' element, " +
                    "the 'ignoreIfNull' attribute provided " +
                    "an invalid boolean value. Use 'true' or 'false'.", ex);
            }
        }

        /// <summary>
        /// Loads the key properties
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if an error
        /// occurs while loading the properties</exception>
        private void LoadKeyProperties()
        {
            if (_reader.Name != "prop")
            {
                throw new InvalidXmlDefinitionException("A 'key' node is " +
                    "missing 'prop' nodes. Each key definition specifies " +
                    "a combination of one or more listed properties that " +
                    "must be unique for each row.  The 'prop' elements " +
                    "each have a 'name' " +
                    "attribute that specifies which existing property definition " +
                    "make up the alternate key.");
            }
            do
            {
                string propName = _reader.GetAttribute("name");
                if (string.IsNullOrEmpty(propName))
                {
                    throw new InvalidXmlDefinitionException("The 'prop' node " +
                        "under a key definition is missing a valid 'name' attribute, " +
                        "which specifies the name of an existing property definition " +
                        "which makes up the alternate key.");
                }
                if (_propDefCol.Contains(propName))
                {
                    _keyDef.Add(_propDefCol[propName]);
                }
                else
                {
                    IPropDef tempKeyPropDef = _defClassFactory.CreatePropDef(propName, "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false);
                    _keyDef.Add(tempKeyPropDef);
                    //This error was moved to the XmlClassDefsLoader.DoPostLoadChecks method so that it handles inherited properties
                    //throw new InvalidXmlDefinitionException(
                    //    String.Format("The property definition '{0}' being named by a " +
                    //    "'prop' element in a key definition does not exist. The property " +
                    //    "definition being referred to must have been defined in a " +
                    //    "'property' element.  Add the property definition or check " +
                    //    "that the spelling and capitalisation are correct.", propName));
                }
                ReadAndIgnoreEndTag();

            } while (_reader.Name == "prop");
        }
    }
}