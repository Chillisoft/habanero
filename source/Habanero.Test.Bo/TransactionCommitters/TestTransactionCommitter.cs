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

using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{
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
            MockBO mockBo = new MockBO();
            mockBo.Save();
            return mockBo;
        }

        internal class MockBOWithCustomRule : MockBO
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

            protected internal override bool HasDuplicateIdentifier(out string errMsg)
            {
                errMsg = "";
                return false;
            }
        }

        private class MockBOWithBeforeSaveUpdatesCompulsoryField : MockBO
        {
            private bool _updateBeforePersistingExecuted;

            public MockBOWithBeforeSaveUpdatesCompulsoryField()
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

        private class MockBOWithUpdateBeforePersisting : MockBO
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

        private class MockBOWithUpdateBeforePersisting_Level2 : MockBO
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
                transactionCommitter.AddBusinessObject(new MockBOWithUpdateBeforePersisting());
            }
        }

        private class MockBOWithUpdateBeforePersistDelegate : MockBO
        {
            public delegate void UpdateObjectBeforePersistingDelegate(ITransactionCommitter transactionCommitter);

            private readonly UpdateObjectBeforePersistingDelegate _updateObjectBeforePersistingDelegate;

            public MockBOWithUpdateBeforePersistDelegate(UpdateObjectBeforePersistingDelegate updateObjectBeforePersistingDelegate)
            {
                _updateObjectBeforePersistingDelegate = updateObjectBeforePersistingDelegate;
            }

            protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
            {
                if (_updateObjectBeforePersistingDelegate != null)
                {
                    _updateObjectBeforePersistingDelegate(transactionCommitter);
                }
                base.UpdateObjectBeforePersisting(transactionCommitter);
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

            MockBOWithUpdateBeforePersisting_Level2 mockBo = new MockBOWithUpdateBeforePersisting_Level2();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(mockBo);
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

            MockBOWithUpdateBeforePersisting mockBo = new MockBOWithUpdateBeforePersisting();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(mockBo);
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

            MockBOWithUpdateBeforePersisting_Level2 mockBo = new MockBOWithUpdateBeforePersisting_Level2();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(mockBo);
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
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(transactional1.Committed);
            Assert.IsTrue(transactional2.Committed);
        }

        [Test]
        public void TestMessageTwoPersistSimpleBO_Failing()
        {
            //---------------Set up test pack-------------------
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(mockBO);

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
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(mockBO);

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
        public void Test_AddCreatedBO_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            mockBo.MarkForDelete();
            TransactionCommitter committerDB = new TransactionCommitterStub();
            //---------------Assert Preconditions---------------
            Assert.IsTrue(mockBo.Status.IsNew);
            Assert.IsTrue(mockBo.Status.IsDeleted);
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
            //---------------Execute Test ----------------------
            committerDB.AddBusinessObject(mockBo);
            //---------------Test Result -----------------------
            Assert.IsTrue(mockBo.Status.IsNew);
            Assert.IsTrue(mockBo.Status.IsDeleted);
            Assert.AreEqual(0, committerDB.OriginalTransactions.Count);
        }
        [Test]
        public void TestPersistSimpleCreatedBO_DoesNotFailInTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            
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
            MockBO mockBo = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            TransactionCommitter committerDB = new TransactionCommitterStub();
            committerDB.AddBusinessObject(mockBo);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
        }
        [Test, ExpectedException(typeof (NotImplementedException))]
        public void TestRaisesException_onError()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committerDB = new TransactionCommitterStubDB();
            StubFailingTransaction trn = new StubFailingTransaction();
            committerDB.AddTransaction(trn);
            committerDB.AddTransaction(new StubSuccessfullTransaction());
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestRaisesException_onError_DoesNotCommit()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committer = new TransactionCommitterStubDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            committer.AddTransaction(transactional1);
            StubFailingTransaction transactional2 = new StubFailingTransaction();
            committer.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            try
            {
                committer.CommitTransaction();
                Assert.Fail("Failure should have occurred as a StubFailingTransaction was added");
            }
                //---------------Test Result -----------------------
            catch (NotImplementedException)
            {
                Assert.IsFalse(transactional1.Committed);
                Assert.IsFalse(transactional2.Committed);
            }
        }

        [Test]
        public void TestUpdateBeforePersisting_ExecutedBeforeValidation()
        {
            //---------------Set up test pack-------------------

            MockBOWithBeforeSaveUpdatesCompulsoryField mockBo = new MockBOWithBeforeSaveUpdatesCompulsoryField();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(mockBo);
            committer.AddTransaction(trnBusObj);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            //Business object will throw an exception if executed in the incorrect order.
            Assert.AreEqual(1, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestUpdateBeforePersistCalled()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB();
            bool updateBeforePersistCalled = false;
            MockBOWithUpdateBeforePersistDelegate mockBo = new MockBOWithUpdateBeforePersistDelegate(
                delegate {
                    updateBeforePersistCalled = true;
                });
            trnCommitter.AddBusinessObject(mockBo);
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            trnCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(updateBeforePersistCalled);
        }

        [Test]
        public void TestUpdateBeforePersistCalled_ForBoAddedInUpdateBeforePersist()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB();
            bool updateBeforePersistCalled = false;
            bool updateBeforePersistCalledForInner = false;
            MockBOWithUpdateBeforePersistDelegate innerMockBo = new MockBOWithUpdateBeforePersistDelegate(
                delegate {
                    updateBeforePersistCalledForInner = true;
                });
            MockBOWithUpdateBeforePersistDelegate mockBo = new MockBOWithUpdateBeforePersistDelegate(
                delegate(ITransactionCommitter committer)
                {
                    updateBeforePersistCalled = true;
                    committer.AddBusinessObject(innerMockBo);
                });
            trnCommitter.AddBusinessObject(mockBo);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, trnCommitter.OriginalTransactions.Count);
            Assert.IsFalse(updateBeforePersistCalled);
            Assert.IsFalse(updateBeforePersistCalledForInner);

            //---------------Execute Test ----------------------
            trnCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, trnCommitter.OriginalTransactions.Count);
            Assert.IsTrue(updateBeforePersistCalled);
            Assert.IsTrue(updateBeforePersistCalledForInner);
        }
    }
}