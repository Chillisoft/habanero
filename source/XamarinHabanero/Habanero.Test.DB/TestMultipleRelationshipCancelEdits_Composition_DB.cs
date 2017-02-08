using Habanero.BO;
using Habanero.Base;
using Habanero.Test.BO;
using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestMultipleRelationshipCancelEdits_Composition_DB : TestMultipleRelationshipCancelEdits_Composition
    {
        protected override void SetupDataAccess()
        {
            TestUsingDatabase.SetupDBDataAccessor();
        }

        [Test]
        public void Test_Acceptance_CancelEditParent_BUGFIX_WhenChildAndParentAlreadyLoaded_WithParentRelationshipLoaded_AndChildrenRelationshipNotLoaded_ShouldNotThrowError()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            RelationshipCol relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship =
                GetRelationship(out organisationTestBO, out relationships, out cpCol);

            var contactPersonTestBO = CreateExistingChild(cpCol);

            FixtureEnvironment.ClearBusinessObjectManager();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            var organisation = contactPersonTestBO.Organisation;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => organisationTestBO.CancelEdits());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_Acceptance_CancelEditParent_BUGFIX_WhenChildAndParentAlreadyLoaded_WithParentRelationshipLoaded_BeforeChildrenRelationshipLoaded_ShouldNotThrowError()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO;
            RelationshipCol relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> aggregateRelationship =
                GetRelationship(out organisationTestBO, out relationships, out cpCol);

            var contactPersonTestBO = CreateExistingChild(cpCol);

            FixtureEnvironment.ClearBusinessObjectManager();
            organisationTestBO = ReloadBoFromDB(organisationTestBO);
            contactPersonTestBO = ReloadBoFromDB(contactPersonTestBO);
            var organisation = contactPersonTestBO.Organisation;
            var contactPeople = organisation.ContactPeople;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => organisationTestBO.CancelEdits());
            //---------------Test Result -----------------------
        }

        private static T ReloadBoFromDB<T>(T businessObject)
            where T : class, IBusinessObject, new()
        {
            var businessObjectLoader = BORegistry.DataAccessor.BusinessObjectLoader;
            return businessObjectLoader.GetBusinessObject<T>(businessObject.ID);
        }
    }
}