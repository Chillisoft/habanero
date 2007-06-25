namespace Habanero.Base
{
    /// <summary>
    /// An enumeration that provides an operator used by a composite filter
    /// clauses to connect individual filter clauses
    /// </summary>
    public enum FilterClauseCompositeOperator
    {
        /// <summary>
        /// An "and" operator to connect filter clauses
        /// </summary>
        OpAnd,
        /// <summary>
        /// An "or" operator to connect filter clauses
        /// </summary>
        OpOr
    }
}
