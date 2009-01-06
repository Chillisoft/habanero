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
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
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
    public class BusinessObjectLoaderDB : BusinessObjectLoaderBase, IBusinessObjectLoader
    {
        private readonly IDatabaseConnection _databaseConnection;

        ///<summary>
        /// Creates a BusinessObjectLoaderDB. Because this is a loader the loads data from a Database, this constructor
        /// requires an IDatabaseConnection object to be passed to it.  This connection will be used for all loading.
        ///</summary>
        ///<param name="databaseConnection"></param>
        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Loads a business object of type T, using the Primary key given as the criteria
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found an <see cref="HabaneroDeveloperException"/> error is throw</returns>
        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            T businessObject = GetBusinessObject<T>(Criteria.FromPrimaryKey(primaryKey));
            if (businessObject != null) return businessObject;

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
            IBusinessObject businessObject = GetBusinessObject(classDef, Criteria.FromPrimaryKey(primaryKey));
            if (businessObject != null) return businessObject;

            throw new BusObjDeleteConcurrencyControlException(
                string.Format(
                    "A Error has occured since the object you are trying to refresh has been deleted by another user."
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
            ClassDef classDef = ClassDef.ClassDefs[typeof (T)];
            ISelectQuery selectQuery = GetSelectQuery(classDef, criteria);
            return GetBusinessObject<T>(selectQuery);
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

        ///<summary>
        ///Create a select Query based on the class definition and the search criteria.
        ///</summary>
        ///<param name="classDef">The class definition.</param>
        ///<param name="criteria">The load criteria.</param>
        ///<returns></returns>
        private static ISelectQuery GetSelectQuery(IClassDef classDef, Criteria criteria)
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
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            T loadedBo = null;
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                if (dr.Read())
                {
                    loadedBo = LoadBOFromReader<T>(dr, selectQueryDB);

                    //Checks to see if the loaded object is the base of a single table inheritance structure
                    // and has a sub type
                    correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
                    //Checks to see if there is a duplicate object meeting this criteria
                    if (dr.Read()) ThrowRetrieveDuplicateObjectException(statement, loadedBo);
                    if (correctSubClassDef == null) return loadedBo;
                }
            }
            loadedBo = GetLoadedBoOfSpecifiedType(loadedBo, correctSubClassDef);
            return loadedBo;
        }

        private static void ThrowRetrieveDuplicateObjectException(ISqlStatement statement, IBusinessObject loadedBo)
        {
            throw new HabaneroDeveloperException("There was an error with loading the class '"
                                                 + loadedBo.ClassDef.ClassNameFull + "'", "Loading a '"
                                                                                          + loadedBo.ClassDef.ClassNameFull + "' with criteria '" + statement.Statement 
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
            SelectQueryDB selectQueryDB = new SelectQueryDB(selectQuery);
            ISqlStatement statement = selectQueryDB.CreateSqlStatement();
            IClassDef correctSubClassDef = null;
            IBusinessObject loadedBo = null;
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                if (dr.Read())
                {
                    loadedBo = LoadBOFromReader(classDef, dr, selectQueryDB);
                    correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
                    
                    if (dr.Read())
                    {
                        ThrowRetrieveDuplicateObjectException(statement, loadedBo);
                    }
                    if (correctSubClassDef == null) return loadedBo;
                }
            }
            if (correctSubClassDef != null)
            {
                BusinessObjectManager.Instance.Remove(loadedBo.ID);
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = subClassBusinessObject;
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
            Criteria criteria = GetCriteriaObject(ClassDef.ClassDefs[typeof(T)], criteriaString);
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

        #region GetBusinessObjectCollection

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="collection">The collection to refresh</param>
        protected override void DoRefresh<T>(BusinessObjectCollection<T> collection) 
        {
            IClassDef classDef = collection.ClassDef;
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            QueryBuilder.PrepareCriteria(classDef, selectQuery.Criteria);

            ISqlStatement statement = selectQuery.CreateSqlStatement();
            ReflectionUtilities.ExecutePrivateMethod(collection, "ClearCurrentCollection");

            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    T loadedBo = (T) LoadBOFromReader(collection.ClassDef, dr, selectQuery);
                    //If the origional collection had the new business object then
                    // use add internal this adds without any events being raised etc.
                    //else adds via the Add method (normal add) this raises events such that the 
                    // user interface can be updated.
                    //Checks to see if the loaded object is the base of a single table inheritance structure
                    // and has a sub type
                    IClassDef correctSubClassDef = GetCorrectSubClassDef(loadedBo, dr);
                    // loads an object of the correct sub type (for single table inheritance)
                    loadedBo = GetLoadedBoOfSpecifiedType(loadedBo, correctSubClassDef);
                    AddBusinessObjectToCollection(collection, loadedBo);
                }
            }

            //The collection should show all loaded object less removed or deleted object not yet persisted
            //     plus all created or added objects not yet persisted.
            //Note_: This behaviour is fundamentally different than the business objects behaviour which 
            //  throws and error if any of the items are dirty when it is being refreshed.
            //Should a refresh be allowed on a dirty collection (what do we do with BO's
            //TODO: I think this could be done via reflection instead of having all these public methods.
            //   especially where done via the interface.
            //  the other option would be for the business object collection to have another method (other than clone)
            //   that returns another type of object that has these methods to eliminate all these 
            //   public accessors
            RestoreEditedLists(collection);
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        protected override void DoRefresh(IBusinessObjectCollection collection)
        {
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            QueryBuilder.PrepareCriteria(collection.ClassDef, selectQuery.Criteria);
            ISqlStatement statement = selectQuery.CreateSqlStatement();

            ReflectionUtilities.ExecutePrivateMethod(collection, "ClearCurrentCollection");


            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    IBusinessObject loadedBo = LoadBOFromReader(collection.ClassDef, dr, selectQuery);
                    AddBusinessObjectToCollection(collection, loadedBo);
                }
            }
            RestoreEditedLists(collection);
        }

        /// <summary>
        /// loads an object of the correct sub type (for single table inheritance)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadedBo"></param>
        /// <param name="correctSubClassDef"></param>
        /// <returns></returns>
        private T GetLoadedBoOfSpecifiedType<T>(T loadedBo, IClassDef correctSubClassDef)
            where T : class, IBusinessObject, new()
        {
            if (correctSubClassDef != null)
            {
                BusinessObjectManager.Instance.Remove(loadedBo.ID);
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = (T)subClassBusinessObject;
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
                throw new HabaneroDeveloperException("A Error has occured since the object being refreshed is being edited.",
                                                     "A Error has occured since the object being refreshed is being edited. ID :- " +
                                                     businessObject.ID.GetObjectId() + " - Class : " + businessObject.ClassDef.ClassNameFull);
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
                return GetBusinessObject(relationshipDef.RelatedObjectClassDef,
                                         Criteria.FromRelationship(relationship));
            return null;
        }

        #endregion


        protected new static IBusinessObjectCollection CreateCollectionOfType(Type BOType)
        {
            Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(BOType);
            return (IBusinessObjectCollection)Activator.CreateInstance(boColType);
        }

        private static T LoadBOFromReader<T>(IDataRecord dataReader, ISelectQuery selectQuery)
            where T : class, IBusinessObject, new()
        {
            T bo = new T();

            return (T) GetLoadedBusinessObject(bo, dataReader, selectQuery);
        }

        private static IBusinessObject LoadBOFromReader(IClassDef classDef, IDataRecord dataReader, ISelectQuery selectQuery)
        {
            IBusinessObject bo = classDef.CreateNewBusinessObject();

            return GetLoadedBusinessObject(bo, dataReader, selectQuery);
        }

        private static IBusinessObject GetLoadedBusinessObject(IBusinessObject bo, IDataRecord dataReader, ISelectQuery selectQuery)
        {
            PopulateBOFromReader(bo, dataReader, selectQuery);
            IPrimaryKey key = bo.ID;

            IBusinessObject boFromObjectManager = GetObjectFromObjectManager(key);

            if (boFromObjectManager == null)
            {
                BusinessObjectManager.Instance.Add(bo);
                return bo;
            }

            if (boFromObjectManager.Status.IsEditing) return boFromObjectManager;

            PopulateBOFromReader(boFromObjectManager, dataReader, selectQuery);
            return boFromObjectManager;
        }

        private static IBusinessObject GetObjectFromObjectManager(IPrimaryKey key)
        {
            BusinessObjectManager businessObjectManager = BusinessObjectManager.Instance;
            return businessObjectManager.Contains(key) ? businessObjectManager[key] : null;           
        }

        private static ClassDef GetCorrectSubClassDef(IBusinessObject bo, IDataRecord dataReader)
        {
            ClassDef classDef = (ClassDef) bo.ClassDef;
            ClassDefCol subClasses = classDef.AllChildren;
            foreach (ClassDef immediateChild in subClasses)
            {
                if (!immediateChild.IsUsingSingleTableInheritance()) continue;

                string discriminatorFieldName = immediateChild.SuperClassDef.Discriminator;
                string discriminatorValue = Convert.ToString(dataReader[discriminatorFieldName]);

                if (String.IsNullOrEmpty(discriminatorValue)) continue;

                ClassDef subClassDef = ClassDef.ClassDefs.FindByClassName(discriminatorValue);

                if (subClassDef != null && subClassDef != bo.ClassDef) return subClassDef;
            }
            return null;
        }

        private static void PopulateBOFromReader(IBusinessObject bo, IDataRecord dr, ISelectQuery selectQuery)
        {
            int i = 0;
            foreach (QueryField field in selectQuery.Fields.Values)
            {
                if (bo.Props.Contains(field.PropertyName))
                {
                    IBOProp boProp = bo.Props[field.PropertyName];
                    //TODO:                   if (!prop.PropDef.Persistable) continue; //BRETT/PETER TODO: to be changed
                    boProp.InitialiseProp(dr[i]);
                }
                i++;
            }
            BusinessObject businessObject = ((BusinessObject) bo);
            businessObject.SetStatus(BOStatus.Statuses.isNew, false);
            businessObject.AfterLoad();
        }
    }
}