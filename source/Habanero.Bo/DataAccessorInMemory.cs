using Habanero.Base;

namespace Habanero.BO
{
    public class DataAccessorInMemory : IDataAccessor
    {
        private IBusinessObjectLoader _businessObjectLoader;
        private ITransactionCommitterFactory _transactionCommiterFactory;

        public DataAccessorInMemory() : this(new DataStoreInMemory())
        {
            
        }
        internal DataAccessorInMemory(DataStoreInMemory dataStore)
        {
            _businessObjectLoader = new BusinessObjectLoaderInMemory(dataStore);
            _transactionCommiterFactory = new TransactionCommitterFactoryInMemory(dataStore);
        }

        #region IDataAccessor Members

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitterFactory TransactionCommiterFactory
        {
            get { return _transactionCommiterFactory; }
        }

        #endregion
    }
}