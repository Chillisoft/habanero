using System;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.BO.Loaders
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
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormGridLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
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
			return _defClassFactory.CreateUIFormGrid(_relationshipName, _gridType, _correspondingRelationshipName);
			//return new UIFormGrid(_relationshipName, _gridType, _correspondingRelationshipName);
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
            _correspondingRelationshipName = _reader.GetAttribute("reverseRelationship");
        }

        /// <summary>
        /// Loads the grid type from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadGridType()
        {
            string className = "Habanero.UI.Grid.EditableGrid"; //"_reader.GetAttribute("gridType");
            string assemblyName = "Habanero.UI.Pro";
            try
            {
                _gridType = TypeLoader.LoadType(assemblyName, className);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "While attempting to load a 'formGrid' element, an " +
                    "error occurred while loading the grid type. " +
                    "The type supplied was '{0}' and the assembly was '{1}'. " +
                    "Please ensure that the type exists in the assembly provided.",
                    className, assemblyName), ex);
            }
        }

        /// <summary>
        /// Loads the relationship name from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadRelationshipName()
        {
            _relationshipName = _reader.GetAttribute("relationship");
        }
    }
}