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
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Habanero.Base;
using Habanero.Base.DataMappers;
using Habanero.Base.Util;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    /// <summary>
    /// This Test Class tests the functionality of the LongText custom property class.
    /// </summary>
    [TestFixture]
    public class TestLongText
    {
        private IClassDef _itsClassDef;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            var asm = Assembly.GetExecutingAssembly();
            ConfigurationManager.Initialise(asm);
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            _itsClassDef = loader.LoadClass
                (@"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""Habanero.Util.LongText"" assembly=""Habanero.Base"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(_itsClassDef);
        }

        //[Test]
        //public void TestEncryption()
        //{
        //    PasswordBlowfish p = new PasswordBlowfish("test", false);
        //    Assert.IsFalse(p.Value.Equals("test"));
        //}

        [Test]
        public void TestLoadingConstructor()
        {
            LongText longText = new LongText("test");
            Assert.IsTrue(longText.Value.Equals("test"));
        }

        [Test]
        public void TestSettingValue()
        {
            LongText longText = new LongText("test");
            longText.Value = "newtest";
            Assert.IsTrue(longText.Value.Equals("newtest"));
        }

        [Test]
        public void TestEquals()
        {
            LongText longText1 = new LongText("test");
            LongText longText2 = new LongText("test");
            Assert.IsTrue(longText1.Equals(longText2));
        }

        [Test]
        public void TestHashCode()
        {
            LongText longText = new LongText("test");

            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
            {
                Assert.AreEqual(-354185609, longText.GetHashCode());
            }
            else
            {
                Assert.AreEqual(-871206010, longText.GetHashCode());
            }
        }

        [Test]
        public void TestToString()
        {
            LongText longText = new LongText("test");
            Assert.AreEqual("test", longText.ToString());
        }

        [Test]
        public void TestPropertyType()
        {
            PropDef propDef = (PropDef) _itsClassDef.PropDefcol["TestProp"];
            Assert.AreEqual(propDef.PropertyType, typeof (LongText));
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty()
        {
            //---------------Set up test pack-------------------
            LongText longText = new LongText("test");
            GeneralDataMapper generalDataMapper = new GeneralDataMapper(typeof(LongText));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(longText, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreSame(longText, returnValue);
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty_InheritedCustomProperty()
        {
            //---------------Set up test pack-------------------
            LongText longText = new LongText("test");
            GeneralDataMapper generalDataMapper = new GeneralDataMapper(typeof(CustomProperty));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(longText, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreSame(longText, returnValue);
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty_InheritedLongText()
        {
            //---------------Set up test pack-------------------
            LongText longText = new ExtendedLongText("test");
            GeneralDataMapper generalDataMapper = new GeneralDataMapper(typeof(LongText));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(longText, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.AreSame(longText, returnValue);
        }

        internal class ExtendedLongText : LongText
        {
            public ExtendedLongText(string text) : base(text)
            {
                
            }

            public ExtendedLongText(object value, bool isLoading) : base(value, isLoading)
            {
            }
        }

        [Test]
        public void Test_BOPropGeneralDataMapper_TryParseCustomProperty_StringValue()
        {
            //---------------Set up test pack-------------------
            string test = "test";
            GeneralDataMapper generalDataMapper = new GeneralDataMapper(typeof(LongText));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            object returnValue;
            generalDataMapper.TryParsePropValue(test, out returnValue);
            //---------------Test Result -----------------------
            Assert.IsNotNull(returnValue);
            Assert.IsInstanceOf(typeof(LongText), returnValue);
            LongText longText = (LongText) returnValue;
            Assert.AreSame(test, longText.Value);
        }

        [Test]
        public void TestPropertyValue()
        {
            //---------------Set up test pack-------------------
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            LongText longText = new LongText("test");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bo.SetPropertyValue("TestProp", longText);
            object actualValue = bo.GetPropertyValue("TestProp");

            //---------------Test Result -----------------------
            Assert.IsNotNull(actualValue);
            Assert.IsInstanceOf(typeof (LongText), actualValue);
            Assert.AreSame(longText, actualValue);
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof (LongText), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
        }

        [Test]
        [Ignore("Xamarin InMemory Only")]
        public void TestPersistSqlParameterType()
        {
            TestUsingDatabase.SetupDBOracleConnection();
            IBusinessObject bo = _itsClassDef.CreateNewBusinessObject();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('*', 2500);
            string value = stringBuilder.ToString();
            bo.SetPropertyValue("TestProp", value);
            var  sqlCol = new TransactionalBusinessObjectDB(bo, DatabaseConnection.CurrentConnection).GetPersistSql();
            ISqlStatement sqlStatement = sqlCol.First();
            IList parameters = sqlStatement.Parameters;
            IDbDataParameter longTextParam = (IDbDataParameter) parameters[1];
            string oracleTypeEnumString = ReflectionUtilities.GetEnumPropertyValue(longTextParam, "OracleType");
            Assert.IsTrue(oracleTypeEnumString == "Clob");
        }

    }

    
}