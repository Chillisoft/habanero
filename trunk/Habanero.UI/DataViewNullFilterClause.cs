using Habanero.Generic;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// Manages a null filter clause, which carries out no filtering of data
    /// </summary>
    public class DataViewNullFilterClause : IFilterClause
    {
        /// <summary>
        /// Returns an empty string
        /// </summary>
        /// <returns>Returns a empty string</returns>
        public string GetFilterClauseString()
        {
            return "";
        }
    }
}
