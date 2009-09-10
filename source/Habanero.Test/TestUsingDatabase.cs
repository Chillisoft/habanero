//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Threading;
using Habanero.BO;
using Habanero.DB;

namespace Habanero.Test
{
    public class TestUsingDatabase : ArchitectureTest
    {
        protected static List<BusinessObject> _objectsToDelete = new List<BusinessObject>();

        public void SetupDBConnection()
        {
            SetupDBDataAccessor();
        }

        public static void SetupDBDataAccessor()
        {
            if (DatabaseConnection.CurrentConnection != null &&
    DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionMySql))
            {
                return;
            }
            DatabaseConnection.CurrentConnection =
                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            DatabaseConnection.CurrentConnection.ConnectionString =
                MyDBConnection.GetDatabaseConfig().GetConnectionString();
            DatabaseConnection.CurrentConnection.GetConnection();

            BORegistry.DataAccessor = new DataAccessorDB();
        }

        public void SetupDBOracleConnection()
        {
            if (DatabaseConnection.CurrentConnection != null &&
                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionOracle))
            {
                return;
            }
            DatabaseConnection.CurrentConnection =
                new DatabaseConnectionOracle("System.Data.OracleClient, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b77a5c561934e089", "System.Data.OracleClient.OracleConnection");
            ConnectionStringOracleFactory oracleConnectionString = new ConnectionStringOracleFactory();
            string connStr = oracleConnectionString.GetConnectionString("core1", "XE", "system", "system", "1521");
            DatabaseConnection.CurrentConnection.ConnectionString = connStr;
            DatabaseConnection.CurrentConnection.GetConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        public static void DeleteObjects()
        {
            DeleteObjects(null);
        }

        public static void DeleteObjects(List<BusinessObject> objectsToDelete)
        {
            if (objectsToDelete == null)
            {
                objectsToDelete = _objectsToDelete;
            }
            int count = objectsToDelete.Count;
            Dictionary<BusinessObject, int> failureHistory = new Dictionary<BusinessObject, int>();
            while (objectsToDelete.Count > 0)
            {
                BusinessObject thisBo = objectsToDelete[objectsToDelete.Count - 1];
                try
                {
                    if (!thisBo.Status.IsNew)
                    {
                        thisBo.CancelEdits();
                        thisBo.MarkForDelete();
                        thisBo.Save();
                    }
                    objectsToDelete.Remove(thisBo);
                }
                catch
                {
                    int failureCount = 0;
                    if (failureHistory.ContainsKey(thisBo))
                    {
                        failureCount = failureHistory[thisBo]++;
                    }
                    else
                    {
                        failureHistory.Add(thisBo, failureCount + 1);
                    }
                    objectsToDelete.Remove(thisBo);
                    if (failureCount <= count)
                    {
                        objectsToDelete.Insert(0, thisBo);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        protected static void AddObjectToDelete(BusinessObject bo)
        {
            _objectsToDelete.Add(bo);
        }
        protected static void WaitForDB()
        {
            Thread.Sleep(10000);
        }
        protected static void WaitForGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

}