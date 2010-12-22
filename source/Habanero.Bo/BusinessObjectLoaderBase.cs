// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
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
    [Serializable]
    public abstract class BusinessObjectLoaderBase: MarshalByRefObject
    {
        /// <summary>
        /// The log file used to log errors or events for this class
        /// </summary>
//        protected static readonly ILog log = LogManager.GetLogger("Habanero.BO.BusinessObjectLoaderBase");
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
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T> {SelectQuery = {Criteria = criteria}};
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
            IBusinessObjectCollection col = CreateCollectionOfType(classDef);
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
            var boColInternal = ((IBusinessObjectCollectionInternal)collection);
            boColInternal.Loading = true;
//            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", true);
            DoRefresh(collection);
            boColInternal.Loading = false;
 //           ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", false);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public void Refresh(IBusinessObjectCollection collection)
        {
            var boColInternal = ((IBusinessObjectCollectionInternal)collection);
//            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", true);
            boColInternal.Loading = true;
            DoRefresh(collection);
            boColInternal.Loading = false;
//            ReflectionUtilities.SetPrivatePropertyValue(collection, "Loading", false);
        }

        /// <summary>
        /// Actual Executes the Refresh this method is impleemented by the inherited classes of the business object loader base.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        protected abstract void DoRefresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new();

        /// <summary>
        /// Actual Executes the Refresh this method is impleemented by the inherited classes of the business object loader base.
        /// </summary>
        /// <param name="collection"></param>
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
            IOrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
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
            IClassDef classDef = ClassDef.ClassDefs[typeof (T)];
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            IOrderCriteria orderCriteriaObj = QueryBuilder.CreateOrderCriteria(classDef, orderCriteria);
            return GetBusinessObjectCollection<T>(criteria, orderCriteriaObj);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of <see cref="IBusinessObject"/>s of type T specified as follows:
        /// If you want record 6 to 15 then 
        /// firstRecordToLoad will be set to 5 (this is zero based) and 
        /// numberOfRecordsToLoad will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the orderCriteria).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 willbe returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the firstRecordToLoad to be zero based since this is consistent with the limit clause in used by MySql etc.
        /// Also, the numberOfRecordsToLoad returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified firstRecordToLoad.
        /// If you give '0' as the value for the numberOfRecordsToLoad parameter, it will load zero records.
        /// </remarks>
        /// <example>
        /// The following code demonstrates how to loop through the invoices in the data store, 
        /// ten at a time, and print their details:
        /// <code>
        /// BusinessObjectCollection&lt;Invoice&gt; col = new BusinessObjectCollection&lt;Invoice&gt;();
        /// int interval = 10;
        /// int firstRecord = 0;
        /// int totalNoOfRecords = firstRecord + 1;
        /// while (firstRecord &lt; totalNoOfRecords)
        /// {
        ///     col.LoadWithLimit("", "InvoiceNo", firstRecord, interval, out totalNoOfRecords);
        ///     Debug.Print("The next {0} invoices:", interval);
        ///     col.ForEach(bo =&gt; Debug.Print(bo.ToString()));
        ///     firstRecord += interval;
        /// }</code>
        /// </example>
        /// <param name="criteria">The search criteria</param>
        /// <param name="orderCriteria">The order-by clause</param>
        /// <param name="firstRecordToLoad">The first record to load (NNB: this is zero based)</param>
        /// <param name="numberOfRecordsToLoad">The number of records to be loaded</param>
        /// <param name="totalNoOfRecords">The total number of records matching the criteria</param>
        /// <returns>The loaded collection, limited in the specified way.</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, IOrderCriteria orderCriteria,
                int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            col.SelectQuery.FirstRecordToLoad = firstRecordToLoad;
            col.SelectQuery.Limit = numberOfRecordsToLoad;
            Refresh(col);
            totalNoOfRecords = col.TotalCountAvailableForPaging;
            return col;
        }

        /// <summary>
        /// Loads a business Object collection of the type defined by the <paramref name="classDef"/> with the appropriate 
        /// criteria and the start no of records and max number of records. For full details 
        /// see <see cref="GetBusinessObjectCollection{T}(string,string,int,int,out int)"/>
        /// </summary>
        /// <param name="classDef"></param>
        /// <param name="criteria"></param>
        /// <param name="orderCriteria"></param>
        /// <param name="firstRecordToLoad"></param>
        /// <param name="numberOfRecordsToLoad"></param>
        /// <param name="totalNoOfRecords"></param>
        /// <returns></returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria,
                IOrderCriteria orderCriteria, int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef);
            col.ClassDef = classDef;
            col.SelectQuery.Criteria = criteria;
            col.SelectQuery.OrderCriteria = orderCriteria;
            col.SelectQuery.FirstRecordToLoad = firstRecordToLoad;
            col.SelectQuery.Limit = numberOfRecordsToLoad;
            Refresh(col);
            totalNoOfRecords = col.TotalCountAvailableForPaging;
            return col;
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of Ts specified as follows:
        /// If you want record 6 to 15 then 
        /// <paramref name="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <paramref name="numberOfRecordsToLoad"/> will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <paramref name="orderCriteriaString"/>).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 willbe returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the <paramref name="firstRecordToLoad"/> to be zero based since this is consistent with the limit clause in used by MySql etc.
        /// Also, the <paramref name="numberOfRecordsToLoad"/> returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified <paramref name="firstRecordToLoad"/>.
        /// If you give '0' as the value for the <paramref name="numberOfRecordsToLoad"/> parameter, it will load zero records.
        /// </remarks>
        /// <example>
        /// The following code demonstrates how to loop through the invoices in the data store, 
        /// ten at a time, and print their details:
        /// <code>
        /// BusinessObjectCollection&lt;Invoice&gt; col = new BusinessObjectCollection&lt;Invoice&gt;();
        /// int interval = 10;
        /// int firstRecord = 0;
        /// int totalNoOfRecords = firstRecord + 1;
        /// while (firstRecord &lt; totalNoOfRecords)
        /// {
        ///     col.LoadWithLimit("", "InvoiceNo", firstRecord, interval, out totalNoOfRecords);
        ///     Debug.Print("The next {0} invoices:", interval);
        ///     col.ForEach(bo =&gt; Debug.Print(bo.ToString()));
        ///     firstRecord += interval;
        /// }</code>
        /// </example>
        /// <param name="criteriaString">The search criteria</param>
        /// <param name="orderCriteriaString">The order-by clause</param>
        /// <param name="firstRecordToLoad">The first record to load (NNB: this is zero based)</param>
        /// <param name="numberOfRecordsToLoad">The number of records to be loaded</param>
        /// <param name="totalNoOfRecords">The total number of records matching the criteria</param>
        /// <returns>The loaded collection, limited in the specified way.</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteriaString,
                int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
            where T : class, IBusinessObject, new()
        {
            CheckNotTypedAsBusinessObject<T>();
            IClassDef classDef = ClassDef.ClassDefs[typeof(T)];
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(classDef, orderCriteriaString);
            return GetBusinessObjectCollection<T>(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>
            (Criteria criteria, IOrderCriteria orderCriteria) where T : class, IBusinessObject, new()
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
            (IClassDef classDef, Criteria criteria, IOrderCriteria orderCriteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef);
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
            IBusinessObjectCollection col = CreateCollectionOfType(classDef);
            col.ClassDef = classDef;
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        #endregion

        /// <summary>
        /// Creates a Generic Collection of <see cref="IBusinessObjectCollection"/> of the Generic
        /// Type determined by the <paramref name="classDef"/>
        /// </summary>
        /// <param name="classDef">The ClassDef to use for the collection (and its <see cref="SelectQuery"/>)</param>
        /// <returns></returns>
        protected static IBusinessObjectCollection CreateCollectionOfType(IClassDef classDef)
        {
            throw new NotImplementedException("CF: Code commented out to get CF to compile");
            //Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(classDef.ClassType);
            //return (IBusinessObjectCollection) Activator.CreateInstance(boColType, classDef);
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

        /// <summary>
        /// Adds the business object <paramref name="loadedBo"/> to the collection identified by <paramref name="collection"/>
        /// </summary>
        /// <param name="collection">The collection to add to</param>
        /// <param name="loadedBo">The bo to be added</param>
        /// <param name="originalPersistedObjects"></param>
        protected static void AddBusinessObjectToCollection
            (IBusinessObjectCollection collection, IBusinessObject loadedBo, Dictionary<string, IBusinessObject> originalPersistedObjects)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (loadedBo == null) throw new ArgumentNullException("loadedBo");
            //If the origional collection had the loaded business object then
            // use add internal. this adds without any events being raised etc.
            //else adds via the Add method (normal add) this raises events such that the 
            // user interface can be updated.
            if (collection.AddedBusinessObjects.Contains(loadedBo))
            {
                collection.AddWithoutEvents(loadedBo);
                collection.PersistedBusinessObjects.Add(loadedBo);
                return;
            }
            string idAsString = loadedBo.ID.AsString_CurrentValue();
            if (originalPersistedObjects.ContainsKey(idAsString))
            {
                collection.AddWithoutEvents(loadedBo);
                originalPersistedObjects[idAsString] = null;
                return;
            }
            collection.PersistedBusinessObjects.Add(loadedBo);
            collection.Add(loadedBo);
        }

        /// <summary>
        /// The collection should show all loaded object less removed or deleted object not yet persisted
        ///     plus all created or added objects not yet persisted.
        ///Note_: This behaviour is fundamentally different than the business objects behaviour which 
        ///  throws and error if any of the items are dirty when it is being refreshed.
        ///Should a refresh be allowed on a dirty collection (what do we do with BO's
        ///I think this could be done via reflection instead of having all these public methods.
        ///   especially where done via the interface.
        ///  the other option would be for the business object collection to have another method (other than clone)
        ///   that returns another type of object that has these methods to eliminate all these 
        ///   public accessors
        /// </summary>
        /// <param name="collection"></param>
        protected static void RestoreEditedLists(IBusinessObjectCollection collection, Dictionary<string, IBusinessObject> originalPersistedCollection)
        {
            ArrayList addedBoArray = new ArrayList();
            addedBoArray.AddRange(collection.AddedBusinessObjects);
            RestoreCreatedCollection(collection);
            RestoreRemovedCollection(collection);
            RestoreMarkForDeleteCollection(collection, originalPersistedCollection);
            RestoreAddedCollection(collection, addedBoArray);
            if (originalPersistedCollection != null)
            {
                CorrectPersistedCollection(collection, originalPersistedCollection);
            }
        }

        /// <summary>
        /// Corrects the items in the <see cref="IBusinessObjectCollection.PersistedBusinessObjects"/> collection.
        /// i.e. the objects that are in the <see cref="IBusinessObjectCollection.PersistedBusinessObjects"/> collection that were not loaded
        /// in this load are removed from that collection.
        /// </summary>
        /// <param name="collection">the collection to be corrected</param>
        /// <param name="originalPersistedCollection">A Dictionary of IDs and objects. If the object is not null then it is to be removed.</param>
        private static void CorrectPersistedCollection(IBusinessObjectCollection collection, Dictionary<string, IBusinessObject> originalPersistedCollection)
        {
            // remove whichever persisted objects have been deleted from the datasource since we last refreshed.
            // this is a performance enhancement.
            foreach (var b in originalPersistedCollection)
            {
                if (b.Value != null)
                {
                    collection.PersistedBusinessObjects.Remove(b.Value);
                }
            }
        }

        /// <summary>
        /// Restores the items in the <see cref="IBusinessObjectCollection.AddedBusinessObjects"/> collection
        ///  back into the collection <paramref name="collection"/>
        /// </summary>
        /// <param name="collection">the collection to be added back into</param>
        /// <param name="addedBoArray">The array of bo's to be added back</param>
        protected static void RestoreAddedCollection(IBusinessObjectCollection collection, ArrayList addedBoArray)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (addedBoArray == null) throw new ArgumentNullException("addedBoArray");
            //The collection should show all loaded object less removed or deleted object not yet persisted
           //      plus all created or added objects not yet persisted.
            //Note_: This behaviour is fundamentally different than the business objects behaviour which 
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

        private static void RestoreMarkForDeleteCollection(IBusinessObjectCollection collection, Dictionary<string, IBusinessObject> originalPersistedCollection)
        {
            foreach (BusinessObject businessObject in Utilities.ToArray<BusinessObject>(collection.MarkedForDeleteBusinessObjects))
            {
                collection.Remove(businessObject);
                collection.RemovedBusinessObjects.Remove(businessObject);
                originalPersistedCollection[businessObject.ID.AsString_CurrentValue()] = null;
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
            foreach (BusinessObject businessObject in Utilities.ToArray<BusinessObject>(collection.RemovedBusinessObjects))
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
            foreach (BusinessObject businessObject in Utilities.ToArray<BusinessObject>(collection.CreatedBusinessObjects))
            {
                collection.AddWithoutEvents(businessObject);
            }
        }

        // ReSharper disable UnusedMember.Global
        /// <summary>
        /// Restores the created collection. I.e. moves the items that are in the created collection
        ///  back to the main collection. Remember the main collection shows all the items from the database
        ///   plus the items added, and created less the items removed or marked for deletion.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="createdBusinessObjects"></param>
        protected static void RestoreCreatedCollection
            (IBusinessObjectCollection collection, IList createdBusinessObjects)
        {
            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note_: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            foreach (IBusinessObject createdBO in createdBusinessObjects)
            {
                collection.CreatedBusinessObjects.Add(createdBO);
                collection.AddWithoutEvents(createdBO);
            }
        }

        /// <summary>
        /// Restores the removed collection. I.e. moves the items that are in the removed collection
        ///  out of the main collection. Remember the main collection shows all the items from the database
        ///   plus the items added, and created less the items removed or marked for deletion.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="removedBusinessObjects"></param>

        protected static void RestoreRemovedCollection

            (IBusinessObjectCollection collection, IList removedBusinessObjects)
        {
            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note_: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            foreach (IBusinessObject removedBO in removedBusinessObjects)
            {
                collection.Remove(removedBO);
            }
            // ReSharper restore UnusedMember.Global
        }

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
            var boColInternal = ((IBusinessObjectCollectionInternal)relatedCol);

            //ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", true);
            boColInternal.Loading = true;
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            IOrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(relationshipCriteria, preparedOrderCriteria);
            LoadBOCollection(relatedCol, (ICollection) col);
            //QueryBuilder.PrepareCriteria(relationship.RelatedObjectClassDef, relationshipCriteria);
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = preparedOrderCriteria;
            boColInternal.Loading = false;
            //ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", false);
            return relatedCol;
        }

        /// <summary>
        /// Adds all the business objects from the <paramref name="loadedBOs"/> collection to the
        /// <see cref="IBusinessObjectCollection"/>
        /// </summary>
        /// <param name="collection">the collection being added to</param>
        /// <param name="loadedBOs">the collection of loaded BOs to add</param>
        protected static void LoadBOCollection(IBusinessObjectCollection collection, ICollection loadedBOs)
        {
            // if the collection is fresh (ie. being loaded for the first time
            // then load without events.
            if (collection.TimeLastLoaded == null)
            {
                collection.PersistedBusinessObjects.Clear();
                foreach (IBusinessObject loadedBo in loadedBOs)
                {
                    collection.AddWithoutEvents(loadedBo);
                    collection.PersistedBusinessObjects.Add(loadedBo);
                }
                collection.TimeLastLoaded = DateTime.Now;
            }  else {

                var boColInternal = ((IBusinessObjectCollectionInternal)collection);
                boColInternal.ClearCurrentCollection();
                //ReflectionUtilities.ExecutePrivateMethod(collection, "ClearCurrentCollection");

                // store the original persisted collection and pass it through. This is to improve performance
                // within the AddBusinessObjectToCollection method when amount of BO's being loaded is big.
                Dictionary<string, IBusinessObject> originalPersistedCollection = new Dictionary<string, IBusinessObject>();
                foreach (IBusinessObject businessObject in collection.PersistedBusinessObjects)
                {
                    originalPersistedCollection.Add(businessObject.ID.AsString_CurrentValue(), businessObject);
                }
                foreach (IBusinessObject loadedBo in loadedBOs)
                {
                    AddBusinessObjectToCollection(collection, loadedBo, originalPersistedCollection);
                }
                RestoreEditedLists(collection, originalPersistedCollection);
                collection.TimeLastLoaded = DateTime.Now;
                
                boColInternal.FireRefreshedEvent();               
//                ReflectionUtilities.ExecutePrivateMethod(collection, "FireRefreshedEvent");
            }
        }

        /// <summary>
        /// Adds all the business objects from the <paramref name="loadedBOs"/> collection to the
        /// <see cref="IBusinessObjectCollection"/>
        /// </summary>
        /// <param name="collection">the collection being added to</param>
        /// <param name="loadedBOs">the collection of loaded BOs to add</param>
        protected static void LoadBOCollection<T>(IBusinessObjectCollection collection, IList<T> loadedBOs)
        {
            IList col = new ArrayList();
            foreach (T loadedBO in loadedBOs) col.Add(loadedBO);
            LoadBOCollection(collection, col);
        }

        /// <summary>
        /// Loads a RelatedBusinessObjectCollection using the Relationship given.  This method is used by relationships to load based on the
        /// fields defined in the relationship.
        /// </summary>
        /// <param name="type">The type of collection to load. This must be a class that implements IBusinessObject</param>
        /// <param name="relationship">The relationship that defines the criteria that must be loaded.  For example, a Person might have
        /// a Relationship called Addresses, which defines the PersonID property as the relationship property. In this case, calling this method
        /// with the Addresses relationship will load a collection of Address where PersonID = '?', where the ? is the value of the owning Person's
        /// PersonID</param>
        /// <returns>The loaded RelatedBusinessObjectCollection</returns>
        public IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IMultipleRelationship relationship)
        {
            IBusinessObjectCollection relatedCol = RelationshipUtils.CreateRelatedBusinessObjectCollection(type, relationship);
            BOColLoaderHelper.SetLoading(relatedCol, true);
            //ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", true);

            IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef,
                                                                        relatedCol.SelectQuery.Criteria, relatedCol.SelectQuery.OrderCriteria);

            LoadBOCollection(relatedCol, col);
            relatedCol.SelectQuery = col.SelectQuery;
            BOColLoaderHelper.SetLoading(relatedCol, false);
            //ReflectionUtilities.SetPrivatePropertyValue(relatedCol, "Loading", false);
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
        public IBusinessObject GetBusinessObjectByValue(IClassDef classDef, object idValue)
        {
            BOPrimaryKey boPrimaryKey = BOPrimaryKey.CreateWithValue((ClassDef) classDef, idValue);

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
            IClassDef classDef = ClassDef.ClassDefs[type];
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
            IClassDef classDef = ClassDef.ClassDefs[typeof(T)];
            return (T) GetBusinessObjectByValue(classDef, idValue);
        }

        /// <summary>
        /// Returns the Business Object for the primary Key from the object manager
        /// </summary>
        /// <param name="key"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        protected static IBusinessObject GetObjectFromObjectManager(IPrimaryKey key, Type boType)
        {
            IBusinessObjectManager businessObjectManager = BORegistry.BusinessObjectManager;
            return businessObjectManager.GetBusinessObject(key);
/*            if (key.IsGuidObjectID)
            {
                return BORegistry.BusinessObjectManager.GetObjectIfInManager(key.ObjectID);
            }
            BOPrimaryKey boPrimaryKey = ((BOPrimaryKey) key);
//            IBusinessObjectCollection find = businessObjectManager.Find(boPrimaryKey.GetKeyCriteria(), boType);
//            return find.Count > 0 ? find[0] : null;
            return businessObjectManager.FindFirst(boPrimaryKey.GetKeyCriteria(), boType);*/
        }

        /// <summary>
        /// Sets the Status for the Business Object to NotNew.
        /// </summary>
        /// <param name="bo"></param>
        protected internal static void SetStatusAfterLoad(IBusinessObject bo)
        {
            if (bo == null) return;
            BusinessObject businessObject = (BusinessObject)bo;
            businessObject.SetStatus(BOStatus.Statuses.isNew, false);
        }       
        
        /// <summary>
        /// Calls the AfterLoad method on the IBusinessObject (by casting it to a <see cref="BusinessObject"/>).
        /// </summary>
        /// <param name="bo"></param>
        protected internal static void CallAfterLoad(IBusinessObject bo)
        {
            if (bo == null) return;
            BusinessObject businessObject = (BusinessObject)bo;
            businessObject.AfterLoad();
        }       

        /// <summary>
        /// Calls the FireUpdatedEvent method on the IBusinessObject (by casting it to a <see cref="BusinessObject"/>).
        /// </summary>
        /// <param name="bo"></param>
        protected internal static void FireUpdatedEvent(IBusinessObject bo)
        {
            if (bo == null) return;
            BusinessObject businessObject = (BusinessObject)bo;
            businessObject.FireUpdatedEvent();
        }
    }
}