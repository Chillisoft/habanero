using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestDataMapper_Guid
    {
        private PropDef _propDef;
        private BOPropGuidDataMapper _dataMapper;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            _propDef = new PropDef("PropName", typeof (Guid), PropReadWriteRule.ReadWrite, null);

            _dataMapper = new BOPropGuidDataMapper();
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
        public void Test_PropDef_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(expectedGuid.ToString("B"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString_EmptyGuidString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedGuid.ToString("B"));

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
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
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_PropDef_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_Null()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(null, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid.ToString("B"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid, out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_EmptyGuidString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedGuid.ToString("B"), out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromInvalidSring()
        {
            //---------------Set up test pack-------------------
            const string invalidString = "Invalid";

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(invalidString, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(dbNullValue, out parsedValue);

            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
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
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid.ToString("B"));

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.NewGuid();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedGuid.ToString("B").ToUpperInvariant(), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_EmptyGuid()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_EmptyGuidString()
        {
            //---------------Set up test pack-------------------
            Guid expectedGuid = Guid.Empty;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedGuid.ToString("B"));

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
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
            Assert.AreEqual("", parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString_FromDBNull()
        {
            //---------------Set up test pack-------------------
            object dbNullValue = DBNull.Value;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(dbNullValue);

            //---------------Test Result -----------------------
            Assert.AreEqual("", parsedValue);
        }
    }
}