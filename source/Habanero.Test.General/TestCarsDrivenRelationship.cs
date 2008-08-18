//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ObjectManager;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestCarsDrivenRelationship : TestUsingDatabase
    {
        public TestCarsDrivenRelationship()
        {
        }

        public static void RunTest()
        {
            TestCarsDrivenRelationship test = new TestCarsDrivenRelationship();
            test.TestGetCarsDriven();
        }

        [Test]
        public void TestGetCarsDriven()
        {
            Car.DeleteAllCars();
            BusObjectManager.Instance.ClearLoadedObjects();
            ContactPersonCompositeKey.DeleteAllContactPeople();
            ContactPersonCompositeKey.ClearContactPersonCol();

            Car car = new Car();
            ContactPersonCompositeKey person = new ContactPersonCompositeKey();
            person.SetPropertyValue("Surname", "Driver Surname");
            person.SetPropertyValue("PK1Prop1", "Driver1");
            person.SetPropertyValue("PK1Prop2", "Driver2");
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("DriverFK1", person.GetPropertyValue("PK1Prop1"));
            car.SetPropertyValue("DriverFK2", person.GetPropertyValue("PK1Prop2"));
            car.Save();
            Assert.AreEqual(car.GetDriver().ID, person.ID);
            Assert.AreEqual(person.GetCarsDriven().Count, 1);
        }

        //TODO: Peter - I Commented out the last line because I had to make the collection reload each time it's
        //retrieved in case a new object is created that would go into this collection.  There are currently no 
        //events caught by a collection if a new object is created.
        [Test]
            public void TestGetCarsDrivenByPersonHeldInMemory()
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
            car.Save();

            Assert.AreEqual(car.GetDriver().ID, person.ID);

            Assert.AreEqual(1, person.GetCarsDriven().Count, "there should be one car for this person");

            IBusinessObjectCollection carsDriven = person.GetCarsDriven();
            Car carDriven = (Car) carsDriven[0];
            Assert.AreEqual(car.ID, carDriven.ID);
            IBusinessObjectCollection carsDriven2 = person.GetCarsDriven();
            //Assert.IsTrue(object.ReferenceEquals(carsDriven, carsDriven2), "The references should be equal since the collection should be kept in memory");
        }
    }
}