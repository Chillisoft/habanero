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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Lists a set of property definitions that indicate which properties
    /// to match together for two classes in a relationship
    /// </summary>
    public class RelKeyDef : IEnumerable<RelPropDef>
    {
        private Dictionary<string, RelPropDef> _relPropDefs;

        /// <summary>
        /// Constructor to create a new RelKeyDef object
        /// </summary>
        public RelKeyDef() : base()
        {
            _relPropDefs = new Dictionary<string, RelPropDef>();
        }

        /// <summary>
        /// Provides an indexing facility for the property definitions
        /// in this key definition so that they can be 
        /// accessed like an array (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>Returns the corresponding RelPropDef object</returns>
        public RelPropDef this[string propName]
        {
            get
            {
                if (!Contains(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "In a relationship property definition, the property " +
                        "with the name '{0}' does not exist in the collection " +
                        "of properties.", propName));
                }
                return _relPropDefs[propName];
            }
        }

        /// <summary>
        /// Adds the related property definition to this key, as long as
        /// a property by that name has not already been added.
        /// </summary>
        /// <param name="relPropDef">The RelPropDef object to be added.</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// argument passed is null</exception>
        public virtual void Add(RelPropDef relPropDef)
        {
            if (relPropDef == null)
            {
				throw new HabaneroArgumentException("relPropDef",
                                                   "ClassDef-Add. You cannot add a null prop def to a classdef");
            }
            if (!Contains(relPropDef))
            {
                _relPropDefs.Add(relPropDef.OwnerPropertyName, relPropDef);
            }
        }

		/// <summary>
		/// Removes a Related Property definition from the key
		/// </summary>
		/// <param name="relPropDef">The Related Property Definition to remove</param>
		protected void Remove(RelPropDef relPropDef)
		{
			if (Contains(relPropDef))
			{
				_relPropDefs.Remove(relPropDef.OwnerPropertyName);
			}
		}

        /// <summary>
        /// Returns true if the specified property is found.
        /// </summary>
		/// <param name="relPropDef">The Related Property Definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		internal protected bool Contains(RelPropDef relPropDef)
        {
			return (_relPropDefs.ContainsKey(relPropDef.OwnerPropertyName));
        }

        /// <summary>
        /// Returns true if a property with this name is part of this key.
        /// </summary>
        /// <param name="propName">The property name to search by</param>
        /// <returns>Returns true if found, false if not</returns>
        internal bool Contains(string propName)
        {
            return (_relPropDefs.ContainsKey(propName));
        }

        /// <summary>
        /// Create a relationship key based on this key definition and
        /// its associated property definitions
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>Returns the new RelKey object</returns>
        public RelKey CreateRelKey(BOPropCol lBoPropCol)
        {
            RelKey lRelKey = new RelKey(this);
            foreach (RelPropDef relPropDef in this)
            {
                lRelKey.Add(relPropDef.CreateRelProp(lBoPropCol));
            }
            return lRelKey;
        }

		//public IEnumerator GetEnumerator()
		//{
		//    return _relPropDefs.Values.GetEnumerator();
		//}

        public int Count
        {
            get
            {
                return _relPropDefs.Count;
            }
		}

		#region IEnumerable<RelPropDef> Members

		IEnumerator<RelPropDef> IEnumerable<RelPropDef>.GetEnumerator()
		{
			return _relPropDefs.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _relPropDefs.Values.GetEnumerator();
		}

		#endregion
    }


}