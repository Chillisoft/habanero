using System;
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

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
        public void TestPersistSimpleBO_Insert_ToDifferentDb()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            MockRepository mock = new MockRepository();
            IDatabaseConnection mockDatabaseConnection = GetMockDatabaseConnectionWithExpectations(mock);
            mock.ReplayAll();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(mockDatabaseConnection);
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo));
            //---------------Test Preconditions ----------------
            
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            mock.VerifyAll();
            AssertBOStateIsValidAfterInsert_Updated(mockBo);
            //BOLoader.Instance.Refresh(mockBo);
            //MockBO savedMockBO =
            //    BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBo.MockBOID.ToString("B") + "'");
            //Assert.AreSame(mockBo, savedMockBO);
        }

        public static IDatabaseConnection GetMockDatabaseConnectionWithExpectations(MockRepository mock)
        {
            IDatabaseConnection mockDatabaseConnection;
            mockDatabaseConnection = mock.CreateMock<IDatabaseConnection>();
            IDbConnection dbConnection = mock.CreateMock<IDbConnection>();
            IDbTransaction dbTransaction = mock.CreateMock<IDbTransaction>();
            IDbCommand dbCommand = mock.CreateMock<IDbCommand>();
            IDbDataParameter dbDataParameter = mock.DynamicMock<IDbDataParameter>();
            dbTransaction.Commit();
            dbConnection.Open();
            dbConnection.Close();
            Expect.Call(dbConnection.CreateCommand()).Return(dbCommand).Repeat.Any();
            Expect.Call(dbCommand.CreateParameter()).Return(dbDataParameter).Repeat.Any();
            Expect.Call(dbConnection.State).Return(ConnectionState.Open);
            Expect.Call(dbConnection.BeginTransaction(IsolationLevel.ReadCommitted)).IgnoreArguments().Return(dbTransaction);
            Expect.Call(mockDatabaseConnection.GetConnection()).Return(dbConnection).Repeat.AtLeastOnce();
            Expect.Call(mockDatabaseConnection.ExecuteSql(null,null)).IgnoreArguments().Return(1).Repeat.AtLeastOnce();
            Expect.Call(mockDatabaseConnection.LeftFieldDelimiter).Return("").Repeat.Any();
            Expect.Call(mockDatabaseConnection.RightFieldDelimiter).Return("").Repeat.Any();
            return mockDatabaseConnection;
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

        private static void CleanupObjectFromDatabase(BusinessObject bo)
        {
            bo.Delete();
            bo.Save();
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
            Assert.IsTrue(address.State.IsEditing);
            Assert.IsFalse(address.State.IsNew);
            Assert.IsTrue(contactPersonTestBO.State.IsDeleted);
            Assert.IsTrue(contactPersonTestBO.State.IsEditing);
            Assert.IsFalse(contactPersonTestBO.State.IsNew);
        }

        [Test]
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
            AssertBOStateIsValidAfterInsert_Updated(contactPersonCompositeKey);
            Assert.IsFalse(ContactPersonTestBO.AllLoadedBusinessObjects().ContainsKey(oldID));
            Assert.IsNotNull(ContactPersonTestBO.AllLoadedBusinessObjects()[contactPersonCompositeKey.ID.GetObjectId()]);
            //---------------Tear Down--------------------------
            contactPersonCompositeKey.Delete();
            contactPersonCompositeKey.Save();
        }

        [Test]
        public void TestAddBusinessObjectToTransactionInBeforeSave()
        {
            //---------------Set up test pack-------------------

            MockBOWithBeforeSave mockBo = new MockBOWithBeforeSave();
            TransactionCommitterStub committer = new TransactionCommitterStub();
            TransactionalBusinessObjectStub trnBusObj = new TransactionalBusinessObjectStub(mockBo);
            committer.AddTransaction(trnBusObj);
            //---------------Execute Test ----------------------

                committer.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, committer.OriginalTransactions.Count);
        }

        [Test]
        public void TestBeforeSave_ExecutedBeforeValidation()
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
            Assert.IsNull( engine.GetCar());
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
            catch (NotImplementedException )
            {
                Assert.IsTrue(mockBo.RollBackExecuted);                
            }
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

        private static void AssertTransactionsInTableAre(int expected)
        {
            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection, "select * from stubdatabasetransaction");
            Assert.AreEqual(expected, DatabaseConnection.CurrentConnection.LoadDataTable(statement, "", "").Rows.Count);
        }

        #endregion

        #region HelperMethods

        private static void CleanStubDatabaseTransactionTable()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from stubdatabasetransaction");
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

    internal class TransactionalBusinessObjectStub
        : TransactionalBusinessObject
    {
        public TransactionalBusinessObjectStub(BusinessObject businessObject) : base(businessObject)
        {
        }

        protected internal override bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            return false;
        }
    }

    #region MockBOs
    internal class MockBoWithRollBack : MockBO
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


    internal class MockBOWithBeforeSaveUpdatesCompulsoryField : MockBO
    {
        private bool _updateBeforePersistingExecuted = false;

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
        protected override bool CheckCustomRules(out string customRuleErrors)
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
        protected internal override void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        {
            _updateBeforePersistingExecuted = true;
        }
    }

    internal class MockBOWithBeforeSave : MockBO
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

    #endregion

}