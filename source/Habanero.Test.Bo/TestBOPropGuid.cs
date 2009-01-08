using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOPropGuid
    {
        private PropDef _propDef;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefsNoUIDef();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);
        }

//        [SetUp]
//        public void TestSetup()
//        {
//            
//        }
        [Test]
        public void Test_InitialiseProp_NullValue()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(null);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid value = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(value);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(value, boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_InitialiseProp_EmptyGuidString_B()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid guid = Guid.Empty;
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid.ToString("B"));
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_B()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid guid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid.ToString("B"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.AreEqual(guid, boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_N()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid guid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid.ToString("N"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual(guid, boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_D()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid guid = Guid.NewGuid();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(guid.ToString("D"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual(guid, boProp.Value);
        }

        [Test]
        public void Test_InitialiseProp_ValidGuidString_P()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
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

        [Test]
        public void Test_InitialiseProp_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            MyBO bo = new MyBO();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(bo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual(bo.MyBoID, boProp.Value);
        }

        [Test, Ignore("Currently throws an error while trying to get the object id")]
        public void Test_InitialiseProp_InValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            MyBO bo = new MyBO();
            bo.SetPropertyValue("MyBoID", null);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(bo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid, "Value should be a guid");
            Assert.AreEqual(bo.MyBoID, boProp.Value);
        }

        [Test]
        public void Test_InialiseProp_DBNUll()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(DBNull.Value);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_InialiseProp_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.InitialiseProp(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_InitialiseProp_InvalidBusinessObject_IDNotGuid()
        {
            //---------------Set up test pack-------------------
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            BOProp boProp = new BOProp(_propDef);
            //Use auto incrementing because it is the only bo prop that has 
            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testautoincid", 1);
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.InitialiseProp(bo);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + bo.ToString(), ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("Guid", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        //If try initialise a property with invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not changed to be in an invalid state. The prop invalid reason is not set.
        [Test]
        public void Test_Initialise_InvalidGuidString()
        {
            BOProp boProp = new BOProp(_propDef);
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
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("Guid", ex.Message);
                Assert.AreEqual(null, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        //If try construct a property with invalid property default value type then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not set to be invalid.
        [Test]
        public void Test_ConstructWithDefault_InvalidGuidString()
        {
            string invalid = "";
            //---------------Assert Precondition----------------
            Assert.AreEqual(typeof (Guid), _propDef.PropertyType);
            //---------------Execute Test ----------------------
            try
            {
                invalid = "Invalid";
                new BOProp(_propDef, invalid);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(" cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("Guid", ex.Message);
            }
        }

        [Test]
        public void Test_SetValue_DBNull()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Execute Test ----------------------
            boProp.Value = DBNull.Value;
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Execute Test ----------------------
            boProp.Value = null;
            //---------------Test Result -----------------------
            Assert.IsTrue(boProp.IsValid);
            Assert.IsNull(boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            //---------------Execute Test ----------------------
            boProp.Value = Guid.Empty;
            //---------------Test Result -----------------------
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedValue = Guid.NewGuid();
            //---------------Assert Precondition ---------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = expectedValue;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid);
            Assert.AreEqual(expectedValue, boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void Test_SetValue_ValidGuidString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedValue = Guid.NewGuid();
            //---------------Assert Precondition ---------------
            Assert.IsNull(boProp.Value);
            //---------------Execute Test ----------------------
            boProp.Value = expectedValue.ToString("P");
            //---------------Test Result -----------------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid);
            Assert.AreEqual(expectedValue, boProp.Value);
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        //If an invalid property types is set to the property then
        //  An error is raised. Stating the error reason.
        //  The property value will be set to the previous property value.
        //  The property is not set to be invalid.
        [Test]
        public void Test_SetValue_InvalidString_RaiseError()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            const string invalid = "Invalid";
            object originalPropValue = Guid.NewGuid();
            boProp.Value = originalPropValue;
            //---------------Assert Precondition ---------------
            Assert.AreEqual(typeof (Guid), _propDef.PropertyType);
            Assert.IsNotNull(boProp.Value);
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = invalid; //expectedGuid.ToString("B");
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(boProp.PropertyName + " cannot be set to " + invalid, ex.Message);
                StringAssert.Contains("It is not a type of ", ex.Message);
                StringAssert.Contains("Guid", ex.Message);
                Assert.AreEqual(originalPropValue, boProp.Value);
                Assert.IsTrue(boProp.IsValid);
            }
        }

        [Test]
        public void Test_PersistedPropertyValueString_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            boProp.InitialiseProp(DBNull.Value);
            boProp.Value = Guid.NewGuid();

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual("", persistedPropertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PersistedPropertyValueString_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            boProp.InitialiseProp(expectedGuid);
            boProp.Value = Guid.NewGuid();

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsNotNull(boProp.PersistedPropertyValue);

            //---------------Execute Test ----------------------
            string persistedPropertyValueString = boProp.PersistedPropertyValueString;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), persistedPropertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_Null()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            boProp.InitialiseProp(DBNull.Value);

            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual("", propertyValueString, "Null persisted prop value should return null string");
        }

        [Test]
        public void Test_PropertyValueString_ValidGuid()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
            boProp.InitialiseProp(expectedGuid);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);

            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), propertyValueString);
        }

        [Test]
        public void Test_PropertyValueString_ValidGuidString()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = new BOProp(_propDef);
            Guid expectedGuid = Guid.NewGuid();
//            boProp.InitialiseProp(expectedGuid.ToString("B"));
            boProp.Value = expectedGuid.ToString("B");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boProp.Value);
            Assert.IsTrue(boProp.Value is Guid);
            //---------------Execute Test ----------------------
            string propertyValueString = boProp.PropertyValueString;

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), propertyValueString);
        }
    }
}