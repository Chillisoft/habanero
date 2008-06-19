using Habanero.Base;
using Habanero.DB;

namespace Habanero.BO
{
    public class DataAccessorDB : IDataAccessor
    {
        private IBusinessObjectLoader _businessObjectLoader;
        private ITransactionCommitterFactory _transactionCommiterFactory;


        public DataAccessorDB()
        {
            _businessObjectLoader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);
            _transactionCommiterFactory = new TransactionCommitterFactoryDB();
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitterFactory TransactionCommiterFactory
        {
            get { return _transactionCommiterFactory; }
        }
    }
}