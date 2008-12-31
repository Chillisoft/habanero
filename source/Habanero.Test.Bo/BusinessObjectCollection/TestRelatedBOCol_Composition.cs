using System;
using System.Collections;
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

    [TestFixture]
    public class TestRelatedBOCol_Composition
    {
        private readonly TestUtilsRelated util = new TestUtilsRelated();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [TearDown]
        public void TearDownTest()
        {
            TestUtil.WaitForGC();
        }

        //   TODO: remove option to do contactPerson.OrganisationID = xcsd.

        [Test]
        public void Test_AddMethod_AddPersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Add(contactPerson);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                AssertMessageIsCorrect(ex, compositionRelationship.RelationshipDef.RelatedObjectClassName, "added", compositionRelationship.RelationshipName);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        private MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(out BusinessObjectCollection<ContactPersonTestBO> cpCol) {
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            return GetCompositionRelationship(out cpCol, organisationTestBO);
        }

        private MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(out BusinessObjectCollection<ContactPersonTestBO> cpCol, OrganisationTestBO organisationTestBO)
        {
            MultipleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            cpCol = compositionRelationship.BusinessObjectCollection;
            return compositionRelationship;
        }


        [Test]
        public void Test_AddMethod_AddNewChild()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            OrganisationTestBO alternateOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);
            Assert.AreSame(contactPerson.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = alternateOrganisationTestBO;
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);
            Assert.AreSame(contactPerson.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = null;
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.OrganisationID, organisationTestBO.OrganisationID);
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(contactPerson));
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
        public void Test_RemoveMethod()
        {
            //An invoice line cannot be removed from an Invoice.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            contactPerson.Save();
            cpCol.LoadAll();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse((bool)ReflectionUtilities.GetPrivatePropertyValue(cpCol, "Loading"));

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Remove(contactPerson);
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
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can not be set to null for myBO since myBO has
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            contactPerson.Organisation = (OrganisationTestBO)compositionRelationship.OwningBO;
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            util.AssertAddedEventNotFired();
            util.AssertRemovedEventNotFired();

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = null;
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);

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
        public void Test_ParentDirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }
        [Test]
        public void Test_GetDirtyChildren_ReturnMark4DeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.MarkForDelete(contactPerson);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO myBO_delete = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = ContactPersonTestBO.CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO_delete, (ICollection)dirtyChildren);
            Assert.Contains(myBO_Edited, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Created, (ICollection)dirtyChildren);
            Assert.AreEqual(3, dirtyChildren.Count);
        }

        [Test]
        public void Test_Relationships_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO myBO_delete = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = ContactPersonTestBO.CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = relationships.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO_delete, (ICollection)dirtyChildren);
            Assert.Contains(myBO_Edited, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Created, (ICollection)dirtyChildren);
            Assert.AreEqual(3, dirtyChildren.Count);
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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
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

        private void AssertMessageIsCorrect(HabaneroDeveloperException ex, string relatedObjectClassName, string operation, string relationshipName)
        {
            StringAssert.Contains("The " + relatedObjectClassName, ex.Message);
            StringAssert.Contains("could not be " + operation + " since the " + relationshipName + " relationship is set up as ", ex.Message);
        }


        #endregion

    }
}