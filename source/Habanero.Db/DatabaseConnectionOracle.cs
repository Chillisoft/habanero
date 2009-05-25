//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the Oracle database
    /// </summary>
    public class DatabaseConnectionOracle : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionOracle(string assemblyName, string className)
            : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringOracleFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionOracle(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string LeftFieldDelimiter
        {
            get { return ""; }
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string RightFieldDelimiter
        {
            get { return ""; }
        }
    }
}