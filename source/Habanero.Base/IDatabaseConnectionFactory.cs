namespace Habanero.Base
{
    ///<summary>An interface describing a class that creates <see cref="IDatabaseConnection"/> objects.
    ///</summary>
    public interface IDatabaseConnectionFactory {
        /// <summary>   
        /// Creates a new database connection with the configuration
        /// provided
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <returns>Returns a new database connection</returns>
        IDatabaseConnection CreateConnection(IDatabaseConfig config);
    }
}