namespace Habanero.Generic
{
    /// <summary>
    /// An enumeration that provides an operator for filter clauses, which
    /// allow only rows of data to be shown that meet the requirements set by the
    /// filter.  Note that some types of operators may not be appropriate for
    /// certain data types, such as "OpLike" for an integer.
    /// </summary>
    public enum FilterClauseOperator
    {
        /// <summary>
        /// The data matches the filter value exactly
        /// </summary>
        OpEquals,
        /// <summary>
        /// The data contains the filter value
        /// </summary>
        OpLike,
        /// <summary>
        /// The data is greater than or equal to the filter value
        /// </summary>
        OpGreaterThanOrEqualTo,
        /// <summary>
        /// The data is less than or equal to the filter value
        /// </summary>
        OpLessThanOrEqualTo
    }
}
