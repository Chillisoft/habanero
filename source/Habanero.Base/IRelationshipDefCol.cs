using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of relationship definitions
    /// </summary>
    public interface IRelationshipDefCol : IEnumerable<IRelationshipDef>
    {
        /// <summary>
        /// Add an existing relationship to the collection
        /// </summary>
        /// <param name="relationshipDef">The existing relationship to add</param>
        void Add(IRelationshipDef relationshipDef);

        /// <summary>
        /// Indicates whether the collection contains the relationship
        /// definition specified
        /// </summary>
        /// <param name="keyName">The name of the definition</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(string keyName);

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["marriage"])
        /// </summary>
        /// <param name="relationshipName">The name of the relationship to
        /// access</param>
        /// <returns>Returns the relationship definition that matches the
        /// name provided</returns>
        IRelationshipDef this[string relationshipName] { get; }

        /// <summary>
        /// Gets the count of items in this collection
        /// </summary>
        int Count { get; }
    }
}