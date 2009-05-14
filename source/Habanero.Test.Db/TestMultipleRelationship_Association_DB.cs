using Habanero.BO;
using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestMultipleRelationship_Association_DB : TestMultipleRelationship_Association
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_DB_CompositeKey()
        {
            //---------------Set up test pack-------------------
            TestUsingDatabase.SetupDBDataAccessor();
            Car car = new Car();
            car.Save();

            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.PK1Prop2 = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.GetCarsDriven().Add(car);

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(contactPerson.PK1Prop1, car.DriverFK1);
            Assert.AreEqual(contactPerson.PK1Prop2, car.DriverFK2);

            //---------------Execute Test ----------------------
            contactPerson.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            Car loadedCar = Broker.GetBusinessObject<Car>(car.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.PK1Prop1, loadedCar.DriverFK1);
            Assert.AreEqual(contactPerson.PK1Prop2, loadedCar.DriverFK2);

        }
    }
}