//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    /// <summary>
    /// This Test Class tests the functionality of the LongText custom property class.
    /// </summary>
    [TestFixture]
    public class TestLongText : TestUsingDatabase
    {
        private ClassDef itsClassDef;

        public TestLongText()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""LongText"" assembly=""Habanero.Util"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            base.SetupDBConnection();
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
            Assert.AreEqual(itsClassDef.PropDefcol["TestProp"].PropertyType, typeof (LongText));
        }

        [Test]
        public void TestPropertyValue()
        {
            IBusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", new LongText("test"));
            Assert.AreSame(typeof (LongText), bo.GetPropertyValue("TestProp").GetType());
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            IBusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof (LongText), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
        }

        [Test]
        public void TestPersistSqlParameterType()
        {
            base.SetupDBOracleConnection();
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('*', 2500);
            string value = stringBuilder.ToString();
            bo.SetPropertyValue("TestProp", value);
            ISqlStatementCollection sqlCol = new TransactionalBusinessObjectDB(bo).GetPersistSql();
            ISqlStatement sqlStatement = sqlCol[0];
            IList parameters = sqlStatement.Parameters;
            IDbDataParameter longTextParam = (IDbDataParameter) parameters[1];
            string oracleTypeEnumString = ReflectionUtilities.getEnumPropertyValue(longTextParam, "OracleType");
            Assert.IsTrue(oracleTypeEnumString == "Clob");
        }
    }
}