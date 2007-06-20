namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a database transaction and some of its
    /// associated functions
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Carries out final steps on all transactions in the collection
        /// after they have been committed
        /// </summary>
        void TransactionCommited();

        /// <summary>
        /// Returns the sql statement collection needed to carry out 
        /// persistance to the database</summary>
        /// <returns>Returns an ISqlStatementCollection object</returns>
        ISqlStatementCollection GetPersistSql();

        /// <summary>
        /// Checks a number of rules, including concurrency, duplicates and
        /// duplicate primary keys
        /// </summary>
        void CheckPersistRules();

        /// <summary>
        /// Rolls back the transactions
        /// </summary>
        void TransactionRolledBack();

        /// <summary>
        /// Cancels the edit
        /// </summary>
        void TransactionCancelEdits();

        /// <summary>
        /// Returns the transaction ranking
        /// </summary>
        /// <returns>Returns the ranking as an integer</returns>
        int TransactionRanking();

        /// <summary>
        /// Returns the ID as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        string StrID();

        /// <summary>
        /// Carries out additional steps before committing changes to the
        /// database
        /// </summary>
        void BeforeCommit();

        /// <summary>
        /// Carries out additional steps after committing changes to the
        /// database
        /// </summary>
        void AfterCommit();
    }
}