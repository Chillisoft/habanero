//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
    /// database, file, message queue, Webservice etc.
    /// This provides an Implementation of the 'Unit of Work' Pattern 
    /// (Fowler - Patterns of Enterprise Application Architecture 184 
    ///   ‘Maintains a list of objects affected by a business transaction and co-ordinates 
    ///   the writing out of changes and the detection and resolution of concurrency problems’).
    /// 
    /// The TransactionCommitter also implements the GOF Strategy Pattern and as such 
    ///   the transaction committer can be implemented with a concrete class or the ITransactionCommitter
    ///   can be implemented by the Application developer to provide any functionality required for the 
    ///   updating of business objects to a datastore.
    /// The TransactionCommitter works with the <see cref="TransactionalBusinessObject"/>. 
    /// The <see cref="TransactionalBusinessObject"/> implements the GOF adaptor pattern. 
    ///   As well as the Fowler - DataMapper 165 pattern. As such it
    ///   wraps the business object and uses the Class definitions (MetaData Mapping Fowler 306) to map the
    ///   business object to the Datastore. It also provides methods to call through to underlying Business
    ///   object methods.
    /// 
    /// The TransactionCommitter and <see cref="TransactionalBusinessObject"/> also work together to ensure that 
    ///   all concurrency control <see cref="IConcurrencyControl"/> strategies for the business object have been 
    ///   implemented.
    /// 
    /// The Application developer can also add Transactions to the TransactionCommitter that are not Business objects
    ///   these objects must implement the <see cref="ITransactional"/> interface. This is typically used 
    ///   when the application developer needs to insert or updated a datasource that is not wrapped by a business object.
    ///   E.g. The application developer may implement a NumberGenerator to generate a code e.g. Product code.
    ///   The Habanero Framework uses this capability to write out <see cref="TransactionLogTable"/>.
    /// When <see cref="CommitTransaction"/> is called all the objects in the TransactionCommitter are executed to the
    ///   datasource in the case of the <see cref="TransactionCommitterDB"/> these are executed within an individual 
    ///   transaction if the transaction fails then all updates to the database are rolled back.
    /// 
    /// In cases where a single object is edited and persisted the Transaction committer does not have to be 
    ///   used by the Application developer. The architecture uses a convenience method
    ///   <see cref="BusinessObject"/>  <see cref="BusinessObject.Save"/> this 
    ///   creates the appropriate transactionCommitter and commits it.
    /// 
    /// The TransactionCommitter is very simple to use the Application developer can add the required objects to
    ///   Transaction Committer. When the business transaction is complete the <see cref="CommitTransaction"/> is called.
    /// <example>
    ///        ContactPerson contactP = New ContactPerson();
    ///        //set relevant data for contact person.
    ///        committerDB.AddBusinessObject(contactP);
    ///        committerDB.CommitTransaction();
    /// </example>
    /// </summary>
    public abstract class TransactionCommitter : ITransactionCommitter
    {
        /// <summary>
        /// A list of all the transactions that are added to the transaction committer by the application developer.
        /// </summary>
        private readonly List<ITransactional> _originalTransactions = new List<ITransactional>();
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitter");
        /// <summary>
        /// A list of all transactions that are executed by the transaction committer.
        /// The executed transactions may exceed the origionalTransactions due to the fact that the
        /// Business object can add additional Business objects to the transaction. This is typically 
        /// done when a business object has composite children objects and it controls the persisting of these children.
        /// E.g. in the case where an Invoice has InvoiceLines the Invoice will add any of its lines that are dirty to 
        /// the _executedTransactions list.
        /// </summary>
        protected List<ITransactional> _executedTransactions = new List<ITransactional>();
        /// <summary>
        /// A flag to indicate whether the commitToDataSource was successful or not.s
        /// </summary>
        protected bool _commitSuccess;
        private bool _runningUpdatingBeforePersisting;

        ///<summary>
        /// Constructs the TransactionCommitter
        ///</summary>
        protected TransactionCommitter()
        {
            _executedTransactions = new List<ITransactional>();
        }

        ///<summary>
        /// Add an object of type business object to the transaction.
        /// The DBTransactionCommiter wraps this Business Object in the
        /// appropriate Transactional Business Object
        ///</summary>
        ///<param name="businessObject"></param>
        public virtual void AddBusinessObject(IBusinessObject businessObject)
        {
            TransactionalBusinessObject transaction = CreateTransactionalBusinessObject(businessObject);
            this.AddTransaction(transaction);
            bool added = _originalTransactions.Contains(transaction);
            if (added && _runningUpdatingBeforePersisting)
            {
                transaction.UpdateObjectBeforePersisting(this);
            }
        }

        /// 
        ///<summary>
        /// This method adds an <see cref="ITransactional"/> to the list of transactions.
        ///</summary>
        ///<param name="transaction"></param>
        public void AddTransaction(ITransactional transaction)
        {
            ITransactional foundTransactional = _originalTransactions.Find(delegate(ITransactional obj)
            {
                return obj.TransactionID() == transaction.TransactionID();
            });
            if (foundTransactional != null) return;
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
        public bool CommitSuccess
        {
            get { return _commitSuccess; }
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

        /// <summary>
        /// If the transaction is a transactional business object then the Upate object before persisting method
        /// of the transactional business object is called. The default behaviour of this is to call through to the 
        ///  UpdateObjectBeforePersisting method of the business object.
        /// </summary>
        private void UpdateObjectBeforePersisting()
        {
            _runningUpdatingBeforePersisting = true;
            try
            {
                foreach (ITransactional transaction in _originalTransactions.ToArray())
                {
                    if (!(transaction is TransactionalBusinessObject)) continue;
                    TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;
                    trnBusObj.UpdateObjectBeforePersisting(this);
                }
            }
            finally
            {
                _runningUpdatingBeforePersisting = false;
            }
        }

        /// <summary>
        /// Validates each Itransactional object in the collection and builds up a
        /// list of errors of error messages.
        /// </summary>
        private void ValidateTransactionCanBePersisted()
        {
            CheckObjectsAreValid();
            CheckTransactionsCanBePersisted();
            ValidateObjectsCanBeDeleted();
            CheckForDuplicateObjects();
            CheckForConcurrencyErrors();
        }
        /// <summary>
        /// Verifies that any <see cref="TransactionalBusinessObject"/>'s that are marked can be persisted
        ///   can be deleted. <see cref="TransactionalBusinessObject.CheckCanDelete"/>
        /// </summary>
        private void CheckTransactionsCanBePersisted()
        {
            string allMessages = "";
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (!(transaction is TransactionalBusinessObject)) continue;
                TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject)transaction;
                string errMsg;
                if (!trnBusObj.CanBePersisted(out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }
            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusObjPersistException(allMessages);
            }
        }

        /// <summary>
        /// Checks any Concurrency control errors for the <see cref="TransactionalBusinessObject"/>
        /// </summary>
        private void CheckForConcurrencyErrors()
        {
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (!(transaction is TransactionalBusinessObject)) continue;
                TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;
                trnBusObj.CheckForConcurrencyErrors();
            }
        }

        /// <summary>
        /// Checks for any Duplicate objects by checking the objects <see cref="IPrimaryKey"/> and alternate keys 
        /// <see cref="IKeyDef"/>.
        /// </summary>
        private void CheckForDuplicateObjects()
        {
            string allMessages = "";
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (!(transaction is TransactionalBusinessObject)) continue;
                TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                string errMsg;
                if (trnBusObj.HasDuplicateIdentifier(out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }

            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusObjDuplicateConcurrencyControlException(allMessages);
            }
        }

        /// <summary>
        /// Check that the object is in a valid state i.e. no <see cref="IPropRule"/>'s are broken.
        /// </summary>
        private void CheckObjectsAreValid()
        {
            string allMessages = "";
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (!(transaction is TransactionalBusinessObject)) continue;
                TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                string errMsg;
                if (!trnBusObj.IsValid(out errMsg))
                {
                    allMessages = Util.StringUtilities.AppendMessage(allMessages, errMsg);
                }
            }

            if (!string.IsNullOrEmpty(allMessages))
            {
                throw new BusObjectInAnInvalidStateException(allMessages);
            }
        }

        /// <summary>
        /// Verifies that any <see cref="TransactionalBusinessObject"/>'s that are marked for deletion
        ///   can be deleted. <see cref="TransactionalBusinessObject.CheckCanDelete"/>
        /// </summary>
        private void ValidateObjectsCanBeDeleted()
        {
            string allMessages = "";
            foreach (ITransactional transaction in _originalTransactions)
            {
                if (!(transaction is TransactionalBusinessObject)) continue;
                TransactionalBusinessObject trnBusObj = (TransactionalBusinessObject) transaction;

                if (!trnBusObj.IsDeleted) continue;

                string errMsg;

                if (!trnBusObj.CheckCanDelete(out errMsg))
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
            if (_commitSuccess)
            {
                UpdateTransactionsAsCommited();
            }
            else
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
        protected void UpdateTransactionsAsCommited()
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



        /// <summary>
        /// Used to decorate a businessObject in a TransactionalBusinessObject. To be overridden in the concrete 
        /// implementation of a TransactionCommitter depending on the type of transaction you need.
        /// </summary>
        /// <param name="businessObject">The business object to decorate</param>
        /// <returns>A decorated Business object (TransactionalBusinessObject)</returns>
        protected abstract TransactionalBusinessObject CreateTransactionalBusinessObject(IBusinessObject businessObject);
    }

}