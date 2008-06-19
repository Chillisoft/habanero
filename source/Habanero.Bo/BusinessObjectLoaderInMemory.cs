using System;
using Habanero.Base;

namespace Habanero.BO
{
    public class BusinessObjectLoaderInMemory : IBusinessObjectLoader
    {
        private readonly DataStoreInMemory _dataStore;

        public BusinessObjectLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        public T GetBusinessObject<T>(IPrimaryKey key) where T : class, IBusinessObject, new()
        {
            if (_dataStore.AllObjects.ContainsKey(key))
                return (T)_dataStore.AllObjects[key];
            else
                return null;
        }

        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            return _dataStore.Find<T>(criteria);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            return _dataStore.FindAll<T>(criteria);
        }

        public void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> updatedCol = GetBusinessObjectCollection<T>(collection.SelectQuery.Criteria);
            collection.ForEach(delegate(T obj) { if (!updatedCol.Contains(obj)) collection.Remove(obj); });
            updatedCol.ForEach(delegate(T obj) { if (!collection.Contains(obj)) collection.Add(obj);});
            
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
            BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(criteria);
            col.Sort(delegate(T x, T y) { return orderCriteria.Compare(x, y); });
            return col;
        }
    }
}