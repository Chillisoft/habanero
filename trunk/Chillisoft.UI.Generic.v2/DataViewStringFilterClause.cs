using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages a filter clause that filters strings
    /// </summary>
    public class DataViewStringFilterClause : DataViewFilterClause
    {
        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The filter column</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value</param>
        internal DataViewStringFilterClause(string filterColumn, FilterClauseOperator clauseOperator, string filterValue)
            : base(filterColumn, clauseOperator, filterValue)
        {
        }

        /// <summary>
        /// Returns the filter value as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        protected override string CreateValueClause()
        {
            string valueClause;
            string finalFilterValue =
                Chillisoft.Util.v2.StringUtilities.ReplaceSingleQuotesWithTwo((string) itsFilterValue);
            if (itsClauseOperator == FilterClauseOperator.OpLike)
            {
                valueClause = "'*" + finalFilterValue + "*'";
            }
            else
            {
                valueClause = "'" + finalFilterValue + "'";
            }
            return valueClause;
        }
    }
}