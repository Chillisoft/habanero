using System;
using Habanero.Bo;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads xml data for a lookup list in a business object
    /// </summary>
    public class XmlBusinessObjectLookupListSourceLoader : XmlLookupListSourceLoader
    {
        //private Type _type;
    	private string _className;
    	private string _assemblyName;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlBusinessObjectLookupListSourceLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlBusinessObjectLookupListSourceLoader()
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            _className = _reader.GetAttribute("className");
            _assemblyName = _reader.GetAttribute("assemblyName");
            //_type = TypeLoader.LoadType(assemblyName, className);
        }

        /// <summary>
        /// Creates a business object lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a BusinessObjectLookupListSource object</returns>
        protected override object Create()
        {
			return new BOLookupListSource(_assemblyName, _className);
			//return new BOLookupListSource(_type);
		}
    }
}