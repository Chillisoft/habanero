using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a filter clause that filters which data to
    /// display in a DataView, according to some criteria set on an integer column
    /// </summary>
    public class DataViewEnumFilterClause : DataViewFilterClause
    {
        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to compare to</param>
        internal DataViewEnumFilterClause(string filterColumn, FilterClauseOperator clauseOperator, object filterValue)
            : base(filterColumn, clauseOperator, filterValue)
        {
            if (_clauseOperator == FilterClauseOperator.OpLike)
            {
                throw new HabaneroArgumentException("clauseOperator",
                                                    "Operator Like is not supported for non string operands");
            }
        }

        /// <summary>
        /// Returns the value part of the clause
        /// </summary>
        /// <returns>Returns a string</returns>
        protected override string CreateValueClause(string stringLikeDelimiter, string dateTimeDelimiter)
        {
            return "'" + _filterValue.ToString() + "'";
        }
    }
}