using Habanero.Base;
using Habanero.Ui.Grid;
using Habanero.Util;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// Manages a filter clause that filters which data to
    /// display in a DataView, according to some criteria set on an integer column
    /// </summary>
    public class DataViewIntegerFilterClause : DataViewFilterClause
    {
        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to compare to</param>
        internal DataViewIntegerFilterClause(string filterColumn, FilterClauseOperator clauseOperator, int filterValue)
            : base(filterColumn, clauseOperator, filterValue)
        {
            if (_clauseOperator == FilterClauseOperator.OpLike)
            {
                throw new HabaneroArgumentException("clauseOperator",
                                                    "Operator Like is not supported for non string operands");
            }
        }

        /// <summary>
        /// Returns the filter value as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        protected override string CreateValueClause()
        {
            return _filterValue.ToString();
        }
    }
}