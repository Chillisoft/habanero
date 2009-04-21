using System.IO;
using Db4objects.Db4o;
using Habanero.BO;
using Habanero.DB4O;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB4O
{
    [TestFixture]
    public class TestBusinessObjectLoader_RefreshCollectionDB4O :
        TestBusinessObjectLoader_RefreshCollection
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
        }

        #endregion

        protected override void DeleteEnginesAndCars()
        {

        }

        protected override void SetupDataAccessor()
        {
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            const string db4oFileStore = "DataStore.db4o";
            if (File.Exists(db4oFileStore)) File.Delete(db4oFileStore);
            DB4ORegistry.DB = Db4oFactory.OpenFile(db4oFileStore);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }
    }
}