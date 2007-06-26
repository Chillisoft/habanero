using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads key definitions from xml data
    /// </summary>
    public class XmlKeyLoader : XmlLoader
    {
        private KeyDef _keyDef;
        private PropDefCol _propDefCol;

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
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlKeyLoader()
        {
        }

        /// <summary>
        /// Loads a key definition from the xml string and property definition
        /// collection provided
        /// </summary>
        /// <param name="xmlKeyDef">The xml key definition string</param>
        /// <param name="propDefs">The collection of property definitions</param>
        /// <returns>Returns the key definition</returns>
        public KeyDef LoadKey(string xmlKeyDef, PropDefCol propDefs)
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
        public KeyDef LoadKey(XmlElement keyElement, PropDefCol propDefs)
        {
            _propDefCol = propDefs;
            return (KeyDef) Load(keyElement);
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

            LoadKeyIgnoreNulls();

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
        /// Loads the attribute that indicates whether to ignore nulls
        /// </summary>
        private void LoadKeyIgnoreNulls()
        {
            try
            {
                _keyDef.IgnoreIfNull = Convert.ToBoolean(_reader.GetAttribute("ignoreIfNull"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'keyDef' element, " +
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
                throw new InvalidXmlDefinitionException("A 'keyDef' node is " +
                    "missing 'prop' nodes. Each key definition specifies " +
                    "limitations on a property, and the prop nodes have a 'name' " +
                    "attribute that specifies which existing property definition " +
                    "is being referred to in the key definition.");
            }
            do
            {
                string propName = _reader.GetAttribute("name");
                if (propName == null || propName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("The 'prop' node " +
                        "under a key definition is missing a valid 'name' attribute, " +
                        "which specifies the name of an existing property definition " +
                        "which is being further defined by the key definition.");
                }
                if (_propDefCol[propName] != null)
                {
                    _keyDef.Add(_propDefCol[propName]);
                }
                else
                {
                    throw new InvalidXmlDefinitionException(
                        String.Format("The property definition '{0}' being named by a " +
                        "'prop' element in a key definition does not exist. The property " +
                        "definition being referred to must have been defined in a " +
                        "'propertyDef' element.  Add the property definition or check " +
                        "that the spelling and capitalisation are correct.", propName));
                }
                ReadAndIgnoreEndTag();

            } while (_reader.Name == "prop");
        }
    }
}