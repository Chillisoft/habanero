using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
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

		//[Test]
		//public void TestGetPropertyValueWithDot()
		//{
		//    Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
		//    IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;
        	
		//    Mock relColControl = new DynamicMock(typeof (IRelationshipCol));
		//    IRelationshipCol mockRelCol = (IRelationshipCol) relColControl.MockInstance;

		//    ClassDef.ClassDefs.Clear();
		//    itsClassDef = MyBO.LoadClassDefWithRelationship();
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
			//Converted to NMock2
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
			Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
			bo1.SetPropertyValue("RelatedID", myRelatedBoGuid);
			relatedBo.SetPropertyValue("MyRelatedTestProp", "MyValue");
			BOMapper mapper = new BOMapper(bo1);
			bo1.Relationships = mockRelCol;

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
			bo1.Relationships = mockRelCol;

			//relColControl.ExpectAndReturn("GetRelatedObject", null, new object[] { "MyRelationship" });
			Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(null);

			//mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(), new object[] { });
			Expect.Call(connection.GetConnection()).Repeat.Never();
			mock.ReplayAll();
			Assert.AreEqual(null, mapper.GetPropertyValueToDisplay("MyRelationship.MyRelatedTestProp"));
			mock.VerifyAll();
		}

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
    }
}