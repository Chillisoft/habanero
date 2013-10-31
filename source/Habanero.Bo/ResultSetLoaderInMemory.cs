using System.Linq;
using Habanero.Base;
using Habanero.Base.Data;

namespace Habanero.BO
{
    public class ResultSetLoaderInMemory : IResultSetLoader
    {
        private readonly DataStoreInMemory _dataStore;

        public ResultSetLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        public QueryResult GetResultSet(ISelectQuery selectQuery)
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