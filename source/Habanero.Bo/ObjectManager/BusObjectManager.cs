using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ObjectManager
{
    ///<summary>
    ///The object manager is a class that contains weak references
    /// to all currently loaded business objects. The object manager is therefore used to ensure that the current user/session only
    /// ever has one reference to a particular business object. This is used to prevent instances where a business object loaded in
    /// two different ways by a single user is represented by two different objects in the system thus resulting in concurrency control 
    /// exceptions.
    ///</summary>
    public class BusObjectManager
    {
        private static readonly BusObjectManager _busObjectManager = new BusObjectManager();

        private readonly Dictionary<string, WeakReference> _loadedBusinessObjects =
            new Dictionary<string, WeakReference>();

        private BusObjectManager() {}

        ///<summary>
        /// Returns the particular instance of the Business Object manager being used. 
        /// This implements the Singleton Design pattern.
        ///</summary>
        public static BusObjectManager Instance
        {
            get { return _busObjectManager; }
        }

        #region IBusObjectManager Members

        /// <summary>
        /// How many busiess objects are currently loaded. This is used primarily for debugging and testing.
        /// </summary>
        internal int Count
        {
            get { return _loadedBusinessObjects.Count; }
        }

        /// <summary>
        /// Add a business object to the object manager.
        /// </summary>
        /// <param name="businessObject"></param>
        internal void Add(IBusinessObject businessObject)
        {
            if (_loadedBusinessObjects.ContainsKey(businessObject.ID.GetObjectId()))
            {
                IBusinessObject loadedBusinessObject = this[businessObject.ID];
                if (!ReferenceEquals(loadedBusinessObject, businessObject))
                {
                    throw new HabaneroDeveloperException(
                        "There was a serious developer exception. Two copies of the business object '"
                        + businessObject.ClassDef.ClassNameFull + "' were added to the object manager",
                        "Two copies of the business object '"
                        + businessObject.ClassDef.ClassNameFull + "' identified by '" + businessObject.ID.GetObjectId() +
                        "' were added to the object manager");
                }
                return;
            }
            _loadedBusinessObjects.Add(businessObject.ID.GetObjectId(), new WeakReference(businessObject));
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="businessObject">The business object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        internal bool Contains(IBusinessObject businessObject)
        {
             if (Contains(businessObject.ID))
             {
                 return (businessObject ==  this[businessObject.ID]);
             }
            return false;
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="id"> The business object id being checked (bo.Id).</param>
        /// <returns> Whether the busienss object is loadd or not</returns>
        internal bool Contains(IPrimaryKey id)
        {
            return Contains(id.GetObjectId());
            
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="objectID">The string identity (usually bo.ID.GetObjectID()) of the object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        internal bool Contains(string objectID)
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
            return containsKey;
        }

        private bool BusinessObjectWeakReferenceIsAlive(string objectID)
        {
            WeakReference boWeakRef = _loadedBusinessObjects[objectID];
            return (boWeakRef.IsAlive && boWeakRef.Target != null);
        }

        /// <summary>
        /// Removes the business object Business object manager. If a 
        /// seperate instance of the business object is loaded in the object manager then it will not be removed.
        /// </summary>
        /// <param name="businessObject">business object to be removed.</param>
        internal void Remove(IBusinessObject businessObject)
        {
            if (Contains(businessObject))
            {
                Remove(businessObject.ID);
            }
        }

        /// <summary>
        /// Removes the business object Business object manager. NB: if a seperate instance of the object 
        /// is loaded in the object manager it will be removed. When possible user <see cref="Remove(IBusinessObject)"/>
        /// </summary>
        /// <param name="id">ID of the business object to be removed.</param>
        internal void Remove(IPrimaryKey id)
        {
            Remove(id.GetObjectId());
        }
        /// <summary>
        /// Removes the business object Business object manager. NB: if a seperate instance of the object 
        /// is loaded in the object manager it will be removed. When possible user <see cref="Remove(IBusinessObject)"/>
        /// </summary>
        /// <param name="id">The string ID of the business object to be removed.</param>
        internal void Remove(string id)
        {
            _loadedBusinessObjects.Remove(id);
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        /// <returns>The business object from the object manger.</returns>
        internal IBusinessObject this[string objectID]
        {
            get {
                if (Contains(objectID))
                {
                    return (IBusinessObject) _loadedBusinessObjects[objectID].Target; 
                }
                throw new HabaneroDeveloperException("There is an application error please contact your system administrator.", 
                        "There was an attempt to retrieve the object identified by '" 
                        + objectID + "' from the object manager but it is not currently loaded.");
            }
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (bo.ID) </param>
        /// <returns>The business object from the object manger.</returns>
        internal IBusinessObject this[IPrimaryKey objectID]
        {
            get { return this[objectID.GetObjectId()]; }
        }


        /// <summary>
        /// Clears all the currently loaded business objects from the object manager. This is only used in testing and debugging.
        /// NNB: this method should only ever be used for testing. E.g. where the tester wants to test concurrency control or 
        /// to ensure that saving or loading from the data base is correct.
        /// </summary>
        public void ClearLoadedObjects()
        {
            _loadedBusinessObjects.Clear();
        }

        #endregion
    }
}