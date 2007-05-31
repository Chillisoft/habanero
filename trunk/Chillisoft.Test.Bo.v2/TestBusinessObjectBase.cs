using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Test.Setup.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
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
            ClassDef.GetClassDefCol().Clear();
            ClassDef classDef = MyBo.LoadClassDefWithLookup();
            BusinessObjectBase bo = classDef.CreateNewBusinessObject();
            bo.SetPropertyValue("TestProp2", "s1");
            Assert.AreEqual("s1", bo.GetPropertyValueForUser("TestProp2"));
            Assert.AreEqual(new Guid("{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"), bo.GetPropertyValue("TestProp2"));
        }

        [Test]
        public void TestApplyEditResetsPreviousValues()
        {
            ClassDef.GetClassDefCol().Clear();
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