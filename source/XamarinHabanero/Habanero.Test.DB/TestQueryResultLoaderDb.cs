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
using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestQueryResultLoaderDb : TestQueryResultLoaderInMemory
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

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
        
        protected override IQueryResultLoader CreateResultSetLoader()
        {
            BORegistry.DataAccessor = new DataAccessorDB(DatabaseConnection.CurrentConnection);
            return new QueryResultLoaderDb(DatabaseConnection.CurrentConnection);
        }


    }
}