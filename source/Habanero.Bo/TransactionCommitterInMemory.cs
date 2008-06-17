using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{

    public class TransactionCommitterInMemory : TransactionCommitter
    {
        private readonly DataStoreInMemory _dataStoreInMemory;

        public TransactionCommitterInMemory(DataStoreInMemory dataStoreInMemory)
        {
            _dataStoreInMemory = dataStoreInMemory;
        }

        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
            
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override void CommitToDatasource()
        {
            UpdateTransactionsAsCommited();
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
            
        }

        /// <summary>
        /// Used to decorate a businessObject in a TransactionalBusinessObject. To be overridden in the concrete 
        /// implementation of a TransactionCommitter depending on the type of transaction you need.
        /// </summary>
        /// <param name="businessObject">The business object to decorate</param>
        /// <returns>A decorated Business object (TransactionalBusinessObject)</returns>
        protected override TransactionalBusinessObject CreateTransactionalBusinessObject(IBusinessObject businessObject)
        {
           return new TransactionalBusinessObject(businessObject);
        }

        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            if (transaction is TransactionalBusinessObject)
            {
                IBusinessObject businessObject = ((TransactionalBusinessObject) transaction).BusinessObject;
                if (!_dataStoreInMemory.AllObjects.ContainsKey(businessObject.PrimaryKey))
                {
                    _dataStoreInMemory.Add(businessObject);
                }
                else if (businessObject.State.IsDeleted) _dataStoreInMemory.Remove(businessObject);
            }
            base.ExecuteTransactionToDataSource(transaction);
        }
    }
}