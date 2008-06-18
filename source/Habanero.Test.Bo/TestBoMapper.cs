//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using NMock;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBoMapper.
    /// </summary>
    [TestFixture]
    public class TestBoMapper : TestUsingDatabase
    {
        private ClassDef itsClassDef;
        private ClassDef itsRelatedClassDef;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

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
            ClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestGetPropertyValueToDisplay_BusinessObjectLookupList_NotInList()
        {
            ContactPersonTestBO.CreateSampleData();
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOLookup("Surname <> abc");
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("Surname = abc");
            BusinessObject bo = (BusinessObject) classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
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
			MockRepository mock = new MockRepository();
			
			//Mock mockDbConnection = new DynamicMock(typeof(IDatabaseConnection));
			IDatabaseConnection connection = mock.CreateMock<IDatabaseConnection>();
			
			//Mock relColControl = new DynamicMock(typeof(IRelationshipCol));
			IRelationshipCol mockRelCol = mock.CreateMock<IRelationshipCol>();

			ClassDef.ClassDefs.Clear();
			itsClassDef = MyBO.LoadClassDefWithRelationship();
			itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
			MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
			Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);
            ((IBusinessObject)bo1).Relationships = mockRelCol;

			//relColControl.ExpectAndReturn("GetRelatedObject", relatedBo, new object[] { "MyRelationship" });
			//Expect.AtLeastOnce.On(mockRelCol).Method("GetRelatedObject").With("MyRelationship").Will(Return.Value(relatedBo));
			Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(relatedBo);

			//mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(), new object[] { });
			//Expect.AtLeastOnce.On(connection).Method("GetConnection").Will(Return.Value(DatabaseConnection.CurrentConnection.GetConnection()));
			Expect.Call(connection.GetConnection()).Repeat.Never();
			mock.ReplayAll();
			Assert.AreEqual("MyValue", mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
			mock.VerifyAll();
		}

        [Test, ExpectedException(typeof (RelationshipNotFoundException))]
        public void TestGetPropertyValueWithDot_IncorrectRelationshipName()
        {
            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadClassDefWithRelationship();
            itsRelatedClassDef = MyRelatedBo.LoadClassDef();
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject();
            BOMapper mapper = new BOMapper(bo1);
            Assert.AreEqual(null, mapper.GetPropertyValueToDisplay("MyIncorrectRelationship.MyRelatedTestProp"));
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
		public void TestGetPropertyValueWithDotNoValue()
		{
			MockRepository mock = new MockRepository();
			//Mock mockDbConnection = new DynamicMock(typeof(IDatabaseConnection));
			//IDatabaseConnection connection = (IDatabaseConnection)mockDbConnection.MockInstance;
			IDatabaseConnection connection = mock.CreateMock<IDatabaseConnection>();

			//Mock relColControl = new DynamicMock(typeof(IRelationshipCol));
			//IRelationshipCol mockRelCol = (IRelationshipCol)relColControl.MockInstance;
			IRelationshipCol mockRelCol = mock.CreateMock<IRelationshipCol>();

			ClassDef.ClassDefs.Clear();
			itsClassDef = MyBO.LoadClassDefWithRelationship();
			itsRelatedClassDef = MyRelatedBo.LoadClassDef();
			MyBO bo1 = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
			MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
			//			Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
			//			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);
            ((IBusinessObject)bo1).Relationships = mockRelCol;

			//relColControl.ExpectAndReturn("GetRelatedObject", null, new object[] { "MyRelationship" });
			Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(null);

			//mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(), new object[] { });
			Expect.Call(connection.GetConnection()).Repeat.Never();
			mock.ReplayAll();
			Assert.AreEqual(null, mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
			mock.VerifyAll();
		}

        [Test]
        public void TestVirtualPropertyValue()
        {
            Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;

            ClassDef.ClassDefs.Clear();
            itsClassDef = MyBO.LoadDefaultClassDef();
            MyBO bo1 = (MyBO) itsClassDef.CreateNewBusinessObject(connection);

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
    }
}