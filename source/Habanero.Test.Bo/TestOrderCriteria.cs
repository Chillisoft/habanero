using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestOrderCriteria 
    {
        [SetUp]
        public  void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            
        }



        [Test]
        public void TestCreate()
        {
            //---------------Set up test pack-------------------
 
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria("TestProp");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, orderCriteria.Count);
            Assert.IsTrue(orderCriteria.Fields.Contains("TestProp"));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestMultiple()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
  
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            OrderCriteria orderCriteria = new OrderCriteria("TestProp");
            orderCriteria.Add("TestProp2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, orderCriteria.Count);
            Assert.IsTrue(orderCriteria.Fields.Contains("TestProp"));
            Assert.IsTrue(orderCriteria.Fields.Contains("TestProp2"));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCompareGreater()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria("Surname");

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
            OrderCriteria orderCriteria = new OrderCriteria("Surname");

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
        public void TestCompareEquals()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria("Surname");

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
        public void TestTwoPropsWithSameFirstValue_Less()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            OrderCriteria orderCriteria = new OrderCriteria("Surname");
            orderCriteria.Add("FirstName");

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
    }
}