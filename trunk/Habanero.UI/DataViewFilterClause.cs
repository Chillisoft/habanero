using Habanero.Generic;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// A super-class for filter clauses that filter which data to
    /// display in a DataView, according to some criteria
    /// </summary>
    public abstract class DataViewFilterClause : FilterClause
    {
        protected readonly object _filterValue;
        protected readonly FilterClauseOperator _clauseOperator;
        protected readonly string _filterColumn;

        /// <summary>
        /// Constructor to create a new filter clause
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to compare to</param>
        protected DataViewFilterClause(string filterColumn, FilterClauseOperator clauseOperator, object filterValue)
        {
            _filterColumn = filterColumn;
            _clauseOperator = clauseOperator;
            _filterValue = filterValue;
        }

        /// <summary>
        /// Returns the full filter clause as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetFilterClauseString()
        {
            return CreateColumnClause() + CreateOperatorClause() + CreateValueClause();
        }

        /// <summary>
        /// Returns the value part of the clause
        /// </summary>
        /// <returns>Returns a string</returns>
        protected abstract string CreateValueClause();

        /// <summary>
        /// Returns the column part of the clause.  If the column contains any
        /// spaces or dashes, it is surrounded by square brackets.
        /// </summary>
        /// <returns>Returns a string</returns>
        private string CreateColumnClause()
        {
            if (_filterColumn.IndexOf(' ') == -1 && _filterColumn.IndexOf('-') == -1)
            {
                return _filterColumn;
            }
            else
            {
                return "[" + _filterColumn + "]";
            }
        }

        /// <summary>
        /// Returns the operator in the clause as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        /// TODO ERIC - shouldn't this throw an error on default?
        private string CreateOperatorClause()
        {
            string opClause;
            switch (_clauseOperator)
            {
                case FilterClauseOperator.OpEquals:
                    opClause = " = ";
                    break;
                case FilterClauseOperator.OpLike:
                    opClause = " like ";
                    break;
                case FilterClauseOperator.OpGreaterThanOrEqualTo:
                    opClause = " >= ";
                    break;
                case FilterClauseOperator.OpLessThanOrEqualTo:
                    opClause = " <= ";
                    break;
                default:
                    opClause = " <unsupported operator> ";
                    break;
            }
            return opClause;
        }
    }
}
