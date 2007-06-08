using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Creates filter clauses that determine which rows of data are displayed
    /// </summary>
    public class DataViewFilterClauseFactory : FilterClauseFactory
    {
        /// <summary>
        /// Creates a new filter clause that filters string values
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateStringFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                     string filterValue)
        {
            return new DataViewStringFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Creates a new filter clause that filters integer values
        /// </summary>
        /// <param name="filterColumn">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateIntegerFilterClause(string filterColumn, FilterClauseOperator clauseOperator,
                                                      int filterValue)
        {
            return new DataViewIntegerFilterClause(filterColumn, clauseOperator, filterValue);
        }

        /// <summary>
        /// Creates a new composite filter clause combining two given filter
        /// clauses the operator provided
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator, such as
        /// "and" or "or"</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        public FilterClause CreateCompositeFilterClause(FilterClause leftClause,
                                                        FilterClauseCompositeOperator compositeOperator,
                                                        FilterClause rightClause)
        {
            return new DataViewCompositeFilterClause(leftClause, compositeOperator, rightClause);
        }

        /// <summary>
        /// Creates a new null filter clause, which does no filtering
        /// </summary>
        public FilterClause CreateNullFilterClause()
        {
            return new DataViewNullFilterClause();
        }
    }
}
