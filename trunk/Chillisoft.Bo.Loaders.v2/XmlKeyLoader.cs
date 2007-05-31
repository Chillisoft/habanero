using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads key definitions from xml data
    /// </summary>
    public class XmlKeyLoader : XmlLoader
    {
        private KeyDef itsKeyDef;
        private PropDefCol itsPropDefCol;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlKeyLoader(string dtdPath) : base(dtdPath)
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
            itsPropDefCol = propDefs;
            return (KeyDef) Load(keyElement);
        }

        /// <summary>
        /// Creates the key definition object from the data read in
        /// </summary>
        /// <returns>Returns a KeyDef object</returns>
        protected override object Create()
        {
            return itsKeyDef;
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            LoadKeyName();

            LoadKeyIgnoreNulls();

            itsReader.Read();
            LoadKeyProperties();
        }

        /// <summary>
        /// Loads the key name
        /// </summary>
        private void LoadKeyName()
        {
            string name = itsReader.GetAttribute("name");
            if (name != null)
            {
                itsKeyDef = new KeyDef(name);
            }
            else
            {
                itsKeyDef = new KeyDef();
            }
        }

        /// <summary>
        /// Loads the attribute that indicates whether to ignore nulls
        /// </summary>
        private void LoadKeyIgnoreNulls()
        {
            itsKeyDef.IgnoreNulls = Convert.ToBoolean(itsReader.GetAttribute("ignoreNulls"));
        }

        /// <summary>
        /// Loads the key properties
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if an error
        /// occurs while loading the properties</exception>
        private void LoadKeyProperties()
        {
            if (itsReader.Name != "prop")
            {
                throw new InvalidXmlDefinitionException("A keyDef node must have one or more prop nodes");
            }
            do
            {
                string propName = itsReader.GetAttribute("name");
                if (itsPropDefCol[propName] != null)
                {
                    itsKeyDef.Add(itsPropDefCol[propName]);
                }
                else
                {
                    throw new InvalidXmlDefinitionException(
                        String.Format("The PropDef named {0} does not exist in the PropDefCol given", propName));
                }
                itsReader.Read();
            } while (itsReader.Name == "prop");
        }
    }
}