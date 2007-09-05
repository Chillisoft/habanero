using System;
using System.Xml;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// A super-class for xml loaders that load lookup list data
    /// </summary>
    public abstract class XmlLookupListLoader : XmlLoader
    {
        protected string _ruleName;
        protected bool _isCompulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlLookupListLoader()
        {
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as a string</param>
        /// <returns>Returns an ILookupList object</returns>
        public ILookupList LoadLookupList(string sourceElement)
        {
            return this.LoadLookupList(this.CreateXmlElement(sourceElement));
        }

        /// <summary>
        /// Loads a lookup list using the specified source element
        /// </summary>
        /// <param name="sourceElement">The source element as an XmlElement
        /// object</param>
        /// <returns>Returns an ILookupList object</returns>
        public ILookupList LoadLookupList(XmlElement sourceElement)
        {
            return (ILookupList) this.Load(sourceElement);
        }

        /// <summary>
        /// Loads the data from the reader
        /// </summary>
        protected override sealed void LoadFromReader()
        {
            _reader.Read();
            LoadLookupListFromReader();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected abstract void LoadLookupListFromReader();

        /// <summary>
        /// Loads the lookup list data into the specified property definition
        /// </summary>
        /// <param name="sourceElement">The source element</param>
        /// <param name="def">The property definition to load into</param>
        /// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public static void LoadLookupListIntoProperty(string sourceElement, PropDef def, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sourceElement);
            string loaderClassName = "Xml" + doc.DocumentElement.Name + "Loader";
            Type loaderType =
                Type.GetType(typeof (XmlLookupListLoader).Namespace + "." + loaderClassName, true, true);
            XmlLookupListLoader loader =
				(XmlLookupListLoader)Activator.CreateInstance(loaderType, new object[] { dtdLoader, defClassFactory });
            def.LookupList = loader.LoadLookupList(doc.DocumentElement);
        }
    }
}