using System.IO;
using Db4objects.Db4o;
using Habanero.BO;
using Habanero.DB4O;
using Habanero.Test.BO;
using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestSingleRelationship_Association_DB4O : TestSingleRelationship_Association
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            const string db4oFileStore = "DataStore.db4o";
            if (File.Exists(db4oFileStore)) File.Delete(db4oFileStore);
            DB4ORegistry.DB = Db4oFactory.OpenFile(db4oFileStore);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// Note: for DB4O it was decided to make the entire object persist as you cannot simply persist a property or two easily.
        /// </summary>
        /// 
        [Test]
        public override void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
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
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Tear Down -------------------------          
        }

        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// Note: for DB4O it was decided to make the entire object persist as you cannot simply persist a property or two easily.
        /// </summary>
        [Test]
        public override void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
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
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisation.Status.IsDirty);
        }

        [Test]
        public override void Test_SetParent_PersistedChild_FindLoadLookupListError()
        {
            // note: this test is not done for DB4O as a databaselookuplist makes no sense in this case (to test at least).
        }
    }
}