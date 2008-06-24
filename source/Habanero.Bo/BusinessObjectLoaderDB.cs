using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    ///<summary>
    ///For details of what this class does, see <see cref="IBusinessObjectLoader"/>.
    ///
    /// All queries (including custom SelectQuery objects) run by this loader will be done using parametrized sql for 
    /// improved type safety and performance.
    /// 
    ///</summary>
    public class BusinessObjectLoaderDB : IBusinessObjectLoader
    {
        //private readonly DataStoreInMemory _dataStoreInMemory;
        private readonly IDatabaseConnection _databaseConnection;

        ///<summary>Creates a BusinessObjectLoaderDB. Because this is a loader the loads data from a Database, this constructor
        /// requires an IDatabaseConnection object to be passed to it.  This connection will be used for all loading.
        ///</summary>
        ///<param name="databaseConnection"></param>
        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            //_dataStoreInMemory = new DataStoreInMemory();
        }

        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            //T foundBO = _dataStoreInMemory.Find<T>(primaryKey);
            //if (foundBO != null) return foundBO;
            return GetBusinessObject<T>(Criteria.FromPrimaryKey(primaryKey));
        }

        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            SelectQuery selectQuery = QueryBuilder.CreateSelectQuery(ClassDef.ClassDefs[typeof (T)]);
            selectQuery.Criteria = criteria;
            return GetBusinessObject<T>(selectQuery);
        }

        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            //T foundBO = _dataStoreInMemory.Find<T>(selectQuery.Criteria);
            //if (foundBO != null) return foundBO;
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                try
                {
                    if (dr.Read())
                    {
                        return LoadBOFromReader<T>(dr, selectQueryDB);
                    }
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }
            return null;
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria)
            where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
        }

        public void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new()
        {
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            BusinessObjectCollection<T> oldCol = collection.Clone();
            collection.Clear();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                try
                {
                    while (dr.Read())
                    {
                        T loadedBo = LoadBOFromReader<T>(dr, selectQuery);
                        if (oldCol.Contains(loadedBo)) collection.AddInternal(loadedBo);
                        else collection.Add(loadedBo);
                    }
                }
                finally
                {
                    if (dr != null && !dr.IsClosed) dr.Close();
                }
            }
        }

        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship)
            where T : class, IBusinessObject, new()
        {
            RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            GetBusinessObjectCollection<T>(relationshipCriteria).ForEach(delegate(T obj) { relatedCol.Add(obj); });
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            return relatedCol;
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria) where T : BusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery) where T : BusinessObject, new()
        {
             BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }


        private T LoadBOFromReader<T>(IDataRecord dr, ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            IBusinessObject ibo = new T();
            BusinessObject bo = (BusinessObject) ibo;
            int i = 0;
            foreach (QueryField field in selectQuery.Fields.Values)
            {
                bo.Props[field.PropertyName].InitialiseProp(dr[i++]);
            }
            T loadedBo = null;
           // loadedBo = _dataStoreInMemory.Find<T>(bo.PrimaryKey);
            //if (loadedBo == null)
            //{
                if (BusinessObject.AllLoadedBusinessObjects().ContainsKey(bo.PrimaryKey.GetObjectId()))
                    ibo = (T) BusinessObject.AllLoadedBusinessObjects()[bo.PrimaryKey.GetObjectId()].Target;
                else
                {
                    //todo: add ibo to the allloadedbusinessobjectscol.
                }
             //   _dataStoreInMemory.Add(ibo);
                loadedBo = (T) ibo;
            //} 
            return loadedBo;
        }
    }
}