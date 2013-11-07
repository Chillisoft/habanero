#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Base.Exceptions;
using Habanero.BO.Exceptions;
using Habanero.Util;

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
        private static Criteria GetCriteriaObject(IClassDef classDef, string criteriaString)
        {
			return CriteriaParser.CreateCriteria(criteriaString);
        }

        /// <summary>
        /// This protected method will prepare the select query of the collection for the refresh operation.
        /// This will include preparing the criteria so that the sources are correctly linked.
        /// </summary>
        /// <param name="col"></param>
		protected virtual void PrepareForRefresh(IBusinessObjectCollection col)
		{
			var classDef = col.ClassDef;
			Criteria criteria = col.SelectQuery.Criteria;
            QueryBuilder.PrepareCriteria(classDef, criteria);
			//Ensure that all the criteria field sources are merged correctly
			col.SelectQuery.Criteria = criteria;
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
        	PrepareForRefresh(collection);
            DoRefresh(collection);
            boColInternal.Loading = false;
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
			boColInternal.Loading = true; 
			PrepareForRefresh(collection);
            DoRefresh(collection);
            boColInternal.Loading = false;
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="collection">The collection to refresh</param> 
        protected virtual void DoRefresh<T>(BusinessObjectCollection<T> collection) 
            where T : class, IBusinessObject, new()
        {
            DoRefreshShared<T>(collection);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        protected virtual void DoRefresh(IBusinessObjectCollection collection)
        {
            DoRefreshShared<IBusinessObject>(collection);
        }

        /// <summary>
        /// This protected method is the common method used for all collection refreshes and loads
        /// (Directly through the loader, through the relationships, or through the Load or Refresh method on the <see cref="BusinessObjectCollection{TBusinessObject}">BusinessObjectCollection</see>). 
        /// </summary>
        /// <typeparam name="T">The specific or general type of the collection being refreshed.</typeparam>
        /// <param name="collection"></param>
        protected virtual void DoRefreshShared<T>(IBusinessObjectCollection collection)
            where T : IBusinessObject
        {
            IClassDef classDef = collection.ClassDef;
            ISelectQuery selectQuery = collection.SelectQuery;
            LoaderResult loaderResult = GetObjectsFromDataStore<T>(classDef, selectQuery);

            collection.TotalCountAvailableForPaging = loaderResult.TotalCountAvailableForPaging;
            string duplicatePersistedObjectsErrorMessage = GetDuplicatePersistedObjectsErrorMessage(selectQuery, loaderResult.LoadMechanismDescription);
            LoadBOCollection(collection, loaderResult.LoadedBoInfos, duplicatePersistedObjectsErrorMessage);

            // NOTES from the original DB loader...
            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note_: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
        }

        /// <summary>
        /// Load the Business Objects from the specific DataStore type that applies to this loader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classDef"></param>
        /// <param name="selectQuery"></param>
        /// <returns></returns>
        protected abstract LoaderResult GetObjectsFromDataStore<T>(IClassDef classDef, ISelectQuery selectQuery) 
            where T : IBusinessObject;

        protected abstract string GetDuplicatePersistedObjectsErrorMessage(ISelectQuery selectQuery, string loadMechanismDescription);

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
            Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(classDef.ClassType);
            return (IBusinessObjectCollection) Activator.CreateInstance(boColType, classDef);
        }

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
                collection.AddedBusinessObjects.Remove(loadedBo);
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

            boColInternal.Loading = true;
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            IOrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(relationshipCriteria, preparedOrderCriteria);
            LoadBOCollection(relatedCol, (ICollection) col);
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = preparedOrderCriteria;
            boColInternal.Loading = false;
            return relatedCol;
        }

        protected class LoadedBoInfo
        {
            public IBusinessObject LoadedBo { get; set; }
            public bool IsFreshlyLoaded { get; set; }
            public bool IsUpdatedInLoading { get; set; }
        }

        protected class LoaderResult
        {
            public List<LoadedBoInfo> LoadedBoInfos { get; set; }
            public int TotalCountAvailableForPaging { get; set; }
            public string LoadMechanismDescription { get; set; }
        }

        protected void LoadBOCollection(IBusinessObjectCollection collection, ICollection loadedBos, string duplicatePersistedObjectsErrorMessage = "")
        {
            var loadedBoInfos = new List<LoadedBoInfo>();
            foreach (var loadedBo in loadedBos)
            {
                loadedBoInfos.Add(new LoadedBoInfo{ LoadedBo = (IBusinessObject)loadedBo });
            }
            LoadBOCollection(collection, loadedBoInfos, duplicatePersistedObjectsErrorMessage);
        }

        /// <summary>
        /// Adds all the business objects from the <paramref name="loadedBOs"/> collection to the
        /// <see cref="IBusinessObjectCollection"/>
        /// </summary>
        /// <param name="collection">the collection being added to</param>
        /// <param name="loadedBOs">the collection of loaded BOs to add</param>
        protected void LoadBOCollection<T>(IBusinessObjectCollection collection, IList<T> loadedBOs)
        {
            IList col = new ArrayList();
            foreach (T loadedBO in loadedBOs) col.Add(loadedBO);
            LoadBOCollection(collection, col);
        }

        /// <summary>
        /// Adds all the business objects from the <paramref name="loadedBoInfos"/> collection to the
        /// <see cref="IBusinessObjectCollection"/>
        /// </summary>
        /// <param name="collection">the collection being added to</param>
        /// <param name="loadedBoInfos">the collection of loaded BOs to add</param>
        protected void LoadBOCollection(IBusinessObjectCollection collection, List<LoadedBoInfo> loadedBoInfos, string duplicatePersistedObjectsErrorMessage = "")
        {
            
            // if the collection is fresh (ie. being loaded for the first time
            // then load without events.
            if (collection.TimeLastLoaded == null)
            {
                collection.PersistedBusinessObjects.Clear();
                var hasAddedBos = collection.AddedBusinessObjects.Count > 0;
                foreach (LoadedBoInfo loadedBoInfo in loadedBoInfos)
                {
                    IBusinessObject loadedBo = loadedBoInfo.LoadedBo;
                    collection.AddWithoutEvents(loadedBo);
                    collection.PersistedBusinessObjects.Add(loadedBo);
                    if (hasAddedBos) collection.AddedBusinessObjects.Remove(loadedBo);
                }
                FinaliseLoad(loadedBoInfos);
            }  else 
            {
                BOColLoaderHelper.ClearCurrentCollection(collection);

                // store the original persisted collection and pass it through. This is to improve performance
                // within the AddBusinessObjectToCollection method when amount of BO's being loaded is big.
                Dictionary<string, IBusinessObject> originalPersistedCollection;
                originalPersistedCollection = GetOriginalPersistedCollection(collection, duplicatePersistedObjectsErrorMessage);
                foreach (LoadedBoInfo loadedBoInfo in loadedBoInfos)
                {
                    IBusinessObject loadedBo = loadedBoInfo.LoadedBo;
                    AddBusinessObjectToCollection(collection, loadedBo, originalPersistedCollection);
                }
                FinaliseLoad(loadedBoInfos);
                RestoreEditedLists(collection, originalPersistedCollection);
            }
            collection.TimeLastLoaded = DateTime.Now;
            BOColLoaderHelper.FireRefreshedEvent(collection);
        }



        private Dictionary<string, IBusinessObject> GetOriginalPersistedCollection(IBusinessObjectCollection collection, string duplicatePersistedObjectsErrorMessage) 
        {
            // store the original persisted collection and pass it through. This is to improve performance
            // within the AddBusinessObjectToCollection method when amount of BO's being loaded is big.
            Dictionary<string, IBusinessObject> originalPersistedCollection = new Dictionary<string, IBusinessObject>();
            
            bool isFirstLoad = collection.TimeLastLoaded == null;
            if (isFirstLoad)
            {
                collection.PersistedBusinessObjects.Clear();
            }
            else
            {
                foreach (IBusinessObject businessObject in collection.PersistedBusinessObjects)
                {
                    try
                    {
                        originalPersistedCollection.Add(businessObject.ID.AsString_CurrentValue(), businessObject);
                    }
                    catch (ArgumentException)
                    {
                        throw new HabaneroDeveloperException("A duplicate Business Object was found in the persisted objects collection of the BusinessObjectCollection during a reload. This is usually indicative of duplicate items being returned in your original query. " +
                                                                           duplicatePersistedObjectsErrorMessage);
                    }
                }
            }
            return originalPersistedCollection;
        }

        private void FinaliseLoad(List<LoadedBoInfo> loadedBoInfos)
        {
            for (int i = 0; i < loadedBoInfos.Count; i++)
            {
                LoadedBoInfo loadedBoInfo = loadedBoInfos[i];
                if (loadedBoInfo.IsUpdatedInLoading)
                {
                    IBusinessObject loadedBo = loadedBoInfo.LoadedBo;
                    CallAfterLoad(loadedBo);

                    if (!loadedBoInfo.IsFreshlyLoaded)
                    {
                        FireUpdatedEvent(loadedBo);
                    }
                }
            }
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

            IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef,
                                                                        relatedCol.SelectQuery.Criteria, relatedCol.SelectQuery.OrderCriteria);

            LoadBOCollection(relatedCol, col);
            relatedCol.SelectQuery = col.SelectQuery;
            BOColLoaderHelper.SetLoading(relatedCol, false);
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
            var businessObject = businessObjectManager.GetBusinessObject(key);
            if (businessObject != null) 
            {
                var returnType = businessObject.GetType();
                if ((boType != returnType) && (!boType.IsAssignableFrom(returnType) && !returnType.IsAssignableFrom(boType)))
                {
                    throw new HabaneroApplicationException(
                         "Incorrect Business Object type returned for passed in key: perhaps you have conflicting GUID keys? (conflicting key: " + key +
                         "; expected type: " + boType + "; got type: " + businessObject.GetType() + ")"
                         );
                }
            }
            return businessObject;
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