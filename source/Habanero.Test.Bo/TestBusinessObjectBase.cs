using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectBase.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectBase : TestUsingDatabase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
        }

        [Test]
        public void TestInstantiate()
        {
            MyBO bo = new MyBO();
            string t = bo.GetPropertyValueString("TestProp");
        }

        //[Test]
        //public void TestIndexer()
        //{
        //    MyBO bo = new MyBO();
        //    bo["TestProp"] = "hello";
        //    Assert.AreEqual("hello", bo.GetPropertyValue("TestProp"));
        //}

        [Test]
        public void TestSettingLookupValueSetsGuid()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithLookup();
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }


        [Test]
        public void TestGetPropertyValueToDisplay()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithStringLookup();
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "Started");
            Assert.AreEqual("S", bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("Started", bo.GetPropertyValueToDisplay("TestProp2"));
        }


        [Test]
        public void TestGetPropertyValueToDisplayWithBOLookupList()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", cp);
            Assert.AreEqual(cp.ContactPersonID, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListWithString()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "abc");
            Assert.AreEqual(cp.ID.ToString(), bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual("abc", bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestBOLookupListNull()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithBOStringLookup();
            ContactPerson.LoadDefaultClassDef();
            ContactPerson cp = BOLoader.Instance.GetBusinessObject<ContactPerson>("Surname = abc");
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", null);
            Assert.AreEqual(null, bo.GetPropertyValue("TestProp2"));
            Assert.AreEqual(null, bo.GetPropertyValueToDisplay("TestProp2"));
        }

        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadDefaultClassDef();

            Mock itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection itsConnection = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;


//			itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//			itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            MyBO bo = (MyBO) classDef.CreateNewBusinessObject(itsConnection);
//			bo.SetPropertyValue("TestProp", "Hello") ;
//			bo.Save() ;

            bo.SetPropertyValue("TestProp", "Goodbye");
            bo.Save();
            bo.Restore();
            Assert.AreEqual("Goodbye", bo.GetPropertyValueString("TestProp"));
        }

    }
}