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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides a collection of property definitions.
    /// </summary>
    public class PropDefCol : IPropDefCol
    {
        private Dictionary<string, IPropDef> _propDefs;

        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public PropDefCol()
        {
            _propDefs = new Dictionary<string, IPropDef>();
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the propertyName is not
        /// found. If you are checking for the existence of a propertyName, use the
        /// Contains() method.</exception>
        public IPropDef this[string propertyName]
        {
            get
            {
                if (!Contains(propertyName.ToUpper()))
                {
                    throw new ArgumentException(String.Format(
                        "The property name '{0}' does not exist in the " +
                        "collection of property definitions.", propertyName));
                }
                return (_propDefs[propertyName.ToUpper()]);

                //else

                //return new PropDef("","",propertyName,;
                //	Throw (New Exception( obj.PropertyName + " is already in this BOProperty Collection",   "obj", e));
            }
        }

        /// <summary>
        /// Add an existing property definition to the collection
        /// </summary>
        /// <param name="propDef">The existing property definition</param>
        public void Add(IPropDef propDef)
        {
            CheckPropNotAlreadyAdded(propDef.PropertyName);
            _propDefs.Add(propDef.PropertyName.ToUpper(), propDef);
        }

        /// <summary>
        /// Adds all the property definitions from the given collection
        /// into this one.
        /// </summary>
        /// <param name="propDefCol">The collection of property definitions</param>
        public void Add(IPropDefCol propDefCol)
        {
            foreach (PropDef def in propDefCol)
            {
                Add(def);
            }
        }

        ///// <summary>
        ///// Create a new property definition and add it to the collection
        ///// </summary>
        ///// <param name="propName">The name of the property, e.g. surname</param>
        ///// <param name="propType">The type of the property, e.g. string</param>
        ///// <param name="propRWStatus">Rules for how a property can be
        ///// accessed. See PropReadWriteRule enumeration for more detail.</param>
        ///// <param name="databaseFieldName">The database field name - this
        ///// allows you to have a database field name that is different to the
        ///// property name, which is useful for migrating systems where
        ///// the database has already been set up</param>
        ///// <param name="defaultValue">The default value that a property 
        ///// of a new object will be set to</param>
        ///// <returns>Returns the new definition created, after it has
        ///// been added to the collection</returns>
        internal PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           string databaseFieldName,
                           object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           databaseFieldName, defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        ///// <summary>
        ///// Creates and adds a new property definition as before, but 
        ///// assumes the database field name is the same as the property name.
        ///// </summary>
        internal PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

		/// <summary>
		/// Removes a property definition from the collection
		/// </summary>
		/// <param name="propDef">The Property definition to remove</param>
		protected void Remove(PropDef propDef)
		{
			if (Contains(propDef))
			{
				_propDefs.Remove(propDef.PropertyName.ToUpper());
			}
		}

        /// <summary>
        /// Indicates if the specified property definition exists
        /// in the collection.
        /// </summary>
        /// <param name="propDef">The Property definition to search for</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(IPropDef propDef)
        {
            return (_propDefs.ContainsValue(propDef));
        }

        /// <summary>
		/// Indicates if a property definition with the given key exists
		/// in the collection.
		/// </summary>
        /// <param name="propertyName">The propertyName to match</param>
		/// <returns>Returns true if found, false if not</returns>
		public bool Contains(string propertyName)
		{
            return (_propDefs.ContainsKey(propertyName.ToUpper()));
		}

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="newObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        public BOPropCol CreateBOPropertyCol(bool newObject)
        {
            BOPropCol lBOPropertyCol = new BOPropCol();
            foreach (IPropDef lPropDef in this)
            {
                lBOPropertyCol.Add(lPropDef.CreateBOProp(newObject));
            }
            return lBOPropertyCol;
        }

		//public IEnumerator GetEnumerator()
		//{
		//    return _propDefs.Values.GetEnumerator();
		//}

        /// <summary>
        /// Checks if a property definition with that name has already been added
        /// and throws an exception if so
        /// </summary>
        /// <param name="propName">The property name</param>
        private void CheckPropNotAlreadyAdded(string propName)
        {
            if (Contains(propName) || Contains(propName.ToUpper()))
            {
                throw new ArgumentException(String.Format(
                    "A property definition with the name '{0}' already " +
                    "exists.", propName));
            }
        }

        /// <summary>
        /// Gets the number of definitions in this collection
        /// </summary>
        public int Count
        {
            get
            {
                return _propDefs.Count;
            }
        }

		#region IEnumerable<PropDef> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<IPropDef> IEnumerable<IPropDef>.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion

        #region Equals

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PropDefCol)) return false;
            PropDefCol otherPropDefCol = (PropDefCol)obj;
            if (this.Count != otherPropDefCol.Count) return false;
            foreach (PropDef def in this)
            {
                if (!otherPropDefCol.Contains(def.PropertyName)) return false;
            }
            return true;
        }

        #endregion


        ///<summary>
        /// Clones the propdefcol.  The new propdefcol has the same propdefs in it.
        ///</summary>
        ///<returns></returns>
        public IPropDefCol Clone()
        {
            PropDefCol newPropDefCol = new PropDefCol();
            foreach (PropDef def in this)
            {
                newPropDefCol.Add(def);
            }
            return newPropDefCol;
        }
    }
}