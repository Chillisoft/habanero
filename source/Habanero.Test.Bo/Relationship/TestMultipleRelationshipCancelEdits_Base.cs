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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    public abstract class TestMultipleRelationshipCancelEdits_Base
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
        
        protected abstract RelationshipType GetRelationshipType();

        protected MultipleRelationship<ContactPersonTestBO> GetRelationship(
            OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = GetRelationshipType();
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef) relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }


        protected MultipleRelationship<ContactPersonTestBO> GetRelationship(
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            OrganisationTestBO organisationTestBO;
            return GetRelationship(out organisationTestBO, out cpCol);
        }

        protected MultipleRelationship<ContactPersonTestBO> GetRelationship(
            out OrganisationTestBO organisationTestBO, 
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipCol relationships;
            return GetRelationship(out organisationTestBO, out relationships, out cpCol);
        }

        protected MultipleRelationship<ContactPersonTestBO> GetRelationship(
            out OrganisationTestBO organisationTestBO, out RelationshipCol relationships,
            out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            relationships = organisationTestBO.Relationships;
            return GetRelationship(organisationTestBO, out cpCol);
        }

        protected virtual void CreateDirtyChildren(BusinessObjectCollection<ContactPersonTestBO> cpCol,
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

        protected static ContactPersonTestBO CreateCreatedChildWithEdits(
            BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO createdChildEdited = CreateCreatedChild(cpCol);
            EditChild(createdChildEdited);
            return createdChildEdited;
        }

        protected static ContactPersonTestBO EditChild(ContactPersonTestBO child)
        {
            child.FirstName = TestUtil.GetRandomString();
            return child;
        }

        protected static void AssertEditedPropDirtyState(bool expected, ContactPersonTestBO child)
        {
            string message = string.Format("Child edited prop expected{0} to be dirty.",
                                           expected ? "" : " not");
            Assert.AreEqual(expected, child.Props["FirstName"].IsDirty, message);
        }

        protected static void AssertFKDirtyState(bool expected, ContactPersonTestBO child)
        {
            string message = string.Format("Child FK expected{0} to be dirty.",
                                           expected ? "" : " not");
            Assert.AreEqual(expected, child.Props["OrganisationID"].IsDirty, message);
        }

        protected static ContactPersonTestBO CreateCreatedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO createdChildUnedited = cpCol.CreateBusinessObject();
            return createdChildUnedited;
        }

        protected static ContactPersonTestBO CreateCreatedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO createdChild = ContactPersonTestBO.CreateUnsavedContactPerson();
            createdChild.Organisation = organisationTestBO;
            return createdChild;
        }

        protected static ContactPersonTestBO CreateAddedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO addedChild = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(addedChild);
            return addedChild;
        }

        protected virtual ContactPersonTestBO CreateAddedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO addedChild = ContactPersonTestBO.CreateSavedContactPerson();
            addedChild.Organisation = organisationTestBO;
            return addedChild;
        }

        protected static ContactPersonTestBO CreateEditedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildEdited = CreateExistingChild(cpCol);
            EditChild(existingChildEdited);
            return existingChildEdited;
        }

        protected static ContactPersonTestBO CreateDeletedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildMarkedForDelete = CreateExistingChild(cpCol);
            cpCol.MarkForDelete(existingChildMarkedForDelete);
            return existingChildMarkedForDelete;
        }

        protected static ContactPersonTestBO CreateDeletedChild_ChildOwnsEdit(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildMarkedForDelete = CreateExistingChild(cpCol);
            existingChildMarkedForDelete.MarkForDelete();
            return existingChildMarkedForDelete;
        }

        protected static ContactPersonTestBO CreateRemovedChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO removedChild = CreateExistingChild(cpCol);
            cpCol.Remove(removedChild);
            return removedChild;
        }

        protected static ContactPersonTestBO CreateRemovedChild_ChildOwnsEdit(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO removedChild = ContactPersonTestBO.CreateUnsavedContactPerson();
            removedChild.Organisation = organisationTestBO;
            organisationTestBO.Save();
            removedChild.Organisation = null;
            return removedChild;
        }

        protected static ContactPersonTestBO CreateExistingChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO existingChildEdited = cpCol.CreateBusinessObject();
            existingChildEdited.Surname = TestUtil.GetRandomString();
            existingChildEdited.FirstName = TestUtil.GetRandomString();
            existingChildEdited.Save();
            return existingChildEdited;
        }

        #endregion Utility Methods
    }
}
