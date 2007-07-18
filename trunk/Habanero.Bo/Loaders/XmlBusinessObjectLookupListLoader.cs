using System;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads xml data for a lookup list in a business object
    /// </summary>
    public class XmlBusinessObjectLookupListLoader : XmlLookupListSourceLoader
    {
        //private Type _type;
    	private string _className;
    	private string _assemblyName;
        private string _criteria;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlBusinessObjectLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlBusinessObjectLookupListLoader()
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");
            //_type = TypeLoader.LoadType(assemblyName, className);
            _criteria = _reader.GetAttribute("criteria");
        }

        /// <summary>
        /// Creates a business object lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a BusinessObjectLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateBusinessObjectLookupListSource(_assemblyName, _className, _criteria);
			//return new BOLookupListSource(_assemblyName, _className);
			//return new BOLookupListSource(_type);
		}
    }
}
