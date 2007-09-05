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