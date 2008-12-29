using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;
using Relationship=Habanero.BO.Relationship;
using System;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSingleRelationship_Composition //:TestBase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
         
         
          
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

       [Test]
        public void Test_SetChild_PersistedChild()
        {
            //An already persi`sted Brain cannot be set as the brain of a person.
            //---------------Set up test pack-------------------
           OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
           OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
           SingleRelationship compositionRelationship = (SingleRelationship) GetCompositionRelationship(organisation);
           ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
           ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            try
            {
                compositionRelationship.SetRelatedObject(contactPerson);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
       }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new Brain can be set as the brain of a person.  This is allowed in Habanero for flexibility, but
            // it is rather recommended that the Person creates the Brain.
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship compositionRelationship = (SingleRelationship)GetCompositionRelationship(organisation);
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            compositionRelationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, compositionRelationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisation.Relationships["ContactPerson"];
            compositionRelationship.RelationshipDef.AddChildAction = AddChildAction.Prevent;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = organisation;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_SetParent_NewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            Relationship compositionRelationship = (Relationship)organisation.Relationships["ContactPerson"];
            compositionRelationship.RelationshipDef.AddChildAction = AddChildAction.Prevent;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------

        }

        private static Relationship GetCompositionRelationship(OrganisationTestBO organisation)
        {
            RelKeyDef def = new RelKeyDef();
            def.Add(new RelPropDef(organisation.Props["OrganisationID"].PropDef, "OrganisationID"));
            SingleRelationshipDef relationshipDef = 
                new SingleRelationshipDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(), 
                    TestUtil.CreateRandomString(), def, false, DeleteParentAction.DeleteRelated, 
                    RemoveChildAction.Prevent, AddChildAction.Prevent);
            return relationshipDef.CreateRelationship(organisation, organisation.Props);
        }


    }
}