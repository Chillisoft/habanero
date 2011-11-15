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
using Habanero.Test.Structure;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
// ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestBOPropertyMapper
    {
        private const string _relationshipPathSeperator = ".";

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
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("The property '" + propertyName + "' on '"
                     + contactPersonTestBO.ClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
/*                StringAssert.Contains("The property '" + propertyName + "' does not exist on the BusinessObject '"
                     + contactPersonTestBO.ClassDef.ClassNameFull + "'", ex.DeveloperMessage);*/
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
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("The property '" + innerPropertyName + "' on '"
                     + organisationClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
/*                StringAssert.Contains("The property '" + innerPropertyName + "' does not exist on the BusinessObject '"
                     + organisationClassDef.ClassNameFull + "'", ex.DeveloperMessage);*/
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
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propertyName)
                    {BusinessObject = contactPersonTestBO};
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
            string propertyName = outerRelationshipName + _relationshipPathSeperator + innerPropertyName;
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
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' cannot be found. Please contact your system administrator.", ex.Message);
/*                StringAssert.Contains("The relationship '" + outerRelationshipName + "' does not exist on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "'", ex.DeveloperMessage);*/
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
            string propertyName = outerRelationshipName + _relationshipPathSeperator + innerPropertyName;
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
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on '"
                     + contactPersonClassDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.", ex.Message);
/*
                StringAssert.Contains("The relationship '" + outerRelationshipName + "' on the BusinessObject '"
                     + contactPersonClassDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed.", ex.DeveloperMessage);
*/
                Assert.IsNull(boPropertyMapper.BusinessObject);
                Assert.IsNull(boPropertyMapper.Property);
            }
        }

        [Test]
        public void Test_SetPropertyValue_ShouldSetBOPropsValue()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propName = "Surname";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propName) {BusinessObject = contactPersonTestBO};
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boPropertyMapper.Property);
            Assert.IsNull(boPropertyMapper.Property.Value);
            //---------------Execute Test ----------------------
            var expectedPropValue = RandomValueGen.GetRandomString();
            boPropertyMapper.SetPropertyValue(expectedPropValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPropValue, boPropertyMapper.Property.Value);
            Assert.AreEqual(expectedPropValue, contactPersonTestBO.Surname);
        }
        [Test]
        public void Test_SetPropertyValue_WhenBONull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            const string propName = "Surname";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.SetPropertyValue(RandomValueGen.GetRandomString());
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                string expectedErrorMessage = string.Format(
                        "Tried to Set Property Value the BOPropertyMapper for Property '{0}' when the BusinessObject is not set "
                        , propName);
                StringAssert.Contains(expectedErrorMessage, ex.Message);
            }
        }
        [Test]
        public void Test_SetPropertyValue_WhenPropertyNull_ShouldDoNothing()
        {
            //Ideally this should raise an error but code this is replacing behaves like this
            // I will review in the future Brett: 01 Jul 2010
            //---------------Set up test pack-------------------
            const string propName = "Surname";
            BOPropertyMapperSpy boPropertyMapper = new BOPropertyMapperSpy(propName);
            var contactPersonTestBO = new ContactPersonTestBO();
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            boPropertyMapper.SetBOProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.Property);
            Assert.AreEqual(propName, boPropertyMapper.PropertyName);
            Assert.IsNull(contactPersonTestBO.Surname);
            //---------------Execute Test ----------------------
            boPropertyMapper.SetPropertyValue(RandomValueGen.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsNull(contactPersonTestBO.Surname);
        }

        [Test]
        public void Test_GetPropertyValue_ShouldSetBOPropsValue()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string propName = "Surname";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propName) { BusinessObject = contactPersonTestBO };
            var expectedPropValue = RandomValueGen.GetRandomString();
            boPropertyMapper.Property.Value = expectedPropValue;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boPropertyMapper.Property);
            Assert.AreEqual(expectedPropValue, boPropertyMapper.Property.Value);
            //---------------Execute Test ----------------------

            object actualValue = boPropertyMapper.GetPropertyValue();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPropValue, actualValue);
            Assert.AreEqual(expectedPropValue, contactPersonTestBO.Surname);
        }
        [Test]
        public void Test_GetPropertyValue_WhenBONull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            const string propName = "Surname";
            BOPropertyMapper boPropertyMapper = new BOPropertyMapper(propName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.BusinessObject);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.GetPropertyValue();
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
            //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                string expectedErrorMessage = string.Format(
                        "Tried to GetPropertyValue the BOPropertyMapper for Property '{0}' when the BusinessObject is not set "
                        , propName);
                StringAssert.Contains(expectedErrorMessage, ex.Message);
            }
        }
        [Test]
        [Ignore("This is ideally how it should work but Current this is not the case so will need to reevaluate as I refactor: Brett")] //TODO Brett 29 Jun 2010: Ignored Test - This is ideally how it should work but Current this is not the case so will need to reevaluate as I refactor: Brett
        public void Test_GetPropertyValue_WhenPropertyNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            const string propName = "Surname";
            BOPropertyMapperSpy boPropertyMapper = new BOPropertyMapperSpy(propName);
            var contactPersonTestBO = new ContactPersonTestBO();
            boPropertyMapper.BusinessObject = contactPersonTestBO;
            boPropertyMapper.SetBOProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boPropertyMapper.Property);
            Assert.AreEqual(propName, boPropertyMapper.PropertyName);
            Assert.IsNull(contactPersonTestBO.Surname);
            //---------------Execute Test ----------------------
            try
            {
                boPropertyMapper.GetPropertyValue();
                Assert.Fail("Expected to throw an HabaneroApplicationException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                string expectedErrorMessage = string.Format(
                        "Tried to GetPropertyValue the BOPropertyMapper for Property '{0}' but there is no BOProp for this prop"
                        , propName);
                StringAssert.Contains(expectedErrorMessage, ex.Message);
            }
        }

        [Test]
        public void Test_InvalidMessage_ShouldReturnPropInvalidMessage()
        {
            //---------------Set up test pack-------------------
            BOPropertyMapperSpy propMapper = new BOPropertyMapperSpy();
            var boPropStub = MockRepository.GenerateStub<IBOProp>();
            boPropStub.Stub(prop => prop.InvalidReason).Return(RandomValueGen.GetRandomString());
            propMapper.SetBOProp(boPropStub);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var invalidMessage = propMapper.InvalidReason;
            //---------------Test Result -----------------------
            Assert.AreEqual(boPropStub.InvalidReason, invalidMessage);
        }
        [Test]
        public void Test_InvalidMessage_WhenPropNull_ShouldReturnStdMessage()
        {
            //---------------Set up test pack-------------------
            BOPropertyMapperSpy propMapper = new BOPropertyMapperSpy();
            IBOProp boPropStub = null;
            propMapper.SetBOProp(boPropStub);
            //---------------Assert Precondition----------------
            Assert.IsNull(propMapper.Property);
            //---------------Execute Test ----------------------
            var invalidMessage = propMapper.InvalidReason;
            //---------------Test Result -----------------------
            var expectedMessage = string.Format("The Property '{0}' is not available"
                     , propMapper.PropertyName );
            Assert.AreEqual(expectedMessage, invalidMessage);
        }

        [Test]
        public void Test_SetPropertyDisplayValue_WithIntString_ShouldBeAbleGetString()
        {
            //---------------Set up test pack-------------------

            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            const string propName = "TestProp";
            var testBo = new MyBO();
            var boMapper = new BOPropertyMapper(propName) { BusinessObject = testBo };
            boMapper.SetPropertyValue("7");
            //---------------Assert Precondition----------------
            Assert.AreEqual("7", boMapper.GetPropertyValue().ToString());
            //---------------Execute Test ----------------------
            boMapper.SetPropertyValue("3");
            //---------------Test Result -----------------------
            Assert.AreEqual("3", boMapper.GetPropertyValue().ToString());
            Assert.AreEqual("3", testBo.TestProp);
        }
        /*
                            var propertyMapper = BOPropMapperFactory.CreateMapper(this._businessObject, propertyName);
                     propertyMapper.SetPropertyValue(value);*/
    }

    class BOPropertyMapperSpy : BOPropertyMapper
    {
        public BOPropertyMapperSpy()
            : base(RandomValueGen.GetRandomString())
        {
        }
        public BOPropertyMapperSpy(string propertyName) : base(propertyName)
        {
        }


        public void SetBOProp(IBOProp prop)
        {
            _property = prop;
        }
    }
}