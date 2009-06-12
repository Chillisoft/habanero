//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Generates parameter names for parameterised sql statements
    /// </summary>
    internal class ParameterNameGenerator : IParameterNameGenerator
    {
        private readonly IDbConnection _connection;
        private int _number;
        private const string _parameterNameBase = "Param";
        private readonly string _prefixCharacter;

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
            switch (connectionNamespace)
            {
                case "System.Data.OracleClient":
                    _prefixCharacter = ":";
                    break;
                case "Npgsql":
                    _prefixCharacter = ":";
                    break;
                case "System.Data.SQLite":
                    _prefixCharacter = ":";
                    break;
                case "MySql.Data.MySqlClient":
                    _prefixCharacter = "?";
                    break;
                default:
                    _prefixCharacter = connectionNamespace == "FirebirdSql.Data.FirebirdClient" ? "@" : "@";
                    break;
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