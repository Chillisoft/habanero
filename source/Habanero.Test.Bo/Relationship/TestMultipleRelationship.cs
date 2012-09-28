#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestMultipleRelationship 
    {
 
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
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
            IClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            RelKeyDef keyDef = new RelKeyDef();
            keyDef.Add(new RelPropDef(contactPersonClassDef.PropDefcol["OrganisationID"], "OrganisationID"));
            MultipleRelationshipDef def = new MultipleRelationshipDef
                (TestUtil.GetRandomString(), typeof(ContactPersonTestBO),
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
            Assert.IsNotNull(collection.SelectQuery.Criteria);
            Assert.IsNotNull(collection.SelectQuery.Criteria.Field);
            Assert.AreEqual("OrganisationID", collection.SelectQuery.Criteria.Field.PropertyName);
            Assert.IsNotNull(collection.SelectQuery.Criteria.Field.Source);
            Assert.AreEqual("ContactPersonTestBO", collection.SelectQuery.Criteria.Field.Source.Name);
            Assert.AreEqual(organisation.OrganisationID.Value, collection.SelectQuery.Criteria.FieldValue);
            Assert.IsInstanceOf(typeof(ContactPersonTestBO), collection.CreateBusinessObject());
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
            myBO_Edited.Surname = TestUtil.GetRandomString();

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

        [Test]
        public void Test_CanDeleteParentWithNewChildren()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            organisationTestBO.ContactPeople.CreateBusinessObject();

            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            organisationTestBO.MarkForDelete();
            organisationTestBO.Save();
            //--------------- Test Result -----------------------
            Assert.IsTrue(organisationTestBO.Status.IsDeleted);
        }

        [Test]
        public void Test_CreateMultipleRelationshipWithTimeout()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef) orgClassDef.RelationshipDefCol["ContactPeople"];
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO> (organisationTestBO, relationshipDef, organisationTestBO.Props, 30000);
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not give an error when constructing");
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutNotExceeded_DoesNotReloadFromDB()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)orgClassDef.RelationshipDefCol["ContactPeople"];
            IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO>(organisationTestBO, relationshipDef, organisationTestBO.Props, 30000);
            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            Assert.AreEqual(0, collection.Count);
            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            TestUtil.Wait(100);
            IBusinessObjectCollection secondCollectionCall = rel.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.AreEqual(0, secondCollectionCall.Count);
            Assert.AreEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutExceeded_WhenTimeOutZero_DoesResetTheTimeLastLoaded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)orgClassDef.RelationshipDefCol["ContactPeople"];
            IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO>(organisationTestBO, relationshipDef, organisationTestBO.Props, 0);
            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collection.TimeLastLoaded);
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            //---------------Execute Test ----------------------
            TestUtil.Wait(1000);
            IBusinessObjectCollection secondCollectionCall = rel.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.IsNotNull(secondCollectionCall.TimeLastLoaded);
            Assert.AreNotEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutExceeded_WhenTimeOutZero_UsingRelationship_DoesResetTheTimeLastLoaded()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)orgClassDef.RelationshipDefCol["ContactPeople"];
            //IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO>(organisationTestBO, relationshipDef, organisationTestBO.Props, 0);
            IBusinessObjectCollection collection = organisationTestBO.ContactPeople;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collection.TimeLastLoaded);
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            //---------------Execute Test ----------------------
            TestUtil.Wait(100);
            IBusinessObjectCollection secondCollectionCall = organisationTestBO.ContactPeople;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.IsNotNull(secondCollectionCall.TimeLastLoaded);
            Assert.AreNotEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutExceeded_WhenTimeOutZero_DoesReloadFromDB()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)orgClassDef.RelationshipDefCol["ContactPeople"];
            IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO>(organisationTestBO, relationshipDef, organisationTestBO.Props, 0);
            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collection.TimeLastLoaded);
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            MultipleRelationship<ContactPersonTestBO> relationship = (MultipleRelationship<ContactPersonTestBO>)organisationTestBO.Relationships["ContactPeople"];
            Assert.AreEqual(0, relationship.TimeOut);
            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            TestUtil.Wait(100);
            IBusinessObjectCollection secondCollectionCall = rel.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.IsNotNull(secondCollectionCall.TimeLastLoaded);
            Assert.AreEqual(1, secondCollectionCall.Count);
            Assert.AreNotEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutExceeded_WhenTimeOutZero_UsingRelationship_DoesReloadFromDB()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            IBusinessObjectCollection collection = organisationTestBO.ContactPeople;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collection.TimeLastLoaded);
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            Assert.AreEqual(0, collection.Count);
            MultipleRelationship<ContactPersonTestBO> relationship = (MultipleRelationship<ContactPersonTestBO>) organisationTestBO.Relationships["ContactPeople"];
            Assert.AreEqual(0, relationship.TimeOut);
            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            TestUtil.Wait(100);
            IBusinessObjectCollection secondCollectionCall = organisationTestBO.ContactPeople;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.IsNotNull(secondCollectionCall.TimeLastLoaded);
            Assert.AreEqual(1, secondCollectionCall.Count);
            Assert.AreNotEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        [Test]
        public void Test_LoadMultiplerelationship_TimeOutExceeded_WhenTimeOutTwo_DoesReloadFromDB()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)orgClassDef.RelationshipDefCol["ContactPeople"];
            IMultipleRelationship rel = new MultipleRelationship<ContactPersonTestBO>(organisationTestBO, relationshipDef, organisationTestBO.Props, 2);
            IBusinessObjectCollection collection = rel.BusinessObjectCollection;
            DateTime? initialTimeLastLoaded = collection.TimeLastLoaded;
            //---------------Assert Precondition----------------
            Assert.AreEqual(initialTimeLastLoaded, collection.TimeLastLoaded);
            //---------------Execute Test ----------------------
            TestUtil.Wait(100);
            IBusinessObjectCollection secondCollectionCall = rel.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(collection, secondCollectionCall);
            Assert.AreNotEqual(initialTimeLastLoaded, secondCollectionCall.TimeLastLoaded);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAggregationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            const RelationshipType relationshipType = RelationshipType.Aggregation;
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

      


    }
}
