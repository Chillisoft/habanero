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
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestBusinessObjectLoaderMultiSource_Standard : TestBusinessObjectLoader
    {
        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
        }

        protected override void DeleteEnginesAndCars()
        {

        }
    }

    [TestFixture]
    public class TestBusinessObjectLoaderMultiSource_GetBusinessObjectCollection : TestBusinessObjectLoader_GetBusinessObjectCollection
    {
        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
        }

        protected override void DeleteEnginesAndCars()
        {

        }
    }

    public class TestBusinessObjectLoaderMultiSource_RefreshCollection : TestBusinessObjectLoader_RefreshCollection
    {
        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorMultiSource(new DataAccessorInMemory());
        }

        protected override void DeleteEnginesAndCars()
        {

        }
    }

    [TestFixture]
    public class TestBusinessObjectLoaderMultiSource 
    {
        [Test]
        public void Test_LoadingWithDefaultOnly()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            MyBO bo1 = CreateMyBO(dataStore);
            BusinessObjectLoaderInMemory defaultBusinessObjectLoader = new BusinessObjectLoaderInMemory(dataStore);

            //---------------Execute Test ----------------------
            BusinessObjectLoaderMultiSource businessObjectLoaderMultiSource = new BusinessObjectLoaderMultiSource(defaultBusinessObjectLoader);
            var loadedBo1 = businessObjectLoaderMultiSource.GetBusinessObject<MyBO>(bo1.ID);
            //---------------Test Result -----------------------
            Assert.AreSame(loadedBo1, bo1);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_LoadingFromMultipleSources()
        {
            //---------------Set up test pack-------------------

            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            MyBO bo1 = CreateMyBO(dataStore1);
            BusinessObjectLoaderInMemory loader1 = new BusinessObjectLoaderInMemory(dataStore1);

            DataStoreInMemory dataStore2 = new DataStoreInMemory();
            MyRelatedBo bo2 = CreateMyRelatedBO(dataStore2);
            BusinessObjectLoaderInMemory loader2 = new BusinessObjectLoaderInMemory(dataStore2);

            BusinessObjectLoaderInMemory defaultBusinessObjectLoader = new BusinessObjectLoaderInMemory(new DataStoreInMemory());
            BusinessObjectLoaderMultiSource businessObjectLoaderMultiSource = new BusinessObjectLoaderMultiSource(defaultBusinessObjectLoader);

            //---------------Execute Test ----------------------
            businessObjectLoaderMultiSource.AddBusinessObjectLoader(typeof (MyBO), loader1);
            businessObjectLoaderMultiSource.AddBusinessObjectLoader(typeof(MyRelatedBo), loader2);
            var loadedBo1 = businessObjectLoaderMultiSource.GetBusinessObject<MyBO>(bo1.ID);
            var loadedBo2 = businessObjectLoaderMultiSource.GetBusinessObject<MyRelatedBo>(bo2.ID);
            //---------------Tear down -------------------------

            Assert.AreSame(loadedBo1, bo1);
            Assert.AreSame(loadedBo2, bo2);
        }

        [Test]
        public void Test_LoadingFromDefaultWithMultipleSourcesConfigured()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore1 = new DataStoreInMemory();
            DataStoreInMemory defaultDataStore = new DataStoreInMemory();

            MyBO bo1 = CreateMyBO(dataStore1);
            MyRelatedBo bo2 = CreateMyRelatedBO(defaultDataStore);

            BusinessObjectLoaderInMemory loader1 = new BusinessObjectLoaderInMemory(dataStore1);
            BusinessObjectLoaderInMemory defaultLoader = new BusinessObjectLoaderInMemory(defaultDataStore);
            BusinessObjectLoaderMultiSource businessObjectLoaderMultiSource = new BusinessObjectLoaderMultiSource(defaultLoader);

            //---------------Execute Test ----------------------
            businessObjectLoaderMultiSource.AddBusinessObjectLoader(typeof(MyBO), loader1);
            var loadedBo1 = businessObjectLoaderMultiSource.GetBusinessObject<MyBO>(bo1.ID);
            var loadedBo2 = businessObjectLoaderMultiSource.GetBusinessObject<MyRelatedBo>(bo2.ID);
            //---------------Tear down -------------------------

            Assert.AreSame(loadedBo1, bo1);
            Assert.AreSame(loadedBo2, bo2);
        }
        
        private MyRelatedBo CreateMyRelatedBO(DataStoreInMemory dataStore2)
        {
            MyRelatedBo.LoadClassDef();
            TransactionCommitterInMemory committer2 = new TransactionCommitterInMemory(dataStore2);
            var bo2 = new MyRelatedBo();
            committer2.AddBusinessObject(bo2);
            committer2.CommitTransaction();
            return bo2;
        }

        private MyBO CreateMyBO(DataStoreInMemory dataStore1)
        {
            MyBO.LoadDefaultClassDef();
            TransactionCommitterInMemory committer1 = new TransactionCommitterInMemory(dataStore1);
            var bo1 = new MyBO();
            committer1.AddBusinessObject(bo1);
            committer1.CommitTransaction();
            return bo1;
        }


    }

}
