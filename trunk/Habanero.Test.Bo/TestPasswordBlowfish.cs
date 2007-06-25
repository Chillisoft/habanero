using System.Data;
using Chillisoft.Test;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Base;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test.Bo
{
    /// <summary>
    /// Summary description for TestPassword.
    /// </summary>
    [TestFixture]
    public class TestPasswordBlowfish : TestUsingDatabase
    {
        ClassDef itsClassDef;

        public TestPasswordBlowfish()
        {
            ClassDef.GetClassDefCol.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<classDef name=""MyBo"" assembly=""Habanero.Test"">
					<propertyDef name=""MyBoID"" type=""Guid"" />
					<propertyDef name=""TestProp"" type=""PasswordBlowfish"" assembly=""Habanero.Bo"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
				</classDef>
			");
            base.SetupDBConnection();
        }

        [Test]
        public void TestEncryption()
        {
            PasswordBlowfish p = new PasswordBlowfish("test", false);
            Assert.IsFalse(p.Value.Equals("test"));
        }

        [Test]
        public void TestLoadingConstructor()
        {
            PasswordBlowfish p = new PasswordBlowfish("test", true);
            Assert.IsTrue(p.Value.Equals("test"));
        }

        [Test]
        public void TestDecryption()
        {
            PasswordBlowfish p = new PasswordBlowfish("test", false);
            PasswordBlowfish p1 = new PasswordBlowfish("test", false);
            Assert.AreEqual("test", p.DecryptedValue);
            Assert.AreEqual(p.DecryptedValue, p1.DecryptedValue);
        }

        [Test]
        public void TestEquals()
        {
            PasswordBlowfish p = new PasswordBlowfish("test", false);
            PasswordBlowfish p1 = new PasswordBlowfish("test", false);
            Assert.IsTrue(p.Equals(p1));
        }

        [Test]
        public void TestToString()
        {
            PasswordBlowfish p = new PasswordBlowfish("test", false);
            BlowfishCrypter crypter = new BlowfishCrypter();
            Assert.AreEqual("test", crypter.DecryptString(p.ToString()));
        }

        [Test]
        public void TestPropertyType()
        {
            Assert.AreEqual(itsClassDef.PropDefcol["TestProp"].PropertyType, typeof (PasswordBlowfish));
        }

        [Test]
        public void TestPropertyValue()
        {
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", new PasswordBlowfish("test", false));
            Assert.AreSame(typeof (PasswordBlowfish), bo.GetPropertyValue("TestProp").GetType());
        }

        [Test]
        public void TestSetPropertyValueWithString()
        {
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            Assert.AreSame(typeof (PasswordBlowfish), bo.GetPropertyValue("TestProp").GetType());
            BlowfishCrypter crypter = new BlowfishCrypter();
            Assert.AreEqual("test", crypter.DecryptString(bo.GetPropertyValueString("TestProp")));
        }

        [Test]
        public void TestPersistSqlUsesString()
        {
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp", "test");
            ISqlStatementCollection sqlCol = bo.GetPersistSql();
            Assert.AreSame(typeof (string), ((IDbDataParameter) sqlCol[0].Parameters[1]).Value.GetType());
            BlowfishCrypter crypter = new BlowfishCrypter();
            Assert.AreEqual("test", crypter.DecryptString((string) ((IDbDataParameter) sqlCol[0].Parameters[1]).Value));
        }
    }
}