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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.Test.Structure;
using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
// ReSharper disable InconsistentNaming
	/// <summary>
	/// Summary description for TestBOMapper.
	/// </summary>
	[TestFixture]
	public class TestBOMapper : TestUsingDatabase
	{
		private IClassDef _itsClassDef;
        private IClassDef _itsRelatedClassDef;

        private static BOMapper CreateBOMapper(IBusinessObject bo)
        {
            return new BOMapper(bo);
        }

		[TestFixtureSetUp]
		public void SetupTestFixture()
		{
			this.SetupDBConnection();
		}

		[Test]
// ReSharper disable InconsistentNaming
		public void Test_SetDisplayPropertyValue_ShouldSetPropValue()
		{
			//---------------Set up test pack-------------------
			const string propName = "TestProp";
			ClassDef.ClassDefs.Clear();

			var myBOClassDef = MyBO.LoadClassDefWithRelationship();
			MyRelatedBo.LoadClassDef();
			var myBO = (MyBO) myBOClassDef.CreateNewBusinessObject();
			var boMapper = new BOMapper(myBO);
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
        public void Test_SetDisplayPropertyValue_WithRelatedPropName_WhenNullRelationship_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
            bo.MyRelationship = null;
            var myNewValue = "MyNewValue";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            Assert.DoesNotThrow(() =>
            {
                mapper.SetDisplayPropertyValue("MyRelationship.TestProp", myNewValue);
            });
            //---------------Test Result -----------------------

        }

        [Test]
        public void Test_SetDisplayPropertyValue_WithRelatedVirtualPropName_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
            bo.MyRelationship = new MyBO();
            var myNewValue = "MyNewValue";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            mapper.SetDisplayPropertyValue("MyRelationship.-MySettableVirtualProp-", myNewValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(myNewValue, bo.MyRelationship.MySettableVirtualProp);
        }

        [Test]
        public void Test_SetDisplayPropertyValue_WithRelatedVirtualPropName_WhenNullRelationship_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
            bo.MyRelationship = null;
            var myNewValue = "MyNewValue";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            Assert.DoesNotThrow(() =>
            {
                mapper.SetDisplayPropertyValue("MyRelationship.-MySettableVirtualProp-", myNewValue);
            });
            //---------------Test Result -----------------------

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
			_itsClassDef = MyBO.LoadClassDefWithLookup();
			MyBO bo1 = (MyBO)_itsClassDef.CreateNewBusinessObject();
			BOMapper boMapper = new BOMapper(bo1);
			//---------------Assert Precondition----------------
			Assert.IsNullOrEmpty(bo1.TestProp2);
			//---------------Execute Test ----------------------
			boMapper.SetDisplayPropertyValue(propertyName, expectedLookupDisplayValue);
			//---------------Test Result -----------------------
			Assert.AreEqual(expectedLookupValue, bo1.GetPropertyValue(propertyName));
			Assert.AreEqual(expectedLookupDisplayValue, bo1.GetPropertyValueToDisplay(propertyName));
		}

		[Test]
		public void Test_SetPropertyDisplayValue_WithIntString_ShouldBeAbleGetString()
		{
			//---------------Set up test pack-------------------

			ClassDef.ClassDefs.Clear();
			MyBO.LoadDefaultClassDef();
			var testBo = new MyBO();
			var boMapper = new BOMapper(testBo);
			const string propName = "TestProp";
			boMapper.SetDisplayPropertyValue(propName, "7");
			//---------------Assert Precondition----------------
			Assert.AreEqual("7", boMapper.GetPropertyValueToDisplay(propName).ToString());
			//---------------Execute Test ----------------------
			boMapper.SetDisplayPropertyValue(propName, "3");
			//---------------Test Result -----------------------
			Assert.AreEqual("3", boMapper.GetPropertyValueToDisplay(propName).ToString());
			Assert.AreEqual("3", testBo.TestProp);
		}
/*
					var propertyMapper = BOPropMapperFactory.CreateMapper(this._businessObject, propertyName);
			 propertyMapper.SetPropertyValue(value);*/
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
			_itsClassDef = MyBO.LoadClassDefWithLookup();
			MyBO bo1 = (MyBO) _itsClassDef.CreateNewBusinessObject();
			bo1.SetPropertyValue("TestProp2", "s1");
			BOMapper mapper = new BOMapper(bo1);
			Assert.AreEqual("s1", mapper.GetPropertyValueToDisplay("TestProp2"));
		}

        [Test]
        public void Test_WhenSetThePropertyToABusinessObject_ShouldSetThePrimaryKey()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            var classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPersonTestBO.LoadDefaultClassDef();
            var cp = new ContactPersonTestBO();
            const string expectedSurname = "abc";
            cp.Surname = expectedSurname;
            var myBO = (BusinessObject)classDef.CreateNewBusinessObject();

            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedSurname, cp.ToString());
            //---------------Execute Test ----------------------
            myBO.SetPropertyValue("TestProp2", cp);
            //---------------Test Result -----------------------
            Assert.AreEqual(cp.ContactPersonID, myBO.GetPropertyValue("TestProp2"), "This is the ID of the related object");
            Assert.AreEqual(expectedSurname, myBO.GetPropertyValueToDisplay("TestProp2"), "This is the ToString of the related object");
        }

		[Test]
		public void Test_GetPropertyValueToDisplay_BusinessObjectLookupList_NotInList()
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
		public void Test_GetPropertyValueToDisplay_SimpleLookup()
		{
			ClassDef.ClassDefs.Clear();
			_itsClassDef = MyBO.LoadClassDefWithSimpleIntegerLookup();
			MyBO bo1 = (MyBO)_itsClassDef.CreateNewBusinessObject();
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
		public void Test_GetPropertyValue_WithDot()
		{
			ClassDef.ClassDefs.Clear();
			_itsClassDef = MyBO.LoadClassDefWithRelationship();
			_itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			//MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
			MyBO bo1 = (MyBO)_itsClassDef.CreateNewBusinessObject();
			MyRelatedBo relatedBo = (MyRelatedBo)_itsRelatedClassDef.CreateNewBusinessObject();
			Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);

			Assert.AreEqual("MyValue", mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
		}

		[Test]
		public void Test_GetPropertyValue_WithDot_IncorrectRelationshipName()
		{
			//---------------Set up test pack-------------------
			ClassDef.ClassDefs.Clear();
			_itsClassDef = MyBO.LoadClassDefWithRelationship();
			_itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			MyBO bo1 = (MyBO) _itsClassDef.CreateNewBusinessObject();
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
        public void Test_GetPropertyValueToDisplay_WhenRelatedPropertyValue_WithNullRelationship_ShouldReturnNullValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
            bo.MyRelationship = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            var propertyValueToDisplay = mapper.GetPropertyValueToDisplay("MyRelationship.TestProp");
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

		[Test]
        public void Test_GetPropertyValueToDisplay_WhenVirtualPropertyValue()
		{
			ClassDef.ClassDefs.Clear();
			_itsClassDef = MyBO.LoadDefaultClassDef();
			MyBO bo1 = (MyBO)_itsClassDef.CreateNewBusinessObject();

			BOMapper mapper = new BOMapper(bo1);
			Assert.AreEqual("MyNameIsMyBo", mapper.GetPropertyValueToDisplay("-MyName-"));
		}

		[Test]
		public void TestGetLookupListDoesntExist()
		{
			ClassDef.ClassDefs.Clear();
			_itsClassDef = MyBO.LoadClassDefWithNoLookup();
			MyBO bo = new MyBO();
			BOMapper mapper = new BOMapper(bo);
			Assert.AreEqual(0, mapper.GetLookupList("TestProp").Count);
		}

		[Test]
        public void Test_GetPropertyValueToDisplay_WhenRelatedVirtualPropertyValue_ShouldReturnRelatedValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
		    bo.MyRelationship = new MyBO();
		    //---------------Assert Precondition----------------
		    //---------------Execute Test ----------------------
		    var mapper = CreateBOMapper(bo);
		    var propertyValueToDisplay = mapper.GetPropertyValueToDisplay("MyRelationship.-MyName-");
            //---------------Test Result -----------------------
		    Assert.AreEqual("MyNameIsMyBo", propertyValueToDisplay);
		}

	    [Test]
        public void Test_GetPropertyValueToDisplay_WhenRelatedVirtualPropertyValue_WithNullRelationship_ShouldReturnNullValue()
	    {
	        //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            var bo = new MyRelatedBo();
            bo.MyRelationship = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            var propertyValueToDisplay = mapper.GetPropertyValueToDisplay("MyRelationship.-MyName-");
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
	    }

	    [Test]
        public void Test_GetPropertyValueToDisplay_WhenRelatedVirtualPropertyValue_WithTwoLevels_ShouldReturnRelatedValue()
	    {
	        //---------------Set up test pack-------------------
			ClassDef.ClassDefs.Clear();
		    MyBO.LoadClassDefWithRelationship();
		    MyRelatedBo.LoadClassDef();
	        var bo = new MyBO();
	        bo.MyRelationship = new MyRelatedBo();
            bo.MyRelationship.MyRelationship = new MyBO();
	        //---------------Assert Precondition----------------

	        //---------------Execute Test ----------------------
	        var mapper = CreateBOMapper(bo);
	        var propertyValueToDisplay = mapper.GetPropertyValueToDisplay("MyRelationship.MyRelationship.-MyName-");
	        //---------------Test Result -----------------------
	        Assert.AreEqual("MyNameIsMyBo", propertyValueToDisplay);
	    }

	    [Test]
        public void Test_GetPropertyValueToDisplay_WhenRelatedVirtualPropertyValue_WithManyLevels_ShouldReturnRelatedValue()
	    {
	        //---------------Set up test pack-------------------
			ClassDef.ClassDefs.Clear();
		    MyBO.LoadClassDefWithRelationship();
		    MyRelatedBo.LoadClassDef();
	        var bo = new MyBO();
	        bo.MyRelationship = new MyRelatedBo();
            bo.MyRelationship.MyRelationship = new MyBO();
            bo.MyRelationship.MyRelationship.MyRelationship = new MyRelatedBo();
            bo.MyRelationship.MyRelationship.MyRelationship.MyRelationship = new MyBO();
	        //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var mapper = CreateBOMapper(bo);
            var propertyValueToDisplay = mapper.GetPropertyValueToDisplay("MyRelationship.MyRelationship.MyRelationship.MyRelationship.-MyName-");
	        //---------------Test Result -----------------------
	        Assert.AreEqual("MyNameIsMyBo", propertyValueToDisplay);
	    }


	}
}