using Habanero.Base;
using Habanero.UI.Grid;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages a filter clause that filters which data to
    /// display in a DataView, according to some criteria set on a string column
    /// </summary>
    public class DataViewStringFilterClause : DataViewFilterClause
    {
        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to compare to</param>
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
                Habanero.Util.StringUtilities.ReplaceSingleQuotesWithTwo((string) _filterValue);
            if (_clauseOperator == FilterClauseOperator.OpLike)
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