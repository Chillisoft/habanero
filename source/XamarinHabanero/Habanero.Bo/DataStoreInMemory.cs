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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// This is an in-memory data store designed to be used primarily for testing.
    ///</summary>
    public class DataStoreInMemory
    {
        private ConcurrentDictionary<Guid, IBusinessObject> _objects = new ConcurrentDictionary<Guid, IBusinessObject>();
        private readonly ConcurrentDictionary<IClassDef, INumberGenerator> _autoIncrementNumberGenerators = new ConcurrentDictionary<IClassDef, INumberGenerator>();
        private readonly object _lock = new object();

        ///<summary>
        /// Returns the number of objects in the memory store.
        ///</summary>
        public int Count
        {
            get { return AllObjects.Count; }
        }

        ///<summary>
        /// Returns a Dictionary of all the objects in the memory store.
        ///</summary>
        public ConcurrentDictionary<Guid, IBusinessObject> AllObjects
        {
            get { return _objects; }
            set { _objects = value;  }
        }

        ///<summary>
        /// Adds a new business object to the memory store.
        ///</summary>
        ///<param name="businessObject"></param>
        public virtual void Add(IBusinessObject businessObject)
        {
            AllObjects.TryAdd(businessObject.ID.ObjectID, businessObject);
        }

        ///<summary>
        /// Finds the object of type T that matches the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public virtual T Find<T>(Criteria criteria) where T : class, IBusinessObject
        {
            return (T) Find(typeof (T), criteria);
        }

        ///<summary>
        /// Finds the object of type BOType that matches  and criteria.
        ///</summary>
        ///<param name="boType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public virtual IBusinessObject Find(Type boType, Criteria criteria)
        {
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (!criteria.IsMatch(bo)) continue;
                if (currentBO == null)
                {
                    currentBO = bo;
                }
                else
                {
                    throw new HabaneroDeveloperException("There was an error with loading the class '"
                              + bo.ClassDef.ClassNameFull + "'", "Loading a '"
                              + bo.ClassDef.ClassNameFull + "' with criteria '" + criteria
                              + "' returned more than one record when only one was expected.");
                }
            }
            return currentBO;
        }        
        
        ///<summary>
        /// Finds a single business object matching the criteria. Throws an Error if more than one BO matches.
        ///</summary>
        ///<param name="classDef">ClassDef to match on.</param>
        ///<param name="criteria">Criteria being used to find the BusinessObject</param>
        ///<returns></returns>
        ///<exception cref="HabaneroDeveloperException">Error if more than one BO matches criteria</exception>
        public virtual IBusinessObject Find(IClassDef classDef, Criteria criteria)
        {
            Type boType = classDef.ClassType;
            IBusinessObject currentBO = null;
            foreach (IBusinessObject bo in GetAllObjectsSnapshot())
            {
                if (bo.ClassDef != classDef && !boType.IsInstanceOfType(bo))
                {
                    continue;
                }
                if (!criteria.IsMatch(bo)) continue;
                if (currentBO == null)
                {
                    currentBO = bo;
                }
                else
                {
                    throw new HabaneroDeveloperException("There was an error with loading the class '"
                              + bo.ClassDef.ClassNameFull + "'", "Loading a '"
                              + bo.ClassDef.ClassNameFull + "' with criteria '" + criteria
                              + "' returned more than one record when only one was expected.");
                }
            }
            return currentBO;
        }

        private IEnumerable<IBusinessObject> GetAllObjectsSnapshot()
        {
            var values = AllObjects.Values;
            var valuesSnapshot = new IBusinessObject[values.Count];
            values.CopyTo(valuesSnapshot, 0);
            return valuesSnapshot;
        }

        ///<summary>
        /// Finds an object of type T that has the primary key primaryKey.
        ///</summary>
        ///<param name="primaryKey"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public virtual T Find<T>(IPrimaryKey primaryKey) where T : class, IBusinessObject
        {
            return (from bo in AllObjects.Values where bo.ID.Equals(primaryKey) select bo as T).FirstOrDefault();
        }

        ///<summary>
        /// Removes the object from the data store.
        ///</summary>
        ///<param name="businessObject"></param>
        public virtual void Remove(IBusinessObject businessObject)
        {
            IBusinessObject value;
            AllObjects.TryRemove(businessObject.ID.ObjectID, out value);
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public virtual BusinessObjectCollection<T> FindAll<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            BusinessObjectCollection<T> col = new BusinessObjectCollection<T> {FindAllInternal<T>(criteria)};
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        ///<summary>
        /// Find all objects that match the criteria.
        ///</summary>
        ///<param name="criteria"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        internal virtual List<T> FindAllInternal<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            var list = new List<T>();
            var boType = typeof (T);
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (criteria == null || criteria.IsMatch(bo)) list.Add((T)bo);
            }

            return list;
        }
        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        ///<param name="boType"></param>
        ///<param name="criteria"></param>
        ///<returns></returns>
        public virtual IBusinessObjectCollection FindAll(Type boType, Criteria criteria)
        {
            IBusinessObjectCollection col = CreateGenericCollection(boType);
            foreach (IBusinessObject bo in AllObjects.Values)
            {
                if (!boType.IsInstanceOfType(bo)) continue;
                if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
            }
            col.SelectQuery.Criteria = criteria;
            return col;

        }

        private static IBusinessObjectCollection CreateGenericCollection(Type boType)
        {
            Type boColType = typeof (BusinessObjectCollection<>).MakeGenericType(boType);
            return (IBusinessObjectCollection) Activator.CreateInstance(boColType);
        }

        ///<summary>
        /// Find all objects of type boType that match the criteria.
        ///</summary>
        /// <param name="classDef"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual IBusinessObjectCollection FindAll(IClassDef classDef, Criteria criteria)
        {
            Type boType = classDef.ClassType;
            IBusinessObjectCollection col = CreateGenericCollection(boType);
            col.ClassDef = classDef;
            foreach (IBusinessObject bo in GetAllObjectsSnapshot()) //AllObjects.Values.ToArray())
            {
                if (bo.ClassDef != classDef && !boType.IsInstanceOfType(bo)) continue;
                if (classDef.TypeParameter != bo.ClassDef.TypeParameter) continue;
                if (criteria == null || criteria.IsMatch(bo)) col.Add(bo);
            }
            col.SelectQuery.Criteria = criteria;
            return col;
        }

        private IEnumerable<IBusinessObject> FindObjectsMatchingType(IClassDef classDef)
        {
            var boType = classDef.ClassType;
            var objectsMatchingType = AllObjects.Values.Where(bo =>
                                                              !(bo.ClassDef != classDef && !boType.IsInstanceOfType(bo))
                                                              && classDef.TypeParameter == bo.ClassDef.TypeParameter);
            return objectsMatchingType;
        }

        /// <summary>
        /// Clears all the objects in the memory datastore
        /// </summary>
        public void ClearAllBusinessObjects()
        {
            AllObjects.Clear();
        }
      
        ///<summary>
        /// The <see cref="INumberGenerator"/>s used to produce the AutoIncremented values for Classes in this DataStore.
        ///</summary>
        protected internal IDictionary<IClassDef, INumberGenerator> AutoIncrementNumberGenerators
        {
            get { return _autoIncrementNumberGenerators; }
        }
        
        ///<summary>
        /// Returns the next value for the AutoIncrement value for the specified <see cref="IClassDef"/>.
        /// If a number generator does not exist for the specified <see cref="IClassDef"/> then one is created.
        ///</summary>
        ///<param name="classDef">The ClassDef that the AutoIncremented value is being generated for.</param>
        ///<returns>The next AutoIncrement value for the ClassDef specified.</returns>
        public virtual long GetNextAutoIncrementingNumber(IClassDef classDef)
        {
            lock (_lock)
            {
                var numberGenerator = _autoIncrementNumberGenerators.GetOrAdd(classDef,
                    def => new NumberGenerator(string.Format("AutoIncrement({0})", classDef.ClassName)));
                return numberGenerator.NextNumber();
            }
        }
    }
}
