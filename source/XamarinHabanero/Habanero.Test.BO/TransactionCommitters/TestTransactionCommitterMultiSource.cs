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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.TransactionCommitters
{
    [TestFixture]
    public class TestTransactionCommitterMultiSource
    {
        [Test]
        public void Test_DefaultDataAccessor_OneObject()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory(dataStore);
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            //---------------Execute Test ----------------------
            ITransactionCommitter transactionCommitter = new TransactionCommitterMultiSource(defaultDataAccessor, new Dictionary<Type, IDataAccessor>());
            transactionCommitter.AddBusinessObject(bo);
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore.Find<MyBO>(bo.ID));
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_DefaultDataAccessor_MultipleObjects()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory(dataStore);
            MyBO.LoadDefaultClassDef();
            MyBO bo1 = new MyBO();
            MyBO bo2 = new MyBO();
            //---------------Execute Test ----------------------
            ITransactionCommitter transactionCommitter = new TransactionCommitterMultiSource(defaultDataAccessor, new Dictionary<Type, IDataAccessor>());
            transactionCommitter.AddBusinessObject(bo1);
            transactionCommitter.AddBusinessObject(bo2);
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore.Find<MyBO>(bo1.ID));
            Assert.IsNotNull(dataStore.Find<MyBO>(bo2.ID));
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_DefaultDataAccessor_AddTransaction()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory(dataStore);
            MyBO.LoadDefaultClassDef();
            MyBO bo1 = new MyBO();
            MyBO bo2 = new MyBO();
            //---------------Execute Test ----------------------
            ITransactionCommitter transactionCommitter = new TransactionCommitterMultiSource(defaultDataAccessor, new Dictionary<Type, IDataAccessor>());
            transactionCommitter.AddTransaction(new TransactionalBusinessObject(bo1));
            transactionCommitter.AddTransaction(new TransactionalBusinessObject(bo2));
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore.Find<MyBO>(bo1.ID));
            Assert.IsNotNull(dataStore.Find<MyBO>(bo2.ID));
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_UsingDataAccessorAssignedForType()
        {
            //---------------Set up test pack-------------------
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory();

            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataStoreInMemory dataStore2 = new DataStoreInMemory();
            DataAccessorInMemory dataAccessorInMemory1 = new DataAccessorInMemory(dataStore1);
            DataAccessorInMemory dataAccessorInMemory2 = new DataAccessorInMemory(dataStore2);
            MyBO.LoadDefaultClassDef();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            MyRelatedBo bo2 = new MyRelatedBo();

            Dictionary<Type, IDataAccessor> dataAccessors = new Dictionary<Type, IDataAccessor>();
            dataAccessors.Add(typeof(MyBO), dataAccessorInMemory1);
            dataAccessors.Add(typeof(MyRelatedBo), dataAccessorInMemory2);

            //---------------Execute Test ----------------------
            ITransactionCommitter transactionCommitter1 = new TransactionCommitterMultiSource(defaultDataAccessor, dataAccessors);
            transactionCommitter1.AddBusinessObject(bo1);
            transactionCommitter1.CommitTransaction();

            ITransactionCommitter transactionCommitter2 = new TransactionCommitterMultiSource(defaultDataAccessor, dataAccessors);
            transactionCommitter2.AddBusinessObject(bo2);
            transactionCommitter2.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore1.Find<MyBO>(bo1.ID));
            Assert.IsNull(dataStore2.Find<MyBO>(bo1.ID));

            Assert.IsNotNull(dataStore2.Find<MyRelatedBo>(bo2.ID));
            Assert.IsNull(dataStore1.Find<MyRelatedBo>(bo2.ID));
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_ShouldThrowError_WhenAddingObjectsOfDifferentTypesWithDifferentDataAccessors()
        {
            //---------------Set up test pack-------------------
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory();

            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataStoreInMemory dataStore2 = new DataStoreInMemory();
            DataAccessorInMemory dataAccessorInMemory1 = new DataAccessorInMemory(dataStore1);
            DataAccessorInMemory dataAccessorInMemory2 = new DataAccessorInMemory(dataStore2);
            MyBO.LoadDefaultClassDef();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            MyRelatedBo bo2 = new MyRelatedBo();

            Dictionary<Type, IDataAccessor> dataAccessors = new Dictionary<Type, IDataAccessor>();
            dataAccessors.Add(typeof(MyBO), dataAccessorInMemory1);
            dataAccessors.Add(typeof(MyRelatedBo), dataAccessorInMemory2);

            //---------------Execute Test ----------------------
            try
            {
                ITransactionCommitter transactionCommitter1 = new TransactionCommitterMultiSource(defaultDataAccessor, dataAccessors);
                transactionCommitter1.AddBusinessObject(bo1);
                transactionCommitter1.AddBusinessObject(bo2);
                transactionCommitter1.CommitTransaction();
                Assert.Fail("Error should have occurred");
            } catch (HabaneroDeveloperException ex)
            //---------------Test Result -----------------------
            {
                Assert.IsNull(dataStore1.Find<MyBO>(bo1.ID));
                Assert.IsNull(dataStore1.Find<MyRelatedBo>(bo2.ID));
                Assert.IsNull(dataStore2.Find<MyBO>(bo1.ID));
                Assert.IsNull(dataStore2.Find<MyRelatedBo>(bo2.ID));

                StringAssert.Contains("MyRelatedBo", ex.DeveloperMessage);
                StringAssert.Contains("was added to a TransactionCommitterMultiSource which has been set up with a different source to this type", ex.DeveloperMessage);

                //correct
            }
            //---------------Tear down -------------------------

        }

        [Test]
        public void Test_Success_WhenAddingObjectsOfDifferentTypesWithSameDataAccessors()
        {
            //---------------Set up test pack-------------------
            IDataAccessor defaultDataAccessor = new DataAccessorInMemory();

            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataAccessorInMemory dataAccessorInMemory1 = new DataAccessorInMemory(dataStore1);
            MyBO.LoadDefaultClassDef();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = new MyBO();
            MyRelatedBo bo2 = new MyRelatedBo();

            Dictionary<Type, IDataAccessor> dataAccessors = new Dictionary<Type, IDataAccessor>();
            dataAccessors.Add(typeof (MyBO), dataAccessorInMemory1);
            dataAccessors.Add(typeof (MyRelatedBo), dataAccessorInMemory1);

            //---------------Execute Test ----------------------
            ITransactionCommitter transactionCommitter1 = new TransactionCommitterMultiSource(defaultDataAccessor,
                                                                                              dataAccessors);
            transactionCommitter1.AddBusinessObject(bo1);
            transactionCommitter1.AddBusinessObject(bo2);
            transactionCommitter1.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore1.Find<MyBO>(bo1.ID));
            Assert.IsNotNull(dataStore1.Find<MyRelatedBo>(bo2.ID));
            //---------------Tear down -------------------------

        }

        [Test]
        public void Test_CommitTransaction_WhenEmpty_ShouldNotCommitOnAnyDataAccessors()
        {
            //---------------Set up test pack-------------------
            IDataAccessor defaultDataAccessor = MockRepository.GenerateStub<IDataAccessor>();
            IDataAccessor alternateDataAccessor = MockRepository.GenerateStub<IDataAccessor>();

            MyBO.LoadDefaultClassDef();
            Dictionary<Type, IDataAccessor> dataAccessors = new Dictionary<Type, IDataAccessor>();
            dataAccessors.Add(typeof(MyBO), alternateDataAccessor);
            ITransactionCommitter transactionCommitter = new TransactionCommitterMultiSource(defaultDataAccessor, dataAccessors);
            //---------------Assert Precondition----------------
            defaultDataAccessor.AssertWasNotCalled(accessor => accessor.CreateTransactionCommitter());
            alternateDataAccessor.AssertWasNotCalled(accessor => accessor.CreateTransactionCommitter());
            //---------------Execute Test ----------------------
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            defaultDataAccessor.AssertWasNotCalled(accessor => accessor.CreateTransactionCommitter());
            alternateDataAccessor.AssertWasNotCalled(accessor => accessor.CreateTransactionCommitter());
        }

    }

   
}
