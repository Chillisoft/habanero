using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.UI.Generic.v2
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
            if (itsClauseOperator == FilterClauseOperator.OpLike)
            {
                throw new CoreBizArgumentException("clauseOperator",
                                                   "Operator Like is not supported for non string operands");
            }
        }

        /// <summary>
        /// Returns the filter value as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        protected override string CreateValueClause()
        {
            return itsFilterValue.ToString();
        }
    }
}
