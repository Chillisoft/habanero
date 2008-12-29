using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.RelatedBusinessObjectCollection
{
    //Composition (strong part of) Relationships in Habanero: 
    //  A strong form of aggregation in which the "whole" is completely responsible for its parts and each "part" object 
    //      is only associated to one "whole" object. E.g  Invoice and Invoice lines. 
    //      The invoices lines only make sense in terms of the invoice. The invoice lines cannot be created, retrieved, 
    //      deleted or persisted independently of the invoice. The invoice lines can never be moved to another invoice.

        //The composition relationship can be either a 1:M or a 1:1. By definition a M:1 and a M:M cannot be a composition relationship. 
        //•	A typical example of a composition relationship is an Invoice and its Invoice lines. An invoice is made up of its invoice lines. 
            //An Invoice Line is part of an Invoice. An invoice Line cannot exist independently of its invoice and an invoice 
            //line can only belong to a single invoice.
        //•	An invoice that has invoice lines cannot be deleted without it deleting its invoice lines. 
            //The invoice’s InvoiceLines relationship would be marked as either prevent delete, delete invoice lines or do nothing.
        //•	An already persisted invoice line cannot be added to an Invoice (In Habanero a new invoice line can be added to an invoice). 
        //•	An Invoice line cannot be removed from its invoice.
        //•	An invoice can create a new invoice line via its InvoiceLines Relationship.
        //•	An invoice is considered to be dirty if it has any dirty invoice line. 
            //A dirty invoice line would be any invoice line that is dirty (edited) and would include a newly created invoice line and an 
            //invoice line that has been marked for deletion.
        //•	If an invoice is persisted then it must persist all its invoice lines.

    //TODO: Tests for Single CompositionRelationship
    [TestFixture]
    public class TestRelatedBOCol_Composition
    {
        private DataStoreInMemory _dataStore;
        private DataAccessorInMemory _dataAccessor;
        private static OrganisationTestBO _organisationTestBO;
        private readonly TestUtilsRelated util = new TestUtilsRelated();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            _dataStore = new DataStoreInMemory();
            _dataAccessor = new DataAccessorInMemory(_dataStore);
            BORegistry.DataAccessor = _dataAccessor;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
            _organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();

        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
            _dataStore.ClearAllBusinessObjects();
            TestUtil.WaitForGC();
        }

        //   TODO: remove option to do contactPerson.OrganisationID = xcsd.
        [Test]
        public void Test_AddMethod_AddPersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------

            Relationship compositionRelationship = GetCompositionRelationship();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(compositionRelationship);
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Add(myBO);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_AddMethod_AddNewChild()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------

            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.AreSame(myBO.Organisation, compositionRelationship.OwningBO);
        }

        [Test]
        public void Test_ResetParent_PersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            // This rule must also be implemented for the reverse relationship.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"]; 
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>) compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            OrganisationTestBO alternateOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsNew);
            Assert.AreSame(myBO.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            try
            {
                myBO.Organisation = alternateOrganisationTestBO;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }
        [Test]
        public void Test_SetParentNull_PersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            // This rule must also be implemented for the reverse relationship.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsNew);
            Assert.AreSame(myBO.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            try
            {
                myBO.Organisation = null;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_ResetParent_NewChild_ReverseRelationship_Loaded()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can be set for myBO since myBO has never
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"]; 
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsTrue(((MultipleRelationship)compositionRelationship).IsRelationshipLoaded);

            //---------------Execute Test ----------------------
            myBO.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(myBO.OrganisationID, organisationTestBO.OrganisationID);
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(myBO));
            util.AssertAddedEventFired();
        }

        [Test]
        public void Test_SetParentNull()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can be set to null for myBO since myBO has never
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());

            //---------------Assert Precondition----------------
            Assert.IsNull(myBO.Organisation);

            //---------------Execute Test ----------------------
            myBO.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(myBO.Organisation);
        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can not be set to null for myBO since myBO has
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            myBO.Organisation = (OrganisationTestBO)compositionRelationship.OwningBO;
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            util.AssertAddedEventNotFired();
            util.AssertRemovedEventNotFired();
            Assert.IsTrue(((MultipleRelationship)compositionRelationship).IsRelationshipLoaded);

            //---------------Execute Test ----------------------
            try
            {
                myBO.Organisation = null;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
                StringAssert.Contains("RemoveChildAction.Prevent", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_RemoveMethod()
        {
            //An invoice line cannot be removed from an Invoice.
            //---------------Set up test pack-------------------
            Relationship compositionRelationship = GetCompositionRelationship();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                new RelatedBusinessObjectCollection<ContactPersonTestBO>(compositionRelationship);
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            myBO.OrganisationID = _organisationTestBO.OrganisationID;
            myBO.Save();
            cpCol.LoadAll();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse((bool) ReflectionUtilities.GetPrivatePropertyValue(cpCol, "Loading"));

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Remove(myBO);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
                StringAssert.Contains("RemoveChildAction.Prevent", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_MultipleRelationship_CollectionIsLoaded_Generic()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            myBO.OrganisationID = organisationTestBO.OrganisationID;
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(compositionRelationship.IsRelationshipLoaded);

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol<ContactPersonTestBO>();

            //---------------Test Result -----------------------
            Assert.IsTrue(((MultipleRelationship)compositionRelationship).IsRelationshipLoaded);
            Assert.AreEqual(1, cpCol.Count);
        }

        [Test]
        public void Test_MultipleRelationship_CollectionIsLoaded()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            myBO.OrganisationID = organisationTestBO.OrganisationID;
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(compositionRelationship.IsRelationshipLoaded);

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();

            //---------------Test Result -----------------------
            Assert.IsTrue(((MultipleRelationship)compositionRelationship).IsRelationshipLoaded);
            Assert.AreEqual(1, cpCol.Count);
        }

        /// <summary>
        /// An invoice is considered to be dirty if it has any dirty invoice line. 
        ///   A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line 
        ///   and an invoice line that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();           

            //---------------Test Result -----------------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        /// <summary>
        /// An invoice is considered to be dirty if it has any dirty invoice line. 
        ///   A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line 
        ///   and an invoice line that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasMark4DeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// An invoice is considered to be dirty if it has any dirty invoice line. 
        ///   A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line 
        ///   and an invoice line that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);

            //---------------Execute Test ----------------------
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.CreateRandomString();
             
            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(myBO));
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(myBO));
        }
        [Test]
        public void Test_GetDirtyChildren_ReturnMark4DeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            cpCol.MarkForDelete(myBO);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(myBO));
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO_delete = CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(myBO_delete));
            Assert.IsTrue(dirtyChildren.Contains(myBO_Edited));
            Assert.IsTrue(dirtyChildren.Contains(myBo_Created));
        }

        [Test]
        public void Test_Relationships_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO_delete = CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = relationships.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(myBO_delete));
            Assert.IsTrue(dirtyChildren.Contains(myBO_Edited));
            Assert.IsTrue(dirtyChildren.Contains(myBo_Created));
        }

        /// <summary>
        /// •	If an invoice is persisted then it must persist all its invoice lines.  
        /// </summary>
        [Test]
        public void Test_ParentPersistsDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship compositionRelationship = (Relationship)relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)compositionRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }
        #region Utils

        private static ContactPersonTestBO CreateSavedContactPerson_AsChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO myBO = CreateUnsavedContactPerson_AsChild(cpCol);
            myBO.Save();
            return myBO;
        }

        private static ContactPersonTestBO CreateUnsavedContactPerson_AsChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            return myBO;
        }
        private static Relationship GetCompositionRelationship()
        {
            MultipleRelationshipDef relationshipDef = new MultipleRelationshipDef(TestUtil.CreateRandomString(),
                                                                                  TestUtil.CreateRandomString(), TestUtil.CreateRandomString(), new RelKeyDef(), false, "", DeleteParentAction.DeleteRelated
                                                                                  , RelationshipType.Composition);
            return relationshipDef.CreateRelationship(_organisationTestBO, _organisationTestBO.Props);
        }

        #endregion

    }
}