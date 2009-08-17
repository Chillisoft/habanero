using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropertyMapper
    {

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            AddressTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
        }

        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Test Result -----------------------
            Assert.AreEqual(propertyName, boPropertyMapper.PropertyName);
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
        }

        [Test]
        public void Test_Construct_WhenNullPropertyName_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new BOPropertyMapper(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propertyName", ex.ParamName);
            }
        }

        [Test]
        public void Test_Construct_WhenStringEmptyPropertyName_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new BOPropertyMapper("");
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propertyName", ex.ParamName);
            }
        }

        [Test]
        public void Test_BusinessObject_GetAndSet()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "FirstName";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
        }

        [Test]
        public void Test_BusinessObject_WhenSetWithBOHavingSpecifiedProperty_ShouldReturnCorrectProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "FirstName";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Props[propertyName], boPropertyMapper.Property);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldReturnNullProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "FirstName";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boPropertyMapper.BusinessObject);
            Assert.IsNotNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
        }


        [Test]
        public void Test_BusinessObject_WhenSetWithBONotHavingSpecifiedProperty_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "SomeNonExistentProperty";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The property '" + propertyName + "' on '"
                     + contactPersonTestBO.ClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The property '" + propertyName + "' does not exist on the BusinessObject '"
                     + contactPersonTestBO.ClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
            }
        }


        [Test]
        public void Test_BusinessObject_WhenSetWithBOHavingSpecifiedProperty_ShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "FirstName";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            bool eventFired = false;
            boPropertyMapper.PropertyChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Props[propertyName], boPropertyMapper.Property);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "FirstName";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            bool eventFired = false;
            boPropertyMapper.PropertyChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boPropertyMapper.BusinessObject);
            Assert.IsNotNull(boPropertyMapper.Property);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_BusinessObject_WhenSetWithBONotHavingSpecifiedProperty_ShouldNotFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propertyName = "SomeNonExistentProperty";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            bool eventFired = false;
            boPropertyMapper.PropertyChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw an Exception");
            }
            //---------------Test Result -----------------------
            catch (Exception)
            {
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
                Assert.IsFalse(eventFired);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingPropertyOnRelatedBO_ShouldReturnRelatedBOsProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Organisation.Props[innerPropertyName], boPropertyMapper.Property);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingPropertyOnRelatedBO_AndRelatedBoIsNull_ShouldReturnNullProperty()
        {
            //---------------Set up test pack-------------------
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO{Organisation = new OrganisationTestBO()};
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            ContactPersonTestBO newContactPersonTestBO = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.IsNotNull(boPropertyMapper.Property);
            Assert.IsNotNull(contactPersonTestBO.Organisation);
            Assert.IsNull(newContactPersonTestBO.Organisation);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = newContactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newContactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_HavingPropertyOnRelatedBO_ShouldReturnNullProperty()
        {
            //---------------Set up test pack-------------------
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO{Organisation = new OrganisationTestBO()};
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boPropertyMapper.BusinessObject);
            Assert.IsNotNull(boPropertyMapper.Property);
            Assert.IsNotNull(contactPersonTestBO.Organisation);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingNonExistingPropertyOnRelatedBO_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            IClassDef organisationClassDef = ClassDef.Get<OrganisationTestBO>();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerPropertyName = "NonExistingProperty";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The property '" + innerPropertyName + "' on '"
                     + organisationClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The property '" + innerPropertyName + "' does not exist on the BusinessObject '"
                     + organisationClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_HavingPropertyOnRelatedBO_ShouldFirePropertyChangeEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            bool eventFired = false;
            boPropertyMapper.PropertyChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Organisation.Props[innerPropertyName], boPropertyMapper.Property);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToDifferentBo_HavingPropertyOnRelatedBO_ShouldChangeProperty()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            OrganisationTestBO newOrganisationTestBO = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(oldOrganisationTestBO.Props[innerPropertyName], boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = newOrganisationTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(newOrganisationTestBO.Props[innerPropertyName], boPropertyMapper.Property);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToDifferentBo_HavingPropertyOnRelatedBO_ShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            OrganisationTestBO newOrganisationTestBO = new OrganisationTestBO();
            bool eventFired = false;
            boPropertyMapper.PropertyChanged += (sender, e) => eventFired = true;
            //---------------Assert Precondition----------------
            Assert.IsFalse(eventFired);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = newOrganisationTestBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void Test_ChangeRelatedBO_WhenSetToNull_HavingPropertyOnRelatedBO_ShouldChangePropertyToNull()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO oldOrganisationTestBO = new OrganisationTestBO();
            contactPersonTestBO.Organisation = oldOrganisationTestBO;
            const string innerPropertyName = "Name";
            const string propertyName = "Organisation." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.AreSame(oldOrganisationTestBO.Props[innerPropertyName], boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            contactPersonTestBO.Organisation = null;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
        }

        // When the child property is not found. i.e. "NonExisting.Addresses"
        [Test]
        public void Test_BusinessObject_WhenSet_HavingNonExistingChildRelationshipForRelatedBo_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            IClassDef contactPersonClassDef = ClassDef.Get<ContactPersonTestBO>();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Organisation = new OrganisationTestBO();
            const string outerRelationshipName = "NonExistingRelationship";
            const string innerPropertyName = "Name";
            const string propertyName = outerRelationshipName + "." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' does not exist on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "'", ex.DeveloperMessage);
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
            }
        }

        // When the child property is a multiple property, then an error should be thrown.
        [Test]
        public void Test_BusinessObject_WhenSet_HavingExistingNonSingleRelationshipOnRelatedBO_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO.LoadDefaultClassDef();
            IClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string outerRelationshipName = "Addresses";
            const string innerPropertyName = "ContactPersonTestBO";
            const string propertyName = outerRelationshipName + "." + innerPropertyName;
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            Assert.IsNull(boPropertyMapper.Property);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.BusinessObject = contactPersonTestBO;
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.", ex.Message);
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed.", ex.DeveloperMessage);
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
            }
        }

    }
}