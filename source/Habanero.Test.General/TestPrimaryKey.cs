using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTestPrimaryKey //:TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        #endregion

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test, Ignore("Difficult to imagine how to implement this")]
        public void TestPrimaryKey_ToStringUpper_WhenTypeIsGuid()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            pkDef.Add(propDef1);
            BOPrimaryKey primaryKey = new BOPrimaryKey(pkDef);

            IBOProp boProp = new BOProp(propDef1, Guid.NewGuid());
            primaryKey.Add(boProp);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string actualIDToString = primaryKey.ToString();
            string getObjectID = primaryKey.GetObjectId();

            //---------------Test Result -----------------------
            Assert.IsNotNull(actualIDToString);
            Assert.IsFalse(string.IsNullOrEmpty(actualIDToString));
            Assert.AreEqual(actualIDToString.ToUpper(), actualIDToString);
            Assert.AreEqual(actualIDToString.ToUpper(), getObjectID);
        }
    }
}