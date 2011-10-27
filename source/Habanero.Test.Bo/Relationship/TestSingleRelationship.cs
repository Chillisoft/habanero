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
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
        }

        [Test]
        public void TestSetRelatedObject()
        {
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            IClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();

            ContactPersonTestBO alternatecontactPerson = new ContactPersonTestBO();
            alternatecontactPerson.Surname = TestUtil.GetRandomString();
            alternatecontactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = null;
            alternatecontactPerson.OrganisationID = organisationTestBO.OrganisationID;
            
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, alternatecontactPerson.Organisation);
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            //---------------Assert preconditions --------------
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetbyObject_InBOManager_AndSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetbyObject_NotInBOManager_AndSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();

            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetbyObject_NotInBOManager_NotSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();

            //---------------Assert preconditions---------------
            Assert.IsTrue(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetCPbyObject_InBOManager_AndSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPerson;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }
        [Test]
        public void Test_SetCPbyObject_InBOManager_AndSaved_NoReverseRelationshipDefined()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            OrganisationTestBO.LoadDefaultClassDef_WithContactPersonRelationship_NoReverseRelationship();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPerson;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPerson.OrganisationID);
        }

        [Test]
        public void Test_SetOrgNullbyObject_InBOManager_AndSaved_NoReverseRelationshipDefined_OwningBOHasForeignKey() 
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithNoContactPersonRelationship();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Organisation = organisationTestBO;
            //---------------Assert preconditions---------------
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.ContactPersonID);
            Assert.IsNotNull(organisationTestBO.OrganisationID);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.ContactPersonID);
            Assert.IsNotNull(organisationTestBO.OrganisationID);
        }

        [Test]
        public void Test_SetOrgbyObject_InBOManager_AndSaved_NoReverseRelationshipDefined_OwningBOHasForeignKey() 
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithNoContactPersonRelationship();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.ContactPersonID);
            Assert.IsNotNull(organisationTestBO.OrganisationID);
        }

        [Test]
        public void Test_SetCPToNullbyObject_InBOManager_AndSaved_NoReverseRelationshipDefined()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDef_NoOrganisationRelationship();
            OrganisationTestBO.LoadDefaultClassDef_WithContactPersonRelationship_NoReverseRelationship();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            organisationTestBO.ContactPerson = contactPerson;
            //---------------Assert preconditions---------------
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPerson.OrganisationID);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = null;
            //---------------Test Result -----------------------
            Assert.IsNull(organisationTestBO.ContactPerson);
            Assert.IsNull(contactPerson.OrganisationID);
        }

        [Test]
        public void Test_SetCPbyObject_NotInBOManager_AndSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();

            //---------------Assert preconditions---------------
            Assert.IsFalse(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPerson;
            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }

        [Test]
        public void Test_SetbyCPObject_NotInBOManager_NotSaved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            //---------------Assert preconditions---------------
            Assert.IsTrue(organisationTestBO.Status.IsNew);
            Assert.IsTrue(contactPerson.Status.IsNew);
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPerson;
            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPerson.OrganisationID);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.AreSame(organisationTestBO, contactPerson.Organisation);
            Assert.AreSame(contactPerson, organisationTestBO.ContactPerson);
        }
        [Test]
        public void Test_SetParentNull_NewChild_BotRelationshipSetUpAsOwning()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            SingleRelationship<OrganisationTestBO> relationshipOrganisation = GetAssociationRelationshipOrganisation(contactPerson);
            relationshipOrganisation.OwningBOHasForeignKey = true;

            SingleRelationship<ContactPersonTestBO> relationshipContactPerson = GetAssociationRelationship(organisation);
            relationshipContactPerson.OwningBOHasForeignKey = true;
            //---------------Assert Preconditon-----------------
            Assert.IsNull(organisation.ContactPerson);
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = organisation;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The corresponding single (one to one) relationships Organisation ", ex.Message);
                StringAssert.Contains("cannot both be configured as having the foreign key", ex.Message);
            }
        }
        [Test]
        public void Test_SetByID_InBOManager_UnsavedOrganisation_NoReverseRelationship_HasOwningForeighKeyFalse()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
                    {
                        Surname = TestUtil.GetRandomString(),
                        FirstName = TestUtil.GetRandomString()
                    };
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            OrganisationTestBO returnedOrg = contactPerson.Organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, returnedOrg);
        }

        [Test]
        public void Test_SetByID_NotInBOManager_SavedOrganisation_NoReverseRelationship_HasOwningForeighKeyFalse()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
            {
                Surname = TestUtil.GetRandomString(),
                FirstName = TestUtil.GetRandomString()
            };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(contactPerson));
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            OrganisationTestBO returnedOrg = contactPerson.Organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, returnedOrg);
        }
        [Test]
        public void Test_SetByID_ToNull_NotInBOManager_SavedOrganisation()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
            {
                Surname = TestUtil.GetRandomString(),
                FirstName = TestUtil.GetRandomString()
            };
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            contactPerson.Save();
            OrganisationTestBO origOrganisation = contactPerson.Organisation;
            //---------------Assert Preconditions --------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(contactPerson));
            Assert.AreSame(organisationTestBO, origOrganisation);
            Assert.IsNotNull(origOrganisation.ContactPerson);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = null;
            OrganisationTestBO returnedOrg = contactPerson.Organisation;
            ContactPersonTestBO returnedContactP = origOrganisation.ContactPerson;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(returnedOrg);
            Assert.IsNull(returnedContactP);
        }
        [Test]
        public void Test_SetByID_ToAnotherOrgID_NotInBOManager_SavedOrganisation()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO organisationTestBO2 = OrganisationTestBO.CreateSavedOrganisation();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
            {
                Surname = TestUtil.GetRandomString(),
                FirstName = TestUtil.GetRandomString()
            };
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            OrganisationTestBO origOrganisation = contactPerson.Organisation;
            //---------------Assert Preconditions --------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(contactPerson));
            Assert.AreSame(organisationTestBO, origOrganisation);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO2.OrganisationID;
            OrganisationTestBO returnedOrg = contactPerson.Organisation;
            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPerson.OrganisationID);
            Assert.AreEqual(organisationTestBO2.OrganisationID,  returnedOrg.OrganisationID);
        }

        [Test]
        public void Test_SetByID_ToAnotherOrgID_InBOManager_SavedOrganisation()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO organisationTestBO2 = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
                        {
                            Surname = TestUtil.GetRandomString(),
                            FirstName = TestUtil.GetRandomString(),
                            OrganisationID = organisationTestBO.OrganisationID
                        };
            //---------------Assert Preconditions --------------
            Assert.AreNotSame(contactPerson, organisationTestBO2.ContactPerson);
            Assert.AreNotSame(contactPerson.Organisation, organisationTestBO2);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO2.OrganisationID;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson.Organisation, organisationTestBO2);
            Assert.AreSame(contactPerson, organisationTestBO2.ContactPerson);
        }
        [Test]
        public void Test_SetByID_ToAnotherOrgID_InBOManager_UnSavedOrganisation()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            OrganisationTestBO organisationTestBO2 = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
                        {
                            Surname = TestUtil.GetRandomString(),
                            FirstName = TestUtil.GetRandomString(),
                            OrganisationID = organisationTestBO.OrganisationID
                        };
            //---------------Assert Preconditions --------------
            Assert.AreNotSame(contactPerson, organisationTestBO2.ContactPerson);
            Assert.AreNotSame(contactPerson.Organisation, organisationTestBO2);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO2.OrganisationID;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson.Organisation, organisationTestBO2);
            Assert.AreSame(contactPerson, organisationTestBO2.ContactPerson);
        }

        [Test]
        public void Test_SetByID_ToAnotherOrgID_InBOManager_UnSavedOrganisation_ReverseRelMultiple()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            OrganisationTestBO organisationTestBO2 = OrganisationTestBO.CreateUnsavedOrganisation();
            MultipleRelationship<ContactPersonTestBO> relationship = GetMultipleRelationship(organisationTestBO, RelationshipType.Association);
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
                        {
                            Surname = TestUtil.GetRandomString(),
                            FirstName = TestUtil.GetRandomString(),
                            OrganisationID = organisationTestBO.OrganisationID
                        };
            //---------------Assert Preconditions --------------
            Assert.AreNotSame(contactPerson.Organisation, organisationTestBO2);
            Assert.AreEqual(0, organisationTestBO2.ContactPeople.Count);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO2.OrganisationID;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson.Organisation, organisationTestBO2);
            Assert.AreEqual(1, organisationTestBO2.ContactPeople.Count);
        }

        [Test]
        public void Test_SetByID_InBOManager_UnsavedOrganisation_NoReverseRelationship()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = true;
            ContactPersonTestBO contactPerson = new ContactPersonTestBO
                        {
                            Surname = TestUtil.GetRandomString(),
                            FirstName = TestUtil.GetRandomString()
                        };
            SetReverseRelationshipOwningBoHasFKFalse(contactPerson);
            //---------------Execute Test ----------------------
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            OrganisationTestBO returnedOrg = contactPerson.Organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, returnedOrg);
        }

        private void SetReverseRelationshipOwningBoHasFKFalse(ContactPersonTestBO contactPerson)
        {
            SingleRelationship<OrganisationTestBO> reverseRelationship =
                contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            RelationshipDef reverseRelationshipDef = (RelationshipDef)reverseRelationship.RelationshipDef;
            reverseRelationshipDef.OwningBOHasForeignKey = false;
        }

        [Test]
        public void Test_IsRemoved()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                relationship.SetRelatedObject(contactPerson);
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
        public void Test_ErrorIfBothOwningBOHasForeignKey_Remove()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = true;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.OrganisationID = organisation.OrganisationID;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                relationship.SetRelatedObject(null);
                Assert.Fail("An error should have occurred as corresponding single relationships are not configured correctly (one should have the OwningBOHasForeignKey property set to false)");
            }
            catch (HabaneroDeveloperException ex)
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
            SingleRelationship<OrganisationTestBO> organisationRelationship =
                contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            organisationRelationship.OwningBOHasForeignKey = true;
            SingleRelationship<ContactPersonTestBO> contactPersonRelationship = organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            contactPersonRelationship.OwningBOHasForeignKey = false;
            bool updatedFired = false;
            OrganisationTestBO boReceivedByEvent = null;
            OrganisationTestBO currentOrganisationInEvent = null;
            Guid? organisationidInEvent = null;

            organisationRelationship.Updated += delegate(object sender, BOEventArgs<OrganisationTestBO> e)
                                                    {
                                                        updatedFired = true;
                                                        boReceivedByEvent = e.BusinessObject;
                                                        organisationidInEvent = contactPersonTestBO.OrganisationID;
                                                        currentOrganisationInEvent = contactPersonTestBO.Organisation;
                                                    };

            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedFired);
            Assert.IsNull(boReceivedByEvent);
            Assert.IsTrue(organisationRelationship.OwningBOHasForeignKey);
            Assert.IsFalse(contactPersonRelationship.OwningBOHasForeignKey);
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
            SingleRelationship<ContactPersonTestBO> contactPersonRelationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            bool updatedFired = false;
            ContactPersonTestBO boReceivedByEvent = null;
            ContactPersonTestBO currentContactPersonInEvent = null;
            Guid? organisationidInEvent = null;

            contactPersonRelationship.Updated += delegate(object sender, BOEventArgs<ContactPersonTestBO> e)
                                                     {
                                                         updatedFired = true;
                                                         boReceivedByEvent = e.BusinessObject;
                                                         organisationidInEvent = boReceivedByEvent.OrganisationID;
                                                         currentContactPersonInEvent = organisationTestBO.ContactPerson;
                                                     };

            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedFired);
            Assert.IsNull(boReceivedByEvent);
            Assert.IsFalse(contactPersonRelationship.OwningBOHasForeignKey);
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPersonTestBO;

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedFired);
            Assert.AreSame(contactPersonTestBO, boReceivedByEvent);
            Assert.AreSame(contactPersonTestBO, currentContactPersonInEvent);
            Assert.AreEqual(organisationTestBO.OrganisationID, organisationidInEvent);
        }

        [Test]
        public void Test_SetRelatedObjectRaisesReverseRelationshipUpdatedEvent_Unreferenced()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            SingleRelationship<OrganisationTestBO> organisationRelationship =
                contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            bool updatedFired = false;
            OrganisationTestBO boReceivedByEvent = null;
            Guid? organisationidInEvent = null;

            organisationRelationship.Updated += delegate(object sender, BOEventArgs<OrganisationTestBO> e)
                                                    {
                                                        updatedFired = true;
                                                        boReceivedByEvent = e.BusinessObject;
                                                        organisationidInEvent = contactPersonTestBO.OrganisationID;
                                                    };

            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedFired);
            Assert.IsNull(boReceivedByEvent);
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPersonTestBO;

            //---------------Test Result -----------------------
            Assert.IsTrue(updatedFired);
            Assert.AreSame(organisationTestBO, boReceivedByEvent);
            Assert.AreEqual(organisationTestBO.OrganisationID, organisationidInEvent);
        }
        [Test]
        public void Test_SetRelatedObjectNoReverseRelationship_Referenced()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");

            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = organisationTestBO;
            contactPersonTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreEqual(organisationTestBO.OrganisationID,  contactPersonTestBO.OrganisationID);
        }
        [Test]
        public void Test_UpdateRelatedObjectNoReverseRelationship_Referenced()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();

            contactPersonTestBO.Organisation = organisationTestBO;
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            contactPersonTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreNotEqual(organisationTestBO.OrganisationID,  contactPersonTestBO.OrganisationID);
        }
        [Test]
        public void Test_UpdateWIDRelatedObjectNoReverseRelationship_Referenced()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef_NoRelationships();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();

            contactPersonTestBO.Organisation = organisationTestBO;
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            contactPersonTestBO.OrganisationID = new OrganisationTestBO().OrganisationID;
            contactPersonTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreNotEqual(organisationTestBO.OrganisationID,  contactPersonTestBO.OrganisationID);
        }
        [Test]
        public void Test_SetRelatedObjectNoReverseRelationship_UnReferenced()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson").OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPersonTestBO;
            organisationTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPersonTestBO.OrganisationID);
        }
        [Test]
        public void Test_UpdateRelatedObjectNoReverseRelationship_UnReferenced()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            organisationTestBO.ContactPerson = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson").OwningBOHasForeignKey);
            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPersonTestBO;
            organisationTestBO.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPersonTestBO.OrganisationID);
        }

        [Test]
        public void Test_UpdateRelatedObject_ShouldRaiseEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
            ISingleRelationship relationship = (ISingleRelationship)contactPersonTestBO.Relationships["Organisation"];
            bool eventCalled = false;
            relationship.RelatedBusinessObjectChanged += delegate { eventCalled = true; };
            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation").OwningBOHasForeignKey);
            Assert.IsFalse(eventCalled);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = organisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventCalled);
            Assert.IsTrue(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
            Assert.AreEqual(organisationTestBO.OrganisationID, contactPersonTestBO.OrganisationID);
        }




//        [Test]
//        public void Test_UpdateUsingIDRelatedObjectNoReverseRelationship_UnReferenced()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef.ClassDefs.Clear();
//            OrganisationTestBO.LoadDefaultClassDef_SingleRel_NoReverseRelationship();
//            ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
//            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
//            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateUnsavedContactPerson();
//            organisationTestBO.ContactPerson = contactPersonTestBO;
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson").OwningBOHasForeignKey);
//            //---------------Execute Test ----------------------
//            organisationTestBO.Props["] = contactPersonTestBO;
//            organisationTestBO.Save();
//            //---------------Test Result -----------------------
//            Assert.IsFalse(contactPersonTestBO.Props["OrganisationID"].IsDirty, "This prop should be updated");
//            Assert.AreEqual(organisationTestBO.OrganisationID, contactPersonTestBO.OrganisationID);
//        }
        private static SingleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO)
        {
            const RelationshipType relationshipType = RelationshipType.Association;
            return GetRelationship(organisationTestBO, relationshipType);
        }
        private static SingleRelationship<OrganisationTestBO> GetAssociationRelationshipOrganisation(ContactPersonTestBO contactPersonTestBo)
        {
            const RelationshipType relationshipType = RelationshipType.Association;
            SingleRelationship<OrganisationTestBO> relationship =
                contactPersonTestBo.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return relationship;
        }

        private static SingleRelationship<ContactPersonTestBO> GetRelationship(OrganisationTestBO organisationTestBO, RelationshipType relationshipType)
        {
            SingleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return relationship;
        }
        private static MultipleRelationship<ContactPersonTestBO> GetMultipleRelationship(OrganisationTestBO organisationTestBO, RelationshipType relationshipType)
        {
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return relationship;
        }
    }

    [TestFixture]
    public class TestSingleRelationship_CompositePrimaryKeyContainsCompositeForeignKey : TestSingleRelationship
    {
        [SetUp]
        public override void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleCompositeReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
        }
    }

    [TestFixture]
    public class TestSingleRelationship_TwoObjectsHaveIdenticallyNamedPrimaryKeyPropertyName
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            BOWithIntID.LoadClassDefWithIntID_RelationshipToSelf();
            BOWithIntID_DifferentType.LoadClassDefWithIntID();
        }
        [Test]
        public void Test_TwoObjectTypesWithTheSameIDField_EditedToHaveTheSamevalue_UseOneInRelationships()
        {
            //--------------- Set up test pack ------------------
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = id };
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            SingleRelationship<BOWithIntID> childRelationship = (SingleRelationship<BOWithIntID>) boWithIntID.Relationships["MyChildBoWithInt"];
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            Assert.IsNull(childRelationship.GetRelatedObject());
            Assert.AreEqual(boWithIntID.IntID, boWithIntID_DifferentType.IntID);
            //--------------- Execute Test ----------------------
            BOWithIntID childBOWithID = new BOWithIntID { IntID = 4 };
            childRelationship.SetRelatedObject(childBOWithID);
            //--------------- Test Result -----------------------
            SingleRelationship<BOWithIntID> parentRelationship = (SingleRelationship<BOWithIntID>) childBOWithID.Relationships["MyParentBOWithInt"];
            Assert.AreSame(childBOWithID ,childRelationship.GetRelatedObject());
            Assert.AreSame(boWithIntID, parentRelationship.GetRelatedObject());
            Assert.AreEqual(childRelationship.GetReverseRelationship(childBOWithID), parentRelationship);
        }
        [Test]
        public void Test_TwoObjectTypesWithTheSameIDField_EdidtedToHaveTheSamevalue_ResetOneInRelationships()
        {
            //--------------- Set up test pack ------------------
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = id };
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            ISingleRelationship childRelationship = (SingleRelationship<BOWithIntID>) boWithIntID.Relationships["MyChildBoWithInt"];
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            Assert.IsNull(childRelationship.GetRelatedObject());
            Assert.AreEqual(boWithIntID.IntID, boWithIntID_DifferentType.IntID);
            //--------------- Execute Test ----------------------
            BOWithIntID childBOWithID = new BOWithIntID { IntID = 4 };
            SingleRelationship<BOWithIntID> parentRelationship = (SingleRelationship<BOWithIntID>) childBOWithID.Relationships["MyParentBOWithInt"];
            parentRelationship.SetRelatedObject(boWithIntID);
            //--------------- Test Result -----------------------
            Assert.AreSame(childBOWithID ,childRelationship.GetRelatedObject());
            Assert.AreSame(boWithIntID, parentRelationship.GetRelatedObject());
            Assert.AreEqual(childRelationship.GetReverseRelationship(childBOWithID), parentRelationship);
        }
        [Test]
        public void Test_TwoObjectTypesWithTheSameIDField_EdidtedToHaveTheSamevalue_ResetOneInRelationships_WDuplicateID()
        {
            //--------------- Set up test pack ------------------
            const int id = 3;
            BOWithIntID boWithIntID = new BOWithIntID { IntID = id };
            BOWithIntID_DifferentType boWithIntID_DifferentType = new BOWithIntID_DifferentType { IntID = id };
            boWithIntID_DifferentType.IntID = boWithIntID.IntID;
            SingleRelationship<BOWithIntID> parentRelationship = (SingleRelationship<BOWithIntID>)boWithIntID.Relationships["MyParentBOWithInt"];
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(2, BORegistry.BusinessObjectManager.Count);
            Assert.IsNull(parentRelationship.GetRelatedObject());
            Assert.AreEqual(boWithIntID.IntID, boWithIntID_DifferentType.IntID);
            //--------------- Execute Test ----------------------
            BOWithIntID childBOWithID = new BOWithIntID { IntID = 4 };
            SingleRelationship<BOWithIntID> childRelationship = (SingleRelationship<BOWithIntID>)childBOWithID.Relationships["MyChildBoWithInt"];
            childRelationship.SetRelatedObject(boWithIntID);
            //--------------- Test Result -----------------------
            Assert.AreSame(childBOWithID ,parentRelationship.GetRelatedObject());
            Assert.AreSame(boWithIntID, childRelationship.GetRelatedObject());
            Assert.AreEqual(parentRelationship.GetReverseRelationship(childBOWithID), childRelationship);
        }
    }

//    [TestFixture]
//    public class TestSingleRelationship_CompositePrimaryKeyContainsCompositeForeignKey : TestSingleRelationship
//    {
//        [SetUp]
//        public override void SetupTest()
//        {
//            ClassDef.ClassDefs.Clear();
//            BORegistry.DataAccessor = new DataAccessorInMemory();
//            BORegistry.BusinessObjectManager.ClearLoadedObjects();
//            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleCompositeReverse();
//            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
//        }
//    }
}