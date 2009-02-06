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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataStoreInMemory
    {

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            new Address();
        }

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
            ContactPersonTestBO.LoadDefaultClassDef();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            dataStore.Add(new ContactPersonTestBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDataStoreRemove()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            dataStore.Add(cp);
            //---------------Execute Test ----------------------
            dataStore.Remove(cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_ClearAll()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            dataStore.Add(new ContactPersonTestBO());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataStore.Count);

            //---------------Execute Test ----------------------
            dataStore.ClearAllBusinessObjects();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
        }

        [Test]
        public void TestFind()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFind_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.Save();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            dataStore.Add(cp);
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP = (ContactPersonTestBO) dataStore.Find(typeof(ContactPersonTestBO), criteria);

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
            ContactPersonTestBO loadedCP = dataStore.Find<ContactPersonTestBO>(cp.ID);

            //---------------Test Result -----------------------
            Assert.AreSame(cp.ID, loadedCP.ID);
            //---------------Tear Down -------------------------      
        }

        [Test]
        public void TestFindAll()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = TestUtil.GetRandomString();
            cp1.Save();
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            cp2.Surname = TestUtil.GetRandomString();
            cp2.Save();

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = dataStore.FindAll<ContactPersonTestBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            Assert.AreEqual(criteria, col.SelectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFindAll_Untyped()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory(new DataStoreInMemory());
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = TestUtil.GetRandomString();
            cp1.Save();
            dataStore.Add(cp1);
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = TestUtil.GetRandomString();
            cp2.Save();
            dataStore.Add(cp2);
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

            //---------------Execute Test ----------------------
            IBusinessObjectCollection col = dataStore.FindAll(typeof(ContactPersonTestBO), criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.Contains(cp1, col);
            Assert.Contains(cp2, col);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFindAll_NullCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            dataStore.Add(cp1);
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> col = dataStore.FindAll<ContactPersonTestBO>(null);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
            Assert.IsNull(col.SelectQuery.Criteria);
            //---------------Tear Down -------------------------
        }

        [Ignore("//TODO Brett 06 Feb 2009: This will be an issue with mutable composite keys using the in memory data store")]
        [Test]
        public void TestCompositeKeyObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //Ther are two datastores so that you can manually add an item to a datastore without
            // the save effecting the datastore you are testing.
            DataStoreInMemory dataStore = new DataStoreInMemory();
            DataStoreInMemory otherDataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(otherDataStore);
            new Car();
            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsFalse(dataStore.AllObjects.ContainsKey(contactPerson.ID));
            //---------------Execute Test ----------------------
            dataStore.Add(contactPerson);
            //In the save process the ID is updated to the persisted field values, so the hash of the ID changes
            // this is why the object is removed and re-added to the BusinessObjectManager (to ensure the dictionary
            // of objects is hashed on the correct, updated value.
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.Save();  
            //---------------Test Result -----------------------
            Assert.IsTrue(dataStore.AllObjects.ContainsKey(contactPerson.ID));
        }

        [Test]
        public void Test_MutableKeyObject_TwoObjectsWithSameFieldNameAndValueAsPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
            BOWithIntID.LoadClassDefWithIntID();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            new Car();
            BOWithIntID boWithIntID = new BOWithIntID();
            boWithIntID.IntID = TestUtil.GetRandomInt();
            boWithIntID.Save();
            BOWithIntID_DifferentType intID_DifferentType = new BOWithIntID_DifferentType();
            intID_DifferentType.IntID = TestUtil.GetRandomInt();
            intID_DifferentType.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, dataStore.Count);
            //---------------Execute Test ----------------------
//            dataStore.Add(intID_DifferentType);
//            // in the save process the ID is updated to the persisted field values, so the hash of the ID changes
//            // this is why the object is removed and re-added to the BusinessObjectManager (to ensure the dictionary
//            // of objects is hashed on the correct, updated value.
//            intID_DifferentType.Save();
            IBusinessObject returnedBOWitID = dataStore.AllObjects[boWithIntID.ID];
            IBusinessObject returnedBOWitID_diffType = dataStore.AllObjects[intID_DifferentType.ID];

            //---------------Test Result -----------------------
            Assert.AreSame(boWithIntID, returnedBOWitID);
            Assert.AreSame(intID_DifferentType, returnedBOWitID_diffType);
        }

        [Test]
        public void TestMutableCompositeKeyObject_TwoObjectsWithSameFieldNameAndValueAsPrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID_DifferentType.LoadClassDefWithIntID_CompositeKey();
            BOWithIntID.LoadClassDefWithIntID_WithCompositeKey();
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            new Car();
            BOWithIntID boWithIntID = new BOWithIntID {IntID = TestUtil.GetRandomInt()};
            boWithIntID.Save();
            BOWithIntID_DifferentType intID_DifferentType = new BOWithIntID_DifferentType();
            intID_DifferentType.IntID = boWithIntID.IntID;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            intID_DifferentType.Save();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, dataStore.Count);

            Assert.IsTrue(dataStore.AllObjects.ContainsKey(boWithIntID.ID));
            Assert.IsTrue(dataStore.AllObjects.ContainsKey(intID_DifferentType.ID));

            IBusinessObject returnedBOWitID = dataStore.AllObjects[boWithIntID.ID];
            IBusinessObject returnedBOWitID_diffType = dataStore.AllObjects[intID_DifferentType.ID];

            Assert.AreSame(boWithIntID, returnedBOWitID);
            Assert.AreSame(intID_DifferentType, returnedBOWitID_diffType);
        }
    }
}
