using Chillisoft.Bo.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
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

            Car car = Car.GetNewCar();
            ContactPerson person = ContactPerson.GetNewContactPerson();
            person.Surname = "Owner Surname";
            person.ApplyEdit();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            car.ApplyEdit();
            Assert.AreEqual(person.GetCarsOwned().Count, 1);
        }

        [Test]
        //Test the references should not be equal since the objects are reloaded each time
            public void TestGetCarsOwnedByPersonNotHeldInMemory()
        {
            Car.DeleteAllCars();
            ContactPerson.DeleteAllContactPeople();

            Car car = Car.GetNewCar();
            ContactPerson person = ContactPerson.GetNewContactPerson();
            person.Surname = "Owner Surname3";
            person.ApplyEdit();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            car.ApplyEdit();
            Assert.AreEqual(car.GetOwner().ID, person.ID);

            Assert.AreEqual(1, person.GetCarsOwned().Count, "there should be one car for this person");

            BusinessObjectBaseCollection carsOwned = person.GetCarsOwned();
            Car carOwned = (Car) carsOwned.item(0);
            Assert.AreEqual(car.ID, carOwned.ID);
            BusinessObjectBaseCollection carsOwned2 = person.GetCarsOwned();
            Assert.IsFalse(object.ReferenceEquals(carsOwned, carsOwned2),
                           "The references should not be equal since the collection should be reloaded each time");
        }
    }
}