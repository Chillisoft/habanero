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
using System.Collections.Generic;
using Habanero.Base;
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
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
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
            IClassDef classDef = MyBO.LoadDefaultClassDef_CompulsoryField_TestProp();
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

        [Test]
        public void Test_CheckDuplicateIdentifier_WhenTransactionCommitterHasAddedBOWithSameUniqueKeyAsMarkedForDeleteBO_WhenDeletedBOInPendingTransactions_ShouldNotReturnError()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            contactPersonTestBO.Save();
            contactPersonTestBO.MarkForDelete();
            var transactionalBusinessObjectForDeletedDuplicate = new TransactionalBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var transactionalBusinessObject = new TransactionalBusinessObject(newContactPersonTestBOWithSameSurname);
            var pendingTransactions = new List<ITransactional> {transactionalBusinessObject, transactionalBusinessObjectForDeletedDuplicate};
            //---------------Assert Precondition----------------
            Assert.IsFalse(transactionalBusinessObjectForDeletedDuplicate.IsNew());
            Assert.IsTrue(transactionalBusinessObjectForDeletedDuplicate.IsDeleted);
            Assert.IsTrue(transactionalBusinessObject.IsNew());
            Assert.IsFalse(transactionalBusinessObject.IsDeleted);
            Assert.AreEqual(surname, contactPersonTestBO.Props["Surname"].PersistedPropertyValueString);
            //---------------Execute Test ----------------------
            var errorMessages = new List<string>();
            transactionalBusinessObject.CheckDuplicateIdentifier(pendingTransactions, errorMessages);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, errorMessages.Count);
        }

        [Test]
        public void Test_CheckDuplicateIdentifier_WhenTransactionCommitterHasAddedBOWithSameUniqueKeyAsMarkedForDeleteBO_WhenDeletedBONotInPendingTransactions_ShouldReturnError()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            contactPersonTestBO.Save();
            contactPersonTestBO.MarkForDelete();
            var transactionalBusinessObjectForDeletedDuplicate = new TransactionalBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var transactionalBusinessObject = new TransactionalBusinessObject(newContactPersonTestBOWithSameSurname);
            var pendingTransactions = new List<ITransactional> { transactionalBusinessObject };
            //---------------Assert Precondition----------------
            Assert.IsFalse(transactionalBusinessObjectForDeletedDuplicate.IsNew());
            Assert.IsTrue(transactionalBusinessObjectForDeletedDuplicate.IsDeleted);
            Assert.IsTrue(transactionalBusinessObject.IsNew());
            Assert.IsFalse(transactionalBusinessObject.IsDeleted);
            Assert.AreEqual(surname, contactPersonTestBO.Props["Surname"].PersistedPropertyValueString);
            //---------------Execute Test ----------------------
            var errorMessages = new List<string>();
            transactionalBusinessObject.CheckDuplicateIdentifier(pendingTransactions, errorMessages);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, errorMessages.Count);
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), errorMessages[0]);
        }

        [Test]
        public void Test_CheckDuplicateIdentifier_WhenTransactionCommitterHasAddedBOsWithSameUniqueKey_ShouldReturnError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var transactionalBusinessObject = new TransactionalBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var transactionalBusinessObjectDuplicate = new TransactionalBusinessObject(newContactPersonTestBOWithSameSurname);
            var pendingTransactions = new List<ITransactional> { transactionalBusinessObject, transactionalBusinessObjectDuplicate };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(surname, contactPersonTestBO.Surname);
            Assert.AreEqual(surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var errorMessages = new List<string>();
            transactionalBusinessObject.CheckDuplicateIdentifier(pendingTransactions, errorMessages);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, errorMessages.Count);
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), errorMessages[0]);
        }

        [Test]
        public void Test_CheckDuplicateIdentifier_WhenTransactionCommitterHasAddedBOsWithSamePrimaryKey_ShouldReturnError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            var surname = contactPersonTestBO.Surname;
            var firstName = contactPersonTestBO.FirstName;
            var transactionalBusinessObject = new TransactionalBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, firstName);
            var transactionalBusinessObjectDuplicate = new TransactionalBusinessObject(newContactPersonTestBOWithSameSurname);
            var pendingTransactions = new List<ITransactional> { transactionalBusinessObject, transactionalBusinessObjectDuplicate };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(contactPersonTestBO.FirstName, newContactPersonTestBOWithSameSurname.FirstName);
            Assert.AreEqual(contactPersonTestBO.Surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var errorMessages = new List<string>();
            transactionalBusinessObject.CheckDuplicateIdentifier(pendingTransactions, errorMessages);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, errorMessages.Count);
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}, FirstName = {1}.", surname, firstName), errorMessages[0]);
        }

        [Test]
        public void Test_CheckDuplicateIdentifier_WhenTransactionCommitterHasAddedBOAndUpdatedBOWithSameUniqueKey_ShouldReturnError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(BOTestUtils.RandomString, BOTestUtils.RandomString);
            contactPersonTestBO.Save();
            contactPersonTestBO.Surname = surname;

            var transactionalBusinessObject = new TransactionalBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var transactionalBusinessObjectDuplicate = new TransactionalBusinessObject(newContactPersonTestBOWithSameSurname);
            var pendingTransactions = new List<ITransactional> { transactionalBusinessObject, transactionalBusinessObjectDuplicate };
            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDirty);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(surname, contactPersonTestBO.Surname);
            Assert.AreEqual(surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var errorMessages = new List<string>();
            transactionalBusinessObject.CheckDuplicateIdentifier(pendingTransactions, errorMessages);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, errorMessages.Count);
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), errorMessages[0]);
        }
    }
}