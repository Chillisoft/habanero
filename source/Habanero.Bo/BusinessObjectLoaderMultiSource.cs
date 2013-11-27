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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Data;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// This is an <see cref="IBusinessObjectLoader"/> that allows you to 
    /// Register the loader to use for various types.
    /// This allows the developer to load different obejcts in their
    /// application from different datasources e.g. Diff Databases.
    ///</summary>
    public class BusinessObjectLoaderMultiSource : IBusinessObjectLoader
    {
        private readonly IBusinessObjectLoader _defaultBusinessObjectLoader;
        private readonly Dictionary<Type, IBusinessObjectLoader> _businessObjectLoaders;

        ///<summary>
        /// Creates this Loader with a default loader.
        /// This will be used in cases where there is not a specific loader registered.
        ///</summary>
        ///<param name="defaultBusinessObjectLoader"></param>
        public BusinessObjectLoaderMultiSource(IBusinessObjectLoader defaultBusinessObjectLoader)
        {
            _defaultBusinessObjectLoader = defaultBusinessObjectLoader;
            _businessObjectLoaders = new Dictionary<Type, IBusinessObjectLoader>();

        }

        ///<summary>
        /// registered a specific Business Object loader with a specified Object Type.
        /// When any object of this type is loaded in future it will be loaded with this.
        /// Loader. This allows you to have business objects loaded from Different sources.
        /// e.g. Different databases XML and Databases etc.
        /// The a loader is not registered for a specific type then the default loader will be used.
        ///</summary>
        ///<param name="type"></param>
        ///<param name="businessObjectLoader"></param>
        public void AddBusinessObjectLoader(Type type, IBusinessObjectLoader businessObjectLoader)
        {
            _businessObjectLoaders.Add(type, businessObjectLoader);
        }

        /// <summary>
        /// Loads a business object of type T, using the Primary key given as the criteria
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof (T)].GetBusinessObject<T>(primaryKey);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(primaryKey);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the Primary key given as the criteria
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, primaryKey);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, primaryKey);
        }

        /// <summary>
        /// Loads a business object of type T, using the criteria given
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(criteria);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(criteria);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, criteria);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, criteria);
        }

        /// <summary>
        /// Loads a business object of type T, using the SelectQuery given. It's important to make sure that T (meaning the ClassDef set up for T)
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a business object
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found, an error is raised</returns>
        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(selectQuery);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, 
        /// using the SelectQuery given. It's important to make sure that the ClassDef parameter given
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a business object
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, ISelectQuery selectQuery)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, selectQuery);
        }

        /// <summary>
        /// Loads a business object of type T, using the SelectQuery given. It's important to make sure that T (meaning the ClassDef set up for T)
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a business object
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The select query to use to load from the data source</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(criteriaString);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteriaString">The criteria to use to load the business object must be of formst "PropName = criteriaValue" e.g. "Surname = Powell"</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an error is raised</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, criteriaString);
        }

        /// <summary>
        /// Loads a business object of type T using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <typeparam name="T">The type of the business object to load</typeparam>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of type T if one was found, otherwise null</returns>
        public T GetRelatedBusinessObject<T>(SingleRelationship<T> relationship) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetRelatedBusinessObject(relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObject(relationship);
        }

        /// <summary>
        /// Loads a business object using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of the type defined by the relationship if one was found, otherwise null</returns>
        public IBusinessObject GetRelatedBusinessObject(ISingleRelationship relationship)
        {
            if (_businessObjectLoaders.ContainsKey(relationship.RelatedObjectClassDef.ClassType))
                return _businessObjectLoaders[relationship.RelatedObjectClassDef.ClassType].GetRelatedBusinessObject(relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObject(relationship);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, criteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="searchCriteria">The select query to use to load from the data source</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, searchCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, searchCriteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, IOrderCriteria orderCriteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria, orderCriteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria, IOrderCriteria orderCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, criteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria, orderCriteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteriaString">The criteria to use to load the business object collection</param>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        /// <returns>The loaded collection</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString, orderCriteria);
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the searchCriteria an given. It's important to make sure that the ClassDef given
        /// has the properties defined in the fields of the select searchCriteria and orderCriteria.  
        /// </summary>
        /// <param name="classDef">The ClassDef for the collection to load</param>
        /// <param name="searchCriteria">The select query to use to load from the data source</param>
        /// <param name="orderCriteria">The order that the collections must be loaded in e.g. Surname, FirstName</param>
        /// <returns>The loaded collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria, string orderCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, searchCriteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, searchCriteria, orderCriteria);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of Ts specified as follows:
        /// If you want record 6 to 15 then 
        /// <paramref name="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <paramref name="numberOfRecordsToLoad"/> will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <paramref name="orderCriteria"/>).
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
        /// <param name="criteria">The search criteria</param>
        /// <param name="orderCriteria">The order-by clause</param>
        /// <param name="firstRecordToLoad">The first record to load (NNB: this is zero based)</param>
        /// <param name="numberOfRecordsToLoad">The number of records to be loaded</param>
        /// <param name="totalNoOfRecords">The total number of records matching the criteria</param>
        /// <returns>The loaded collection, limited in the specified way.</returns>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, IOrderCriteria orderCriteria, int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }

        /// <summary>
        /// Loads the Business object collection with the appropriate items.
        /// See <see cref="IBusinessObjectLoader.GetBusinessObjectCollection{T}(Habanero.Base.Criteria,Habanero.Base.IOrderCriteria,int,int,out int)"/> for a full explanation.
        /// </summary>
        /// <param name="def"></param>
        /// <param name="criteria"></param>
        /// <param name="orderCriteria"></param>
        /// <param name="firstRecordToLoad"></param>
        /// <param name="numberOfRecordsToLoad"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef def, Criteria criteria, IOrderCriteria orderCriteria, int firstRecordToLoad, int numberOfRecordsToLoad, out int records)
        {
            if (_businessObjectLoaders.ContainsKey(def.ClassType))
                return _businessObjectLoaders[def.ClassType].GetBusinessObjectCollection(def, criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out records);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(def, criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out records);
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
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteriaString, int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString, orderCriteriaString, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString, orderCriteriaString, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
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
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(selectQuery);
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
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, selectQuery);
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
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                _businessObjectLoaders[typeof(T)].Refresh(collection);
            else
                _defaultBusinessObjectLoader.Refresh(collection);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public void Refresh(IBusinessObjectCollection collection)
        {
            if (_businessObjectLoaders.ContainsKey(collection.ClassDef.ClassType))
                _businessObjectLoaders[collection.ClassDef.ClassType].Refresh(collection);
            else
                _defaultBusinessObjectLoader.Refresh(collection);
        }

        /// <summary>
        /// Reloads a businessObject from the datasource using the id of the object.
        /// A dirty object will not be refreshed from the database and the appropriate error will be raised.
        /// Cancel all edits before refreshing the object or save before refreshing.
        /// </summary>
        /// <exception cref="HabaneroDeveloperException">Exception thrown if the object is dirty and refresh is called.</exception>
        /// <param name="businessObject">The businessObject to refresh</param>
        public IBusinessObject Refresh(IBusinessObject businessObject)
        {
            if (_businessObjectLoaders.ContainsKey(businessObject.GetType()))
                return _businessObjectLoaders[businessObject.GetType()].Refresh(businessObject);
            return _defaultBusinessObjectLoader.Refresh(businessObject);
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
        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IMultipleRelationship relationship) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetRelatedBusinessObjectCollection<T>(relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObjectCollection<T>(relationship);
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
            if (_businessObjectLoaders.ContainsKey(type))
                return _businessObjectLoaders[type].GetRelatedBusinessObjectCollection(type, relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObjectCollection(type, relationship);
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
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectByValue(classDef, idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue(classDef, idValue);
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
            if (_businessObjectLoaders.ContainsKey(type))
                return _businessObjectLoaders[type].GetBusinessObjectByValue(type, idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue(type, idValue);
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
        public T GetBusinessObjectByValue<T>(object idValue) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectByValue<T>(idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue<T>(idValue);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        public int GetCount(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetCount(classDef, criteria);
            return _defaultBusinessObjectLoader.GetCount(classDef, criteria);
        }

    }
}