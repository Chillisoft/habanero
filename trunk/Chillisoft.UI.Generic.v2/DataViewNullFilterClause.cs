using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages a null filter clause, which carries out no filtering of data
    /// </summary>
    public class DataViewNullFilterClause : FilterClause
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
