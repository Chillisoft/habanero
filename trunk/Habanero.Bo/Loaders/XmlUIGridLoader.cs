using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI grid definitions from xml data
    /// </summary>
    public class XmlUIGridLoader : XmlLoader
    {
        private UIGridDef _collection;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIGridLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIGridLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a grid definition from the xml string provided
        /// </summary>
        /// <param name="formDefElement">The xml string</param>
        /// <returns>Returns a UIGridDef object</returns>
        public UIGridDef LoadUIGridDef(string formDefElement)
        {
            return this.LoadUIGridDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a grid definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIGridDef object</returns>
        public UIGridDef LoadUIGridDef(XmlElement formDefElement)
        {
            return (UIGridDef) this.Load(formDefElement);
        }

        /// <summary>
        /// Creates a grid definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        protected override object Create()
        {
            return _collection;
        }

        /// <summary>
        /// Loads grid definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_collection = _defClassFactory.CreateUIGridDef();
			//_collection = new UIGridDef();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _reader.Read();
            XmlUIGridColumnLoader propLoader = new XmlUIGridColumnLoader(DtdLoader, _defClassFactory);
            while (_reader.Name == "column")
            {
                _collection.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            }

            if (_collection.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'column' " +
                    "elements were specified in a 'grid' element.  Ensure " +
                    "that the element " +
                    "contains one or more 'column' elements, which " +
                    "specify the columns to appear in the grid.");
            }
        }
    }
}