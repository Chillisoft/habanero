//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestCarDriverRelationship.
    /// </summary>
    /// 	
    [TestFixture]
    public class TestCarDriverRelationship : TestUsingDatabase
    {
        public TestCarDriverRelationship()
        {
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.SetupDBConnection();
        }

        public static void RunTest()
        {
            TestCarDriverRelationship test = new TestCarDriverRelationship();
            test.TestGetCarDriver();
        }

        [Test]
        public void TestGetCarDriver()
        {
            Car.DeleteAllCars();
            ContactPersonCompositeKey.DeleteAllContactPeople();

            Car car = new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();
            person.SetPropertyValue("Surname", "Driver Surname");
            person.SetPropertyValue("PK1Prop1", "Driver1");
            person.SetPropertyValue("PK1Prop2", "Driver2");
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("DriverFK1", person.GetPropertyValue("PK1Prop1"));
            car.SetPropertyValue("DriverFK2", person.GetPropertyValue("PK1Prop2"));
            BOPrimaryKey bob = car.GetDriver().ID;
            BOPrimaryKey bo2 = person.ID;
            Assert.IsTrue(bob.Equals(bo2));
            Assert.AreEqual(bob, bo2);
            //Assert.AreEqual(car.GetDriver().ID, person.ID);
        }

        public void TestGetCarDriverNull()
        {
            Car.DeleteAllCars();
            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            Assert.IsTrue(car.GetDriver() == null);
        }

        public void TestGetCarDriverIsSame()
        {
            Car.DeleteAllCars();
            ContactPersonCompositeKey.DeleteAllContactPeople();

            Car car = new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();
            person.SetPropertyValue("Surname", "Driver Surname");
            person.SetPropertyValue("PK1Prop1", "Driver11");
            person.SetPropertyValue("PK1Prop2", "Driver21");
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("DriverFK1", person.GetPropertyValue("PK1Prop1"));
            car.SetPropertyValue("DriverFK2", person.GetPropertyValue("PK1Prop2"));
            Assert.AreEqual(car.GetDriver().ID, person.ID);
            Assert.IsTrue(object.ReferenceEquals(person, car.GetDriver()));

            person = car.GetDriver();
            ContactPersonCompositeKey.ClearContactPersonCol();
            Assert.IsTrue(object.ReferenceEquals(person, car.GetDriver()),
                          "Should be the same since the Driver reference is being " +
                          "maintained in the car class and the object is therefore " +
                          "not being reloaded");
        }
    }
}