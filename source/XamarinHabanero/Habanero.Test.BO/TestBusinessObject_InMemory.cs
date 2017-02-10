using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBusinessObject_InMemory : TestBusinessObject
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetup()
        {
            SetupDataAccessor();
        }

        protected override void SetupDataAccessor()
        {
            FixtureEnvironment.SetupInMemoryDataAccessor();
        }

        [SetUp]
        public override void SetupTest()
        {
            SetupDataAccessor();
            FixtureEnvironment.ClearBusinessObjectManager();
            TestUtil.WaitForGC();
            ClassDef.ClassDefs.Clear();
            //new Address();
        }


        [Test]
        public void Test_ToString_WhenHasObjectID_ShouldReturnObjectIDToString()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            Car myBO = new Car();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string toString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsValue().ToString(), toString);
        }
        [Test]
        public void Test_ToString_WhenHasCompositePrimaryKey_AndValueSet_ShouldReturnAggregateOfKeyProps()
        {
            //---------------Set up test pack-------------------
            BOWithCompositePK.LoadClassDefs();
            var myBO = new BOWithCompositePK();
            const string pk1Prop1Value = "somevalue";
            const string pk1Prop2Value = "anothervalue";
            myBO.PK1Prop1 = pk1Prop1Value;
            myBO.PK1Prop2 = pk1Prop2Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            Assert.IsNotNull(myBO.PK1Prop2);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.ToString(), actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasCompositePrimaryKey_AndValueSet_ShouldReturnGuidIdToString()
        {
            //---------------Set up test pack-------------------
            BOWithCompositePK.LoadClassDefs();
            var myBO = new BOWithCompositePK();
            //---------------Assert Precondition----------------
            Assert.IsNullOrEmpty(myBO.PK1Prop1);
            Assert.IsNullOrEmpty(myBO.PK1Prop2);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }
        
        [Test]
        public void Test_ToString_WhenHasStringPKProp_AndValueSet_ShouldReturnTheSingleValueAsAToString()
        {
            //---------------Set up test pack-------------------
            BOWithStringPKProp.LoadClassDefs();
            var myBO = new BOWithStringPKProp();
            const string pk1Prop1Value = "somevalue";
            myBO.PK1Prop1 = pk1Prop1Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(pk1Prop1Value, actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasStringPKProp_AndValueSet_ShouldReturnGuidIDToString()
        {
            //---------------Set up test pack-------------------
            BOWithStringPKProp.LoadClassDefs();
            var myBO = new BOWithStringPKProp();
            //---------------Assert Precondition----------------
            Assert.IsNullOrEmpty(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }       
        [Test]
        public void Test_ToString_WhenHasIntPKProp_AndValueSet_ShouldReturnTheSingleValueAsAToString()
        {
            //---------------Set up test pack-------------------
            BOWithIntPKProp.LoadClassDefs();
            var myBO = new BOWithIntPKProp();
            int pk1Prop1Value = RandomValueGen.GetRandomInt();
            myBO.PK1Prop1 = pk1Prop1Value;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(pk1Prop1Value.ToString(), actualToString);
        }
        [Test]
        public void Test_ToString_WhenHasIntPKProp_AndValueSet_ShouldReturnGuidIDToString()
        {
            //---------------Set up test pack-------------------
            BOWithIntPKProp.LoadClassDefs();
            var myBO = new BOWithIntPKProp();
            //---------------Assert Precondition----------------
            Assert.IsNull(myBO.PK1Prop1);
            //---------------Execute Test ----------------------
            string actualToString = myBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.ID.GetAsGuid().ToString(), actualToString);
        }

    }
}