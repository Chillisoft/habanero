namespace Habanero.Generic
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
        StringGuidPairCollection GetLookupList();

        /// <summary>
        /// Returns the contents of a lookup-list using the database 
        /// connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        StringGuidPairCollection GetLookupList(IDatabaseConnection connection);
    }
}