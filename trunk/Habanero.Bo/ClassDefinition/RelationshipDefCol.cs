using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Bo;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Manages a collection of relationship definitions
    /// </summary>
    public class RelationshipDefCol 
    {
        private Dictionary<string, RelationshipDef> _relDefs;

        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public RelationshipDefCol()
        {
            _relDefs = new Dictionary<string, RelationshipDef>();
        }

        /// <summary>
        /// Add an existing relationship to the collection
        /// </summary>
        /// <param name="relationshipDef">The existing relationship to add</param>
        public void Add(RelationshipDef relationshipDef)
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
		protected void Remove(RelationshipDef relationshipDef)
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
		protected bool Contains(RelationshipDef relationshipDef)
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
        public RelationshipDef this[string relationshipName]
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
        public RelationshipCol CreateRelationshipCol(BOPropCol lBoPropCol, BusinessObject bo)
        {
            RelationshipCol lRelationshipCol = new RelationshipCol(bo);
            ;
            foreach (RelationshipDef lRelationshipDef in this)
            {
                lRelationshipCol.Add(lRelationshipDef.CreateRelationship(bo, lBoPropCol));
            }
            return lRelationshipCol;
        }

        public IEnumerator GetEnumerator()
        {
            return _relDefs.Values.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return _relDefs.Count;
            }
        }

    }
}