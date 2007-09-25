namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a database transaction and some of its
    /// associated functions
    /// </summary>
    public  interface ITransaction
    {
		/// <summary>
		/// Returns the ID as a string
		/// </summary>
		/// <returns>Returns a string</returns>
		string StrID();

    	/// <summary>
		/// Notifies this ITransaction object that it has been added to the 
		/// specified Transaction object.
		/// Returns true if the transaction can be added, otherwise, it returns false.
		/// </summary>
		/// <param name="transaction">The transaction committer object that this transaction is being added to.</param>
		/// <returns>Returns an indication of whether it 
		/// can be added to the transaction or not.</returns>
		bool AddingToTransaction(ITransactionCommitter transaction);

		//Mark: This mapping to a specific method is not needed,
		//  rather use AddingToTransaction, which is more general and flexible.
		///// <summary>
		///// Checks a number of rules, including concurrency, duplicates and
		///// duplicate primary keys
		///// </summary>
		//void CheckPersistRules();

		/// <summary>
		/// Carries out additional steps before committing changes to the
		/// database.
		/// </summary>
		void BeforeCommit();

		/// <summary>
		/// Returns the sql statement collection needed to carry out 
		/// persistance to the database</summary>
		/// <returns>Returns an ISqlStatementCollection object</returns>
		ISqlStatementCollection GetPersistSql();

		/// <summary>
		/// Carries out additional steps after committing changes to the
		/// database
		/// </summary>
		void AfterCommit();

		/// <summary>
        /// Carries out final steps on all transactions in the collection
        /// after they have been committed
        /// </summary>
        void TransactionCommitted();

        /// <summary>
        /// Rolls back the transactions
        /// </summary>
        void TransactionRolledBack();

        /// <summary>
        /// Cancels the edit
        /// </summary>
        void TransactionCancelEdits();
		    	
    }
}