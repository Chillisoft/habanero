using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    public class TransactionCommitterMultiSource : ITransactionCommitter
    {
        private readonly IDataAccessor _defaultDataAccessor;
        private readonly Dictionary<Type, IDataAccessor> _dataAccessors;

        private ITransactionCommitter _transactionCommitter;
        private IDataAccessor _myDataAccessor;

        public TransactionCommitterMultiSource(IDataAccessor defaultDataAccessor, Dictionary<Type, IDataAccessor> dataAccessors)
        {
            _defaultDataAccessor = defaultDataAccessor;
            _dataAccessors = dataAccessors;
        }


        public void AddBusinessObject(IBusinessObject businessObject)
        {
            if (_myDataAccessor == null)
            {
                _myDataAccessor = GetDataAccessorForType(businessObject.GetType());
                _transactionCommitter = _myDataAccessor.CreateTransactionCommitter();
            } else
            {
                IDataAccessor dataAccessorToUseForType = GetDataAccessorForType(businessObject.GetType());
                if (dataAccessorToUseForType != _myDataAccessor)
                {
                    throw new HabaneroDeveloperException("A problem occurred while trying to save, please see log for details", string.Format("A BusinessObject of type {0} was added to a TransactionCommitterMultiSource which has been set up with a different source to this type.", businessObject.GetType().FullName));
                }
            }
            _transactionCommitter.AddBusinessObject(businessObject);
        }

        private IDataAccessor GetDataAccessorForType(Type type)
        {
            return _dataAccessors.ContainsKey(type) ? _dataAccessors[type] : _defaultDataAccessor;
        }

        public void AddTransaction(ITransactional transaction)
        {
            if (_myDataAccessor == null)
            {
                _myDataAccessor = _defaultDataAccessor;
                _transactionCommitter = _myDataAccessor.CreateTransactionCommitter();
            }
            _transactionCommitter.AddTransaction(transaction);
        }

        public List<Guid> CommitTransaction()
        {
            return _transactionCommitter != null ? _transactionCommitter.CommitTransaction() : new List<Guid>();
        }
    }
}