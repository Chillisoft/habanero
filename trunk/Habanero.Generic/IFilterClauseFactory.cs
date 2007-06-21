namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a class that creates filter clauses that determine
    /// which rows of data are displayed
    /// </summary>
    public interface IFilterClauseFactory
    {
        /// <summary>
        /// Creates a new filter clause that filters string values
        /// </summary>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateStringFilterClause(string columnName, FilterClauseOperator clauseOperator, string filterValue);

        /// <summary>
        /// Creates a new filter clause that filters integer values
        /// </summary>
        /// <param name="columnName">The column of data on which to do the
        /// filtering</param>
        /// <param name="clauseOperator">The clause operator</param>
        /// <param name="filterValue">The filter value to be compared to</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateIntegerFilterClause(string columnName, FilterClauseOperator clauseOperator, int filterValue);

        /// <summary>
        /// Creates a new composite filter clause combining two given filter
        /// clauses the operator provided
        /// </summary>
        /// <param name="leftClause">The left filter clause</param>
        /// <param name="compositeOperator">The composite operator, such as
        /// "and" or "or"</param>
        /// <param name="rightClause">The right filter clause</param>
        /// <returns>Returns the new filter clause object</returns>
        IFilterClause CreateCompositeFilterClause(IFilterClause leftClause,
                                                 FilterClauseCompositeOperator compositeOperator,
                                                 IFilterClause rightClause);

        /// <summary>
        /// Creates a new null filter clause, which does no filtering
        /// </summary>
        IFilterClause CreateNullFilterClause();
    }
}
