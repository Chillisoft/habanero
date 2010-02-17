using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataAccessorMultiSource
    {
        [Test]
        public void Test_Construct_WithDefaults()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(BusinessObjectLoaderMultiSource), dataAccessor.BusinessObjectLoader);
            Assert.IsInstanceOf(typeof(TransactionCommitterMultiSource), dataAccessor.CreateTransactionCommitter());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_UsingDefaultBusinessObjectLoader()
        {
            //---------------Set up test pack-------------------
            DataAccessorMultiSource dataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
            ITransactionCommitter committer = dataAccessor.CreateTransactionCommitter();
            MyBO.LoadDefaultClassDef();
            var bo1 = new MyBO();
            committer.AddBusinessObject(bo1);
            committer.CommitTransaction();
            //---------------Execute Test ----------------------
            var loadedBo1 = dataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(bo1.ID);
            //---------------Test Result -----------------------
            Assert.AreSame(loadedBo1, bo1);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_LoadingFromMultipleSources()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataStoreInMemory dataStore2 = new DataStoreInMemory();

            MyBO.LoadDefaultClassDef();
            TransactionCommitterInMemory committer1 = new TransactionCommitterInMemory(dataStore1);
            var bo1 = new MyBO();
            committer1.AddBusinessObject(bo1);
            committer1.CommitTransaction();

            MyRelatedBo.LoadClassDef();
            TransactionCommitterInMemory committer2 = new TransactionCommitterInMemory(dataStore2);
            var bo2 = new MyRelatedBo();
            committer2.AddBusinessObject(bo2);
            committer2.CommitTransaction();

            DataAccessorInMemory dataAccessorInMemory1 = new DataAccessorInMemory(dataStore1);
            DataAccessorInMemory dataAccessorInMemory2 = new DataAccessorInMemory(dataStore2);

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            DataAccessorMultiSource dataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
            dataAccessor.AddDataAccessor(typeof(MyBO), dataAccessorInMemory1);
            dataAccessor.AddDataAccessor(typeof(MyRelatedBo), dataAccessorInMemory2);
            var loadedBo1 = dataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(bo1.ID);
            var loadedBo2 = dataAccessor.BusinessObjectLoader.GetBusinessObject<MyRelatedBo>(bo2.ID);
            //---------------Tear down -------------------------

            Assert.AreSame(loadedBo1, bo1);
            Assert.AreSame(loadedBo2, bo2);
        }

        [Test]
        public void Test_SavingToMultipleSources()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataStoreInMemory dataStore2 = new DataStoreInMemory();

            DataAccessorInMemory dataAccessorInMemory1 = new DataAccessorInMemory(dataStore1);
            DataAccessorInMemory dataAccessorInMemory2 = new DataAccessorInMemory(dataStore2);

            DataAccessorMultiSource dataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
            dataAccessor.AddDataAccessor(typeof(MyBO), dataAccessorInMemory1);
            dataAccessor.AddDataAccessor(typeof(MyRelatedBo), dataAccessorInMemory2);
            MyBO.LoadDefaultClassDef();
            MyRelatedBo.LoadClassDef();
            var bo1 = new MyBO();
            var bo2 = new MyRelatedBo();
            //---------------Execute Test ----------------------
            ITransactionCommitter committer1 = dataAccessor.CreateTransactionCommitter();
            committer1.AddBusinessObject(bo1);
            committer1.CommitTransaction();

            ITransactionCommitter committer2 = dataAccessor.CreateTransactionCommitter();
            committer2.AddBusinessObject(bo2);
            committer2.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.IsNotNull(dataStore1.Find<MyBO>(bo1.ID));
            Assert.IsNotNull(dataStore2.Find<MyRelatedBo>(bo2.ID));
            //---------------Tear down -------------------------

        }

    }
}
