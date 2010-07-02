using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// A Data Accessor that is used to configure loading of business objects from 
    /// multiple data sources.
    /// E.g. By using <see cref="AddDataAccessor"/> you can configure the DataSource 
    /// that should be used for loading and saving of each Type of Business Object.
    /// </summary>
    public class DataAccessorMultiSource : IDataAccessor
    {
        private readonly IDataAccessor _defaultDataAccessor;
        private readonly BusinessObjectLoaderMultiSource _multiSourceBusinessObjectLoader;

        private readonly Dictionary<Type, IDataAccessor> _dataAccessors;
        /// <summary>
        /// Construct the DataSource with a default accessor
        /// </summary>
        /// <param name="defaultDataAccessor"></param>
        public DataAccessorMultiSource(IDataAccessor defaultDataAccessor)
        {
            _dataAccessors= new Dictionary<Type, IDataAccessor>();
            _defaultDataAccessor = defaultDataAccessor;
            _multiSourceBusinessObjectLoader = new BusinessObjectLoaderMultiSource(_defaultDataAccessor.BusinessObjectLoader);
        }
        /// <summary>
        /// Returns the <see cref="IBusinessObjectLoader"/> to be used when using this
        /// <see cref="IDataAccessor"/>.
        /// </summary>
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _multiSourceBusinessObjectLoader; }
        }
        /// <summary>
        /// Returns the <see cref="ITransactionCommitter"/> to be used when using this
        /// <see cref="IDataAccessor"/>.
        /// </summary>
        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterMultiSource(_defaultDataAccessor, _dataAccessors);
        }
        /// <summary>
        /// Add the <paramref name="dataAccessor"/> for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The business object type</param>
        /// <param name="dataAccessor">The data accessor to use for loading and persisting business objects.</param>
        public void AddDataAccessor(Type type, IDataAccessor dataAccessor)
        {
            _dataAccessors.Add(type, dataAccessor);
            _multiSourceBusinessObjectLoader.AddBusinessObjectLoader(type, dataAccessor.BusinessObjectLoader);
        }
    }
}