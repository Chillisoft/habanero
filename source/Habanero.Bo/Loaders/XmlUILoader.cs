using System.Xml;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI definitions from xml data
    /// </summary>
    public class XmlUILoader : XmlLoader
    {
        private UIForm _uiForm;
        private UIGrid _uiGrid;
        private string _name;

        //private string _xmlUICollections;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUILoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUILoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }


        //		public XmlUILoader(string xmlUICollections) {
        //			_xmlUICollections = xmlUICollections;
        //		}

        //		public IEnumerable LoadUIPropertyCollections() 
        //		{
        //			return LoadUIDef(_xmlUICollections );
        //		}

        /// <summary>
        /// Loads a UI definition from the xml string provided
        /// </summary>
        /// <param name="uiDefElement">The xml string</param>
        /// <returns>Returns the UI definition object</returns>
        public UIDef LoadUIDef(string uiDefElement)
        {
            return LoadUIDef(this.CreateXmlElement(uiDefElement));
        }

        /// <summary>
        /// Loads a UI definition from the xml element provided
        /// </summary>
        /// <param name="uiDefElement">The xml element</param>
        /// <returns>Returns the UI definition object</returns>
        public UIDef LoadUIDef(XmlElement uiDefElement)
        {
            return (UIDef) this.Load(uiDefElement);
        }

        /// <summary>
        /// Creates a UI definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIDef object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateUIDef(_name, _uiForm, _uiGrid);
			//return new UIDef(_name, _uiFormDef, _uiGridDef);
        }

        /// <summary>
        /// Loads UI definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            _name = _reader.GetAttribute("name");
            _reader.Read();
            if (_reader.Name == "grid")
            {
                XmlUIGridLoader loader = new XmlUIGridLoader(DtdLoader, _defClassFactory);
                _uiGrid = loader.LoadUIGridDef(_reader.ReadOuterXml());
            }
            if (_reader.Name == "form")
            {
                XmlUIFormLoader loader = new XmlUIFormLoader(DtdLoader, _defClassFactory);
                _uiForm = loader.LoadUIFormDef(_reader.ReadOuterXml());
            }
        }
    }
}