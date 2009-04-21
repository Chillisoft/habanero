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
            Db4oFactory.Configure().ObjectClass(typeof(BusinessObjectDTO)).CascadeOnUpdate(true);
            _businessObjectLoader = new BusinessObjectLoaderDB4O(_objectContainer);
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public virtual ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB4O(_objectContainer);
        }
    }
}