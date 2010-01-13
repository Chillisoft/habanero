using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataAccessorMultiSource : IDataAccessor
    {
        private IDataAccessor _defaultDataAccessor;
        private BusinessObjectLoaderMultiSource _multiSourceBusinessObjectLoader;

        private Dictionary<Type, IDataAccessor> _dataAccessors;

        public DataAccessorMultiSource(IDataAccessor defaultDataAccessor)
        {
            _dataAccessors= new Dictionary<Type, IDataAccessor>();
            _defaultDataAccessor = defaultDataAccessor;
            _multiSourceBusinessObjectLoader = new BusinessObjectLoaderMultiSource(_defaultDataAccessor.BusinessObjectLoader);
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _multiSourceBusinessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterMultiSource(_defaultDataAccessor, _dataAccessors);
        }

        public void AddDataAccessor(Type type, IDataAccessor dataAccessor)
        {
            _dataAccessors.Add(type, dataAccessor);
            _multiSourceBusinessObjectLoader.AddBusinessObjectLoader(type, dataAccessor.BusinessObjectLoader);
        }
    }
}