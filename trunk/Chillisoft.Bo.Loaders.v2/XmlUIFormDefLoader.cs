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
        private UIFormDef itsUIFormDef;

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
            return itsUIFormDef;
        }

        /// <summary>
        /// Loads form definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsUIFormDef = new UIFormDef();

            //itsReader.Read();
            //string className = itsReader.GetAttribute("class");
            //string assemblyName = itsReader.GetAttribute("assembly");
            //itsCollection.Class = TypeLoader.LoadType(assemblyName, className);
            //itsCollection.Name = new UIPropertyCollectionName(itsCollection.Class, itsReader.GetAttribute("name"));

            itsReader.Read();
            itsUIFormDef.Width = Convert.ToInt32(itsReader.GetAttribute("width"));
            itsUIFormDef.Height = Convert.ToInt32(itsReader.GetAttribute("height"));
            itsUIFormDef.Heading = itsReader.GetAttribute("heading");

            itsReader.Read();
            XmlUIFormTabLoader loader = new XmlUIFormTabLoader(itsDtdPath);
            do
            {
                itsUIFormDef.Add(loader.LoadUIFormTab(itsReader.ReadOuterXml()));
            } while (itsReader.Name == "uiFormTab");
        }
    }
}