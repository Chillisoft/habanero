using Habanero.Bo;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestCarOwnerRelationship.
    /// </summary>
    /// 	
    [TestFixture]
    public class TestCarOwnerRelationship : TestUsingDatabase
    {
        public TestCarOwnerRelationship()
        {
        }

        public static void RunTest()
        {
            TestCarOwnerRelationship test = new TestCarOwnerRelationship();
            test.TestGetCarOwner();
        }

        [Test]
        public void TestGetCarOwner()
        {
            Car.DeleteAllCars();
            ContactPerson.DeleteAllContactPeople();

            Car car = new Car();
            ContactPerson person = new ContactPerson();
            person.Surname = "Owner Surname";
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            Assert.AreEqual(car.GetOwner().ID, person.ID);
        }

        //Test that the related object does not have a reference held
        public void TestGetCarOwnerIsNotSame()
        {
            Car.DeleteAllCars();
            ContactPerson.DeleteAllContactPeople();

            Car car = new Car();
            ContactPerson person = new ContactPerson();
            person.Surname = "Owner Surname";
            person.Save();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("OwnerId", person.GetPropertyValue("ContactPersonID"));
            Assert.AreEqual(car.GetOwner().ID, person.ID);
            Assert.IsTrue(object.ReferenceEquals(person, car.GetOwner()),
                          "Should be the same since GetOwner recovers object from object manager");

            person = car.GetOwner();
            ContactPerson.ClearContactPersonCol();
            Assert.IsFalse(object.ReferenceEquals(person, car.GetOwner()),
                           "Should not be the same since the Owner reference is being " +
                           " not maintained in the car class and the object is therefore " +
                           " being reloaded each time");
        }

        [Test]
        public void TestGetCarsOwnedByPerson()
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

            Car carOwned = (Car) person.GetCarsOwned()[0];
            Assert.AreEqual(car.ID, carOwned.ID);
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