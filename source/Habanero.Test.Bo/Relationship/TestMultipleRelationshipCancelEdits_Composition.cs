// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestMultipleRelationshipCancelEdits_Composition : TestMultipleRelationshipCancelEdits_Aggregation
    {
        protected override RelationshipType GetRelationshipType()
        {
            return RelationshipType.Composition;
        }

        protected void CreateDirtyChildren(BusinessObjectCollection<ContactPersonTestBO> cpCol,
                                out ContactPersonTestBO existingChild,
                                out ContactPersonTestBO editedChild,
                                out ContactPersonTestBO deletedChild,
                                out ContactPersonTestBO createdChildWithEdits,
                                out ContactPersonTestBO createdChild)
        {
            createdChild = CreateCreatedChild(cpCol);
            createdChildWithEdits = CreateCreatedChildWithEdits(cpCol);
            existingChild = CreateExistingChild(cpCol);
            editedChild = CreateEditedChild(cpCol);
            deletedChild = CreateDeletedChild(cpCol);
        }

        /// <summary>
        /// • If edits to an invoice are cancelled then it must cancel edits to all its invoice lines.
        /// </summary>
        [Test]
        public override void Test_Acceptance_CancelEditParent_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            RelationshipCol relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship =
                GetRelationship(out organisationTestBO, out relationships, out cpCol);

            ContactPersonTestBO existingChild;
            ContactPersonTestBO editedChild;
            ContactPersonTestBO deletedChild;
            ContactPersonTestBO createdChild;
            ContactPersonTestBO createdChildWithEdits;
            CreateDirtyChildren(cpCol, out existingChild, out editedChild,
                                out deletedChild,
                                out createdChildWithEdits, out createdChild);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            Assert.AreEqual(4, cpCol.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(createdChildWithEdits.Status.IsDirty);
            Assert.IsTrue(createdChild.Status.IsDirty);


            //---------------Execute Test ----------------------
            organisationTestBO.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(3, cpCol.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsFalse(deletedChild.Status.IsDeleted);
            Assert.IsFalse(createdChildWithEdits.Status.IsDirty);
            Assert.IsFalse(createdChild.Status.IsDirty);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then the dirty children should be cancelled
        /// </summary>
        [Test]
        public override void Test_Acceptance_CancelEdit_CancelsEditsForDirtyChildren()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetRelationship(out cpCol);

            ContactPersonTestBO existingChild;
            ContactPersonTestBO editedChild;
            ContactPersonTestBO deletedChild;
            ContactPersonTestBO createdChild;
            ContactPersonTestBO createdChildWithEdits;
            CreateDirtyChildren(cpCol, out existingChild, out editedChild,
                                out deletedChild,
                                out createdChildWithEdits, out createdChild);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.AreEqual(4, aggregateRelationship.GetDirtyChildren().Count);
            Assert.AreEqual(4, cpCol.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsTrue(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsTrue(deletedChild.Status.IsDeleted);
            Assert.IsTrue(createdChildWithEdits.Status.IsDirty);
            Assert.IsTrue(createdChild.Status.IsDirty);

            //---------------Execute Test ----------------------
            aggregateRelationship.CancelEdits();

            //---------------Test Result -----------------------
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.AreEqual(0, aggregateRelationship.GetDirtyChildren().Count);

            Assert.AreEqual(3, cpCol.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            Assert.IsFalse(editedChild.Status.IsDirty);
            Assert.IsFalse(existingChild.Status.IsDirty);
            Assert.IsFalse(deletedChild.Status.IsDeleted);
            Assert.IsFalse(createdChildWithEdits.Status.IsDirty);
            Assert.IsFalse(createdChild.Status.IsDirty);
        }

        /// <summary>
        /// • If CancelEdit is called on the relationship, then 
        /// an Created child will be removed and cancelled.
        /// </summary>
        [Test]
        public override void Test_CancelEdit_Owner_ForCreatedChild()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            Habanero.BO.MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetRelationship(out cpCol);
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
//            Assert.IsTrue(createdChild.Status.IsDeleted, "Is permanently deleted");
            Assert.IsTrue(createdChild.Status.IsNew, "Is permanently deleted");
            Assert.IsNull(createdChild.Organisation);
        }
        [Test]
        public override void Test_CancelEdit_Owner_ForAddedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_Owner_ForAddedChild();
        }
        [Test]
        public override void Test_CancelEdit_Owner_ForRemovedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_Owner_ForRemovedChild();
        }
        [Test]
        public override void Test_CancelEdit_NonOwner_ForAddedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_NonOwner_ForAddedChild();
        }
        [Test]
        public override void Test_CancelEdit_NonOwner_ForRemovedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_NonOwner_ForRemovedChild();
        }
        [Test]
        public override void Test_CancelEdit_Owner_ChildOwnsTheEdit_ForAddedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_Owner_ChildOwnsTheEdit_ForAddedChild();
        }
        [Test]
        public override void Test_CancelEdit_Owner_ChildOwnsTheEdit_ForRemovedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_Owner_ChildOwnsTheEdit_ForRemovedChild();
        }
        [Test]
        public override void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForAddedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForAddedChild();
        }
        [Test]
        public override void Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForRemovedChild()
        {
            //This scenario does not apply for a Composition relationship
            //base.Test_CancelEdit_NonOwner_ChildOwnsTheEdit_ForRemovedChild();
        }
    }
}