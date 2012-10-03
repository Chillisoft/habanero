using System.Data;

namespace Habanero.Base
{
    /// <summary>
    /// Database parameter container
    /// </summary>
    public class Param
    {
        private readonly DbType _dbType;
        private readonly string _paramName;
        private readonly string _paramValue;

        /// <summary>
        /// Database parameter container constructor
        /// </summary>
        /// <param name="dbType">Type of database</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="paramValue">Parameter value</param>
        public Param(DbType dbType, string paramName, string paramValue)
        {
            _dbType = dbType;
            _paramName = paramName;
            _paramValue = paramValue;
        }

        /// <summary>
        /// Read-only access to database type this parameter is bound to
        /// </summary>
        public DbType DbType
        {
            get { return _dbType; }
        }

        /// <summary>
        /// Read-only access to the name of the parameter this is bound to
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }

        /// <summary>
        /// Read-only access to the value of the parameter this is bound to
        /// </summary>
        public string ParamValue
        {
            get { return _paramValue; }
        }
    }
}