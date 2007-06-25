using System;
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads lookup list data from the reader, using sql information as
    /// specified in the reader
    /// </summary>
    public class XmlDatabaseLookupListSourceLoader : XmlLookupListSourceLoader
    {
        private string _sqlString;
    	private string _assemblyName;
    	private string _className;

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlDatabaseLookupListSourceLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a loader
        /// </summary>
        public XmlDatabaseLookupListSourceLoader()
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader, using the sql string,
        /// class name and assembly name specified in the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            _sqlString = _reader.GetAttribute("sqlString");
            _className = _reader.GetAttribute("className");
            _assemblyName = _reader.GetAttribute("assemblyName");
        }

        /// <summary>
        /// Creates a database lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a DatabaseLookupListSource object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateDatabaseLookupListSource(_sqlString, _assemblyName, _className);
			//return new DatabaseLookupListSource(_sqlString, _assemblyName, _className);
		}
    }
}