#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
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
            //BORegistry.DataAccessor = null;
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
            BORegistry.DataAccessor = new DataAccessorDB();
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
            BORegistry.DataAccessor = new DataAccessorDB();
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