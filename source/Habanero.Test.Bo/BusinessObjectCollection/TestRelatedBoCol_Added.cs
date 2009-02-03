using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.RelatedBusinessObjectCollection;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    [TestFixture]
    public class TestRelatedBoCol_AddedBOs //:TestBase
    {
        private static OrganisationTestBO _organisationTestBO;
        private readonly TestUtilsRelated util = new TestUtilsRelated();

        #region Setup

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed            
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            _organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
        }
        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is completeBORegistry.DataAccessor
            TestUtil.WaitForGC();
        }

        #endregion

        #region Utils

//        private static RelatedBusinessObjectCollection<ContactPersonTestBO> CreateCol_OneCP(out ContactPersonTestBO cp)
//        {
//            cp = CreateSavedContactPerson();
//            return CreateRelatedCPCol();
//        }

        private static RelatedBusinessObjectCollection<ContactPersonTestBO> CreateRelatedCPCol()
        {
            return BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
        }

        private static MultipleRelationship<ContactPersonTestBO> GetContactPersonRelationship()
        {
            if (_organisationTestBO == null)
            {
                _organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            }
            return _organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
        }

//        private static RelatedBusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
//        {
//            CreateSavedContactPerson();
//            return CreateRelatedCPCol();
//        }

        private static ContactPersonTestBO CreateSavedContactPerson()
        {
            MultipleRelationship<ContactPersonTestBO> relationship = GetContactPersonRelationship();
            ContactPersonTestBO cp = relationship.BusinessObjectCollection.CreateBusinessObject();
            cp.Surname = TestUtil.GetRandomString();
            cp.FirstName = TestUtil.GetRandomString();
            cp.Save();
            return cp;
        }

        #endregion

        #region Added Business object in current cpCollection
        [Test]
        public void Test_AddMethod()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            util.RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(util.AddedEventFired);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.IsTrue(util.AddedEventFired);
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
        }

        [Test]
        public void Test_Add_NewBO_AddsToCreatedCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO newCP = ContactPersonTestBO.CreateUnsavedContactPerson
                (BOTestUtils.RandomString, BOTestUtils.RandomString);
            util.RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsNull(newCP.OrganisationID);

            //---------------Execute Test ----------------------
            cpCol.Add(newCP);

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsNotNull(newCP.OrganisationID);
            Assert.Contains(newCP, cpCol);
            Assert.IsTrue(util.AddedEventFired);
        }
        [Test]
        public void Test_Add_NullBO()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            const ContactPersonTestBO newCP = null;
            util.RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Add(newCP);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("could not be added since the  business object is null", ex.Message);
            }

            //---------------Test Result -----------------------

        }

        [Test]
        public void Test_Add_PersistedBO_AddsToAddedCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO newCP = ContactPersonTestBO.CreateUnsavedContactPerson
                (BOTestUtils.RandomString, BOTestUtils.RandomString);
            newCP.Save();
            util.RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsNull(newCP.OrganisationID);

            //---------------Execute Test ----------------------
            cpCol.Add(newCP);

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.Contains(newCP, cpCol);
            Assert.IsTrue(util.AddedEventFired);
            Assert.IsNotNull(newCP.OrganisationID);
        }

        [Test]
        public void Test_AddMethod_WithEnumerable_Collection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            Collection<ContactPersonTestBO> cpCollection = new Collection<ContactPersonTestBO>();
            cpCollection.Add(myBO);
            cpCollection.Add(myBO2);
            cpCollection.Add(myBO3);
            //---------------Execute Test ----------------------
            cpCol.Add(cpCollection);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
        }


        [Test]
        public void Test_AddMethod_WithEnumerable_List()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            List<ContactPersonTestBO> list = new List<ContactPersonTestBO>();
            list.Add(myBO);
            list.Add(myBO2);
            list.Add(myBO3);
            //---------------Execute Test ----------------------
            cpCol.Add(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
            Assert.IsNotNull(myBO3.OrganisationID);
            Assert.IsNotNull(myBO2.OrganisationID);
            Assert.IsNotNull(myBO.OrganisationID);
        }

        [Test]
        public void Test_AddMethod_WithParamArray()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();

            //---------------Execute Test ----------------------
            cpCol.Add(myBO, myBO2, myBO3);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
        }

        [Test]
        public void Test_AddMethod_WithCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            cpCol.Add(myBO, myBO2, myBO3);

            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");

            ///---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpColCopied =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            cpColCopied.Add(cpCol);

            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpColCopied.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual(3, cpColCopied.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpColCopied.AddedBusinessObjects.Count);
            Assert.AreEqual(myBO, cpColCopied[0], "Added object should be in the copied cpCollection");
            Assert.AreEqual(myBO2, cpColCopied[1], "Added object should be in the copied cpCollection");
            Assert.AreEqual(myBO3, cpColCopied[2], "Added object should be in the copied cpCollection");
        }

        [Test]
        public void Test_Add_PersistedBOs()
        {
            //---------------Set up test pack-------------------
            //The persisted business objects should not be added since this is a normal cpCollection which does not 
            // modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection

            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(organisationTestBO.Relationships["ContactPeople"]);
            ContactPersonTestBO myBO = CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = CreateSavedContactPerson();
            util.RegisterForAddedEvent(cpCol);

            //-------Assert Preconditions
            Assert.AreEqual(0, cpCol.Count, "Three objects should be in the cpCollection");

            ///---------------Execute Test ----------------------
            cpCol.Add(myBO, myBO2, myBO3);

            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual
                (3, cpCol.AddedBusinessObjects.Count,
                 "The persisted business objects should not be in the AddedList since this is a normal cpCollection which does not modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection");
            Assert.IsTrue(util.AddedEventFired);
        }


        [Test]
        public void Test_Add_PersistedBO_RestoreAll()
        {
            //---------------Set up test pack-------------------
            //The persisted objects are added to the added cpCollection
            // when restore is called the added objects should be removed from the cpCollection.
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO, myBO2, myBO3);
            util.RegisterForRemovedEvent(cpCol);

            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual(3, cpCol.AddedBusinessObjects.Count, "Three objects should be in the cpCollection");
            Assert.IsFalse(util.RemovedEventFired);
            Assert.IsNotNull(myBO.OrganisationID);
            Assert.IsTrue(myBO.Status.IsDirty);

            ///---------------Execute Test ----------------------
            cpCol.CancelEdits();

            //---------------Test Result ----------------------- - Result
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsNull(myBO.OrganisationID);
            Assert.IsTrue(util.RemovedEventFired);
        }

        [Test]
        public void TestAddMethod_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            cpCol.Add(myBO);
            util.RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);
        }

        [Test]
        public void TestAddMethod_PersistedObject_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            util.RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);
            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);
        }

        [Test]
        public void TestAddMethod_SaveBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            util.RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);

            //---------------Execute Test ----------------------
            myBO.Save();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(util.AddedEventFired);
        }


        [Test]
        public void TestAddMethod_SaveAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            util.RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsFalse(util.AddedEventFired);
            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            Assert.IsFalse(util.AddedEventFired);
        }



        [Test]
        public void TestAddMethod_RestoreBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            Assert.IsNotNull(myBO.OrganisationID);

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            Assert.IsNull(myBO.OrganisationID);
        }

        [Test]
        public void TestAddMethod_RefreshAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();

            cpCol.Add(myBO);
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsNotNull(myBO.OrganisationID);
            Assert.IsNull(myBO.Props["OrganisationID"].PersistedPropertyValue);
            util.AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsNotNull(myBO.OrganisationID);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void TestAddMethod_Refresh_LoadWithCriteria_BusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            cpCol.Load("Surname='bbb'", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("aaa");

            cpCol.Add(myBO);
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void TestAddMethod_Remove_Added_NonPersisted_BusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson
                (TestUtil.GetRandomString(), TestUtil.GetRandomString());
            cpCol.Add(myBO);
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            Assert.IsNotNull(myBO.OrganisationID);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsNull(myBO.OrganisationID);
            util.AssertRemovedEventFired();
            util.AssertAddedEventNotFired();
        }

        [Test]
        public void TestAddMethod_RemoveAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            util.AssertRemovedEventFired();
            util.AssertAddedEventNotFired();
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_MarkForDeleteAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertRemovedEventFired();
        }

        #endregion

        #region Added Business Objects Marked for Deletion

        [Test]
        public void Test_Mark4Delete_Added_RefreshAll()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MultipleRelationship<ContactPersonTestBO> relationship = GetContactPersonRelationship();
            relationship.Initialise();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = (RelatedBusinessObjectCollection<ContactPersonTestBO>) relationship.BusinessObjectCollection;
            
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_LoadWCriteria_RefreshAll()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_RestoreBO()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            util.AssertAddedEventFired();
            Assert.IsTrue
                (myBO.Status.IsDirty, "Should be dirty since is readded to collection when cancel edits called");
            //TODO: test that mybo.orgid set correctly.
            util.AssertRemovedEventNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_RestoreBO_LoadWCriteria()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedEventFired();
            util.AssertRemovedEventNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_RemoveBO()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);
            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_Mark4DeleteBO()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_BO_Mark4Delete()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.MarkForDelete();

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_AddBo()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            util.AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_RestoreAll()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CancelEdits();

            //---------------Test Result -----------------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            util.AssertAddedEventFired();
            util.AssertRemovedEventFired();
        }

        [Test]
        public void Test_Mark4Delete_Added_SaveAll()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_boSave()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(GetContactPersonRelationship());

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Save();

            //---------------Test Result -----------------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        #endregion


    }
}