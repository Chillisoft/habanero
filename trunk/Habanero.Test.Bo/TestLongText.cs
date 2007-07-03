using System.Data;
using System.Text;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Util;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test.Bo
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
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" type=""LongText"" assembly=""Habanero.Util"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
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
        public void TestEquals()
        {
            LongText longText1 = new LongText("test");
            LongText longText2 = new LongText("test");
            Assert.IsTrue(longText1.Equals(longText2));
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
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", new LongText("test"));
            Assert.AreSame(typeof (LongText), bo.GetPropertyValue("TestProp").GetType());
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof (LongText), bo.GetPropertyValue("TestProp").GetType());
            Assert.AreEqual("test", bo.GetPropertyValueString("TestProp"));
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
            ISqlStatementCollection sqlCol = bo.GetPersistSql();
            ISqlStatement sqlStatement = sqlCol[0];
            System.Collections.IList parameters = sqlStatement.Parameters;
            IDbDataParameter longTextParam = (IDbDataParameter) parameters[1];
            string oracleTypeEnumString = ReflectionUtilities.getEnumPropertyValue(longTextParam, "OracleType");
            Assert.IsTrue(oracleTypeEnumString == "Clob");
        }
    }
}