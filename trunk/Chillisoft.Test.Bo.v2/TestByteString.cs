using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Generic;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// This Test Class tests the functionality of the ByteString custom property class.
    /// </summary>
    [TestFixture]
    public class TestByteString : TestUsingDatabase
    {
        private ClassDef itsClassDef;

        //These unicode bytes spell 'test':   t - 116, e - 101, s - 115, t - 116
        private byte[] itsByteArrSpelling_test = { 116, 0, 101, 0, 115, 0, 116, 0 };

        public TestByteString()
        {
            ClassDef.GetClassDefCol.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" type=""Guid"" />
					<propertyDef name=""TestProp"" type=""ByteString"" assembly=""Habanero.Bo"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
				</classDef>
			");
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
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", new ByteString("test"));
            Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof(ByteString), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValueString("TestProp"));
        }

        [Test]
        public void TestPersistSqlParameterValue()
        {
            base.SetupDBOracleConnection();
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            ISqlStatementCollection sqlCol = bo.GetPersistSql();
            ISqlStatement sqlStatement = sqlCol[0];
            System.Collections.IList parameters = sqlStatement.Parameters;
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
