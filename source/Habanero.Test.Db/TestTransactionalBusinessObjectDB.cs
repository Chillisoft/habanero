// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
