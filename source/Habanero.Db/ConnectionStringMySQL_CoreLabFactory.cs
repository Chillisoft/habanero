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

using System;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Summary description for ConnectionStringMySQL_CoreLabFactory.
    /// </summary>
    public class ConnectionStringMySQL_CoreLabFactory : ConnectionStringFactory
    {
        public ConnectionStringMySQL_CoreLabFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void CheckArguments(string server, string database, string username, string password,
                                               string port)
        {
            if (server == "" || database == "" || username == "")
            {
                throw new ArgumentException("The server, database and username of a connect string can never be empty.");
            }
        }

        protected override string CreateConnectionString(string server, string database, string username,
                                                         string password, string port)
        {
            if (port == "")
            {
                port = "3306";
            }
            if (password != "")
            {
                return
                    String.Format("User={2}; Host={0}; Port={4}; Database={1}; Password={3};", server, database,
                                  username, password, port);
            }
            else
            {
                return String.Format("User={2}; Host={0}; Port={3}; Database={1};", server, database, username, port);
            }
        }
    }
}