using System.IO;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    public class FirebirdEmbeddedTestsBase
    {
        private string[] _filesToRemove;
        private IBusinessObjectManager _originalBusinessObjectManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _filesToRemove = (new TestUsingDatabase()).SetupTemporaryFirebirdDatabase();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            foreach (var f in _filesToRemove)
            {
                try
                {
                    File.Delete(f);
                }
                catch { }
            }
        }

        [SetUp]
        public void Setup()
        {
            FixtureEnvironment.SetupNewIsolatedBusinessObjectManager();
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorDB();
            FixtureEnvironment.ClearBusinessObjectManager();
            TestUtil.WaitForGC();
            _originalBusinessObjectManager = BORegistry.BusinessObjectManager;
        }

        [TearDown]
        public virtual void TearDown()
        {
            BORegistry.BusinessObjectManager = _originalBusinessObjectManager;
        }

    }
}