using System;
using Chillisoft.Test;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Db;
using Habanero.Base;
using Habanero.Test;
using NMock;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test.Bo
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
        public void TestSettingLookupValueSetsGuid()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef classDef = MyBo.LoadClassDefWithLookup();
            BusinessObject bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueToDisplay("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }

        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef classDef = MyBo.LoadDefaultClassDef();

            Mock itsDatabaseConnectionMockControl = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection itsConnection = (IDatabaseConnection) itsDatabaseConnectionMockControl.MockInstance;


//			itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
//			itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});
            itsDatabaseConnectionMockControl.ExpectAndReturn("GetConnection",
                                                             DatabaseConnection.CurrentConnection.GetConnection());
            itsDatabaseConnectionMockControl.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            MyBo bo = (MyBo) classDef.CreateNewBusinessObject(itsConnection);
//			bo.SetPropertyValue("TestProp", "Hello") ;
//			bo.ApplyEdit() ;

            bo.SetPropertyValue("TestProp", "Goodbye");
            bo.ApplyEdit();
            bo.CancelEdit();
            Assert.AreEqual("Goodbye", bo.GetPropertyValueString("TestProp"));
        }
    }
}