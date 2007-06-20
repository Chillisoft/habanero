using System.Data;
using Habanero.Generic;

namespace Habanero.Db
{
    /// <summary>
    /// Generates parameter names for parameterised sql statements
    /// </summary>
    public class ParameterNameGenerator : IParameterNameGenerator
    {
        private IDbConnection _connection;
        private int _number;
        private string _parameterNameBase = "Param";
        private string _prefixCharacter;

        /// <summary>
        /// Constructor to initialise a new generator
        /// </summary>
        /// <param name="connection">A database connection</param>
        public ParameterNameGenerator(IDbConnection connection)
        {
            _connection = connection;
            if (_connection == null) {
                _prefixCharacter = "?";
                return;
            }
            string connectionNamespace = _connection.GetType().Namespace;
            if (connectionNamespace == "System.Data.OracleClient")
//            if (connectionNamespace.Substring(0, 17) == "Oracle.DataAccess")
            {
                _prefixCharacter = ":";
            }
            else if (connectionNamespace == "MySql.Data.MySqlClient")
            {
                _prefixCharacter = "?";
            }
            else
            {
                _prefixCharacter = "@";
            }
        }

        /// <summary>
        /// Generates a parameter name with the current seed value and
        /// increments the seed
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetNextParameterName()
        {
            return _prefixCharacter + _parameterNameBase + _number++;
        }

        /// <summary>
        /// Sets the parameter count back to zero
        /// </summary>
        public void Reset()
        {
            _number = 0;
        }
    }
}