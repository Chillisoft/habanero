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

using System;
using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the Microsoft Access database
    /// </summary>
    public class DatabaseConnectionAccess : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionAccess(string assemblyName, string className) : base(assemblyName, className)
        {
            _sqlFormatter = new Base.SqlFormatter("[", "]", "TOP", "");
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringAccessFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionAccess(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
            _sqlFormatter = new SqlFormatter("[", "]", "TOP", "");
        }

        /// <summary>
        /// Gets the IsolationLevel to use for this connection
        /// </summary>
        public override IsolationLevel IsolationLevel
        {
            get { return IsolationLevel.ReadUncommitted; }
        }

        /// <summary>
        /// Creates an <see cref="IParameterNameGenerator"/> for this database connection.  This is used to create names for parameters
        /// added to an <see cref="ISqlStatement"/> because each database uses a different naming convention for their parameters.
        /// </summary>
        /// <returns>The <see cref="IParameterNameGenerator"/> valid for this <see cref="IDatabaseConnection"/></returns>
        public override IParameterNameGenerator CreateParameterNameGenerator() {
            return new ParameterNameGenerator("@");
        }
    }
}