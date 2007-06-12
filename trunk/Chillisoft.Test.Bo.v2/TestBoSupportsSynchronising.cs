using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Test.Setup.v2;
using Chillisoft.Util.v2;
using NMock;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestBOSupportsSynchronising.
    /// </summary>
    [TestFixture]
    public class TestBoSupportsSynchronising : TestUsingDatabase
    {
        private ClassDef itsClassDef;
        private MyBo itsMyBo;
        private MyBo itsMyBoNonSync;
        private IDatabaseConnection itsConnection;
        private IMock itsConnectionMock;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            ClassDef.GetClassDefCol.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"" >
					<propertyDef name=""MyBoID"" type=""Guid"" />
					<propertyDef name=""TestProp"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
				</classDef>
			");
            itsConnectionMock = new DynamicMock(typeof (IDatabaseConnection));
            itsConnection = (IDatabaseConnection) itsConnectionMock.MockInstance;

            itsMyBoNonSync = (MyBo) itsClassDef.CreateNewBusinessObject();
            itsMyBoNonSync.SetDatabaseConnection(itsConnection);

            itsClassDef.SupportsSynchronising = true;

            itsMyBo = (MyBo) itsClassDef.CreateNewBusinessObject();
            itsMyBo.SetDatabaseConnection(itsConnection);
            GlobalRegistry.SynchronisationController = new VersionNumberSynchronisationController();
        }


        [Test]
        public void TestCreateBusinessObjectWithSupportsSynchronising()
        {
            itsMyBo.GetPropertyValue("SyncVersionNumber");
            itsMyBo.GetPropertyValue("SyncVersionNumberAtLastSync");
        }

        [Test]
        public void TestVersionNumberAtCreation()
        {
            Assert.AreEqual(0, itsMyBo.GetPropertyValue("SyncVersionNumber"));
            Assert.AreEqual(0, itsMyBo.GetPropertyValue("SyncVersionNumberAtLastSync"));
            Assert.IsNotNull(itsMyBo.GetPropertyValue("SyncVersionNumber"));
        }

        [Test]
        public void TestVersionNumberIncrementsOnSave()
        {
            itsConnectionMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            itsConnectionMock.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            itsMyBo.ApplyEdit();
            Assert.AreEqual(1, itsMyBo.GetPropertyValue("SyncVersionNumber"));
            Assert.AreEqual(0, itsMyBo.GetPropertyValue("SyncVersionNumberAtLastSync"));

            itsConnectionMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            itsConnectionMock.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            itsMyBo.SetPropertyValue("TestProp", "value2");
            itsMyBo.ApplyEdit();
            Assert.AreEqual(2, itsMyBo.GetPropertyValue("SyncVersionNumber"));
            Assert.AreEqual(0, itsMyBo.GetPropertyValue("SyncVersionNumberAtLastSync"));

            itsConnectionMock.Verify();
        }

        [Test, ExpectedException(typeof (CoreBizArgumentException))]
        public void TestNonSyncSupporterDoesntHaveSyncProps()
        {
            itsMyBoNonSync.GetPropertyValue("SyncVersionNumber");
        }

        [Test]
        public void TestNonSyncSupporterApplyEdit()
        {
            itsConnectionMock.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection());
            itsConnectionMock.ExpectAndReturn("ExecuteSql", 1, new object[] {null, null});

            itsMyBoNonSync.ApplyEdit();
            itsConnectionMock.Verify();
        }
    }
}