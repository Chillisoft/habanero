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
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDataAccessorInMemory
    {
        [Test]
        public void Test_UsesBusinessObjectLoaderInMemory()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(BusinessObjectLoaderInMemory), dataAccessorInMemory.BusinessObjectLoader);
        }

        [Test]
        public void Test_UsesTransactionCommitterInMemory()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(TransactionCommitterInMemory), dataAccessorInMemory.CreateTransactionCommitter());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_StandardConstructor_CreatesADataStore()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataAccessorInMemory.DataStoreInMemory);
        }

        [Test]
        public void Test_Constructor_UsesPassedInDataStore()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = MockRepository.GenerateMock<DataStoreInMemory>();
            //---------------Execute Test ----------------------
            DataAccessorInMemory dataAccessorInMemory = new DataAccessorInMemory(dataStore);
            //---------------Test Result -----------------------
            Assert.AreSame(dataStore, dataAccessorInMemory.DataStoreInMemory);
        }

    }
}