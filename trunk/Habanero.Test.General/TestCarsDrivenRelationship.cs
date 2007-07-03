using Habanero.Bo;
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
            Car.ClearCarCol();
            ContactPersonCompositeKey.DeleteAllContactPeople();
            ContactPersonCompositeKey.ClearContactPersonCol();

            Car car = Car.GetNewCar();
            ContactPersonCompositeKey person = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            person.SetPropertyValue("Surname", "Driver Surname");
            person.SetPropertyValue("PK1Prop1", "Driver1");
            person.SetPropertyValue("PK1Prop2", "Driver2");
            person.ApplyEdit();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("DriverFK1", person.GetPropertyValue("PK1Prop1"));
            car.SetPropertyValue("DriverFK2", person.GetPropertyValue("PK1Prop2"));
            car.ApplyEdit();
            Assert.AreEqual(car.GetDriver().ID, person.ID);
            Assert.AreEqual(person.GetCarsDriven().Count, 1);
        }

        [Test]
        //TODO: Peter - I Commented out the last line because I had to make the collection reload each time it's
            //retrieved in case a new object is created that would go into this collection.  There are currently no 
            //events caught by a collection if a new object is created.
            public void TestGetCarsDrivenByPersonHeldInMemory()
        {
            Car.DeleteAllCars();
            ContactPersonCompositeKey.DeleteAllContactPeople();

            Car car = Car.GetNewCar();
            ContactPersonCompositeKey person = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            person.SetPropertyValue("Surname", "Driver Surname");
            person.SetPropertyValue("PK1Prop1", "Driver11");
            person.SetPropertyValue("PK1Prop2", "Driver21");
            person.ApplyEdit();

            car.SetPropertyValue("CarRegNo", "NP32459");
            car.SetPropertyValue("DriverFK1", person.GetPropertyValue("PK1Prop1"));
            car.SetPropertyValue("DriverFK2", person.GetPropertyValue("PK1Prop2"));
            car.ApplyEdit();

            Assert.AreEqual(car.GetDriver().ID, person.ID);

            Assert.AreEqual(1, person.GetCarsDriven().Count, "there should be one car for this person");

            BusinessObjectCollection<BusinessObject> carsDriven = person.GetCarsDriven();
            Car carDriven = (Car) carsDriven.item(0);
            Assert.AreEqual(car.ID, carDriven.ID);
            BusinessObjectCollection<BusinessObject> carsDriven2 = person.GetCarsDriven();
            //Assert.IsTrue(object.ReferenceEquals(carsDriven, carsDriven2), "The references should be equal since the collection should be kept in memory");
        }
    }
}