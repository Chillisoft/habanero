using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataMapper_Bool
    {
        private PropDef _propDef;
        private BOPropBoolDataMapper _dataMapper;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (bool), PropReadWriteRule.ReadWrite, null);
            _dataMapper = new BOPropBoolDataMapper();
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromInValidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(invalidString);

            //---------------Test Result -----------------------
            Assert.AreEqual(invalidString, parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _propDef.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
        }
        [Test]
        public void Test_PropDef_ParsePropValue_EmptyString()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parseSucceed = _propDef.TryParsePropValue("", out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parseSucceed);
        }
        [Test]
        public void Test_PropDef_ParsePropValue_FromValidSring_False()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "FAlse";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _propDef.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
            Assert.IsTrue(parsedOK);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _propDef.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsFalse(parsedOK);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _propDef.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
        }


        [Test]
        public void Test_PropDef_ConvertValueToString_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(null);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "true";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            object parsedValue = _propDef.ConvertValueToString(expectedBoolString);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedBoolString, parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(false);

            //---------------Test Result -----------------------
            Assert.AreEqual("False", parsedValue);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromValidSring_true()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "true";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _propDef.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSuccess = _dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSuccess);
        }


        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_true()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "true";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_TRUE()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "TRUE";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
            Assert.IsTrue(parsedOK);

        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_T()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "t";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_Yes()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "Yes";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_Y()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "Y";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_1()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "1";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_Neg1()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "-1";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_False()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "FAlse";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_F()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "f";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_No()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "No";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
            Assert.IsTrue(parsedOK);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_N()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "N";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
            Assert.IsTrue(parsedOK);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_0()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "0";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(false, parsedValue);
            Assert.IsTrue(parsedOK);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring_NonZero()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "3";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(expectedBoolString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(null, parsedValue);
            Assert.IsFalse(parsedOK);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedOK = _dataMapper.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsFalse(parsedOK);
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            _dataMapper.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(null);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const string expectedBoolString = "true";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            object parsedValue = _dataMapper.ConvertValueToString(expectedBoolString);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedBoolString, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(false);

            //---------------Test Result -----------------------
            Assert.AreEqual("False", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_True()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(true);

            //---------------Test Result -----------------------
            Assert.AreEqual("True", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromInValidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(invalidString);

            //---------------Test Result -----------------------
            Assert.AreEqual(invalidString, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue = _dataMapper.ConvertValueToString(dbNullValue);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }
    }
}