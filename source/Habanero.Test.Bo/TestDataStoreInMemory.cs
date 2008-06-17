using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemory
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
        public void TestDataStoreRemove()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPerson cp = new ContactPerson();
            dataStore.Add(cp);
            //---------------Execute Test ----------------------
            dataStore.Remove(cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFind()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("Surname", Criteria.Op.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFind_PrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(cp.PrimaryKey);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFindAll()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.Op.Equals, now);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = dataStore.FindAll<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.AreEqual(criteria, col.Criteria);
            //---------------Tear Down -------------------------
        }
    }
}
