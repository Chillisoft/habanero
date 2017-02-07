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
    /// <summary>
    /// This is a Business Object Manager that can be used for ASP and similarly stateless environments.
    /// </summary>
    public class BusinessObjectManagerNull : IBusinessObjectManager
    {

        #region Implementation of IBusinessObjectManager

        /// <summary>
        /// Gets the number of business objects currently loaded. This is used
        /// primarily for debugging and testing.
        /// </summary>
        public int Count
        {
            get { return 0; }
        }

        /// <summary>
        /// Add a business object to the object manager.
        /// </summary>
        /// <param name="businessObject"></param>
        public void Add(IBusinessObject businessObject)
        {
            // Do Nothing
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="businessObject">The business object being checked.</param>
        /// <returns>Whether the busienss object is loadd or not</returns>
        public bool Contains(IBusinessObject businessObject)
        {
            // Do Nothing
            return false;
        }

        /// <summary>
        /// Checks whether the business object is currently loaded.
        /// </summary>
        /// <param name="id"> The business object id being checked (bo.Id).</param>
        /// <returns> Whether the busienss object is loadd or not</returns>
        public bool Contains(IPrimaryKey id)
        {
            // Do Nothing
            return false;
        }

        ///<summary>
        /// Checks whether the business object is in the <see cref="BusinessObjectManager"/>.
        ///</summary>
        ///<param name="objectID">The <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/>.<see cref="IPrimaryKey.ObjectID"/> value.</param>
        ///<returns>Whether the business object is in the <see cref="BusinessObjectManager"/> or not.</returns>
        public bool Contains(Guid objectID)
        {
            // Do Nothing
            return false;
        }

        /// <summary>
        /// Removes the business object Business object manager. If a 
        /// seperate instance of the business object is loaded in the object manager then it will not be removed.
        /// </summary>
        /// <param name="businessObject">business object to be removed.</param>
        public void Remove(IBusinessObject businessObject)
        {
            // Do Nothing
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (usually bo.ID.GetObjectID</param>
        /// <returns>The business object from the object manger.</returns>
        public IBusinessObject this[Guid objectID]
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the business object identified by the objectID from the business object manager.
        /// </summary>
        /// <param name="objectID">The business object id of the object being returned. (bo.ID) </param>
        /// <returns>The business object from the object manger.</returns>
        public IBusinessObject this[IPrimaryKey objectID]
        {
            get { return null; }
        }

        /// <summary>
        /// Clears all the currently loaded business objects from the object manager. This is only used in testing and debugging.
        /// NNB: this method should only ever be used for testing. E.g. where the tester wants to test concurrency control or 
        /// to ensure that saving or loading from the data base is correct.
        /// </summary>
        public void ClearLoadedObjects()
        {
            // Do Nothing
        }

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <typeparam name="T">The Type of business object to find</typeparam>
        /// <param name="criteria">The Criteria to match on</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public IList<T> Find<T>(Criteria criteria) where T : class, IBusinessObject, new()
        {
            // Do Nothing
            return new List<T>();
        }

        /// <summary>
        /// Finds the First Business Object that matches the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst<T>(Criteria criteria)
        {
            // Do Nothing
            return null;
        }

        ///<summary>
        /// Returns the BusinessObject identified by key.
        /// If no Business object is found then returns null.
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public IBusinessObject GetBusinessObject(IPrimaryKey key)
        {
            // Do Nothing
            return null;
        }

        /// <summary>
        /// Finds all the loaded business objects that match the type T and the Criteria given.
        /// </summary>
        /// <param name="criteria">The Criteria to match on</param>
        /// <param name="boType">The business object type being searched for</param>
        /// <returns>A collection of all loaded matching business objects</returns>
        public IList Find(Criteria criteria, Type boType)
        {
            // Do Nothing
            return new ArrayList();
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
            // Do Nothing
            return null;
        }

        /// <summary>
        /// Finds the First Business Object that matches the IClassDef classDef and the key given. Uses the internal composite key dictionary, 
        /// so this method is far faster than the other FindFirst methods for finding objects with composite keys.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="classDef"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst(BOPrimaryKey key, IClassDef classDef)
        {
            // Do Nothing
            return null;
        }

        /// <summary>
        /// Finds the First Business Object that matches the type boType and the Criteria given.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        public IBusinessObject FindFirst(Criteria criteria, Type boType)
        {
            // Do Nothing
            return null;
        }

        /// <summary>
        /// Finds the First Business Object that matches the classDef classDef and the Criteria given.
        /// </summary>
        /// <param name="criteria">Criteria to match on.</param>
        /// <param name="classDef">ClassDef that the BusinessObject must match.</param>
        /// <returns></returns>
        public IBusinessObject FindFirst(Criteria criteria, IClassDef classDef)
        {
            // Do Nothing
            return null;
        }

        ///<summary>
        /// Adds the Business Object to the Object Manager and removes the existing object.
        /// This is used for Deserialising objects etc where a new exact deserialised replacement of the
        /// origional businessObject has been made.
        ///</summary>
        ///<param name="businessObject"></param>
        public void AddWithReplace(IBusinessObject businessObject)
        {
            // Do Nothing
        }

        /// <summary>
        /// Returns the object specified by the guid passed in, if the object exists in the object manager.
        /// Returns null if the object is not found.
        /// </summary>
        /// <param name="id">The Id of the object to search the object manager for</param>
        /// <returns>The object identified by the ID, or null if the object is not found in the manager</returns>
        public IBusinessObject GetObjectIfInManager(Guid id)
        {
            // Do Nothing
            return null;
        }

        #endregion
    }
}