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
		bool AddTransactionObject(ITransaction transaction);

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
