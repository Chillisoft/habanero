using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads primary key information from xml data
    /// </summary>
    public class XmlPrimaryKeyLoader : XmlLoader
    {
        private PrimaryKeyDef itsPrimaryKeyDef;
        private PropDefCol itsPropDefCol;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPrimaryKeyLoader(string dtdPath) : base(dtdPath)
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
        public PrimaryKeyDef LoadPrimaryKey(string xmlPrimaryKeyDef, PropDefCol propDefs)
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
        public PrimaryKeyDef LoadPrimaryKey(XmlElement primaryKeyElement, PropDefCol propDefs)
        {
            itsPropDefCol = propDefs;
            return (PrimaryKeyDef) Load(primaryKeyElement);
        }

        /// <summary>
        /// Creates the primary key definition from the data loaded
        /// </summary>
        /// <returns>Returns a PrimaryKeyDef object</returns>
        protected override object Create()
        {
            return itsPrimaryKeyDef;
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsPrimaryKeyDef = new PrimaryKeyDef();

            itsReader.Read();
            LoadIsObjectID();
            itsReader.Read();
            LoadPropertyDefs();
        }

        /// <summary>
        /// Loads the "isObjectID" attribute from the reader
        /// </summary>
        private void LoadIsObjectID()
        {
            if (itsReader.GetAttribute("isObjectID") == "true")
            {
                itsPrimaryKeyDef.IsObjectID = true;
            }
            else
            {
                itsPrimaryKeyDef.IsObjectID = false;
            }
        }

        /// <summary>
        /// Loads the property definitions from the reader
        /// </summary>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if an
        /// error occurs while loading the data</exception>
        private void LoadPropertyDefs()
        {
            if (itsReader.Name != "prop")
            {
                throw new InvalidXmlDefinitionException("A primaryKeyDef node must have one or more prop nodes");
            }
            do
            {
                string propName = itsReader.GetAttribute("name");
                if (itsPropDefCol[propName] != null)
                {
                    itsPrimaryKeyDef.Add(itsPropDefCol[propName]);
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