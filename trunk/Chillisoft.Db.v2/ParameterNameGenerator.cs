using System.Data;
using Chillisoft.Generic.v2;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Generates parameter names for parameterised sql statements
    /// </summary>
    public class ParameterNameGenerator : IParameterNameGenerator
    {
        private IDbConnection mConnection;
        private int mNumber;
        private string mParameterNameBase = "Param";
        private string mPrefixCharacter;

        /// <summary>
        /// Constructor to initialise a new generator
        /// </summary>
        /// <param name="connection">A database connection</param>
        public ParameterNameGenerator(IDbConnection connection)
        {
            mConnection = connection;
            if (mConnection == null) {
                mPrefixCharacter = "?";
                return;
            }
            string connectionNamespace = mConnection.GetType().Namespace;
            if (connectionNamespace == "System.Data.OracleClient")
//            if (connectionNamespace.Substring(0, 17) == "Oracle.DataAccess")
            {
                mPrefixCharacter = ":";
            }
            else if (connectionNamespace == "MySql.Data.MySqlClient")
            {
                mPrefixCharacter = "?";
            }
            else
            {
                mPrefixCharacter = "@";
            }
        }

        /// <summary>
        /// Generates a parameter name with the current seed value and
        /// increments the seed
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetNextParameterName()
        {
            return mPrefixCharacter + mParameterNameBase + mNumber++;
        }

        /// <summary>
        /// Sets the parameter count back to zero
        /// </summary>
        public void Reset()
        {
            mNumber = 0;
        }
    }
}