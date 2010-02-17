using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDataAccessorDB
    {
        [Test]
        public void Test_UsesBusinessObjectLoaderDB()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessorDb = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(BusinessObjectLoaderDB), dataAccessorDb.BusinessObjectLoader);
        }

        [Test]
        public void Test_UsesTransactionCommitterDB()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessorDb = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(TransactionCommitterDB), dataAccessorDb.CreateTransactionCommitter());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_StandardConstructor_UsesCurrentConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection = MockRepository.GenerateMock<IDatabaseConnection>();
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessorDb = new DataAccessorDB();
            //---------------Test Result -----------------------
            Assert.AreSame(DatabaseConnection.CurrentConnection, ((BusinessObjectLoaderDB)dataAccessorDb.BusinessObjectLoader).DatabaseConnection);
            Assert.AreSame(DatabaseConnection.CurrentConnection, ((TransactionCommitterDB)dataAccessorDb.CreateTransactionCommitter()).DatabaseConnection);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_Constructor_UsesPassedInDatabaseConnection()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection connection = MockRepository.GenerateMock<IDatabaseConnection>();
            //---------------Execute Test ----------------------
            DataAccessorDB dataAccessorDb = new DataAccessorDB(connection);
            //---------------Test Result -----------------------
            Assert.AreSame(connection, ((BusinessObjectLoaderDB)dataAccessorDb.BusinessObjectLoader).DatabaseConnection);
            Assert.AreSame(connection, ((TransactionCommitterDB)dataAccessorDb.CreateTransactionCommitter()).DatabaseConnection);
            //---------------Tear down -------------------------

        }

    }
}
