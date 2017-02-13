using Habanero.Base;

namespace Habanero.DB
{
	///<summary>
	/// Used to store specific SQL formatting information for an SQL Server Compact Edition database.
	/// Typically databases differ in the characters used to differentiate fields and tables e.g. [ and ] for ms sql and
	/// ` for MySQL.
	///</summary>
	public class SqlFormatterForSqlServerCe : SqlFormatter
	{
		///<summary>
		/// Constructor of a sql formatter
		///</summary>
		///<param name="leftFieldDelimiter">The left field delimiter to be used for formatting a sql statement</param>
		///<param name="rightFieldDelimiter">The right field delimiter to be used for formatting a sql statement</param>
		///<param name="limitClauseAtBeginning"></param>
		///<param name="limitClauseAtEnd"></param>
		public SqlFormatterForSqlServerCe(string leftFieldDelimiter, string rightFieldDelimiter, string limitClauseAtBeginning, string limitClauseAtEnd)
			: base(leftFieldDelimiter, rightFieldDelimiter, limitClauseAtBeginning, limitClauseAtEnd)
		{
		}

	    /// <summary>
	    /// Returns the beginning limit clause with the limit specified
	    /// </summary>
	    /// <param name="limit">The limit</param>
	    /// <returns>Returns a string</returns>
	    public override string GetLimitClauseCriteriaForBegin(int limit)
		{
			return string.IsNullOrEmpty(LimitClauseAtBeginning) ? "" : string.Format("{0}({1})", LimitClauseAtBeginning, limit);
		}
	}
}