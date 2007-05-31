namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// An interface to model a class that creates filter clauses
    /// </summary>
    /// TODO ERIC - rename to IFilterClauseFactory
    public interface FilterClauseFactory
    {
        /// <summary>
        /// Create a new filter clause that filters string values
        /// </summary>
        /// <param name="filterColumn">The filter column</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value</param>
        /// <returns>Returns the new filter clause object</returns>
        FilterClause CreateStringFilterClause(string columnName, FilterClauseOperator clauseOperator, string filterValue);

        /// <summary>
        /// Create a new filter clause that filters integer values
        /// </summary>
        /// <param name="filterColumn">The filter column</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value</param>
        /// <returns>Returns the new filter clause object</returns>
        FilterClause CreateIntegerFilterClause(string columnName, FilterClauseOperator clauseOperator, int filterValue);

        /// <summary>
        /// Create a new composite filter clause
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        FilterClause CreateCompositeFilterClause(FilterClause leftClause,
                                                 FilterClauseCompositeOperator compositeOperator,
                                                 FilterClause rightClause);

        /// <summary>
        /// Create a new null filter clause
        /// </summary>
        FilterClause CreateNullFilterClause();
    }
}