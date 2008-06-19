using Habanero.Base;
using Habanero.DB;

namespace Habanero.BO
{
    public class DataAccessorDB : IDataAccessor
    {
        private IBusinessObjectLoader _businessObjectLoader;


        public DataAccessorDB()
        {
            _businessObjectLoader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB();
        }

  
    }
}