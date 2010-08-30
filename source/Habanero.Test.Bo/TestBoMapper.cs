//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.Structure;
using Habanero.Util;
using NMock;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBOMapper.
    /// </summary>
    [TestFixture]
    public class TestBOMapper : TestUsingDatabase
    {
        private IClassDef itsClassDef;
        private IClassDef itsRelatedClassDef;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void Test_SetDisplayPropertyValue_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            const string propName = "TestProp";
            ClassDef.ClassDefs.Clear();

            var myBOClassDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO myBO = (MyBO) myBOClassDef.CreateNewBusinessObject();
            BOMapper boMapper = new BOMapper(myBO);
            var initialPropValue = RandomValueGen.GetRandomString();
            myBO.SetPropertyValue(propName, initialPropValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual(initialPropValue, myBO.GetPropertyValue(propName));
            //---------------Execute Test ----------------------
            var expectedPropValue = RandomValueGen.GetRandomString();
            boMapper.SetDisplayPropertyValue(propName, expectedPropValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPropValue, myBO.GetPropertyValue(propName));
        }
        [Test]
        public void Test_SetDisplayPropertyValue_WithRelatedPropName_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            const string underlyingPropName = "MyRelatedTestProp";
            const string propName = "MyRelationship." + underlyingPropName;
            ClassDef.ClassDefs.Clear();

            var myBOClassDef = MyBO.LoadClassDefWithRelationship();
            var relatedClassDef = MyRelatedBo.LoadClassDef();

            MyBO myBO = (MyBO)myBOClassDef.CreateNewBusinessObject();
            MyRelatedBo myRelatedBo = (MyRelatedBo) relatedClassDef.CreateNewBusinessObject();
            myBO.Relationships.SetRelatedObject("MyRelationship", myRelatedBo);
            BOMapper boMapper = new BOMapper(myBO);
            var initialPropValue = RandomValueGen.GetRandomString();
            myRelatedBo.SetPropertyValue(underlyingPropName, initialPropValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual(initialPropValue, myRelatedBo.GetPropertyValue(underlyingPropName));
            //---------------Execute Test ----------------------
            var expectedPropValue = RandomValueGen.GetRandomString();
            boMapper.SetDisplayPropertyValue(propName, expectedPropValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPropValue, myRelatedBo.GetPropertyValue(underlyingPropName));
        }

        [Test]
        public void Test_SetDisplayPropertyValue_WhenLookupList_ShouldSetUnderlyingValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            const string propertyName = "TestProp2";
            const string expectedLookupDisplayValue = "s1";
            Guid expectedLookupValue;
            StringUtilities.GuidTryParse("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}", out expectedLookupValue);
            itsClassDef = MyBO.LoadClassDefWithLookup();
            MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject();
            BOMapper boMapper = new BOMapper(bo1);
            //---------------Assert Precondition----------------
            Assert.IsNull(bo1.TestProp2);
            //---------------Execute Test ----------------------
            boMapper.SetDisplayPropertyValue(propertyName, expectedLookupDisplayValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLookupValue, bo1.GetPropertyValue(propertyName));
            Assert.AreEqual(expectedLookupDisplayValue, bo1.GetPropertyValueToDisplay(propertyName));
        }
/*

        [Test]
        public void Test_IsDirty_WhenBOPropDirty_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            const string propName = "TestProp";
            ClassDef.ClassDefs.Clear();

            var myBOClassDef = MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO myBO = (MyBO)myBOClassDef.CreateNewBusinessObject();
            BOMapper boMapper = new BOMapper(myBO);
            var initialPropValue = RandomValueGen.GetRandomString();
            myBO.SetPropertyValue(propName, initialPropValue);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            boMapper.IsDirty()
            //---------------Test Result -----------------------
            Assert.Fail("Not Yet Implemented");
        }
*/

        [Test]
        public void TestGetPropertyValueToDisplay()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithLookup();
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp2", "s1");
            BOMapper mapper = new BOMapper(bo1);
            Assert.AreEqual("s1", mapper.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestGetPropertyValueToDisplay_BusinessObjectLookupList()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPersonTestBO.LoadDefaultClassDef();
            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestGetPropertyValueToDisplay_BusinessObjectLookupList_NotInList()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = MyBO.LoadClassDefWithBOLookup("Surname <> abc");
            ContactPersonTestBO.LoadDefaultClassDef();

            Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, "abc");
            ContactPersonTestBO cp = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);
            BusinessObject bo = (BusinessObject)classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.IsNotNull(bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2").ToString());
        }

        [Test]
        public void TestGetPropertyValueToDisplay_SimpleLookup()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithSimpleIntegerLookup();
            MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp2", "Text");
            BOMapper mapper = new BOMapper(bo1);
            Assert.AreEqual("Text", mapper.GetPropertyValueToDisplay("TestProp2"));
        }

		//[Test]
		//public void TestGetPropertyValueWithDot()
		//{
		//    Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
		//    IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;
        	
		//    Mock relColControl = new DynamicMock(typeof (IRelationshipCol));
		//    IRelationshipCol mockRelCol = (IRelationshipCol) relColControl.MockInstance;

		//    ClassDef.ClassDefs.Clear();
		//    _classDef = MyBO.LoadClassDefWithRelationship();
		//    itsRelatedClassDef = MyRelatedBo.LoadClassDef();
		//    MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject(connection);
		//    MyRelatedBo relatedBo = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
		//    Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
		//    bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
		//    relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
		//    BOMapper mapper = new BOMapper(bo1);
		//    bo1.Relationships = mockRelCol;

		//    relColControl.ExpectAndReturn("GetRelatedObject", relatedBo, new object[] {"MyRelationship"});

		//    mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
		//                                     new object[] {});
		//    Assert.AreEqual("MyValue", mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
		//}

		[Test]
		public void TestGetPropertyValueWithDot()
		{
			ClassDef.ClassDefs.Clear();
			itsClassDef = MyBO.LoadClassDefWithRelationship();
			itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			//MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
			MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject();
			MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
			Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);

			Assert.AreEqual("MyValue", mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
		}

        [Test]
        public void TestGetPropertyValueWithDot_IncorrectRelationshipName()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            BOMapper mapper = new BOMapper(bo1);
            //---------------Execute Test ----------------------
            try
            {
                mapper.GetPropertyValueToDisplay("MyIncorrectRelationship.MyRelatedTestProp");
                Assert.Fail("Expected to throw an RelationshipNotFoundException");
            }
                //---------------Test Result -----------------------
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains("The relationship 'MyIncorrectRelationship' on 'MyBO' cannot be found", ex.Message);
            }
        }

//        [Test]
//        public void TestGetPropertyValueWithDotNoValue()
//        {
//            Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
//            IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;

//            Mock relColControl = new DynamicMock(typeof (IRelationshipCol));
//            IRelationshipCol mockRelCol = (IRelationshipCol) relColControl.MockInstance;

//            ClassDef.ClassDefs.Clear();
//            itsClassDef = MyBO.LoadClassDefWithRelationship();
//            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
//            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject(connection);
//            MyRelatedBo relatedBo = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
////			Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
////			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
//            relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
//            BOMapper mapper = new BOMapper(bo1);
//            bo1.Relationships = mockRelCol;

//            relColControl.ExpectAndReturn("GetRelatedObject", null, new object[] {"MyRelationship"});

//            mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
//                                             new object[] {});
//            Assert.AreEqual(null, mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
//        }

		[Test]
		public void TestGetPropertyValueWithDotNoValue_WhenPropDoesNotExist()
		{
			ClassDef.ClassDefs.Clear();
			itsClassDef = MyBO.LoadClassDefWithRelationship();
			itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			//MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
			MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject();
			MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
			//			Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
			//			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);

			Assert.AreEqual(null, mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
		}

        [Test]
        public void TestVirtualPropertyValue()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadDefaultClassDef();
            MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject();

            BOMapper mapper = new BOMapper(bo1);
            Assert.AreEqual("MyNameIsMyBo", mapper.GetPropertyValueToDisplay("-MyName-"));
        }

        [Test]
        public void TestGetLookupListDoesntExist()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithNoLookup();
            MyBO bo = new MyBO();
            BOMapper mapper = new BOMapper(bo);
            Assert.AreEqual(0, mapper.GetLookupList("TestProp").Count);
        }

        [Test]
        public void TestVirtualPropertyValueWithDot()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyRelatedBo.LoadClassDef_WithUIDefVirtualProp();
            itsRelatedClassDef = MyBO.LoadClassDefWithRelationship();
            //MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
            MyRelatedBo bo1 = (MyRelatedBo)itsClassDef.CreateNewBusinessObject();
            MyBO relatedBo = (MyBO)itsRelatedClassDef.CreateNewBusinessObject();
            Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
            bo1.SetPropertyValue("MyBoID", myRelatedBoGuid);
            BOMapper mapper = new BOMapper(bo1);
            Assert.AreEqual("MyNameIsMyBo", mapper.GetPropertyValueToDisplay("MyRelationship.-MyName-"));
        }


    }
}