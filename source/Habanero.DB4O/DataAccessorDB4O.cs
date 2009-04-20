using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.DB4O
{
    public class DataAccessorDB4O : IDataAccessor
    {
        private readonly IBusinessObjectLoader _businessObjectLoader;
        private IObjectContainer _objectContainer;

        public DataAccessorDB4O(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _businessObjectLoader = new BusinessObjectLoaderDB4O(_objectContainer);
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB4O(_objectContainer);
        }
    }
}