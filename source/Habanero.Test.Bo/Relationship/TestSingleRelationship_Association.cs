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
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.Test.Structure;
using Habanero.Util;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestSingleRelationship_Association
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();

        }


        [Test]
        public void Test_SetChild_PersistedChild()
        {
            //An already persisted contactperson can be set as the contact person of an organisation
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new ContactPerson can be set as the contact person of an organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }

//        public void SetupDBConnection()
//        {
//            if (DatabaseConnection.CurrentConnection != null &&
//                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionMySql))
//            {
//                return;
//            }
//            DatabaseConnection.CurrentConnection =
//                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
//            DatabaseConnection.CurrentConnection.ConnectionString =
//                MyDBConnection.GetDatabaseConfig().GetConnectionString();
//            DatabaseConnection.CurrentConnection.GetConnection();
//
//            BORegistry.DataAccessor = new DataAccessorDB();
//        }
//
//        public void SetupDBOracleConnection()
//        {
//            if (DatabaseConnection.CurrentConnection != null &&
//                DatabaseConnection.CurrentConnection.GetType() == typeof(DatabaseConnectionOracle))
//            {
//                return;
//            }
//            DatabaseConnection.CurrentConnection =
//                new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
//            ConnectionStringOracleFactory oracleConnectionString = new ConnectionStringOracleFactory();
//            string connStr = oracleConnectionString.GetConnectionString("core1", "XE", "system", "system", "1521");
//            DatabaseConnection.CurrentConnection.ConnectionString = connStr;
//            DatabaseConnection.CurrentConnection.GetConnection();
//            BORegistry.DataAccessor = new DataAccessorDB();
//        }
        [Test]
        public  virtual void Test_SetParent_PersistedChild_FindLoadLookupListError()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection.CurrentConnection =
                                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetDatabaseConfig().GetConnectionString();
            DatabaseConnection.CurrentConnection.GetConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation(); 
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition -----------------------
            Assert.IsNotNull(organisation.OrganisationID);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParent_PersistedChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation(); 
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition -----------------------
            Assert.IsNotNull(organisation.OrganisationID);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParent_NewChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }


        [Test]
        public void Test_SetParentNull_PersistedChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);

        }


        [Test]
        public void Test_ResetParent_NewChild_SetToNull_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParentNull_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }
        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation(); 
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            IBusinessObjectCollection collection = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(organisation.ClassDef, null, "");
            //---------------Assert Precondition -----------------------
            Assert.IsNotNull(organisation.OrganisationID);
            Assert.Contains(organisation, collection);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParent_NewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }


        [Test]
        public void Test_SetParentNull_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);

        }


        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParentNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }

        [Test]
        public void Test_RemoveMethod()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);

            //---------------Assert Precondition----------------
            Assert.AreEqual(contactPerson.OrganisationID, organisation.OrganisationID);
            Assert.AreSame(organisation.ContactPerson, contactPerson);

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(null);

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
            Assert.IsNotNull(organisation.OrganisationID);
        }
        
        [Test]
        public void Test_DirtyIfHasNewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();


            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Organisation = organisationTestBO;
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_DirtyIfHasAddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = contactPerson;
            bool isDirty = organisationTestBO.Status.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_AddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisationTestBO.ContactPerson = contactPerson;

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
            
            //---------------Tear Down -------------------------          
        }
        
        [Test]
        public void Test_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection)dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }

        [Test]
        public void Test_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();
            myBO = organisationTestBO.ContactPerson;

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            ReflectionUtilities.SetPropertyValue(myBO.Status, "IsDeleted", true);
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            organisationTestBO.Save();
            ReflectionUtilities.SetPropertyValue(myBO.Status, "IsDeleted", true);
            //ReflectionUtilities.SetPropertyValue(myBO.Status, "IsDirty", true);

            //---------------Assert Preconditions---------------
            Assert.IsTrue(myBO.Status.IsDeleted);
            Assert.IsTrue(myBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.Contains(myBO, (ICollection)dirtyChildren);
        }

        [Test]
        public void Test_DirtyIfHasRemoveChildren()
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
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = null;
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Removed()
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
            organisationTestBO.ContactPerson = null;

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }


        [Test]
        public void Test_NotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);

            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(relationship.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        /// <summary>
        /// An Organisation is not considered dirty if it has a dirty contact person.
        /// </summary>
        [Test]
        public void Test_ParentNotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(organisation.Status.IsDirty);
        }

        /// <summary>
        /// Created child (ie a new object in the association relationship): saved when the parent is saved.
        /// </summary>
        [Test]
        public void Test_NewChild_SavesWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            organisation.ContactPerson = contactPerson;
            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
        }

        /// <summary>
        /// Marked for Delete child (ie an object in the association relationship that is marked for delete): deleted when the parent is saved
        /// </summary>
        [Test]
        public void Test_MarkedForDeleteChild_SavesWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            organisation.ContactPerson = contactPerson;
            contactPerson.Save();
            contactPerson.MarkForDelete();

            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPerson);
        }

        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_OnlyIDChanged()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsTrue(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Tear Down -------------------------          
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public virtual void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;
            contactPerson.Surname = TestUtil.GetRandomString();

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsTrue(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Tear Down -------------------------          
        }

        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_IDOnly()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;
            organisation.Save();
            organisation.ContactPerson = null;

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsTrue(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisation.Status.IsDirty);
        }

        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public virtual void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;
            organisation.Save();
            contactPerson.Surname = TestUtil.GetRandomString();
            organisation.ContactPerson = null;

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsTrue(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisation.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisation.Status.IsDirty);
        }



        [Test]
        public void Test_NewParentWithClassTableInheritance_NewChild()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());

            Habanero.Test.Structure.Person person = new Person();
            person.LastName = "bob";
            Vehicle vehicle = new Vehicle();
            vehicle.DateAssembled = DateTime.Now;
            person.VehiclesOwned.Add(vehicle);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            TransactionCommitter committer = (TransactionCommitter) BORegistry.DataAccessor.CreateTransactionCommitter();
            committer.AddBusinessObject(person);
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, committer.OriginalTransactions.Count, "There should only be two transactions: one insert for the person and one insert for the vehicle"); 

        }

        protected static SingleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO)
        {
            const RelationshipType relationshipType = RelationshipType.Association;
            SingleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            relationshipDef.OwningBOHasForeignKey = false;
            return compositionRelationship;
        }
    }



   
}
