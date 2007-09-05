using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Provides an empty lookup-list
    /// </summary>
    public class NullLookupList : ILookupList
    {
        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <returns>Returns an empty lookup-list</returns>
        public Dictionary<string, object> GetLookupList()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns a new empty lookup-list
        /// </summary>
        /// <param name="connection">A parameter preserved for polymorphism.
        /// This can be set to null.</param>
        /// <returns>Returns an empty lookup-list</returns>
        public Dictionary<string, object> GetLookupList(IDatabaseConnection connection)
        {
            return new Dictionary<string, object>();
        }
    }
}