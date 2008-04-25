using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// This base class manages and commits a collection of ITransactions to a datasource. 
    /// The sub classes of this class implement a specific strategy e.g. Committing to a 
    /// database.
    /// </summary>
    public abstract class TransactionCommitter   
    {
        private readonly List<TransactionalBusinessObject> _originalTransactions = new List<TransactionalBusinessObject>();
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitter");
        protected List<TransactionalBusinessObject> _executedTransactions = new List<TransactionalBusinessObject>();
        protected bool _CommittSuccess = false;

        ///<summary>
        /// Constructs the TransactionCommitter
        ///</summary>
        public TransactionCommitter()
        {
            _executedTransactions = new List<TransactionalBusinessObject>();
        }

        ///<summary>
        /// Returns a list of transactions that will be committed.
        ///</summary>
        internal List<TransactionalBusinessObject> OriginalTransactions
        {
            get { return _originalTransactions; }
        }

        ///<summary>
        /// Returns true if the transaction was successfully committed to the databasource.
        /// Else returns false. If no attempt to commit has been made will return false
        ///</summary>
        public bool CommittSuccess
        {
            get { return _CommittSuccess; }
        }

        /// 
        ///<summary>
        /// This method adds an TransactionalBusinessObject to the list of transactions.
        ///</summary>
        ///<param name="transaction"></param>
        internal void AddTransaction(TransactionalBusinessObject transaction)
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
            UpdateObjectBeforePersisting();
            ValidateTransactionCanBePersisted();
            BeginDataSource();
        }

        private  void UpdateObjectBeforePersisting()
        {
            foreach (TransactionalBusinessObject transaction in _originalTransactions.ToArray())
            {
                transaction.UpdateObjectBeforePersisting(this);
            }
        }

        /// <summary>
        /// Validates each Itransactional object in the collection and builds up a
        /// list of errors of error messages.
        /// </summary>
        private void ValidateTransactionCanBePersisted()
        {
            CheckObjectsAreValid();
            ValidateObjectsCanBeDeleted();
            CheckForDuplicateObjects();
            CheckForOptimisticConcurrencyErrors();
        }

        private void CheckForOptimisticConcurrencyErrors()
        {
            
            foreach (TransactionalBusinessObject transaction in _originalTransactions)
            {
                transaction.CheckForConcurrencyErrors();
            }
        }

        private void CheckForDuplicateObjects()
        {
            string allMessages = "";
            foreach (TransactionalBusinessObject transaction in _originalTransactions)
            {
                string errMsg;
                if (transaction.HasDuplicateIdentifier(out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }

            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusObjDuplicateConcurrencyControlException(allMessages);
            }
        }


        private void CheckObjectsAreValid()
        {
            string allMessages = "";
            foreach (TransactionalBusinessObject transaction in _originalTransactions)
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
        }

        private void ValidateObjectsCanBeDeleted()
        {
            string allMessages = "";
            foreach (TransactionalBusinessObject transaction in _originalTransactions)
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
                foreach (TransactionalBusinessObject transaction in _originalTransactions)
                {
                    ExecuteTransactionToDataSource(transaction);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error rolling back transaction: " + Environment.NewLine +
                    ExceptionUtilities.GetExceptionString(ex, 4, true));
                TryRollback();
                UpdateTransactionsAsRolledBack();
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
            if (_CommittSuccess)
            {
                UpdateTransactionsCommited();
            }else
            {
                UpdateTransactionsAsRolledBack();
            }
        }

        private void UpdateTransactionsAsRolledBack()
        {
            foreach (TransactionalBusinessObject transaction in _originalTransactions)
            {
                transaction.UpdateAsRolledBack();
            }
            foreach (TransactionalBusinessObject transaction in _executedTransactions)
            {
                transaction.UpdateAsRolledBack();
            }
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
            foreach (TransactionalBusinessObject transaction in _executedTransactions)
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

        ///// <summary>
        ///// Tries to execute an individual transaction against the datasource.
        ///// 1'st phase of a 2 phase database commit.
        ///// </summary>
        protected virtual void ExecuteTransactionToDataSource(TransactionalBusinessObject transaction)
        {
            _executedTransactions.Add(transaction);
        }

        ///<summary>
        /// Add an object of type business object to the transaction.
        /// The DBTransactionCommiter wraps this Business Object in the
        /// appropriate Transactional Business Object
        ///</summary>
        ///<param name="bo"></param>
        public virtual void AddBusinessObject(BusinessObject bo)
        {
            this.AddTransaction(new TransactionalBusinessObject(bo));
        }
    }
}