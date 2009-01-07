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
    public class TestBOPropLookupList_SimpleLookupList
    {
//        private PropDef _propDef_int;
        private PropDef _propDef_guid;
        private readonly Guid _validGuid = Guid.NewGuid();
        private Dictionary<string, object> _collection;
        private Dictionary<string, object> _collection_GuidString;
        private const string _validLookupValue = "ValidValue";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
//            _propDef_int = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            _propDef_guid = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            _collection = new Dictionary<string, object>
                              {
                                  {_validLookupValue, _validGuid},
                                  {"Another Value", Guid.NewGuid()}
                              };
            _collection_GuidString = new Dictionary<string, object>
                              {
                                  {_validLookupValue, _validGuid.ToString()},
                                  {"Another Value", Guid.NewGuid()}
                              };
            _propDef_guid.LookupList = new SimpleLookupList(_collection);
        }

        [Test]
        public void Test_SetLookupListForPropDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(null);

            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(NullLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = simpleLookupList;
            //---------------Test Result -----------------------
            Assert.IsNotNull(propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);

        }

        [Test]
        public void Test_NoPropDefSet_ThrowsError()
        {
            //---------------Set up test pack-------------------
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                simpleLookupList.GetKeyLookupList();
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (Exception ex)
            {
                StringAssert.Contains("There is an application setup error. There is no propdef set for the simple lookup list.", ex.Message);
            }
            //---------------Test Result -----------------------

        }

        [Test]
        public void Test_SimpleLookup_Create_SetsUpKeyLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, _collection.Count);
            //---------------Execute Test ----------------------
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            simpleLookupList.PropDef = propDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(_collection.Count, simpleLookupList.GetLookupList().Count);
            Assert.IsNotNull(simpleLookupList.GetKeyLookupList());
            Assert.AreEqual(2, simpleLookupList.GetKeyLookupList().Count);
        }

        [Test]
        public void Test_SetLookupList_SetsUpKeyLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(NullLookupList), propDef.LookupList);
            Assert.AreEqual(_collection.Count, simpleLookupList.GetLookupList().Count);
            //---------------Execute Test ----------------------
            propDef.LookupList = simpleLookupList;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            Assert.AreEqual(_collection.Count, simpleLookupList.GetLookupList().Count);
            Assert.AreEqual(_collection.Count, simpleLookupList.GetKeyLookupList().Count);
        }
        [Test]
        public void Test_SimpleLookupList_GetKey_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            propDef.LookupList = simpleLookupList;
            Dictionary<string, object> list = simpleLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validGuid, returnedKey);
        }
        [Test]
        public void Test_SimpleLookupList_GetKey_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            propDef.LookupList = simpleLookupList;
            Dictionary<string, object> list = simpleLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue("InvalidValue", out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedKey);
        }

        [Test]
        public void Test_SimpleLookupList_GetValue_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            propDef.LookupList = simpleLookupList;
            Dictionary< object, string> list = simpleLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validGuid, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }
        [Test]
        public void Test_SimpleLookupList_GetValue_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            SimpleLookupList simpleLookupList = new SimpleLookupList(_collection);
            propDef.LookupList = simpleLookupList;
            Dictionary<object, string> list = simpleLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(Guid.NewGuid(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }
        [Test]
        public void Test_BOPropLookupList_CreateNoLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            //---------------Assert Precondition----------------
            Assert.IsFalse(def.HasLookupList());
            //---------------Execute Test ----------------------
            try
            {
                new BOPropLookupList(def);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The application tried to configure a BOPropLookupList - with the propDef", ex.DeveloperMessage);
                StringAssert.Contains("does not have a lookup list defined", ex.DeveloperMessage);
            }
        }
        [Test]
        public void Test_BOPropLookupList_CreateNoLookupList_DefaultConstructor()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            //---------------Assert Precondition----------------
            Assert.IsFalse(def.HasLookupList());
            //---------------Execute Test ----------------------
            try
            {
                new BOPropLookupList(def, Guid.NewGuid());
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The application tried to configure a BOPropLookupList - with the propDef", ex.DeveloperMessage);
                StringAssert.Contains("does not have a lookup list defined", ex.DeveloperMessage);
            }
        }
        [Test]
        public void Test_BOPropLookupList_CreateNullPropDef()
        {
            //---------------Set up test pack-------------------
            const PropDef def = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(def);
            //---------------Execute Test ----------------------
            try
            {
                new BOPropLookupList(def);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propDef", ex.ParamName);
            }
        }
        [Test]
        public void Test_BOPropLookupList_CreateNullPropDef_Alt()
        {
            //---------------Set up test pack-------------------
            const PropDef def = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(def);
            //---------------Execute Test ----------------------
            try
            {
                new BOPropLookupList(def, Guid.NewGuid());
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("propDef", ex.ParamName);
            }
        }
        [Test]
        public void Test_BOPropLookupList_CreateWithLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null)
                              {LookupList = new SimpleLookupList(_collection)};
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
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            boProp.InitialiseProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }
        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            boProp.InitialiseProp(_validGuid);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validGuid, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }
        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_InvalidGuid()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            Guid invalidGuid = Guid.NewGuid();
            boProp.InitialiseProp(invalidGuid);

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(invalidGuid, boProp.Value);

            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;

            //---------------Test Result -----------------------
            Assert.IsNull( propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidGuidStringLookUpList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            propDef.LookupList = new SimpleLookupList(_collection_GuidString);
            BOProp boProp = new BOPropLookupList(propDef);
            Guid guid = _validGuid;
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid);
            //---------------Test Result -----------------------
            Assert.AreEqual(guid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_P()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            Guid guid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid.ToString("P"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual(guid, boProp.Value);
        }


        #region Int Lookup list

        [Test]
        public void Test_SimpleLookupList_Int_String_GetKey_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object>
                                                            {{_validLookupValue, validInt.ToString()}};

            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;
            Dictionary<string, object> list = simpleLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(validInt.ToString(), returnedKey);
        }

        [Test]
        public void Test_SimpleLookupList_Int_GetValue_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object> { { _validLookupValue, validInt.ToString() } };
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;
            Dictionary<object, string> list = simpleLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof(SimpleLookupList), propDef.LookupList);
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(validInt, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_SimpleLookupList_Int_GuidPropType()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object> { { _validLookupValue, validInt.ToString() } };
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;
            
            //---------------Assert Precondition----------------
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            try
            {
                simpleLookupList.GetKeyLookupList();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There is an application setup error Please contact your system administrator", ex.Message);
                StringAssert.Contains("There is a class definition setup error the simple lookup list has lookup value items that are not of type", ex.DeveloperMessage);
                StringAssert.Contains("Guid", ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_SimpleLookupList_Int_GetValue_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object> { { _validLookupValue, validInt.ToString() } };
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;
            Dictionary<object, string> list = simpleLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.AreSame(propDef, simpleLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(3, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_ValidInt()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object> { { _validLookupValue, validInt.ToString() } };
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;

            BOProp boProp = new BOPropLookupList(propDef);
            boProp.InitialiseProp(validInt);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(validInt, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(validInt, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }
        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_InvalidInt()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(int), PropReadWriteRule.ReadWrite, null);
            const int validInt = 1;
            Dictionary<string, object> collection_int = new Dictionary<string, object> { { _validLookupValue, validInt.ToString() } };
            SimpleLookupList simpleLookupList = new SimpleLookupList(collection_int);
            propDef.LookupList = simpleLookupList;

            BOProp boProp = new BOPropLookupList(propDef);
            boProp.InitialiseProp(3);

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(3, boProp.Value);

            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;

            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
            //TODO Brett: Should we return the invalid value or maybe a string saying invalid value or something
        }
        #endregion

        [Test]
        public void Test_InialiseProp_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validGuid);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }
        [Test]
        public void Test_InialiseProp_ValidGuidString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validGuid.ToString());
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        //If try initialise a property with invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_Initialise_InvalidString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            const string invalid = "Invalid";
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
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
        public void Test_InialiseProp_DBNUll()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(DBNull.Value);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InialiseProp_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }
        [Test]
        public void Test_InitialiseProp_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_SetValue_InvalidString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            const string invalid = "Invalid";
            object origionalPropValue = Guid.NewGuid();
            boProp.Value = origionalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
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
                Assert.AreEqual(origionalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }
        [Test]
        public void Test_SetValue_ValidGuidString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            object origionalPropValue = Guid.NewGuid();
            boProp.Value = origionalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = _validGuid.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_SetValue_ValidDisplayValueString()
        {
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            object origionalPropValue = Guid.NewGuid();
            boProp.Value = origionalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = _validLookupValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_SetValue_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Execute Test ----------------------
            boProp.Value = Guid.Empty;
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            Assert.IsNull(boProp.PropertyValueToDisplay);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test, Ignore("//TODO Brett: This will work when remove legacy code from setproperty value")]
        public void Test_BOSetPropertyValue_InvalidString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_guid.PropertyName];
            const string invalid = "Invalid";
            object origionalPropValue = Guid.NewGuid();
            businessObject.SetPropertyValue(_propDef_guid.PropertyName, origionalPropValue);

            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(origionalPropValue, boProp.Value);
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
                //TODO Brett: Waiting for removing This functionality from BusinessObject.
                StringAssert.Contains(boProp.PropertyName + " cannot be set to '" + invalid + "'", ex.Message);
                StringAssert.Contains("this value does not exist in the lookup list", ex.Message);
                Assert.AreEqual(origionalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidGuidString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp)businessObject.Props[_propDef_guid.PropertyName];
            object origionalPropValue = Guid.NewGuid();
            boProp.Value = origionalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validGuid.ToString());
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidDisplayValueString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp)businessObject.Props[_propDef_guid.PropertyName];
            object origionalPropValue = Guid.NewGuid();
            boProp.Value = origionalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validGuid, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        private IBusinessObject GetBusinessObjectStub()
        {
            PropDefCol propDefCol = new PropDefCol {_propDef_guid};

            PrimaryKeyDef def = new PrimaryKeyDef {_propDef_guid};
            ClassDef classDef = new ClassDef(typeof(BusinessObjectStub), def, propDefCol, new KeyDefCol(), null);
            BusinessObjectStub businessObjectStub = new BusinessObjectStub(classDef);
            BOProp prop = new BOPropLookupList(_propDef_guid);
            businessObjectStub.Props.Remove(prop.PropertyName);
            businessObjectStub.Props.Add(prop);
            return businessObjectStub;
        }
    }

    internal class BusinessObjectStub : BusinessObject
    {
        public BusinessObjectStub(ClassDef def):base(def)
        {
        }
    }
}