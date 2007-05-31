using System.Xml;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI form tab information from xml data
    /// </summary>
    public class XmlUIFormTabLoader : XmlLoader
    {
        private UIFormTab itsTab;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormTabLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIFormTabLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Loads a form tab definition from the xml string provided
        /// </summary>
        /// <param name="formTabElement">The xml string</param>
        /// <returns>Returns a UIFormTab object</returns>
        public UIFormTab LoadUIFormTab(string formTabElement)
        {
            return this.LoadUIFormTab(this.CreateXmlElement(formTabElement));
        }

        /// <summary>
        /// Loads a form tab definition from the xml element provided
        /// </summary>
        /// <param name="formTabElement">The xml element</param>
        /// <returns>Returns a UIFormTab object</returns>
        public UIFormTab LoadUIFormTab(XmlElement formTabElement)
        {
            return (UIFormTab) this.Load(formTabElement);
        }

        /// <summary>
        /// Creates a form tab definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormTab object</returns>
        protected override object Create()
        {
            return itsTab;
        }

        /// <summary>
        /// Loads form tab data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsTab = new UIFormTab();

            //itsReader.Read();
            //string className = itsReader.GetAttribute("class");
            //string assemblyName = itsReader.GetAttribute("assembly");
            //itsCollection.Class = TypeLoader.LoadType(assemblyName, className);
            //itsCollection.Name = new UIPropertyCollectionName(itsCollection.Class, itsReader.GetAttribute("name"));

            itsReader.Read();
            itsTab.Name = itsReader.GetAttribute("name");
            itsReader.Read();
            if (itsReader.Name == "uiFormGrid")
            {
                XmlUIFormGridLoader gridLoader = new XmlUIFormGridLoader(itsDtdPath);
                itsTab.UIFormGrid = gridLoader.LoadUIFormGrid(itsReader.ReadOuterXml());
            }
            else
            {
                XmlUIFormColumnLoader loader = new XmlUIFormColumnLoader(itsDtdPath);
                do
                {
                    itsTab.Add(loader.LoadUIFormColumn(itsReader.ReadOuterXml()));
                } while (itsReader.Name == "uiFormColumn");
            }
        }
    }
}