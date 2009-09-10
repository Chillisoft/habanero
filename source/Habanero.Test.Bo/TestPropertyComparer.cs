//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropertyComparer : TestUsingDatabase
    {

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            SetupDBConnection();
        }

        [Test]
        public void TestSimpleCompare_String()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "5";
            Car car2 = new Car();
            car2.CarRegNo = "2";

            //---------------Execute Test ----------------------
            PropertyComparer<Car, string> comparer = new PropertyComparer<Car, string>("CarRegNo");
            int comparisonResult = comparer.Compare(car1, car2);
            //---------------Test Result -----------------------
            Assert.Greater(comparisonResult, 0, "car1 should be greater than car2 when compared on CarRegNo");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareThroughRelationship()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "5";
            Car car2 = new Car();
            car2.CarRegNo = "2";

            Engine engine1 = new Engine();
            engine1.CarID = car1.CarID;
            engine1.EngineNo = "20";

            Engine engine2 = new Engine();
            engine2.CarID = car2.CarID;
            engine2.EngineNo = "50";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(car2);
            committer.AddBusinessObject(engine1);
            committer.AddBusinessObject(engine2);
            committer.CommitTransaction();

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            PropertyComparer<Engine, string> comparer = new PropertyComparer<Engine, string>("CarRegNo");
            comparer.Source = new Source("Car");
            int comparisonResult = comparer.Compare(engine1, engine2);
            //---------------Test Result -----------------------
            Assert.Greater(comparisonResult, 0, "engine1 should be greater as its car's regno is greater");
            //---------------Tear Down -------------------------     
        }
    }
}
