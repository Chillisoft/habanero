using System.Data;
using Habanero.Base;

namespace Habanero.BO
{
    public class BusinessObjectLoaderDB : IBusinessObjectLoader
    {
        private readonly DataStoreInMemory _loadedBusinessObjects;
        private readonly IDatabaseConnection _databaseConnection;

        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _loadedBusinessObjects = new DataStoreInMemory();
        }

        public T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject
        {
            T foundBO = _loadedBusinessObjects.Find<T>(key);
            if (foundBO != null) return foundBO;
            return (T)BOLoader.Instance.GetBusinessObjectByID(typeof(T), key);
        }

        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject
        {
            T foundBO = _loadedBusinessObjects.Find<T>(criteria);
            if (foundBO != null) return foundBO;
            return (T)BOLoader.Instance.GetBusinessObject(typeof(T), criteria.ToString());
            //QueryDB selectQuery = new QueryFactoryDB().CreateSelectQuery<T>();
            //selectQuery.DatabaseCriteria = databaseCriteria;
            //ISqlStatement statement = selectQuery.CreateSqlStatement();
            //T loadedObject = new T();
            //using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            //{
            //    try
            //    {
            //        if (dr.Read())
            //        {
            //            int i = 0;
            //            foreach (BOProp prop in loadedObject.Props.SortedValues)
            //            {
            //                if (!prop.PropDef.Persistable) continue; //BRETT/PETER TODO: to be changed
            //                try
            //                {
            //                    prop.InitialiseProp(dr[i++]);
            //                }
            //                catch (IndexOutOfRangeException)
            //                {
            //                }
            //            }
            //            return loadedObject;
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //    finally
            //    {
            //        if (dr != null && !dr.IsClosed)
            //        {
            //            dr.Close();
            //        }
            //    }
            //}
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject
        {
            BusinessObjectCollection<T> col = BOLoader.Instance.GetBusinessObjectCol<T>(criteria.ToString(), "");
            col.Criteria = criteria;
            return col;
        }

        public void Refresh<T>(BusinessObjectCollection<T> collection) where T :  class, IBusinessObject, new()
        {
            //QueryDB selectQuery = new QueryFactoryDB().CreateSelectQuery<T>(collection.Criteria);
            //ISqlStatement statement = selectQuery.CreateSqlStatement();
            //BusinessObjectCollection<T> oldCol = collection.Clone();
            //collection.Clear();
            //using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            //{
            //    try
            //    {
            //        while (dr.Read())
            //        {
            //            T bo = (T)BOLoader.Instance.GetBusinessObject(new T(), dr);
            //            if (oldCol.Contains(bo))
            //            {
            //                collection.AddInternal(bo);
            //            }
            //            else
            //            {
            //                collection.Add(bo);
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        if (dr != null && !dr.IsClosed)
            //        {
            //            dr.Close();
            //        }
            //    }
            //}
        }
    }
}