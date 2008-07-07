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
        private readonly List<ITransactional> _originalTransactions = new List<ITransactional>();
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitter");

        protected List<ITransactional> _executedTransactions = new List<ITransactional>();

        protected bool _CommittSuccess = false;

        ///<summary>
        /// Constructs the TransactionCommitter
        ///</summary>
        public TransactionCommitter()
        {
            _executedTransactions = new List<ITransactional>();
        }
        /// <summary>
        /// A List of all the transactions that where actually committed to the data source. This will include any updates required
        /// as a result of concurrency control, transaction logging, or deleting or dereferrencing children.
        /// </summary>
        internal List<ITransactional> ExecutedTransactions
        {
            get { return _executedTransactions; }
        }

        ///<summary>
        /// Returns a list of transactions that will be committed. (i.e. the list that the user origionally added.
        ///</summary>
        internal List<ITransactional> OriginalTransactions
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
        /// This method adds an ITransactional to the list of transactions.
        ///</summary>
        ///<param name="transaction"></param>
        public void AddTransaction(ITransactional transaction)
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
            foreach (ITransactional transaction in _originalTransactions.ToArray())
            {
                if (transaction is TransactionalBusinessObject)
                {
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;
                    trnBusObj.UpdateObjectBeforePersisting(this);
                }
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
            
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (transaction is TransactionalBusinessObject)
                {
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;
                    trnBusObj.CheckForConcurrencyErrors();
                }
            }
        }

        private void CheckForDuplicateObjects()
        {
            string allMessages = "";
            foreach (ITransactional transaction in _originalTransactions)
            {
                string errMsg;
                if (transaction is TransactionalBusinessObject)
                {
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                    if (trnBusObj.HasDuplicateIdentifier(out errMsg))
                    {
                        allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                    }
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
            foreach (ITransactional transaction in _originalTransactions)
            {
                string errMsg;
                if (transaction is TransactionalBusinessObject)
                {
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                    if (!trnBusObj.IsValid(out errMsg))
                    {
                        allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                    }
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
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (transaction is TransactionalBusinessObject)
                {
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                    if (!trnBusObj.IsDeleted) continue;

                    string errMsg;

                    if (!trnBusObj.CheckCanDelete(out errMsg))
                    {
                        allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                    }

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
                foreach (ITransactional transaction in _originalTransactions)
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
                UpdateTransactionsAsCommited();
            }else
            {
                UpdateTransactionsAsRolledBack();
            }
        }

        private void UpdateTransactionsAsRolledBack()
        {
            foreach (ITransactional transaction in _originalTransactions)
            {
                transaction.UpdateAsRolledBack();
            }
            foreach (ITransactional transaction in _executedTransactions)
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
        private void UpdateTransactionsAsCommited()
        {
            foreach (ITransactional transaction in _executedTransactions)
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
        protected virtual void ExecuteTransactionToDataSource(ITransactional transaction)
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