using Habanero.Base.Data;

namespace Habanero.Base
{
    /// <summary>
    /// Loads an <see cref="IQueryResult"/> given a <see cref="ISelectQuery"/>
    /// </summary>
    public interface IQueryResultLoader
    {
        /// <summary>
        /// Populates a <see cref="QueryResult"/> using the given <see cref="ISelectQuery"/>.
        /// With this method you can execute a custom select query and get a result set back. If you are loading against a
        /// database, the <see cref="IClassDef"/> associated to the <see cref="ISelectQuery"/> will be used to map property names 
        /// to database fields and will also be used to convert values that are returned from the database to the expected
        /// type. This can be used to get result sets that span tables.  
        /// </summary>
        /// <param name="selectQuery">The select query to execute</param>
        /// <returns>A <see cref="QueryResult"/> that contains the results of the query</returns>
        IQueryResult GetResultSet(ISelectQuery selectQuery);
    }
}