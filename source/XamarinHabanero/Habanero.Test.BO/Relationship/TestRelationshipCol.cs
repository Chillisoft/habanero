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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.Relationship
{
    /// <summary>
    /// Summary description for TestRelationshipCol.
    /// </summary>
    [TestFixture]
    public class TestRelationshipCol 
    {
        
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
        }

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            //new Address();
        }

        [Test]
        public void TestMissingRelationshipErrorMessageSingle()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            try
            {
                bo1.Relationships.GetRelatedObject<MyBO>("WrongRelationshipName");
                Assert.Fail("Expected to throw an RelationshipNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains("The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO", ex.Message);
            }
        }

        [Test]
        public void TestMissingRelationshipErrorMessageMultiple()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            try
            {
                bo1.Relationships.GetRelatedCollection("WrongRelationshipName");
                Assert.Fail("Expected to throw an RelationshipNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains("The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO", ex.Message);
            }
        }

        [Test]
        public void TestInvalidRelationshipAccessSingle()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            try
            {
                bo1.Relationships.GetRelatedCollection("MyRelationship");
                Assert.Fail("Expected to throw an InvalidRelationshipAccessException");
            }
                //---------------Test Result -----------------------
            catch (InvalidRelationshipAccessException ex)
            {
                StringAssert.Contains("The 'single' relationship MyRelationship was accessed as a 'multiple' relationship (using GetRelatedCollection()).", ex.Message);
            }
        }

        [Test]
        public void TestInvalidRelationshipAccessMultiple()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            try
            {
                bo1.Relationships.GetRelatedObject<MyBO>("MyMultipleRelationship");
                Assert.Fail("Expected to throw an InvalidRelationshipAccessException");
            }
                //---------------Test Result -----------------------
            catch (InvalidRelationshipAccessException ex)
            {
                StringAssert.Contains("The 'multiple' relationship MyMultipleRelationship was accessed as a 'single' relationship (using GetRelatedObject()).", ex.Message);
            }
        }

        [Test]
        public void TestIndexer()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            IRelationship relationship = bo1.Relationships["MyMultipleRelationship"];

            //---------------Test Result -----------------------
            Assert.IsNotNull(relationship);
        }

   
        [Test]
        public void TestGetSingle_Generic()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            const string relationshipName = "MyRelationship";

            //---------------Execute Test ----------------------
            SingleRelationship<MyRelatedBo> relationship = bo1.Relationships.GetSingle<MyRelatedBo>(relationshipName);
            
            //---------------Test Result -----------------------

            Assert.IsNotNull(relationship);
            Assert.AreEqual(relationshipName, relationship.RelationshipName);
        }

        [Test]
        public void TestGetSingle_Generic_Fail()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyMultipleRelationship";

            //---------------Execute Test ----------------------
            try
            {
                SingleRelationship<MyRelatedBo> relationship = bo1.Relationships.GetSingle<MyRelatedBo>(relationshipName);
                Assert.Fail("Should have failed because we're accessing a multiple relationship as a single.");
            //---------------Test Result -----------------------
            }
            catch (InvalidRelationshipAccessException)
            {

            }
        }

        [Test]
        public void TestGetMultiple_Generic()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyMultipleRelationship";

            //---------------Execute Test ----------------------
            MultipleRelationship<MyRelatedBo> relationship = bo1.Relationships.GetMultiple<MyRelatedBo>(relationshipName);

            //---------------Test Result -----------------------

            Assert.IsNotNull(relationship);
            Assert.AreEqual(relationshipName, relationship.RelationshipName);
        }

        [Test]
        public void TestGetMultiple_Generic_Fail()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyRelationship";

            //---------------Execute Test ----------------------
            try
            {
                MultipleRelationship<MyRelatedBo> relationship = bo1.Relationships.GetMultiple<MyRelatedBo>(relationshipName);
                Assert.Fail("Should have failed because we're accessing a single relationship as a multiple.");
                //---------------Test Result -----------------------
            }
            catch (InvalidRelationshipAccessException)
            {

            }
        }


        [Test]
        public void TestGetSingle()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyRelationship";

            //---------------Execute Test ----------------------
            ISingleRelationship relationship = bo1.Relationships.GetSingle(relationshipName);

            //---------------Test Result -----------------------

            Assert.IsNotNull(relationship);
            Assert.AreEqual(relationshipName, relationship.RelationshipName);
        }

        [Test]
        public void TestGetSingle_Fail()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyMultipleRelationship";

            //---------------Execute Test ----------------------
            try
            {
                ISingleRelationship relationship = bo1.Relationships.GetSingle(relationshipName);
                Assert.Fail("Should have failed because we're accessing a multiple relationship as a single.");
                //---------------Test Result -----------------------
            }
            catch (InvalidRelationshipAccessException)
            {

            }
        }

        [Test]
        public void TestGetMultiple()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyMultipleRelationship";

            //---------------Execute Test ----------------------
            IMultipleRelationship relationship = bo1.Relationships.GetMultiple(relationshipName);

            //---------------Test Result -----------------------

            Assert.IsNotNull(relationship);
            Assert.AreEqual(relationshipName, relationship.RelationshipName);
        }

        [Test]
        public void TestGetMultiple_Fail()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            string relationshipName = "MyRelationship";

            //---------------Execute Test ----------------------
            try
            {
                IMultipleRelationship relationship = bo1.Relationships.GetMultiple(relationshipName);
                Assert.Fail("Should have failed because we're accessing a single relationship as a multiple.");
                //---------------Test Result -----------------------
            }
            catch (InvalidRelationshipAccessException)
            {

            }
        }

        [Test]
        public void TestSetRelatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            IClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo)relatedClassDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.SetRelatedObject("MyRelationship", relatedBo1);

            //---------------Test Result -----------------------
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }

        [Test]
        public void TestSetRelatedBusinessObjectWithWrongRelationshipType()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = MyBO.LoadClassDefWithRelationship();
            IClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo)relatedClassDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            try
            {
                bo1.Relationships.SetRelatedObject("MyMultipleRelationship", relatedBo1);
                Assert.Fail("Expected to throw an InvalidRelationshipAccessException");
            }
                //---------------Test Result -----------------------
            catch (InvalidRelationshipAccessException ex)
            {
                StringAssert.Contains("SetRelatedObject() was passed a relationship (MyMultipleRelationship) that is of type 'multiple' when it expects a 'single' relationship", ex.Message);
            }
        }

        [Test]
        public void TestIsDirty_NotDirtyRelationship()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadClassDefWithAssociationRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();

            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.MyMultipleRelationship.IsDirty);

            //---------------Execute Test ----------------------
            bool isDirty = bo.Relationships.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsFalse(isDirty, "Should be dirty since dirty Association relationships do make the RelationshipCol dirty");
        }


        [Test]
        public void TestIsDirty_AssociationRelationship()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadClassDefWithAssociationRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            bo.MyMultipleRelationship.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.MyMultipleRelationship.IsDirty);

            //---------------Execute Test ----------------------
            bool isDirty = bo.Relationships.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty, "Should be dirty since dirty Association relationships do make the RelationshipCol dirty");
        }

        [Test]
        public void TestIsDirty_AggregationRelationship()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadClassDefWithAggregationRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            bo.MyMultipleRelationship.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.MyMultipleRelationship.IsDirty);

            //---------------Execute Test ----------------------
            bool isDirty = bo.Relationships.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty, "Should be dirty since dirty Aggregation relationships do make the RelationshipCol dirty");
        }

        [Test]
        public void TestIsDirty_CompositionRelationship()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            MyBO.LoadClassDefWithCompositionRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            bo.MyMultipleRelationship.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.IsTrue(bo.MyMultipleRelationship.IsDirty);

            //---------------Execute Test ----------------------
            bool isDirty = bo.Relationships.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty, "Should be dirty since dirty Composition relationships do make the RelationshipCol dirty");
        }

        [Test]
        public void Test_AddDirtyChildrenToTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            new AddressTestBO();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithMultipleRelationshipToAddress();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            MultipleRelationship<ContactPersonTestBO> contactPersonRelationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            BusinessObjectCollection<ContactPersonTestBO> contactPersonCol = contactPersonRelationship.BusinessObjectCollection;
            MultipleRelationship<AddressTestBO> addressRelationship = organisationTestBO.Relationships.GetMultiple<AddressTestBO>("Addresses");
            BusinessObjectCollection<AddressTestBO> addressCol = addressRelationship.BusinessObjectCollection;

            contactPersonCol.CreateBusinessObject();
            addressCol.CreateBusinessObject();

            //---------------Execute Test ----------------------
            TransactionCommitterInMemory tc = new TransactionCommitterInMemory(new DataStoreInMemory());

             relationships.AddDirtyChildrenToTransactionCommitter(tc);

            //---------------Test Result -----------------------
             Assert.AreEqual(2, tc.OriginalTransactions.Count);
        }

        [Test]
        public void Test_CancelEdits()
        {
            //---------------Set up test pack-------------------
            new AddressTestBO();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithMultipleRelationshipToAddress();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            MultipleRelationship<ContactPersonTestBO> contactPersonRelationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            BusinessObjectCollection<ContactPersonTestBO> contactPersonCol = contactPersonRelationship.BusinessObjectCollection;
            MultipleRelationship<AddressTestBO> addressRelationship = organisationTestBO.Relationships.GetMultiple<AddressTestBO>("Addresses");
            BusinessObjectCollection<AddressTestBO> addressCol = addressRelationship.BusinessObjectCollection;

            contactPersonCol.CreateBusinessObject();
            addressCol.CreateBusinessObject();
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationships.IsDirty);
            //---------------Execute Test ----------------------
            relationships.CancelEdits();
            //---------------Test Result -----------------------
            Assert.IsFalse(relationships.IsDirty);
        }

        [Test]
        [Ignore("Xmarin Port - Need to re-write the mocking to use NSub instead of Rhino")]
        public void Test_CancelEdits_CallsCancelEditsOnDirtyRelationshipsOnly()
        {
            //---------------Set up test pack-------------------
            new AddressTestBO();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithMultipleRelationshipToAddress();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationshipCol = new RelationshipCol(organisationTestBO);
            // Setup Mocks
            MockRepository mockRepository = new MockRepository();
            RelationshipBase dirtyRelationship = mockRepository.PartialMock<RelationshipBase>();
            RelationshipBase nonDirtyRelationship = mockRepository.PartialMock<RelationshipBase>();

            Expect.Call(dirtyRelationship.RelationshipName).Return("DirtyRelationship").Repeat.Any();
            Expect.Call(dirtyRelationship.IsDirty).Return(true).Repeat.Once();
            Expect.Call(dirtyRelationship.CancelEdits).Repeat.Once();

            Expect.Call(nonDirtyRelationship.RelationshipName).Return("NonDirtyRelationship").Repeat.Any();
            Expect.Call(nonDirtyRelationship.IsDirty).Return(false).Repeat.Once();
            Expect.Call(nonDirtyRelationship.CancelEdits).Repeat.Never();
            mockRepository.ReplayAll();
            relationshipCol.Add(dirtyRelationship);
            relationshipCol.Add(nonDirtyRelationship);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            relationshipCol.CancelEdits();
            //---------------Test Result -----------------------
            mockRepository.VerifyAll();
        }

        
    }
}