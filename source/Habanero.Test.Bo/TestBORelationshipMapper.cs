using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBORelationshipMapper
    {

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            string relationshipName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Test Result -----------------------
            Assert.AreEqual(relationshipName, boRelationshipMapper.RelationshipName);
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }       
        
        [Test]
        public void Test_Construct_WhenNullRelationshipName_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new BORelationshipMapper(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("relationshipName", ex.ParamName);
            }
        }    

        [Test]
        public void Test_Construct_WhenStringEmptyRelationshipName_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new BORelationshipMapper("");
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("relationshipName", ex.ParamName);
            }
        }

        [Test]
        public void Test_BusinessObject_GetAndSet()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
        }

        [Test]
        public void Test_BusinessObject_WhenSetWithBOHavingSpecifiedRelationship_ShouldReturnCorrectRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Relationships[relationshipName], boRelationshipMapper.Relationship);
        }        
        
        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldReturnNullRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boRelationshipMapper.BusinessObject);
            Assert.IsNotNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }


        [Test]
        public void Test_BusinessObject_WhenSetWithBONotHavingSpecifiedRelationship_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "SomeNonExistentRelationship";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            try
            {
                boRelationshipMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + relationshipName + "' on '"
                     + contactPersonTestBO.ClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + relationshipName + "' does not exist on the BusinessObject '"
                     + contactPersonTestBO.ClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boRelationshipMapper.BusinessObject);
                Assert.IsNull(boRelationshipMapper.Relationship);
            }
        }   


        [Test]
        public void Test_BusinessObject_WhenSetWithBOHavingSpecifiedRelationship_ShouldFireRelationshipChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            bool eventFired = false;
            boRelationshipMapper.RelationshipChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO,boRelationshipMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Relationships[relationshipName],boRelationshipMapper.Relationship);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldFireRelationshipChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            bool eventFired = false;
            boRelationshipMapper.RelationshipChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boRelationshipMapper.BusinessObject);
            Assert.IsNotNull(boRelationshipMapper.Relationship);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_BusinessObject_WhenSetWithBONotHavingSpecifiedRelationship_ShouldNotFireRelationshipChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "SomeNonExistentRelationship";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            bool eventFired = false;
            boRelationshipMapper.RelationshipChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            try
            {
                boRelationshipMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw an Exception");
            }
            //---------------Test Result -----------------------
            catch (Exception)
            {
                Assert.IsNull(boRelationshipMapper.BusinessObject);
                Assert.IsNull(boRelationshipMapper.Relationship);
                Assert.IsFalse(eventFired);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingRelationshipOnRelatedBO_ShouldReturnRelatedBOsRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Organisation.Relationships[innerRelationshipName], boRelationshipMapper.Relationship);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingRelationshipOnRelatedBO_AndRelatedBoIsNull_ShouldReturnNullRelationship()
        {
            //---------------Set up test pack-------------------
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO{Organisation = new OrganisationTestBO()};
            ContactPersonTestBO newContactPersonTestBO = new ContactPersonTestBO();
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.IsNotNull(boRelationshipMapper.Relationship);
            Assert.IsNotNull(contactPersonTestBO.Organisation);
            Assert.IsNull(newContactPersonTestBO.Organisation);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = newContactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newContactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_HavingRelationshipOnRelatedBO_ShouldReturnNullRelationship()
        {
            //---------------Set up test pack-------------------
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO { Organisation = new OrganisationTestBO() };
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boRelationshipMapper.BusinessObject);
            Assert.IsNotNull(boRelationshipMapper.Relationship);
            Assert.IsNotNull(contactPersonTestBO.Organisation);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingNonExistingRelationshipOnRelatedBO_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef organisationClassDef = ClassDef.Get<OrganisationTestBO>();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerRelationshipName = "NonExistingRelationship";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            try
            {
                boRelationshipMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + innerRelationshipName + "' on '"
                     + organisationClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + innerRelationshipName + "' does not exist on the BusinessObject '"
                     + organisationClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boRelationshipMapper.BusinessObject);
                Assert.IsNull(boRelationshipMapper.Relationship);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingRelationshipOnRelatedBO_ShouldFireRelationshipChangeEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            bool eventFired = false;
            boRelationshipMapper.RelationshipChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Organisation.Relationships[innerRelationshipName], boRelationshipMapper.Relationship);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToDifferentBo_HavingRelationshipOnRelatedBO_ShouldChangeRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            OrganisationTestBO newOrganisationTestBO = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(oldOrganisationTestBO.Relationships[innerRelationshipName], boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = newOrganisationTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(newOrganisationTestBO.Relationships[innerRelationshipName], boRelationshipMapper.Relationship);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToDifferentBo_HavingRelationshipOnRelatedBO_ShouldFireRelationshipChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            OrganisationTestBO newOrganisationTestBO = new OrganisationTestBO();
            bool eventFired = false;
            boRelationshipMapper.RelationshipChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = newOrganisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToNull_HavingRelationshipOnRelatedBO_ShouldChangeRelationshipToNull()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.AreSame(oldOrganisationTestBO.Relationships[innerRelationshipName], boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = null;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }
        
        // When the child relationship is not found. i.e. "NonExisting.Addresses"
        [Test]
        public void Test_BusinessObject_WhenSet_HavingNonExistingChildRelationshipForRelatedBo_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef contactPersonClassDef = ClassDef.Get<ContactPersonTestBO>();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerRelationshipName = "Addresses";
            const string outerRelationshipName = "NonExistingRelationship";
            const string relationshipName = outerRelationshipName + "." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            try
            {
                boRelationshipMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' does not exist on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boRelationshipMapper.BusinessObject);
                Assert.IsNull(boRelationshipMapper.Relationship);
            }
        }

        // When the child relationship is a multiple relationship, then an error should be thrown.
        [Test]
        public void Test_BusinessObject_WhenSet_HavingExistingNonSingleRelationshipOnRelatedBO_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO.LoadDefaultClassDef();
            ClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string innerRelationshipName = "ContactPersonTestBO";
            const string outerRelationshipName = "Addresses";
            const string relationshipName = outerRelationshipName + "." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            try
            {
                boRelationshipMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed.", ex.DeveloperMessage);
                Assert.IsNull(boRelationshipMapper.BusinessObject);
                Assert.IsNull(boRelationshipMapper.Relationship);
            }
        }

    }
}
