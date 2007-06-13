using System;
using Chillisoft.Bo.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads lookup list data from the reader, using sql information as
    /// specified in the reader
    /// </summary>
    public class XmlDatabaseLookupListSourceLoader : XmlLookupListSourceLoader
    {
        private string _sqlString;
        private Type _boType;

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlDatabaseLookupListSourceLoader(string dtdPath) : base(dtdPath)
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
            _sqlString = itsReader.GetAttribute("sqlString");
            string className = itsReader.GetAttribute("className");
            string assemblyName = itsReader.GetAttribute("assemblyName");
            if (className != null && className.Length > 0 && assemblyName != null && assemblyName.Length > 0)
            {
                _boType = TypeLoader.LoadType(assemblyName, className);
                if (_boType == null)
                {
                    throw new TypeLoadException("The type with class name " + className + " and assembly " +
                                                assemblyName +
                                                " was not found when creating a database lookup list source");
                }
            }
        }

        /// <summary>
        /// Creates a database lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a DatabaseLookupListSource object</returns>
        protected override object Create()
        {
            return new DatabaseLookupListSource(_sqlString, _boType);
        }
    }
}