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

        [Test]
        //Test the references should not be equal since the objects are reloaded each time
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

            BusinessObjectCollection<BusinessObject> carsOwned = person.GetCarsOwned();
            Car carOwned = (Car) carsOwned[0];
            Assert.AreEqual(car.ID, carOwned.ID);
            BusinessObjectCollection<BusinessObject> carsOwned2 = person.GetCarsOwned();
            Assert.IsFalse(object.ReferenceEquals(carsOwned, carsOwned2),
                           "The references should not be equal since the collection should be reloaded each time");
        }
    }
}