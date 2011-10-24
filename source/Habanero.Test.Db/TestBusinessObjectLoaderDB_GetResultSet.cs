using System;
using System.Collections.Generic;
using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB_GetResultSet :
        TestBusinessObjectLoader_GetResultSet
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        //protected override void DeleteEnginesAndCars()
        //{
        //    Engine.DeleteAllEngines();
        //    Car.DeleteAllCars();
        //}

        public TestBusinessObjectLoaderDB_GetResultSet()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }
    }
}