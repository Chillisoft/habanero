namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a filter clause that filters which data to
    /// display, according to some criteria
    /// </summary>
    public interface IFilterClause
    {
        /// <summary>
        /// Returns the filter clause as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetFilterClauseString();
    }
}
