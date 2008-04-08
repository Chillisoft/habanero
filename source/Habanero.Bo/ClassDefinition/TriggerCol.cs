//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections;
using System.Collections.Generic;
using Habanero.BO;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a collection of triggers assigned to a specific
    /// user interface control
    /// </summary>
    public class TriggerCol : List<Trigger>, IEnumerable<Trigger>
    {
//        private Dictionary<string, Trigger> _triggers;
//
//        /// <summary>
//        /// A constructor to create a new empty collection
//        /// </summary>
//        public TriggerCol()
//        {
//            _triggers = new Dictionary<string, Trigger>();
//        }
//
//        /// <summary>
//        /// Provides an indexing facility for the collection so that items
//        /// in the collection can be accessed like an array 
//        /// (e.g. collection["surname"])
//        /// </summary>
//        /// <exception cref="ArgumentException">Thrown if the key is not
//        /// found. If you are checking for the existence of a key, use the
//        /// Contains() method.</exception>
//        public Trigger this[string key]
//        {
//            get
//            {
//                if (!Contains(key.ToUpper()))
//                {
//                    throw new ArgumentException(String.Format(
//                        "The property name '{0}' does not exist in the " +
//                        "collection of property definitions.", key));
//                }
//                return (_triggers[key.ToUpper()]);
//
//                //else
//
//                //return new Trigger("","",key,;
//                //	Throw (New Exception( obj.PropertyName + " is already in this BOProperty Collection",   "obj", e));
//            }
//        }
//
//        /// <summary>
//        /// Add an existing property definition to the collection
//        /// </summary>
//        /// <param name="trigger">The existing property definition</param>
//        public void Add(Trigger trigger)
//        {
//            CheckPropNotAlreadyAdded(trigger.PropertyName);
//            _triggers.Add(trigger.PropertyName.ToUpper(), trigger);
//        }
//
//        /// <summary>
//        /// Adds all the property definitions from the given collection
//        /// into this one.
//        /// </summary>
//        /// <param name="triggerCol">The collection of property definitions</param>
//        public void Add(TriggerCol triggerCol)
//        {
//            foreach (Trigger def in triggerCol)
//            {
//                Add(def);
//            }
//        }
//
//        ///// <summary>
//        ///// Create a new property definition and add it to the collection
//        ///// </summary>
//        ///// <param name="propName">The name of the property, e.g. surname</param>
//        ///// <param name="propType">The type of the property, e.g. string</param>
//        ///// <param name="propRWStatus">Rules for how a property can be
//        ///// accessed. See PropReadWriteRule enumeration for more detail.</param>
//        ///// <param name="databaseFieldName">The database field name - this
//        ///// allows you to have a database field name that is different to the
//        ///// property name, which is useful for migrating systems where
//        ///// the database has already been set up</param>
//        ///// <param name="defaultValue">The default value that a property 
//        ///// of a new object will be set to</param>
//        ///// <returns>Returns the new definition created, after it has
//        ///// been added to the collection</returns>
//        internal Trigger Add(string propName,
//                           Type propType,
//                           PropReadWriteRule propRWStatus,
//                           string databaseFieldName,
//                           object defaultValue)
//        {
//            CheckPropNotAlreadyAdded(propName);
//            Trigger lTrigger = new Trigger(propName, propType, propRWStatus,
//                                           databaseFieldName, defaultValue);
//            _triggers.Add(lTrigger.PropertyName.ToUpper(), lTrigger);
//            return lTrigger;
//        }
//
//        ///// <summary>
//        ///// Creates and adds a new property definition as before, but 
//        ///// assumes the database field name is the same as the property name.
//        ///// </summary>
//        internal Trigger Add(string propName,
//                           Type propType,
//                           PropReadWriteRule propRWStatus,
//                           object defaultValue)
//        {
//            CheckPropNotAlreadyAdded(propName);
//            Trigger lTrigger = new Trigger(propName, propType, propRWStatus,
//                                           defaultValue);
//            _triggers.Add(lTrigger.PropertyName.ToUpper(), lTrigger);
//            return lTrigger;
//        }
//
//		/// <summary>
//		/// Removes a property definition from the collection
//		/// </summary>
//		/// <param name="trigger">The Property definition to remove</param>
//		protected void Remove(Trigger trigger)
//		{
//			if (Contains(trigger))
//			{
//				_triggers.Remove(trigger.PropertyName.ToUpper());
//			}
//		}
//
//		/// <summary>
//		/// Indicates if the specified property definition exists
//		/// in the collection.
//		/// </summary>
//		/// <param name="trigger">The Property definition to search for</param>
//		/// <returns>Returns true if found, false if not</returns>
//		protected bool Contains(Trigger trigger)
//		{
//			return (_triggers.ContainsKey(trigger.PropertyName.ToUpper()));
//		}
//
//		/// <summary>
//		/// Indicates if a property definition with the given key exists
//		/// in the collection.
//		/// </summary>
//		/// <param name="key">The key to match</param>
//		/// <returns>Returns true if found, false if not</returns>
//		public bool Contains(string key)
//		{
//			return (_triggers.ContainsKey(key.ToUpper()));
//		}
//
//        /// <summary>
//        /// Creates a business object property collection that mirrors
//        /// this one.  The new collection will contain a BOProp object for
//        /// each Trigger object in this collection, with that BOProp object
//        /// storing an instance of the Trigger object.
//        /// </summary>
//        /// <param name="newObject">Whether the new BOProps in the
//        /// collection will be new objects. See Trigger.CreateBOProp
//        /// for more info.</param>
//        /// <returns>Returns the new BOPropCol object</returns>
//        public BOPropCol CreateBOPropertyCol(bool newObject)
//        {
//            BOPropCol lBOPropertyCol = new BOPropCol();
//            foreach (Trigger lTrigger in this)
//            {
//                lBOPropertyCol.Add(lTrigger.CreateBOProp(newObject));
//                if (lTrigger.AutoIncrementing) {
//                    lBOPropertyCol.AutoIncrementingPropertyName = lTrigger.PropertyName;
//                }
//            }
//            return lBOPropertyCol;
//        }
//
//		//public IEnumerator GetEnumerator()
//		//{
//		//    return _triggers.Values.GetEnumerator();
//		//}
//
//        /// <summary>
//        /// Checks if a property definition with that name has already been added
//        /// and throws an exception if so
//        /// </summary>
//        /// <param name="propName">The property name</param>
//        private void CheckPropNotAlreadyAdded(string propName)
//        {
//            if (Contains(propName) || Contains(propName.ToUpper()))
//            {
//                throw new ArgumentException(String.Format(
//                    "A property definition with the name '{0}' already " +
//                    "exists.", propName));
//            }
//        }
//
//        public int Count
//        {
//            get
//            {
//                return _triggers.Count;
//            }
//        }

		#region IEnumerable<Trigger> Members

		IEnumerator<Trigger> IEnumerable<Trigger>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
    }
}