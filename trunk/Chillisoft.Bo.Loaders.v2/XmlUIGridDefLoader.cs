using System.Xml;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
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
        public XmlUIGridDefLoader(string dtdPath) : base(dtdPath)
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
            _collection = new UIGridDef();

            //itsReader.Read();
            //string className = itsReader.GetAttribute("class");
            //string assemblyName = itsReader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, itsReader.GetAttribute("name"));

            itsReader.Read();
            itsReader.Read();
            XmlUIGridPropertyLoader propLoader = new XmlUIGridPropertyLoader(itsDtdPath);
            do
            {
                _collection.Add(propLoader.LoadUIProperty(itsReader.ReadOuterXml()));
            } while (itsReader.Name == "uiGridProperty");
        }
    }
}