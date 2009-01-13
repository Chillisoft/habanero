using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{
    [TestFixture]
    public class TestTransactionalBusinessObject : TestUsingDatabase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        #endregion

        [Test]
        public void Test_BusinessObject_TrySaveThrowsUserError_IfValidateFails()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            TransactionalBusinessObject transactionalBusinessObject = new TransactionalBusinessObject(bo);
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            string invalidReason;
            bool valid = transactionalBusinessObject.IsValid(out invalidReason);
            //---------------Test Result -----------------------
            StringAssert.Contains("Test Prop' is a compulsory field and has no value", invalidReason);
            Assert.IsFalse(valid);
            Assert.IsFalse(bo.Status.IsValid());
        }
    }
}