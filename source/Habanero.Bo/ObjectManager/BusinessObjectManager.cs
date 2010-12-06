// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using log4net;

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
    ///  behaviour when the business object loader <see cref="IBusinessObjectLoader"/> gets a business object that is already in the object
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
    public class BusinessObjectManager : IBusinessObjectManager
    {
        private static readonly ILog _log = LogManager.GetLogger("Habanero.BO.BusinessObjectManager");
        /// <summary>
        /// The Single Instance of the <see cref="BusinessObjectManager"/> used by the Singleton.
        /// </summary>
        protected static IBusinessObjectManager _businessObjectManager = new BusinessObjectManager();

        //protected readonly Dictionary<string, WeakReference> _loadedBusinessObjects =
        //    new Dictionary<string, WeakReference>();
        /// <summary> The Busienss Objects Loaded in memory. This is a <see cref="WeakReference"/> so that the objects can still be garbage collected. </summary>
        protected readonly Dictionary<Guid, WeakReference> _loadedBusinessObjects = new Dictionary<Guid, WeakReference>();
        protected Dictionary<string, Guid> _compositeKeyIDs = new Dictionary<string, Guid>();
        protected readonly object _lock = new object();

        private readonly EventHandler<BOEventArgs> _updateIDEventHandler;

        /// <summary>
        /// The constructor for the Business Object Manager.
        /// </summary>
        protected internal BusinessObjectManager()
        {
            _updateIDEventHandler = ObjectID_Updated_Handler;
        }

        ///<summary>
        /// Returns the particular instance of the Business Object manager being used. 
        /// This implements the Singleton Design pattern.
        ///</summary>
        public static IBusinessObjectManager Instance
        {
            get { return _businessObjectManager; }
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
        public virtual void Add(IBusinessObject businessObject)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering void Add(IBusinessObject businessObject)");
            lock (_lock)
            {
                IBusinessObject loadedBusinessObject = GetObjectIfInManager(businessObject.ID.ObjectID);
                if (loadedBusinessObject != null) 
                {
                    if (ReferenceEquals(loadedBusinessObject, businessObject))
                    {
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting void Add(IBusinessObject businessObject) (exit 1)");
                        return;
                    }

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

                //If the object is not a guid id, store the current key in a dictionary along with the guid, so that the object
                // can be found quickly using its key.
                if (!businessObject.ID.IsGuidObjectID && (businessObject.ID.GetAsValue() != null && businessObject.ID.GetAsValue().ToString() != businessObject.ID.ObjectID.ToString()))
                {
                    try
                    {
                        _compositeKeyIDs.Add(businessObject.ID.AsString_CurrentValue(), businessObject.ID.ObjectID);
                    } catch (ArgumentException )
                    {
                        _compositeKeyIDs[businessObject.ID.AsString_CurrentValue()] = businessObject.ID.ObjectID;
                    }
                }
            }
            businessObject.IDUpdated += _updateIDEventHandler; //Register for ID Updated event this event is fired
            // if any of the properties that make up the primary key are changed/Updated this event is fires.
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting void Add(IBusinessObject businessObject) (exit 2)");
        }

        /// <summary>
        /// If the businessObject's IDUpdated Event has been fired then the business object is removed with the
        ///   old ID and added with the new ID. This is only required in cases with mutable primary keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ObjectID_Updated_Handler(object sender, BOEventArgs e)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering ObjectID_Updated_Handler(object sender, BOEventArgs e)");
            //if (e.BusinessObject.ID.IsGuidObjectID) return;
            lock (_lock)
            {
                this.Remove(e.BusinessObject);
                this.Add(e.BusinessObject);
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting ObjectID_Updated_Handler(object sender, BOEventArgs e)");
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="businessObject">The business object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        public bool Contains(IBusinessObject businessObject)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Contains(IBusinessObject businessObject)");
            lock (_lock)
            {
                Guid objectID = businessObject.ID.ObjectID;
                if (Contains(objectID))
                {
                    //if business object references are the same return true
                    IBusinessObject businessObjectFromManager;
                    try
                    {
                        businessObjectFromManager = this[objectID];
                    } catch (HabaneroDeveloperException) // this still might happen (although I don't know why since we lock...) 
                    {
                        return false;
                    }
                    if (ReferenceEquals(businessObject, businessObjectFromManager))
                    {
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(IBusinessObject businessObject) (exit 1)");
                        return true;
                    }
                }
                Guid previousObjectID = ((BOPrimaryKey) businessObject.ID).PreviousObjectID;
                if (Contains(previousObjectID))
                {
                    IBusinessObject businessObjectFromManager;
                    try
                    {
                        businessObjectFromManager = this[previousObjectID];
                    }
                    catch (HabaneroDeveloperException) // this still might happen (although I don't know why since we lock...)
                    {
                        return false;
                    }
                    if (ReferenceEquals(businessObject, businessObjectFromManager))
                    {
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(IBusinessObject businessObject) (exit 2)");
                        return true;
                    }
                }
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(IBusinessObject businessObject) (exit 3)");
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

        ///<summary>
        /// Checks whether the business object is in the <see cref="BusinessObjectManager"/>.
        ///</summary>
        ///<param name="objectID">The <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/>.<see cref="IPrimaryKey.ObjectID"/> value.</param>
        ///<returns>Whether the business object is in the <see cref="BusinessObjectManager"/> or not.</returns>
        public bool Contains(Guid objectID)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Contains(Guid objectID)");
            lock (_lock)
            {
                bool containsKey = _loadedBusinessObjects.ContainsKey(objectID);
                if (containsKey)
                {
                    if (!BusinessObjectWeakReferenceIsAlive(objectID))
                    {
                        _loadedBusinessObjects.Remove(objectID);
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(Guid objectID) (exit 1)");
                        return false;
                    }
                    //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(Guid objectID) (exit 2)");
                    return true;
                }
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Contains(Guid objectID) (exit 3)");
            return false;
        }

        private bool BusinessObjectWeakReferenceIsAlive(Guid objectID)
        {
            WeakReference boWeakRef = _loadedBusinessObjects[objectID];
            return WeakReferenceIsAlive(boWeakRef);
        }

        private static bool WeakReferenceIsAlive(WeakReference boWeakRef)
        {
            return (boWeakRef != null && boWeakRef.Target != null && boWeakRef.IsAlive);
        }

        /// <summary>
        /// Removes the business object Business object manager. If a 
        /// seperate instance of the business object is loaded in the object manager then it will not be removed.
        /// </summary>
        /// <param name="businessObject">business object to be removed.</param>
        public void Remove(IBusinessObject businessObject)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Remove(IBusinessObject businessObject)");
            lock (_lock)
            {
                if (!Contains(businessObject))
                {
                    //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Remove(IBusinessObject businessObject) (exit 1)");
                    return;
                }

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
                // If the object doesn't have a guid id, remove it from the composite key dictionary.
                // use all 3 possible key descriptions because the key might have changed 
                // (eg if this is being called from ObjectID_Updated_Handler)
                if (!businessObject.ID.IsGuidObjectID)
                {
                    _compositeKeyIDs.Remove(businessObject.ID.AsString_CurrentValue());
                    _compositeKeyIDs.Remove(businessObject.ID.AsString_PreviousValue());
                    _compositeKeyIDs.Remove(businessObject.ID.AsString_LastPersistedValue());
                }
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Remove(IBusinessObject businessObject) (exit 2)");
        }

        /// <summary>
        /// Removes the business object Business object manager. NB: if a seperate instance of the object 
        /// is loaded in the object manager it will be removed. When possible user <see cref="Remove(IBusinessObject)"/>
        /// </summary>
        /// <param name="objectID">The string ID of the business object to be removed.</param>
        /// <param name="businessObject">The business object being removed at this position.</param>
        protected void Remove(Guid objectID, IBusinessObject businessObject)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Remove(Guid objectID, IBusinessObject businessObject)");
            lock (_lock)
            {
                if (Contains(objectID) && ReferenceEquals(businessObject, this[objectID]))
                {
                    _loadedBusinessObjects.Remove(objectID);
                    DeregisterForIDUpdatedEvent(businessObject);
                }
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Remove(Guid objectID, IBusinessObject businessObject)");
        }

        /// <summary>
        /// Dereigisters for the IDValueUpdatedEvent.
        /// </summary>
        /// <param name="businessObject"></param>
        protected void DeregisterForIDUpdatedEvent(IBusinessObject businessObject)
        {
            businessObject.IDUpdated -= _updateIDEventHandler;
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        /// <returns>The business object from the object manger.</returns>
        public virtual IBusinessObject this[Guid objectID]
        {
            get
            {
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering this[Guid objectID]");
                lock (_lock)
                {
                    if (Contains(objectID))
                    {
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting this[Guid objectID] (exit 1)");
                        return (IBusinessObject) _loadedBusinessObjects[objectID].Target;
                    }
                }
                string message = "There was an attempt to retrieve the object identified by '" + objectID
                                 + "' from the object manager but it is not currently loaded.";
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting this[Guid objectID] (exit 2)");
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
        public virtual IBusinessObject this[IPrimaryKey objectID]
        {
            get { return this[objectID.ObjectID]; }
        }

        /// <summary>
        /// Clears all the currently loaded business objects from the object manager. This is only used in testing and debugging.
        /// NNB: this method should only ever be used for testing. E.g. where the tester wants to test concurrency control or 
        /// to ensure that saving or loading from the data base is correct.
        /// </summary>
        public void ClearLoadedObjects()
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering ClearLoadedObjects()");
            lock (_lock)
            {
                if (_loadedBusinessObjects.Count == 0)
                {
                    //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting ClearLoadedObjects() (exit 1)");
                    return;
                }
                Guid[] keysArray = new Guid[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Keys.CopyTo(keysArray, 0);
                foreach (Guid key in keysArray)
                {
                    IBusinessObject businessObject = GetObjectIfInManager(key);
                    if (businessObject == null) continue;
                    this.Remove(key, businessObject);
                }
                _compositeKeyIDs = new Dictionary<string, Guid>();
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting ClearLoadedObjects() (exit 2)");
        }

        #endregion

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <typeparam name="T">The Type of business object to find</typeparam>
        /// <param name="criteria">The Criteria to match on</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public IList<T> Find<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Find<T>(Criteria criteria)");
            lock (_lock)
            {
                IList<T> collection = new List<T>();
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    //----------Removed WeakReferenceAlive Check for performance reasons.
                    //if (!WeakReferenceIsAlive(weakReference)) continue;
                    //BusinessObject bo = (BusinessObject) weakReference.Target;
                    IBusinessObject bo = GetBusinessObject(weakReference);
                    if (bo == null) continue;
                    try
                    {
                        if (bo is T && (criteria == null || criteria.IsMatch(bo, false)))
                        {
                            collection.Add(bo as T);
                        }
                    }
                    //For Dynamic Business Objects the Props may have been added since the business object was loaded.
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing
                    }
                }
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Find<T>(Criteria criteria)");
                return collection;
            }
        }

        /// <summary>
        /// Finds the First Business Object that matches the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst<T>(Criteria criteria)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering FindFirst<T>(Criteria criteria)");
            lock (_lock)
            {
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    //----------Removed WeakReferenceAlive Check for performance reasons.
                    //if (!WeakReferenceIsAlive(weakReference)) continue;
                    //BusinessObject bo = (BusinessObject) weakReference.Target;
                    IBusinessObject bo = GetBusinessObject(weakReference);
                    if (bo == null) continue;
                    try
                    {
                        if (bo is T && (criteria == null || criteria.IsMatch(bo, false)))
                        {
                            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": FindFirst<T>(Criteria criteria) (exit 1)");
                            return bo;
                        }
                    }
                    //For Dynamic Business Objects the Props may have been added since the business object was loaded.
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing
                    }
                }
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": FindFirst<T>(Criteria criteria) (exit 2)");
                return null;
            }
        }
        ///<summary>
        /// Returns the BusinessObject identified by key.
        /// If no Business object is found then returns null.
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public IBusinessObject GetBusinessObject(IPrimaryKey key)
        {
            if (key.IsGuidObjectID)
            {
                return GetObjectIfInManager(key.ObjectID);
            }
            BOPrimaryKey boPrimaryKey = ((BOPrimaryKey) key);
            if (boPrimaryKey.BusinessObject == null) return null;
            return this.FindFirst(boPrimaryKey, boPrimaryKey.BusinessObject.ClassDef);
        }

        private static IBusinessObject GetBusinessObject(WeakReference weakReference)
        {
            try
            {
                return (BusinessObject)weakReference.Target;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria">The Criteria to match on</param>
        /// <param name="boType">The business object type being searched for</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public IList Find(Criteria criteria, Type boType)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering Find(Criteria criteria, Type boType)");
            lock (_lock)
            {
                IList collection = new ArrayList();
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    //----------Removed WeakReferenceAlive Check for performance reasons.
                    //if (!WeakReferenceIsAlive(weakReference)) continue;
                    //BusinessObject bo = (BusinessObject) weakReference.Target;
                    IBusinessObject bo = GetBusinessObject(weakReference);
                    if (bo == null) continue;
                    
                    //For Dynamic Business Objects the Props may have been added since the business object was loaded.
                    try
                    {
                        if (boType.IsInstanceOfType(bo) && (criteria == null || criteria.IsMatch(bo, false)))
                        {
                            collection.Add(bo);
                        }
                    }
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing
                    }
                }
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting Find(Criteria criteria, Type boType)");
                return collection;
            }
        }

        /// <summary>
        /// Finds the First Business Object that matches the type boType and the key given. Uses the internal composite key dictionary, 
        /// so this method is far faster than the other FindFirst methods for finding objects with composite keys.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst(BOPrimaryKey key, Type boType)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering FindFirst(BOPrimaryKey key, Type boType)");
            lock (_lock)
            {
                string asStringCurrentValue = key.AsString_CurrentValue();
                if (_compositeKeyIDs.ContainsKey(asStringCurrentValue))
                {
                    try
                    {
                        return this[_compositeKeyIDs[asStringCurrentValue]];
                    }
                    catch (HabaneroDeveloperException ex)
                    {
                        _compositeKeyIDs.Remove(asStringCurrentValue);
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(BOPrimaryKey key, Type boType) (exit 1)");
                        return FindFirst(key.GetKeyCriteria(), boType);
                    }
                }
                else
                {
                    //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(BOPrimaryKey key, Type boType) (exit 2)");
                    return null;
                }
            }
        }

        /// <summary>
        /// Finds the First Business Object that matches the IClassDef classDef and the key given. Uses the internal composite key dictionary, 
        /// so this method is far faster than the other FindFirst methods for finding objects with composite keys.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst(BOPrimaryKey key, IClassDef classDef)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering FindFirst(BOPrimaryKey key, IClassDef classDef)");
            lock (_lock)
            {
                string asStringCurrentValue = key.AsString_CurrentValue();
                if (_compositeKeyIDs.ContainsKey(asStringCurrentValue))
                {
                    try
                    {
                        return this[_compositeKeyIDs[asStringCurrentValue]];
                    }
                    catch (HabaneroDeveloperException ex)
                    {
                        _compositeKeyIDs.Remove(asStringCurrentValue);
                        //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(BOPrimaryKey key, IClassDef classDef) (exit 1)");
                        return FindFirst(key.GetKeyCriteria(), classDef);
                    }
                }
                else
                {
                    //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(BOPrimaryKey key, IClassDef classDef) (exit 2)");
                    return null;
                }
            }
        }  

        /// <summary>
        /// Finds the First Business Object that matches the type boType and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst(Criteria criteria, Type boType)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering FindFirst(Criteria criteria, Type boType)");
            lock (_lock)
            {
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    //----------Removed WeakReferenceAlive Check for performance reasons.
//                    if (!WeakReferenceIsAlive(weakReference)) continue;
                    //BusinessObject bo = (BusinessObject) weakReference.Target;
                    IBusinessObject bo = GetBusinessObject(weakReference);
                    if (bo == null) continue;
                    //For Dynamic Business Objects the Props may have been added since the business object was loaded.
                    try
                    {
                        if (boType.IsInstanceOfType(bo) && (criteria == null || criteria.IsMatch(bo, false)))
                        {
                            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(Criteria criteria, Type boType) (exit 1)");
                            return bo;
                        }
                    }
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing
                    }
                }
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(Criteria criteria, Type boType) (exit 2)");
                return null;
            }
        }  
        
        /// <summary>
        /// Finds the First Business Object that matches the classDef classDef and the Criteria given.
        /// </summary>
        /// <param name="criteria">Criteria to match on.</param>
        /// <param name="classDef">ClassDef that the BusinessObject must match.</param>
        /// <returns></returns>
        public IBusinessObject FindFirst(Criteria criteria, IClassDef classDef)
        {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering FindFirst(Criteria criteria, IClassDef classDef)");
            lock (_lock)
            {
                WeakReference[] valueArray = new WeakReference[_loadedBusinessObjects.Count];
                _loadedBusinessObjects.Values.CopyTo(valueArray, 0);
                foreach (WeakReference weakReference in valueArray)
                {
                    //----------Removed WeakReferenceAlive Check for performance reasons.
//                    if (!WeakReferenceIsAlive(weakReference)) continue;
                    //BusinessObject bo = (BusinessObject) weakReference.Target;
                    IBusinessObject bo = GetBusinessObject(weakReference);
                    if (bo == null) continue;
                    //For Dynamic Business Objects the Props may have been added since the business object was loaded.
                    try
                    {
                        if (bo.ClassDef==classDef && (criteria == null || criteria.IsMatch(bo, false)))
                        {
                            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(Criteria criteria, IClassDef classDef) (exit 1)");
                            return bo;
                        }
                    }
                    catch (InvalidPropertyNameException)
                    {
                        //Do Nothing
                    }
                }
                //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting FindFirst(Criteria criteria, IClassDef classDef) (exit 2)");
                return null;
            }
        }

        ///<summary>
        /// Adds the Business Object to the Object Manager and removes the existing object.
        /// This is used for Deserialising objects etc where a new exact deserialised replacement of the
        /// origional businessObject has been made.
        ///</summary>
        ///<param name="businessObject"></param>
        public void AddWithReplace(BusinessObject businessObject) {
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Entering AddWithReplace(BusinessObject businessObject)");
            lock (_lock)
            {
                if (this.Contains(businessObject.ID))
                {
                    _loadedBusinessObjects.Remove(businessObject.ID.ObjectID);
                }
                this.Add(businessObject);
            }
            //_log.Debug(Thread.CurrentThread.ManagedThreadId + ": Exiting AddWithReplace(BusinessObject businessObject)");
        }

        /// <summary>
        /// Returns the object specified by the guid passed in, if the object exists in the object manager.
        /// Returns null if the object is not found.
        /// </summary>
        /// <param name="id">The Id of the object to search the object manager for</param>
        /// <returns>The object identified by the ID, or null if the object is not found in the manager</returns>
        public IBusinessObject GetObjectIfInManager(Guid id)
        {
            lock (_lock)
            {
                if (this.Contains(id))
                {
                    try
                    {
                        return this[id];
                    } catch (HabaneroDeveloperException ex)
                    {
                        _log.Debug(
                            "Error in GetObjectIfInManager: Contains returned true but this[] threw an exception: " +
                            ex.Message + Environment.NewLine + ex.StackTrace);
                        return null;
                    }
                }
                return null;
            }
        }
    }

}