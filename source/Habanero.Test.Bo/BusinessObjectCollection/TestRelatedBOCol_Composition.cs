//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.IO;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.DB4O;
using Habanero.Test.BO.RelatedBusinessObjectCollection;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    [TestFixture]
    public class TestRelatedBOCol_Composition
    {
        private readonly TestUtilsRelated util = new TestUtilsRelated();

        [TestFixtureSetUp]
        public virtual void TestFixtureSetup()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [TearDown]
        public void TearDownTest()
        {
            TestUtil.WaitForGC();
        }

        //   TODO: remove option to do contactPerson.OrganisationID = xcsd.

        [Test]
        public void Test_AddMethod_AddPersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Add(contactPerson);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                AssertMessageIsCorrect(ex, compositionRelationship.RelationshipDef.RelatedObjectClassName, "added", compositionRelationship.RelationshipName);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        private static MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(out BusinessObjectCollection<ContactPersonTestBO> cpCol) {
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            return GetCompositionRelationship(out cpCol, organisationTestBO);
        }

        private static MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(out BusinessObjectCollection<ContactPersonTestBO> cpCol, OrganisationTestBO organisationTestBO)
        {
            MultipleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            cpCol = compositionRelationship.BusinessObjectCollection;
            return compositionRelationship;
        }


        [Test]
        public void Test_AddMethod_AddNewChild()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.GetRandomString(), TestUtil.GetRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.AreSame(myBO.Organisation, compositionRelationship.OwningBO);
        }

        [Test]
        public void Test_ResetParent_PersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            // This rule must also be implemented for the reverse relationship.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            OrganisationTestBO alternateOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = alternateOrganisationTestBO;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }
        [Test]
        public void Test_SetParentNull_PersistedChild()
        {
            //An already persisted invoice line cannot be added to an Invoice 
            // This rule must also be implemented for the reverse relationship.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);
            Assert.AreSame(contactPerson.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = null;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_ResetParent_NewChild_ReverseRelationship_Loaded()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can be set for myBO since myBO has never
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            GetCompositionRelationship(out cpCol, organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.GetRandomString(), TestUtil.GetRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.OrganisationID, organisationTestBO.OrganisationID);
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(contactPerson));
            util.AssertAddedEventFired();
        }

        [Test]
        public void Test_SetParentNull()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can be set to null for myBO since myBO has never
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.GetRandomString(), TestUtil.GetRandomString());

            //---------------Assert Precondition----------------
            Assert.IsNull(myBO.Organisation);

            //---------------Execute Test ----------------------
            myBO.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(myBO.Organisation);
        }

        [Test]
        public void Test_RemoveMethod()
        {
            //An invoice line cannot be removed from an Invoice.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.GetRandomString(), TestUtil.GetRandomString());
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            contactPerson.Save();
            cpCol.LoadAll();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse((bool)ReflectionUtilities.GetPrivatePropertyValue(cpCol, "Loading"));

            //---------------Execute Test ----------------------
            try
            {
                cpCol.Remove(contactPerson);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
                StringAssert.Contains("RemoveChildAction.Prevent", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            }
        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //An new invoice line can be added to an Invoice 
            //(In Habanero a new invoice line can be added to an invoice). 
            // This rule is also be applied for the reverse relationship
            // In this case the organisation can not be set to null for myBO since myBO has
            //   been associated with am organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> compositionRelationship = GetCompositionRelationship(out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.GetRandomString(), TestUtil.GetRandomString());
            contactPerson.Organisation = (OrganisationTestBO)compositionRelationship.OwningBO;
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            util.AssertAddedEventNotFired();
            util.AssertRemovedEventNotFired();

            //---------------Execute Test ----------------------
            //try
            //{
                contactPerson.Organisation = null;
            //    Assert.Fail("expected Err");
            //}
            //    //---------------Test Result -----------------------
            //catch (HabaneroDeveloperException ex)
            //{
            //    StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
            //    StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            //    StringAssert.Contains("RemoveChildAction.Prevent", ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    Assert.Fail("HabaneroDeveloperException not thrown. Exception Thrown was : " + ex.Message);
            //}
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }

        #region Utils

        private void AssertMessageIsCorrect(HabaneroDeveloperException ex, string relatedObjectClassName, string operation, string relationshipName)
        {
            StringAssert.Contains("The " + relatedObjectClassName, ex.Message);
            StringAssert.Contains("could not be " + operation + " since the " + relationshipName + " relationship is set up as ", ex.Message);
        }


        #endregion

    }


    




}