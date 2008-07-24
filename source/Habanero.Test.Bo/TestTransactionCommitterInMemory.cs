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
    public class TestTransactionCommitterInMemory
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();


        }
        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void TestInsert()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            IBusinessObjectLoader loader = new BusinessObjectLoaderInMemory(dataStore);
            ITransactionCommitter transactionCommitter = new TransactionCommitterInMemory(dataStore);
            //---------------Execute Test ----------------------
            transactionCommitter.AddBusinessObject(cp);
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreSame(cp, loader.GetBusinessObject<ContactPersonTestBO>(cp.ID));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestUpdate()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            IBusinessObjectLoader loader = new BusinessObjectLoaderInMemory(dataStore);
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            firstTransactionCommitter.AddBusinessObject(cp);
            firstTransactionCommitter.CommitTransaction();

            //---------------Execute Test ----------------------
            cp.Surname = Guid.NewGuid().ToString("N");
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            secondTransactionCommitter.AddBusinessObject(cp);
            secondTransactionCommitter.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreSame(cp, loader.GetBusinessObject<ContactPersonTestBO>(cp.ID));
            Assert.IsFalse(cp.State.IsDirty);
            //---------------Tear Down -------------------------
        }

        private ContactPersonTestBO GetContactPerson()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            return cp;
        }

        [Test]
        public void TestDelete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            IBusinessObjectLoader loader = new BusinessObjectLoaderInMemory(dataStore);
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            firstTransactionCommitter.AddBusinessObject(cp);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            Assert.AreEqual(1, dataStore.Count);

            //---------------Execute Test ----------------------
            cp.Delete();
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            secondTransactionCommitter.AddBusinessObject(cp);
            secondTransactionCommitter.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
//            Assert.IsNull(loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey));

        }

        [Test, ExpectedException(typeof(BusinessObjectReferentialIntegrityException)), Ignore(" new data store memory")]
        public void TestPreventDelete()
        {
            //---------------Set up test pack-------------------

            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.Delete();
            ITransactionCommitter committer = new TransactionCommitterInMemory(new DataStoreInMemory());
            committer.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
        }
    }
}
