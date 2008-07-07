using System;
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

        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            return GetBusinessObject(classDef, Criteria.FromPrimaryKey(primaryKey));
        }


        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(ClassDef.ClassDefs[typeof (T)]);
            selectQuery.Criteria = criteria;
            return GetBusinessObject<T>(selectQuery);
        }

        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
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

        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            selectQuery.Criteria = criteria;
            return GetBusinessObject(classDef, selectQuery);
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, ISelectQuery selectQuery) 
        {
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                try
                {
                    if (dr.Read())
                    {
                        return LoadBOFromReader(classDef, dr, selectQueryDB);
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

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="collection">The collection to refresh</param>
        public void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new()
        {
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            BusinessObjectCollection<T> updatedCol = collection.Clone();
            updatedCol.SelectQuery = collection.SelectQuery;
            updatedCol.Clear();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                try
                {
                    while (dr.Read())
                    {
                        T loadedBo = LoadBOFromReader<T>(dr, selectQuery);
                        updatedCol.Add(loadedBo);
                    }
                }
                finally
                {
                    if (dr != null && !dr.IsClosed) dr.Close();
                }
            }
            collection.ForEach(delegate(T obj) { if (!updatedCol.Contains(obj)) collection.Remove(obj); });
            updatedCol.ForEach(delegate(T obj) { if (!collection.Contains(obj)) collection.Add(obj); });
        }

        public void Refresh(IBusinessObjectCollection collection)
        {
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            IBusinessObjectCollection updatedCol = collection.Clone();
            updatedCol.SelectQuery = collection.SelectQuery;
            updatedCol.Clear();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                try
                {
                    while (dr.Read())
                    {
                        IBusinessObject loadedBo = LoadBOFromReader(collection.ClassDef, dr, selectQuery);
                        updatedCol.Add(loadedBo);
                    }
                }
                finally
                {
                    if (dr != null && !dr.IsClosed) dr.Close();
                }
            }
            foreach (IBusinessObject obj in collection)
            {
                if (!updatedCol.Contains(obj)) collection.Remove(obj);
            }
            foreach (IBusinessObject obj in updatedCol)
            {
                if (!collection.Contains(obj)) collection.Add(obj);
            }
        }

        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship)
            where T : class, IBusinessObject, new()
        {
            RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            GetBusinessObjectCollection<T>(relationshipCriteria, relationship.OrderCriteria).ForEach(delegate(T obj) { relatedCol.Add(obj); });
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = relationship.OrderCriteria;
            return relatedCol;
        }

        public T GetRelatedBusinessObject<T>(SingleRelationship relationship) where T : class, IBusinessObject, new()
        {
            return GetBusinessObject<T>(Criteria.FromRelationship(relationship));
        }

        public IBusinessObject GetRelatedBusinessObject(SingleRelationship relationship)
        {
            return GetBusinessObject(relationship.RelationshipDef.RelatedObjectClassDef, Criteria.FromRelationship(relationship));
        }



     



        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria)
            where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria, OrderCriteria orderCriteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        private IBusinessObjectCollection CreateCollectionOfType(Type BOType)
        {
            Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(BOType);
            return (IBusinessObjectCollection)Activator.CreateInstance(boColType);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, ISelectQuery selectQuery)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        private T LoadBOFromReader<T>(IDataRecord dr, ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            T bo = new T();
            BusinessObject bo1 = (BusinessObject)(IBusinessObject) bo;
            int i = 0;
            foreach (QueryField field in selectQuery.Fields.Values)
            {
                if (bo1.Props.Contains(field.PropertyName)) bo1.Props[field.PropertyName].InitialiseProp(dr[i++]);
            }
   
            T boFromAllLoadedObjects = null;
            if (BusinessObject.AllLoadedBusinessObjects().ContainsKey(bo.PrimaryKey.GetObjectId()))
            {
                boFromAllLoadedObjects = (T)BusinessObject.AllLoadedBusinessObjects()[bo.PrimaryKey.GetObjectId()].Target;
                if (boFromAllLoadedObjects == null)
                {
                    BusinessObject.AllLoadedBusinessObjects().Remove(bo.PrimaryKey.GetObjectId());
                }
            }
            else
            {
                //todo: add ibo to the allloadedbusinessobjectscol.
            }
            if (boFromAllLoadedObjects != null) return boFromAllLoadedObjects;
            return bo;
        }

        private IBusinessObject LoadBOFromReader(IClassDef classDef, IDataReader dataReader, ISelectQuery selectQuery)
        {
            IBusinessObject bo = classDef.CreateNewBusinessObject();
            int i = 0;
            foreach (QueryField field in selectQuery.Fields.Values)
            {
                bo.Props[field.PropertyName].InitialiseProp(dataReader[i++]);
            }

            IBusinessObject boFromAllLoadedObjects = null;
            if (BusinessObject.AllLoadedBusinessObjects().ContainsKey(bo.PrimaryKey.GetObjectId()))
            {
                boFromAllLoadedObjects = (IBusinessObject) BusinessObject.AllLoadedBusinessObjects()[bo.PrimaryKey.GetObjectId()].Target;
                if (boFromAllLoadedObjects == null)
                {
                    BusinessObject.AllLoadedBusinessObjects().Remove(bo.PrimaryKey.GetObjectId());
                }
            }
            else
            {
                //todo: add ibo to the allloadedbusinessobjectscol.
            }
            if (boFromAllLoadedObjects != null) return boFromAllLoadedObjects;
            return bo;
        }
    }
}