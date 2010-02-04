using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using NUnit.Framework;

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

    }

   
}
