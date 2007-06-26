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
    public class XmlDatabaseLookupListLoader : XmlLookupListSourceLoader
    {
        private string _sqlString;
    	private string _assemblyName;
    	private string _className;

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlDatabaseLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a loader
        /// </summary>
        public XmlDatabaseLookupListLoader()
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader, using the sql string,
        /// class name and assembly name specified in the reader
        /// </summary>
        protected override void LoadLookupListSourceFromReader()
        {
            _sqlString = _reader.GetAttribute("sql");
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");
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