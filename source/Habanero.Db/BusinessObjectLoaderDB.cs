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
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

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
        private IDictionary<IClassDef, IBusinessObject> _tempObjectsByClassDef;
        private IDictionary<Type, IBusinessObject> _tempObjectsByType;

        ///<summary>
        /// Creates a BusinessObjectLoaderDB. Because this is a loader the loads data from a Database, this constructor
        /// requires an IDatabaseConnection object to be passed to it.  This connection will be used for all loading.
        ///</summary>
        ///<param name="databaseConnection"></param>
        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _tempObjectsByClassDef = new Dictionary<IClassDef, IBusinessObject>();
            _tempObjectsByType = new Dictionary<Type, IBusinessObject>();
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
            T businessObject = GetBusinessObject<T>(Criteria.FromPrimaryKey(primaryKey));
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
            IClassDef classDef = ClassDef.Get<T>();
            Source source = selectQuery.Source;
            QueryBuilder.PrepareSource(classDef, ref source);
            selectQuery.Source = source;
            QueryBuilder.PrepareCriteria(classDef, selectQuery.Criteria);
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            T loadedBo = null;
            bool objectUpdatedInLoading = false;
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
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
            bool isFreshlyLoaded = loadedBo.Status.IsNew;
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
            // Peter: this code is here to improve performance.  It's a little messy, but essentially a "temp" object
            // is stored in a dictionary and reused as the object populated to perform a search on the business object
            // manager.

            T bo = GetTempBO<T>();

            IBusinessObject loadedBusinessObject = GetLoadedBusinessObject(bo, dataReader, selectQuery, out objectUpdatedInLoading);
            if (loadedBusinessObject == bo)
            {
                var tempObject = new T();
                _tempObjectsByType[typeof(T)] = tempObject;
                BORegistry.BusinessObjectManager.Remove(tempObject);
            }
            return (T)loadedBusinessObject;
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
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(classDef);
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
            Source source = selectQuery.Source;
            QueryBuilder.PrepareSource(classDef, ref source);
            selectQuery.Source = source;
            QueryBuilder.PrepareCriteria(classDef, selectQuery.Criteria);
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            IBusinessObject loadedBo = null;
            bool objectUpdatedInLoading = false;
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
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
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = subClassBusinessObject;
            }
            if (loadedBo == null) return null;
            bool isFreshlyLoaded = loadedBo.Status.IsNew;
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
            Criteria criteria = GetCriteriaObject(ClassDef.ClassDefs[typeof (T)], criteriaString);
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
            Criteria criteria = GetCriteriaObject(classDef, criteriaString);
            return GetBusinessObject(classDef, criteria);
        }

        private static Criteria GetCriteriaObject(IClassDef classDef, string criteriaString)
        {
            Criteria criteria = CriteriaParser.CreateCriteria(criteriaString);
            QueryBuilder.PrepareCriteria(classDef, criteria);
            return criteria;
        }

        #endregion

        #region GetBusinessObjectCollection

        protected override LoaderResult GetObjectsFromDataStore<T>(IClassDef classDef, ISelectQuery selectQuery) 
        {
            int totalCountAvailableForPaging;
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);

            int totalNoOfRecords = GetTotalNoOfRecordsIfNeeded(classDef, selectQueryDB);
            List<LoadedBoInfo> loadedBoInfos;
            string loadMechanismDescription;
            if (IsLoadNecessary(selectQueryDB, totalNoOfRecords))
            {
                ISqlStatement statement = CreateStatementAdjustedForLimits(selectQueryDB, totalNoOfRecords);

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

        protected override string GetDuplicatePersistedObjectsErrorMessage(ISelectQuery selectQuery, string loadMechanismDescription)
        {
            return String.Format("This can be caused by a view returning duplicates or where the primary key of your object is incorrectly defined. " 
                + "The database table used for loading this collections was '{0}' and the original load sql query was as follows: '{1}'", selectQuery.Source, loadMechanismDescription);
        }

        protected virtual List<LoadedBoInfo> GetLoadedBusinessObjectsFromDB<T>(IClassDef classDef, ISqlStatement statement, SelectQueryDB selectQuery) where T : IBusinessObject
        {
            List<LoadedBoInfo> loadedBoInfos = new List<LoadedBoInfo>();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    var loadedBoInfo = LoadCorrectlyTypedBOFromReader<T>(dr, classDef, selectQuery);
                    loadedBoInfos.Add(loadedBoInfo);
                }
            }
            return loadedBoInfos;
        }

        private static void AddBusinessObjectToCollection(IBusinessObjectCollection collection, IBusinessObject loadedBo, Dictionary<string, IBusinessObject> originalPersistedCollection, bool isFirstLoad)
        {
            //If the origional collection had the new business object then
            // use add internal this adds without any events being raised etc.
            //else adds via the Add method (normal add) this raises events such that the 
            // user interface can be updated.
            if (isFirstLoad)
            {
                collection.AddWithoutEvents(loadedBo);
                collection.PersistedBusinessObjects.Add(loadedBo);
            }
            else
            {
                AddBusinessObjectToCollection(collection, loadedBo, originalPersistedCollection);
            }
        }

        private LoadedBoInfo LoadCorrectlyTypedBOFromReader<T>(IDataReader dr, IClassDef classDef, SelectQueryDB selectQuery) where T : IBusinessObject
        {
            bool objectUpdatedInLoading;
            T loadedBo = (T) LoadBOFromReader(classDef, dr, selectQuery, out objectUpdatedInLoading);
            //Checks to see if the loaded object is the base of a single table inheritance structure
            // and has a sub type
            IClassDef correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
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
            ISelectQuery selectQuery = QueryBuilder.CreateSelectCountQuery(classDef, criteria);
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery, _databaseConnection);
            SqlFormatter sqlFormatter = new SqlFormatter("", "", "", "");
            ISqlStatement statement = selectQueryDB.CreateSqlStatement(sqlFormatter);
            int totalNoOfRecords = 0;
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
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
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
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

        #region GetRelatedBusinessObjectCollection

        ///// <summary>
        ///// Loads a RelatedBusinessObjectCollection using the Relationship given.  This method is used by relationships to load based on the
        ///// fields defined in the relationship.
        ///// </summary>
        ///// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        ///// <param name="relationship">The relationship that defines the criteria that must be loaded.  For example, a Person might have
        ///// a Relationship called Addresses, which defines the PersonID property as the relationship property. In this case, calling this method
        ///// with the Addresses relationship will load a collection of Address where PersonID = '?', where the ? is the value of the owning Person's
        ///// PersonID</param>
        ///// <returns>The loaded RelatedBusinessObjectCollection</returns>
        //public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship)
        //    where T : class, IBusinessObject, new()
        //{
        //    RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
        //    Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
        //    OrderCriteria preparedOrderCriteria =
        //        QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

        //    BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(relationshipCriteria, preparedOrderCriteria);
        //    foreach (T businessObject in col)
        //    {
        //        AddBusinessObjectToCollection(relatedCol, businessObject);
        //    }
        //    relatedCol.SelectQuery = col.SelectQuery;
        //    return relatedCol;
        //}

        ///// <summary>
        ///// Loads a RelatedBusinessObjectCollection using the Relationship given.  This method is used by relationships to load based on the
        ///// fields defined in the relationship.
        ///// </summary>
        ///// <param name="type">The type of collection to load. This must be a class that implements IBusinessObject</param>
        ///// <param name="relationship">The relationship that defines the criteria that must be loaded.  For example, a Person might have
        ///// a Relationship called Addresses, which defines the PersonID property as the relationship property. In this case, calling this method
        ///// with the Addresses relationship will load a collection of Address where PersonID = '?', where the ? is the value of the owning Person's
        ///// PersonID</param>
        ///// <returns>The loaded RelatedBusinessObjectCollection</returns>
        //public IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IRelationship relationship)
        //{
        //    IBusinessObjectCollection relatedCol = CreateRelatedBusinessObjectCollection(type, relationship);
        //    Criteria relationshipCriteria = Criteria.FromRelationship(relationship);

        //    OrderCriteria preparedOrderCriteria =
        //        QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

        //    IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef,
        //                                                                relationshipCriteria, preparedOrderCriteria);
        //    foreach (IBusinessObject businessObject in col)
        //    {
        //        AddBusinessObjectToCollection(relatedCol, businessObject);
        //    }
        //    relatedCol.SelectQuery = col.SelectQuery;
        //    return relatedCol;
        //}

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

        private T GetTempBO<T>() where T : class, IBusinessObject, new()
        {
            T bo;
            try
            {
                bo = (T) _tempObjectsByType[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                bo = new T();
                BORegistry.BusinessObjectManager.Remove(bo);
                _tempObjectsByType[typeof(T)] = bo;
            }
            return bo;
        }

        private IBusinessObject LoadBOFromReader
            (IClassDef classDef, IDataRecord dataReader, ISelectQuery selectQuery, out bool objectUpdatedInLoading)
        {
            // Peter: this code is here to improve performance.  It's a little messy, but essentially a "temp" object
            // is stored in a dictionary and reused as the object populated to perform a search on the business object
            // manager.
            objectUpdatedInLoading = false;
            IBusinessObject bo;
            try
            {
                bo = _tempObjectsByClassDef[classDef];
            } catch (KeyNotFoundException)
            {
                bo = classDef.CreateNewBusinessObject();
                BORegistry.BusinessObjectManager.Remove(bo);
                _tempObjectsByClassDef[classDef] = bo;
            }

            IBusinessObject loadedBusinessObject = GetLoadedBusinessObject(bo, dataReader, selectQuery, out objectUpdatedInLoading);
            if (loadedBusinessObject == bo)
            {
                var tempObject = classDef.CreateNewBusinessObject();
                _tempObjectsByClassDef[classDef] = tempObject;
                BORegistry.BusinessObjectManager.Remove(tempObject);
            }
            return loadedBusinessObject;
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
//                 ((ClassDef)bo.ClassDef).SuperClassDef
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
            ClassDef classDef = (ClassDef) bo.ClassDef;
            ClassDefCol subClasses = classDef.AllChildren;
            foreach (ClassDef immediateChild in subClasses)
            {
                if (!immediateChild.IsUsingSingleTableInheritance()) continue;

                string discriminatorFieldName = immediateChild.SuperClassDef.Discriminator;
                string discriminatorValue = Convert.ToString(dataReader[discriminatorFieldName]);

                if (String.IsNullOrEmpty(discriminatorValue)) continue;

                IClassDef subClassDef = ClassDef.ClassDefs.FindByClassName(discriminatorValue);

                if (subClassDef != null && !object.ReferenceEquals(subClassDef, classDef)) return subClassDef;
            }
            return null;
        }

        private static bool PopulateBOFromReader(IBusinessObject bo, IDataRecord dr, ISelectQuery selectQuery)
        {
            int i = 0;
            bool objectUpdatedInLoading = false;
            foreach (QueryField field in selectQuery.Fields.Values)
            {
                try
                {
                    IBOProp boProp = bo.Props[field.PropertyName];
                    objectUpdatedInLoading = objectUpdatedInLoading | boProp.InitialiseProp(dr[i]);   // set objectUpdatedInLoading to true if any initialiseprop returns true
                } catch (InvalidPropertyNameException)
                {
                    // do nothing - this was to increase performance as catching this exception is quicker than always doing a
                    // bo.Props.Contains(field.PropertyName) call.
                }
                i++;
            }
            //SetStatusAfterLoad(bo);
            return objectUpdatedInLoading;
        }
    }
}