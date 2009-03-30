using Habanero.Base;
using NUnit.Framework;
using Habanero.BO;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    [Ignore("Working on this")] //TODO Mark 24 Mar 2009: Ignored Tests - Working on this
    public class TestMultipleRelationshipCancelEdits_Association : TestMultipleRelationshipCancelEdits_Base
    {
        protected override RelationshipType GetRelationshipType()
        {
            return RelationshipType.Association;
        }

        /// <summary>
        /// • If edits to a car are cancelled then it must cancel edits to all its tyres.
        /// </summary>
        [Test]
        public virtual void Test_Acceptance_CancelEditParent_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            RelationshipCol relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship =
                GetRelationship(out organisationTestBO, out relationships, out cpCol);

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
            Assert.IsTrue(associationRelationship.IsDirty);
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
            Assert.IsFalse(associationRelationship.IsDirty);
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
        public virtual void Test_Acceptance_CancelEdit_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);

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
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(6, associationRelationship.GetDirtyChildren().Count);
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
            associationRelationship.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);

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
        public void Test_CancelEdit_Owner_ForDirtyChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_Owner_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO addedChild = CreateAddedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(addedChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_Owner_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO createdChild = CreateCreatedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(createdChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(createdChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_Owner_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO removedChild = CreateRemovedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(removedChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public void Test_CancelEdit_Owner_ForDeletedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO deletedChild = CreateDeletedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(deletedChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(deletedChild.Status.IsDirty);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public void Test_CancelEdit_NonOwner_ForDirtyChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(editedChild.Status.IsDirty);
            AssertEditedPropDirtyState(true, editedChild);
            AssertFKDirtyState(false, editedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_NonOwner_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO addedChild = CreateAddedChild(cpCol);
            addedChild = EditChild(addedChild);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, addedChild);
            AssertFKDirtyState(true, addedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
        public void Test_CancelEdit_NonOwner_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO createdChild = EditChild(CreateCreatedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, createdChild);
            AssertFKDirtyState(true, createdChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_NonOwner_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO removedChild = EditChild(CreateRemovedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, removedChild);
            AssertFKDirtyState(true, removedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
        public void Test_CancelEdit_NonOwner_ForDeletedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
            ContactPersonTestBO deletedChild = EditChild(CreateDeletedChild(cpCol));
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, deletedChild);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_Owner_ChildOwnsTheEdit_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO addedChild = CreateAddedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(addedChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public void Test_CancelEdit_Owner_ChildOwnsTheEdit_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO createdChild = CreateCreatedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(createdChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
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
        public virtual void Test_CancelEdit_Owner_ChildOwnsTheEdit_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO removedChild = CreateRemovedChild_ChildOwnsEdit(organisationTestBO);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(removedChild.Status.IsDirty);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        ///// <summary>
        ///// • If CancelEdit is called on a Dirty child, then 
        ///// edits to the child will be cancelled, and the relationship will be left as is.
        ///// </summary>
        //[Test]
        //public void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForDirtyChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
        //    ContactPersonTestBO editedChild = CreateEditedChild(cpCol);
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(1, cpCol.Count);
        //    Assert.IsTrue(editedChild.Status.IsDirty);
        //    AssertEditedPropDirtyState(true, editedChild);
        //    AssertFKDirtyState(false, editedChild);
        //    Assert.IsTrue(associationRelationship.IsDirty);
        //    Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
        //    Assert.IsFalse(associationRelationship.IsDirty);
        //    Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}

        /// <summary>
        /// • If CancelEdit is called on the parent when a child is Added through edits on the child, then 
        /// edits to the child will not be cancelled, and the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 02 Mar 2009: Ignored Test - Working on this
        public virtual void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForAddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO addedChild = EditChild(CreateAddedChild_ChildOwnsEdit(organisationTestBO));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, addedChild);
            AssertFKDirtyState(true, addedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, addedChild);
            AssertFKDirtyState(true, addedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the parent when a child is Created and associated through 
        /// edits on the child, then edits to the child will not be cancelled, and 
        /// the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 02 Mar 2009: Ignored Test - Working on this
        public void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO createdChild = EditChild(CreateCreatedChild_ChildOwnsEdit(organisationTestBO));
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, createdChild);
            AssertFKDirtyState(true, createdChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            AssertEditedPropDirtyState(true, createdChild);
            AssertFKDirtyState(true, createdChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// • If CancelEdit is called on the parent when a child is Removed through 
        /// edits on the child, then edits to the child will not be cancelled, and 
        /// the relationship will be left as is.
        /// </summary>
        [Test]
        [Ignore("Working on this")] //TODO Mark 02 Mar 2009: Ignored Test - Working on this
        public virtual void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForRemovedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out organisationTestBO, out cpCol);
            ContactPersonTestBO removedChild = EditChild(CreateRemovedChild_ChildOwnsEdit(organisationTestBO));
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, removedChild);
            AssertFKDirtyState(true, removedChild);
            Assert.IsTrue(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            associationRelationship.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            AssertEditedPropDirtyState(true, removedChild);
            AssertFKDirtyState(true, removedChild);
            Assert.IsFalse(associationRelationship.IsDirty);
            Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }

        ///// <summary>
        ///// • If CancelEdit is called on a Deleted child, then 
        ///// edits to the child will be cancelled, and the mark for delete will be undone.
        ///// </summary>
        //[Test]
        //public void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForDeletedChild()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectCollection<ContactPersonTestBO> cpCol;
        //    MultipleRelationship<ContactPersonTestBO> associationRelationship = GetRelationship(out cpCol);
        //    ContactPersonTestBO deletedChild = EditChild(CreateDeletedChild(cpCol));
        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(0, cpCol.Count);
        //    AssertEditedPropDirtyState(true, deletedChild);
        //    Assert.IsTrue(deletedChild.Status.IsDeleted);
        //    Assert.IsTrue(associationRelationship.IsDirty);
        //    Assert.AreEqual(1, associationRelationship.GetDirtyChildren().Count);
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
        //    Assert.IsFalse(associationRelationship.IsDirty);
        //    Assert.AreEqual(0, associationRelationship.GetDirtyChildren().Count);
        //    Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        //    Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        //}
    }
}
