using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransactionCommitter : TestUsingDatabase
    {
        private static readonly string _customRuleErrorMessage = "Broken Rule";

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            CleanStubDatabaseTransactionTable();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void TestAddTransactionsToATransactionCommiter()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();

            //---------------Execute Test ----------------------
            committerDB.AddTransaction(new StubSuccessfullTransaction());
            committerDB.AddTransaction(new StubSuccessfullTransaction());

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committerDB.OriginalTransactions.Count);
        }

        [Test]
        public void TestCommitAddedTransactions()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStub committerDB = new TransactionCommitterStub();
            StubSuccessfullTransaction transactional1 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional1);
            StubSuccessfullTransaction transactional2 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsTrue(transactional1.Committed);
            Assert.IsTrue(transactional2.Committed);
        }


        [Test, ExpectedException(typeof (NotImplementedException))]
        public void TestRaisesException_onError()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStubDB committerDB = new TransactionCommitterStubDB();
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
            TransactionCommitterStub committerDB = new TransactionCommitterStub();
            StubSuccessfullTransaction transactional1 = new StubSuccessfullTransaction();
            committerDB.AddTransaction(transactional1);
            StubFailingTransaction transactional2 = new StubFailingTransaction();
            committerDB.AddTransaction(transactional2);
            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
                //---------------Test Result -----------------------
            catch (NotImplementedException)
            {
                Assert.IsFalse(transactional1.Committed);
                Assert.IsFalse(transactional2.Committed);
            }
        }

        [Test]
        public void TestDatabaseTransaction_Success()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            committerDB.AddTransaction(transactional1);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.IsTrue(transactional1.Committed);
            AssertTransactionsInTableAre(1);
        }

        private static void AssertTransactionsInTableAre(int expected)
        {
            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection, "select * from stubdatabasetransaction");
            Assert.AreEqual(expected, DatabaseConnection.CurrentConnection.LoadDataTable(statement, "", "").Rows.Count);
        }

        private static void CleanStubDatabaseTransactionTable()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from stubdatabasetransaction");
        }

        public void TestDatabaseTransaction_SuccessTwoTransactions()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            StubDatabaseTransaction transactional2 = new StubDatabaseTransaction();
            committerDB.AddTransaction(transactional1);
            committerDB.AddTransaction(transactional2);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertTransactionsInTableAre(2);
        }

        [Test]
        public void TestDatabaseTransaction_Failure()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransaction transactional1 = new StubDatabaseTransaction();
            StubDatabaseFailureTransaction transactional2 = new StubDatabaseFailureTransaction();
            committerDB.AddTransaction(transactional1);
            committerDB.AddTransaction(transactional2);

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
            }
            catch (NotImplementedException)
            {
            }
            //---------------Test Result -----------------------
            Assert.IsFalse(transactional1.Committed);
            Assert.IsFalse(transactional2.Committed);
            AssertTransactionsInTableAre(0);
        }

        [Test]
        public void TestMultipleStatementsInOneITransactional()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            StubDatabaseTransactionMultiple transactional1 = new StubDatabaseTransactionMultiple();
            committerDB.AddTransaction(transactional1);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertTransactionsInTableAre(2);
        }

        [Test]
        public void TestPersistSimpleBO_Insert()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            MockBO savedMockBO =
                BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBo.MockBOID.ToString("B") + "'");
            Assert.AreSame(mockBo, savedMockBO);
        }

        [Test]
        public void TestPersistSimpleBO_Update()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPersistSimpleBO_UpdateNonDirty()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            Guid? mockBOProp1 = mockBo.MockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPersistSimpleBO_Delete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            mockBo.Delete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test]
        public void TestPersistSimpleBO_NewDelete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            mockBo.Delete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test, ExpectedException(typeof (BusObjectInAnInvalidStateException))]
        public void TestPersistSimpleBO_Insert_InvalidData()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            //AssertBOStateIsValidAfterInsert_Updated(contactPersonTestBO);
            //BOLoader.Instance.Refresh(contactPersonTestBO);
            //MockBO savedMockBO =
            //    BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + contactPersonTestBO.MockBOID.ToString("B") + "'");
            //Assert.AreSame(contactPersonTestBO, savedMockBO);
        }

        [Test]
        public void TestPersistSimpleBO_Delete_InvalidData()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

            //---------------Execute Test ----------------------
            contactPersonTestBO.Delete();
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
        }

        [Test]
        public void TestPersistSimpleBO_FailingCustomRules()
        {
            //---------------Set up test pack-------------------
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBO));

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
        public void TestMessageTwoPersistSimpleBO_Failing()
        {
            //---------------Set up test pack-------------------
            MockBOWithCustomRule mockBO = new MockBOWithCustomRule();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBO));

            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO));

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
        public void TestPersistSimpleBO_Update_NotUsingTransactionalBusinessObject()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(mockBo);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }


        [Test, ExpectedException(typeof (BusinessObjectReferentialIntegrityException))]
        public void TestPreventDelete()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestNotPreventDelete_ForNonDeleted()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.FirstName = Guid.NewGuid().ToString();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.State.IsDirty);
        }

        [Test]
        public void TestDeleteRelated()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);
        }

        [Test]
        public void TestDeleteRelatedWithFailureAfter()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            committerDB.AddTransaction(new StubDatabaseFailureTransaction());

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail();
            }
            catch (NotImplementedException)
            {
            }
            //---------------Test Result -----------------------
            AssertBusinessObjectInDatabase(contactPersonTestBO);
            AssertBusinessObjectInDatabase(address);

            Assert.IsTrue(address.State.IsDeleted);
            Assert.IsTrue(contactPersonTestBO.State.IsDeleted);
            Assert.IsFalse(address.State.IsNew);
            Assert.IsFalse(contactPersonTestBO.State.IsNew);
        }

        [Test]
        public void Test3LayerDeleteRelated()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();
            org.Delete();

            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(org);
            //---------------Execute Test ----------------------

            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(org);
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDatabase(org);
            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);
        }

        [Test]
        public void TestCheckForDuplicatePrimaryKeys()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            DoTestCheckForDuplicateObjects();
        }

        private void DoTestCheckForDuplicateObjects()
        {
            ContactPersonTestBO contactPersonCompositeKey = GetSavedContactPersonCompositeKey();
            ContactPersonTestBO duplicateContactPerson = new ContactPersonTestBO();
            duplicateContactPerson.ContactPersonID = Guid.NewGuid();
            duplicateContactPerson.Surname = contactPersonCompositeKey.Surname;
            duplicateContactPerson.FirstName = contactPersonCompositeKey.FirstName;
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(duplicateContactPerson);
            //---------------Execute Test ----------------------
            try
            {
                committer.CommitTransaction();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch (BusObjDuplicateConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Surname"));
                Assert.IsTrue(ex.Message.Contains("FirstName"));
            }
            finally
            {
                //---------------Tear Down--------------------------
                contactPersonCompositeKey.Delete();
                contactPersonCompositeKey.Save();
                if (!duplicateContactPerson.State.IsNew)
                {
                    duplicateContactPerson.Delete();
                    duplicateContactPerson.Save();
                }
            }
        }

        [Test]
        public void TestCheckForDuplicateAlternateKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositeAlternateKey();
            DoTestCheckForDuplicateObjects();
        }

        [Test]
        public void TestCompositePrimaryUpdatesOrigObjectIDAfterSaving()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO contactPersonCompositeKey = GetUnsavedContactPersonCompositeKey();
            string oldID = contactPersonCompositeKey.ID.GetObjectId();
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(contactPersonCompositeKey);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            BOPrimaryKey objectID = contactPersonCompositeKey.ID;
            Assert.AreEqual(objectID.GetOrigObjectID(), objectID.GetObjectId());
            Assert.IsNotNull(ContactPersonTestBO.AllLoaded()[objectID.GetOrigObjectID()]);
            Assert.IsFalse(ContactPersonTestBO.AllLoaded().ContainsKey(oldID));
        }

        [Test]
        public void TestStorePersistedValue_BOProp()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.Surname = Guid.NewGuid().ToString();
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(cp);

            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(cp.Surname, cp.Props["Surname"].PersistedPropertyValueString);
        }
        
        [Test]
        public void TestChangeCompositePrimaryKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO contactPersonCompositeKey = GetSavedContactPersonCompositeKey();
            string oldID = contactPersonCompositeKey.ID.GetObjectId();
            Assert.IsNotNull(ContactPersonTestBO.AllLoaded()[oldID]);
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(contactPersonCompositeKey);
            contactPersonCompositeKey.FirstName = "newName";
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterInsert_Updated(contactPersonCompositeKey);
            Assert.IsFalse(ContactPersonTestBO.AllLoaded().ContainsKey(oldID));
            Assert.IsNotNull(ContactPersonTestBO.AllLoaded()[contactPersonCompositeKey.ID.GetObjectId()]);
            //---------------Tear Down--------------------------
            contactPersonCompositeKey.Delete();
            contactPersonCompositeKey.Save();
        }

        [Test]
        public void TestAddBusinessObjectToTransactionInBeforeSave()
        {
            //---------------Set up test pack-------------------

            MockBOWithBeforeSave mockBo = new MockBOWithBeforeSave();
            //Add this to the commiter
            //Execute 
            //Check that MockBOWithBeforeSave and its beforesave object are commited to database
            TransactionCommitterStub committer = new TransactionCommitterStub();
            committer.AddTransaction(new TransactionalBusinessObject(mockBo));
            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committer.OriginalTransactions.Count);
        }

        #region CustomAsserts

        private static void AssertBusinessObjectNotInDatabase(BusinessObject bo)
        {
            BusinessObject missingBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            Assert.IsNull(missingBO);
        }

        private static void AssertBusinessObjectInDatabase(BusinessObject bo)
        {
            BusinessObject loadedBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            Assert.IsNotNull(loadedBO);
        }

        #endregion

        #region HelperMethods

        private static ContactPersonTestBO GetSavedContactPersonCompositeKey()
        {
            ContactPersonTestBO contactPersonCompositeKey = GetUnsavedContactPersonCompositeKey();
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(contactPersonCompositeKey);
            committer.CommitTransaction();
            return contactPersonCompositeKey;
        }

        private static ContactPersonTestBO GetUnsavedContactPersonCompositeKey()
        {
            ContactPersonTestBO contactPersonCompositeKey = new ContactPersonTestBO();
            contactPersonCompositeKey.ContactPersonID = Guid.NewGuid();
            contactPersonCompositeKey.Surname = "Somebody";
            contactPersonCompositeKey.FirstName = "Else";
            return contactPersonCompositeKey;
        }

        private static MockBO CreateSavedMockBO()
        {
            MockBO mockBo = new MockBO();
            mockBo.Save();
            return mockBo;
        }

        private static void AssertMockBONotInDatabase(Guid mockBOID)
        {
            MockBO savedMockBO =
                BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBOID.ToString("B") + "'");
            Assert.IsNull(savedMockBO);
        }

        private static void AssertBOStateIsValidAfterDelete(BusinessObject mockBo)
        {
            Assert.IsTrue(mockBo.State.IsDeleted);
            Assert.IsTrue(mockBo.State.IsNew);
        }

        private static void AssertBOStateIsValidAfterInsert_Updated(BusinessObject mockBo)
        {
            Assert.IsFalse(mockBo.State.IsNew);
            Assert.IsFalse(mockBo.State.IsDirty);
            Assert.IsFalse(mockBo.State.IsDeleted);
            Assert.IsFalse(mockBo.State.IsEditing);
            Assert.IsTrue(mockBo.State.IsValid);
        }

        internal class MockBOWithCustomRule : MockBO
        {
            /// <summary>
            /// Override this method in subclasses of BusinessObject to check custom rules for that
            /// class.  The default implementation returns true and sets customRuleErrors to the empty string.
            /// </summary>
            /// <param name="customRuleErrors">The error string to display</param>
            /// <returns>true if no custom rule errors are encountered.</returns>
            protected override bool CheckCustomRules(out string customRuleErrors)
            {
                customRuleErrors = _customRuleErrorMessage;
                return false;
            }
        }

        #endregion
    }

    internal class MockBOWithBeforeSave:MockBO
    {
        /// <summary>
        /// Steps to carry out before the Save() command is run. You can add objects to the current
        /// transaction using this method, such as a database number generator.  No validity checks are 
        /// made to the BusinessObject after this step, so be careful not to invalidate the object.
        /// </summary>
        /// <param name="transactionCommitter">The current transaction committer - any objects added to this will
        /// be committed in the same transaction as this one.</param>
        protected internal override void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        {
            transactionCommitter.AddTransaction(new StubSuccessfullTransaction());
        }
    }
}