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
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//ensure that a new BOManagager.Instance is used
            BORegistry.DataAccessor = new DataAccessorInMemory();
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
    }
}