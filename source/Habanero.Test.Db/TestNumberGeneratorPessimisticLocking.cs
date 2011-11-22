using System;
using System.Threading;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestNumberGeneratorPessimisticLocking
    {
// ReSharper disable InconsistentNaming
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();//ensure that a new BOManagager.Instance is used
//            BORegistry.DataAccessor = new DataAccessorInMemory();
            SetupDBDataAccessor();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        private static void SetupDBDataAccessor()
        {
            TestUsingDatabase.SetupDBDataAccessor();
        }
        [Test]
        public void Test_Construct_ShouldLoadClassDefs_ForBOSequenceNumberLocking()
        {
            //---------------Set up test pack-------------------
            var classDefCol = ClassDef.ClassDefs;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            INumberGenerator numberGenerator = new NumberGeneratorPessimisticLocking("ReturnDeliveryTransactionNumber");
            //---------------Test Result -----------------------
            Assert.IsNotNull(numberGenerator);
            Assert.IsTrue(classDefCol.Contains(typeof(BOSequenceNumberLocking)), "ClassDef should contain Def for number gen");
        }

        [Test]
        public void Test_ConstructTwice_ShouldNotCauseError()
        {
            //---------------Set up test pack-------------------
            var classDefCol = ClassDef.ClassDefs;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            new NumberGeneratorPessimisticLocking("ReturnDeliveryTransactionNumber");
            INumberGenerator numberGenerator2 = new NumberGeneratorPessimisticLocking("ReturnDeliveryTransactionNumber");
            //---------------Test Result -----------------------
            Assert.IsNotNull(numberGenerator2);
            Assert.AreEqual(1, classDefCol.Count);
            Assert.IsTrue(classDefCol.Contains(typeof(BOSequenceNumberLocking)), "ClassDef should contain Def for number gen");
        }

        [Test]
        public void ReleaseLocks_WhenHasLock_ShouldReleaseTheLocks_FixBugBug_2136()
        {
            //---------------Set up test pack-------------------
            var numberType = "SomeType" + TestUtil.GetRandomInt();  
            var numberGenerator = new NumberGeneratorPessimisticLockingStub2(numberType);
            numberGenerator.CallReleaseLocks();
            numberGenerator.NextNumber();
            //---------------Assert Precondition----------------
            Assert.IsTrue(numberGenerator.IsLocked,"Should be locked");
            //---------------Execute Test ----------------------
            numberGenerator.CallReleaseLocks();
            //---------------Test Result -----------------------
            Assert.IsFalse(numberGenerator.IsLocked, "Should not be locked");
        }

        [Test]
        public void ReleaseLocks_WhenHasLock_ShouldReleaseTheLocksInDatabase_FixBugBug_2136()
        {
            //---------------Set up test pack-------------------
            var numberType = "SomeType" + TestUtil.GetRandomInt();
            var numberGenerator = new NumberGeneratorPessimisticLockingStub2(numberType);
            numberGenerator.CallReleaseLocks();
            var nextNumber = numberGenerator.NextNumber();
            //---------------Assert Precondition----------------
            Assert.IsTrue(numberGenerator.IsLocked, "Should be locked");
            var numberGenerator2 = new NumberGeneratorPessimisticLockingStub2(numberType);
            Assert.IsTrue(numberGenerator2.IsLocked, "Newly loaded one from the DB should be unlocked");
            //---------------Execute Test ----------------------
            numberGenerator.CallReleaseLocks();
            //---------------Test Result -----------------------
            Assert.IsFalse(numberGenerator.IsLocked, "Should not be locked");

            // Check that newly loaded number Gen is unlocked
            var numberGenerator3 = new NumberGeneratorPessimisticLockingStub2(numberType);
            Assert.IsFalse(numberGenerator3.IsLocked, "Newly loaded number Gen from the DB should be unlocked");
        }

        private class NumberGeneratorPessimisticLockingStub : NumberGeneratorPessimisticLocking
        {
            public NumberGeneratorPessimisticLockingStub() : base("SomeType",1)
            {
            }
            public void CallReleaseLocks()
            {
                base.ReleaseLocks();
            }
        }
        private class NumberGeneratorPessimisticLockingStub2 : NumberGeneratorPessimisticLocking
        {
            public NumberGeneratorPessimisticLockingStub2(string numberType)
                : base(numberType, 1)
            {
            }
            public void CallReleaseLocks()
            {
                base.ReleaseLocks();
            }
        }
    }
}