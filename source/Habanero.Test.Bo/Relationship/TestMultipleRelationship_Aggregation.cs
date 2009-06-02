//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Collections.Generic;
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregationRelationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

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
            GetAggregationRelationship(organisationTestBO, out cpCol);

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
            MultipleRelationship<ContactPersonTestBO> aggregationRelationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);
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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            GetAggregationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

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
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }


        [Test]
        public void Test_GetDirtyChildren_WhenCreated_WhenInsertParentAction_Insert_ShouldReturnCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        [Test]
        public void Test_GetDirtyChildren_WhenCreated_WhenInsertParentAction_DoNothing_ShouldNotReturnCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);
            ((RelationshipDef) relationship.RelationshipDef).InsertParentAction = InsertParentAction.DoNothing;
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            Assert.AreEqual(InsertParentAction.DoNothing, relationship.RelationshipDef.InsertParentAction);
            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        [Test]
        public void Test_GetDirtyChildren_Added()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
        public void Test_PersistParent_PersistsDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship = GetAggregationRelationship
                (organisationTestBO, out cpCol);

            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.GetRandomString();

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

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship
            (OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            const RelationshipType relationshipType = RelationshipType.Aggregation;
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef) relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }
    }
}