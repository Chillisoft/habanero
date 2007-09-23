using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
	///<summary>
	/// This is the interface for a class that is used to commit 
	/// transactions to the relevent data source.
	///</summary>
	public interface ITransactionCommitter
	{
		/// <summary>
		/// Adds an Itransaction object to the collection of transactions
		/// </summary>
		/// <param name="transaction">An Itransaction object</param>
		void AddTransactionObject(ITransaction transaction);

		///<summary>
		/// This returns the number of rows that were affected by the 
		/// transaction
		///</summary>
		int NumberOfRowsUpdated{ get; }

		/// <summary>
		/// Cancels edits for all transactions in the collection
		/// </summary>
		void CancelEdits();

		/// <summary>
		/// Commits all transactions in the collection to the database
		/// </summary>
		void CommitTransaction();

		/// <summary>
		/// Rolls back all transactions in the collection
		/// </summary>
		void CancelTransaction();
	}
}
