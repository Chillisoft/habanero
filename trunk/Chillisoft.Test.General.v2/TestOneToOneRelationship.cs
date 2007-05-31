using NUnit.Framework;

namespace Chillisoft.Test.General.v2
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

            Car car = Car.GetNewCar();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.ApplyEdit();
            Engine engine = Engine.GetNewEngine();

            engine.SetPropertyValue("EngineNo", "NO111");
            engine.SetPropertyValue("CarID", car.GetPropertyValue("CarID"));
            engine.ApplyEdit();

            Assert.AreEqual(car.GetEngine().ID, engine.ID);
            Assert.AreEqual(engine.GetCar().ID, car.ID);
        }
    }
}