using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// A super-class for xml loaders that load lookup list data
    /// </summary>
    public abstract class XmlLookupListSourceLoader : XmlLoader
    {
        protected string _ruleName;
        protected bool _isCompulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlLookupListSourceLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlLookupListSourceLoader()
        {
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as a string</param>
        /// <returns>Returns an ILookupListSource object</returns>
        public ILookupListSource LoadLookupListSource(string sourceElement)
        {
            return this.LoadLookupListSource(this.CreateXmlElement(sourceElement));
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as an XmlElement
        /// object</param>
        /// <returns>Returns an ILookupListSource object</returns>
        public ILookupListSource LoadLookupListSource(XmlElement sourceElement)
        {
            return (ILookupListSource) this.Load(sourceElement);
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            itsReader.Read();
            LoadLookupListSourceFromReader();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected abstract void LoadLookupListSourceFromReader();

        /// <summary>
        /// Loads the lookup list data into the specified property definition
        /// </summary>
        /// <param name="sourceElement">The source element</param>
        /// <param name="def">The property definition to load into</param>
        /// <param name="dtdPath">The dtd path</param>
        public static void LoadLookupListSourceIntoProperty(string sourceElement, PropDef def, string dtdPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sourceElement);
            string loaderClassName = "Xml" + doc.DocumentElement.Name + "Loader";
            Type loaderType =
                Type.GetType(typeof (XmlLookupListSourceLoader).Namespace + "." + loaderClassName, true, true);
            XmlLookupListSourceLoader loader =
                (XmlLookupListSourceLoader) Activator.CreateInstance(loaderType, new object[] {dtdPath});
            def.LookupListSource = loader.LoadLookupListSource(doc.DocumentElement);
        }
    }
}