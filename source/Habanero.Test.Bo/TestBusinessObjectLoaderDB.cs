using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB : TestUsingDatabase
    {
        public TestBusinessObjectLoaderDB()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestGetBusinessObjectWithPrimaryKey_InLoadedCol()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();

            BusinessObjectLoaderDB loader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        //TODO: stop this using the BOLoader
        [Test]
        public void TestGetBusinessObjectWithPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            BOLoader.Instance.ClearLoadedBusinessObjects();
            BusinessObjectLoaderDB loader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);

            //---------------Test Result -----------------------
            Assert.AreNotSame(cp, loadedCP);
            Assert.AreEqual(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("Working on this")]
        public void TestGetBusinessObjectByDatabaseCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            BusinessObjectLoaderDB loader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);



            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            //ContactPersonTestBO loadedCP = loader.GetBusinessObjectByDatabaseCriteria<ContactPersonTestBO>("Surname = '" + cp.Surname + "'");

            //---------------Test Result -----------------------
            //TODO: assert are same
           // Assert.AreEqual(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------          
        }


    }

    public class BusinessObjectLoaderDB
    {
        private readonly IDatabaseConnection _databaseConnection;

        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject
        {
            if (BusinessObject.AllLoadedBusinessObjects().ContainsKey(key.GetObjectId()))
            {
                return (T) BusinessObject.AllLoadedBusinessObjects()[key.GetObjectId()].Target;
            } else
            {
                return (T) BOLoader.Instance.GetBusinessObjectByID(typeof (T), key);
            }
        }

        //public T GetBusinessObjectByDatabaseCriteria<T>(string databaseCriteria) where T : class, IBusinessObject
        //{

        //}


        //public T GetBusinessObjectByDatabaseCriteria<T>(string databaseCriteria) where T : BusinessObject, new()
        //{
        //    QueryDB selectQuery = new QueryFactoryDB().CreateSelectQuery<T>();
        //    selectQuery.DatabaseCriteria = databaseCriteria;
        //    ISqlStatement statement = selectQuery.CreateSqlStatement();
        //    T loadedObject = new T();
        //    using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
        //    {
        //        try
        //        {
        //            if (dr.Read())
        //            {
        //                int i = 0;
        //                foreach (BOProp prop in loadedObject.Props.SortedValues)
        //                {
        //                    if (!prop.PropDef.Persistable) continue; //BRETT/PETER TODO: to be changed
        //                    try
        //                    {
        //                        prop.InitialiseProp(dr[i++]);
        //                    }
        //                    catch (IndexOutOfRangeException)
        //                    {
        //                    }
        //                }
        //                return loadedObject;
        //            } else
        //            {
        //                return null;
        //            }
        //        }
        //        finally
        //        {
        //            if (dr != null && !dr.IsClosed)
        //            {
        //                dr.Close();
        //            }
        //        }
        //    }
        //}
    }
}
