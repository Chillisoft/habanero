using Habanero.BO;

namespace Habanero.Test
{
    public static class FixtureEnvironment
    {
        public static void ResetBORegistryBusinessObjectManager()
        {
            BORegistry.BusinessObjectManager = null;
        }

        public static void SetupInMemoryDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        public static void SetupNewIsolatedBusinessObjectManager()
        {
            //Ensures a new BOMan is created and used for each test
            BORegistry.BusinessObjectManager = new BusinessObjectManagerIsolated();
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        public static void ClearBusinessObjectManager()
        {
            BORegistry.BusinessObjectManager.ClearLoadedObjects();
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        private class BusinessObjectManagerIsolated : BusinessObjectManager
        {
        }
    }
}