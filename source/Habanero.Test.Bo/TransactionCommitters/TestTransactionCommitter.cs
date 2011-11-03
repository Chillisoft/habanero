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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{ // ReSharper disable InconsistentNaming

    [TestFixture]
    public class TestTransactionCommitter : TestUsingDatabase
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

        private const string _customRuleErrorMessage = "Broken Rule";



        private static MockBO CreateSavedMockBO()
        {
            MockBO fakeBO = new MockBO();
            fakeBO.Save();
            return fakeBO;
        }

        internal class FakeBOWithCustomRule : MockBO
        {
            /// <summary>
            /// Override this method in subclasses of BusinessObject to check custom rules for that
            /// class.  The default implementation returns true and sets customRuleErrors to the empty string.
            /// </summary>
            /// <param name="customRuleErrors">The error string to display</param>
            /// <returns>true if no custom rule errors are encountered.</returns>
            protected override bool AreCustomRulesValid(out string customRuleErrors)
            {
                customRuleErrors = _customRuleErrorMessage;
                return false;
            }
        }

        private class TransactionalBusinessObjectStub : TransactionalBusinessObject
        {
            public TransactionalBusinessObjectStub(IBusinessObject businessObject)
                : base(businessObject)
            {
            }
        }

        private class FakeBOWithBeforeSaveUpdatesCompulsoryField : MockBO
        {
            private bool _updateBeforePersistingExecuted;

            public FakeBOWithBeforeSaveUpdatesCompulsoryField()
            {
                _updateBeforePersistingExecuted = false;
            }

            /// <summary>
            /// Override this method in subclasses of BusinessObject to check custom rules for that
            /// class.  The default implementation returns true and sets customRuleErrors to the empty string.
            /// </summary>
            /// <param name="customRuleErrors">The error string to display</param>
            /// <returns>true if no custom rule errors are encountered.</returns>
            protected override bool AreCustomRulesValid(out string customRuleErrors)
            {
                customRuleErrors = "";
                if (!_updateBeforePersistingExecuted)
                {
                    customRuleErrors = "UpdateBeforePersisting not executed";
                    return false;
                }
                return true;
            }

            ///<summary>
            /// Executes any custom code required by the business object before it is persisted to the database.
            /// This has the additionl capability of creating or updating other business objects and adding these
            /// to the transaction committer.
            /// <remarks> Recursive call to UpdateObjectBeforePersisting will not be done i.e. it is the bo developers responsibility to implement</remarks>
            ///</summary>
            ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
            protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
            {
                _updateBeforePersistingExecuted = true;
            }
        }

        private class FakeBOWithUpdateBeforePersisting : MockBO
        {
            /// <summary>
            /// Steps to carry out before the Save() command is run. You can add objects to the current
            /// transaction using this method, such as a database number generator.  No validity checks are 
            /// made to the BusinessObject after this step, so be careful not to invalidate the object.
            /// </summary>
            /// <param name="transactionCommitter">The current transaction committer - any objects added to this will
            /// be committed in the same transaction as this one.</param>
            protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
            {
                transactionCommitter.AddTransaction(new StubSuccessfullTransaction());
            }
        }

        private class FakeBOWithUpdateBeforePersistingLevel2 : MockBO
        {
            /// <summary>
            /// Steps to carry out before the Save() command is run. You can add objects to the current
            /// transaction using this method, such as a database number generator.  No validity checks are 
            /// made to the BusinessObject after this step, so be careful not to invalidate the object.
            /// </summary>
            /// <param name="transactionCommitter">The current transaction committer - any objects added to this will
            /// be committed in the same transaction as this one.</param>
            protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
            {
                transactionCommitter.AddBusinessObject(new FakeBOWithUpdateBeforePersisting());
            }
        }

      

        [Test]
        public void Test_CannotAddSameTransactionToCommitter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committer = new TransactionCommitterStub();
            StubSuccessfullTransaction transaction = new StubSuccessfullTransaction();
            committer.AddTransaction(transaction);

            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, committer.OriginalTransactions.Count);

            //---------------Execute Test ----------------------
            committer.AddTransaction(transaction);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestAddBusinessObjectToTransaction_NotUpdateBeforePersisting_2LevelsDeep()
        {
            //---------------Set up test pack-------------------

            FakeBOWithUpdateBeforePersistingLevel2 fakeBO = new FakeBOWithUpdateBeforePersistingLevel2();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(fakeBO);
            //---------------Execute Test ----------------------

            committer.AddTransaction(trnBusObj);


            //---------------Test Result -----------------------
            Assert.AreEqual(1, committer.OriginalTransactions.Count,
                            "There should only be the recently added business object not any of its object that are added in update before persist");
        }

        [Test]
        public void TestAddBusinessObjectToTransactionInUpdateBeforePersisting()
        {
            //---------------Set up test pack-------------------

            FakeBOWithUpdateBeforePersisting fakeBO = new FakeBOWithUpdateBeforePersisting();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(fakeBO);
            committer.AddTransaction(trnBusObj);
            //---------------Execute Test ----------------------

            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestAddBusinessObjectToTransactionInUpdateBeforePersisting_2LevelsDeep()
        {
            //---------------Set up test pack-------------------

            FakeBOWithUpdateBeforePersistingLevel2 fakeBO = new FakeBOWithUpdateBeforePersistingLevel2();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(fakeBO);
            committer.AddTransaction(trnBusObj);
            //---------------Execute Test ----------------------

            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestAddTransactionsToATransactionCommiter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committer = new TransactionCommitterStub();

            //---------------Execute Test ----------------------
            committer.AddTransaction(new StubSuccessfullTransaction());
            committer.AddTransaction(new StubSuccessfullTransaction());

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestCommitAddedTransactions()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committer = new TransactionCommitterStub();
            StubSuccessfullTransaction transactional1 = new StubSuccessfullTransaction();
            committer.AddTransaction(transactional1);
            StubSuccessfullTransaction transactional2 = new StubSuccessfullTransaction();
            committer.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            List<Guid> executedTransactions  = committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, executedTransactions.Count);
            Assert.IsTrue(transactional1.Committed);
            Assert.IsTrue(transactional2.Committed);
        }

        [Test]
        public void TestMessageTwoPersistSimpleBO_Failing()
        {
            //---------------Set up test pack-------------------
            FakeBOWithCustomRule fakeBO = new FakeBOWithCustomRule();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(fakeBO);

            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            committerDB.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            try
            {
                 committerDB.CommitTransaction();
            }
            catch (BusObjectInAnInvalidStateException ex)
                //---------------Test Result -----------------------
            {
                Assert.IsTrue(ex.Message.Contains(_customRuleErrorMessage));
                Assert.IsTrue(ex.Message.Contains("Surname"));
            }
        }

        [Test]
        public void TestPersistSimpleBO_FailingCustomRules()
        {
            //---------------Set up test pack-------------------
            FakeBOWithCustomRule fakeBO = new FakeBOWithCustomRule();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(fakeBO);

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
            catch (BusObjectInAnInvalidStateException ex)
                //---------------Test Result -----------------------
            {
                Assert.IsTrue(ex.Message.Contains(_customRuleErrorMessage));
            }
        }

        [Test]
        public void Test_AddNull_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committerDB = new TransactionCommitterStub();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
            //---------------Execute Test ----------------------
            committerDB.AddBusinessObject(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
        }
        [Test]
        public void Test_InsertNull_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committerDB = new TransactionCommitterStub();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
            //---------------Execute Test ----------------------
            committerDB.InsertBusinessObject(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
        }
        [Test]
        public void Test_AddCreatedBO_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            MockBO fakeBO = new MockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            fakeBO.MockBOProp1 = mockBOProp1;
            fakeBO.MarkForDelete();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            //---------------Assert Preconditions---------------
            Assert.IsTrue(fakeBO.Status.IsNew);
            Assert.IsTrue(fakeBO.Status.IsDeleted);
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
            //---------------Execute Test ----------------------
            committerDB.AddBusinessObject(fakeBO);
            //---------------Test Result -----------------------
            Assert.IsTrue(fakeBO.Status.IsNew);
            Assert.IsTrue(fakeBO.Status.IsDeleted);
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
        }
        [Test]
        public void TestPersistSimpleCreatedBO_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            MockBO fakeBO = new MockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            fakeBO.MockBOProp1 = mockBOProp1;
            
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(fakeBO);
            fakeBO.MarkForDelete();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, committerDB.OriginalTransactions.Count);
            Assert.IsTrue(fakeBO.Status.IsNew);
            Assert.IsTrue(fakeBO.Status.IsDeleted);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(fakeBO.Status.IsNew);
            Assert.IsTrue(fakeBO.Status.IsDeleted);
        }

        [Test]
        public void TestPersistCreated_Deleted_Invalid_BO_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            MockBOWithCompulsoryField mockBo = new MockBOWithCompulsoryField();
            mockBo.MockBOProp1 = null;

            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(mockBo);
            mockBo.MarkForDelete();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, committerDB.OriginalTransactions.Count);
            Assert.IsTrue(mockBo.Status.IsNew);
            Assert.IsTrue(mockBo.Status.IsDeleted);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(mockBo.Status.IsNew);
            Assert.IsTrue(mockBo.Status.IsDeleted);
        }

        [Test]
        public void TestPersistSimpleBO_Update_NotUsingTransactionalBusinessObject()
        {
            //---------------Set up test pack-------------------
            MockBO fakeBO = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            fakeBO.MockBOProp1 = mockBOProp1;
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(fakeBO);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(fakeBO);
        }

        [Test]
        public void TestUpdateBeforePersisting_ExecutedBeforeValidation()
        {
            //---------------Set up test pack-------------------

            FakeBOWithBeforeSaveUpdatesCompulsoryField fakeBO = new FakeBOWithBeforeSaveUpdatesCompulsoryField();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(fakeBO);
            committer.AddTransaction(trnBusObj);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            //Business object will throw an exception if executed in the incorrect order.
            Assert.AreEqual(1, committer.OriginalTransactions.Count);
        }

        [Test]
        //This Acceptance Test has a corresponding unit test on the CheckDuplicateIdentifier method in TestTransactionalBusinessObject
        public void Test_Commit_WhenTransactionCommitterHasAddedBOWithSameUniqueKeyAsMarkedForDeleteBO_WhenDeletedBOInTransaction_ShouldNotThrowDuplicateError()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager = new BusinessObjectManagerNull();
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname,BOTestUtils.RandomString);
            contactPersonTestBO.Save();
            var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            contactPersonTestBO.MarkForDelete();
            committer.AddBusinessObject(contactPersonTestBO);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            committer.AddBusinessObject(newContactPersonTestBOWithSameSurname);
            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDeleted);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.IsFalse(newContactPersonTestBOWithSameSurname.Status.IsDeleted);
            Assert.AreEqual(surname, contactPersonTestBO.Props["Surname"].PersistedPropertyValueString);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsFalse(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDeleted);
            newContactPersonTestBOWithSameSurname.MarkForDelete();
            newContactPersonTestBOWithSameSurname.Save();
        }

        [Test]
        //This Acceptance Test has a corresponding unit test on the CheckDuplicateIdentifier method in TestTransactionalBusinessObject
        public void Test_Commit_WhenTransactionCommitterHasAddedBOWithSameUniqueKeyAsMarkedForDeleteBO_WhenDeletedBONotInTransaction_ShouldThrowDuplicateError()
        {
            //---------------Set up test pack-------------------
            BORegistry.BusinessObjectManager = new BusinessObjectManager();
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname,BOTestUtils.RandomString);
            contactPersonTestBO.Save();
            var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            contactPersonTestBO.MarkForDelete();
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            committer.AddBusinessObject(newContactPersonTestBOWithSameSurname);
            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDeleted);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.IsFalse(newContactPersonTestBOWithSameSurname.Status.IsDeleted);
            Assert.AreEqual(surname, contactPersonTestBO.Props["Surname"].PersistedPropertyValueString);
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<BusObjDuplicateConcurrencyControlException>(() =>
            {
                committer.CommitTransaction();
            });
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), exception.Message);
        }

        [Test]
        //This Acceptance Test has a corresponding unit test on the CheckDuplicateIdentifier method in TestTransactionalBusinessObject
        public void Test_Commit_WhenTransactionCommitterHasAddedBOsWithSameUniqueKey_ShouldThrowDuplicateError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(contactPersonTestBO);
            committer.AddBusinessObject(newContactPersonTestBOWithSameSurname);
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(surname, contactPersonTestBO.Surname);
            Assert.AreEqual(surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<BusObjDuplicateConcurrencyControlException>(() =>
            {
                committer.CommitTransaction();
            });
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), exception.Message);
        }

        [Test]
        //This Acceptance Test has a corresponding unit test on the CheckDuplicateIdentifier method in TestTransactionalBusinessObject
        public void Test_Commit_WhenTransactionCommitterHasAddedBOsWithSamePrimaryKey_ShouldThrowDuplicateError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            var surname = contactPersonTestBO.Surname;
            var firstName = contactPersonTestBO.FirstName;
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, firstName);
            var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(contactPersonTestBO);
            committer.AddBusinessObject(newContactPersonTestBOWithSameSurname);
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(contactPersonTestBO.FirstName, newContactPersonTestBOWithSameSurname.FirstName);
            Assert.AreEqual(contactPersonTestBO.Surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<BusObjDuplicateConcurrencyControlException>(() =>
            {
                committer.CommitTransaction();
            });
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}, FirstName = {1}.", surname, firstName), exception.Message);
        }

        [Test]
        //This Acceptance Test has a corresponding unit test on the CheckDuplicateIdentifier method in TestTransactionalBusinessObject
        public void Test_Commit_WhenTransactionCommitterHasAddedBOAndUpdatedBOWithSameUniqueKey_ShouldThrowDuplicateError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithKeyOnSurname();
            var surname = BOTestUtils.RandomString;
            var contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson(BOTestUtils.RandomString, BOTestUtils.RandomString);
            var newContactPersonTestBOWithSameSurname = ContactPersonTestBO.CreateUnsavedContactPerson(surname, BOTestUtils.RandomString);
            var committer = BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(contactPersonTestBO);
            committer.CommitTransaction();
            contactPersonTestBO.Surname = surname;
            committer.AddBusinessObject(contactPersonTestBO);
            committer.AddBusinessObject(newContactPersonTestBOWithSameSurname);
            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDirty);
            Assert.IsTrue(newContactPersonTestBOWithSameSurname.Status.IsNew);
            Assert.AreEqual(surname, contactPersonTestBO.Surname);
            Assert.AreEqual(surname, newContactPersonTestBOWithSameSurname.Surname);
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<BusObjDuplicateConcurrencyControlException>(() =>
            {
                committer.CommitTransaction();
            });
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("A 'Contact Person Test BO' already exists with the same identifier: Surname = {0}.", surname), exception.Message);
        }
      
    }
}