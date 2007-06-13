using System;
using System.Xml;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI form grid information from xml data
    /// </summary>
    public class XmlUIFormGridLoader : XmlLoader
    {
        private string _relationshipName;
        private Type _gridType;
        private string _correspondingRelationshipName;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIFormGridLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormGridLoader()
        {
        }

        /// <summary>
        /// Loads a form grid definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIFormGrid">The xml string</param>
        /// <returns>Returns a UIFormGrid object</returns>
        public UIFormGrid LoadUIFormGrid(string xmlUIFormGrid)
        {
            return this.LoadUIFormGrid(this.CreateXmlElement(xmlUIFormGrid));
        }

        /// <summary>
        /// Loads a form grid definition from the xml element provided
        /// </summary>
        /// <param name="xmlUIFormGrid">The xml element</param>
        /// <returns>Returns a UIFormGrid object</returns>
        public UIFormGrid LoadUIFormGrid(XmlElement xmlUIFormGrid)
        {
            return (UIFormGrid) Load(xmlUIFormGrid);
        }

        /// <summary>
        /// Creates a form grid definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormGrid object</returns>
        protected override object Create()
        {
            return new UIFormGrid(_relationshipName, _gridType, _correspondingRelationshipName);
        }

        /// <summary>
        /// Loads form grid data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadRelationshipName();
            LoadGridType();
            LoadCorrespondingRelationshipName();
        }

        /// <summary>
        /// Loads the corresponding relationship name from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadCorrespondingRelationshipName()
        {
            _correspondingRelationshipName = _reader.GetAttribute("correspondingRelationshipName");
        }

        /// <summary>
        /// Loads the grid type from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadGridType()
        {
            string className = _reader.GetAttribute("gridType");
            string assemblyName = _reader.GetAttribute("gridTypeAssembly");
            _gridType = TypeLoader.LoadType(assemblyName, className);
        }

        /// <summary>
        /// Loads the relationship name from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadRelationshipName()
        {
            _relationshipName = _reader.GetAttribute("relationshipName");
        }
    }
}