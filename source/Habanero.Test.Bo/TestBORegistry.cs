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

using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBORegistry 
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            BORegistry.ClearCustomDataAccessors();
        }

        [SetUp]
        public void SetupTest()
        {
            BORegistry.ClearCustomDataAccessors();
        }

        [TearDown]
        public void TearDownTest()
        {
        
        }

        [Test]
        public void TestSetDataAccessor()
        {
            //---------------Set up test pack-------------------
            IDataAccessor dataAccessor = new DataAccessorDB();
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor = dataAccessor;
            //---------------Test Result -----------------------
            Assert.AreSame(dataAccessor, BORegistry.DataAccessor);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDataAccessorDBConstructor()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLoaderDB), dataAccessor.BusinessObjectLoader);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDataAccessorDB_CreateTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(TransactionCommitterDB), dataAccessor.CreateTransactionCommitter());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDataAccessorDB_PassesConnectionInConstructorToBOLoader()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection customConnection = new TestSelectQueryDB.DatabaseConnectionStub();
            IDatabaseConnection defaultConnection = new TestSelectQueryDB.DatabaseConnectionStub();
            DatabaseConnection.CurrentConnection = defaultConnection;
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(customConnection, DatabaseConnection.CurrentConnection);
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessor = new DataAccessorDB(customConnection);
            //---------------Test Result -----------------------
            BusinessObjectLoaderDB businessObjectLoader = (BusinessObjectLoaderDB) dataAccessor.BusinessObjectLoader;
            Assert.AreEqual(customConnection, businessObjectLoader.DatabaseConnection);
            Assert.AreNotEqual(defaultConnection, businessObjectLoader.DatabaseConnection);
        }

        [Test]
        public void TestDataAccessorDB_ParameterlessConstructorUsesCurrentConnection()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection defaultConnection = new TestSelectQueryDB.DatabaseConnectionStub();
            DatabaseConnection.CurrentConnection = defaultConnection;
            //---------------Assert Precondition----------------
            Assert.AreEqual(defaultConnection, DatabaseConnection.CurrentConnection);
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessor = new DataAccessorDB();
            //---------------Test Result -----------------------
            BusinessObjectLoaderDB businessObjectLoader = (BusinessObjectLoaderDB)dataAccessor.BusinessObjectLoader;
            Assert.AreEqual(defaultConnection, businessObjectLoader.DatabaseConnection);
        }
        
        [Test]
        public void TestDataAccessorDB_PassesConnectionInConstructorToTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection customConnection = new TestSelectQueryDB.DatabaseConnectionStub();
            IDatabaseConnection defaultConnection = new TestSelectQueryDB.DatabaseConnectionStub();
            DatabaseConnection.CurrentConnection = defaultConnection;
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(customConnection, DatabaseConnection.CurrentConnection);
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessor = new DataAccessorDB(customConnection);
            //---------------Test Result -----------------------
            ITransactionCommitter transactionCommitter = dataAccessor.CreateTransactionCommitter();
            Assert.IsInstanceOfType(typeof(TransactionCommitterDB), transactionCommitter);
            TransactionCommitterDB transactionCommitterDB = (TransactionCommitterDB) transactionCommitter;
            Assert.AreEqual(customConnection, transactionCommitterDB.DatabaseConnection);
            Assert.AreNotEqual(defaultConnection, transactionCommitterDB.DatabaseConnection);
        }

        [Test]
        public void TestDataAccessorInMemoryConstructor()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BusinessObjectLoaderInMemory), dataAccessor.BusinessObjectLoader);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDataAccessorInMemory_CreateTransactionCommitter()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(TransactionCommitterInMemory), dataAccessor.CreateTransactionCommitter());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_GetDataAccessor_CustomClassDefConnection()
        {
            //---------------Set up test pack-------------------
            DataAccessorInMemory defaultAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customAccessor = new DataAccessorInMemory();
            BORegistry.DataAccessor = defaultAccessor;
            BORegistry.AddDataAccessor(typeof (MyBO), customAccessor);
            //---------------Assert Precondition----------------
            Assert.AreEqual(defaultAccessor, BORegistry.DataAccessor);
            //---------------Execute Test ----------------------
            IDataAccessor testDataAccessor = BORegistry.GetDataAccessor(typeof (MyBO));
            //---------------Test Result -----------------------
            Assert.AreEqual(customAccessor, testDataAccessor);
            Assert.AreNotSame(customAccessor, BORegistry.DataAccessor);
        }

        [Test]
        public void Test_GetDataAccessor_CustomClassDefConnection_NotFound()
        {
            //---------------Set up test pack-------------------
            DataAccessorInMemory defaultAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customAccessor = new DataAccessorInMemory();
            BORegistry.DataAccessor = defaultAccessor;
            BORegistry.AddDataAccessor(typeof(MyBO), customAccessor);
            //---------------Assert Precondition----------------
            Assert.AreEqual(defaultAccessor, BORegistry.DataAccessor);
            //---------------Execute Test ----------------------
            IDataAccessor testDataAccessor = BORegistry.GetDataAccessor(typeof(Shape));
            //---------------Test Result -----------------------
            Assert.AreEqual(defaultAccessor, testDataAccessor);
            Assert.AreSame(defaultAccessor, BORegistry.DataAccessor);
        }

        [Test]
        public void Test_ClearCustomDataAccessors_ClearsThem()
        {
            //---------------Set up test pack-------------------
            DataAccessorInMemory defaultAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customAccessor = new DataAccessorInMemory();
            BORegistry.DataAccessor = defaultAccessor;
            BORegistry.AddDataAccessor(typeof(MyBO), customAccessor);
            //---------------Assert Precondition----------------
            Assert.AreEqual(defaultAccessor, BORegistry.DataAccessor);
            Assert.AreEqual(customAccessor, BORegistry.GetDataAccessor(typeof(MyBO)));
            //---------------Execute Test ----------------------
            BORegistry.ClearCustomDataAccessors();
            //---------------Test Result -----------------------
            Assert.AreNotEqual(customAccessor, BORegistry.GetDataAccessor(typeof(MyBO)));
            Assert.AreEqual(defaultAccessor, BORegistry.GetDataAccessor(typeof(MyBO)));
            Assert.IsNotNull(BORegistry.DataAccessor);
            Assert.AreEqual(defaultAccessor, BORegistry.DataAccessor);
        }
    }
}