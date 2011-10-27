//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Habanero.Base;

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

        public T GetBusinessObject<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof (T)].GetBusinessObject<T>(primaryKey);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(primaryKey);
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, IPrimaryKey primaryKey)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, primaryKey);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, primaryKey);
        }

        public T GetBusinessObject<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(criteria);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(criteria);
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, criteria);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, criteria);
        }

        public T GetBusinessObject<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(selectQuery);
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, ISelectQuery selectQuery)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, selectQuery);
        }

        public T GetBusinessObject<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObject<T>(criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObject<T>(criteriaString);
        }

        public IBusinessObject GetBusinessObject(IClassDef classDef, string criteriaString)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObject(classDef, criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObject(classDef, criteriaString);
        }

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

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, criteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, searchCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, searchCriteria);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, IOrderCriteria orderCriteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria, orderCriteria);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, Criteria criteria, IOrderCriteria orderCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, criteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, criteria, orderCriteria);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteria) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString, orderCriteria);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, string searchCriteria, string orderCriteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, searchCriteria, orderCriteria);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, searchCriteria, orderCriteria);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(Criteria criteria, IOrderCriteria orderCriteria, int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef def, Criteria criteria, IOrderCriteria orderCriteria, int firstRecordToLoad, int numberOfRecordsToLoad, out int records)
        {
            if (_businessObjectLoaders.ContainsKey(def.ClassType))
                return _businessObjectLoaders[def.ClassType].GetBusinessObjectCollection(def, criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out records);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(def, criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out records);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(string criteriaString, string orderCriteriaString, int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(criteriaString, orderCriteriaString, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(criteriaString, orderCriteriaString, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }

        public BusinessObjectCollection<T> GetBusinessObjectCollection<T>(ISelectQuery selectQuery) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectCollection<T>(selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection<T>(selectQuery);
        }

        public IBusinessObjectCollection GetBusinessObjectCollection(IClassDef classDef, ISelectQuery selectQuery)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectCollection(classDef, selectQuery);
            return _defaultBusinessObjectLoader.GetBusinessObjectCollection(classDef, selectQuery);
        }

        public void Refresh<T>(BusinessObjectCollection<T> collection) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                _businessObjectLoaders[typeof(T)].Refresh(collection);
            else
                _defaultBusinessObjectLoader.Refresh(collection);
        }

        public void Refresh(IBusinessObjectCollection collection)
        {
            if (_businessObjectLoaders.ContainsKey(collection.ClassDef.ClassType))
                _businessObjectLoaders[collection.ClassDef.ClassType].Refresh(collection);
            else
                _defaultBusinessObjectLoader.Refresh(collection);
        }

        public IBusinessObject Refresh(IBusinessObject businessObject)
        {
            if (_businessObjectLoaders.ContainsKey(businessObject.GetType()))
                return _businessObjectLoaders[businessObject.GetType()].Refresh(businessObject);
            return _defaultBusinessObjectLoader.Refresh(businessObject);
        }

        public RelatedBusinessObjectCollection<T> GetRelatedBusinessObjectCollection<T>(IMultipleRelationship relationship) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetRelatedBusinessObjectCollection<T>(relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObjectCollection<T>(relationship);
        }

        public IBusinessObjectCollection GetRelatedBusinessObjectCollection(Type type, IMultipleRelationship relationship)
        {
            if (_businessObjectLoaders.ContainsKey(type))
                return _businessObjectLoaders[type].GetRelatedBusinessObjectCollection(type, relationship);
            return _defaultBusinessObjectLoader.GetRelatedBusinessObjectCollection(type, relationship);
        }

        public IBusinessObject GetBusinessObjectByValue(IClassDef classDef, object idValue)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetBusinessObjectByValue(classDef, idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue(classDef, idValue);
        }

        public IBusinessObject GetBusinessObjectByValue(Type type, object idValue)
        {
            if (_businessObjectLoaders.ContainsKey(type))
                return _businessObjectLoaders[type].GetBusinessObjectByValue(type, idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue(type, idValue);
        }

        public T GetBusinessObjectByValue<T>(object idValue) where T : class, IBusinessObject, new()
        {
            if (_businessObjectLoaders.ContainsKey(typeof(T)))
                return _businessObjectLoaders[typeof(T)].GetBusinessObjectByValue<T>(idValue);
            return _defaultBusinessObjectLoader.GetBusinessObjectByValue<T>(idValue);
        }

        public int GetCount(IClassDef classDef, Criteria criteria)
        {
            if (_businessObjectLoaders.ContainsKey(classDef.ClassType))
                return _businessObjectLoaders[classDef.ClassType].GetCount(classDef, criteria);
            return _defaultBusinessObjectLoader.GetCount(classDef, criteria);
        }

        public ResultSet GetResultSet(ISelectQuery selectQuery)
        {
            throw new NotImplementedException();
        }
    }
}