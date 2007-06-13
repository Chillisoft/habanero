using System;
using System.Xml;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI form definitions from xml data
    /// </summary>
    public class XmlUIFormDefLoader : XmlLoader
    {
        private UIFormDef _uiFormDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIFormDefLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormDefLoader()
        {
        }

        /// <summary>
        /// Loads a form definition from the xml string provided
        /// </summary>
        /// <param name="formDefElement">The xml string</param>
        /// <returns>Returns a UIFormDef object</returns>
        public UIFormDef LoadUIFormDef(string formDefElement)
        {
            return this.LoadUIFormDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a form definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIFormDef object</returns>
        public UIFormDef LoadUIFormDef(XmlElement formDefElement)
        {
            return (UIFormDef) this.Load(formDefElement);
        }

        /// <summary>
        /// Creates a form definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormDef object</returns>
        protected override object Create()
        {
            return _uiFormDef;
        }

        /// <summary>
        /// Loads form definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _uiFormDef = new UIFormDef();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //itsCollection.Name = new UIPropertyCollectionName(itsCollection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _uiFormDef.Width = Convert.ToInt32(_reader.GetAttribute("width"));
            _uiFormDef.Height = Convert.ToInt32(_reader.GetAttribute("height"));
            _uiFormDef.Heading = _reader.GetAttribute("heading");

            _reader.Read();
            XmlUIFormTabLoader loader = new XmlUIFormTabLoader(_dtdPath);
            do
            {
                _uiFormDef.Add(loader.LoadUIFormTab(_reader.ReadOuterXml()));
            } while (_reader.Name == "uiFormTab");
        }
    }
}