// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{



    /// <summary>
    /// This class contains the definition of a Foreign Key that defines the properties <see cref="RelPropDef"/> that
    ///   that forms a relationship between two Classes. 
    /// This class collaborates with the <see cref="RelPropDef"/>, the <see cref="ClassDef"/> 
    ///   to provide a definition of the properties involved in the <see cref="RelationshipDef"/> between 
    ///   two <see cref="IBusinessObject"/>. This provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// the RelKeyDef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelKeyDef (Relationship Key Definition) is a list of relationship Property Defs <see cref="RelPropDef"/> that 
    ///   define the properties that form the persistant relationship definition (<see cref="RelationshipDef"/> between 
    ///   two Business object defitions (<see cref="ClassDef"/>.
    ///   <see cref="IBusinessObject"/>.
    /// </summary>
    public class RelKeyDef : IRelKeyDef
    {
        private readonly Dictionary<string, IRelPropDef> _relPropDefs;

        /// <summary>
        /// Constructor to create a new RelKeyDef object
        /// </summary>
        public RelKeyDef()
        {
            _relPropDefs = new Dictionary<string, IRelPropDef>();
        }

        /// <summary>
        /// Provides an indexing facility for the property definitions
        /// in this key definition so that they can be 
        /// accessed like an array (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>Returns the corresponding RelPropDef object</returns>
        public IRelPropDef this[string propName]
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
        public virtual void Add(IRelPropDef relPropDef)
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
		protected void Remove(IRelPropDef relPropDef)
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
		internal protected bool Contains(IRelPropDef relPropDef)
        {
			return (_relPropDefs.ContainsKey(relPropDef.OwnerPropertyName));
        }

        /// <summary>
        /// Returns true if a property with this name is part of this key.
        /// </summary>
        /// <param name="propName">The property name to search by</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(string propName)
        {
            return (_relPropDefs.ContainsKey(propName));
        }

        /// <summary>
        /// Create a relationship key based on this key definition and
        /// its associated property definitions
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>Returns the new RelKey object</returns>
        public IRelKey CreateRelKey(IBOPropCol lBoPropCol)
        {
            RelKey lRelKey = new RelKey(this, lBoPropCol);
            return lRelKey;
        }

        ///<summary>
        /// The number of property definitiosn defined in the relKeyDef
        ///</summary>
        public int Count
        {
            get
            {
                return _relPropDefs.Count;
            }
		}

		#region IEnumerable Members

        public IEnumerator<IRelPropDef> GetEnumerator()
        {
            return _relPropDefs.Values.GetEnumerator();
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
			return _relPropDefs.Values.GetEnumerator();
		}

		#endregion
    }


}