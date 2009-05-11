using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
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
            return WithDB4O(delegate(IObjectContainer db)
                                {
                                    string typeName = typeof (T).Name;
                                    IList<BusinessObjectDTO> matchingObjects =
                                        db.Query<BusinessObjectDTO>(
                                            delegate(BusinessObjectDTO obj)
                                                {
                                                    return obj.ClassName == typeName && obj.ID == primaryKey.ToString();
                                                });
                                    return GetFirstObjectFromMatchedObjects<T>(matchingObjects, null, true);
                                }
                );
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            BOPrimaryKey boPrimaryKey = ((BOPrimaryKey)primaryKey);
            Criteria criteria = boPrimaryKey.GetKeyCriteria();
            string primaryKeyString = boPrimaryKey.ToString();
            return WithDB4O(db =>
                            {
                                IList<BusinessObjectDTO> matchingObjects =
                                    db.Query<BusinessObjectDTO>(
                                        obj =>
                                            {
                                                return obj.ClassDefName == classDef.ClassName && obj.ID == primaryKeyString;
                                            });
                                return GetFirstObjectFromMatchedObjects<BusinessObject>(matchingObjects, classDef, true);
                            }
                );
        }

        private static T GetFirstObjectFromMatchedObjects<T>(IList<BusinessObjectDTO> matchingObjects, IClassDef classDef, bool errorIfNotFound) where T : class, IBusinessObject, new()
        {

            if (matchingObjects.Count > 1)
            {
                string className = classDef != null ? classDef.ClassName : typeof(T).Name;
                string message = "There was an error with loading the class " + className + ". The query returned more than one record when only one was expected";
                throw new HabaneroDeveloperException(message,message);
            }
            BusinessObjectDTO matchedDTO = ((matchingObjects.Count > 0) ? matchingObjects[0] : null);
            if (matchedDTO == null)
            {
                string className = classDef != null ? classDef.ClassName : typeof(T).Name;
                if (errorIfNotFound) throw new BusObjDeleteConcurrencyControlException(string.Format("A Error has occured since the object you are trying to refresh has been deleted by another user. There are no records in the database for the Class: {0} identified by", className));
                return null;
            }

            T bo = GetBoFromDTO<T>(classDef, matchedDTO);

            T businessObjectFromManager = null;
            Type typeToGet = classDef == null ? typeof (T) : classDef.ClassType;
            businessObjectFromManager = (T)GetObjectFromObjectManager(bo.ID, typeToGet);
            if (businessObjectFromManager != null)
            {
                return businessObjectFromManager;
            }
            else
            {
                BusinessObjectManager.Instance.Add(bo);
            }
           
            SetStatusAfterLoad(bo);

            return bo;
        }

        private static T GetBoFromDTO<T>(IClassDef classDef, BusinessObjectDTO matchedBO) where T : class, IBusinessObject, new()
        {
            T tempBO ;

            if (classDef != null)
            {
                bool useSpecificClassDef = (!string.IsNullOrEmpty(classDef.TypeParameter));
                tempBO = (T)classDef.CreateNewBusinessObject(useSpecificClassDef);
            } else
            {
                tempBO = new T();
            }
            BusinessObjectManager.Instance.Remove(tempBO);
            foreach (IBOProp boProp in tempBO.Props)
            {
                try
                {
                    tempBO.Props[boProp.PropertyName].InitialiseProp(matchedBO.Props[boProp.PropertyName.ToUpper()]);
                } catch (KeyNotFoundException) {}

            }
            return tempBO;
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
                                // validate the criteria against a sample object
                                //T newObj = new T();
                                //BusinessObjectManager.Instance.Remove(newObj);
                                //criteria.IsMatch(newObj);
                                string typeName = typeof(T).Name;
                                IList<BusinessObjectDTO> matchingObjects = db.Query<BusinessObjectDTO>(obj => obj.ClassName == typeName && criteria.IsMatch(obj));
                                return GetFirstObjectFromMatchedObjects<T>(matchingObjects, null, false);
                            }
                );
        }
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            return WithDB4O(db =>
                            {
                                // validate the criteria against a sample object
                                ///IBusinessObject newObj = classDef.CreateNewBusinessObject();
                                //BusinessObjectManager.Instance.Remove(newObj);
                                //criteria.IsMatch(newObj);

                                IList<BusinessObjectDTO> matchingObjects =
                                    db.Query<BusinessObjectDTO>(
                                        obj => obj.ClassDefName == classDef.ClassName && criteria.IsMatch(obj));
                                return GetFirstObjectFromMatchedObjects<BusinessObject>(matchingObjects, classDef, false);
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
        public T GetBusinessObject<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            Criteria criteriaObject = CriteriaParser.CreateCriteria(criteriaString);
            return GetBusinessObject<T>(criteriaObject);
        }
        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString)
        {
            Criteria criteriaObject = CriteriaParser.CreateCriteria(criteriaString);
            return GetBusinessObject(classDef,criteriaObject);
        }
        public T GetRelatedBusinessObject<T>(SingleRelationship<T> relationship) where T : class, IBusinessObject, new() {
            return GetBusinessObject<T>(Criteria.FromRelationship(relationship));
        }
        public IBusinessObject GetRelatedBusinessObject(ISingleRelationship relationship) {
            IRelationshipDef relationshipDef = relationship.RelationshipDef;
            if (relationshipDef.RelatedObjectClassDef != null)
                return GetBusinessObject(relationshipDef.RelatedObjectClassDef,
                                         Criteria.FromRelationship(relationship));
            return null;
        
        
        }
        protected override void DoRefresh(IBusinessObjectCollection collection)
        {
            ISelectQuery selectQuery = collection.SelectQuery;
            Criteria criteria = selectQuery.Criteria;
            OrderCriteria orderCriteria = selectQuery.OrderCriteria;

            IClassDef classDef = collection.ClassDef;
            QueryBuilder.PrepareCriteria(classDef, criteria);

            WithDB4O<IBusinessObject>(db =>
            {
                IList<BusinessObjectDTO> matchingObjects;
                string className = classDef.ClassName;
                if (criteria == null)
                {
                    matchingObjects = db.Query<BusinessObjectDTO>(
                        delegate(BusinessObjectDTO obj)
                            {
                                return obj.ClassDefName == className;
                            });
                }
                else
                {
                    matchingObjects = db.Query<BusinessObjectDTO>(obj => obj.ClassDefName == className && criteria.IsMatch(obj));
                }
                List<IBusinessObject> loadedBOs = new List<IBusinessObject>(matchingObjects.Count);
                foreach (BusinessObjectDTO matchingObject in matchingObjects)
                {
                    BusinessObject tempBO = GetBoFromDTO<BusinessObject>(classDef, matchingObject);
                    if (BusinessObjectManager.Instance.Contains(tempBO.ID))
                    {
                        loadedBOs.Add(GetLoadedBusinessObject(tempBO)); //BusinessObjectManager.Instance[matchingObject.ID]);
                    }
                    else
                    {
                        loadedBOs.Add(tempBO);
                        BusinessObjectManager.Instance.Add(tempBO);
                        SetStatusAfterLoad(tempBO);
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

            IClassDef classDef = collection.ClassDef;
            QueryBuilder.PrepareCriteria(classDef, criteria);
            string criteriaFieldValue = "";
            if (criteria != null)
            {
                criteriaFieldValue = criteria.FieldValue.ToString();
            }

            WithDB4O<T>(db =>
                        {
                            IList<BusinessObjectDTO> matchingObjects;
                            string className = classDef.ClassName;
                            if (criteria == null)
                            {
                                matchingObjects = db.Query<BusinessObjectDTO>(obj => obj.ClassDefName == className);
                            }
                            else
                            {
                                matchingObjects = db.Query<BusinessObjectDTO>(
                                    delegate(BusinessObjectDTO obj)
                                        {
                                            criteria.FieldValue = criteriaFieldValue;
                                            return obj.ClassDefName == className && criteria.IsMatch(obj);
                                        });
                            }
                            List<T> loadedBOs = new List<T>(matchingObjects.Count);

                            foreach (BusinessObjectDTO matchingObject in matchingObjects)
                            {


                                T bo = GetBoFromDTO<T>(classDef, matchingObject);
                                if (BusinessObjectManager.Instance.Contains(bo.ID))
                                {
                                    loadedBOs.Add((T) GetLoadedBusinessObject(bo)); //BusinessObjectManager.Instance[matchingObject.ID]);
                                }
                                else
                                {
                                    loadedBOs.Add(bo);
                                    BusinessObjectManager.Instance.Add(bo);
                                    SetStatusAfterLoad(bo);
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

        public IBusinessObject Refresh(IBusinessObject businessObject)
        {
            if (businessObject.Status.IsNew)
            {
                return businessObject;
            }
            if (businessObject.Status.IsEditing)
            {
                throw new HabaneroDeveloperException
                    ("A Error has occured since the object being refreshed is being edited.",
                     "A Error has occured since the object being refreshed is being edited. ID :- "
                     + businessObject.ID.AsString_CurrentValue() + " - Class : " + businessObject.ClassDef.ClassNameFull);
            }
            businessObject = GetBusinessObject(businessObject.ClassDef, businessObject.ID);
            return businessObject;
        }
        public int GetCount(IClassDef classDef, Criteria criteria) {

            IBusinessObjectCollection collection = GetBusinessObjectCollection(classDef, criteria);
            return collection.Count;
        }
    }
}