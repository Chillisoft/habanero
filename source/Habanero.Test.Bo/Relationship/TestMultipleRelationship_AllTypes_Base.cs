using Habanero.BO;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public abstract class TestMultipleRelationship_AllTypes_Base
    {
        [Test]
        public void ReverseRelationship_GetRelatedObject_BUGFIX_WhenChildAndParentAlreadyLoaded_WithChildrenRelationshipNotLoaded_ShouldNotLeaveChildAddedToParentRelationship()
        {
            //---------------Set up test pack-------------------
            var contactPersonTestBO = CreateSavedContactPersonTestBoAndOrganisation();
            var organisationTestBO = contactPersonTestBO.Organisation;

            BusinessObjectManager.Instance.ClearLoadedObjects();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var organisation = contactPersonTestBO.Organisation;
            //---------------Test Result -----------------------
            var contactPeople = organisation.ContactPeople;
            Assert.AreEqual(1, contactPeople.Count, "Should have one Contact Person");
            Assert.AreEqual(1, contactPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, contactPeople.AddedBusinessObjects.Count, "Added bos collection should be empty");
        }

        [Test]
        public void ReverseRelationship_GetRelatedObject_BUGFIX_WhenChildAndParentAlreadyLoaded_WithChildrenRelationshipLoaded_ShouldNotLeaveChildAddedToParentRelationship()
        {
            //---------------Set up test pack-------------------
            var contactPersonTestBO = CreateSavedContactPersonTestBoAndOrganisation();
            var organisationTestBO = contactPersonTestBO.Organisation;

            BusinessObjectManager.Instance.ClearLoadedObjects();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            var loadedCollection = organisationTestBO.ContactPeople;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var organisation = contactPersonTestBO.Organisation;
            //---------------Test Result -----------------------
            var contactPeople = organisation.ContactPeople;
            Assert.AreEqual(1, contactPeople.Count, "Should have one Contact Person");
            Assert.AreEqual(1, contactPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, contactPeople.AddedBusinessObjects.Count, "Added bos collection should be empty");
        }

        [Test]
        [Ignore("Known issue with GetRelatedObject")] //TODO Mark 29 May 2013: Ignored Test - Known issue with GetRelatedObject
        public void ReverseRelationship_GetRelatedObject_BUGFIX_WhenChildAndParentAlreadyLoaded_AndParentSwitchedToUnloadedParent_ShouldAddChildToParentRelationship()
        {
            //---------------Set up test pack-------------------
            var contactPersonTestBO = CreateSavedContactPersonTestBoAndOrganisation();
            var newOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            var organisationTestBO = contactPersonTestBO.Organisation;

            BusinessObjectManager.Instance.ClearLoadedObjects();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            contactPersonTestBO.OrganisationID = newOrganisationTestBO.OrganisationID;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var organisation = contactPersonTestBO.Organisation;
            //---------------Test Result -----------------------
            var contactPeople = organisation.ContactPeople;
            Assert.AreEqual(1, contactPeople.Count, "Should have one Contact Person");
            Assert.AreEqual(0, contactPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, contactPeople.AddedBusinessObjects.Count);
        }

        [Test]
        public void ReverseRelationship_GetRelatedObject_BUGFIX_WhenChildAndParentAlreadyLoaded_AndParentSwitchedToLoadedParent_ShouldAddChildToParentRelationship()
        {
            //---------------Set up test pack-------------------
            var contactPersonTestBO = CreateSavedContactPersonTestBoAndOrganisation();
            var newOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            var organisationTestBO = contactPersonTestBO.Organisation;

            BusinessObjectManager.Instance.ClearLoadedObjects();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            newOrganisationTestBO = ReloadBoFromDB(newOrganisationTestBO);
            contactPersonTestBO.OrganisationID = newOrganisationTestBO.OrganisationID;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var organisation = contactPersonTestBO.Organisation;
            //---------------Test Result -----------------------
            var contactPeople = organisation.ContactPeople;
            Assert.AreEqual(1, contactPeople.Count, "Should have one Contact Person");
            Assert.AreEqual(0, contactPeople.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, contactPeople.AddedBusinessObjects.Count);
        }


        private static ContactPersonTestBO CreateSavedContactPersonTestBoAndOrganisation()
        {
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            organisationTestBO.Save();
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        private static T ReloadBoFromDB<T>(T businessObject)
            where T : class, IBusinessObject, new()
        {
            var businessObjectLoader = BORegistry.DataAccessor.BusinessObjectLoader;
            return businessObjectLoader.GetBusinessObject<T>(businessObject.ID);
        }
    }
}