using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    public class DataStoreInMemory
    {
        private Dictionary<IPrimaryKey, IBusinessObject> _objects = new Dictionary<IPrimaryKey, IBusinessObject>();

        public int Count
        {
            get { return _objects.Count; }
        }

        public void Add(IBusinessObject businessObject)
        {
            _objects.Add(businessObject.PrimaryKey, businessObject);
        }

        public Dictionary<IPrimaryKey, IBusinessObject> AllObjects
        {
            get { return _objects; }
        }

        public T Find<T>(Criteria criteria) where T : class, IBusinessObject
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                T boAsT = bo as T;
                if (boAsT == null) continue; ;
                if (criteria.IsMatch(boAsT)) return boAsT;
            }
            return null;
        }

        public IBusinessObject Find(Type BOType, Criteria criteria)
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (BOType.IsInstanceOfType(bo) && criteria.IsMatch(bo)) return bo;
            }
            return null;
        }

        public T Find<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject
        {
            foreach (IBusinessObject bo in _objects.Values)
            {
                if (bo.PrimaryKey.Equals(primaryKey)) return bo as T;
            }
            return null;
        }

        public void Remove(IBusinessObject businessObject)
        {
            _objects.Remove(businessObject.PrimaryKey);
        }

        public BusinessObjectCollection<T> FindAll<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            foreach (IBusinessObject bo in _objects.Values)
            {
                T boAsT = bo as T;
                if (boAsT == null) continue; ;
                if (criteria == null || criteria.IsMatch(boAsT)) col.Add(boAsT);
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        public IBusinessObjectCollection FindAll(Type BOType, Criteria criteria) 
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(BOType);
            IBusinessObjectCollection col = (IBusinessObjectCollection) Activator.CreateInstance(boColType);
            foreach (IBusinessObject bo in _objects.Values)
            {

                if (BOType.IsInstanceOfType(bo))
                {
                    if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
                }
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }
    }
}