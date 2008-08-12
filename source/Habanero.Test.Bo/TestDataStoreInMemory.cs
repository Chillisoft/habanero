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
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
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
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);

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
    }
}
