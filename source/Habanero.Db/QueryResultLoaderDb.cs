using System.Data;
using Habanero.Base;
using Habanero.Base.Data;
using Habanero.BO;

namespace Habanero.DB
{
    public class QueryResultLoaderDb : IQueryResultLoader
    {
        private readonly IDatabaseConnection _databaseConnection;

        public QueryResultLoaderDb(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public IQueryResult GetResultSet(ISelectQuery selectQuery)
        {
            var classDef = selectQuery.ClassDef;
            var criteria = selectQuery.Criteria;
            QueryBuilder.PrepareCriteria(classDef, criteria);
            //Ensure that all the criteria field sources are merged correctly
            selectQuery.Criteria = criteria;
            selectQuery.Fields.ForEach(pair =>
            {
                var field = pair.Value;
                var fieldSource = field.Source;
                QueryBuilder.PrepareField(fieldSource, classDef, field);
                selectQuery.Source.MergeWith(field.Source);
                field.Source = field.Source.ChildSourceLeaf;
            });
            var queryDb = new SelectQueryDB(selectQuery, _databaseConnection);
            var statement = queryDb.CreateSqlStatement();
            var resultSet = new QueryResult();
            var propNames = selectQuery.Fields.Keys;
            propNames.ForEach(resultSet.AddField);

            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    var rawValues = new object[dr.FieldCount];
                    dr.GetValues(rawValues);
                    resultSet.AddResult(rawValues);
                }
            }
            return resultSet;
        }
    }
}