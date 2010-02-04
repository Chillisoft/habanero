using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    public class TransactionCommitterRemote : ITransactionCommitter
    {
        private readonly ITransactionCommitter _remoteTransactionCommitter;

        public TransactionCommitterRemote(ITransactionCommitter remoteTransactionCommitter)
        {
            _remoteTransactionCommitter = remoteTransactionCommitter;
        }

        public void AddBusinessObject(IBusinessObject businessObject)
        {
            _remoteTransactionCommitter.AddBusinessObject(businessObject);
        }
        public void AddTransaction(ITransactional transaction) { _remoteTransactionCommitter.AddTransaction(transaction); }
        List<Guid> ITransactionCommitter.CommitTransaction() {
            List<Guid> executedTransactions = _remoteTransactionCommitter.CommitTransaction();
            executedTransactions.ForEach(guid => ((BusinessObject)BusinessObjectManager.Instance[guid]).UpdateStateAsPersisted());
            return executedTransactions;
        }

       
    }
}