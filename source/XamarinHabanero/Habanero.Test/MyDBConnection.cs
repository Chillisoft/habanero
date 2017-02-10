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
using Habanero.Base.Util;
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

        public static DatabaseConfig GetDatabaseConfig(string vendor = "")
        {
            if (string.IsNullOrWhiteSpace(vendor)) vendor = DefaultTestsDbSuffix;
            var suffix = string.IsNullOrWhiteSpace(vendor) ? string.Empty : "_" + vendor.ToUpper();
            var databaseConfig = DatabaseConfig.ReadFromConfigFile("DatabaseConfig" + suffix);
            if (databaseConfig.Vendor.ToUpper() == DatabaseConfig.MySql)
            {
                //Fix to prevent problems running tests with stored procedures
                databaseConfig.Database = databaseConfig.Database.ToLower();
            }
            return databaseConfig;
        }

        private static string DefaultTestsDbSuffix
        {
            get { return ConfigurationManager.AppSettings["DefaultTestsDBSuffix"]; }
        }
    }
}