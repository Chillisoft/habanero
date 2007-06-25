using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI grid definitions from xml data
    /// </summary>
    public class XmlUIGridDefLoader : XmlLoader
    {
        private UIGridDef _collection;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIGridDefLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlUIGridDefLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
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
			XmlUIGridPropertyLoader propLoader = new XmlUIGridPropertyLoader(_dtdPath, _defClassFactory);
            while (_reader.Name == "uiGridProperty")
            {
                _collection.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            }

            if (_collection.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'uiGridProperty' " +
                    "elements were specified in a 'uiGridDef' element.  Ensure " +
                    "that the element " +
                    "contains one or more 'uiGridProperty' elements, which " +
                    "specify the columns to appear in the grid.");
            }
        }
    }
}