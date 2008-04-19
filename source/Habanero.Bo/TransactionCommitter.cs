using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// This base class manages and commits a collection of ITransactions to a datasource. 
    /// The sub classes of this class implement a specific strategy e.g. Committing to a 
    /// database.
    /// </summary>
    public abstract class TransactionCommitter   
    {
        private readonly List<ITransactionalBusinessObject> _originalTransactions = new List<ITransactionalBusinessObject>();
        protected List<ITransactionalBusinessObject> _executedTransactions = new List<ITransactionalBusinessObject>();

        ///<summary>
        /// Returns a list of transactions that will be committed.
        ///</summary>
        internal List<ITransactionalBusinessObject> OriginalTransactions
        {
            get { return _originalTransactions; }
        }


        /// TODO: Peter I do not think this should exist this is where the binding to 
        /// SQL is being forced we should just be adding business objects in and the
        /// TransactionalCommitter will wrap the BO into A transactionalBusinessObject of the 
        /// appropriate type.
        /// 
        ///<summary>
        /// This method adds an TransactionalBusinessObject to the list of transactions.
        ///</summary>
        ///<param name="transaction"></param>
        internal void AddTransaction(ITransactionalBusinessObject transaction)
        {
            _originalTransactions.Add(transaction);
        }

        ///<summary>
        /// Commit the transactions to the datasource e.g. the database, file, memory DB
        ///</summary>
        ///<returns></returns>
        public void CommitTransaction()
        {
            Begin();
            Execute();
            Commit();
        }
        /// <summary>
        /// Begins the transaction. First validates that all the objects in the transaction are valid.
        /// Once the objects are validated the Datasource transaction is started.
        /// </summary>
        private void Begin()
        {
            ValidateTransactionCanBePersisted();
            BeginDataSource();
        }
        /// <summary>
        /// Validates each Itransactional object in the collection and builds up a
        /// list of errors of error messages.
        /// </summary>
        private void ValidateTransactionCanBePersisted()
        {
            string allMessages = "";
            foreach (ITransactionalBusinessObject transaction in _originalTransactions)
            {
                string errMsg;
                if (!transaction.IsValid(out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }

            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusObjectInAnInvalidStateException(allMessages);
            }
            foreach (ITransactionalBusinessObject transaction in _originalTransactions)
            {
                if (!transaction.IsDeleted) continue;

                string errMsg;
                if (!DeleteHelper.CheckCanDelete(transaction.BusinessObject, out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }
            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusinessObjectReferentialIntegrityException(allMessages);
            }
        }
        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected abstract void BeginDataSource();

        /// <summary>
        /// Executes the transactions and rolls back in the event of an error.
        /// </summary>
        private void Execute()
        {
            try
            {
                foreach (ITransactionalBusinessObject transaction in _originalTransactions)
                {
                    ExecuteTransactionToDataSource(transaction);
                }
            }
            catch (Exception)
            {
                //TODO:log
                TryRollback();
                throw;
            }
        }


        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit and 
        /// marks all the ITransactional objects as committed.
        /// </summary>
        private void Commit()
        {
            CommitToDatasource();
            UpdateTransactionsCommited();
        }
        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected abstract void CommitToDatasource();
        /// <summary>
        /// Marks all the ITransactional objects as committed.
        /// </summary>
        private void UpdateTransactionsCommited()
        {
            foreach (ITransactionalBusinessObject transaction in _executedTransactions)
            {
                transaction.UpdateStateAsCommitted();
            }
        }
        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="CommitToDatasource"/>
        /// </summary>
        protected abstract void TryRollback();

        public TransactionCommitter()
        {
            _executedTransactions = new List<ITransactionalBusinessObject>();
        }

        ///// <summary>
        ///// Tries to execute an individual transaction against the datasource.
        ///// 1'st phase of a 2 phase database commit.
        ///// </summary>
        protected virtual void ExecuteTransactionToDataSource(ITransactionalBusinessObject transaction)
        {
            _executedTransactions.Add(transaction);
        }
    }
}