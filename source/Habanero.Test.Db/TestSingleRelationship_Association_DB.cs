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
using Habanero.BO;
using Habanero.Test.BO;
using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSingleRelationship_Association_DB : TestSingleRelationship_Association
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }



        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_DB()
        {
            //---------------Set up test pack-------------------
            TestUsingDatabase.SetupDBDataAccessor();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;
            contactPerson.Surname = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------
            organisation.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedContactPerson = Broker.GetBusinessObject<ContactPersonTestBO>(contactPerson.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.OrganisationID, loadedContactPerson.OrganisationID);

        }




        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_DB()
        {
            //---------------Set up test pack-------------------
            TestUsingDatabase.SetupDBDataAccessor();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            organisation.ContactPerson = contactPerson;
            organisation.Save();

            contactPerson.Surname = TestUtil.GetRandomString();
            organisation.ContactPerson = null;

            //---------------Execute Test ----------------------
            organisation.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            ContactPersonTestBO loadedContactPerson = Broker.GetBusinessObject<ContactPersonTestBO>(contactPerson.ID);

            //---------------Test Result -----------------------
            Assert.IsNull(loadedContactPerson.OrganisationID);

        }

    }
}