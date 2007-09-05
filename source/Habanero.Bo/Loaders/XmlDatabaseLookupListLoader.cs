using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads lookup list data from the reader, using sql information as
    /// specified in the reader
    /// </summary>
    public class XmlDatabaseLookupListLoader : XmlLookupListLoader
    {
        private string _sqlString;
        private int _timeout;
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
        /// Loads the lookup list data from the reader, using the sql string, time-out
        /// class name and assembly name specified in the reader
        /// </summary>
        protected override void LoadLookupListFromReader()
        {
            _sqlString = _reader.GetAttribute("sql");
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");

            if (!Int32.TryParse(_reader.GetAttribute("timeout"), out _timeout) ||
                _timeout < 0)
            {
                throw new InvalidXmlDefinitionException("In a 'databaseLookupList' " +
                    "element, an invalid integer was assigned to the 'timeout' " +
                    "attribute.  The value must be a positive integer or zero.");
            }

        }

        /// <summary>
        /// Creates a database lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a DatabaseLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateDatabaseLookupList(_sqlString, _timeout,_assemblyName, _className);
			//return new DatabaseLookupList(_sqlString, _assemblyName, _className);
		}
    }
}