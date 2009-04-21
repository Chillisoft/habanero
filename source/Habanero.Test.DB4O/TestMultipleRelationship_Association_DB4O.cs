using System.IO;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB4O;
using Habanero.Test.BO;
using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestMultipleRelationship_Association_DB4O : TestMultipleRelationship_Association
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
        [Test]
        public override void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            organisationTestBO.ContactPeople.Add(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

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
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.Surname = TestUtil.GetRandomString();
            organisationTestBO.ContactPeople.Remove(contactPerson);

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }

        [Test]
        public override void Test_AddDirtyChildrenToTransactionCommitter_AddedChild()
        {
            // this test is ignored because the effect is tested above, and we can't access the internal methods
            // to check the internals.
        }

    }
}