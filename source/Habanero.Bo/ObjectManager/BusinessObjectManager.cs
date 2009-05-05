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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// The business object manager is a class that contains weak references
    /// to all currently loaded business objects. The object manager is therefore used to ensure that the current user/session only
    /// ever has one reference to a particular business object. This is used to prevent situations where a business object loaded in
    /// two different ways by a single user is represented by two different instance objects in the system. Not having an object manager 
    /// could result in concurrency control exceptions even when only one user has.
    /// Whenever an object is requested to be loaded the Business Object loader first checks to see if the object already exists in the
    ///  object manager if it does then the object from the object manager is returned else the newly loaded object is added to the
    ///  object manager and then returned. NB: There are various strategies that the business object can implement to control the
    ///  behaviour when the business object loader <see cref="BusinessObjectLoaderDB"/> gets a business object that is already in the object
    ///  manager. The default behaviour is to refresh all the objects data if the object is not in edit mode. If the object is in edit mode 
    ///  the default behaviour is to do nothing. This strategy helps to prevent Inconsistant read and Inconsistant write concurrency control
    ///  errors.
    /// When an object is deleted from the datasource or disposed of by the garbage collecter it is removed from the object manager.
    /// When a new object is persisted for the first time it is updated to the object manager.
    /// 
    /// The BusinessObjectManager is an implementation of the Identity Map Pattern 
    /// (See 216 Fowler 'Patters Of Enterprise Application Architecture' - 'Ensures that each object loaded only once by keeping every loaded
    ///   object in a map. Looks up objects using the map when referring to them.'
    /// 
    /// This class should not be used by the end user of the Habanero framework except in tests where the user is writing tests 
    /// where the application developer is testing for concurrency issues in which case the ClearLoadedObjects can be called.
    /// 
    /// Only one Business object manager will be loaded per user session. To implement this the Singleton Pattern from the GOF is used.
    ///</summary>
    public class BusinessObjectManager
    {
        /// <summary>
        /// The Single Instance of the <see cref="BusinessObjectManager"/> used by the Singleton.
        /// </summary>
        protected static BusinessObjectManager _businessObjectManager = new BusinessObjectManager();

        //protected readonly Dictionary<string, WeakReference> _loadedBusinessObjects =
        //    new Dictionary<string, WeakReference>();
        /// <summary> The Busienss Objects Loaded in memory. This is a <see cref="WeakReference"/> so that the objects can still be garbage collected. </summary>
        protected readonly Dictionary<Guid, WeakReference> _loadedBusinessObjects =
            new Dictionary<Guid, WeakReference>();

        private readonly EventHandler<BOEventArgs> _updateIDEventHandler;
        /// <summary>
        /// The constructor for the Busienss Object Manager.
        /// </summary>
        protected internal BusinessObjectManager()
        {
            _updateIDEventHandler = ObjectID_Updated_Handler;
  
        }

        ///<summary>
        /// Returns the particular instance of the Business Object manager being used. 
        /// This implements the Singleton Design pattern.
        ///</summary>
        public static BusinessObjectManager Instance
        {
            get { return _businessObjectManager; }
            internal set { _businessObjectManager = value; }
        }

        #region IBusObjectManager Members

        /// <summary>
        /// Gets the number of business objects currently loaded. This is used
        /// primarily for debugging and testing.
        /// </summary>
        public int Count
        {
            get { return _loadedBusinessObjects.Count; }
        }

        /// <summary>
        /// Add a business object to the object manager.
        /// </summary>
        /// <param name="businessObject"></param>
        public void Add(IBusinessObject businessObject)
        {
            lock (_loadedBusinessObjects)
            {
                //if (_loadedBusinessObjects.ContainsKey(businessObject.ID.AsString_CurrentValue()))
                if (_loadedBusinessObjects.ContainsKey(businessObject.ID.ObjectID))
                {
                    //IBusinessObject loadedBusinessObject = this[businessObject.ID];
                    IBusinessObject loadedBusinessObject = this[businessObject.ID.ObjectID];
                    if (ReferenceEquals(loadedBusinessObject, businessObject)) return;

                    string developerMessage = string.Format
                        ("Two copies of the business object '{0}' identified by '{1}' "
                         + "were added to the object manager", businessObject.ClassDef.ClassNameFull,
                         businessObject.ID.ObjectID);
                    string userMessage = "There was a serious developer exception. " + Environment.NewLine
                                         + developerMessage;
                    throw new HabaneroDeveloperException(userMessage, developerMessage);
                }

                //_loadedBusinessObjects.Add(businessObject.ID.AsString_CurrentValue(), new WeakReference(businessObject));
                _loadedBusinessObjects.Add(businessObject.ID.ObjectID, new WeakReference(businessObject));
            }
            businessObject.IDUpdated += _updateIDEventHandler; //Register for ID Updated event this event is fired
            // if any of the properties that make up the primary key are changed/Updated this event is fires.
        }

        /// <summary>
        /// If the businessObject's IDUpdated Event has been fired then the business object is removed with the
        ///   old ID and added with the new ID. This is only required in cases with mutable primary keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ObjectID_Updated_Handler(object sender, BOEventArgs e)
        {
            if (!e.BusinessObject.ID.IsGuidObjectID) return;
            lock (_loadedBusinessObjects)
            {
                this.Remove(e.BusinessObject);
                this.Add(e.BusinessObject);
            }
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="businessObject">The business object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        public bool Contains(IBusinessObject businessObject)
        {
            //if (Contains(businessObject.ID))
            //{
            //    //if business object references are the same return true
            //    if (ReferenceEquals(businessObject, this[businessObject.ID])) return true;
            //}
            ////if contains by previous value and refrences are equal return true  --- this._primaryKey.AsString_PreviousValue()
            //string objectID = businessObject.ID.AsString_PreviousValue();
            //if (Contains(objectID))
            //{
            //    if (ReferenceEquals(businessObject, this[objectID])) return true;
            //}
            ////if contans by last persisted value and references are equal return true. --- this._primaryKey.AsString_LastPersistedValue()
            //objectID = businessObject.ID.AsString_LastPersistedValue();
            //if (Contains(objectID))
            //{
            //    if (ReferenceEquals(businessObject, this[objectID])) return true;
            //}
            //return false;

            Guid objectID = businessObject.ID.ObjectID;
            if (Contains(objectID))
            {
                //if business object references are the same return true
                if (ReferenceEquals(businessObject, this[objectID])) return true;
            }
            Guid previousObjectID = ((BOPrimaryKey) businessObject.ID).PreviousObjectID;
            if (Contains(previousObjectID))
            {
                if (ReferenceEquals(businessObject, this[previousObjectID])) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="id"> The business object id being checked (bo.Id).</param>
        /// <returns> Whether the busienss object is loadd or not</returns>
        public bool Contains(IPrimaryKey id)
        {
            return Contains(id.ObjectID);
        }

        //TODO Brett/Sori 10/02/2009 : this method should be removed

        ///<summary>
        /// Checks whether the business object is in the <see cref="BusinessObjectManager"/>.
        ///</summary>
        ///<param name="objectID">The <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/>.<see cref="IPrimaryKey.ObjectID"/> value.</param>
        ///<returns>Whether the business object is in the <see cref="BusinessObjectManager"/> or not.</returns>
        public bool Contains(Guid objectID)
        {
            //foreach (KeyValuePair<string, WeakReference> pair in _loadedBusinessObjects)
            //{
            //    WeakReference weakReference = pair.Value;
            //    if (!weakReference.IsAlive) continue;
            //    IBusinessObject businessObject = (IBusinessObject) weakReference.Target;
            //    if (businessObject.ID.ObjectID == objectID)
            //    {
            //        return true;
            //    }
            //}
            //return false;

            lock (_loadedBusinessObjects)
            {
                bool containsKey = _loadedBusinessObjects.ContainsKey(objectID);
                if (containsKey)
                {
                    if (!BusinessObjectWeakReferenceIsAlive(objectID))
                    {
                        _loadedBusinessObjects.Remove(objectID);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        //private bool BusinessObjectWeakReferenceIsAlive(string objectID)
        //{
        //    WeakReference boWeakRef = _loadedBusinessObjects[objectID];
        //    return WeakReferenceIsAlive(boWeakRef);
        //}

        private bool BusinessObjectWeakReferenceIsAlive(Guid objectID)
        {
            WeakReference boWeakRef = _loadedBusinessObjects[objectID];
            return WeakReferenceIsAlive(boWeakRef);
        }

        private static bool WeakReferenceIsAlive(WeakReference boWeakRef)
        {
            return (boWeakRef != null && boWeakRef.IsAlive && boWeakRef.Target != null);
        }

        /// <summary>
        /// Removes the business object Business object manager. If a 
        /// seperate instance of the business object is loaded in the object manager then it will not be removed.
        /// </summary>
        /// <param name="businessObject">business object to be removed.</param>
        public void Remove(IBusinessObject businessObject)
        {
            lock (_loadedBusinessObjects)
            {
                if (!Contains(businessObject)) return;

                //string objectID = businessObject.ID.AsString_CurrentValue();
                Guid objectID = businessObject.ID.ObjectID;
                if (Contains(objectID))
                {
                    Remove(objectID, businessObject);
                }
                objectID = ((BOPrimaryKey) businessObject.ID).PreviousObjectID;
                if (Contains(objectID))
                {
                    Remove(objectID, businessObject);
                }
                //objectID = businessObject.ID.AsString_LastPersistedValue();
                //if (Contains(objectID))
                //{
                //    Remove(objectID, businessObject);
                //}
            }
        }

        ///// <summary>
        ///// Removes the business object Business object manager. NB: if a seperate instance of the object 
        ///// is loaded in the object manager it will be removed. When possible user <see cref="Remove(IBusinessObject)"/>
        ///// </summary>
        ///// <param name="objectID">The string ID of the business object to be removed.</param>
        ///// <param name="businessObject">The business object being removed at this position.</param>
        //protected void Remove(string objectID, IBusinessObject businessObject)
        //{
        //    lock (_loadedBusinessObjects)
        //    {
        //        if (Contains(objectID) && ReferenceEquals(businessObject, this[objectID]))
        //        {
        //            _loadedBusinessObjects.Remove(objectID);

        //            DeregisterForIDUpdatedEvent(businessObject);
        //        }
        //    }
        //}

        /// <summary>
        /// Removes the business object Business object manager. NB: if a seperate instance of the object 
        /// is loaded in the object manager it will be removed. When possible user <see cref="Remove(IBusinessObject)"/>
        /// </summary>
        /// <param name="objectID">The string ID of the business object to be removed.</param>
        /// <param name="businessObject">The business object being removed at this position.</param>
        protected void Remove(Guid objectID, IBusinessObject businessObject)
        {
            lock (_loadedBusinessObjects)
            {
                if (Contains(objectID) && ReferenceEquals(businessObject, this[objectID]))
                {
                    _loadedBusinessObjects.Remove(objectID);

                    DeregisterForIDUpdatedEvent(businessObject);
                }
            }
        }
        /// <summary>
        /// Dereigisters for the IDValueUpdatedEvent.
        /// </summary>
        /// <param name="businessObject"></param>
        protected void DeregisterForIDUpdatedEvent(IBusinessObject businessObject)
        {
            businessObject.IDUpdated -= _updateIDEventHandler;
        }

        ///// <summary>
        ///// Returns the business object identified by the objectID from the business object manager.
        ///// </summary>
        ///// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        ///// <returns>The business object from the object manger.</returns>
        //internal IBusinessObject this[string objectID]
        //{
        //    get
        //    {
        //        lock (_loadedBusinessObjects)
        //        {
        //            if (Contains(objectID)) return (IBusinessObject)_loadedBusinessObjects[objectID].Target;
        //        }
        //        string message = "There was an attempt to retrieve the object identified by '" + objectID
        //                         + "' from the object manager but it is not currently loaded.";
        //        throw new HabaneroDeveloperException
        //            ("There is an application error please contact your system administrator." + Environment.NewLine
        //             + message, message);
        //    }
        //}

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        /// <returns>The business object from the object manger.</returns>
        public IBusinessObject this[Guid objectID]
        {
            get
            {
                lock (_loadedBusinessObjects)
                {
                    if (Contains(objectID)) return (IBusinessObject) _loadedBusinessObjects[objectID].Target;
                }
                string message = "There was an attempt to retrieve the object identified by '" + objectID
                                 + "' from the object manager but it is not currently loaded.";
                throw new HabaneroDeveloperException
                    ("There is an application error please contact your system administrator." + Environment.NewLine
                     + message, message);
            }
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (bo.ID) </param>
        /// <returns>The business object from the object manger.</returns>
        public IBusinessObject this[IPrimaryKey objectID]
        {
            //get { return this[objectID.AsString_CurrentValue()]; }
            get { return this[objectID.ObjectID]; }
        }

        /// <summary>
        /// Clears all the currently loaded business objects from the object manager. This is only used in testing and debugging.
        /// NNB: this method should only ever be used for testing. E.g. where the tester wants to test concurrency control or 
        /// to ensure that saving or loading from the data base is correct.
        /// </summary>
        public void ClearLoadedObjects()
        {
            lock (_loadedBusinessObjects)
            {
                if (_loadedBusinessObjects.Count == 0) return;
                //string[] keysArray = new string[_loadedBusinessObjects.Count];
                Guid[] keysArray = new Guid[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Keys.CopyTo(keysArray, 0);
                foreach (Guid key in keysArray)
                {
                    //if (key == null) return;
                    if (!Contains(key)) continue;
                    IBusinessObject businessObject = this[key];
                    this.Remove(key, businessObject);
                }
            }
        }

        #endregion

        //TODO 20 Jan 2009: improve performance of this, it's currently just using brute force.
        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <typeparam name="T">The Type of business object to find</typeparam>
        /// <param name="criteria">The Criteria to match on</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public BusinessObjectCollection<T> Find<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            lock (_loadedBusinessObjects)
            {
                BusinessObjectCollection<T> collection = new BusinessObjectCollection<T>();
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    if (!WeakReferenceIsAlive(weakReference)) continue;

                    BusinessObject bo = (BusinessObject) weakReference.Target;
                    if (bo is T && (criteria == null || criteria.IsMatch(bo, false)))
                    {
                        collection.Add(bo as T);
                    }
                }
                return collection;
            }
        }
        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria">The Criteria to match on</param>
        /// <param name="boType">The business object type being searched for</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public IBusinessObjectCollection Find(Criteria criteria, Type boType)
        {
            lock (_loadedBusinessObjects)
            {
                Type boColType = typeof(BusinessObjectCollection<>).MakeGenericType(boType);
                IBusinessObjectCollection collection = (IBusinessObjectCollection)Activator.CreateInstance(boColType);
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    if (!WeakReferenceIsAlive(weakReference)) continue;

                    BusinessObject bo = (BusinessObject)weakReference.Target;
                    if (boType.IsInstanceOfType(bo) && (criteria == null || criteria.IsMatch(bo, false)))
                    {
                        collection.Add(bo);
                    }
                }
                return collection;
            }
        }
    }
}