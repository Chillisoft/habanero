using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBroker
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ClassDef.ClassDefs.Clear();

            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            BORegistry.ClearCustomDataAccessors();
        }

        [SetUp]
        public void Setup()
        {
            BORegistry.ClearCustomDataAccessors();
        }

        private static MyBO CreateSavedMyBO()
        {
            //MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.Save();
            return myBO;
        }

        private static MyBO CreateSavedMyBOWithSingleRelationship()
        {
            //MyBO.LoadClassDefWithRelationship();
            //MyRelatedBo.LoadClassDef();

            MyRelatedBo relatedBo = new MyRelatedBo();
            relatedBo.Save();

            MyBO myBO = new MyBO();
            myBO.SetPropertyValue("RelatedID", relatedBo.GetPropertyValue("MyRelatedBoID"));
            myBO.Save();

            return myBO;
        }

        private static MyBO CreateSavedMyBOWithMultipleRelationship()
        {
            //MyBO.LoadClassDefWithRelationship();
            //MyRelatedBo.LoadClassDef();

            MyBO myBO = new MyBO();
            myBO.Save();

            MyRelatedBo relatedBo = new MyRelatedBo();
            relatedBo.SetPropertyValue("MyBoID", myBO.GetPropertyValue("MyBoID"));
            relatedBo.Save();

            return myBO;
        }

        [Test]
        public void Test_GetBusinessObject_T_IPrimaryKey_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(myBO.ID);
            MyBO regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(myBO.ID);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_IPrimaryKey_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(myBO.ID);
            MyBO regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject<MyBO>(myBO.ID);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_IPrimaryKey_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, myBO.ID);
            IBusinessObject regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, myBO.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_IPrimaryKey_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, myBO.ID);
            IBusinessObject regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, myBO.ID);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_Criteria_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(criteria);
            MyBO regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_Criteria_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(criteria);
            MyBO regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_Criteria_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, criteria);
            IBusinessObject regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, criteria);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_Criteria_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, criteria);
            IBusinessObject regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, criteria);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_ISelectQuery_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            SelectQuery selectQuery = new SelectQuery(criteria);
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(selectQuery);
            MyBO regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(selectQuery);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_ISelectQuery_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            SelectQuery selectQuery = new SelectQuery(criteria);
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(selectQuery);
            MyBO regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject<MyBO>(selectQuery);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_ISelectQuery_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            SelectQuery selectQuery = new SelectQuery(criteria);
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, selectQuery);
            IBusinessObject regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, selectQuery);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_ISelectQuery_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            SelectQuery selectQuery = new SelectQuery(criteria);
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, selectQuery);
            IBusinessObject regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, selectQuery);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_string_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string criteria = "TestProp is null";
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(criteria);
            MyBO regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_T_string_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string criteria = "TestProp is null";
            MyBO brokerBO = Broker.GetBusinessObject<MyBO>(criteria);
            MyBO regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_string_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string criteria = "TestProp is null";
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, criteria);
            IBusinessObject regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, criteria);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObject_IClassDef_string_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string criteria = "TestProp is null";
            IBusinessObject brokerBO = Broker.GetBusinessObject(myBO.ClassDef, criteria);
            IBusinessObject regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObject(myBO.ClassDef, criteria);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyBO), brokerBO);
            Assert.IsInstanceOfType(typeof(MyBO), regBO);
            Assert.AreEqual(myBO, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_T_SingleRelationshipT_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBOWithSingleRelationship();
            MyRelatedBo relatedBo = myBO.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship");
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>) myBO.Relationships["MyRelationship"];
            //---------------Assert Precondition----------------
            Assert.IsNotNull(relatedBo);
            //---------------Execute Test ----------------------
            MyRelatedBo brokerBO = Broker.GetRelatedBusinessObject<MyRelatedBo>(relationship);
            MyRelatedBo regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject<MyRelatedBo>(relationship);
            //---------------Test Result -----------------------
            Assert.AreEqual(relatedBo, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_T_SingleRelationshipT_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customDataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), customDataAccessor);
            BORegistry.AddDataAccessor(typeof(MyRelatedBo), customDataAccessor);

            MyBO myBO = CreateSavedMyBOWithSingleRelationship();
            MyRelatedBo relatedBo = myBO.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship");
            SingleRelationship<MyRelatedBo> relationship = (SingleRelationship<MyRelatedBo>)myBO.Relationships["MyRelationship"];
            //---------------Assert Precondition----------------
            Assert.IsNotNull(relatedBo);
            //---------------Execute Test ----------------------
            MyRelatedBo brokerBO = Broker.GetRelatedBusinessObject<MyRelatedBo>(relationship);
            MyRelatedBo regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetRelatedBusinessObject<MyRelatedBo>(relationship);
            //---------------Test Result -----------------------
            Assert.AreEqual(relatedBo, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_ISingleRelationship_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBOWithSingleRelationship();
            MyRelatedBo relatedBo = myBO.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship");
            ISingleRelationship relationship = (SingleRelationship<MyRelatedBo>)myBO.Relationships["MyRelationship"];
            //---------------Assert Precondition----------------
            Assert.IsNotNull(relatedBo);
            //---------------Execute Test ----------------------
            IBusinessObject brokerBO = Broker.GetRelatedBusinessObject(relationship);
            IBusinessObject regBO = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject(relationship);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyRelatedBo), brokerBO);
            Assert.IsInstanceOfType(typeof(MyRelatedBo), regBO);
            Assert.AreEqual(relatedBo, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_ISingleRelationship_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customDataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), customDataAccessor);
            BORegistry.AddDataAccessor(typeof(MyRelatedBo), customDataAccessor);

            MyBO myBO = CreateSavedMyBOWithSingleRelationship();
            MyRelatedBo relatedBo = myBO.Relationships.GetRelatedObject<MyRelatedBo>("MyRelationship");
            ISingleRelationship relationship = (SingleRelationship<MyRelatedBo>)myBO.Relationships["MyRelationship"];
            //---------------Assert Precondition----------------
            Assert.IsNotNull(relatedBo);
            //---------------Execute Test ----------------------
            IBusinessObject brokerBO = Broker.GetRelatedBusinessObject(relationship);
            IBusinessObject regBO = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetRelatedBusinessObject(relationship);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(MyRelatedBo), brokerBO);
            Assert.IsInstanceOfType(typeof(MyRelatedBo), regBO);
            Assert.AreEqual(relatedBo, brokerBO);
            Assert.AreEqual(regBO, brokerBO);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_T_Criteria_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            BusinessObjectCollection<MyBO> brokerCol = Broker.GetBusinessObjectCollection<MyBO>(criteria);
            BusinessObjectCollection<MyBO> regCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, brokerCol.Count);
            Assert.AreEqual(myBO, brokerCol[0]);
            Assert.AreEqual(regCol, brokerCol);
        }

        [Test]
        public void Test_GetBusinessObjectCollection_T_Criteria_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), new DataAccessorInMemory());

            MyBO myBO = CreateSavedMyBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("TestProp", Criteria.ComparisonOp.Equals, null);
            BusinessObjectCollection<MyBO> brokerCol = Broker.GetBusinessObjectCollection<MyBO>(criteria);
            BusinessObjectCollection<MyBO> regCol = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetBusinessObjectCollection<MyBO>(criteria);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, brokerCol.Count);
            Assert.AreEqual(myBO, brokerCol[0]);
            Assert.AreEqual(regCol, brokerCol);
        }

        [Test]
        public void Test_GetRelatedBusinessObjectCollection_T_IMultipleRelationship_DefaultAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();

            MyBO myBO = CreateSavedMyBOWithMultipleRelationship();
            BusinessObjectCollection<MyRelatedBo> relatedBos = myBO.Relationships.GetRelatedCollection<MyRelatedBo>("MyMultipleRelationship");
            IMultipleRelationship relationship = (IMultipleRelationship)myBO.Relationships["MyMultipleRelationship"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, relatedBos.Count);
            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<MyRelatedBo> brokerCol = Broker.GetRelatedBusinessObjectCollection<MyRelatedBo>(relationship);
            RelatedBusinessObjectCollection<MyRelatedBo> regCol = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<MyRelatedBo>(relationship);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, brokerCol.Count);
            Assert.AreEqual(relatedBos, brokerCol);
            Assert.AreEqual(regCol, brokerCol);
        }

        [Test]
        public void Test_GetRelatedBusinessObjectCollection_T_IMultipleRelationship_CustomAccessor()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            DataAccessorInMemory customDataAccessor = new DataAccessorInMemory();
            BORegistry.AddDataAccessor(typeof(MyBO), customDataAccessor);
            BORegistry.AddDataAccessor(typeof(MyRelatedBo), customDataAccessor);

            MyBO myBO = CreateSavedMyBOWithMultipleRelationship();
            BusinessObjectCollection<MyRelatedBo> relatedBos = myBO.Relationships.GetRelatedCollection<MyRelatedBo>("MyMultipleRelationship");
            IMultipleRelationship relationship = (IMultipleRelationship)myBO.Relationships["MyMultipleRelationship"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, relatedBos.Count);
            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<MyRelatedBo> brokerCol = Broker.GetRelatedBusinessObjectCollection<MyRelatedBo>(relationship);
            RelatedBusinessObjectCollection<MyRelatedBo> regCol = BORegistry.GetDataAccessor(typeof(MyBO)).BusinessObjectLoader.GetRelatedBusinessObjectCollection<MyRelatedBo>(relationship);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, brokerCol.Count);
            Assert.AreEqual(relatedBos, brokerCol);
            Assert.AreEqual(regCol, brokerCol);
        }

        //...TODO 2009-05-06 Tests to carry on a similar vein for the remaining:

        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString) where T : class, IBusinessObject, new()
        //public static IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria)
        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteria)
        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria,
        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteriaString,
        //public static IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria, OrderCriteria orderCriteria)
        //public static BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        //public static IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, ISelectQuery selectQuery)
        //public static IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria, string orderCriteria)
        //public static IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria)
        //public static void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new()
        //public static void Refresh(IBusinessObjectCollection collection)
        //public static IBusinessObject Refresh(IBusinessObject businessObject)
        //public static IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IMultipleRelationship relationship)
    }
}
