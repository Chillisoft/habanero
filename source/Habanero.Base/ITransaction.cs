//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
		/// Returns the transaction ranking
		/// </summary>
		/// <returns>Returns the ranking as an integer</returns>
		int TransactionRanking();

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
        /// database
        /// </summary>
        void BeforeCommit(ITransactionCommitter transactionCommitter);

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
