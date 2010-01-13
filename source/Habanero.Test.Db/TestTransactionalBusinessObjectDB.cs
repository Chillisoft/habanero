using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestTransactionalBusinessObjectDB : TestUsingDatabase
    {
        private IDatabaseConnection _databaseConnection;
        [TestFixtureSetUp]
        public void SetupDatabase()
        {
            base.SetupDBConnection();
            _databaseConnection = DatabaseConnection.CurrentConnection;
        }
        
        [Test]
        public void Test_UsingGivenDatabaseConnection_Insert()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            DatabaseConnection.CurrentConnection = null;

            //---------------Assert preconditions---------------
            Assert.AreNotSame(_databaseConnection, DatabaseConnection.CurrentConnection);

            //---------------Execute Test ----------------------
            TransactionalBusinessObjectDB transactional = new TransactionalBusinessObjectDB(bo, _databaseConnection);
            SqlStatement sqlStatement = (SqlStatement) transactional.GetPersistSql()[0];

            //---------------Test Result -----------------------
            Assert.AreSame(_databaseConnection, sqlStatement.Connection);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_UsingGivenDatabaseConnection_Update()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.Save();
            bo.TestProp = Guid.NewGuid().ToString();
            DatabaseConnection.CurrentConnection = null;

            //---------------Assert preconditions---------------
            Assert.AreNotSame(_databaseConnection, DatabaseConnection.CurrentConnection);

            //---------------Execute Test ----------------------
            TransactionalBusinessObjectDB transactional = new TransactionalBusinessObjectDB(bo, _databaseConnection);
            SqlStatement sqlStatement = (SqlStatement) transactional.GetPersistSql()[0];

            //---------------Test Result -----------------------
            Assert.AreSame(_databaseConnection, sqlStatement.Connection);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_UsingGivenDatabaseConnection_Delete()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.Save();
            bo.MarkForDelete();
            DatabaseConnection.CurrentConnection = null;

            //---------------Assert preconditions---------------
            Assert.AreNotSame(_databaseConnection, DatabaseConnection.CurrentConnection);
            //---------------Execute Test ----------------------
            TransactionalBusinessObjectDB transactional = new TransactionalBusinessObjectDB(bo, _databaseConnection);
            SqlStatement sqlStatement = (SqlStatement) transactional.GetPersistSql()[0];

            //---------------Test Result -----------------------
            Assert.AreSame(_databaseConnection, sqlStatement.Connection);
            //---------------Tear down -------------------------
        }


    }
}
