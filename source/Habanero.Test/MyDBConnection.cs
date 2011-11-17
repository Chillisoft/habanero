#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using Habanero.DB;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for MyDBConnection.
    /// </summary>
    public class MyDBConnection
    {
        public static String GetConnectionString()
        {
            return GetDatabaseConfig().GetConnectionString();
        }

        public static DatabaseConfig GetDatabaseConfig()
        {
            IDictionary settings = new Hashtable();

            settings.Add("vendor", DatabaseConfig.MySql);
            settings.Add("server", "localhost");
            settings.Add("database", "habanero_test_trunk");
            settings.Add("username", "root");
            settings.Add("password", "root");
            settings.Add("port", "3306");

            DatabaseConfig config = new DatabaseConfig(settings);
            return config;
        }
    }
}