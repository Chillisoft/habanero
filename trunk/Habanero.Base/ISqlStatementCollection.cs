using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a collection of sql statements
    /// </summary>
    public interface ISqlStatementCollection : IEnumerable
    {
        /// <summary>
        /// Returns the number of statements in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to check</param>
        /// <returns>Returns the sql statement at the index position
        /// chosen</returns>
        ISqlStatement this[int index] { get; }
    }
}