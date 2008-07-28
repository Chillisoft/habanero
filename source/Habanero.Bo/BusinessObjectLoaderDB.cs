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
using Habanero.BO.ObjectManager;

namespace Habanero.BO
{
    ///<summary>
    ///For details of what this class does, see <see cref="IBusinessObjectLoader"/>.
    ///
    /// All queries (including custom SelectQuery objects) run by this loader will be done using parametrized sql for 
    /// improved type safety and performance.
    /// 
    ///</summary>
    public class BusinessObjectLoaderDB : IBusinessObjectLoader
    {
        //private readonly DataStoreInMemory _dataStoreInMemory;
        private readonly IDatabaseConnection _databaseConnection;

        ///<summary>Creates a BusinessObjectLoaderDB. Because this is a loader the loads data from a Database, this constructor
        /// requires an IDatabaseConnection object to be passed to it.  This connection will be used for all loading.
        ///</summary>
        ///<param name="databaseConnection"></param>
        public BusinessObjectLoaderDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            //_dataStoreInMemory = new DataStoreInMemory();
        }

        /// <summary>
        /// Loads a business object of type T, using the Primary key given as the criteria
        /// </summary>
        /// <typeparam name="T">The type of object to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="primaryKey">The primary key to use to load the business object</param>
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found, the first is returned</returns>
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
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found, the first is returned</returns>
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
            ISelectQuery selectQuery = QueryBuilder.CreateSelectQuery(ClassDef.ClassDefs[typeof (T)]);
            selectQuery.Criteria = criteria;
            return GetBusinessObject<T>(selectQuery);
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
                }
            }
            // loads an object of the correct sub type (for single table inheritance)
            if (correctSubClassDef != null)
            {
                BusinessObject.AllLoadedBusinessObjects().Remove(loadedBo.ID.GetObjectId());
                BusObjectManager.Instance.Remove(loadedBo.ID);
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = (T) subClassBusinessObject;
            }

            return loadedBo;
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
        /// <returns>The business object that was found. If none was found, null is returned. If more than one is found, the first is returned</returns>
        public IBusinessObject GetBusinessObject(IClassDef classDef, ISelectQuery selectQuery)
        {
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
                    if (correctSubClassDef == null) return loadedBo;
                }
            }
            if (correctSubClassDef != null)
            {
                BusinessObject.AllLoadedBusinessObjects().Remove(loadedBo.ID.GetObjectId());
                BusObjectManager.Instance.Remove(loadedBo.ID);
                IBusinessObject subClassBusinessObject = GetBusinessObject(correctSubClassDef, loadedBo.ID);
                loadedBo = subClassBusinessObject;
            }
            return loadedBo;
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
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T>();
            col.SelectQuery.Criteria = criteria;
            Refresh(col);
            return col;
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
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            BusinessObjectCollection<T> clonedCol = collection.Clone();
            collection.Clear();

            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    T loadedBo = (T) LoadBOFromReader(collection.ClassDef, dr, selectQuery);
                    //If the origional collection had the new business object then
                    // use add internal this adds without any events being raised etc.
                    //else adds via the Add method (normal add) this raises events such that the 
                    // user interface can be updated.
                    if (clonedCol.Contains(loadedBo))
                    {
                        collection.AddInternal(loadedBo);
                    }
                    else
                    {
                        collection.Add(loadedBo);
                    }
                }
            }
        }

        /// <summary>
        /// Reloads a BusinessObjectCollection using the criteria it was originally loaded with.  You can also change the criteria or order
        /// it loads with by editing its SelectQuery object. The collection will be cleared as such and reloaded (although Added events will
        /// only fire for the new objects added to the collection, not for the ones that already existed).
        /// </summary>
        /// <param name="collection">The collection to refresh</param>
        public void Refresh(IBusinessObjectCollection collection)
        {
            SelectQueryDB selectQuery = new SelectQueryDB(collection.SelectQuery);
            ISqlStatement statement = selectQuery.CreateSqlStatement();
            IBusinessObjectCollection updatedCol = collection.Clone();
            updatedCol.SelectQuery = collection.SelectQuery;
            updatedCol.Clear();
            using (IDataReader dr = _databaseConnection.LoadDataReader(statement))
            {
                while (dr.Read())
                {
                    IBusinessObject loadedBo = LoadBOFromReader(collection.ClassDef, dr, selectQuery);
                    updatedCol.Add(loadedBo);
                }
            }
            foreach (IBusinessObject obj in collection)
            {
                if (!updatedCol.Contains(obj)) collection.Remove(obj);
            }
            foreach (IBusinessObject obj in updatedCol)
            {
                if (!collection.Contains(obj)) collection.Add(obj);
            }
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
        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IRelationship relationship)
            where T : class, IBusinessObject, new()
        {
            RelatedBusinessObjectCollection<T> relatedCol = new RelatedBusinessObjectCollection<T>(relationship);
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);
            GetBusinessObjectCollection<T>(relationshipCriteria, relationship.OrderCriteria).ForEach(
                delegate(T obj) { relatedCol.Add(obj); });
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = relationship.OrderCriteria;
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

            IBusinessObjectCollection col = GetBusinessObjectCollection(relationship.RelatedObjectClassDef, 
                        relationshipCriteria, relationship.OrderCriteria);
            foreach (IBusinessObject businessObject in col)
            {
                relatedCol.Add(businessObject);
            }
            relatedCol.SelectQuery.Criteria = relationshipCriteria;
            relatedCol.SelectQuery.OrderCriteria = relationship.OrderCriteria;
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
            return GetBusinessObject(relationship.RelationshipDef.RelatedObjectClassDef,
                                     Criteria.FromRelationship(relationship));
        }

        /// <summary>
        /// Loads a BusinessObjectCollection using the criteria given, applying the order criteria to order the collection that is returned. 
        /// </summary>
        /// <typeparam name="T">The type of collection to load. This must be a class that implements IBusinessObject and has a parameterless constructor</typeparam>
        /// <param name="criteria">The criteria to use to load the business object collection</param>
        /// <returns>The loaded collection</returns>
        /// <param name="orderCriteria">The order criteria to use (ie what fields to order the collection on)</param>
        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, OrderCriteria orderCriteria)
            where T : class, IBusinessObject, new()
        {
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
        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria,
                                                                     OrderCriteria orderCriteria)
        {
            IBusinessObjectCollection col = CreateCollectionOfType(classDef.ClassType);
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
            col.SelectQuery = selectQuery;
            Refresh(col);
            return col;
        }

        private static IBusinessObjectCollection CreateCollectionOfType(Type BOType)
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(BOType);
            return (IBusinessObjectCollection) Activator.CreateInstance(boColType);
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

            if (boFromObjectManager != null) return boFromObjectManager;

            BusObjectManager.Instance.Add(bo);
            ((BusinessObject)bo).AfterLoad();
            return bo;
        }

        private static IBusinessObject GetObjectFromObjectManager(IPrimaryKey key)
        {
            BusObjectManager busObjectManager = BusObjectManager.Instance;
            return busObjectManager.Contains(key) ? busObjectManager[key] : null;           
        }

        private static ClassDef GetCorrectSubClassDef(IBusinessObject bo, IDataRecord dataReader)
        {
            ClassDef classDef = (ClassDef) bo.ClassDef;
            ClassDefCol subClasses = classDef.ImmediateChildren;
            foreach (ClassDef immediateChild in subClasses)
            {
                if (!immediateChild.IsUsingSingleTableInheritance()) continue;

                string discriminatorFieldName = immediateChild.SuperClassDef.Discriminator;
                string discriminatorValue = Convert.ToString(dataReader[discriminatorFieldName]);

                if (String.IsNullOrEmpty(discriminatorValue)) continue;

                ClassDef subClassDef = ClassDef.ClassDefs.FindByClassName(discriminatorValue);

                if (subClassDef != null) return subClassDef;
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
                    bo.Props[field.PropertyName].InitialiseProp(dr[i]);
                }
                i++;
            }
            ((BusinessObject) bo).SetState(BOState.States.isNew, false);
        }
    }
}