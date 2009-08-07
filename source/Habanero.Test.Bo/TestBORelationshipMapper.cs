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
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
            BORegistry.DataAccessor = new DataAccessorInMemory();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO,boRelationshipMapper.BusinessObject);
        }

        [Test]
        public void Test_BusinessObject_WhenSetWithBOHavingSpecifiedRelationship_ShouldReturnCorrectRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string relationshipName = "Organisation";
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO,boRelationshipMapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO.Relationships[relationshipName],boRelationshipMapper.Relationship);
        }        
        
        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldReturnNullRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsNull(contactPersonTestBO.Organisation);
            //---------------Execute Test ----------------------
            boRelationshipMapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_HavingRelationshipOnRelatedBO_ShouldReturnNullRelationship()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            const string innerRelationshipName = "ContactPeople";
            const string relationshipName = "Organisation." + innerRelationshipName;
            BORelationshipMapper boRelationshipMapper = new BORelationshipMapper(relationshipName);
            //---------------Assert Precondition----------------
            Assert.IsNull(boRelationshipMapper.BusinessObject);
            Assert.IsNull(boRelationshipMapper.Relationship);
            Assert.IsNull(contactPersonTestBO.Organisation);
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            ClassDef organisationClassDef = OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
        public void Test_ChangeRelatedBO_WhenSetToNull_HavingRelationshipOnRelatedBO_ShouldChangeRelationshipToNull()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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
            ClassDef contactPersonClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef();
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

        //TODO Mark 07 Aug 2009: Check if we need some tests on RelationshipChanged for Two/More levels of Relationships.

    }

    /// <summary>
    /// This is a mapper class that handles the mapping of a relationship name 
    /// to a specific relationship for a specified <see cref="IBusinessObject"/>.
    /// The relationship name can be specified as a path through single relationships on the <see cref="IBusinessObject"/>
    /// and its' relationship tree.
    /// <remarks>For Example:<br/>
    /// For the ContactPerson BusinessObject when the relationshipName is "Organisation",
    ///  the returned <see cref="IRelationship"/> will be the "Organisation" relationship on ContactPerson.<br/>
    /// If the relationshipName was "Organisation.Address" then the Organisation relationship on the contact person will 
    /// be traversed and monitored and return the corresponding "Address" <see cref="IRelationship"/> for the ContactPerson's current Organisation.</remarks>
    /// </summary>
    public class BORelationshipMapper
    {
        private IBusinessObject _businessObject;
        private IRelationship _relationship;
        private readonly BORelationshipMapper _childBoRelationshipMapper;
        private readonly BORelationshipMapper _localBoRelationshipMapper;
        private ISingleRelationship _childRelationship;
        public event EventHandler RelationshipChanged;

        public BORelationshipMapper(string relationshipName)
        {
            if (String.IsNullOrEmpty(relationshipName)) throw new ArgumentNullException("relationshipName");
            RelationshipName = relationshipName;
            if (RelationshipName.Contains("."))
            {
                string[] parts = RelationshipName.Split('.');
                string localRelationshipName = parts[0];
                _localBoRelationshipMapper = new BORelationshipMapper(localRelationshipName);
                string remainingPath = String.Join(".", parts, 1, parts.Length - 1);
                _childBoRelationshipMapper = new BORelationshipMapper(remainingPath);
                _childBoRelationshipMapper.RelationshipChanged += (sender, e) => FireRelationshipChanged();
            }
        }

        public string RelationshipName { get; private set; }

        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                IBusinessObject businessObject = value;
                IRelationship relationship = null;

                if (_childBoRelationshipMapper != null)
                {
                    _localBoRelationshipMapper.BusinessObject = businessObject;
                    UpdateChildRelationship();
                    _businessObject = businessObject;
                    return;
                }
                if (businessObject != null)
                {
                    if (businessObject.Relationships.Contains(RelationshipName))
                        relationship = businessObject.Relationships[RelationshipName];
                    else
                    {
                        IClassDef classDef = businessObject.ClassDef;
                        throw new HabaneroDeveloperException("The relationship '" + RelationshipName + "' on '"
                            + classDef.ClassName + "' cannot be found. Please contact your system administrator.", 
                            "The relationship '" + RelationshipName + "' does not exist on the BusinessObject '"
                            + classDef.ClassNameFull + "'.");
                    }
                }
                _businessObject = businessObject;
                Relationship = relationship;
            }
        }

        private void UpdateChildRelationship()
        {
            DeRegisterForChildRelationshipEvents();
            IRelationship childRelationship = _localBoRelationshipMapper.Relationship;
            if (childRelationship != null && !(childRelationship is ISingleRelationship))
            {
                IClassDef classDef = childRelationship.OwningBO.ClassDef;
                throw new HabaneroDeveloperException("The relationship '" + _localBoRelationshipMapper.RelationshipName + "' on '"
                     + classDef.ClassName + "' is not a Single Relationship. Please contact your system administrator.",
                            "The relationship '" + _localBoRelationshipMapper.RelationshipName + "' on the BusinessObject '"
                     + classDef.ClassNameFull + "' is not a Single Relationship therefore cannot be traversed."); 
            }
            _childRelationship = (ISingleRelationship)childRelationship;
            RegisterForChildRelationshipEvents(); 
            UpdateChildRelationshipBO();
        }

        private void RegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null)
                _childRelationship.RelatedBusinessObjectChanged += ChildRelationship_OnRelatedBusinessObjectChanged;
        }

        private void DeRegisterForChildRelationshipEvents()
        {
            if (_childRelationship != null) _childRelationship.RelatedBusinessObjectChanged -= ChildRelationship_OnRelatedBusinessObjectChanged;
        }

        private void ChildRelationship_OnRelatedBusinessObjectChanged(object sender, EventArgs e)
        {
            UpdateChildRelationshipBO();
        }

        private void UpdateChildRelationshipBO()
        {
            IBusinessObject relatedObject = null;
            if (_childRelationship != null) relatedObject = _childRelationship.GetRelatedObject();
            _childBoRelationshipMapper.BusinessObject = relatedObject;
        }

        public IRelationship Relationship
        {
            get
            {
                if (_childBoRelationshipMapper != null) return _childBoRelationshipMapper.Relationship;
                return _relationship;
            }
            private set
            {
                _relationship = value;
                FireRelationshipChanged();
            }
        }

        private void FireRelationshipChanged()
        {
            if (RelationshipChanged != null) RelationshipChanged(this, new EventArgs());
        }
    }

}
