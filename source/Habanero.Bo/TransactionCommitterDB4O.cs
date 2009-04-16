using System;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
using log4net;

namespace Habanero.BO
{
    public class TransactionCommitterDB4O : TransactionCommitter
    {

        private IObjectContainer _db4o;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitterDB4O");
        public TransactionCommitterDB4O(string db4OFileName) { DB4OFileName = db4OFileName; }

        protected override void BeginDataSource()
        {
            _db4o = Db4oFactory.OpenFile(DB4OFileName);
        }

        protected override void TryRollback()
        {
            if (_db4o == null) return;
            try
            {
                _db4o.Rollback();
            }
            finally
            {
                _db4o.Close();
            }
        }

        protected internal override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            if (transaction is TransactionalBusinessObject)
            {
                TransactionalBusinessObject tbo = transaction as TransactionalBusinessObject;
                _db4o.Store(tbo.BusinessObject);
                _executedTransactions.Add(transaction);
            }
        }

        protected override bool CommitToDatasource()
        {
            try
            {
                _db4o.Commit();
                return true;
            }
            finally
            {
                if (_db4o != null) _db4o.Close();
            }
        }

        protected internal override TransactionalBusinessObject CreateTransactionalBusinessObject(IBusinessObject businessObject)
        {
            return new TransactionalBusinessObject(businessObject);
        }

        public string DB4OFileName { get; private set; }
    }
}