using System;
using System.Xml;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI form column information from xml data
    /// </summary>
    public class XmlUIFormColumnLoader : XmlLoader
    {
        private UIFormColumn _column;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIFormColumnLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormColumnLoader()
        {
        }

        /// <summary>
        /// Loads a form column definition from the xml string provided
        /// </summary>
        /// <param name="formColumnElement">The xml string</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public UIFormColumn LoadUIFormColumn(string formColumnElement)
        {
            return this.LoadUIFormColumn(this.CreateXmlElement(formColumnElement));
        }

        /// <summary>
        /// Loads a form column definition from the xml element provided
        /// </summary>
        /// <param name="formColumnElement">The xml element</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public UIFormColumn LoadUIFormColumn(XmlElement formColumnElement)
        {
            return (UIFormColumn) this.Load(formColumnElement);
        }

        /// <summary>
        /// Creates a form column definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormColumn object</returns>
        protected override object Create()
        {
            return _column;
        }

        /// <summary>
        /// Loads form column data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _column = new UIFormColumn();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _column.Width = Convert.ToInt32(_reader.GetAttribute("width"));
            _reader.Read();
            XmlUIFormPropertyLoader propLoader = new XmlUIFormPropertyLoader(_dtdPath);
            do
            {
                _column.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            } while (_reader.Name == "uiFormProperty");
        }
    }
}