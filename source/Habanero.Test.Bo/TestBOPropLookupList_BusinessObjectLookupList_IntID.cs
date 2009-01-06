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
    public class TestBOPropLookupList_BusinessObjectLookupList_IntID
    {
        private const string _validLookupValue = "ValidValue";

        private PropDef _propDef_int;
        private BusinessObjectCollection<TestAutoInc> _collection_IntId;
        private TestAutoInc _validBusinessObject;
        private int _validIntID;


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
            TestAutoInc.LoadClassDefWithIntID();
            BORegistry.DataAccessor = new DataAccessorInMemory();


            _propDef_int = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            _validBusinessObject = new TestAutoInc {TestField = _validLookupValue};
            _validIntID = 3;
            _validBusinessObject.TestAutoIncID = _validIntID;
            _collection_IntId = new BusinessObjectCollection<TestAutoInc> {_validBusinessObject};

            _propDef_int.LookupList = new BusinessObjectLookupListStub(typeof (TestAutoInc), _collection_IntId);
        }

        #region Lookup List Int

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_Exists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<string, object> list = businessObjectLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            object returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_NotExists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
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
        public void Test_BusinessObjectLookupList_GetValue_Exists_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Int_WithLookupList();
            BusinessObjectLookupList businessObjectLookupList = (BusinessObjectLookupList) propDef.LookupList;
            Dictionary<object, string> list = businessObjectLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validIntID, out returnedValue);
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
            Dictionary<object, string> list = businessObjectLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validBusinessObject.ID.GetAsValue(), out returnedValue);
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
            Dictionary<object, string> list = businessObjectLookupList.GetKeyLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (BusinessObjectLookupList), propDef.LookupList);
            Assert.AreSame(propDef, businessObjectLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(5, out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }

        private PropDef GetPropDef_Int_WithLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            BusinessObjectLookupList businessObjectLookupList = new BusinessObjectLookupListStub
                (typeof (TestAutoInc), _collection_IntId);
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
            Assert.AreEqual(_validBusinessObject.TestAutoIncID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.TestAutoIncID, boProp.Value);
            Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
            Assert.AreEqual(_validLookupValue, propertyValueToDisplay);
        }

        [Test]
        public void Test_BOPropLookupList_Int_PropValueToDisplay_ValidInt()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Int_WithLookupList());
            boProp.InitialiseProp(_validBusinessObject.TestAutoIncID);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(_validBusinessObject.TestAutoIncID, boProp.Value);
            //---------------Execute Test ----------------------
            object propertyValueToDisplay = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validBusinessObject.TestAutoIncID, boProp.Value);
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
            Assert.IsTrue(boProp.IsValid);

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
                    Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
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
                    const int origionalPropValue = 99;
                    boProp.Value = origionalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
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
                    const int origionalPropValue = 99;
                    businessObject.SetPropertyValue(_propDef_int.PropertyName, origionalPropValue);
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
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
                        StringAssert.Contains(boProp.PropertyName + " cannot be set to '" + invalid + "'", ex.Message);
                        StringAssert.Contains("this value does not exist in the lookup list", ex.Message);
                        Assert.AreEqual(origionalPropValue, boProp.Value);
                        Assert.IsTrue(boProp.IsValid);
                    }
                }
        
                [Test]
                public void Test_BOSetPropertyValue_ValidIntString()
                {
                    IBusinessObject businessObject = GetBusinessObjectStub();
                    BOProp boProp = (BOProp)businessObject.Props[_propDef_int.PropertyName];
                    const int origionalPropValue = 99;
                    boProp.Value = origionalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
                    Assert.IsNotNull(boProp.Value);
                    //---------------Execute Test ----------------------
                    businessObject.SetPropertyValue(boProp.PropertyName, _validBusinessObject.ToString());
                    //---------------Test Result -----------------------
                    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
                    Assert.IsInstanceOfType(typeof(int), boProp.Value);
                    Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
                }
        
                [Test]
                public void Test_BOSetPropertyValue_ValidDisplayValueString()
                {
                    IBusinessObject businessObject = GetBusinessObjectStub();
                    BOProp boProp = (BOProp)businessObject.Props[_propDef_int.PropertyName];
                    const int origionalPropValue = 99;
                    boProp.Value = origionalPropValue;
                    //---------------Assert Precondition----------------
                    Assert.AreEqual(typeof(int), boProp.PropDef.PropertyType);
                    Assert.IsNotNull(boProp.Value);
                    //---------------Execute Test ----------------------
                    businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
                    //---------------Test Result -----------------------
                    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
                    Assert.IsInstanceOfType(typeof(int), boProp.Value);
                    Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
                }
        
                private IBusinessObject GetBusinessObjectStub()
                {
                    PropDefCol propDefCol = new PropDefCol {_propDef_int};
        
                    PrimaryKeyDef def = new PrimaryKeyDef {_propDef_int};
                    def.IsGuidObjectID = false;
                    ClassDef classDef = new ClassDef(typeof(BusinessObjectStub), def, propDefCol, new KeyDefCol(), null);
                    BusinessObjectStub businessObjectStub = new BusinessObjectStub(classDef);
                    BOProp prop = new BOPropLookupList(_propDef_int);
                    businessObjectStub.Props.Remove(prop.PropertyName);
                    businessObjectStub.Props.Add(prop);
                    return businessObjectStub;
                }
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