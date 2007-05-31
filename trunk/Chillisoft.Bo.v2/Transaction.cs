using System;
using System.Collections;
using System.Data;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;
using log4net;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Manages a collection of transactions to commit to the database
    /// </summary>
    public class Transaction
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.v2.Transaction");

        private SortedList colTransactions = null;
        private IDatabaseConnection itsDatabaseConnection;
        private IList listTransactions;

        /// <summary>
        /// Constructor to initialise a transaction
        /// </summary>
        public Transaction() : this(DatabaseConnection.CurrentConnection)
        {
        }

        /// <summary>
        /// Constructor to initialise a transaction with a specified
        /// database connection
        /// </summary>
        /// <param name="databaseConnection">A database connection</param>
        public Transaction(IDatabaseConnection databaseConnection)
        {
            itsDatabaseConnection = databaseConnection;
            ClearTransactionCol();
        }

        /// <summary>
        /// Clears the collection of transactions
        /// </summary>
        private void ClearTransactionCol()
        {
            colTransactions = new SortedList();
            listTransactions = new ArrayList();
        }

        /// <summary>
        /// Adds an Itransaction object to the collection of transactions
        /// </summary>
        /// <param name="transaction">An Itransaction object</param>
        public void AddTransactionObject(ITransaction transaction)
        {
            //check if the transaction object is in a valid state before adding to the col
            transaction.CheckPersistRules();
            //if the transaction already exists then ignore
            if (!colTransactions.ContainsKey(transaction.StrID()))
            {
                colTransactions.Add(transaction.StrID(), transaction);
                listTransactions.Add(transaction);
            }
        }

        /// <summary>
        /// Rolls back all transactions in the collection
        /// </summary>
        public void CancelAllEdits()
        {
            ITransaction transaction;
            foreach (DictionaryEntry item in colTransactions)
            {
                transaction = (ITransaction) item.Value;
                transaction.TransactionRolledBack();
            }
            TransactionRolledBack();
            ClearTransactionCol();
        }

        /// <summary>
        /// Rolls back all transactions in the collection
        /// </summary>
        /// TODO ERIC - this method surprises me - it completely duplicates
        /// the above (why not put it inside the above)
        /// - could remove duplication in method above
        private void TransactionRolledBack()
        {
            ITransaction transaction;
            foreach (DictionaryEntry item in colTransactions)
            {
                transaction = (ITransaction) item.Value;
                transaction.TransactionRolledBack();
            }
        }

        /// <summary>
        /// Commits all transactions in the collection to the database
        /// </summary>
        public void CommitTransaction()
        {
            IDbConnection connection = DatabaseConnection.CurrentConnection.GetConnection();
            connection.Open();

            IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            //ITransaction transaction;
            try
            {
                foreach (ITransaction transaction in listTransactions)
                {
                    //transaction = (ITransaction)item.Value;
                    transaction.BeforeCommit();
                }
                foreach (ITransaction transaction in listTransactions)
                {
                    //transaction = (ITransaction)item.Value;
                    if (transaction.GetPersistSql().Count > 0)
                    {
                        itsDatabaseConnection.ExecuteSql(transaction.GetPersistSql(), dbTransaction);
                    }
                }
                dbTransaction.Commit();
            }
            catch (Exception e)
            {
                log.Error("Error commiting transaction: " + Environment.NewLine + ExceptionUtil.GetExceptionString(e, 4));
                dbTransaction.Rollback();
                TransactionRolledBack();
                throw e;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            foreach (ITransaction transaction in listTransactions)
            {
                //transaction = (ITransaction)item.Value;
                transaction.AfterCommit();
            }
            TransactionCommited();
            ClearTransactionCol();
        }

        /// <summary>
        /// Carries out final steps on all transactions in the collection
        /// after they have been committed
        /// </summary>
        private void TransactionCommited()
        {
            ITransaction transaction;
            foreach (DictionaryEntry item in colTransactions)
            {
                transaction = (ITransaction) item.Value;
                transaction.TransactionCommited();
            }
        }

        /// <summary>
        /// Cancels edits for all transactions in the collection
        /// </summary>
        /// TODO ERIC - this seems to duplicate CancelAllEdits() - one of them
        /// needs to be renamed
        public void CancelEdits()
        {
            ITransaction transaction;
            foreach (DictionaryEntry item in colTransactions)
            {
                transaction = (ITransaction) item.Value;
                transaction.TransactionCancelEdits();
            }
        }
    }
}