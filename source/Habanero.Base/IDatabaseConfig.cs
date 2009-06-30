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

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that stores database configuration 
    /// settings and creates connections using these settings
    /// </summary>
    public interface IDatabaseConfig
    {
        /// <summary>
        /// Gets and sets access to the database vendor setting
        /// </summary>
        string Vendor { get; set; }

        /// <summary>
        /// Gets and sets access to the database server setting
        /// </summary>
        string Server { get; set; }

        /// <summary>
        /// Gets and sets access to the database name setting
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// Gets and sets access to the username setting
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets and sets access to the password setting
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets and sets access to the port setting
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Returns a connection string tailored for the database vendor
        /// </summary>
        /// <returns>Returns a connection string</returns>
        String GetConnectionString();

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        IDatabaseConnection GetDatabaseConnection();
    }
}