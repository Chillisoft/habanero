using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
	[TestFixture]
	public class TestSqlFormatterForSqlServerCe
	{
		[Test]
		public void GetLimitClauseCriteriaForBegin_ShouldFormatTopWithBrackets()
		{
			//---------------Set up test pack-------------------
			SqlFormatterForSqlServerCe sqlFormatter = new SqlFormatterForSqlServerCe("[", "]", "TOP", "");
			int limit = TestUtil.GetRandomInt();
			//---------------Assert Precondition----------------
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			//---------------Execute Test ----------------------
			string limitClauseCriteriaForBegin = sqlFormatter.GetLimitClauseCriteriaForBegin(limit);
			//---------------Test Result -----------------------
			string expected = string.Format("TOP({0})", limit);
			Assert.AreEqual(expected,limitClauseCriteriaForBegin);
		}
	}
}