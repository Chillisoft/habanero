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