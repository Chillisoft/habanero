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

            //itsReader.Read();
            //string className = itsReader.GetAttribute("class");
            //string assemblyName = itsReader.GetAttribute("assembly");
            //itsCollection.Class = TypeLoader.LoadType(assemblyName, className);
            //itsCollection.Name = new UIPropertyCollectionName(itsCollection.Class, itsReader.GetAttribute("name"));

            itsReader.Read();
            _column.Width = Convert.ToInt32(itsReader.GetAttribute("width"));
            itsReader.Read();
            XmlUIFormPropertyLoader propLoader = new XmlUIFormPropertyLoader(itsDtdPath);
            do
            {
                _column.Add(propLoader.LoadUIProperty(itsReader.ReadOuterXml()));
            } while (itsReader.Name == "uiFormProperty");
        }
    }
}