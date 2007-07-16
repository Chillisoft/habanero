using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
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
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlLookupListSourceLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
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
        public ILookupList LoadLookupListSource(string sourceElement)
        {
            return this.LoadLookupListSource(this.CreateXmlElement(sourceElement));
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as an XmlElement
        /// object</param>
        /// <returns>Returns an ILookupListSource object</returns>
        public ILookupList LoadLookupListSource(XmlElement sourceElement)
        {
            return (ILookupList) this.Load(sourceElement);
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            _reader.Read();
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
        /// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public static void LoadLookupListSourceIntoProperty(string sourceElement, PropDef def, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sourceElement);
            string loaderClassName = "Xml" + doc.DocumentElement.Name + "Loader";
            Type loaderType =
                Type.GetType(typeof (XmlLookupListSourceLoader).Namespace + "." + loaderClassName, true, true);
            XmlLookupListSourceLoader loader =
				(XmlLookupListSourceLoader)Activator.CreateInstance(loaderType, new object[] { dtdLoader, defClassFactory });
            def.LookupList = loader.LoadLookupListSource(doc.DocumentElement);
        }
    }
}