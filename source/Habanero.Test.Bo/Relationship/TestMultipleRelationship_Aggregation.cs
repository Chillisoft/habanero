using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestMultipleRelationship_Aggregation
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }



        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregationRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(aggregationRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(aggregationRelationship.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();
            bool isDirty = organisationTestBO.Status.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_DirtyIfHasAddedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregationRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsFalse(aggregationRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            Assert.IsTrue(aggregationRelationship.IsDirty);
        }

        [Test]
        public void Test_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);
            bool isDirty = organisationTestBO.Status.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_DirtyIfHasRemoveChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
        }



        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasRemovedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);
            bool isDirty = organisationTestBO.Status.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_DirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();
            bool isDirty = organisationTestBO.Status.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }


        [Test]
        public void Test_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_Added()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.MarkForDelete(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_Removed()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.MarkForDelete(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        /// <summary>
        /// • If a car is persisted then it must persist all its tyres.
        /// </summary>
        [Test]
        public void Test_ParentPersistsDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(myBO.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = RelationshipType.Aggregation;
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }
    }

    [TestFixture]
    public class TestMultipleRelationship_Aggregation_DB : TestMultipleRelationship_Aggregation
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }
}
