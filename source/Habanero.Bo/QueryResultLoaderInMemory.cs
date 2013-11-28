using System.Linq;
using Habanero.Base;
using Habanero.Base.Data;

namespace Habanero.BO
{
    /// <summary>
    /// Loads an <see cref="IQueryResult"/> given a <see cref="ISelectQuery"/> from an in memory data store (<see cref="DataStoreInMemory"/>)
    /// </summary>
    public class QueryResultLoaderInMemory : IQueryResultLoader
    {
        private readonly DataStoreInMemory _dataStore;

        /// <summary>
        /// Constructor - give it the dataStore to load from.
        /// </summary>
        /// <param name="dataStore"></param>
        public QueryResultLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Populates a <see cref="QueryResult"/> using the given <see cref="ISelectQuery"/>.
        /// With this method you can execute a custom select query and get a result set back. If you are loading against a
        /// database, the <see cref="IClassDef"/> associated to the <see cref="ISelectQuery"/> will be used to map property names 
        /// to database fields and will also be used to convert values that are returned from the database to the expected
        /// type. This can be used to get result sets that span tables.  
        /// </summary>
        /// <param name="selectQuery">The select query to execute</param>
        /// <returns>A <see cref="QueryResult"/> that contains the results of the query</returns>
        public IQueryResult GetResultSet(ISelectQuery selectQuery)
        {
            QueryBuilder.PrepareCriteria(selectQuery.ClassDef, selectQuery.Criteria);
            var collection = _dataStore.FindAll(selectQuery.ClassDef, selectQuery.Criteria);
            var resultSet = new QueryResult();
            var propNames = selectQuery.Fields.Keys;
            propNames.ForEach(resultSet.AddField);

            foreach (IBusinessObject bo in collection)
            {
                var bo1 = bo;
                resultSet.AddResult(
                    propNames.Select(s => new BOMapper(bo1).GetPropertyValueToDisplay(s))
                        .ToArray()
                    );
            }
            var sorter = new QueryResultSorter();
            sorter.Sort(resultSet, selectQuery.OrderCriteria);
            return resultSet;
        }
    }
}