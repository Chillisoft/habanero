using System.Xml;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI definitions from xml data
    /// </summary>
    public class XmlUIDefLoader : XmlLoader
    {
        private UIFormDef itsUIFormDef;
        private UIGridDef itsUIGridDef;
        private string itsName;

        //private string itsXmlUICollections;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIDefLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIDefLoader(string dtdPath) : base(dtdPath)
        {
        }


        //		public XmlUIDefLoader(string xmlUICollections) {
        //			itsXmlUICollections = xmlUICollections;
        //		}

        //		public IEnumerable LoadUIPropertyCollections() 
        //		{
        //			return LoadUIDef(itsXmlUICollections );
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
            return new UIDef(itsName, itsUIFormDef, itsUIGridDef);
        }

        /// <summary>
        /// Loads UI definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            itsName = itsReader.GetAttribute("name");
            itsReader.Read();
            if (itsReader.Name == "uiGridDef")
            {
                XmlUIGridDefLoader loader = new XmlUIGridDefLoader(itsDtdPath);
                itsUIGridDef = loader.LoadUIGridDef(itsReader.ReadOuterXml());
            }
            if (itsReader.Name == "uiFormDef")
            {
                XmlUIFormDefLoader loader = new XmlUIFormDefLoader(itsDtdPath);
                itsUIFormDef = loader.LoadUIFormDef(itsReader.ReadOuterXml());
            }
        }
    }
}