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
    public class TestBOPropLookupList_BusinessObjectLookupList_GuidID
    {
        private PropDef _propDef_guid;
        private IBusinessObjectCollection _collection;
        private MyBO _validBusinessObject;
        private string _validLookupValue;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            _propDef_guid = new PropDef("PropName", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            _validBusinessObject = new MyBO {TestProp = "ValidValue"};
            _collection = new BusinessObjectCollection<MyBO> {_validBusinessObject};
            _validLookupValue = _validBusinessObject.ToString();

            _propDef_guid.LookupList = new BusinessObjectLookupListStub(typeof (MyBO), _collection);
        }

        //TODO :: If prop.value is set to a value of the appropriate type but is not in the list then the
        //   property must be set to be in an invalid state with the appropriate reason.
        //  Do same for simple lookup list
        // TODO: Test BusinessObject is being constructed with appropriate BOPropLookupList
        [Test]
        public void Test_SetLookupListForPropDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (NullLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = businessObjectLookupList;
            //---------------Test Result -----------------------
            Assert.IsNotNull(propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
        }

        [Test]
        public void Test_SimpleLookup_Create_SetsUpKeyLookupList()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, _collection.Count);
            //---------------Execute Test ----------------------
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            //---------------Assert Precondition----------------
            Assert.AreEqual(_collection.Count, businessObjectLookupList.GetLookupList().Count);
            Assert.IsNotNull(businessObjectLookupList.GetKeyLookupList());
            Assert.AreEqual(_collection.Count, businessObjectLookupList.GetKeyLookupList().Count);
        }

        [Test]
        public void Test_SetLookupList_SetsUpKeyLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (NullLookupList), propDef.LookupList);
            Assert.AreEqual(_collection.Count, businessObjectLookupList.GetLookupList().Count);
            //---------------Execute Test ----------------------
            propDef.LookupList = businessObjectLookupList;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            Assert.AreEqual(_collection.Count, businessObjectLookupList.GetLookupList().Count);
            Assert.AreEqual(_collection.Count, businessObjectLookupList.GetKeyLookupList().Count);
        }

        #region LookupListGuid

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            propDef.LookupList = businessObjectLookupList;
            Dictionary<string, object> list = businessObjectLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validBusinessObject.ID.GetAsGuid(), returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            propDef.LookupList = businessObjectLookupList;
            Dictionary<string, object> list = businessObjectLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
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
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            propDef.LookupList = businessObjectLookupList;
            Dictionary<object, string> list = businessObjectLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validBusinessObject.ID.GetAsGuid(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Guid_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<object, string> list = businessObjectLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(Guid.NewGuid(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }

        private PropDef GetPropDef_Guid_WithLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (MyBO), _collection);
            propDef.LookupList = businessObjectLookupList;
            return propDef;
        }

        [Test]
        public void Test_BOPropLookupList_CreateWithLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null)
                              {LookupList = new BusinessObjectLookupListStub(typeof (MyBO), _collection)};
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

            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
            boProp.InitialiseProp(null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_InitialiseProp_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
            Assert.IsInstanceOfType(typeof (Guid), boProp.Value);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
            boProp.InitialiseProp(_validBusinessObject.MyBoID);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.MyBoID, boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_InvalidGuid()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
            Guid guidNotInLookupList = Guid.NewGuid();
            boProp.InitialiseProp(guidNotInLookupList);

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(guidNotInLookupList, boProp.Value);
            Assert.IsTrue(boProp.IsValid);

            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;

            //---------------Test Result -----------------------
            Assert.IsNull(propertyValueToDisplay);
        }


        [Test]
        public void Test_InialiseProp_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validBusinessObject.ID.GetAsValue());
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof(Guid), boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        [Test]
        public void Test_InialiseProp_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validBusinessObject);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof(Guid), boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
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
            boProp.InitialiseProp(_validBusinessObject.ID.GetAsValue().ToString());
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(Guid), boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
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
                    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
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
                    object originalPropValue = Guid.NewGuid();
                    boProp.Value = originalPropValue;
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
                        Assert.AreEqual(originalPropValue, boProp.Value);
                        Assert.IsTrue(boProp.IsValid);
                    }
                }

                [Test]
                public void Test_SetValue_ValidDisplayValueString()
                {
                    BOProp boProp = new BOPropLookupList(_propDef_guid);
                    object originalPropValue = Guid.NewGuid();
                    boProp.Value = originalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
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
                [Test, Ignore("REmove previous logic in businessObject.SetPropertyValue")]
                public void Test_BOSetPropertyValue_InvalidString()
                {
                    IBusinessObject businessObject = GetBusinessObjectStub();
                    BOProp boProp = (BOProp) businessObject.Props[_propDef_guid.PropertyName];
                    const string invalid = "Invalid";
                    object originalPropValue = Guid.NewGuid();
                    businessObject.SetPropertyValue(_propDef_guid.PropertyName, originalPropValue);
        
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
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
                public void Test_BOSetPropertyValue_ValidGuidString()
                {
                    IBusinessObject businessObject = GetBusinessObjectStub();
                    BOProp boProp = (BOProp)businessObject.Props[_propDef_guid.PropertyName];
                    object originalPropValue = Guid.NewGuid();
                    boProp.Value = originalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
                    Assert.IsNotNull(boProp.Value);
                    //---------------Execute Test ----------------------
                    businessObject.SetPropertyValue(boProp.PropertyName, _validBusinessObject.ToString());
                    //---------------Test Result -----------------------
                    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
                    Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
                }
        
                [Test]
                public void Test_BOSetPropertyValue_ValidDisplayValueString()
                {
                    IBusinessObject businessObject = GetBusinessObjectStub();
                    BOProp boProp = (BOProp)businessObject.Props[_propDef_guid.PropertyName];
                    object originalPropValue = Guid.NewGuid();
                    boProp.Value = originalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(Guid), boProp.PropDef.PropertyType);
                    Assert.IsNotNull(boProp.Value);
                    //---------------Execute Test ----------------------
                    businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
                    //---------------Test Result -----------------------
                    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
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

        #endregion
    }
}