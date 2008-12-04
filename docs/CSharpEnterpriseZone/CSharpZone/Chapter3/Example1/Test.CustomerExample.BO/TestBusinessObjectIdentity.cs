using System;
using CustomerExample.BO;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Test.CustomerExample.BO
{
    [TestFixture]
    public class TestBusinessObjectIdentity : TestBase
    {
        [SetUp]
        public override void SetupTest()
        {
            //Runs every time that any testmethod is executed
            base.SetupTest();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public override void TearDownTest()
        {
            //runs every time any testmethod is complete
            base.TearDownTest();
        }

        [Test]
        public void Test_CreateCustomerWithIdentity()
        {
            //This test shows the that the PrimaryKeyDef and BOPrimaryKey
            // are set up according to the class definition defined for 
            // customer. It also shows that the CustomerID is automatically
            // set to a new GUID Value.

           //---------------Execute Test ----------------------
            Customer customer = new Customer();

            //---------------Test Result -----------------------
            ClassDef customerClassDef = customer.ClassDef;
            PrimaryKeyDef primaryKeyDef = customerClassDef.PrimaryKeyDef;
            Assert.AreEqual(1, primaryKeyDef.Count);

            IPrimaryKey customerPrimaryKey = customer.ID;
            Assert.AreEqual(1, customerPrimaryKey.Count);
            Assert.IsTrue(customerPrimaryKey.Contains("CustomerID"));
            IBOProp customerIDBOProp = customerPrimaryKey["CustomerID"];

            Assert.IsNotNull(customerIDBOProp.Value, "Since the CustomerID is an object id it should be set to a value");
        }

        [Test]
        public void Test_CreateBO_WithNonGuidID()
        {
            //This test shows the that the PrimaryKeyDef and BOPrimaryKey
            // are set up according to the class definition defined for 
            // BOWithNonGuidID. It also shows that the Properties
            // for the Business object are not set to any value.
            //---------------Execute Test ----------------------
            BOWithNonGuidID boWithNonGuidID = new BOWithNonGuidID();

            //---------------Test Result -----------------------
            ClassDef boWithNonGuidIDClassDef = boWithNonGuidID.ClassDef;
            PrimaryKeyDef primaryKeyDef = boWithNonGuidIDClassDef.PrimaryKeyDef;
            Assert.AreEqual(3, primaryKeyDef.Count);

            IPrimaryKey boWithNonGuidIDPrimaryKey = boWithNonGuidID.ID;
            Assert.AreEqual(3, boWithNonGuidIDPrimaryKey.Count);
            Assert.IsTrue(boWithNonGuidIDPrimaryKey.Contains("Surname"));
            IBOProp surnameIDBOProp = boWithNonGuidIDPrimaryKey["Surname"];

            Assert.IsNull(surnameIDBOProp.Value, 
                "Since the surnameIDBOProp is not an object id it should be not be set to a value on object creation");
        }
    }

    internal class BOWithNonGuidID:BusinessObject
    {
        /// <summary>
        /// Overriding this method allows the developer to construct the class definitions for
        ///   a business object directly in code instead of based on ClassDefs.xml
        /// </summary>
        /// <returns></returns>
        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }
        private static ClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof(BOWithNonGuidID)))
            {
                return ClassDef.ClassDefs[typeof(BOWithNonGuidID)];
            }
            return CreateClassDef();
        }
        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();
            PrimaryKeyDef primaryKey = CreatePrimaryKey(lPropDefCol);

            const RelationshipDefCol relDefs = null;
            KeyDefCol keysCol = new KeyDefCol();
            ClassDef lClassDef =
                new ClassDef(typeof(BOWithNonGuidID), primaryKey, lPropDefCol, keysCol, relDefs);

            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PrimaryKeyDef CreatePrimaryKey(PropDefCol lPropDefCol)
        {
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = false;
            primaryKey.Add(lPropDefCol["Surname"]);
            primaryKey.Add(lPropDefCol["FirstName"]);
            primaryKey.Add(lPropDefCol["DateOfBirth"]);
            return primaryKey;
        }

        protected static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef = new PropDef("Surname", typeof(String), PropReadWriteRule.ReadWrite, null);
            propDef.AddPropRule(new PropRuleString("ContactPerson-" + propDef.PropertyName, "", 2, 50, null));
            lPropDefCol.Add(propDef);

            propDef = new PropDef("FirstName", typeof(String), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("DateOfBirth", typeof(DateTime), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(propDef);

            return lPropDefCol;
        }
    }
}