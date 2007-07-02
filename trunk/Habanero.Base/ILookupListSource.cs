using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a lookup-list content provider
    /// </summary>
    public interface ILookupListSource
    {
        /// <summary>
        /// Returns the contents of a lookup-list
        /// </summary>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        Dictionary<string, object> GetLookupList();

        ///// <summary>
        ///// Returns the contents of a lookup-list using the database 
        ///// connection provided
        ///// </summary>
        ///// <param name="connection">The database connection</param>
        ///// <returns>Returns a collection of string-Guid pairs</returns>
        Dictionary<string, object> GetLookupList(IDatabaseConnection connection);
    }
}