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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// The business object loader in memory is a business object loader that can be used in place of a normal datastore loader.
    /// This is used to demonstrate the abilitiy to swap out loaders for different datastores and for testing so as to minimise.
    /// The continual database hits that result from testing using a traditional database.
    ///</summary>
    public class BusinessObjectLoaderInMemory : BusinessObjectLoaderBase, IBusinessObjectLoader
    {
        private readonly DataStoreInMemory _dataStore;

        ///<summary>
        /// Constuctor for business object loaded in memory. This constructs the loader with the appropriate data store.
        ///</summary>
        ///<param name="dataStore"></param>
        public BusinessObjectLoaderInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
        }

        #region GetBusinessObject Members

        /// <summary>
        /// Loads a business object of type T, using the Primary key given as the criteria
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            if (_dataStore.AllObjects.ContainsKey(primaryKey.ObjectID))
                return (T) _dataStore.AllObjects[primaryKey.ObjectID];

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
//            if (_dataStore.AllObjects.ContainsKey(primaryKey))
//                return _dataStore.AllObjects[primaryKey];
            IBusinessObject businessObject = _dataStore.Find
                (classDef.ClassType, ((BOPrimaryKey) primaryKey).GetKeyCriteria());
            if (businessObject == null)
            {
                throw new BusObjDeleteConcurrencyControlException
                    (string.Format
                         ("A Error has occured since the object you are trying to refresh has been deleted by another user."
                          + " There are no records in the database for the Class: {0} identified by {1} \n",
                          classDef.ClassNameFull, primaryKey));
            }
            return businessObject;
        }

        /// <summary>
        /// Loads a business object of type T, using the criteria given
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            return GetBusinessObjectShared<T>(criteria);
        }

		private T GetBusinessObjectShared<T>(Criteria criteria) where T : class, IBusinessObject, new()
    	{
			IClassDef classDef = ClassDef.ClassDefs[typeof(T)];
			QueryBuilder.PrepareCriteria(classDef, criteria);
    		return _dataStore.Find<T>(criteria);
    	}

    	private IBusinessObject GetBusinessObjectShared(IClassDef classDef, Criteria criteria)
    	{
    		if (classDef == null) throw new ArgumentNullException("classDef");
			QueryBuilder.PrepareCriteria(classDef, criteria);
    		return _dataStore.Find(classDef, criteria);
    	}

    	/// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
        	return GetBusinessObjectShared(classDef, criteria);
        }

    	/// <summary>
        /// Loads a business object of type T, using the SelectQuery given. It's important to make sure that T (meaning the ClassDef set up for T)
        /// has the properties defined in the fields of the select query.  
        /// This method allows you to define a custom query to load a business object
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="selectQuery">The select query to use to load from the data source</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
			return GetBusinessObjectShared<T>(selectQuery.Criteria);
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
			return GetBusinessObjectShared(classDef, selectQuery.Criteria);
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
            Criteria criteriaObject = CriteriaParser.CreateCriteria(criteriaString);
			return GetBusinessObjectShared<T>(criteriaObject);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteriaString">The criteria to use to load the business object must be of formst "PropName = criteriaValue" e.g. "Surname = Powell"</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an error is raised</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString)
        {
            Criteria criteriaObject = CriteriaParser.CreateCriteria(criteriaString);
			return GetBusinessObjectShared(classDef, criteriaObject);
        }

        /// <summary>
        /// Reloads a businessObject from the datasource using the id of the object.
        /// A dirty object will not be refreshed from the database and the appropriate error will be raised.
        /// Cancel all edits before refreshing the object
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
                     + businessObject.ID.AsString_CurrentValue() + " Class : " + businessObject.ClassDef.ClassNameFull);
            }
            return businessObject;
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        public int GetCount(IClassDef classDef, Criteria criteria)
        {
            IBusinessObjectCollection collection = _dataStore.FindAll(classDef, criteria);
            return collection.Count;
        }

        #endregion //GetBusinessObject Members

        #region GetBusinessObjectCollection Members

        /// <summary>
        /// Load the Business Objects from the specific DataStore type that applies to this loader.
        /// In this case these objects are loaded from a <see cref="DataStoreInMemory"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classDef"></param>
        /// <param name="selectQuery"></param>
        /// <returns></returns>
        protected override LoaderResult GetObjectsFromDataStore<T>(IClassDef classDef, ISelectQuery selectQuery)
        {
            Criteria criteria = selectQuery.Criteria;
            IOrderCriteria orderCriteria = selectQuery.OrderCriteria;

            IBusinessObjectCollection loadedBos = _dataStore.FindAll(classDef, criteria);
            loadedBos.Sort(orderCriteria);

            int totalNoOfRecords = -1;
            int totalCountAvailableForPaging = totalNoOfRecords == -1 ? loadedBos.Count : totalNoOfRecords;

            ApplyLimitsToList(selectQuery, loadedBos);
            var loadedBoInfos = new List<LoadedBoInfo>();
            foreach (var loadedBo in loadedBos)
            {
                loadedBoInfos.Add(new LoadedBoInfo {LoadedBo = (IBusinessObject) loadedBo});
            }
            return new LoaderResult
            {
                LoadedBoInfos = loadedBoInfos,
                TotalCountAvailableForPaging = totalCountAvailableForPaging,
                LoadMechanismDescription = Convert.ToString(selectQuery.Criteria),
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
            return String.Format("This can be caused by the data store returning duplicates or where the primary key of your object is incorrectly defined. "
                + "The source used for loading this collections was '{0}' and the criteria was '{1}'", selectQuery.Source, loadMechanismDescription);
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

        #endregion //GetBusinessObjectCollection Members

        #region GetRelatedBusinessObjectCollection Members

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
            IRelationshipDef relationshipDef = relationship.RelationshipDef;
            if (relationshipDef.RelatedObjectClassDef != null)
                return GetBusinessObject(relationshipDef.RelatedObjectClassDef, Criteria.FromRelationship(relationship));
            return null;
        }

        ///<summary>
        /// Returns the In memory datastore that this Loader is using
        ///</summary>
        ///<returns></returns>
        public DataStoreInMemory GetMemoryDatabase()
        {
            return _dataStore;
        }

        #endregion

       

    }
}