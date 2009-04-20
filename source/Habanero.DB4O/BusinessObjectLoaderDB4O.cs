using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.DB4O
{
    public class BusinessObjectLoaderDB4O : BusinessObjectLoaderBase, IBusinessObjectLoader
    {
        private readonly IObjectContainer _objectContainer;

        public BusinessObjectLoaderDB4O(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        private delegate T DBSearch<T>(IObjectContainer db);
        private T WithDB4O<T>(DBSearch<T> action)
        {
            return action(DB4ORegistry.DB);

            //IObjectContainer db = Db4oFactory.OpenFile(_fileName);
            //try
            //{
            //    return action(db);
            //}
            //finally
            //{
            //    if (db != null) db.Close();
            //}
        }

        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            Criteria criteria = ((BOPrimaryKey)primaryKey).GetKeyCriteria();
            return WithDB4O(db =>
                            {
                                IList<T> matchingObjects = db.Query<T>(obj => criteria.IsMatch(obj, false));
                                return (T)GetFirstObjectFromMatchedObjects(matchingObjects, typeof(T).Name, true);
                            }
                );
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            Criteria criteria = ((BOPrimaryKey)primaryKey).GetKeyCriteria();
            return WithDB4O(db =>
                            {
                                IList<BusinessObject> matchingObjects =
                                    db.Query<BusinessObject>(
                                        obj => obj.ClassDefName == classDef.ClassName && criteria.IsMatch(obj, false));
                                return GetFirstObjectFromMatchedObjects(matchingObjects, classDef.ClassName, true);
                            }
                );
        }

        private T GetFirstObjectFromMatchedObjects<T>(IList<T> matchingObjects, string className, bool errorIfNotFound) where T : class, IBusinessObject
        {
            T matchedBO = (T)((matchingObjects.Count > 0) ? matchingObjects[0] : null);
            if (matchedBO == null)
            {
                if (errorIfNotFound) throw new BusObjDeleteConcurrencyControlException(string.Format("A Error has occured since the object you are trying to refresh has been deleted by another user. There are no records in the database for the Class: {0} identified by", className));
                return null;
            }
            if (BusinessObjectManager.Instance.Contains(matchedBO.ID))
            {
                return (T)BusinessObjectManager.Instance[matchedBO.ID];
            }
            return matchedBO;
        }


        //private IBusinessObject GetFirstObjectFromMatchedObjects(IList matchingObjects) {
        //    IBusinessObject matchedBO = (IBusinessObject) ((matchingObjects.Count > 0) ? matchingObjects[0] : null);
        //    if (matchedBO == null) return null;
        //    if (BusinessObjectManager.Instance.Contains(matchedBO.ID))
        //    {
        //        return BusinessObjectManager.Instance[matchedBO.ID];
        //    }
        //    return matchedBO;
        //}

        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            return WithDB4O(db =>
                            {
                                IList<T> matchingObjects = db.Query<T>(obj => criteria.IsMatch(obj, false));
                                return (T)GetFirstObjectFromMatchedObjects(matchingObjects, typeof(T).Name, false);
                            }
                );
        }
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            return WithDB4O(db =>
                            {
                                IList<BusinessObject> matchingObjects =
                                    db.Query<BusinessObject>(
                                        obj => obj.ClassDefName == classDef.ClassName && criteria.IsMatch(obj, false));
                                return GetFirstObjectFromMatchedObjects(matchingObjects, classDef.ClassName, false);
                            }
                );

        }
        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            return GetBusinessObject<T>(selectQuery.Criteria);
        }
        public IBusinessObject GetBusinessObject(IClassDef classDef, ISelectQuery selectQuery)
        {
            return GetBusinessObject(classDef, selectQuery.Criteria);
        }
        public T GetBusinessObject<T>(string criteriaString) where T : class, IBusinessObject, new() { throw new NotImplementedException(); }
        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString) { throw new NotImplementedException(); }
        public T GetRelatedBusinessObject<T>(SingleRelationship<T> relationship) where T : class, IBusinessObject, new() { throw new NotImplementedException(); }
        public IBusinessObject GetRelatedBusinessObject(ISingleRelationship relationship) { throw new NotImplementedException(); }
        protected override void DoRefresh(IBusinessObjectCollection collection)
        {
            ISelectQuery selectQuery = collection.SelectQuery;
            Criteria criteria = selectQuery.Criteria;
            OrderCriteria orderCriteria = selectQuery.OrderCriteria;

            QueryBuilder.PrepareCriteria(collection.ClassDef, criteria);

            WithDB4O<IBusinessObject>(db =>
            {
                IList<IBusinessObject> matchingObjects;
                if (criteria == null)
                {
                    matchingObjects = db.Query<IBusinessObject>();
                }
                else
                {
                    matchingObjects = db.Query<IBusinessObject>(obj => criteria.IsMatch(obj, false));
                }
                List<IBusinessObject> loadedBOs = new List<IBusinessObject>(matchingObjects.Count);
                foreach (IBusinessObject matchingObject in matchingObjects)
                {
                    if (BusinessObjectManager.Instance.Contains(matchingObject.ID))
                    {
                        loadedBOs.Add(GetLoadedBusinessObject(matchingObject)); //BusinessObjectManager.Instance[matchingObject.ID]);
                    }
                    else
                    {
                        loadedBOs.Add(matchingObject);
                    }
                }
                loadedBOs.Sort(orderCriteria.Compare);
                collection.TotalCountAvailableForPaging = matchingObjects.Count;
                ApplyLimitsToList(selectQuery, loadedBOs);
                LoadBOCollection(collection, (ICollection)loadedBOs);
                return null;
            }
             );

        }
        protected override void DoRefresh<T>(BusinessObjectCollection<T> collection)
        {
            ISelectQuery selectQuery = collection.SelectQuery;
            Criteria criteria = selectQuery.Criteria;
            OrderCriteria orderCriteria = selectQuery.OrderCriteria;

            QueryBuilder.PrepareCriteria(collection.ClassDef, criteria);

            WithDB4O<T>(db =>
                        {
                            IList<T> matchingObjects;
                            if (criteria == null)
                            {
                                matchingObjects = db.Query<T>();
                            }
                            else
                            {
                                matchingObjects = db.Query<T>(obj => criteria.IsMatch(obj, false));
                            }
                            List<IBusinessObject> loadedBOs = new List<IBusinessObject>(matchingObjects.Count);
                            foreach (T matchingObject in matchingObjects)
                            {
                                if (BusinessObjectManager.Instance.Contains(matchingObject.ID))
                                {
                                    loadedBOs.Add(GetLoadedBusinessObject(matchingObject)); //BusinessObjectManager.Instance[matchingObject.ID]);
                                }
                                else
                                {
                                    loadedBOs.Add(matchingObject);
                                }
                            }
                            loadedBOs.Sort(orderCriteria.Compare);
                            collection.TotalCountAvailableForPaging = matchingObjects.Count;
                            ApplyLimitsToList(selectQuery, loadedBOs);
                            LoadBOCollection(collection, (ICollection) loadedBOs);
                            return null;
                        }
                );
        }

        private static IBusinessObject GetLoadedBusinessObject
            (IBusinessObject bo)
        {
            IPrimaryKey key = bo.ID;

            IBusinessObject boFromObjectManager = GetObjectFromObjectManager(key, bo.ClassDef.ClassType);

            if (boFromObjectManager == null)
            {
                BusinessObjectManager.Instance.Add(bo);
                return bo;
            }

            // if the object is new it means there is an object in the BusinessObjectManager that has the same primary
            // key as the one being loaded.  We want to return the one that was loaded without putting it into the 
            // BusinessObjectManager (as that would cause an error).  This is only used to check for duplicates or in 
            // similar scenarios.

            if (boFromObjectManager.Status.IsNew) boFromObjectManager = bo;
            if (boFromObjectManager.Status.IsEditing) return boFromObjectManager;
            return boFromObjectManager;
        }

        private static void ApplyLimitsToList(ISelectQuery selectQuery, IList loadedBos)
        {
            int firstRecordToLoad = selectQuery.FirstRecordToLoad;
            if (firstRecordToLoad < 0)
            {
                throw new IndexOutOfRangeException("FirstRecordToLoad should not be negative.");
            }
            if (firstRecordToLoad > loadedBos.Count)
            {
                loadedBos.Clear();
                return;
            }
            if (firstRecordToLoad > 0)
            {
                for (int i = 0; i < firstRecordToLoad; i++)
                {
                    loadedBos.RemoveAt(0);
                }
            }
            if (selectQuery.Limit < 0) return;
            while (loadedBos.Count > selectQuery.Limit)
            {
                loadedBos.RemoveAt(selectQuery.Limit);
            }
        }

        public IBusinessObject Refresh(IBusinessObject businessObject) { throw new NotImplementedException(); }
        public int GetCount(IClassDef classDef, Criteria criteria) { throw new NotImplementedException(); }
    }
}