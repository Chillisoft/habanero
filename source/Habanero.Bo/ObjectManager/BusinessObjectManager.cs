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
        private static readonly BusinessObjectManager _businessObjectManager = new BusinessObjectManager();

        private readonly Dictionary<string, WeakReference> _loadedBusinessObjects =
            new Dictionary<string, WeakReference>();

        private readonly EventHandler<BOKeyEventArgs> _updateIDEventHandler;

        private BusinessObjectManager()
        {
            _updateIDEventHandler = ID_OnUpdated;
        }

        ///<summary>
        /// Returns the particular instance of the Business Object manager being used. 
        /// This implements the Singleton Design pattern.
        ///</summary>
        public static BusinessObjectManager Instance
        {
            get { return _businessObjectManager; }
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
                if (ReferenceEquals(loadedBusinessObject, businessObject)) return;

                throw new HabaneroDeveloperException(
                    "There was a serious developer exception. Two copies of the business object '"
                    + businessObject.ClassDef.ClassNameFull + "' were added to the object manager",
                    "Two copies of the business object '"
                    + businessObject.ClassDef.ClassNameFull + "' identified by '" + businessObject.ID.GetObjectId() +
                    "' were added to the object manager");
            }
            //If object is new and does not have IsObjectID then add with the Guid.?
            //Else add as below
            _loadedBusinessObjects.Add(businessObject.ID.GetObjectId(), new WeakReference(businessObject));
            businessObject.ID.Updated += _updateIDEventHandler; //TODO Brett 13 Jan 2009: My concerns are that this will cause the a memory leak
            // to investigate with tests.
        }

        private void ID_OnUpdated(object sender, BOKeyEventArgs e)
        {
          //RemoveUnderThisID   ((BOPrimaryKey)e.BOKey).GetObjectId()
            //Add with 
//            ((BOPrimaryKey)e.BOKey).PersistedDatabaseWhereClause()
            //Remove with old ID and add with new id.
            //If object is new and does not have objectId then remove with Guid?
            //Else remove with Previous Value.
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