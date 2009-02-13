//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
    ///<summary>
    /// A base class for BusinessObjectLoader implementations. This base class covers all the 
    /// handling most of the overload methods and just leaves the Refresh methods to be 
    /// implemented in order to complete the implementation of the BusinessObjectLoader.
    ///</summary>
    public abstract class BusinessObjectLoaderBase
    {
        protected static readonly ILog log = LogManager.GetLogger("Habanero.BO.BusinessObjectLoaderBase");
        private static Criteria GetCriteriaObject(IClassDef classDef, string criteriaString)
        {
            Criteria criteria = CriteriaParser.CreateCriteria(criteriaString);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            return criteria;
        }

        #region GetBusinessObjectCollection

        private static void CheckNotTypedAsBusinessObject<T>() where T : class, IBusinessObject, new()
        {
            if (typeof (T) == typeof (BusinessObject))
            {
                throw ExceptionHelper.CreateLoaderGenericTypeMethodException();
            }
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            Criteria criteria = GetCriteriaObject(ClassDef.ClassDefs[typeof (T)], criteriaString);
            //            Criteria criteria = CriteriaParser.CreateCriteria(criteriaString);
            //            QueryBuilder.PrepareCriteria(ClassDef.ClassDefs[typeof(T)], criteria);
            return GetBusinessObjectCollection<T>(criteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.ClassDef = classDef;
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
            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", true);
            DoRefresh(collection);
            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", false);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public void Refresh(IBusinessObjectCollection collection)
        {
            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", true);
            DoRefresh(collection);
            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", false);
        }

        protected abstract void DoRefresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new();
        protected abstract void DoRefresh(IBusinessObjectCollection collection);

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteriaString">The select query to use to load from the data source</param>
        /// <param name="orderCriteria">The order that the collections must be loaded in e.g. Surname, FirstName</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection
            (IClassDef classDef, string criteriaString, string orderCriteria)
        {
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            OrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
            return GetBusinessObjectCollection(classDef, criteria, orderCriteriaObj);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteriaString">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string criteriaString)
        {
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            return GetBusinessObjectCollection(classDef, criteria);
        }


        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteria)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            ClassDef classDef = ClassDef.ClassDefs[typeof (T)];
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            OrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
            return GetBusinessObjectCollection<T>(criteria, orderCriteriaObj);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>
            (Criteria criteria, OrderCriteria orderCriteria) where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public IBusinessObjectCollection GetBusinessObjectCollection
            (IClassDef classDef, Criteria criteria, OrderCriteria orderCriteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            if (orderCriteria == null) orderCriteria = new OrderCriteria();
            col.ClassDef = classDef;
            QueryBuilder.PrepareCriteria(classDef, criteria);
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the SelectQuery given. It's important to make sure that T (meaning the ClassDef set up for T)
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a businessobjectcollection so that you can perhaps load from multiple
        /// tables using a join (if loading from a database source).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the SelectQuery given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a businessobjectcollection so that you can perhaps load from multiple
        /// tables using a join (if loading from a database source).
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, ISelectQuery selectQuery)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
            col.ClassDef = classDef;
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        #endregion

        protected static IBusinessObjectCollection CreateCollectionOfType(Type BOType)
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(BOType);
            return (IBusinessObjectCollection) Activator.CreateInstance(boColType);
        }

//        protected static void AddBusinessObjectToCollection<T>
//            (BusinessObjectCollection<T> collection, T loadedBo, BusinessObjectCollection<T> clonedCol)
//            where T : class, IBusinessObject, new()
//        {
//            //If the origional collection had the new business object then
//            // use add internal this adds without any events being raised etc.
//            //else adds via the Add method (normal add) this raises events such that the 
//            // user interface can be updated.
//            if (clonedCol.Contains(loadedBo))
//            {
//                ((IBusinessObjectCollection) collection).AddWithoutEvents(loadedBo);
//            }
//            else
//            {
//                collection.Add(loadedBo);
//            }
//            collection.PersistedBOCol.Add(loadedBo);
//        }

        protected static void AddBusinessObjectToCollection
            (IBusinessObjectCollection collection, IBusinessObject loadedBo)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (loadedBo == null) throw new ArgumentNullException("loadedBo");
            //If the origional collection had the new business object then
            // use add internal this adds without any events being raised etc.
            //else adds via the Add method (normal add) this raises events such that the 
            // user interface can be updated.
            if (collection.AddedBusinessObjects.Contains(loadedBo))
            {
                collection.AddWithoutEvents(loadedBo);
                collection.PersistedBusinessObjects.Add(loadedBo);
                return;
            }
            if (collection.PersistedBusinessObjects.Contains(loadedBo))
            {
                collection.AddWithoutEvents(loadedBo);
                return;
            }
            collection.PersistedBusinessObjects.Add(loadedBo);
           // ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", true);
            collection.Add(loadedBo);
           // ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", false);
        }

        //The collection should show all loaded object less removed or deleted object not yet persisted
        //     plus all created or added objects not yet persisted.
        //Note: This behaviour is fundamentally different than the business objects behaviour which 
        //  throws and error if any of the items are dirty when it is being refreshed.
        //Should a refresh be allowed on a dirty collection (what do we do with BO's
        //I think this could be done via reflection instead of having all these public methods.
        //   especially where done via the interface.
        //  the other option would be for the business object collection to have another method (other than clone)
        //   that returns another type of object that has these methods to eliminate all these 
        //   public accessors
        protected static void RestoreEditedLists(IBusinessObjectCollection collection)
        {
            ArrayList addedBoArray = new ArrayList();
            addedBoArray.AddRange(collection.AddedBusinessObjects);
            
            RestoreCreatedCollection(collection);
            RestoreRemovedCollection(collection);
            RestoreMarkForDeleteCollection(collection);
            RestoreAddedCollection(collection, addedBoArray);
        }
        protected static void RestoreAddedCollection(IBusinessObjectCollection collection, ArrayList addedBoArray)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (addedBoArray == null) throw new ArgumentNullException("addedBoArray");
            //The collection should show all loaded object less removed or deleted object not yet persisted
           //      plus all created or added objects not yet persisted.
            //Note: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            foreach (IBusinessObject addedBO in addedBoArray)
            {
                if (!collection.Contains(addedBO) && !collection.MarkedForDeleteBusinessObjects.Contains(addedBO))
                {
                    collection.AddWithoutEvents(addedBO);
                }
                if (!collection.AddedBusinessObjects.Contains(addedBO))
                {
                    collection.AddedBusinessObjects.Add(addedBO);
                }
            }
        }

        private static void RestoreMarkForDeleteCollection(IBusinessObjectCollection collection)
        {
            foreach (BusinessObject businessObject in collection.MarkedForDeleteBusinessObjects)
            {
                collection.Remove(businessObject);
                collection.RemovedBusinessObjects.Remove(businessObject);
            }
        }

        /// <summary>
        /// Restores the removed collection. I.e. moves the items that are in the removed collection
        ///  back to the main collection. Remember the main collection shows all the items from the database
        ///   plus the items added, and created less the items removed or marked for deletion.
        /// </summary>
        /// <param name="collection"></param>
        private static void RestoreRemovedCollection(IBusinessObjectCollection collection)
        {
            foreach (BusinessObject businessObject in collection.RemovedBusinessObjects)
            {
                collection.Remove(businessObject);
            }
        }
        /// <summary>
        /// Restores the created collection. I.e. moves the items that are in the created collection
        ///  back to the main collection. Remember the main collection shows all the items from the database
        ///   plus the items added, and created less the items removed or marked for deletion.
        /// </summary>
        /// <param name="collection"></param>
        private static void RestoreCreatedCollection(IBusinessObjectCollection collection)
        {
            foreach (BusinessObject businessObject in collection.CreatedBusinessObjects)
            {
                collection.AddWithoutEvents(businessObject);
            }
        }

        protected static void RestoreCreatedCollection
            (IBusinessObjectCollection collection, IList createdBusinessObjects)
        {
            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            foreach (IBusinessObject createdBO in createdBusinessObjects)
            {
                collection.CreatedBusinessObjects.Add(createdBO);
                collection.AddWithoutEvents(createdBO);
            }
        }

        protected static void RestoreRemovedCollection
            (IBusinessObjectCollection collection, IList removedBusinessObjects)
        {
            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            foreach (IBusinessObject removedBO in removedBusinessObjects)
            {
                collection.Remove(removedBO);
            }
        }

//        /// <summary>
//        /// Ensures that the added collection and main collection are synchronised after a refresh.
//        /// </summary>
//        /// <param name="collection">the main bo collection</param>
//        /// <param name="addedBusinessObjects">The list of added BO's prior to loading</param>
//        protected static void RestoreAddedCollection(IBusinessObjectCollection collection, IList addedBusinessObjects)
//        {
//            //The collection should show all loaded object less removed or deleted object not yet persisted
//            //     plus all created or added objects not yet persisted.
//            //Note: This behaviour is fundamentally different than the business objects behaviour which 
//            //  throws and error if any of the items are dirty when it is being refreshed.
//            //Should a refresh be allowed on a dirty collection (what do we do with BO's
//            foreach (IBusinessObject addedBO in addedBusinessObjects)
//            {
//                if (collection.Contains(addedBO)) continue;
//                collection.Add(addedBO);
////                collection.AddedBOCol.Add(addedBO);
//            }
//        }
        /// <summary>
        /// Loads a RelatedBusinessObjectCollection using the Relationship given.  This method is used by relationships to load based on the
        /// fields defined in the relationship.
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="relationship">The relationship that defines the criteria that must be loaded.  For example, a Person might have
        /// a Relationship called Addresses, which defines the PersonID property as the relationship property. In this case, calling this method
        /// with the Addresses relationship will load a collection of Address where PersonID = '?', where the ? is the value of the owning Person's
        /// PersonID</param>
        /// <returns>The loaded RelatedBusinessObjectCollection</returns>
        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IMultipleRelationship relationship)
            where T : class, IBusinessObject, new()
        {
            RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
            ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", true);

            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            OrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(relationshipCriteria, preparedOrderCriteria);
            LoadBOCollection(relatedCol, col);
            //QueryBuilder.PrepareCriteria(relationship.RelatedObjectClassDef, relationshipCriteria);
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = preparedOrderCriteria;
            ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", false);
            return relatedCol;
        }


        protected void LoadBOCollection(IBusinessObjectCollection collection, ICollection loadedBos)
        {
            ReflectionUtilities.ExecutePrivateMethod(collection, "ClearCurrentCollection");

            // made internal or something and used via reflection.
            // I (Brett) am not comfortable with it being on the Interface. 
            foreach (IBusinessObject loadedBo in loadedBos)
            {
                AddBusinessObjectToCollection(collection, loadedBo);
            }
            RestoreEditedLists(collection);
        }

        /// <summary>
        /// Loads a RelatedBusinessObjectCollection using the Relationship given.  This method is used by relationships to load based on the
        /// fields defined in the relationship.
        /// </summary>
        /// <param name="type">The type of collection to load. This must be a class that implements IBusinessObject</typeparam>
        /// <param name="relationship">The relationship that defines the criteria that must be loaded.  For example, a Person might have
        /// a Relationship called Addresses, which defines the PersonID property as the relationship property. In this case, calling this method
        /// with the Addresses relationship will load a collection of Address where PersonID = '?', where the ? is the value of the owning Person's
        /// PersonID</param>
        /// <returns>The loaded RelatedBusinessObjectCollection</returns>
        public IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IMultipleRelationship relationship)
        {
            IBusinessObjectCollection relatedCol = RelationshipUtils.CreateRelatedBusinessObjectCollection(type, relationship);
            
            ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", true);

            IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef,
                                                                        relatedCol.SelectQuery.Criteria, relatedCol.SelectQuery.OrderCriteria);

            LoadBOCollection(relatedCol, col);
            relatedCol.SelectQuery = col.SelectQuery;
            ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", false);
            return relatedCol;
        }

        ///<summary>
        /// For a given value e.g. a Guid Identifier '{......}' this will 
        /// load the business object from the Data store.
        /// This can only be used for business objects that have a single property for the primary key
        /// (i.e. non composite primary keys)
        ///</summary>
        ///<param name="classDef">The Class definition of the Business Object to load</param>
        ///<param name="idValue">The value of the primary key of the business object</param>
        ///<returns>the Business Object that matches the value of the id. If the primary key cannot be constructed
        /// e.g. the primary key is composite then returns null. If the Business Object cannot be loaded then returns
        /// <see cref="BusObjDeleteConcurrencyControlException"/>
        ///  </returns>
        /// <exception cref="BusObjDeleteConcurrencyControlException"/>
        public IBusinessObject GetBusinessObjectByValue(ClassDef classDef, object idValue)
        {
            BOPrimaryKey boPrimaryKey = BOPrimaryKey.CreateWithValue(classDef, idValue);

            return boPrimaryKey == null ? null : BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, boPrimaryKey);
        }

        ///<summary>
        /// For a given value e.g. a Guid Identifier '{......}' this will 
        /// load the business object from the Data store.
        /// This can only be used for business objects that have a single property for the primary key
        /// (i.e. non composite primary keys)
        ///</summary>
        ///<param name="type">The type of business object to be loaded</param>
        ///<param name="idValue">The value of the primary key of the business object</param>
        ///<returns>the Business Object that matches the value of the id. If the primary key cannot be constructed
        /// e.g. the primary key is composite then returns null. If the Business Object cannot be loaded then returns
        /// <see cref="BusObjDeleteConcurrencyControlException"/>
        ///  </returns>
        /// <exception cref="BusObjDeleteConcurrencyControlException"/>
        public IBusinessObject GetBusinessObjectByValue(Type type, object idValue)
        {
            ClassDef classDef = ClassDef.ClassDefs[type];
            return GetBusinessObjectByValue(classDef, idValue);
        }
        ///<summary>
        /// For a given value e.g. a Guid Identifier '{......}' this will 
        /// load the business object from the Data store.
        /// This can only be used for business objects that have a single property for the primary key
        /// (i.e. non composite primary keys)
        ///</summary>
        ///<param name="idValue">The value of the primary key of the business object</param>
        ///<returns>the Business Object that matches the value of the id. If the primary key cannot be constructed
        /// e.g. the primary key is composite then returns null. If the Business Object cannot be loaded then returns
        /// <see cref="BusObjDeleteConcurrencyControlException"/>
        ///  </returns>
        /// <exception cref="BusObjDeleteConcurrencyControlException"/>
        public T GetBusinessObjectByValue<T>(object idValue)
                        where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            ClassDef classDef = ClassDef.ClassDefs[typeof(T)];
            return (T) GetBusinessObjectByValue(classDef, idValue);
        }
    }
}