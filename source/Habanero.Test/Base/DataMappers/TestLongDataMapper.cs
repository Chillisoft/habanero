#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using Habanero.Base;
using Habanero.Base.DataMappers;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.Base.DataMappers
{
    [TestFixture]
    public class TestLongDataMapper
    {
        private PropDef _propDef;
        private DataMapper _dataMapper;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            
            _propDef = new PropDef("PropName", typeof (long), PropReadWriteRule.ReadWrite, null);

            _dataMapper = new LongDataMapper();
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
            const long expectedlong = 1;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(expectedlong.ToString(), out parsedValue);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong, parsedValue);
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
        public void Test_PropDef_ParsePropValue_WithDecimal_Max()
        {
            //---------------Set up test pack-------------------
            const decimal value = decimal.MaxValue;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        }

        

        [Test]
        public void Test_PropDef_ParsePropValue_WithDecimal_NoRoundingNecessary()
        {
            //---------------Set up test pack-------------------
            const decimal value = 100.00m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_WithDecimal_RoundUp()
        {
            //---------------Set up test pack-------------------
            const decimal value = 123.50m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_WithDecimal_RoundDown()
        {
            //---------------Set up test pack-------------------
            const decimal value = 321.49m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_PropDef_ParsePropValue_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            FakeBOWithLongObjectIDProp.LoadNumberGenClassDef();
            const long validLongID = 3;
            var primaryKey = MockRepository.GenerateStub<IPrimaryKey>();
            primaryKey.Stub(key => key.GetAsValue()).Return(validLongID);
            var bo = MockRepository.GenerateStub<IBusinessObject>();
            bo.Stub(o => o.ID).Return(primaryKey);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _propDef.TryParsePropValue(bo, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
            Assert.AreEqual(validLongID, parsedValue);
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
            const long expectedlong = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            string parsedValue = _propDef.ConvertValueToString(expectedlong);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong.ToString().ToUpperInvariant(), parsedValue);
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
        public void Test_DataMapper_ParsePropValue_ValidBusinessObject()
        {
            //---------------Set up test pack-------------------
            FakeBOWithLongObjectIDProp.LoadNumberGenClassDef();
            const long validLongID = 3;
            var primaryKey = MockRepository.GenerateStub<IPrimaryKey>();
            primaryKey.Stub(key => key.GetAsValue()).Return(validLongID);
            var bo = MockRepository.GenerateStub<IBusinessObject>();
            bo.Stub(o => o.ID).Return(primaryKey);
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(bo, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(parsedValue);
            Assert.IsTrue(parsedSucceed);
            Assert.AreEqual(validLongID, parsedValue);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_FromValidSring()
        {
            //---------------Set up test pack-------------------
            const long expectedlong = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedlong.ToString(), out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_AboveLongMax()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MaxValue + 0.01m;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_EqualsLongMax()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MaxValue;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_BelowLongMax()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MaxValue - 0.01m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_BelowLongMin()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MinValue - 0.01m;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.IsNull(parsedValue);
            Assert.IsFalse(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_EqualsLongMin()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MinValue;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_AboveLongMin()
        {
            //---------------Set up test pack-------------------
            const decimal value = long.MinValue + 0.01m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_NoRoundingNecessary()
        {
            //---------------Set up test pack-------------------
            const decimal value = 100.00m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_RoundUp()
        {
            //---------------Set up test pack-------------------
            const decimal value = 123.50m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue_WithDecimal_RoundDown()
        {
            //---------------Set up test pack-------------------
            const decimal value = 321.49m;
            decimal expectedLongeger = Math.Round(value);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(value, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLongeger, parsedValue);
            Assert.IsTrue(parsedSucceed);
        }

        [Test]
        public void Test_DataMapper_ParsePropValue()
        {
            //---------------Set up test pack-------------------
            const long expectedlong = 4;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object parsedValue;
            bool parsedSucceed = _dataMapper.TryParsePropValue(expectedlong, out parsedValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong, parsedValue);
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
            const long expectedlong = 4;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedlong);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong.ToString().ToUpperInvariant(), parsedValue);
        }

        [Test]
        public void Test_DataMapper_ConvertValueToString()
        {
            //---------------Set up test pack-------------------
            const long expectedlong = 4;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string parsedValue = _dataMapper.ConvertValueToString(expectedlong);

            //---------------Test Result -----------------------
            Assert.AreEqual(expectedlong.ToString().ToUpperInvariant(), parsedValue);
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
    ///<summary>
    /// A simple sequential number generator business object
    ///</summary>
    internal class FakeBOWithLongObjectIDProp : BusinessObject
    {
        /// <summary>
        /// Gets or sets the sequence number
        /// </summary>
        internal virtual long? LongProp
        {
            get { return ((long?)(base.GetPropertyValue("LongProp"))); }
            set { base.SetPropertyValue("LongProp", value); }
        }

        internal static void LoadNumberGenClassDef()
        {
            if (!ClassDef.ClassDefs.Contains(typeof(BOSequenceNumber)))
            {
                LoadNumberGenClassDef(null);
            }
        }

        internal static void LoadNumberGenClassDef(string tableName)
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            string classDef = "<class name=\"FakeBOWithLongObjectIDProp\" assembly=\"Habanero.Test.BO\" >" +
                              "<property  name=\"LongProp\" type=\"Int64\" />" +
                              "<primaryKey isObjectID=\"false\">" +
                              "<prop name=\"LongProp\" />" +
                              "</primaryKey>" +
                              "</class>";
            IClassDef itsClassDef = itsLoader.LoadClass(classDef);
            ClassDef.ClassDefs.Add(itsClassDef);
            return;
        }
    }
}