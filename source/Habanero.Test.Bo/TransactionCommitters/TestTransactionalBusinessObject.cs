//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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

        [Test]
        public void Test_BusinessObject_TransactionID()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            TransactionalBusinessObject transactionalBusinessObject = new TransactionalBusinessObject(bo);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string transactionID = transactionalBusinessObject.TransactionID();
            //---------------Test Result -----------------------
            Assert.AreEqual(bo.ID.ObjectID.ToString(), transactionID);
        }


        [Test]
        public void Test_BusinessObject_TransactionID_CompositeKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO bo = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            TransactionalBusinessObject transactionalBusinessObject = new TransactionalBusinessObject(bo);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string transactionID = transactionalBusinessObject.TransactionID();
            //---------------Test Result -----------------------
            Assert.AreEqual(bo.ID.ObjectID.ToString(), transactionID);
        }
    }
}