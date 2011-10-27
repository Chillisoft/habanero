//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
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
        /// The name of the Assembly to use - the assembly that contains the IDbConnection class for this database type.
        /// This does not need to be specified normally, but if you want to use a custom data provider you will need to 
        /// set this property before using the <see cref="IDatabaseConnectionFactory"/> to create the <see cref="IDatabaseConnection"/>.
        /// This must be the full name of the assembly if you are to be sure to get the right assembly.  Alternately if the dll is
        /// placed in the same folder as the application you can just specify the name of the file (without the .dll extension).
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// The fully qualified name of the type to use when creating the IDbConnection.
        /// This does not need to be specified normally, but if you want to use a custom data provider you will need to 
        /// set this property before using the <see cref="IDatabaseConnectionFactory"/> to create the <see cref="IDatabaseConnection"/>.
        /// This class must exist withing the assembly specified in the <see cref="AssemblyName"/> property, and be fully qualified
        /// i.e. it must include the namespace.
        /// </summary>
        string FullClassName { get; set; }

        /// <summary>
        /// The full assembly name of the assembly containing <see cref="IConnectionStringFactory"/> to use.
        /// This does not need to be specified if you are using one of the standard Habanero database types.
        /// </summary>
        string ConnectionStringFactoryAssemblyName { get; set; }

        /// <summary>
        /// The fully qualified class name of the <see cref="IConnectionStringFactory"/> to use.
        /// This does not need to be specified if you are using one of the standard Habanero database types.
        /// </summary>
        string ConnectionStringFactoryClassName { get; set; }

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