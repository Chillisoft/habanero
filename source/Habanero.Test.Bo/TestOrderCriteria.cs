using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestOrderCriteria : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }
        [SetUp]
        public  void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
           
        }



        [Test]
        public void TestAdd()
        {
            //---------------Set up test pack-------------------
 
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("TestProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, orderCriteria.Fields.Count);
            Assert.Contains(new OrderCriteria.Field("TestProp", OrderCriteria.SortDirection.Ascending), orderCriteria.Fields);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestAdd_Field()
        {
            //---------------Set up test pack-------------------
            OrderCriteria.Field field = new OrderCriteria.Field("TestProp", OrderCriteria.SortDirection.Descending);
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add(field);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, orderCriteria.Fields.Count);
            Assert.Contains(field, orderCriteria.Fields);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestMultiple()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("TestProp");  
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            orderCriteria.Add("TestProp2", OrderCriteria.SortDirection.Descending);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, orderCriteria.Fields.Count);
            Assert.Contains(new OrderCriteria.Field("TestProp2", OrderCriteria.SortDirection.Descending), orderCriteria.Fields);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareGreater()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");

            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "zzzzzz";
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "ffffff";
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(cp1, cp2);
            //---------------Test Result -----------------------
            Assert.Greater(comparisonResult, 0);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareLess()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");

            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "aaaaaa";
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "bbbbbb";
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(cp1, cp2);
            //---------------Test Result -----------------------
            Assert.Less(comparisonResult, 0);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareLess_Desc()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname", OrderCriteria.SortDirection.Descending);
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "aaaaaa";
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "bbbbbb";
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(cp1, cp2);
            //---------------Test Result -----------------------
            Assert.Greater(comparisonResult, 0);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname");

            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "aaaaaa";
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = "aaaaaa";
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(cp1, cp2);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, comparisonResult);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareTwoPropsWithSameFirstValue_Less()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria().Add("Surname").Add("FirstName");

            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.Surname = "bbbb";
            cp1.FirstName = "aaaa";
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.Surname = cp1.Surname;
            cp2.FirstName = "zzzz";
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(cp1, cp2);
            //---------------Test Result -----------------------
            Assert.Less(comparisonResult, 0);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompare_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "2";
            Car car2 = new Car();
            car2.CarRegNo = "2";

            Engine engine1 = new Engine();
            engine1.CarID = car1.CarID;
            engine1.EngineNo = "20";

            Engine engine2 = new Engine();
            engine2.CarID = car2.CarID;
            engine2.EngineNo = "50";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(car2);
            committer.AddBusinessObject(engine1);
            committer.AddBusinessObject(engine2);
            committer.CommitTransaction();

            OrderCriteria orderCriteria = OrderCriteria.FromString("Car.CarRegNo, EngineNo");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            int comparisonResult = orderCriteria.Compare(engine1, engine2);
            //---------------Test Result -----------------------
            Assert.Less(comparisonResult, 0, "engine1 should be less as the car regnos are equal and its engine no is less");
            //---------------Tear Down -------------------------     
        }

        [Test]
        public void TestField_Constructor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            string name = "TestProp";
            OrderCriteria.Field field = new OrderCriteria.Field(name, OrderCriteria.SortDirection.Descending);

            //---------------Test Result -----------------------
            Assert.AreEqual(name, field.Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, field.SortDirection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestField_ToString()
        {
            //---------------Set up test pack-------------------
            string name = "TestProp";
            OrderCriteria.Field field = new OrderCriteria.Field(name, OrderCriteria.SortDirection.Descending);
            //---------------Execute Test ----------------------
            string fieldAsString = field.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(name + " DESC", fieldAsString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestField_Source()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            OrderCriteria.Field field = new OrderCriteria.Field("TestProp", OrderCriteria.SortDirection.Descending);
            string source = Guid.NewGuid().ToString("N");
            field.Source = source;
            //---------------Test Result -----------------------
            Assert.AreEqual(source, field.Source);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestField_ToStringSource()
        {
            //---------------Set up test pack-------------------
            string name = "TestProp";
            OrderCriteria.Field field = new OrderCriteria.Field(name, OrderCriteria.SortDirection.Descending);
            string source = Guid.NewGuid().ToString("N");
            field.Source = source;
            //---------------Execute Test ----------------------
            string fieldAsString = field.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("{0}.{1} DESC", source, name), fieldAsString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestField_FromString()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            OrderCriteria.Field field = OrderCriteria.Field.FromString("TestProp dEsc");
            //---------------Test Result -----------------------
            Assert.AreEqual("TestProp", field.Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, field.SortDirection);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFieldFromString_WithSource()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            OrderCriteria.Field field = OrderCriteria.Field.FromString("MyRelationship.TestProp dEsc");
            //---------------Test Result -----------------------
            Assert.AreEqual("MyRelationship", field.Source);
            Assert.AreEqual("TestProp", field.Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, field.SortDirection);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestField_Compare_ThroughRelationship()
        {
            //---------------Set up test pack-------------------
            Car car1 = new Car();
            car1.CarRegNo = "5";
            Car car2 = new Car();
            car2.CarRegNo = "2";

            Engine engine1 = new Engine();
            engine1.CarID = car1.CarID;
            engine1.EngineNo = "20";

            Engine engine2 = new Engine();
            engine2.CarID = car2.CarID;
            engine2.EngineNo = "50";

            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(car1);
            committer.AddBusinessObject(car2);
            committer.AddBusinessObject(engine1);
            committer.AddBusinessObject(engine2);
            committer.CommitTransaction();

            OrderCriteria.Field field = OrderCriteria.Field.FromString("Car.CarRegNo");

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            int comparisonResult = field.Compare(engine1, engine2);
            //---------------Test Result -----------------------
            Assert.Greater(comparisonResult, 0, "engine1 should be greater as its car's regno is greater");
            //---------------Tear Down -------------------------     
        }
 

        [Test]
        public void TestToString()
        {
            //---------------Set up test pack-------------------
            OrderCriteria orderCriteria = new OrderCriteria().Add("TestProp");
            orderCriteria.Add("TestProp2", OrderCriteria.SortDirection.Descending);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------

            string orderCriteriaString = orderCriteria.ToString();
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("TestProp ASC, TestProp2 DESC", orderCriteriaString);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("TestProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, orderCriteria.Fields.Count);
            Assert.AreEqual("TestProp", orderCriteria.Fields[0].Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Ascending, orderCriteria.Fields[0].SortDirection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_Desc()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("TestProp DESC");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, orderCriteria.Fields.Count);
            Assert.AreEqual("TestProp", orderCriteria.Fields[0].Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, orderCriteria.Fields[0].SortDirection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_Multiple()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("TestProp DESC, TestProp2 ASC");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, orderCriteria.Fields.Count);
            Assert.AreEqual("TestProp", orderCriteria.Fields[0].Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, orderCriteria.Fields[0].SortDirection);
            Assert.AreEqual("TestProp2", orderCriteria.Fields[1].Name);
            Assert.AreEqual(OrderCriteria.SortDirection.Ascending, orderCriteria.Fields[1].SortDirection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_NullString()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(orderCriteria);
            Assert.AreEqual(0, orderCriteria.Fields.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_BlankString()
        {
            //---------------Set up test pack-------------------
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString(" ");
            //---------------Test Result -----------------------
            Assert.IsNotNull(orderCriteria);
            Assert.AreEqual(0, orderCriteria.Fields.Count);
            //---------------Tear Down -------------------------
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestFromString_InvalidSortOrder()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            try
            {
                OrderCriteria orderCriteria = OrderCriteria.FromString("TestProp adfe");
                Assert.Fail("FromString should have failed due to a badly named sort order");
            //---------------Test Result -----------------------
            } catch (ArgumentException ex)
            {
                StringAssert.Contains("is an invalid sort order. Valid options are ASC and DESC", ex.Message);
                throw;
            }
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestFromString_SortOrder_IsCaseInsensitive()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = OrderCriteria.FromString("TestProp dEsc, TestProp2 aSc");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, orderCriteria.Fields.Count);
            Assert.AreEqual(OrderCriteria.SortDirection.Descending, orderCriteria.Fields[0].SortDirection);
            Assert.AreEqual(OrderCriteria.SortDirection.Ascending, orderCriteria.Fields[1].SortDirection);
            //---------------Tear Down -------------------------
        }

     
    }
}