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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Habanero.BO;
using Habanero.Base;
using Habanero.DB;
using Habanero.Test.Migrations;

namespace Habanero.Test
{
    public class TestUsingDatabase : ArchitectureTest
    {
        protected static List<BusinessObject> _objectsToDelete = new List<BusinessObject>();

        public void SetupDBConnection(string vendor = "")
        {
            SetupDBDataAccessor(vendor);
        }

        public static void SetupDBDataAccessor(string vendor = "")
        {
            SetupDatabaseConnection(vendor);
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        private static void SetupDatabaseConnection(string vendor)
        {
            var databaseConnection = CreateDatabaseConnection(vendor);
            DatabaseConnection.CurrentConnection = databaseConnection;
            TestConnection(databaseConnection);
        }

        private static void TestConnection(IDatabaseConnection databaseConnection)
        {
            databaseConnection.GetConnection();
        }

        public static IDatabaseConnection CreateDatabaseConnection(string vendor)
        {
            var databaseConfig = MyDBConnection.GetDatabaseConfig(vendor);
            var currentDatabaseConnection = DatabaseConnection.CurrentConnection;
            if (currentDatabaseConnection != null &&
                currentDatabaseConnection.GetType().Name.ToLower().Contains(databaseConfig.Vendor.ToLower()))
            {
                return currentDatabaseConnection;
            }

            return databaseConfig.GetDatabaseConnection();
        }

        public static void SetupDBOracleConnection()
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

        private class BlobWithName
        {
            private static Semaphore _lock = new Semaphore(1, 1);

            public byte[] Data { get; private set; }
            public string Name { get; private set; }
            public BlobWithName(byte[] data, string name)
            {
                Name = name;
                Data = data;
            }
            public bool WriteOutIfDoesntExist()
            {
                _lock.WaitOne();
                try
                {
                    if (!File.Exists(Name))
                    {
                        File.WriteAllBytes(Name, Data);
                        return true;
                    }
                }
                finally
                {
                    _lock.Release();
                }
                return false;
            }
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