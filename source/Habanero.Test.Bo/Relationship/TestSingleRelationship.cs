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

using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    /// <summary>
    /// Summary description for TestSingleRelationship.
    /// </summary>
    [TestFixture]
    public class TestSingleRelationship
    {

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
           
        }

        [Test]
        public void TestSetRelatedObject()
        {
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            ClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO) classDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo) relatedClassDef.CreateNewBusinessObject();
            bo1.Relationships.GetSingle("MyRelationship").SetRelatedObject(relatedBo1);
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }


        [Test]
        public void Test_SetToNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
           
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = null;

            //---------------Test Result -----------------------
            Assert.IsNull(organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetToNull_ByID()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            contactPerson.OrganisationID = null;

            //---------------Execute Test ----------------------
            ContactPersonTestBO currentContactPerson = organisationTestBO.ContactPerson;

            //---------------Test Result -----------------------
            Assert.IsNull(currentContactPerson);
        }


        [Test]
        public void Test_SetToAlternate_ByID_InBOManager()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();

            ContactPersonTestBO alternatecontactPerson = new ContactPersonTestBO();
            alternatecontactPerson.Surname = TestUtil.CreateRandomString();
            alternatecontactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = null;
            alternatecontactPerson.OrganisationID = organisationTestBO.OrganisationID;
            
            //---------------Test Result -----------------------
            //Assert.AreSame(organisationTestBO, alternatecontactPerson.Organisation);
            Assert.AreSame(alternatecontactPerson, organisationTestBO.ContactPerson);
        }
        
        [Test]
        public void Test_SetByID_InBOManager()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();


            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }
        



        [Test]
        public void Test_IsRemoved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsRemoved);
            Assert.IsNull(relationship.RemovedBO);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = null;

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsRemoved);
            Assert.AreSame(myBO, relationship.RemovedBO);

        }

        [Test]
        public void Test_IsRemoved_False()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            organisationTestBO.ContactPerson = null;

            //---------------Assert Precondition----------------
            Assert.IsTrue(relationship.IsRemoved);
            Assert.AreSame(contactPerson, relationship.RemovedBO);

            //---------------Execute Test ----------------------

            organisationTestBO.ContactPerson = contactPerson;

            //---------------Test Result -----------------------
            Assert.IsFalse(relationship.IsRemoved);
            Assert.IsNull(relationship.RemovedBO);
        }

        [Test]
        public void Test_ErrorIfBothOwningBOHasForeignKey()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = true;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);

            //---------------Assert Precondition----------------
            Assert.AreEqual(contactPerson.OrganisationID, organisation.OrganisationID);
            Assert.AreSame(organisation.ContactPerson, contactPerson);

            //---------------Execute Test ----------------------
            try
            {
                relationship.SetRelatedObject(null);
                Assert.Fail("An error should have occurred as corresponding single relationships are not configured correctly (one should have the OwningBOHasForeignKey property set to false)");
            } catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The corresponding single (one to one) relationships ", ex.Message);
                StringAssert.Contains("ContactPerson (on OrganisationTestBO)", ex.Message);
                StringAssert.Contains("Organisation (on ContactPersonTestBO)", ex.Message);
                StringAssert.Contains("cannot both be configured as having the foreign key", ex.Message);
            }
        }


        [Test]
        public void Test_SetRelatedObjectRaisesUpdatedEvent_Referenced()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<OrganisationTestBO> relationship =
                contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            bool updatedFired = false;
            OrganisationTestBO boReceivedByEvent = null;
            OrganisationTestBO currentOrganisationInEvent = null;
            Guid? organisationidInEvent = null;
            
            relationship.Updated += delegate(object sender, BOEventArgs<OrganisationTestBO> e)
                                    {
                                        updatedFired = true;
                                        boReceivedByEvent = e.BusinessObject;
                                        organisationidInEvent = contactPersonTestBO.OrganisationID;
                                        currentOrganisationInEvent = contactPersonTestBO.Organisation;
                                    };

            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedFired);
            Assert.IsNull(boReceivedByEvent);

            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedFired);
            Assert.AreSame(organisationTestBO, boReceivedByEvent);
            Assert.AreSame(organisationTestBO, currentOrganisationInEvent);
            Assert.AreEqual(organisationTestBO.OrganisationID, organisationidInEvent);
        }


        [Test]
        public void Test_SetRelatedObjectRaisesUpdatedEvent_Unreferenced()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            SingleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            bool updatedFired = false;
            ContactPersonTestBO boReceivedByEvent = null;
            ContactPersonTestBO currentContactPersonInEvent = null;
            Guid? organisationidInEvent = null;

            relationship.Updated += delegate(object sender, BOEventArgs<ContactPersonTestBO> e)
                                    {
                                        updatedFired = true;
                                        boReceivedByEvent = e.BusinessObject;
                                        organisationidInEvent = boReceivedByEvent.OrganisationID;
                                        currentContactPersonInEvent = organisationTestBO.ContactPerson;
                                    };

            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedFired);
            Assert.IsNull(boReceivedByEvent);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPersonTestBO;

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedFired);
            Assert.AreSame(contactPersonTestBO, boReceivedByEvent);
            Assert.AreSame(contactPersonTestBO, currentContactPersonInEvent);
            Assert.AreEqual(organisationTestBO.OrganisationID, organisationidInEvent);
        }

        private SingleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO)
        {
            RelationshipType relationshipType = RelationshipType.Association;
            return GetRelationship(organisationTestBO, relationshipType);
        }

        private SingleRelationship<ContactPersonTestBO> GetRelationship(OrganisationTestBO organisationTestBO, RelationshipType relationshipType)
        {
            SingleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return compositionRelationship;
        }
    }
}