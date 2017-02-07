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

namespace Habanero.BO.ClassDefinition
{
    
    /// <summary>
    /// Manages a collection of relationship definitions
    /// </summary>
    public class RelationshipDefCol : IRelationshipDefCol
    {
        private readonly Dictionary<string, IRelationshipDef> _relDefs;

        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public RelationshipDefCol()
        {
            _relDefs = new Dictionary<string, IRelationshipDef>();
        }

        /// <summary>
        /// Add an existing relationship to the collection
        /// </summary>
        /// <param name="relationshipDef">The existing relationship to add</param>
        public void Add(IRelationshipDef relationshipDef)
        {
            if (Contains(relationshipDef))
            {
                throw new ArgumentException(String.Format(
                    "A relationship definition with the name '{0}' already " +
                    "exists.", relationshipDef.RelationshipName));
            }
            _relDefs.Add(relationshipDef.RelationshipName, relationshipDef);
        }

		/// <summary>
		/// Removes a relationship definition from the collection
		/// </summary>
		/// <param name="relationshipDef">The Relationship definition to remove</param>
		protected void Remove(IRelationshipDef relationshipDef)
		{
			if (Contains(relationshipDef))
			{
                _relDefs.Remove(relationshipDef.RelationshipName);
			}
		}


		/// <summary>
		/// Indicates whether the collection contains the relationship
		/// definition specified
		/// </summary>
		/// <param name="relationshipDef">The Relationship definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		protected bool Contains(IRelationshipDef relationshipDef)
		{
            return _relDefs.ContainsKey(relationshipDef.RelationshipName);
		}


		/// <summary>
		/// Indicates whether the collection contains the relationship
		/// definition specified
		/// </summary>
		/// <param name="keyName">The name of the definition</param>
		/// <returns>Returns true if found, false if not</returns>
		public bool Contains(string keyName)
		{
			return _relDefs.ContainsKey(keyName);
		}

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["marriage"])
        /// </summary>
        /// <param name="relationshipName">The name of the relationship to
        /// access</param>
        /// <returns>Returns the relationship definition that matches the
        /// name provided</returns>
        public IRelationshipDef this[string relationshipName]
        {
            get
            {
                if (!Contains(relationshipName))
                {
                    throw new ArgumentException(String.Format(
                        "The relationship name '{0}' does not exist in the " +
                        "collection of relationship definitions.", relationshipName));
                }
                return _relDefs[relationshipName];
            }
        }

        /// <summary>
        /// Create a new collection of relationships
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <param name="bo">The business object that will manage these
        /// relationships</param>
        /// <returns>Returns the new collection created</returns>
        public RelationshipCol CreateRelationshipCol(IBOPropCol lBoPropCol, IBusinessObject bo)
        {
            RelationshipCol lRelationshipCol = new RelationshipCol(bo);
            foreach (RelationshipDef lRelationshipDef in this)
            {
                lRelationshipCol.Add(lRelationshipDef.CreateRelationship(bo, lBoPropCol));
            }
            return lRelationshipCol;
        }

		//public IEnumerator GetEnumerator()
		//{
		//    return _relDefs.Values.GetEnumerator();
		//}

        /// <summary>
        /// Gets the count of items in this collection
        /// </summary>
        public int Count
        {
            get
            {
                return _relDefs.Count;
            }
		}

        /// <summary>
        /// The ClassDef this RelationshipDefCol belongs to
        /// </summary>
        public IClassDef ClassDef { get; set; }

        #region IEnumerable<IRelationshipDef> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
       public IEnumerator<IRelationshipDef> GetEnumerator()
		{
			return _relDefs.Values.GetEnumerator();
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
			return _relDefs.Values.GetEnumerator();
		}

		#endregion

    }
}