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
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides a collection of property definitions.
    /// </summary>
    public class PropDefCol : IPropDefCol
    {
        private readonly Dictionary<string, IPropDef> _propDefs;

        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public PropDefCol()
        {
            _propDefs = new Dictionary<string, IPropDef>();
        }

        #region IPropDefCol Members

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
                if (propertyName == null) throw new ArgumentNullException("propertyName");
                //This is written to Catch the Exception and rethrow as a new exception so as to 
                // overcomne the performance issues found during Profiling.
                try
                {
                    return (_propDefs[propertyName.ToUpper()]);
                }
                catch (Exception)
                {
                    throw new ArgumentException
                        (String.Format
                             ("The property name '{0}' does not exist in the " + "collection of property definitions.",
                              propertyName));
                }

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
        /// Removes a property definition from the collection
        /// </summary>
        /// <param name="propDef">The Property definition to remove</param>
        public void Remove(IPropDef propDef)
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
            if (propertyName == null) return false;
            return (_propDefs.ContainsKey(propertyName.ToUpper()));
        }

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="isNewObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        public IBOPropCol CreateBOPropertyCol(bool isNewObject)
        {
            BOPropCol lBOPropertyCol = new BOPropCol();
            foreach (IPropDef lPropDef in this)
            {
                lBOPropertyCol.Add(lPropDef.CreateBOProp(isNewObject));
            }
            return lBOPropertyCol;
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return _propDefs.Values.GetEnumerator();
        //}

        /// <summary>
        /// Gets the number of definitions in this collection
        /// </summary>
        public int Count
        {
            get { return _propDefs.Count; }
        }

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

        ///<summary>
        /// Clones the propdefcol. NNB: The new propdefcol has the same propdefs in it (i.e. the propdefs are not copied).
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

        /// <summary>
        /// Clones the propdefcol. This method was created so that you could control the depth of the copy. 
        /// The reason is so that you can limit the
        ///   extra memory used in cases where the propdef does not need to be copied.
        /// </summary>
        /// <param name="clonePropDefs">If true then makes a full copy of the propdefs else only 
        /// makes a copy of the propdefcol.</param>
        /// <returns></returns>
        public IPropDefCol Clone(bool clonePropDefs)
        {
            PropDefCol newPropDefCol = new PropDefCol();
            foreach (PropDef def in this)
            {
                if (clonePropDefs)
                {
                    newPropDefCol.Add(def.Clone());
                }
                else
                {
                    newPropDefCol.Add(def);
                }
            }
            return newPropDefCol;
        }

        #endregion 

        #region Equals

        IEnumerator<IPropDef> IEnumerable<IPropDef>.GetEnumerator()
        {
            return this._propDefs.Values.GetEnumerator();
        }

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
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((PropDefCol) obj);
        }

        ///<summary>
        /// Returns true if the PropDefCol and all its PropDefs are equal.
        ///</summary>
        ///<param name="obj">the PropDefCol to compare to</param>
        ///<returns></returns>
        public bool Equals(PropDefCol obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PropDefCol)) return false;
            PropDefCol otherPropDefCol = (PropDefCol) obj;
            if (Count != otherPropDefCol.Count) return false;
            foreach (PropDef def in this)
            {
                if (!otherPropDefCol.Contains(def.PropertyName)) return false;
                bool equals = def.Equals(otherPropDefCol[def.PropertyName]);
                if (!equals) return false;
            }
            return true;
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (_propDefs != null ? _propDefs.GetHashCode() : 0);
        }

        #endregion

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

        /// <summary>
        /// Create a new property definition and add it to the collection
        /// </summary>
        /// <param name="propName">The name of the property, e.g. surname</param>
        /// <param name="propType">The type of the property, e.g. string</param>
        /// <param name="propRWStatus">Rules for how a property can be
        /// accessed. See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        /// <returns>Returns the new definition created, after it has
        /// been added to the collection</returns>
        internal IPropDef Add
            (string propName, Type propType, PropReadWriteRule propRWStatus, string databaseFieldName,
             object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus, databaseFieldName, defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        /// <summary>
        /// Creates and adds a new property definition as before, but 
        /// assumes the database field name is the same as the property name.
        /// </summary>
        internal IPropDef Add(string propName, Type propType, PropReadWriteRule propRWStatus, object defaultValue)
        {
            CheckPropNotAlreadyAdded(propName);
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus, defaultValue);
            _propDefs.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        /// <summary>
        /// Checks if a property definition with that name has already been added
        /// and throws an exception if so
        /// </summary>
        /// <param name="propName">The property name</param>
        private void CheckPropNotAlreadyAdded(string propName)
        {
            if (propName == null) throw new ArgumentNullException("propName");
            if (Contains(propName.ToUpper()))
            {
                throw new ArgumentException
                    (String.Format("A property definition with the name '{0}' already " + "exists.", propName));
            }
        }
    }
}