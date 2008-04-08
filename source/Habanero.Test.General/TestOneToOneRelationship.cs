//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestOneToOneRelationship.
    /// </summary>
    /// 	
    [TestFixture]
    public class TestOneToOneRelationship : TestUsingDatabase
    {
        public TestOneToOneRelationship()
        {
        }

        public static void RunTest()
        {
            TestOneToOneRelationship test = new TestOneToOneRelationship();
            test.TestGetCarEngine();
        }

        [Test]
        public void TestGetCarEngine()
        {
            Car.DeleteAllCars();
            Engine.DeleteAllEngines();

            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.Save();
            Engine engine = new Engine();

            engine.SetPropertyValue("EngineNo", "NO111");
            engine.SetPropertyValue("CarID", car.GetPropertyValue("CarID"));
            engine.Save();

            Assert.AreEqual(car.GetEngine().ID, engine.ID);
            Assert.AreEqual(engine.GetCar().ID, car.ID);
        }
    }
}