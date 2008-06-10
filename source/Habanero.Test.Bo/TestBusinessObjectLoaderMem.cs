using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObjectLoaderMem
    {
        [Test]
        public void TestDataStoreConstructor()
        {
            //---------------Set up test pack-------------------
            
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDataStoreAdd()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            dataStore.Add(new ContactPerson());
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestGetBusinessObjectWithPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
   
            dataStore.Add(cp);
            BusinessObjectLoaderInMemory loader = new BusinessObjectLoaderInMemory(dataStore);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey);
            //---------------Test Result -----------------------
            Assert.AreSame(cp, loadedCP);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetBusinessObjectWhenNotExists()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectLoaderInMemory loader = new BusinessObjectLoaderInMemory(dataStore);
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = loader.GetBusinessObject<ContactPersonTestBO>(new ContactPersonTestBO().PrimaryKey) ;
            //---------------Test Result -----------------------
            Assert.IsNull(loadedCP);
            //---------------Tear Down -------------------------          
        }
    }

    internal class BusinessObjectLoaderInMemory
    {
        private readonly DataStoreInMemory _dataStore;

        public BusinessObjectLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        public T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject
        {
            if (_dataStore.AllObjects.ContainsKey(key))
                return (T)_dataStore.AllObjects[key];
            else
                return null;
        }
    }

    internal class DataStoreInMemory 
    {
        private Dictionary<IPrimaryKey, IBusinessObject> _objects = new Dictionary<IPrimaryKey, IBusinessObject>();
        public int Count
        {
            get { return _objects.Count; }
        }

        public void Add(IBusinessObject businessObject)
        {
            _objects.Add(businessObject.PrimaryKey, businessObject);
        }

        public Dictionary<IPrimaryKey, IBusinessObject> AllObjects
        {
            get { return _objects; }
        }
   
    }
}
