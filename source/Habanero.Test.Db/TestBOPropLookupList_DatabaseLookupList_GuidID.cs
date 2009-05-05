//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBOPropLookupList_DatabaseLookupList_GuidID : TestUsingDatabase
    {
        private PropDef _propDef_guid;
        private Guid _validID = new Guid("{6EAE79DD-11A8-4f31-8AF5-A08F22FE556E}");
        private const string _validLookupValue = "test2";
        private const string _sql = "select LookupID, LookupValue from database_lookup_guid";

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
            _propDef_guid = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql) {PropDef = _propDef_guid};
            databaseLookupList.GetLookupList();
            _propDef_guid.LookupList = databaseLookupList;
        }

        [Test]
        public void Test_GetKey_FromLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Dictionary<string, string> lookupList = databaseLookupList.GetLookupList();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, lookupList.Count, "There should be two item in the lookup list");
            Assert.IsTrue(lookupList.ContainsKey(_validLookupValue));
            string objectIDAsString = lookupList[_validLookupValue];
            Assert.AreEqual(_validID.ToString(), objectIDAsString);
        }

        private static string GuidToUpper(Guid guid)
        {
            return StringUtilities.GuidToUpper(guid);
        }

        [Test]
        public void Test_GetIDValueLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Dictionary<string, string> idValueLookupList = databaseLookupList.GetIDValueLookupList();
            //---------------Test Result -----------------------
            Assert.IsNotNull(idValueLookupList);
        }

        [Test]
        public void Test_GetValue_FromKeyValueList()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Dictionary<string, string> idValueLookupList = databaseLookupList.GetIDValueLookupList();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, idValueLookupList.Count, "There should be two item in the lookup list");
            string guidToString = _validID.ToString();
            Assert.IsTrue(idValueLookupList.ContainsKey(guidToString));
            string returnedValue = idValueLookupList[guidToString];
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        //TODO :: If prop.value is set to a value of the appropriate type but is not in the list then the
        //   property must be set to be in an invalid state with the appropriate reason.
        //  Do same for simple lookup list
        [Test]
        public void Test_SetLookupListForPropDef()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
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
            Assert.AreEqual(2, databaseLookupList.GetLookupList().Count);
            Assert.IsNotNull(databaseLookupList.GetIDValueLookupList());
            Assert.AreEqual(2, databaseLookupList.GetIDValueLookupList().Count);
        }

        private DatabaseLookupList GetDatabaseLookupList()
        {
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            _propDef_guid.LookupList = databaseLookupList;
            return databaseLookupList;
        }

        [Test]
        public void Test_SetLookupList_SetsUpKeyLookupList()
        {
            //---------------Set up test pack------------------

            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (NullLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            propDef.LookupList = databaseLookupList;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            Assert.AreEqual(2, databaseLookupList.GetLookupList().Count);
            Assert.AreEqual(2, databaseLookupList.GetIDValueLookupList().Count);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_Exists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            Dictionary<string, string> list = databaseLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedKey;
            bool keyReturned = list.TryGetValue(_validLookupValue, out returnedKey);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validID.ToString(), returnedKey);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetKey_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            Dictionary<string, string> list = databaseLookupList.GetLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedKey;
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
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            databaseLookupList.GetLookupList();
            Dictionary<string, string> list = databaseLookupList.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(_validID.ToString(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(keyReturned);
            Assert.AreEqual(_validLookupValue, returnedValue);
        }

        [Test]
        public void Test_BusinessObjectLookupList_GetValue_NotExists()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = GetPropDef_Guid_WithLookupList();
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            databaseLookupList.GetLookupList();
            Dictionary<string, string> list = databaseLookupList.GetIDValueLookupList();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            Assert.AreSame(propDef, databaseLookupList.PropDef);
            //---------------Execute Test ----------------------
            string returnedValue;
            bool keyReturned = list.TryGetValue(Guid.NewGuid().ToString("N").ToUpperInvariant(), out returnedValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(keyReturned);
            Assert.IsNull(returnedValue);
        }


        [Test]
        public void Test_GetLookupList_LookupListIncorrectType()
        {
            //---------------Set up test pack------------------

            PropDef propDef = new PropDef("PropName", typeof (int), PropReadWriteRule.ReadWrite, null);
            DatabaseLookupList databaseLookupList = new DatabaseLookupList(_sql);
            propDef.LookupList = databaseLookupList;
            //---------------Assert Precondition----------------
            Assert.IsInstanceOfType(typeof (DatabaseLookupList), propDef.LookupList);
            //---------------Execute Test ----------------------
            try
            {
                databaseLookupList.GetLookupList();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("There is an application setup error Please contact your system administrator", ex.Message);
                StringAssert.Contains
                    ("There is a class definition setup error the database lookup list has lookup value items that are not of type",
                     ex.DeveloperMessage);
                StringAssert.Contains("Int32", ex.DeveloperMessage);
            }
        }


        private static PropDef GetPropDef_Guid_WithLookupList()
        {
            PropDef propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            propDef.LookupList = new DatabaseLookupList(_sql, 10000, "", "", true);
            propDef.LookupList.GetLookupList();
            propDef.LookupList.GetIDValueLookupList();
            return propDef;
        }

        [Test]
        public void Test_BOPropLookupList_CreateWithLookupList()
        {
            //---------------Set up test pack-------------------
            PropDef def = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null)
                              {LookupList = new DatabaseLookupList(_sql)};
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
        public void Test_BOPropLookupList_InitialiseProp_ValidID()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validID);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.IsInstanceOfType(typeof (Guid), boProp.Value);
        }

        [Test]
        public void Test_BOPropLookupList_PropValueToDisplay_ValidID()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
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
        //public void Test_BOPropLookupList_PropValueToDisplay_ValidGuid()
        //{
        //    //---------------Set up test pack-------------------
        //    BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
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
        public void Test_BOPropLookupList_PropValueToDisplay_InvalidGuid()
        {
            //GetPropertyValueToDisplay where the guid value is not 
            // in the lookup list (should return null)
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(GetPropDef_Guid_WithLookupList());
            Guid guidNotInLookupList = Guid.NewGuid();
            boProp.InitialiseProp(guidNotInLookupList);
            boProp.Validate();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(guidNotInLookupList, boProp.Value);
            Assert.IsFalse(boProp.IsValid);

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
            boProp.InitialiseProp(_validID);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsInstanceOfType(typeof (Guid), boProp.Value);
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        //[Test]
        //public void Test_InialiseProp_ValidBusinessObject()
        //{
        //    //---------------Set up test pack-------------------
        //    BOProp boProp = new BOPropLookupList(_propDef_guid);
        //    //---------------Assert Precondition----------------
        //    Assert.IsNull(boProp.Value);
        //    //---------------Execute Test ----------------------
        //    boProp.InitialiseProp(_validBusinessObject);
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(boProp.Value);
        //    Assert.IsInstanceOfType(typeof(Guid), boProp.Value);
        //    Assert.AreEqual(_validBusinessObject.ID.GetAsValue(), boProp.Value);
        //    Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        //}

        [Test]
        public void Test_InialiseProp_ValidGuidString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(_validID.ToString());
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (Guid), boProp.Value);
            Assert.AreEqual(_validID, boProp.Value);
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
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
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
                StringAssert.Contains("this value cannot be converted to a System.Guid", ex.Message);
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
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
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
            BOProp boProp = new BOPropLookupList(_propDef_guid);
            const string invalid = "Invalid";
            object originalPropValue = _validID;
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
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
                StringAssert.Contains("this value cannot be converted to a System.Guid", ex.Message);
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
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = _validLookupValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
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
            BOProp boProp = (BOProp) businessObject.Props[_propDef_guid.PropertyName];
            const string invalid = "Invalid";
            object originalPropValue = _validID;
            businessObject.SetPropertyValue(_propDef_guid.PropertyName, originalPropValue);

            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
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
                //StringAssert.Contains
                //    ("You are trying to set the value for a lookup property " + boProp.PropertyName + " to '" + invalid + "'", ex.Message);
                //StringAssert.Contains("this value does not exist in the lookup list", ex.Message);
                StringAssert.Contains
                    (boProp.PropertyName + " cannot be set to '" + invalid
                     + "' this value cannot be converted to a System.Guid", ex.Message);
                Assert.AreEqual(originalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_BOSetPropertyValue_ValidGuidString()
        {
            IBusinessObject businessObject = GetBusinessObjectStub();
            BOProp boProp = (BOProp) businessObject.Props[_propDef_guid.PropertyName];
            object originalPropValue = Guid.NewGuid();
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
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
            BOProp boProp = (BOProp) businessObject.Props[_propDef_guid.PropertyName];
            object originalPropValue = Guid.NewGuid();
            boProp.Value = originalPropValue;
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (Guid), boProp.PropDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            businessObject.SetPropertyValue(boProp.PropertyName, _validLookupValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(_validID, boProp.Value);
            Assert.AreEqual(_validLookupValue, boProp.PropertyValueToDisplay);
        }

        private IBusinessObject GetBusinessObjectStub()
        {
            PropDefCol propDefCol = new PropDefCol {_propDef_guid};

            PrimaryKeyDef def = new PrimaryKeyDef {_propDef_guid};
            ClassDef classDef = new ClassDef(typeof (BusinessObjectStub), def, propDefCol, new KeyDefCol(), null);
            BusinessObjectStub businessObjectStub = new BusinessObjectStub(classDef);
            BOProp prop = new BOPropLookupList(_propDef_guid);
            businessObjectStub.Props.Remove(prop.PropertyName);
            businessObjectStub.Props.Add(prop);
            return businessObjectStub;
        }

        //#endregion
    }
}