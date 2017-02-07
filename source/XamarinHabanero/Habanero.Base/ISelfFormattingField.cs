using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Interface for QueryFiels which implement self-formatting functionality
    /// </summary>
    public interface ISelfFormattingField
    {
        ///<summary>
        /// Gets the formatted string for this field to be used in queries
        ///</summary>
        ///<param name="formatter">An ISqlFormatter to format with</param>
        ///<param name="aliases">The dictionary of aliases within the context to be formatted for</param>
        ///<returns>A string which can be used as part of a sql statement</returns>
        string GetFormattedStringWith(ISqlFormatter formatter, IDictionary<string, string> aliases);
    }
}