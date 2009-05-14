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
using System.Collections;
using System.Data;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    /// <summary>
    /// This Test Class tests the functionality of the ByteString custom property class.
    /// </summary>
    [TestFixture]
    public class TestByteString : TestUsingDatabase
    {
        private readonly ClassDef itsClassDef;

        //These unicode bytes spell 'test':   t - 116, e - 101, s - 115, t - 116
        private readonly byte[] itsByteArrSpelling_test = { 116, 0, 101, 0, 115, 0, 116, 0 };

        public TestByteString()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""ByteString"" assembly=""Habanero.Util"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            base.SetupDBConnection();
        }

        [Test]
        public void TestLoadingConstructor()
        {
            ByteString byteString = new ByteString(itsByteArrSpelling_test, true);
            Assert.IsTrue(byteString.Value.Equals("test"));
        }

        [Test]
        public void TestNonLoadingConstructor()
        {
            ByteString byteString = new ByteString("test", false);
            Assert.IsTrue(byteString.Value.Equals("test"));
        }

        [Test]
        public void TestStringConstructor()
        {
            ByteString byteString = new ByteString("test");
            Assert.IsTrue(byteString.Value.Equals("test"));
        }

        [Test]
        public void TestOtherTypeConstructor()
        {
            ByteString byteString = new ByteString(1, false);
            Assert.AreEqual("1", byteString.Value);
        }

        [Test]
        public void TestSettingValue()
        {
            ByteString byteString = new ByteString("test");
            byteString.Value = "newtest";
            Assert.IsTrue(byteString.Value.Equals("newtest"));
        }

        [Test]
        public void TestEquals()
        {
            ByteString byteString1 = new ByteString("test");
            ByteString byteString2 = new ByteString("test");
            Assert.IsTrue(byteString1.Equals(byteString2));
        }

        [Test]
        public void TestEqualsWithByte()
        {
            ByteString byteString1 = new ByteString("test");
            ByteString byteString2 = new ByteString(itsByteArrSpelling_test,true);
            Assert.IsTrue(byteString1.Equals(byteString2));
        }

        [Test]
        public void TestEqualsBothByte()
        {
            ByteString byteString1 = new ByteString(itsByteArrSpelling_test,false);
            ByteString byteString2 = new ByteString(itsByteArrSpelling_test,true);
            Assert.IsTrue(byteString1.Equals(byteString2));
        }

        [Test]
        public void TestHashCode()
        {
            ByteString byteString = new ByteString("test");
            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86") {
                Assert.AreEqual(-354185609, byteString.GetHashCode());
            } else {
                Assert.AreEqual(-871206010, byteString.GetHashCode());
            }

        }

        [Test]
        public void TestToString()
        {
            ByteString byteString = new ByteString("test");
            Assert.AreEqual("test", byteString.ToString());
        }

        [Test]
        public void TestToStringFromByte()
        {
            ByteString byteString = new ByteString(itsByteArrSpelling_test,true);
            Assert.AreEqual("test", byteString.ToString());
        }

        [Test]
        public void TestPropertyType()
        {
            Assert.AreEqual(itsClassDef.PropDefcol["TestProp"].PropertyType, typeof(ByteString));
        }

        [Test]
        public void TestPropertyValue()
        {
            IBusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", new ByteString("test"));
            Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            IBusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValue("TestProp").ToString());
        }

        [Test]
        public void TestPersistSqlParameterValue()
        {
            base.SetupDBOracleConnection();
            IBusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            ISqlStatementCollection sqlCol = new TransactionalBusinessObjectDB(bo).GetPersistSql();
            ISqlStatement sqlStatement = sqlCol[0];
            IList parameters = sqlStatement.Parameters;
            IDbDataParameter byteStringParam = (IDbDataParameter)parameters[1];
            Assert.IsTrue(byteStringParam.Value is byte[]);
            byte[] paramValue = (byte[])byteStringParam.Value;
            Assert.AreEqual(paramValue.Length, itsByteArrSpelling_test.Length);
            Assert.AreEqual(paramValue[0], itsByteArrSpelling_test[0]);
            Assert.AreEqual(paramValue[1], itsByteArrSpelling_test[1]);
            Assert.AreEqual(paramValue[2], itsByteArrSpelling_test[2]);
            Assert.AreEqual(paramValue[3], itsByteArrSpelling_test[3]);
            Assert.AreEqual(paramValue[4], itsByteArrSpelling_test[4]);
            Assert.AreEqual(paramValue[5], itsByteArrSpelling_test[5]);
            Assert.AreEqual(paramValue[6], itsByteArrSpelling_test[6]);
            Assert.AreEqual(paramValue[7], itsByteArrSpelling_test[7]);
        }
    }
}