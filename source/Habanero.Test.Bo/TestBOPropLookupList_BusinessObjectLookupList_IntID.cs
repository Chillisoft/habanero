using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropLookupList_BusinessObjectLookupList_IntID
    {
        private const string _validLookupValue = "ValidValue";

        private PropDef _propDef_int;
        private BusinessObjectCollection<BOWithIntID> _collection_IntId;
        private BOWithIntID _validBusinessObject;
        private int _validIntID;


        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BORegistry.DataAccessor = new DataAccessorInMemory();

            _propDef_int = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            _validBusinessObject = new BOWithIntID {TestField = _validLookupValue};
            _validIntID = 3;
            _validBusinessObject.IntID = _validIntID;
            _collection_IntId = new BusinessObjectCollection<BOWithIntID> {_validBusinessObject};

            _propDef_int.LookupList = new BusinessObjectLookupListStub(typeof (BOWithIntID), _collection_IntId);
        }

        #region Lookup List Int

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_Exists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, string> list = businessObjectLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue().ToString(), returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_NotExists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, string> list = businessObjectLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedKey;
            bool keyReturned = list.TryGetValue("InvalidValue", out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_Exists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, string> list = businessObjectLookupList.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validIntID.ToString(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_Exists_Int_UseGetAsValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, string> list = businessObjectLookupList.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validBusinessObject.ID.GetAsValue().ToString(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_NotExists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, string> list = businessObjectLookupList.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue("5", out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }

        private PropDef GetPropDef_Int_WithLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (BOWithIntID), _collection_IntId);
            propDef.LookupList = businessObjectLookupList;
            return propDef;
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_NullValue()
        {
            //---------------Set up test pack-------------------

            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());
            boProp.InitialiseProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_Int_InitialiseProp_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validBusinessObject.IntID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.IntID, boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());
            boProp.InitialiseProp(_validBusinessObject.IntID);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validBusinessObject.IntID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.IntID, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_InvalidInt()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());
            int intNotInLookupList = _validIntID + 22;
            boProp.InitialiseProp(intNotInLookupList);

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(intNotInLookupList, boProp.Value);
            Assert.IsFalse(boProp.IsValid);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;

            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

        #endregion

        [Test]
        public void Test_InialiseProp_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            int expectedIntId = (int) _validBusinessObject.ID.GetAsValue();
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectedIntId);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
            Assert.AreEqual(expectedIntId, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InialiseProp_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InialiseProp_ValidintString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_int);
            int expectIntID = (int) _validBusinessObject.ID.GetAsValue();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(expectIntID.ToString());
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (int), boProp.Value);

            Assert.AreEqual(expectIntID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        //If try initialise a property with invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_Initialise_InvalidString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            const string invalid = "Invalid";
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.InitialiseProp(invalid);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to '" + invalid + "'", ex.Message);
                StringAssert.Contains("this value does not exist in the lookup list", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_InitialiseProp_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_SetValue_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            const int originalPropValue = 99;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = _validLookupValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }


        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_BOSetPropertyValue_InvalidString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_int.PropertyName];
            const string invalid = "Invalid";
            int originalPropValue = _validIntID;
            businessObject.SetPropertyValue(_propDef_int.PropertyName, originalPropValue);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(originalPropValue, boProp.Value);
            Assert.IsInstanceOfType(typeof (BOPropLookupList), boProp);
            Assert.IsTrue(boProp.IsValid);
            //---------------Execute Test ----------------------
            try
            {
                businessObject.SetPropertyValue(boProp.PropertyName, invalid);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroApplicationException ex)
            {
                //You are trying to set the value for a lookup property PropName to 'Invalid' this value does not exist in the lookup list
                StringAssert.Contains(boProp.PropertyName + " cannot be set to '" + invalid + "'", ex.Message);
                StringAssert.Contains("this value does not exist in the lookup list", ex.Message);
                Assert.AreEqual(originalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidIntString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_int.PropertyName];
            const int originalPropValue = 99;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validBusinessObject.ToString());
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidDisplayValueString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_int.PropertyName];
            const int originalPropValue = 99;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        private IBusinessObject GetBusinessObjectStub()
        {
            PropDefCol propDefCol = new PropDefCol {_propDef_int};

            PrimaryKeyDef def = new PrimaryKeyDef {_propDef_int};
            def.IsGuidObjectID = false;
            ClassDef classDef = new ClassDef(typeof (BusinessObjectStub), def, propDefCol, new KeyDefCol(), null);
            BusinessObjectStub businessObjectStub = new BusinessObjectStub(classDef);
            BOProp prop = new BOPropLookupList(_propDef_int);
            businessObjectStub.Props.Remove(prop.PropertyName);
            businessObjectStub.Props.Add(prop);
            return businessObjectStub;
        }

        [Test]
        public void TestPropertyValueToDisplay_BusinessObjectLookupList_NotInList()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_int.PropertyName];
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue"};
            string expectedPropValueToDisplay = bo1.ToString();
            bo1.IntID = 55;
            object expctedID = bo1.IntID;
            bo1.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            boProp.Value = expctedID;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(expctedID, boProp.Value);
            Assert.AreEqual(expectedPropValueToDisplay, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_GetBusinessObjectForProp()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOPropLookupList boProp = (BOPropLookupList) businessObject.Props[_propDef_int.PropertyName];
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue", IntID = 55};
            object expctedID = bo1.IntID;
            bo1.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            boProp.Value = expctedID;
            IBusinessObject objectForProp = boProp.GetBusinessObjectForProp(autoIncClassDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(objectForProp);
        }

        [Test]
        public void Test_GetBusinessObjectForProp_ID_WithDatabase()
        {
            ClassDef.ClassDefs.Clear();
            DatabaseConnection.CurrentConnection =
                    new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetDatabaseConfig().GetConnectionString();
            DatabaseConnection.CurrentConnection.GetConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
            BOWithIntID.DeleteAllBOWithIntID();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOPropLookupList boProp = (BOPropLookupList) businessObject.Props[_propDef_int.PropertyName];
            BOWithIntID bo1 = new BOWithIntID {TestField = "PropValue", IntID = 55};
            object expectedID = bo1.IntID;
            bo1.Save();
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            boProp.Value = expectedID;
            IBusinessObject objectForProp = boProp.GetBusinessObjectForProp(autoIncClassDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(objectForProp);
        }

        [Test]
        public void Test_InMemoryLoader_LoadWithIntID()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo1 = new BOWithIntID { TestField = "PropValue", IntID = 55 };
            bo1.Save();
            IPrimaryKey id = bo1.ID;
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo1.Status.IsNew);
            Assert.IsNotNull(bo1.IntID);
            //---------------Execute Test ----------------------
            BOWithIntID returnedBO = (BOWithIntID) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                                                       (autoIncClassDef, id);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnedBO);
        }
//        [Test, Ignore("Problem with In Memory Database")]
//        public void Test_Memory_LoadWithIntID_ManualCreatePrimaryKey()
//        {
//            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
//            BOWithIntID bo1 = new BOWithIntID { TestField = "PropValue", IntID = 55 };
//            bo1.Save();
//            IPrimaryKey id = BusinessObjectLoaderBase.GetRelatedBOPrimaryKeyByValue(autoIncClassDef, bo1.IntID);
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(bo1.Status.IsNew);
//            Assert.IsNotNull(bo1.IntID);
//            //---------------Execute Test ----------------------
//            BOWithIntID returnedBO = (BOWithIntID)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
//                                                       (autoIncClassDef, id);
//            //---------------Test Result -----------------------
//            Assert.IsNotNull(returnedBO);
//        }
//        [Test]
//        public void Test_DB_LoadWithIntID_ManualCreatePrimaryKey()
//        {
//            ClassDef.ClassDefs.Clear();
//            DatabaseConnection.CurrentConnection = new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
//            DatabaseConnection.CurrentConnection.ConnectionString = MyDBConnection.GetDatabaseConfig().GetConnectionString();
//            DatabaseConnection.CurrentConnection.GetConnection();
//            BORegistry.DataAccessor = new DataAccessorDB();
//            BOWithIntID.DeleteAllBOWithIntID();
//            ClassDef autoIncClassDef = BOWithIntID.LoadClassDefWithIntID();
//            BOWithIntID bo1 = new BOWithIntID { TestField = "PropValue", IntID = 55 };
//            bo1.Save();
//            IPrimaryKey id = BusinessObjectLoaderBase.GetRelatedBOPrimaryKeyByValue(autoIncClassDef, bo1.IntID);
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(bo1.Status.IsNew);
//            Assert.IsNotNull(bo1.IntID);
//            //---------------Execute Test ----------------------
//            BOWithIntID returnedBO = (BOWithIntID)BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
//                                                       (autoIncClassDef, id);
//            //---------------Test Result -----------------------
//            Assert.IsNotNull(returnedBO);
//        }
    }

    internal class BusinessObjectLookupListStub : BusinessObjectLookupList
    {
        private readonly IBusinessObjectCollection _boCollection;

        public BusinessObjectLookupListStub(Type boType, IBusinessObjectCollection boCollection) : base(boType)
        {
            _boCollection = boCollection;
        }

        public override IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _boCollection;
        }
    }
}