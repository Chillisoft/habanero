using System;
using System.Threading;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestMultipleRelationshipCancelEdits_Aggregation
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        #region Utility Methods

        private static MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(
            OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            const RelationshipType relationshipType = RelationshipType.Aggregation;
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef) relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            OrganisationTestBO organisationTestBO;
            return GetAggregationRelationship(out organisationTestBO, out cpCol);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(
            out OrganisationTestBO organisationTestBO, 
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipCol relationships;
            return GetAggregationRelationship(out organisationTestBO, out relationships, out cpCol);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(
            out OrganisationTestBO organisationTestBO, out RelationshipCol relationships,
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            relationships = organisationTestBO.Relationships;
            return GetAggregationRelationship(organisationTestBO, out cpCol);
        }

        private static void CreateDirtyChildren(BusinessObjectCollection<ContactPersonTestBO> cpCol,
                                                out ContactPersonTestBO existingChild,
                                                out ContactPersonTestBO editedChild,
                                                out ContactPersonTestBO deletedChild,
                                                out ContactPersonTestBO removedChild, out ContactPersonTestBO addedChild,
                                                out ContactPersonTestBO createdChildWithEdits,
                                                out ContactPersonTestBO createdChild)
        {
            createdChild = CreateCreatedChild(cpCol);
            createdChildWithEdits = CreateCreatedChildWithEdits(cpCol);
            existingChild = CreateExistingChild(cpCol);
            editedChild = CreateEditedChild(cpCol);
            deletedChild = CreateDeletedChild(cpCol);
            removedChild = CreateRemovedChild(cpCol);
            addedChild = CreateAddedChild(cpCol);
        }

        private static ContactPersonTestBO CreateCreatedChildWithEdits(
            BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO createdChildEdited = CreateCreatedChild(cpCol);
            EditChild(createdChildEdited);
            return createdChildEdited;
        }

        private static ContactPersonTestBO EditChild(ContactPersonTestBO child)
        {
            child.FirstName = TestUtil.GetRandomString();
            return child;
        }

        private static void AssertEditedPropDirtyState(bool expected, ContactPersonTestBO child)
        {
            string message = string.Format("Child edited prop expected{0} to be dirty.",
                                           expected ? "" : " not");
            Assert.AreEqual(expected, child.Props["FirstName"].IsDirty, message);
        }

        private static void AssertFKDirtyState(bool expected, ContactPersonTestBO child)
        {
            string message = string.Format("Child FK expected{0} to be dirty.",
                                           expected ? "" : " not");
            Assert.AreEqual(expected, child.Props["OrganisationID"].IsDirty, message);
        }

        private static ContactPersonTestBO CreateCreatedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO createdChildUnedited = cpCol.CreateBusinessObject();
            return createdChildUnedited;
        }

        private static ContactPersonTestBO CreateCreatedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO createdChild = ContactPersonTestBO.CreateUnsavedContactPerson();
            createdChild.Organisation = organisationTestBO;
            return createdChild;
        }

        private static ContactPersonTestBO CreateAddedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO addedChild = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(addedChild);
            return addedChild;
        }

        private ContactPersonTestBO CreateAddedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO addedChild = ContactPersonTestBO.CreateSavedContactPerson();
            addedChild.Organisation = organisationTestBO;
            return addedChild;
        }

        private static ContactPersonTestBO CreateEditedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildEdited = CreateExistingChild(cpCol);
            EditChild(existingChildEdited);
            return existingChildEdited;
        }

        private static ContactPersonTestBO CreateDeletedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildMarkedForDelete = CreateExistingChild(cpCol);
            existingChildMarkedForDelete.MarkForDelete();
            return existingChildMarkedForDelete;
        }

        private static ContactPersonTestBO CreateRemovedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO removedChild = CreateExistingChild(cpCol);
            cpCol.Remove(removedChild);
            return removedChild;
        }

        private static ContactPersonTestBO CreateRemovedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO removedChild = ContactPersonTestBO.CreateUnsavedContactPerson();
            removedChild.Organisation = organisationTestBO;
            organisationTestBO.Save();
            removedChild.Organisation = null;
            return removedChild;
        }

        private static ContactPersonTestBO CreateExistingChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildEdited = cpCol.CreateBusinessObject();
            existingChildEdited.Surname = TestUtil.GetRandomString();
            existingChildEdited.FirstName = TestUtil.GetRandomString();
            existingChildEdited.Save();
            return existingChildEdited;
        }

        #endregion Utility Methods

        /// <summary>
        /// • If edits to a car are cancelled then it must cancel edits to all its tyres.
        /// </summary>
        [Test]
        public void Test_Acceptance_CancelEditParent_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            RelationshipCol relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship =
                GetAggregationRelationship(out organisationTestBO, out relationships, out cpCol);

            ContactPersonTestBO existingChild;
            ContactPersonTestBO editedChild;
            ContactPersonTestBO deletedChild;
            ContactPersonTestBO removedChild;
            ContactPersonTestBO addedChild;
            ContactPersonTestBO createdChild;
            ContactPersonTestBO createdChildWithEdits;
            CreateDirtyChildren(cpCol, out existingChild, out editedChild,
                                out deletedChild, out removedChild, out addedChild,
                                out createdChildWithEdits, out createdChild);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            Assert.AreEqual(5, cpCol.Count);
            Assert.AreEqual(4, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(createdChildWithEdits.Status.IsDirty);
            Assert.IsTrue(createdChild.Status.IsDirty);


            //---------------Execute Test ----------------------
            organisationTestBO.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(4, cpCol.Count);
            Assert.AreEqual(4, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsFalse(deletedChild.Status.IsDeleted);
            Assert.IsFalse(removedChild.Status.IsDirty);
            Assert.IsFalse(addedChild.Status.IsDirty);
            Assert.IsFalse(createdChildWithEdits.Status.IsDirty);
            Assert.IsFalse(createdChild.Status.IsDirty);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then the dirty children should be cancelled
        /// </summary>
        [Test]
        public void Test_Acceptance_CancelEdit_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);

            ContactPersonTestBO existingChild;
            ContactPersonTestBO editedChild;
            ContactPersonTestBO deletedChild;
            ContactPersonTestBO removedChild;
            ContactPersonTestBO addedChild;
            ContactPersonTestBO createdChild;
            ContactPersonTestBO createdChildWithEdits;
            CreateDirtyChildren(cpCol, out existingChild, out editedChild,
                                out deletedChild, out removedChild, out addedChild,
                                out createdChildWithEdits, out createdChild);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(6, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(5, cpCol.Count);
            Assert.AreEqual(4, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(createdChildWithEdits.Status.IsDirty);
            Assert.IsTrue(createdChild.Status.IsDirty);

            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);

            Assert.AreEqual(4, cpCol.Count);
            Assert.AreEqual(4, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsFalse(deletedChild.Status.IsDeleted);
            Assert.IsFalse(removedChild.Status.IsDirty);
            Assert.IsFalse(addedChild.Status.IsDirty);
            Assert.IsFalse(createdChildWithEdits.Status.IsDirty);
            Assert.IsFalse(createdChild.Status.IsDirty);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// a Dirty child will be cancelled.
        /// </summary>
        [Test]
        public void Test_CancelEdit_ForDirtyChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// an Added child will be removed and cancelled.
        /// </summary>
        [Test]
        public void Test_CancelEdit_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO addedChild = CreateAddedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(addedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// an Created child will be removed and cancelled.
        /// </summary>
        [Test]
        public void Test_CancelEdit_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO createdChild = CreateCreatedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(createdChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(createdChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// an Removed child will be added and cancelled.
        /// </summary>
        [Test]
        public void Test_CancelEdit_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO removedChild = CreateRemovedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(removedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// a Deleted child will be cancelled and the mark for delete will be undone.
        /// </summary>
        [Test]
        public void Test_CancelEdit_ForDeletedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO deletedChild = CreateDeletedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(deletedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(deletedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }



        /// <summary>
        /// • If CancelEdit is called on a Dirty child, then 
        /// edits to the child will be cancelled, and the relationship will be left as is.
        /// </summary>
        [Test]
        public void Test_Child_CancelEdit_ForDirtyChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(editedChild.Status.IsDirty);
            AssertEditedPropDirtyState(true, editedChild);
            AssertFKDirtyState(false, editedChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            editedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(false, editedChild);
            AssertFKDirtyState(false, editedChild);
            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on a Added child, then 
        /// edits to the child will be cancelled, and the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        public void Test_Child_CancelEdit_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO addedChild = EditChild(CreateAddedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, addedChild);
            AssertFKDirtyState(true, addedChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            addedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(false, addedChild);
            AssertFKDirtyState(true, addedChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on a Created child, then 
        /// edits to the child will be cancelled, and the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        public void Test_Child_CancelEdit_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO createdChild = EditChild(CreateCreatedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, createdChild);
            AssertFKDirtyState(true, createdChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            createdChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(false, createdChild);
            AssertFKDirtyState(true, createdChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on a Removed child, then 
        /// edits to the child will be cancelled, and the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        public void Test_Child_CancelEdit_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO removedChild = EditChild(CreateRemovedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, removedChild);
            AssertFKDirtyState(true, removedChild);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            removedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(false, removedChild);
            AssertFKDirtyState(true, removedChild);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on a Deleted child, then 
        /// edits to the child will be cancelled, and the mark for delete will be undone.
        /// </summary>
        [Test]
        public void Test_Child_CancelEdit_ForDeletedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
            ContactPersonTestBO deletedChild = EditChild(CreateDeletedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, deletedChild);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            deletedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(false, deletedChild);
            Assert.IsFalse(deletedChild.Status.IsDeleted);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        //=========================================================================================================================
        //=========================================================================================================================
        //=========================================================================================================================
        //=========================================================================================================================
        
        /// <summary>
        /// • If the child owns the edit and the child's edit is cancelled, then 
        /// the Added child will be removed and cancelled.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        public void Test_Reverse_CancelEdit_ChildOwnsTheEdit_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO; 
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO addedChild = CreateAddedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            addedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(addedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If the child owns the edit and the child's edit is cancelled, then 
        /// the Created child will be removed and cancelled.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        public void Test_Reverse_CancelEdit_ChildOwnsTheEdit_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO createdChild = CreateCreatedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(createdChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            createdChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(createdChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If the child owns the edit and the child's edit is cancelled, then 
        /// the Removed child will be added and cancelled.
        /// </summary>
        [Test]
        public void Test_Reverse_CancelEdit_ChildOwnsTheEdit_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO removedChild = CreateRemovedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            removedChild.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(removedChild.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        ///// <summary>
        ///// • If CancelEdit is called on the relationship, then 
        ///// a Deleted child will be cancelled and the mark for delete will be undone.
        ///// </summary>
        //[Test]
        //public void Test_CancelEdit_ForDeletedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO deletedChild = CreateDeletedChild(cpCol);
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(0, cpCol.Count);
        //    Assert.IsTrue(deletedChild.Status.IsDirty);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    aggregateRelationship.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    Assert.IsFalse(deletedChild.Status.IsDirty);
        //    Assert.IsFalse(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}



        ///// <summary>
        ///// • If CancelEdit is called on a Dirty child, then 
        ///// edits to the child will be cancelled, and the relationship will be left as is.
        ///// </summary>
        //[Test]
        //public void Test_Child_CancelEdit_ForDirtyChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    Assert.IsTrue(editedChild.Status.IsDirty);
        //    AssertEditedPropDirtyState(true, editedChild);
        //    AssertFKDirtyState(false, editedChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    editedChild.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(false, editedChild);
        //    AssertFKDirtyState(false, editedChild);
        //    Assert.IsFalse(editedChild.Status.IsDirty);
        //    Assert.IsFalse(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}

        ///// <summary>
        ///// • If CancelEdit is called on a Added child, then 
        ///// edits to the child will be cancelled, and the relationship will be left as is.
        ///// </summary>
        //[Test]
        //[Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        //public void Test_Child_CancelEdit_ForAddedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO addedChild = EditChild(CreateAddedChild(cpCol));
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(true, addedChild);
        //    AssertFKDirtyState(true, addedChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    addedChild.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(false, addedChild);
        //    AssertFKDirtyState(true, addedChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}

        ///// <summary>
        ///// • If CancelEdit is called on a Created child, then 
        ///// edits to the child will be cancelled, and the relationship will be left as is.
        ///// </summary>
        //[Test]
        //[Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        //public void Test_Child_CancelEdit_ForCreatedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO createdChild = EditChild(CreateCreatedChild(cpCol));
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(true, createdChild);
        //    AssertFKDirtyState(true, createdChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    createdChild.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(false, createdChild);
        //    AssertFKDirtyState(true, createdChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}

        ///// <summary>
        ///// • If CancelEdit is called on a Removed child, then 
        ///// edits to the child will be cancelled, and the relationship will be left as is.
        ///// </summary>
        //[Test]
        //[Ignore("Working on this")] //TODO Mark 27 Feb 2009: Ignored Test - Working on this
        //public void Test_Child_CancelEdit_ForRemovedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO removedChild = EditChild(CreateRemovedChild(cpCol));
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(0, cpCol.Count);
        //    AssertEditedPropDirtyState(true, removedChild);
        //    AssertFKDirtyState(true, removedChild);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    removedChild.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(0, cpCol.Count);
        //    AssertEditedPropDirtyState(false, removedChild);
        //    AssertFKDirtyState(true, removedChild);
        //    Assert.IsFalse(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}

        ///// <summary>
        ///// • If CancelEdit is called on a Deleted child, then 
        ///// edits to the child will be cancelled, and the mark for delete will be undone.
        ///// </summary>
        //[Test]
        //public void Test_Child_CancelEdit_ForDeletedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship(out cpCol);
        //    ContactPersonTestBO deletedChild = EditChild(CreateDeletedChild(cpCol));
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(0, cpCol.Count);
        //    AssertEditedPropDirtyState(true, deletedChild);
        //    Assert.IsTrue(deletedChild.Status.IsDeleted);
        //    Assert.IsTrue(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(1, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
        //    //---------------Execute Test ----------------------
        //    deletedChild.CancelEdits();
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    AssertEditedPropDirtyState(false, deletedChild);
        //    Assert.IsFalse(deletedChild.Status.IsDeleted);
        //    Assert.IsFalse(aggregateRelationship.IsDirty);
        //    Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}
    }
}