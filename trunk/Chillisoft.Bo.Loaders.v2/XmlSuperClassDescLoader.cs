using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads super class information from xml data
    /// </summary>
    /// TODO ERIC - unclear what desc means - could rename to description
    /// or def or nothing
    public class XmlSuperClassDescLoader : XmlLoader
    {
        private ORMapping itsORMapping;
        private ClassDef itsSuperClassDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlSuperClassDescLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlSuperClassDescLoader()
        {
        }

        /// <summary>
        /// Loads super class information from the given xml string
        /// </summary>
        /// <param name="xmlSuperClassDesc">The xml string containing the
        /// super class definition</param>
        /// <returns>Returns the property rule object</returns>
        public SuperClassDesc LoadSuperClassDesc(string xmlSuperClassDesc)
        {
            return this.LoadSuperClassDesc(this.CreateXmlElement(xmlSuperClassDesc));
        }

        /// <summary>
        /// Loads super class information from the given xml element
        /// </summary>
        /// <param name="xmlSuperClassDesc">The xml element containing the
        /// super class definition</param>
        /// <returns>Returns the property rule object</returns>
        public SuperClassDesc LoadSuperClassDesc(XmlElement xmlSuperClassDesc)
        {
            return (SuperClassDesc) this.Load(xmlSuperClassDesc);
        }

        /// <summary>
        /// Creates a new SuperClassDesc object using the data that
        /// has been loaded for the object
        /// </summary>
        /// <returns>Returns a SuperClassDesc object</returns>
        protected override object Create()
        {
            return new SuperClassDesc(itsSuperClassDef, itsORMapping);
        }

        /// <summary>
        /// Load the class data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            string className = itsReader.GetAttribute("className");
            string assemblyName = itsReader.GetAttribute("assemblyName");
			itsSuperClassDef = ClassDef.GetClassDefCol[assemblyName, className];
			itsORMapping = (ORMapping)Enum.Parse(typeof(ORMapping), itsReader.GetAttribute("orMapping"));
        }
    }
}