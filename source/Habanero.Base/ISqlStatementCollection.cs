using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a collection of sql statements
    /// </summary>
    public interface ISqlStatementCollection : IEnumerable<ISqlStatement>
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

    	/// <summary>
    	/// Adds a sql statement object to the collection
    	/// </summary>
    	/// <param name="statement">The sql statement object</param>
    	void Add(ISqlStatement statement);

    	/// <summary>
    	/// Adds the contents of another sql statement collection into this
    	/// collection
    	/// </summary>
    	/// <param name="statementCollection">The other collection</param>
    	void Add(ISqlStatementCollection statementCollection);

    	/// <summary>
    	/// Inserts a sql statement object at the position specified
    	/// </summary>
    	/// <param name="index">The position to insert at</param>
    	/// <param name="sql">The sql statement object to add</param>
    	void Insert(int index, ISqlStatement sql);
    }
}