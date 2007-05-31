using System.Collections;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Manages a collection of relationship definitions
    /// </summary>
    public class RelationshipDefCol : DictionaryBase
    {
        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public RelationshipDefCol()
        {
        }

        /// <summary>
        /// Add an existing relationship to the collection
        /// </summary>
        /// <param name="lRelationshipDef">The existing relationship to add</param>
        public void Add(RelationshipDef lRelationshipDef)
        {
            //TODO_Err: Add sensible error handling if prop already exists etc
            base.Dictionary.Add(lRelationshipDef.RelationshipName, lRelationshipDef);
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
                //TODOErr: put appropriate err handling
                return ((RelationshipDef) Dictionary[relationshipName]);
            }
        }

        /// <summary>
        /// Create a new collection of relationships
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <param name="bo">The business object that will manage these
        /// relationships</param>
        /// <returns>Returns the new collection created</returns>
        public RelationshipCol CreateRelationshipCol(BOPropCol lBoPropCol, BusinessObjectBase bo)
        {
            RelationshipCol lRelationshipCol = new RelationshipCol(bo);
            RelationshipDef lRelationshipDef;
            foreach (DictionaryEntry item in this)
            {
                lRelationshipDef = (RelationshipDef) item.Value;
                lRelationshipCol.Add(lRelationshipDef.CreateRelationship(bo, lBoPropCol));
            }
            return lRelationshipCol;
        }
    }
}