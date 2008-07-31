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
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestCarsOwnedRelationship : TestUsingDatabase
    {
        public TestCarsOwnedRelationship()
        {
        }

        public static void RunTest()
        {
            TestCarsOwnedRelationship test = new TestCarsOwnedRelationship();
            test.TestGetCarsOwned();
        }

        [Test]
        public void TestGetCarsOwned()
        {
            Car.DeleteAllCars();
            ContactPerson.DeleteAllContactPeople();

            Car car = new Car();
            ContactPerson person = new ContactPerson();
            person.Surname = "Owner Surname";
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            car.Save();
            Assert.AreEqual(person.GetCarsOwned().Count, 1);
        }

        //Test the references should not be equal since the objects are reloaded each time
        [Test]
            public void TestGetCarsOwnedByPersonNotHeldInMemory()
        {
            Car.DeleteAllCars();
            ContactPerson.DeleteAllContactPeople();

            Car car = new Car();
            ContactPerson person = new ContactPerson();
            person.Surname = "Owner Surname3";
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            car.Save();
            Assert.AreEqual(car.GetOwner().ID, person.ID);

            Assert.AreEqual(1, person.GetCarsOwned().Count, "there should be one car for this person");

            IBusinessObjectCollection carsOwned = person.GetCarsOwned();
            Car carOwned = (Car) carsOwned[0];
            Assert.AreEqual(car.ID, carOwned.ID);
            IBusinessObjectCollection carsOwned2 = person.GetCarsOwned();
            Assert.IsFalse(ReferenceEquals(carsOwned, carsOwned2),
                           "The references should not be equal since the collection should be reloaded each time");
        }
    }
}