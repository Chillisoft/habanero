using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Creates new filter clauses for different data types
    /// </summary>
    public class DataViewFilterClauseFactory : FilterClauseFactory
    {
        /// <summary>
        /// Create a new filter clause that filters string values
        /// </summary>
        /// <param name="filterColumn">The filter column</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateStringFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                     string filterValue)
        {
            return new DataViewStringFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Create a new filter clause that filters integer values
        /// </summary>
        /// <param name="filterColumn">The filter column</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateIntegerFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                      int filterValue)
        {
            return new DataViewIntegerFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Create a new composite filter clause
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateCompositeFilterClause(FilterClause leftClause,
                                                        FilterClauseCompositeOperator compositeOperator,
                                                        FilterClause rightClause)
        {
            return new DataViewCompositeFilterClause(leftClause, compositeOperator, rightClause);
        }

        /// <summary>
        /// Create a new null filter clause
        /// </summary>
        public FilterClause CreateNullFilterClause()
        {
            return new DataViewNullFilterClause();
        }
    }
}