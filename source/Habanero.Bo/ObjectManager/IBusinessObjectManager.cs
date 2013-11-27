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
    public interface IBusinessObjectManager
    {
        /// <summary>
        /// Gets the number of business objects currently loaded. This is used
        /// primarily for debugging and testing.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Add a business object to the object manager.
        /// </summary>
        /// <param name="businessObject"></param>
        void Add(IBusinessObject businessObject);

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="businessObject">The business object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        bool Contains(IBusinessObject businessObject);

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="id"> The business object id being checked (bo.Id).</param>
        /// <returns> Whether the busienss object is loadd or not</returns>
        bool Contains(IPrimaryKey id);

        ///<summary>
        /// Checks whether the business object is in the <see cref="BusinessObjectManager"/>.
        ///</summary>
        ///<param name="objectID">The <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/>.<see cref="IPrimaryKey.ObjectID"/> value.</param>
        ///<returns>Whether the business object is in the <see cref="BusinessObjectManager"/> or not.</returns>
        bool Contains(Guid objectID);

        /// <summary>
        /// Removes the business object Business object manager. If a 
        /// seperate instance of the business object is loaded in the object manager then it will not be removed.
        /// </summary>
        /// <param name="businessObject">business object to be removed.</param>
        void Remove(IBusinessObject businessObject);

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        /// <returns>The business object from the object manger.</returns>
        IBusinessObject this[Guid objectID] { get; }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (bo.ID) </param>
        /// <returns>The business object from the object manger.</returns>
        IBusinessObject this[IPrimaryKey objectID] { get; }

        /// <summary>
        /// Clears all the currently loaded business objects from the object manager. This is only used in testing and debugging.
        /// NNB: this method should only ever be used for testing. E.g. where the tester wants to test concurrency control or 
        /// to ensure that saving or loading from the data base is correct.
        /// </summary>
        void ClearLoadedObjects();

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <typeparam name="T">The Type of business object to find</typeparam>
        /// <param name="criteria">The Criteria to match on</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        IList<T> Find<T>(Criteria criteria) where T : class, IBusinessObject, new();

        /// <summary>
        /// Finds the First Business Object that matches the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IBusinessObject FindFirst<T>(Criteria criteria);

        ///<summary>
        /// Returns the BusinessObject identified by key.
        /// If no Business object is found then returns null.
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        IBusinessObject GetBusinessObject(IPrimaryKey key);

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria">The Criteria to match on</param>
        /// <param name="boType">The business object type being searched for</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        IList Find(Criteria criteria, Type boType);

        /// <summary>
        /// Finds the First Business Object that matches the type boType and the key given. Uses the internal composite key dictionary, 
        /// so this method is far faster than the other FindFirst methods for finding objects with composite keys.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        IBusinessObject FindFirst(BOPrimaryKey key, Type boType);

        /// <summary>
        /// Finds the First Business Object that matches the IClassDef classDef and the key given. Uses the internal composite key dictionary, 
        /// so this method is far faster than the other FindFirst methods for finding objects with composite keys.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="classDef"></param>
        /// <returns></returns>
        IBusinessObject FindFirst(BOPrimaryKey key, IClassDef classDef);

        /// <summary>
        /// Finds the First Business Object that matches the type boType and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        IBusinessObject FindFirst(Criteria criteria, Type boType);

        /// <summary>
        /// Finds the First Business Object that matches the classDef classDef and the Criteria given.
        /// </summary>
        /// <param name="criteria">Criteria to match on.</param>
        /// <param name="classDef">ClassDef that the BusinessObject must match.</param>
        /// <returns></returns>
        IBusinessObject FindFirst(Criteria criteria, IClassDef classDef);

        ///<summary>
        /// Adds the Business Object to the Object Manager and removes the existing object.
        /// This is used for Deserialising objects etc where a new exact deserialised replacement of the
        /// original businessObject has been made.
        ///</summary>
        ///<param name="businessObject"></param>
        void AddWithReplace(IBusinessObject businessObject);

        /// <summary>
        /// Returns the object specified by the guid passed in, if the object exists in the object manager.
        /// Returns null if the object is not found.
        /// </summary>
        /// <param name="id">The Id of the object to search the object manager for</param>
        /// <returns>The object identified by the ID, or null if the object is not found in the manager</returns>
        IBusinessObject GetObjectIfInManager(Guid id);
    }
}