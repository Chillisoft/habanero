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

using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestMultipleRelationship 
    {
 
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [Test]
        public void TestTypeOfMultipleCollection()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            new AddressTestBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();

            Assert.AreSame(typeof(RelatedBusinessObjectCollection<AddressTestBO>), cp.Addresses.GetType());
        }

        [Test]
        public void TestReloadingRelationship()
        {
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            new AddressTestBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            IBusinessObjectCollection addresses = cp.Addresses;
            Assert.AreSame(addresses, cp.Addresses);
        }

        [Test]
        public void TestColIsInstantiatedButNotLoaded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef();
            ClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefOrganisationRelationship();
            RelKeyDef keyDef = new RelKeyDef();
            keyDef.Add(new RelPropDef(contactPersonClassDef.PropDefcol["OrganisationID"], "OrganisationID"));
            MultipleRelationshipDef def = new MultipleRelationshipDef
                (TestUtil.CreateRandomString(), typeof(ContactPersonTestBO),
                 keyDef, false, "", DeleteParentAction.DoNothing);

            OrganisationTestBO organisation = new OrganisationTestBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IMultipleRelationship rel = (IMultipleRelationship) def.CreateRelationship(organisation, organisation.Props);

            //---------------Test Result -----------------------

            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
            Assert.AreSame(contactPersonClassDef, collection.ClassDef);
            Assert.IsInstanceOfType(typeof(ContactPersonTestBO), collection.CreateBusinessObject());
        }


        [Test]
        public void Test_Composition_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(compositionRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(compositionRelationship.IsDirty);
        }

        [Test]
        public void Test_Aggregation_DirtyIfHasCreatedChildren()
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

        [Test]
        public void Test_Association_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(associationRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(associationRelationship.IsDirty);
        }

        [Test]
        public void Test_Composition_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);
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

        [Test]
        public void Test_Aggregation_DirtyIfHasMarkForDeleteChildren()
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

        [Test]
        public void Test_Association_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);
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

        [Test]
        public void Test_Aggregation_DirtyIfHasRemoveChildren()
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

        [Test]
        public void Test_Association_DirtyIfHasRemoveChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);
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

        [Test]
        public void Test_Composition_DirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);

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


        [Test]
        public void Test_Aggregation_DirtyIfHasDirtyChildren()
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


        [Test]
        public void Test_Association_NotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(relationship.IsDirty);
        }





        private MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = RelationshipType.Composition;
            return GetRelationship(organisationTestBO, relationshipType, out cpCol);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = RelationshipType.Aggregation;
            return GetRelationship(organisationTestBO, relationshipType, out cpCol);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = RelationshipType.Association;
            return GetRelationship(organisationTestBO, relationshipType, out cpCol);
        }

        private MultipleRelationship<ContactPersonTestBO> GetRelationship(OrganisationTestBO organisationTestBO, RelationshipType relationshipType, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }

        [Test]
        public void Test_Composition_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);

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
        public void Test_Aggregation_GetDirtyChildren_Edited()
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
        public void Test_Association_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        [Test]
        public void Test_Composition_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);

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
        public void Test_Aggregation_GetDirtyChildren_Created()
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
        public void Test_Association_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

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
        public void Test_Composition_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);

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
        public void Test_Aggregation_GetDirtyChildren_MarkedForDelete()
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
        public void Test_Association_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

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
        public void Test_Composition_GetDirtyChildren_Removed()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO, out cpCol);

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
        public void Test_Aggregation_GetDirtyChildren_Removed()
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
        public void Test_Association_GetDirtyChildren_Removed()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

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
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO myBO_delete = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = ContactPersonTestBO.CreateUnsavedContactPerson_AsChild(cpCol);
            ContactPersonTestBO myBo_Removed = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.Remove(myBo_Removed);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO_delete, (ICollection)dirtyChildren);
            Assert.Contains(myBO_Edited, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Created, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Removed, (ICollection)dirtyChildren);
            Assert.AreEqual(4, dirtyChildren.Count);
        }


    }
}
