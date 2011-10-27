// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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