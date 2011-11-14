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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.ClassDefinition;
using Habanero.Test.BO.TransactionCommitters;
using Habanero.Test.Structure;
using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestTransactionCommitterDB : TestUsingDatabase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            CleanStubDatabaseTransactionTable();
            ContactPersonTestBO.DeleteAllContactPeople();
            BORegistry.DataAccessor = new DataAccessorDB();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
            if (ClassDef.ClassDefs.Count > 0 && (ClassDef.ClassDefs.Contains("Habanero.Test.BO", "AddressTestBO")))
            {
                var classDef = ClassDef.Get<AddressTestBO>();
                string defaultCpAddressTableName = "contact_person_address";
                if (classDef.TableName.ToLower() != defaultCpAddressTableName)
                {
                    AddressTestBO.DropCpAddressTable(classDef.TableName);
                }
            }

            if (ClassDef.ClassDefs.Count > 0 && (ClassDef.ClassDefs.Contains("Habanero.Test.BO", "ContactPersonTestBO")))
            {
                var classDef = ClassDef.Get<ContactPersonTestBO>();
                string defaultContactPersonTableName = "contact_person";
                if (classDef.TableName.ToLower() != defaultContactPersonTableName)
                {
                    ContactPersonTestBO.DropContactPersonTable(classDef.TableName);
                }
            }
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        #endregion


        [Test]
        public void TestRaisesException_onError()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committerDB = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
            StubFailingTransaction trn = new StubFailingTransaction();
            committerDB.AddTransaction(trn);
            committerDB.AddTransaction(new StubSuccessfullTransaction());
            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail("Expected to throw an NotImplementedException");
            }
                //---------------Test Result -----------------------
            catch (NotImplementedException)
            {
            }
        }

        [Test]
        public void TestUpdateBeforePersistCalled()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
            bool updateBeforePersistCalled = false;
            MockBOWithUpdateBeforePersistDelegate mockBo = new MockBOWithUpdateBeforePersistDelegate(
                delegate
                {
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
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
            bool updateBeforePersistCalled = false;
            bool updateBeforePersistCalledForInner = false;
            MockBOWithUpdateBeforePersistDelegate innerMockBo = new MockBOWithUpdateBeforePersistDelegate(
                delegate
                {
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
            Assert.AreEqual(1, trnCommitter.GetOriginalTransactions().Count);
            Assert.IsFalse(updateBeforePersistCalled);
            Assert.IsFalse(updateBeforePersistCalledForInner);

            //---------------Execute Test ----------------------
            trnCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, trnCommitter.GetOriginalTransactions().Count);
            Assert.IsTrue(updateBeforePersistCalled);
            Assert.IsTrue(updateBeforePersistCalledForInner);
        }

        [Test]
        public void TestRaisesException_onError_DoesNotCommit()
        {
            //---------------Set up test pack-------------------
            TransactionCommitter committer = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
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


        private static void DoTestCheckForDuplicateObjects()
        {
            ContactPersonTestBO contactPersonCompositeKey = GetSavedContactPersonCompositeKey();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO duplicateContactPerson = new ContactPersonTestBO
                                                             {
                                                                 ContactPersonID = Guid.NewGuid(),
                                                                 Surname = contactPersonCompositeKey.Surname,
                                                                 FirstName = contactPersonCompositeKey.FirstName
                                                             };
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
                StringAssert.Contains("Surname", ex.Message);
                StringAssert.Contains("FirstName", ex.Message);
            }
            finally
            {
                //---------------Tear Down--------------------------
                contactPersonCompositeKey.MarkForDelete();
                contactPersonCompositeKey.Save();
                if (!duplicateContactPerson.Status.IsNew)
                {
                    duplicateContactPerson.MarkForDelete();
                    duplicateContactPerson.Save();
                }
            }
        }

        private static void AssertTransactionsInTableAre(int expected)
        {
            SqlStatement statement =
                new SqlStatement(DatabaseConnection.CurrentConnection, "select * from stubdatabasetransaction");
            Assert.AreEqual(expected, DatabaseConnection.CurrentConnection.LoadDataTable(statement, "", "").Rows.Count);
        }


        private static void AssertBusinessObjectNotInDatabase(IBusinessObject bo)
        {
//            Criteria criteria = new Criteria(bo.ID[0], );
            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(bo.ClassDef, bo.ID);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains("A Error has occured since the object you are trying to refresh has been ", ex.Message);
            }
        }

        private static void AssertBusinessObjectInDatabase(IBusinessObject bo)
        {
//            IBusinessObject loadedBO =
//                BOLoader.Instance.GetBusinessObject(bo.GetType(), bo.ID.ToString());
            IBusinessObject loadedBO =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(bo.ClassDef, bo.ID);
            Assert.IsNotNull(loadedBO);
        }

        private static void AssertMockBONotInDatabase(Guid mockBOID)
        {
            //    MockBO savedMockBO =
            //        BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBOID.ToString("B") + "'");
            Criteria criteria = new Criteria("MockBOID", Criteria.ComparisonOp.Equals, mockBOID);
            IBusinessObject savedMockBO =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MockBO>((criteria));
            Assert.IsNull(savedMockBO);
        }

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
            bo.MarkForDelete();
            bo.Save();
        }

        private static ContactPersonTestBO GetSavedContactPersonCompositeKey()
        {
            ContactPersonTestBO contactPersonCompositeKey = GetUnsavedContactPersonCompositeKey();
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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

        private class MockBoWithRollBack : MockBO
        {
            private bool _rollBackExecuted;

            public bool RollBackExecuted
            {
                get { return _rollBackExecuted; }
            }

            protected override void UpdateAsTransactionRolledBack()
            {
                base.UpdateAsTransactionRolledBack();
                _rollBackExecuted = true;
            }
        }
        // ReSharper restore RedundantAssignment


        protected virtual void CreateContactPersonTable()
        {
            var contactPersonTableName = "contact_person_" + TestUtil.GetRandomString();
            ContactPersonTestBO.CreateContactPersonTable(contactPersonTableName);
        }

        protected virtual void CreateAddressTable(string tableNameExtension)
        {
            var contactPersonAddressTableName = "contact_person_address_" + tableNameExtension;
            AddressTestBO.CreateContactPersonAddressTable(contactPersonAddressTableName, "contact_person_" + tableNameExtension);
        }

        private void CreateContactPersonTable(string tableNameExtension)
        {
            var contactPersonTableName = "contact_person_" + tableNameExtension;
            ContactPersonTestBO.CreateContactPersonTable(contactPersonTableName);
        }

        [Test]
        public void Test3LayerDeleteRelated()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorDB();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO address;
            var tableNameExtension = TestUtil.GetRandomString();
            CreateContactPersonTable(tableNameExtension);
            CreateAddressTable(tableNameExtension);
            var contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, tableNameExtension);

            var org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1,org.ContactPeople.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, org.Relationships.Count);
            Assert.IsTrue(org.Relationships.Contains("ContactPeople"));
            Assert.AreEqual(DeleteParentAction.DeleteRelated, org.Relationships["ContactPeople"].DeleteParentAction);
            //---------------Execute Test ----------------------
            //IBusinessObjectCollection colContactPeople = org.Relationships["ContactPeople"].GetRelatedBusinessObjectCol();
            //ContactPersonTestBO loadedCP = (ContactPersonTestBO)colContactPeople[0];
            //IBusinessObjectCollection colAddresses = loadedCP.Relationships["Addresses"].GetRelatedBusinessObjectCol();
            //Address loadedAdddress = (Address)colAddresses[0];

            org.MarkForDelete();

            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(org);
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            //Assert.AreSame(contactPersonTestBO, loadedCP);
            //Assert.AreSame(address, loadedAdddress);

            AssertBusinessObjectNotInDatabase(org);
            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);

            BOTestUtils.AssertBOStateIsValidAfterDelete(org);
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            BOTestUtils.AssertBOStateIsValidAfterDelete(address);
        }

        [Test]
        public void Test3LayerDeleteRelated_WithDeletedObjectChildAndGrandchild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            //AddressTestBO.LoadDefaultClassDef();

            OrganisationTestBO.LoadDefaultClassDef_WithMultipleRelationshipToAddress();
            AddressTestBO address;
/*            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());*/


            var tableNameExtension = TestUtil.GetRandomString();
            CreateContactPersonTable(tableNameExtension);
            CreateAddressTable(tableNameExtension);
            var contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, tableNameExtension);


            var org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();
            address.OrganisationID = org.OrganisationID;
            address.Save();
            org.MarkForDelete();

            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(org);

            //---------------Assert Precondition----------------
            //Assert.AreEqual(1, org.ContactPeople.Count);
            Assert.AreEqual(0, org.ContactPeople.Count);
            Assert.AreEqual(1, org.ContactPeople.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, ((IMultipleRelationship) org.Relationships["Addresses"]).BusinessObjectCollection.Count);
            Assert.AreEqual(1, ((IMultipleRelationship) org.Relationships["Addresses"]).BusinessObjectCollection.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(2, org.Relationships.Count);
            Assert.IsTrue(org.Relationships.Contains("ContactPeople"));
            Assert.AreEqual(DeleteParentAction.DeleteRelated, org.Relationships["ContactPeople"].DeleteParentAction);
            Assert.AreEqual(DeleteParentAction.DeleteRelated, org.Relationships["Addresses"].DeleteParentAction);

            //---------------Execute Test ----------------------

            committer.CommitTransaction();
            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(org);
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            BOTestUtils.AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDatabase(org);
            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);
        }

        [Test]
        public void TestChangeCompositePrimaryKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO contactPersonCompositeKey = GetSavedContactPersonCompositeKey();
            Guid oldID = contactPersonCompositeKey.ID.ObjectID;
            Assert.IsNotNull(BORegistry.BusinessObjectManager[oldID]);
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(contactPersonCompositeKey);
            contactPersonCompositeKey.FirstName = "newName";
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(contactPersonCompositeKey);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(oldID));
            Assert.IsNotNull(BORegistry.BusinessObjectManager[contactPersonCompositeKey.ID.ObjectID]);
            //---------------Tear Down--------------------------
            contactPersonCompositeKey.MarkForDelete();
            contactPersonCompositeKey.Save();
        }


        [Test]
        public void Test_DoNotChangeChangeCompositePrimaryKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO contactPersonCompositeKey = GetSavedContactPersonCompositeKey();
            //            string oldID = contactPersonCompositeKey.ID.AsString_CurrentValue();
            //            Assert.IsNotNull(BusinessObjectManager.Instance[oldID]);
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(contactPersonCompositeKey);
            string origFirstname = contactPersonCompositeKey.FirstName;
            contactPersonCompositeKey.FirstName = "Temp Firstname";
            contactPersonCompositeKey.FirstName = origFirstname;
            IBOProp prop = contactPersonCompositeKey.Props["FirstName"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(prop.IsDirty);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(contactPersonCompositeKey);
            //            Assert.IsFalse(BusinessObjectManager.Instance.Contains(oldID));
            //            Assert.IsNotNull(BusinessObjectManager.Instance[contactPersonCompositeKey.ID.AsString_CurrentValue()]);
            //            //---------------Tear Down--------------------------
            //            contactPersonCompositeKey.MarkForDelete();
            //            contactPersonCompositeKey.Save();
        }
//                                <prop name=""Surname"" />
//                        <prop name=""FirstName"" />
        [Test]
        public void TestCheckForDuplicateAlternateKey()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositeAlternateKey();
            DoTestCheckForDuplicateObjects();
        }

        [Test]
        public void TestCheckForDuplicatePrimaryKeys()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            DoTestCheckForDuplicateObjects();
        }

        [Test]
        public void TestCompositePrimaryUpdatesOrigObjectIDAfterSaving()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKeyNameSurname();
            ContactPersonTestBO contactPersonCompositeKey = new ContactPersonTestBO();
            string oldID = contactPersonCompositeKey.ID.AsString_CurrentValue();
            contactPersonCompositeKey.ContactPersonID = Guid.NewGuid();
            contactPersonCompositeKey.Surname = "Somebody";
            contactPersonCompositeKey.FirstName = "Else";
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(contactPersonCompositeKey);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            IPrimaryKey objectID = contactPersonCompositeKey.ID;
            Assert.AreNotEqual(oldID, objectID.AsString_CurrentValue());
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(contactPersonCompositeKey.ID.ObjectID));
        }

        [Test]
        public void TestDatabaseTransaction_Failure()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
        public void TestDatabaseTransaction_Success()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
        public void TestDeleteDoNothing()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);
            contactPersonTestBO.MarkForDelete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
        public void TestDeleteRelated()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
/*            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());*/
            
            var tableNameExtension = TestUtil.GetRandomString();
            CreateContactPersonTable(tableNameExtension);
            CreateAddressTable(tableNameExtension);
            var contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, tableNameExtension);

            contactPersonTestBO.MarkForDelete();
            var committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            BOTestUtils.AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDatabase(contactPersonTestBO);
            AssertBusinessObjectNotInDatabase(address);
        }

        [Test]
        public void TestDeleteRelated_WhenCircularDelete_ShouldResolve()
        {
            //---------------Set up test pack-------------------
            Entity.LoadDefaultClassDef_WithCircularDeleteRelatedToSelf();
            Entity entity1 = new Entity();
            Entity entity2 = new Entity();
            entity1.Relationships.SetRelatedObject("RelatedEntity", entity2);
            entity2.Relationships.SetRelatedObject("RelatedEntity", entity1);
            entity1.Save();
            entity2.Save();
            entity1.MarkForDelete();
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(entity1);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(entity1);
            BOTestUtils.AssertBOStateIsValidAfterDelete(entity2);
            AssertBusinessObjectNotInDatabase(entity1);
            AssertBusinessObjectNotInDatabase(entity2);
        }

        [Test,
         Ignore(
             "This test is being ignored due to the fact that we do not have a philosophy for compositional parents deleting their children etc"
             )]
        public void TestDeleteRelatedWithFailure_CancelEditsOnParent()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());
            contactPersonTestBO.MarkForDelete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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
            contactPersonTestBO.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Status.IsEditing);
            Assert.IsFalse(contactPersonTestBO.Status.IsDeleted);

            Assert.Ignore(
                "This test is being ignored due to the fact that we do not have a philosophy for compositional parents deleting their children etc");
            Assert.IsFalse(address.Status.IsEditing);
            Assert.IsFalse(address.Status.IsDeleted);
        }

        [Test]
        public void TestDeleteRelatedWithFailureAfter()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
/*            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());*/
            
            var tableNameExtension = TestUtil.GetRandomString();
            CreateContactPersonTable(tableNameExtension);
            CreateAddressTable(tableNameExtension);
            var contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, tableNameExtension);

            contactPersonTestBO.MarkForDelete();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
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

            Assert.IsTrue(address.Status.IsDeleted);
            Assert.IsTrue(address.Status.IsEditing);
            Assert.IsFalse(address.Status.IsNew);
            Assert.IsTrue(contactPersonTestBO.Status.IsDeleted);
            Assert.IsTrue(contactPersonTestBO.Status.IsEditing);
            Assert.IsFalse(contactPersonTestBO.Status.IsNew);
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
            const string carIDProp = "CarID";
            engine.SetPropertyValue(carIDProp, car.GetPropertyValue(carIDProp));
            engine.Save();
            //Verify test pack - i.e. that engine saved correctly
//            BOLoader.Instance.Refresh(engine);
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(engine);
            Assert.AreSame(engine.GetCar(), car);

            //---------------Execute Test ----------------------
            car.MarkForDelete();
            car.Save();

            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetPropertyValue(carIDProp));
            Assert.IsNull(engine.GetCar());
            //---------------Test TearDown -----------------------

            Car.DeleteAllCars();
            Engine.DeleteAllEngines();
        }

        [Test]
        public void TestMultipleStatementsInOneITransactional()
        {
            //---------------Set up test pack-------------------
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            StubDatabaseTransactionMultiple transactional1 = new StubDatabaseTransactionMultiple();
            committerDB.AddTransaction(transactional1);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            AssertTransactionsInTableAre(2);
        }

        [Test]
        public void TestNotPreventDelete_ForNonDeleted()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            contactPersonTestBO.FirstName = Guid.NewGuid().ToString();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Status.IsDirty);
        }

        [Test]
        public void TestPersistSimpleBO_Delete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            mockBo.MarkForDelete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test]
        public void TestPersistSimpleBO_Delete_InvalidData()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            contactPersonTestBO.Surname = null;
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            contactPersonTestBO.MarkForDelete();
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPersonTestBO);
        }

        [Test]
        public void TestPersistSimpleBO_Insert()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
//            BOLoader.Instance.Refresh(mockBo);
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(mockBo);
            //MockBO savedMockBO =
            //    BOLoader.Instance.GetBusinessObject<MockBO>("MockBOID = '" + mockBo.MockBOID.ToString("B") + "'");
            Criteria criteria = new Criteria("MockBOID", Criteria.ComparisonOp.Equals, mockBo.MockBOID);
            MockBO savedMockBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MockBO>(criteria);
            Assert.AreSame(mockBo, savedMockBO);
        }

        [Test]
        public void TestPersistSimpleBO_Insert_InvalidData()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(contactPersonTestBO, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail("Expected to throw an BusObjectInAnInvalidStateException");
            }
                //---------------Test Result -----------------------
            catch (BusObjectInAnInvalidStateException ex)
            {
                StringAssert.Contains("'ContactPersonTestBO.Surname' is a compulsory field and has no value", ex.Message);
            }
        }

        [Test]
        public void TestPersistSimpleBO_NewDelete()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = new MockBO();
            TransactionCommitterInMemory inMemoryCommitter = new TransactionCommitterInMemory(new DataStoreInMemory());
            inMemoryCommitter.AddBusinessObject(mockBo);
            inMemoryCommitter.CommitTransaction();

            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            mockBo.MarkForDelete();
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(mockBo);
            AssertMockBONotInDatabase(mockBo.MockBOID);
        }

        [Test]
        public void TestPersistSimpleBO_Update()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPersistSimpleBO_Update_NotUsingTransactionalBusinessObject_DB()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            Guid mockBOProp1 = Guid.NewGuid();
            mockBo.MockBOProp1 = mockBOProp1;
            TransactionCommitter committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddBusinessObject(mockBo);

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPersistSimpleBO_UpdateNonDirty()
        {
            //---------------Set up test pack-------------------
            MockBO mockBo = CreateSavedMockBO();
            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            Guid? mockBOProp1 = mockBo.MockBOProp1;
            committerDB.AddTransaction(new TransactionalBusinessObjectDB(mockBo, committerDB.DatabaseConnection));

            //---------------Execute Test ----------------------
            committerDB.CommitTransaction();

            //---------------Test Result -----------------------
            TransactionCommitterTestHelper.AssertBOStateIsValidAfterInsert_Updated(mockBo);
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(mockBo);
            Assert.AreEqual(mockBOProp1, mockBo.MockBOProp1);
        }

        [Test]
        public void TestPreventDelete()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
//            contactPersonTestBO.MarkForDelete();
            ReflectionUtilities.SetPropertyValue(contactPersonTestBO.Status, "IsDeleted", true);

            TransactionCommitterDB committerDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committerDB.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            try
            {
                committerDB.CommitTransaction();
                Assert.Fail("Expected to throw an BusObjPersistException");
            }
                //---------------Test Result -----------------------
            catch (BusObjPersistException ex)
            {
                StringAssert.Contains("You cannot delete ContactPersonTestBO identified by ", ex.Message);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestPreventDelete_ThreeLevels()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
            OrganisationTestBO.LoadDefaultClassDef();

            OrganisationTestBO org = new OrganisationTestBO();
            contactPersonTestBO.SetPropertyValue("OrganisationID", org.OrganisationID);
            org.Save();
            contactPersonTestBO.Save();
            //org.MarkForDelete();

            //TransactionCommitterDB committer = new TransactionCommitterDB();
            //committer.AddBusinessObject(org);
            //---------------Execute Test ----------------------

            try
            {
                //committer.CommitTransaction();
                org.MarkForDelete();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch (BusObjDeleteException ex)
            {
                StringAssert.Contains("as the IsDeletable is set to false for the object", ex.Message);
                StringAssert.Contains("since it is related to 1 Business Objects via the Addresses relationship", ex.Message);
            }
            finally
            {
                CleanupObjectFromDatabase(address);
                CleanupObjectFromDatabase(contactPersonTestBO);
                CleanupObjectFromDatabase(org);
            }
        }

        [Test]
        public void TestRollBack_OnError()
        {
            //---------------Set up test pack-------------------
            //Create object in DB
            MockBoWithRollBack mockBo = new MockBoWithRollBack();
            StubDatabaseFailureTransaction trnFail = new StubDatabaseFailureTransaction();
            TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
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

        [Test]
        public void TestSearchForChildrenToDeleteWithChildDeletedInTransaction()
        {
            //---------------Set up test pack-------------------
            AddressTestBO address;
/*            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());*/

            var tableNameExtension = TestUtil.GetRandomString();
            CreateContactPersonTable(tableNameExtension);
            CreateAddressTable(tableNameExtension);
            var contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, tableNameExtension);
            //---------------Execute Test ----------------------
            address.MarkForDelete();
            contactPersonTestBO.MarkForDelete();
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(address);
            committer.AddBusinessObject(contactPersonTestBO);
            committer.CommitTransaction();
            //---------------Test Result -----------------------

            BOTestUtils.AssertBOStateIsValidAfterDelete(address);
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            //---------------Tear Down -------------------------          
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
            TransactionCommitterDB committer = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            committer.AddBusinessObject(cp);

            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(cp.Surname, cp.Props["Surname"].PersistedPropertyValueString);
        }


        /// <summary>
        /// the <see cref="TransactionCommitterDB"/>shouldn't check for duplicates on the primary key as the object is new
        /// and has an autoincrementing id field.
        /// </summary>
        [Test]
        public void TestAutoIncrementingFieldInNewPrimaryKeyDoesntCheckDuplicates()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();

            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testfield", "testing 123");

            MockRepository mockRepos = new MockRepository();
            IBusinessObjectLoader mockBusinessObjectLoader = mockRepos.DynamicMock<IBusinessObjectLoader>();

            Expect.Call(
                mockBusinessObjectLoader.GetBusinessObjectCollection(null, new Criteria("", Criteria.ComparisonOp.Equals, "")))
                .IgnoreArguments()
                .Repeat.Never();

            DataAccessorStub dataAccessor = new DataAccessorStub();
            dataAccessor.BusinessObjectLoader = mockBusinessObjectLoader;

            BORegistry.DataAccessor = dataAccessor;
            mockRepos.ReplayAll();
            TransactionCommitterDB transactionCommitterDB = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            transactionCommitterDB.AddBusinessObject(bo);

            //---------------Execute Test ----------------------
            transactionCommitterDB.CommitTransaction();

            //---------------Test Result -----------------------
            mockRepos.VerifyAll();

        }
        private class DataAccessorStub : IDataAccessor
        {
            private IBusinessObjectLoader _businessObjectLoader;
            public IBusinessObjectLoader BusinessObjectLoader { get { return _businessObjectLoader; } set { _businessObjectLoader = value; } }

            public ITransactionCommitter CreateTransactionCommitter() { return null; }
        }

        private class MockBOWithUpdateBeforePersistDelegate : MockBO
        {
            public delegate void UpdateObjectBeforePersistingDelegate(ITransactionCommitter transactionCommitter);

            private readonly UpdateObjectBeforePersistingDelegate _updateObjectBeforePersistingDelegate;

            public MockBOWithUpdateBeforePersistDelegate(UpdateObjectBeforePersistingDelegate updateObjectBeforePersistingDelegate)
            {
                _updateObjectBeforePersistingDelegate = updateObjectBeforePersistingDelegate;
            }

            protected override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
            {
                if (_updateObjectBeforePersistingDelegate != null)
                {
                    _updateObjectBeforePersistingDelegate(transactionCommitter);
                }
                base.UpdateObjectBeforePersisting(transactionCommitter);
            }
        }
    }
}