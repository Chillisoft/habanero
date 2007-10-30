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

using CoreBiz.Db;

namespace CoreBiz.Bo {
	/// <summary>
	/// Summary description for ITransaction.
	/// </summary>
	public interface ITransaction {
		void TransactionCommited();
		SqlStatementCollection GetPersistSql();
		void CheckPersistRules();
		void TransactionRolledBack();
		void TransactionCancelEdits();
		int TransactionRanking();
		string StrID();
		void BeforeCommit();
		void AfterCommit();
	}
}