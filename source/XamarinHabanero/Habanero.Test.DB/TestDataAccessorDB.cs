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
