using Habanero.Base;

namespace Habanero.BO
{
    public class DataAccessorInMemory : IDataAccessor
    {
        private readonly DataStoreInMemory _dataStore;
        private IBusinessObjectLoader _businessObjectLoader;

        public DataAccessorInMemory() : this(new DataStoreInMemory())
        {
            
        }
        internal DataAccessorInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;

            _businessObjectLoader = new BusinessObjectLoaderInMemory(_dataStore);
        }


        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterInMemory(_dataStore);
        }


    }
}