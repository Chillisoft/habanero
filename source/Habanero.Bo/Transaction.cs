//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Habanero.Base.Exceptions;
using Habanero.DB;
using Habanero.Base;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a collection of transactions to commit to the database
    /// </summary>
	public class Transaction : ITransactionCommitter, ITransaction
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.Transaction");

        private IDatabaseConnection _databaseConnection;
		private SortedDictionary<string, ITransaction> _transactions;
        private List<ITransaction> _transactionsList;
		private Guid _transactionId = Guid.NewGuid();
    	private int _numberOfRowsUpdated;

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
            _databaseConnection = databaseConnection;
            ClearTransactionCol();
        }

        /// <summary>
        /// Adds an Itransaction object to the collection of transactions
        /// </summary>
        /// <param name="transaction">An Itransaction object</param>
        public bool AddTransactionObject(ITransaction transaction)
        {
			////check if the transaction object is in a valid state before adding to the col
			//transaction.CheckPersistRules();
			bool mustAdd = transaction.AddingToTransaction(this);
            //if the transaction already exists then ignore
			if (mustAdd && !_transactions.ContainsKey(transaction.StrID()))
            {
				_transactions.Add(transaction.StrID(), transaction);
                _transactionsList.Add(transaction);
            }

     
            return mustAdd;
        }

        /// <summary>
        /// Clears the collection of transactions
        /// </summary>
        private void ClearTransactionCol()
        {
        	_numberOfRowsUpdated = 0;
			_transactions = new SortedDictionary<string, ITransaction>();
            _transactionsList = new List<ITransaction>();
        }

        /// <summary>
        /// Rolls back all transactions in the collection
        /// </summary>
        public void CancelTransaction()
        {
            TransactionRolledBack();
            ClearTransactionCol();
        }
		
    	///<summary>
    	/// This returns the number of rows that were affected by the 
    	/// transaction
    	///</summary>
    	public int NumberOfRowsUpdated
    	{
    		get { return _numberOfRowsUpdated; }
    	}

    	/// <summary>
		/// Cancels edits for all transactions in the collection
		/// </summary>
		public void CancelEdits()
		{
			TransactionCancelEdits();
		}

    	
    	/// <summary>
        /// Commits all transactions in the collection to the database
        /// </summary>
		public void CommitTransaction()
    	{
    		IDbConnection connection = _databaseConnection.GetConnection();
    		//IDbConnection connection = DatabaseConnection.CurrentConnection.GetConnection();
    		connection.Open();
    		using (IDbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
    		{
    			try
    			{
    				BeforeCommit(this);
    				SqlStatementCollection statementCollection = GetPersistSql();
    				if (statementCollection.Count > 0)
    				{
    					_numberOfRowsUpdated = _databaseConnection.ExecuteSql(statementCollection, dbTransaction);
    					dbTransaction.Commit();
    				}
    				else
    				{
    					_numberOfRowsUpdated = 0;
    				}
    				AfterCommit();
    			}
    			catch (Exception e)
    			{
    				log.Error("Error commiting transaction: " + Environment.NewLine +
    				          ExceptionUtilities.GetExceptionString(e, 4, true));
    				dbTransaction.Rollback();
    				TransactionRolledBack();
    				throw;
    			}
    			finally
    			{
    				if (connection != null && connection.State == ConnectionState.Open)
    				{
    					connection.Close();
    				}
    			}
    		}
    		TransactionCommited();
    		ClearTransactionCol();
    	}

    	#region ITransaction Implementation

		/// <summary>
        /// Carries out final steps on all transactions in the collection
        /// after they have been committed
        /// </summary>
        private void TransactionCommited()
        {
			foreach (ITransaction transaction in _transactionsList)
            {
                transaction.TransactionCommitted();
            }
        }

		/// <summary>
		/// Returns the sql statement collection needed to carry out 
		/// persistance to the database</summary>
		/// <returns>Returns an ISqlStatementCollection object</returns>
		private SqlStatementCollection GetPersistSql()
		{
			SqlStatementCollection statementCollection;
			statementCollection = new SqlStatementCollection();
			foreach (ITransaction transaction in _transactionsList)
			{
				ISqlStatementCollection thisStatementCollection = transaction.GetPersistSql();
				statementCollection.Add(thisStatementCollection);
			}
			return statementCollection;
		}

		/// <summary>
		/// Rolls back all transactions in the collection
		/// </summary>
		private void TransactionRolledBack()
		{
			foreach (ITransaction transaction in _transactionsList)
			{
				transaction.TransactionRolledBack();
			}
		}

		/// <summary>
		/// Cancels the edit
		/// </summary>
		private void TransactionCancelEdits()
		{
            foreach (ITransaction transaction in _transactionsList)
			{
				transaction.TransactionCancelEdits();
			}
		}

		/// <summary>
		/// Carries out additional steps before committing changes to the
		/// database, and returns true if it is ok to continue.
		/// If false is returned, then the commit will be aborted.
		/// </summary>
		/// <returns>Returns an indication of whether it is 
		/// ok to continue with the commit.</returns>
        private void BeforeCommit(ITransactionCommitter transactionCommitter)
		{
		    int i = 0;
		    do {
                int j = _transactionsList.Count;

		        List<ITransaction> transactions = _transactionsList.GetRange(i, j - i);
                transactions.ForEach(delegate(ITransaction obj) { obj.BeforeCommit(this); });
		        i = j;
            } while (i < _transactionsList.Count); 
		}

    	/// <summary>
    	/// Carries out additional steps after committing changes to the
    	/// database
    	/// </summary>
    	private void AfterCommit()
		{
            foreach (ITransaction transaction in _transactionsList)
			{
				transaction.AfterCommit();
			}
		}
		

		#endregion //ITransaction interface

    	#region ITransaction Members

    	/// <summary>
    	/// Carries out final steps on all transactions in the collection
    	/// after they have been committed
    	/// </summary>
    	void ITransaction.TransactionCommitted()
    	{
			TransactionCommited();
    	}

    	/// <summary>
    	/// Returns the sql statement collection needed to carry out 
    	/// persistance to the database</summary>
    	/// <returns>Returns an ISqlStatementCollection object</returns>
    	ISqlStatementCollection ITransaction.GetPersistSql()
    	{
			return GetPersistSql();
    	}

		/// <summary>
		/// Notifies this ITransaction object that it has been added to the 
		/// specified Transaction object
		/// </summary>
		bool ITransaction.AddingToTransaction(ITransactionCommitter transaction)
		{
			return true;
		}

		///// <summary>
		///// Checks a number of rules, including concurrency, duplicates and
		///// duplicate primary keys
		///// </summary>
		//void ITransaction.CheckPersistRules()
		//{
		//    //All transactions have their persist rules checked when
		//    // they are added the the transaction, so there is nothing 
		//    // to be checked for this Transaction's transaction collection.
		//}

    	/// <summary>
    	/// Rolls back the transactions
    	/// </summary>
    	void ITransaction.TransactionRolledBack()
    	{
			TransactionRolledBack();
    	}

    	/// <summary>
    	/// Cancels the edit
    	/// </summary>
    	void ITransaction.TransactionCancelEdits()
    	{
			TransactionCancelEdits();
    	}

    	/// <summary>
    	/// Returns the transaction ranking
    	/// </summary>
    	/// <returns>Returns the ranking as an integer</returns>
    	int ITransaction.TransactionRanking()
    	{
    		int ranking = int.MinValue;
			//Set the ranking to the maximum ranking of the transaction collection
            foreach (ITransaction transaction in _transactionsList)
    		{
    			int thisRanking = transaction.TransactionRanking();
				if (thisRanking > ranking)
				{
					ranking = thisRanking;
				}
    		}
    		return ranking;
    	}

    	/// <summary>
    	/// Returns the ID as a string
    	/// </summary>
    	/// <returns>Returns a string</returns>
    	string ITransaction.StrID()
    	{
			return _transactionId.ToString();
    	}

		/// <summary>
		/// Carries out additional steps before committing changes to the
		/// database, and returns true if it is ok to continue.
		/// If false is returned, then the commit will be aborted.
		/// </summary>
		/// <returns>Returns an indication of whether it is 
		/// ok to continue with the commit.</returns>
        void ITransaction.BeforeCommit(ITransactionCommitter transactionCommitter)
    	{
            BeforeCommit(transactionCommitter);
    	}

    	/// <summary>
    	/// Carries out additional steps after committing changes to the
    	/// database
    	/// </summary>
    	void ITransaction.AfterCommit()
    	{
			AfterCommit();
    	}

    	#endregion
    }
}