using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransactionCommitterDB : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            CleanStubDatabaseTransactionTable();
            ContactPersonTestBO.DeleteAllContactPeople();
            BORegistry.DataAccessor = new DataAccessorDB();
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
   

        [Test]
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
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
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
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
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
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
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

        [Test, ExpectedException(typeof(BusObjectInAnInvalidStateException))]
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
        public void TestPersistSimpleBO_Update_NotUsingTransactionalBusinessObject_DB()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            TransactionCommitter committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(mockBo);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BOLoader.Instance.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test, ExpectedException(typeof(BusinessObjectReferentialIntegrityException))]
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
        public void TestPreventDelete_ThreeLevels()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();
            org.Delete();

            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(org);
            //---------------Execute Test ----------------------

            try
            {
                committer.CommitTransaction();
                Assert.Fail();
            }
            //---------------Test Result -----------------------
            catch (BusinessObjectReferentialIntegrityException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains("There are 1 objects related through the 'ContactPeople.Addresses' relationship"));
            }
            finally
            {
                CleanupObjectFromDatabase(address);
                CleanupObjectFromDatabase(contactPersonTestBO);
                CleanupObjectFromDatabase(org);
            }
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
        public void TestDeleteDoNothing()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail("Delete should fail as there is referential integrity and the delete action is 'Do Nothing'");
            }
            catch (DatabaseWriteException ex)
            {
                StringAssert.Contains("There was an error writing to the database", ex.Message);
            }
            //---------------Test Result -----------------------

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
            Assert.IsTrue(address.State.IsEditing);
            Assert.IsFalse(address.State.IsNew);
            Assert.IsTrue(contactPersonTestBO.State.IsDeleted);
            Assert.IsTrue(contactPersonTestBO.State.IsEditing);
            Assert.IsFalse(contactPersonTestBO.State.IsNew);
        }

        [Test, Ignore("This test is being ignored due to the fact that we do not have a philosophy for compositional parents deleting their children etc")]
        public void TestDeleteRelatedWithFailure_CancelEditsOnParent()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.Delete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB();
            committerDB.AddBusinessObject(contactPersonTestBO);
            committerDB.AddTransaction(new StubDatabaseFailureTransaction());

            try
            {
                committerDB.CommitTransaction();
                Assert.Fail();
            }
            catch (NotImplementedException)
            {
            }

            //---------------Execute Test ----------------------
            contactPersonTestBO.Restore();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.State.IsEditing);
            Assert.IsFalse(contactPersonTestBO.State.IsDeleted);

            Assert.Ignore("This test is being ignored due to the fact that we do not have a philosophy for compositional parents deleting their children etc");
            Assert.IsFalse(address.State.IsEditing);
            Assert.IsFalse(address.State.IsDeleted);
        }



        // it can happen that a child is in teh transaction, deleted before the parent.  In this case
        // the search for child objects of the parent can possibly deadlock as the search is happening
        // outside the delete transaction.
        [Test]
        public void TestSearchForChildrenToDeleteWithChildDeletedInTransaction()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
              ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //---------------Execute Test ----------------------
            address.Delete();
            contactPersonTestBO.Delete();
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(address);
            committer.AddBusinessObject(contactPersonTestBO);
            committer.CommitTransaction();
            //---------------Test Result -----------------------

            AssertBOStateIsValidAfterDelete(address);
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            //---------------Tear Down -------------------------          
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
        public void Test3LayerDeleteRelated_WithDeletedObjectChildAndGrandchild()
        {
            //---------------Set up test pack-------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef_WithRelationShipToAddress();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();
            address.OrganisationID = org.OrganisationID;
            address.Save();
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

        [Test]
        public void TestCheckForDuplicateAlternateKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositeAlternateKey();
            DoTestCheckForDuplicateObjects();
        }

        private static void DoTestCheckForDuplicateObjects()
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
            IPrimaryKey objectID = contactPersonCompositeKey.ID;
            Assert.AreEqual(objectID.GetOrigObjectID(), objectID.GetObjectId());
            Assert.IsNotNull(ContactPersonTestBO.AllLoadedBusinessObjects()[objectID.GetOrigObjectID()]);
            Assert.IsFalse(ContactPersonTestBO.AllLoadedBusinessObjects().ContainsKey(oldID));
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
            Assert.IsNotNull(ContactPersonTestBO.AllLoadedBusinessObjects()[oldID]);
            TransactionCommitterDB committer = new TransactionCommitterDB();
            committer.AddBusinessObject(contactPersonCompositeKey);
            contactPersonCompositeKey.FirstName = "newName";
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(contactPersonCompositeKey);
            Assert.IsFalse(ContactPersonTestBO.AllLoadedBusinessObjects().ContainsKey(oldID));
            Assert.IsNotNull(ContactPersonTestBO.AllLoadedBusinessObjects()[contactPersonCompositeKey.ID.GetObjectId()]);
            //---------------Tear Down--------------------------
            contactPersonCompositeKey.Delete();
            contactPersonCompositeKey.Save();
        }

        [Test]
        public void TestDereferenceRelatedObjects()
        {
            //The Car has a single relationship to engine. The car->engine relationship is marked 
            // as a dereference related relationship.
            Car.DeleteAllCars();
            Engine.DeleteAllEngines();
            //---------------Set up test pack-------------------

            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.Save();

            Engine engine = new Engine();

            engine.SetPropertyValue("EngineNo", "NO111");
            string carIDProp = "CarID";
            engine.SetPropertyValue(carIDProp, car.GetPropertyValue(carIDProp));
            engine.Save();
            //Verify test pack - i.e. that engine saved correctly
            BOLoader.Instance.Refresh(engine);
            Assert.AreSame(engine.GetCar(), car);

            //---------------Execute Test ----------------------
            car.Delete();
            car.Save();

            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetPropertyValue(carIDProp));
            Assert.IsNull(engine.GetCar());
            //---------------Test TearDown -----------------------

            Car.DeleteAllCars();
            Engine.DeleteAllEngines();
        }


        //Rollback failure must reset concurrency version number.
        [Test]
        public void TestRollBack_OnError()
        {
            //---------------Set up test pack-------------------
            //Create object in DB
            MockBoWithRollBack mockBo = new MockBoWithRollBack();
            StubDatabaseFailureTransaction trnFail = new StubDatabaseFailureTransaction();
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB();
            trnCommitter.AddBusinessObject(mockBo);
            trnCommitter.AddTransaction(trnFail);
            Assert.IsFalse(mockBo.RollBackExecuted);
            //---------------Execute Test ----------------------
            try
            {
                trnCommitter.CommitTransaction();
                Assert.Fail();
            }
            //---------------Test Result -----------------------
            //Raise Exception that the object has been edited since 
            // the user last edited.
            catch (NotImplementedException)
            {
                Assert.IsTrue(mockBo.RollBackExecuted);
            }
        }

        #region custom asserts

        private static void AssertTransactionsInTableAre(int expected)
        {
            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection, "select * from stubdatabasetransaction");
            Assert.AreEqual(expected, DatabaseConnection.CurrentConnection.LoadDataTable(statement, "", "").Rows.Count);
        }


        private static void AssertBOStateIsValidAfterDelete(BusinessObject mockBo)
        {
            Assert.IsTrue(mockBo.State.IsDeleted);
            Assert.IsTrue(mockBo.State.IsNew);
        }

        private static void AssertBusinessObjectNotInDatabase(BusinessObject bo)
        {
            IBusinessObject missingBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            Assert.IsNull(missingBO);
        }
        private static void AssertBusinessObjectInDatabase(BusinessObject bo)
        {
            IBusinessObject loadedBO =
                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            Assert.IsNotNull(loadedBO);
        }

        private static void AssertMockBONotInDatabase(Guid mockBOID)
        {
            MockBO savedMockBO =
                BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBOID.ToString("B") + "'");
            Assert.IsNull(savedMockBO);
        }



        #endregion

        #region HelperMethods

        private static void CleanStubDatabaseTransactionTable()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from stubdatabasetransaction");
        }

        private static MockBO CreateSavedMockBO()
        {
            MockBO mockBo = new MockBO();
            mockBo.Save();
            return mockBo;
        }


        private static void CleanupObjectFromDatabase(IBusinessObject bo)
        {
            bo.Delete();
            bo.Save();
        }

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

        #endregion

        #region MockBOs
        private class MockBoWithRollBack : MockBO
        {
            private bool _rollBackExecuted = false;

            public bool RollBackExecuted
            {
                get { return _rollBackExecuted; }
            }

            protected internal override void UpdateAsTransactionRolledBack()
            {
                base.UpdateAsTransactionRolledBack();
                _rollBackExecuted = true;
            }
        }
        #endregion
    }
}
