using System.Data;
using Habanero.Base;
using Habanero.Base.Data;
using Habanero.BO;

namespace Habanero.DB
{
    /// <summary>
    /// Loads an <see cref="IQueryResult"/> given a <see cref="ISelectQuery"/> from the database.
    /// </summary>
    public class QueryResultLoaderDb : IQueryResultLoader
    {
        private readonly IDatabaseConnection _databaseConnection;

        /// <summary>
        /// Creates the result set loader with the given <see cref="IDatabaseConnection"/>
        /// </summary>
        /// <param name="databaseConnection"></param>
        public QueryResultLoaderDb(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
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