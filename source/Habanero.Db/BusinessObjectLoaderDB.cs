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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;

namespace Habanero.DB
{
    ///<summary>
    /// This is an implementation of an <see cref="IBusinessObjectLoader"/>. This is used for loading objects from any database.
    /// This class implements the Loading part of the Data Mapper (165) and more specifically the metadata mapping (306) (Fowler -
    /// 'Patterns of Enterprise Application Architecture')
    ///For details of what this class does, see <see cref="IBusinessObjectLoader"/>.
    ///
    /// All queries (including custom SelectQuery objects) run by this loader will be done using parametrized sql for 
    /// improved type safety, system security and performance.
    /// 
    /// When loading one or more object from the datastore the Business Object loader should check to see if it is already loaded in 
    /// the object manager first. If the object does not exist in the object manager then it must be loaded from the datastore and added
    /// to the object manager <see cref="BusinessObjectManager"/>
    ///</summary>
    [Serializable]
    public class BusinessObjectLoaderDB : BusinessObjectLoaderBase, IBusinessObjectLoader
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ConcurrentDictionary<IClassDef, SingleItemStack<IBusinessObject>> _tempObjectsByClassDef;
        private readonly ConcurrentDictionary<Type, SingleItemStack<IBusinessObject>> _tempObjectsByType;

        /// <summary>
        /// The Single Item Stack class operates just like a stack, but only retains the most recent item pushed.
        /// As a result it's implementation is very fast and simple.
        /// </summary>
        /// <typeparam name="T">The type of the item to store.</typeparam>
        private class SingleItemStack<T> where T : class
        {
            private readonly object _lock = new object();
            private T _item;

            /// <summary>
            /// Return and remove the current item. Returns null if no item exists.
            /// </summary>
            /// <returns>The most recently pushed item if it hasn't been popped yet.</returns>
            public T Pop()
            {
                lock (_lock)
                {
                    var item = _item;
                    _item = null;
                    return item;
                }
            }

            /// <summary>
            /// Sets the latest item, replacing any item that was pushed before.
            /// </summary>
            /// <param name="item">The item to push.</param>
            public void Push(T item)
            {
                lock (_lock)
                {
                    _item = item;
                }
            }
        }

        ///<summary>
        /// Creates a BusinessObjectLoaderDB. Because this is a loader the loads data from a Database, this constructor
        /// requires an IDatabaseConnection object to be passed to it.  This connection will be used for all loading.
        ///</summary>
        ///<param name="databaseConnection"></param>
        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _tempObjectsByClassDef = new ConcurrentDictionary<IClassDef, SingleItemStack<IBusinessObject>>();
            _tempObjectsByType = new ConcurrentDictionary<Type, SingleItemStack<IBusinessObject>>();
        }

        /// <summary>
        /// The <see cref="IDatabaseConnection"/> this loader is using.
        /// </summary>
        public IDatabaseConnection DatabaseConnection
        {
            get { return _databaseConnection; }
        }

        ///<summary>
        ///Create a select Query based on the class definition and the primary key.
        ///</summary>
        ///<param name="classDef">The class definition.</param>
        ///<param name="primaryKey">The primary key of the object.</param>
        ///<returns></returns>
        public ISelectQuery GetSelectQuery(IClassDef classDef, IPrimaryKey primaryKey)
        {
            return GetSelectQuery(classDef, Criteria.FromPrimaryKey(primaryKey));
        }

        #region GetBusinessObject

        /// <summary>
        /// Loads a business object of type T, using the Primary key given as the criteria
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements 
        /// IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. 
        /// If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            var businessObject = GetBusinessObject<T>(Criteria.FromPrimaryKey(primaryKey));
            if (businessObject != null) return businessObject;

            throw new BusObjDeleteConcurrencyControlException
                (string.Format
                     ("A Error has occured since the object you are trying to refresh has been deleted by another user."
                      + " There are no records in the database for the Class: {0} identified by {1} \n", typeof (T).Name,
                      primaryKey));
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the Primary key given as the criteria
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            IBusinessObject businessObject = GetBusinessObject(classDef, Criteria.FromPrimaryKey(primaryKey));
            if (businessObject != null) return businessObject;

            throw new BusObjDeleteConcurrencyControlException
                (string.Format
                     ("A Error has occured since the object you are trying to refresh has been deleted by another user."
                      + " There are no records in the database for the Class: {0} identified by {1} \n",
                      classDef.ClassNameFull, primaryKey));
        }

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="criteria">The search criteria</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            IClassDef classDef = ClassDef.ClassDefs[typeof (T)];
            ISelectQuery selectQuery = GetSelectQuery(classDef, criteria);
            return GetBusinessObject<T>(selectQuery);
        }

        ///<summary>
        ///Create a select Query based on the class definition and the search criteria.
        ///</summary>
        ///<param name="classDef">The class definition.</param>
        ///<param name="criteria">The load criteria.</param>
        ///<returns></returns>
        protected ISelectQuery GetSelectQuery(IClassDef classDef, Criteria criteria)
        {
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            selectQuery.Criteria = criteria;
            return selectQuery;
        }

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="selectQuery">The select query</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            var classDef = ClassDef.Get<T>();
            var source = selectQuery.Source;
            QueryBuilder.PrepareSource(classDef, ref source);
            selectQuery.Source = source;
            QueryBuilder.PrepareCriteria(classDef, selectQuery.Criteria);
            var selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            var statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            T loadedBo = null;
            var objectUpdatedInLoading = false;
            using (var dr = _databaseConnection.LoadDataReader(statement))
            {
                if (dr.Read())
                {
                    loadedBo = LoadBOFromReader<T>(dr, selectQueryDB, out objectUpdatedInLoading);

                    //Checks to see if the loaded object is the base of a single table inheritance structure
                    // and has a sub type if so then returns the correct sub type.
                    correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
                    //Checks to see if there is a duplicate object meeting this criteria
                    if (dr.Read()) ThrowRetrieveDuplicateObjectException(statement, loadedBo);
                   
                }
            }
            if (correctSubClassDef != null) 
            {
                loadedBo = GetLoadedBoOfSpecifiedType(loadedBo, correctSubClassDef);
            }
            if (loadedBo == null) return null;
            var isFreshlyLoaded = loadedBo.Status.IsNew;
            SetStatusAfterLoad(loadedBo);
            if (objectUpdatedInLoading)
            {
                CallAfterLoad(loadedBo);
                if (!isFreshlyLoaded) FireUpdatedEvent(loadedBo);
            }
            
            return loadedBo;
        }

        private T LoadBOFromReader<T>(IDataRecord dataReader, ISelectQuery selectQuery, out bool objectUpdatedInLoading) 
                where T : class, IBusinessObject, new()
        {
            var tempBoCache = _tempObjectsByType.GetOrAdd(typeof (T), type => new SingleItemStack<IBusinessObject>());
            var tempBo = (T) tempBoCache.Pop() ?? CreateNewTempBo<T>();

            var loadedBusinessObject = GetLoadedBusinessObject(tempBo, dataReader, selectQuery, out objectUpdatedInLoading);
            if (loadedBusinessObject != tempBo)
            {
                tempBoCache.Push(tempBo);
            }
            return (T)loadedBusinessObject;
        }

        private static T CreateNewTempBo<T>() where T : class, IBusinessObject, new()
        {
            var newTempBo = new T();
            BORegistry.BusinessObjectManager.Remove(newTempBo);
            return newTempBo;
        }


        private static void ThrowRetrieveDuplicateObjectException(ISqlStatement statement, IBusinessObject loadedBo)
        {
            throw new HabaneroDeveloperException
                ("There was an error with loading the class '" + loadedBo.ClassDef.ClassNameFull + "'",
                 "Loading a '" + loadedBo.ClassDef.ClassNameFull + "' with criteria '" + statement.Statement
                 + "' returned more than one record when only one was expected.");
        }

        /// <summary>
        /// Returns the business object of the type provided that meets the search criteria.  
        /// An exception is thrown if more than one business object is found that matches the criteria.  
        /// If that situation could arise, rather use GetBusinessObjectCol.
        /// </summary>
        /// <param name="classDef">The class def of the business object being loaded</param>
        /// <param name="criteria">The load criteria of the object being loaded.</param>
        /// <returns>Returns the business object found</returns>
        /// <exception cref="UserException">Thrown if more than one object matches the criteria</exception>
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            var selectQuery = QueryBuilder.CreateSelectQuery(classDef);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            selectQuery.Criteria = criteria;
            return GetBusinessObject(classDef, selectQuery);
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
            var source = selectQuery.Source;
            QueryBuilder.PrepareSource(classDef, ref source);
            selectQuery.Source = source;
            QueryBuilder.PrepareCriteria(classDef, selectQuery.Criteria);
            var selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            var statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            IBusinessObject loadedBo = null;
            var objectUpdatedInLoading = false;
            using (var dr = _databaseConnection.LoadDataReader(statement))
            {
                if (dr.Read())
                {
                    loadedBo = LoadBOFromReader(classDef, dr, selectQueryDB, out objectUpdatedInLoading);
                    correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);

                    if (dr.Read())
                    {
                        ThrowRetrieveDuplicateObjectException(statement, loadedBo);
                    }
                }
            }
            if (correctSubClassDef != null)
            {
                BORegistry.BusinessObjectManager.Remove(loadedBo);
                var subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = subClassBusinessObject;
            }
            if (loadedBo == null) return null;
            var isFreshlyLoaded = loadedBo.Status.IsNew;
            SetStatusAfterLoad(loadedBo);
            if (objectUpdatedInLoading)
            {
                CallAfterLoad(loadedBo);
                if (!isFreshlyLoaded) FireUpdatedEvent(loadedBo);
            }
            
            return loadedBo;
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
            var criteria = GetCriteriaObject(ClassDef.ClassDefs[typeof (T)], criteriaString);
            return GetBusinessObject<T>(criteria);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteriaString">The criteria to use to load the business object must be of formst "PropName = criteriaValue" e.g. "Surname = Powell"</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an error is raised</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString)
        {
            var criteria = GetCriteriaObject(classDef, criteriaString);
            return GetBusinessObject(classDef, criteria);
        }

        private static Criteria GetCriteriaObject(IClassDef classDef, string criteriaString)
        {
            var criteria = CriteriaParser.CreateCriteria(criteriaString);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            return criteria;
        }

        #endregion

        #region GetBusinessObjectCollection

        /// <summary>
        /// Load the Business Objects from the specific DataStore type that applies to this loader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classDef"></param>
        /// <param name="selectQuery"></param>
        /// <returns></returns>
        protected override LoaderResult GetObjectsFromDataStore<T>(IClassDef classDef, ISelectQuery selectQuery) 
        {
            int totalCountAvailableForPaging;
            var selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);

            var totalNoOfRecords = GetTotalNoOfRecordsIfNeeded(classDef, selectQueryDB);
            List<LoadedBoInfo> loadedBoInfos;
            string loadMechanismDescription;
            if (IsLoadNecessary(selectQueryDB, totalNoOfRecords))
            {
                var statement = CreateStatementAdjustedForLimits(selectQueryDB, totalNoOfRecords);

                loadedBoInfos = GetLoadedBusinessObjectsFromDB<T>(classDef, statement, selectQueryDB);

                totalCountAvailableForPaging = totalNoOfRecords == -1 ? loadedBoInfos.Count : totalNoOfRecords;
                loadMechanismDescription = statement.ToString();
            }
            else
            {
                // The load you ARE concerned about limits, and it is past the end of the total, or the limit is 0
                loadedBoInfos = new List<LoadedBoInfo>();
                totalCountAvailableForPaging = totalNoOfRecords;
                loadMechanismDescription = "[No SQL query due to request of a load past the end of the dataset or of a zero sized limit]";
            }
            
            return new LoaderResult
                {
                    LoadedBoInfos = loadedBoInfos,
                    TotalCountAvailableForPaging = totalCountAvailableForPaging,
                    LoadMechanismDescription = loadMechanismDescription,
                };
        }

        /// <summary>
        /// Returns a message describing the duplicate persisted objects
        /// </summary>
        /// <param name="selectQuery">The select query</param>
        /// <param name="loadMechanismDescription">A description of the load mechanism</param>
        /// <returns>A descriptive error message</returns>
        protected override string GetDuplicatePersistedObjectsErrorMessage(ISelectQuery selectQuery, string loadMechanismDescription)
        {
            return String.Format("This can be caused by a view returning duplicates or where the primary key of your object is incorrectly defined. " 
                + "The database table used for loading this collections was '{0}' and the original load sql query was as follows: '{1}'", selectQuery.Source, loadMechanismDescription);
        }

        /// <summary>
        /// Actually loads the objects using the statement given.
        /// </summary>
        /// <typeparam name="T">The type of class you are loading</typeparam>
        /// <param name="classDef">The classdef to use when loading</param>
        /// <param name="statement">The sql statement to run</param>
        /// <param name="selectQuery">The original select query</param>
        /// <returns></returns>
        protected virtual List<LoadedBoInfo> GetLoadedBusinessObjectsFromDB<T>(IClassDef classDef, ISqlStatement statement, SelectQueryDB selectQuery) where T : IBusinessObject
        {
            var loadedBoInfos = new List<LoadedBoInfo>();
            using (var dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    var loadedBoInfo = LoadCorrectlyTypedBOFromReader<T>(dr, classDef, selectQuery);
                    loadedBoInfos.Add(loadedBoInfo);
                }
            }
            return loadedBoInfos;
        }

        private LoadedBoInfo LoadCorrectlyTypedBOFromReader<T>(IDataReader dr, IClassDef classDef, SelectQueryDB selectQuery) where T : IBusinessObject
        {
            bool objectUpdatedInLoading;
            var loadedBo = (T) LoadBOFromReader(classDef, dr, selectQuery, out objectUpdatedInLoading);
            //Checks to see if the loaded object is the base of a single table inheritance structure
            // and has a sub type
            var correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
            // loads an object of the correct sub type (for single table inheritance)
            loadedBo = GetLoadedBoOfSpecifiedType(loadedBo, correctSubClassDef);

            // these collections are used to determine which objects should the AfterLoad and FireUpdatedEvent methods be called on
            var loadedBoInfo = new LoadedBoInfo();
            loadedBoInfo.IsFreshlyLoaded = loadedBo.Status.IsNew;
            SetStatusAfterLoad(loadedBo);
            loadedBoInfo.LoadedBo = loadedBo;
            loadedBoInfo.IsUpdatedInLoading = objectUpdatedInLoading;
            return loadedBoInfo;
        }

        private int GetTotalNoOfRecordsIfNeeded(IClassDef classDef, ISelectQuery selectQuery)
        {
            int totalNoOfRecords = -1;
            if ((selectQuery.FirstRecordToLoad > 0) || (selectQuery.Limit >= 0))
            {
                totalNoOfRecords = GetCount(classDef, selectQuery.Criteria);
            }
            return totalNoOfRecords;
        }

        private static bool IsLoadNecessary(ISelectQuery selectQuery, int totalNoOfRecords)
        {
            if (selectQuery.FirstRecordToLoad < 0)
            {
                throw new IndexOutOfRangeException("FirstRecordToLoad should not be negative.");
            }
            //If the total number of records has not been determined, then the load is necessary.
            if (totalNoOfRecords < 0) return true;
            //If the limit is set to zero, then it is pointless to do a load.
            if (selectQuery.Limit == 0) return false;
            //If the first record is beyond the end of the records then there is nothing to load.
            return selectQuery.FirstRecordToLoad <= totalNoOfRecords;
        }

        private static ISqlStatement CreateStatementAdjustedForLimits(SelectQueryDB selectQuery, int totalNoOfRecords)
        {
            int originalLimit = selectQuery.Limit;
            if (selectQuery.FirstRecordToLoad > 0)
            {
                int remainingNumberOfItems = totalNoOfRecords - selectQuery.FirstRecordToLoad;
                if (originalLimit >= 0)
                {
                    selectQuery.Limit = (originalLimit < remainingNumberOfItems)
                                            ? originalLimit
                                            : remainingNumberOfItems;
                }
                else
                {
                    selectQuery.Limit = remainingNumberOfItems;
                }
            }
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            selectQuery.Limit = originalLimit;
            return statement;
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        public int GetCount(IClassDef classDef, Criteria criteria)
        {
            var selectQuery = QueryBuilder.CreateSelectCountQuery(classDef, criteria);
            var selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            var sqlFormatter = new SqlFormatter("", "", "", "");
            var statement = selectQueryDB.CreateSqlStatement(sqlFormatter);
            var totalNoOfRecords = 0;
            using (var dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    totalNoOfRecords = Convert.ToInt32(dr[0].ToString());
                }
            }
            return totalNoOfRecords;
        }

        /// <summary>
        /// loads an object of the correct sub type (for single table inheritance)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadedBo"></param>
        /// <param name="correctSubClassDef"></param>
        /// <returns></returns>
        private T GetLoadedBoOfSpecifiedType<T>(T loadedBo, IClassDef correctSubClassDef)
            where T : IBusinessObject
        {
            if (correctSubClassDef != null)
            {
                BORegistry.BusinessObjectManager.Remove(loadedBo);
                var subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = (T) subClassBusinessObject;
            }
            return loadedBo;
        }

        /// <summary>
        /// Reloads a businessObject from the datasource using the id of the object.
        /// A dirty object will not be refreshed from the database and the appropriate error will be raised.
        /// Cancel all edits before refreshing the object.
        /// </summary>
        /// <exception cref="HabaneroDeveloperException">Exception thrown if the object is dirty and refresh is called.</exception>
        /// <param name="businessObject">The businessObject to refresh</param>
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

        #endregion

        #region GetRelatedBusinessObject

        /// <summary>
        /// Loads a business object of type T using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <typeparam name="T">The type of the business object to load</typeparam>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of type T if one was found, otherwise null</returns>
        public T GetRelatedBusinessObject<T>(SingleRelationship<T> relationship) where T : class, IBusinessObject, new()
        {
            return GetBusinessObject<T>(Criteria.FromRelationship(relationship));
        }

        /// <summary>
        /// Loads a business object using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of the type defined by the relationship if one was found, otherwise null</returns>
        public IBusinessObject GetRelatedBusinessObject(ISingleRelationship relationship)
        {
            RelationshipDef relationshipDef = (RelationshipDef) relationship.RelationshipDef;
            if (relationshipDef.RelatedObjectClassDef != null)
                return GetBusinessObject(relationshipDef.RelatedObjectClassDef, Criteria.FromRelationship(relationship));
            return null;
        }

        #endregion

       // ReSharper disable RedundantAssignment

        private IBusinessObject LoadBOFromReader
            (IClassDef classDef, IDataRecord dataReader, ISelectQuery selectQuery, out bool objectUpdatedInLoading)
        {
            objectUpdatedInLoading = false;
            var tempBoCache = _tempObjectsByClassDef.GetOrAdd(classDef, def => new SingleItemStack<IBusinessObject>());
            var tempBo = tempBoCache.Pop() ?? CreateNewTempBo(classDef);

            IBusinessObject loadedBusinessObject = GetLoadedBusinessObject(tempBo, dataReader, selectQuery, out objectUpdatedInLoading);
            if (loadedBusinessObject != tempBo)
            {
                tempBoCache.Push(tempBo);
            }
            return loadedBusinessObject;
        }


        private static IBusinessObject CreateNewTempBo(IClassDef classDef)
        {
            var newTempBo = classDef.CreateNewBusinessObject();
            BORegistry.BusinessObjectManager.Remove(newTempBo);
            return newTempBo;
        }


        // ReSharper restore RedundantAssignment
        private IBusinessObject GetLoadedBusinessObject
            (IBusinessObject bo, IDataRecord dataReader, ISelectQuery selectQuery, out bool objectUpdatedInLoading)
        {
            objectUpdatedInLoading = false;
            PopulateBOFromReader(bo, dataReader, selectQuery);
            IPrimaryKey key = bo.ID;

            IBusinessObject boFromObjectManager = GetObjectFromObjectManager(key, bo.ClassDef.ClassType);
            var boManager = BORegistry.BusinessObjectManager;
            if (boFromObjectManager == null )
            {
                objectUpdatedInLoading = true;
                boManager.Add(bo);
                return bo;
            }
            //This is a Hack to deal with the fact that Class table inheritance does not work well
            // i.e. if ContactPerson inherits from Contact and you load a collection of Contacts then
            // the contact person will load as type contact. If you later try load this as a ContactPerson
            // then the incorrect type is loaded from the BusinessObjectManager.
            if (!bo.GetType().IsInstanceOfType(boFromObjectManager) && bo.ClassDef.IsUsingClassTableInheritance())
            {
                boManager.Remove(boFromObjectManager);
                boManager.Add(bo);
                return bo;
            }
            // if the object is new it means there is an object in the BusinessObjectManager that has the same primary
            // key as the one being loaded.  We want to return the one that was loaded without putting it into the 
            // BusinessObjectManager (as that would cause an error).  This is only used to check for duplicates or in 
            // similar scenarios.

            if (boFromObjectManager.Status.IsNew) boFromObjectManager = bo;
            if (boFromObjectManager.Status.IsEditing) return boFromObjectManager;

            objectUpdatedInLoading = PopulateBOFromReader(boFromObjectManager, dataReader, selectQuery);
            return boFromObjectManager;
        }

        /// <summary>
        /// Checks to see if the loaded object is the base of a single table inheritance structure
        ///   and has a sub type
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        protected static IClassDef GetCorrectSubClassDef(IBusinessObject bo, IDataRecord dataReader)
        {
            var classDef = (ClassDef) bo.ClassDef;
            var subClasses = classDef.AllChildren;
            foreach (ClassDef immediateChild in subClasses)
            {
                if (!immediateChild.IsUsingSingleTableInheritance()) continue;

                var discriminatorFieldName = immediateChild.SuperClassDef.Discriminator;
                var discriminatorValue = Convert.ToString(dataReader[discriminatorFieldName]);

                if (String.IsNullOrEmpty(discriminatorValue)) continue;

                var subClassDef = ClassDef.ClassDefs.FindByClassName(discriminatorValue);

                if (subClassDef != null && !ReferenceEquals(subClassDef, classDef)) return subClassDef;
            }
            return null;
        }

        private static bool PopulateBOFromReader(IBusinessObject bo, IDataRecord dr, ISelectQuery selectQuery)
        {
            var i = 0;
            var objectUpdatedInLoading = false;
            foreach (var field in selectQuery.Fields.Values)
            {
                try
                {
                    IBOProp boProp = bo.Props[field.PropertyName];
                    objectUpdatedInLoading = objectUpdatedInLoading | boProp.InitialiseProp(dr[i]);   // set objectUpdatedInLoading to true if any initialiseprop returns true
                } catch (InvalidPropertyNameException)
                {
                    // do nothing - this was to increase performance as catching this exception is quicker than always doing a bo.Props.Contains(field.PropertyName) call.
                }
                i += 1;
            }
            return objectUpdatedInLoading;
        }
    }
}