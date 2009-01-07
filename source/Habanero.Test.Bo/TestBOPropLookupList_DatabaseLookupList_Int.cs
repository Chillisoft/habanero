using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropLookupList_DatabaseLookupList_Int : TestUsingDatabase
    {
        private PropDef _propDef_int;
        private const int _intKeyDoesNotExistInList = 99;

        private const int _validID = 7;
        private const string _validLookupValue = "TestInt7";
        private const string _sql = "select LookupID, LookupValue from database_lookup_int";
        private const int _noOfRowsInDatabase = 2;

        //TODO: test where the propDef is string how to validate etc worried that the data
        // type of the lookup value and the ID value is the same what happens.
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            this.SetupDBConnection();

            BORegistry.DataAccessor = new DataAccessorDB();
            _propDef_int = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            _propDef_int.LookupList = databaseLookupList;
            databaseLookupList.GetLookupList();
        }


        //TODO :: If prop.value is set to a value of the appropriate type but is not in the list then the
        //   property must be set to be in an invalid state with the appropriate reason.
        //  Do same for simple lookup list
        // TODO: Test BusinessObject is being constructed with appropriate BOPropLookupList
        [Test]
        public void Test_SetLookupListForPropDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);

            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (NullLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = databaseLookupList;
            //---------------Test Result -----------------------
            Assert.IsNotNull(propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
        }

        [Test]
        public void Test_SimpleLookup_Create_SetsUpKeyLookupList()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DatabaseLookupList databaseLookupList = GetDatabaseLookupList();
            //---------------Assert Precondition----------------
            Assert.AreEqual(_noOfRowsInDatabase, databaseLookupList.GetLookupList().Count);
            Assert.IsNotNull(databaseLookupList.GetKeyLookupList());
            Assert.AreEqual(_noOfRowsInDatabase, databaseLookupList.GetKeyLookupList().Count);
        }

        private DatabaseLookupList GetDatabaseLookupList()
        {
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            _propDef_int.LookupList = databaseLookupList;
            return databaseLookupList;
        }

        [Test]
        public void Test_SetLookupList_SetsUpKeyLookupList()
        {
            //---------------Set up test pack------------------

            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (NullLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = databaseLookupList;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            Assert.AreEqual(_noOfRowsInDatabase, databaseLookupList.GetLookupList().Count);
            Assert.AreEqual(_noOfRowsInDatabase, databaseLookupList.GetKeyLookupList().Count);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_Exists()
        {
            //---------------Set up test pack-------------------
            DatabaseLookupList databaseLookupList;
            PropDef propDef = GetPropDef_WithDatabaseLookupList(out databaseLookupList);
            Dictionary<string, object> list = databaseLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            Assert.AreEqual(_noOfRowsInDatabase, databaseLookupList.GetLookupList().Count);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validID, returnedKey);
        }

        private static PropDef GetPropDef_WithDatabaseLookupList(out DatabaseLookupList databaseLookupList)
        {
            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            return propDef;
        }

        [Test]
        public void Test_GetLookupList_LookupListIncorrectType()
        {
            //---------------Set up test pack------------------

            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(DatabaseLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            try
            {
                databaseLookupList.GetLookupList();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There is an application setup error Please contact your system administrator", ex.Message);
                StringAssert.Contains("There is a class definition setup error the database lookup list has lookup value items that are not of type", ex.DeveloperMessage);
                StringAssert.Contains("Guid", ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            Dictionary<string, object> list = databaseLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue("InvalidValue", out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            databaseLookupList.GetLookupList();
            Dictionary<object, string> list = databaseLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validID, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_int_WithLookupList();
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            databaseLookupList.GetLookupList();
            Dictionary<object, string> list = databaseLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_intKeyDoesNotExistInList, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }

        private static PropDef GetPropDef_int_WithLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            propDef.LookupList = new DatabaseLookupList(_sql);
            propDef.LookupList.GetLookupList();
            propDef.LookupList.GetKeyLookupList();
            return propDef;
        }

        [Test]
        public void Test_BOPropLookupList_CreateWithLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null) { LookupList = new DatabaseLookupList(_sql) };
            //---------------Assert Precondition----------------
            Assert.IsTrue(def.HasLookupList());
            //---------------Execute Test ----------------------
            BOPropLookupList boPropLookupList = new BOPropLookupList(def);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boPropLookupList);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_NullValue()
        {
            //---------------Set up test pack-------------------

            BOProp boProp = new BOPropLookupList(GetPropDef_int_WithLookupList());
            boProp.InitialiseProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_InitialiseProp_ValidID()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_int_WithLookupList());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validID);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.IsInstanceOfType(typeof(int), boProp.Value);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidID()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_int_WithLookupList());
            boProp.InitialiseProp(_validID);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        //[Test]
        //public void Test_BOPropLookupList_PropValueToDisplay_Validint()
        //{
        //    //---------------Set up test pack-------------------
        //    BOProp boProp = new BOPropLookupList(GetPropDef_int_WithLookupList());
        //    boProp.InitialiseProp(_validBusinessObject.MyBoID);
        //    //---------------Assert Precondition----------------
        //    Assert.IsNotNull(boProp.Value);
        //    Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
        //    //---------------Execute Test ----------------------
        //    object propertyValueToDisplay = boProp.PropertyValueToDisplay;
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
        //    Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        //}

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_Invalidint()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_int_WithLookupList());
            boProp.InitialiseProp(_intKeyDoesNotExistInList);

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_intKeyDoesNotExistInList, boProp.Value);
            Assert.IsTrue(boProp.IsValid);

            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;

            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

        [Test]
        public void Test_InialiseProp_Validint()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validID);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof(int), boProp.Value);
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InialiseProp_ValidintString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validID.ToString());
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (int), boProp.Value);
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InitialiseProp_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_SetValue_InvalidString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            const string invalid = "Invalid";
            object originalPropValue = _validID;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = invalid;
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
        public void Test_SetValue_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_int);
            object originalPropValue = _intKeyDoesNotExistInList;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = _validLookupValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID , boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        #region Business Object SetPropertyValueTests.

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_BOSetPropertyValue_InvalidString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp)businessObject.Props[_propDef_int.PropertyName];
            const string invalid = "Invalid";
            object originalPropValue = _intKeyDoesNotExistInList;
            businessObject.SetPropertyValue(_propDef_int.PropertyName, originalPropValue);

            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(originalPropValue, boProp.Value);
            Assert.IsInstanceOfType(typeof(BOPropLookupList), boProp);
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
            BOProp boProp = (BOProp)businessObject.Props[_propDef_int.PropertyName];
            object originalPropValue = _validID;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validID.ToString());
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidDisplayValueString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp)businessObject.Props[_propDef_int.PropertyName];
            object originalPropValue = _intKeyDoesNotExistInList;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
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

        #endregion


        //#endregion
    }
}