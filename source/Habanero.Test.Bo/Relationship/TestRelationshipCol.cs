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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
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
        }

        [Test, ExpectedException(typeof (RelationshipNotFoundException),
            ExpectedMessage ="The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO"
        )]
        public void TestMissingRelationshipErrorMessageSingle()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.GetRelatedObject<MyBO>("WrongRelationshipName");
        }

        [Test, ExpectedException(typeof (RelationshipNotFoundException),
              ExpectedMessage = "The relationship WrongRelationshipName was not found on a BusinessObject of type Habanero.Test.MyBO"
        )]
        public void TestMissingRelationshipErrorMessageMultiple()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.GetRelatedCollection("WrongRelationshipName");
        }

        [Test, ExpectedException(typeof (InvalidRelationshipAccessException),
               ExpectedMessage = "The 'single' relationship MyRelationship was accessed as a 'multiple' relationship (using GetRelatedCollection())."
        )]
        public void TestInvalidRelationshipAccessSingle()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.GetRelatedCollection("MyRelationship");
        }

        [Test, ExpectedException(typeof (InvalidRelationshipAccessException),
               ExpectedMessage="The 'multiple' relationship MyMultipleRelationship was accessed as a 'single' relationship (using GetRelatedObject())."
        )]
        public void TestInvalidRelationshipAccessMultiple()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.GetRelatedObject<MyBO>("MyMultipleRelationship");
        }

        [Test]
        public void TestIndexer()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            IRelationship relationship = bo1.Relationships["MyMultipleRelationship"];

            //---------------Test Result -----------------------
            Assert.IsNotNull(relationship);
        }

        //[Test]
        //public void TestIndexer_Generic()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = MyBO.LoadClassDefWithRelationship();
        //    MyRelatedBo.LoadClassDef();
        //    MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();

        //    //---------------Execute Test ----------------------
        //    Relationship<MyRelatedBo> relationship = bo1.Relationships<MyRelatedBo>["MyMultipleRelationship"];
            
        //    //---------------Test Result -----------------------

        //    Assert.IsNotNull(relationship);
        //    //MultipleRelationship<MyRelatedBo> multipleRelationship = relationship as MultipleRelationship<MyRelatedBo>;
        //    //Assert.IsNotNull(multipleRelationship);

        //}

        [Test]
        public void TestSetRelatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            ClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo)relatedClassDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.SetRelatedObject("MyRelationship", relatedBo1);

            //---------------Test Result -----------------------
            Assert.AreSame(relatedBo1, bo1.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship"));
            Assert.AreSame(bo1.GetPropertyValue("RelatedID"), relatedBo1.GetPropertyValue("MyRelatedBoID"));
        }

        [Test, ExpectedException(typeof (InvalidRelationshipAccessException),
            ExpectedMessage="SetRelatedObject() was passed a relationship (MyMultipleRelationship) that is of type 'multiple' when it expects a 'single' relationship"
        )]
        public void TestSetRelatedBusinessObjectWithWrongRelationshipType()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWithRelationship();
            ClassDef relatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO)classDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo1 = (MyRelatedBo)relatedClassDef.CreateNewBusinessObject();

            //---------------Execute Test ----------------------
            bo1.Relationships.SetRelatedObject("MyMultipleRelationship", relatedBo1);
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
            Assert.IsFalse(isDirty, "Should not be dirty since dirty Association relationships do not make the RelationshipCol dirty");
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
    }
}