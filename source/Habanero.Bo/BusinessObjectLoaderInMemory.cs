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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

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
            if (_dataStore.AllObjects.ContainsKey(primaryKey))
                return (T) _dataStore.AllObjects[primaryKey];

            throw new BusObjDeleteConcurrencyControlException(
                string.Format(
                    "A Error has occured since the object you are trying to refresh has been deleted by another user."
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
            if (_dataStore.AllObjects.ContainsKey(primaryKey))
                return _dataStore.AllObjects[primaryKey];

            throw new BusObjDeleteConcurrencyControlException(
                string.Format(
                    "A Error has occured since the object you are trying to refresh has been deleted by another user."
                    + " There are no records in the database for the Class: {0} identified by {1} \n", classDef.ClassNameFull,
                    primaryKey));
        }

        /// <summary>
        /// Loads a business object of type T, using the criteria given
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            return _dataStore.Find<T>(criteria);
        }

        /// <summary>
        /// Loads a business object of the type identified by a <see cref="ClassDef"/>, using the criteria given
        /// </summary>
        /// <param name="classDef">The ClassDef of the object to load.</param>
        /// <param name="criteria">The criteria to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            return _dataStore.Find(classDef.ClassType, criteria);
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
            return _dataStore.Find<T>(selectQuery.Criteria);
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
            return _dataStore.Find(classDef.ClassType, selectQuery.Criteria);
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
            return _dataStore.Find<T>(criteriaObject);
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
            return _dataStore.Find(classDef.ClassType, criteriaObject);
        }

        /// <summary>
        /// Reloads a businessObject from the datasource using the id of the object.
        /// A dirty object will not be refreshed from the database and the appropriate error will be raised.
        /// Cancel all edits before refreshing the object or call see TODO: Refresh with refresh dirty objects = true.
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
                throw new HabaneroDeveloperException("A Error has occured since the object being refreshed is being edited.",
                    "A Error has occured since the object being refreshed is being edited. ID :- " +
                    businessObject.ID.GetObjectId() + " Class : " + businessObject.ClassDef.ClassNameFull);
            }
            return businessObject;
        }

        #endregion //GetBusinessObject Members

        #region GetBusinessObjectCollection Members

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="collection">The collection to refresh</param>
        public override void Refresh<T>(BusinessObjectCollection<T> collection) 
            //where T : class, IBusinessObject, new()
        {
            if (typeof(T) == typeof(BusinessObject))
            {
                Refresh((IBusinessObjectCollection)collection);
                return;
            }
            ISelectQuery selectQuery = collection.SelectQuery;
            Criteria criteria = selectQuery.Criteria;
            OrderCriteria orderCriteria = selectQuery.OrderCriteria;

            QueryBuilder.PrepareCriteria(collection.ClassDef, criteria);

            BusinessObjectCollection<T> loadedBos = _dataStore.FindAll<T>(criteria);
            loadedBos.Sort(delegate(T x, T y) { return orderCriteria.Compare(x, y); });
            if (selectQuery.Limit >= 0)
            {
                while (loadedBos.Count > selectQuery.Limit)
                {
                    loadedBos.RemoveAt(selectQuery.Limit);
                }
            }
            LoadBOCollection(collection, loadedBos);
        }

        private static void LoadBOCollection(IBusinessObjectCollection collection, IBusinessObjectCollection loadedBos)
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
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public override void Refresh(IBusinessObjectCollection collection)
        {
            ISelectQuery selectQuery = collection.SelectQuery;
            Criteria criteria = selectQuery.Criteria;
            OrderCriteria orderCriteria = selectQuery.OrderCriteria;

            IClassDef classDef = collection.ClassDef;
            QueryBuilder.PrepareCriteria(classDef, criteria);

            IBusinessObjectCollection loadedBos = _dataStore.FindAll(classDef.ClassType, criteria);
            loadedBos.Sort(orderCriteria);
            if (selectQuery.Limit >= 0)
            {
                while (loadedBos.Count > selectQuery.Limit)
                {
                    loadedBos.RemoveAt(selectQuery.Limit);
                }
            }
            LoadBOCollection(collection, loadedBos);
        }



        #endregion //GetBusinessObjectCollection Members

        #region GetRelatedBusinessObjectCollection Members

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
        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship)
            where T : class, IBusinessObject, new()
        {
            RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            OrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            BusinessObjectCollection<T> col = GetBusinessObjectCollection<T>(relationshipCriteria, preparedOrderCriteria);
//            col.ForEach(delegate(T obj) { relatedCol.Add(obj); });
            LoadBOCollection(relatedCol, col);
            relatedCol.SelectQuery = col.SelectQuery;
            return relatedCol;
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
        public IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IRelationship relationship)
        {
            IBusinessObjectCollection relatedCol = CreateRelatedBusinessObjectCollection(type, relationship);
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            OrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef,
                        relationshipCriteria, preparedOrderCriteria);
            LoadBOCollection(relatedCol, col);
            relatedCol.SelectQuery = col.SelectQuery;
            return relatedCol;
        }

        ///<summary>
        /// Creates a RelatedBusinessObjectCollection.
        ///</summary>
        /// <param name="boType">The type of BO to make a generic collection of</param>
        /// <param name="relationship">The multiple relationship this collection is for</param>
        ///<returns> A BusinessObjectCollection of the correct type. </returns>
        private static IBusinessObjectCollection CreateRelatedBusinessObjectCollection(Type boType,
                                                                                IRelationship relationship)
        {
            Type type = typeof(RelatedBusinessObjectCollection<>);
            type = type.MakeGenericType(boType);
            return (IBusinessObjectCollection)Activator.CreateInstance(type, relationship);
        }

        /// <summary>
        /// Loads a business object of type T using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <typeparam name="T">The type of the business object to load</typeparam>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of type T if one was found, otherwise null</returns>
        public T GetRelatedBusinessObject<T>(SingleRelationship relationship) where T : class, IBusinessObject, new()
        {
            return GetBusinessObject<T>(Criteria.FromRelationship(relationship));
        }

        /// <summary>
        /// Loads a business object using the relationship given. The relationship will be converted into a
        /// Criteria object that defines the relationship and this will be used to load the related object.
        /// </summary>
        /// <param name="relationship">The relationship to use to load the object</param>
        /// <returns>An object of the type defined by the relationship if one was found, otherwise null</returns>
        public IBusinessObject GetRelatedBusinessObject(SingleRelationship relationship)
        {
            if (relationship.RelationshipDef.RelatedObjectClassDef != null)
                return GetBusinessObject(relationship.RelationshipDef.RelatedObjectClassDef,
                                         Criteria.FromRelationship(relationship));
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