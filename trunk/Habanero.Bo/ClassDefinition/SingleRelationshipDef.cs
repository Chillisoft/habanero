using System;
using Habanero.Bo;
using Habanero.Db;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Defines a relationship where the owner relates to only one other object.
    /// </summary>
    public class SingleRelationshipDef : RelationshipDef
    {
	
		#region Constructors

        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object</param>
        /// TODO ERIC - review last param
        public SingleRelationshipDef(string relationshipName, Type relatedObjectClassType, RelKeyDef relKeyDef,
                                     bool keepReferenceToRelatedObject)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject)
        {
		}

		/// <summary>
		/// Constructor to create a new single relationship definition
		/// </summary>
		/// <param name="relationshipName">A name for the relationship</param>
		/// <param name="relatedObjectAssemblyName">The assembly name of the related object</param>
		/// <param name="relatedObjectClassName">The class name of the related object</param>
		/// <param name="relKeyDef">The related key definition</param>
		/// <param name="keepReferenceToRelatedObject">Whether to keep a
		/// reference to the related object</param>
		/// TODO ERIC - review last param
		public SingleRelationshipDef(string relationshipName, string relatedObjectAssemblyName, string relatedObjectClassName, RelKeyDef relKeyDef,
									 bool keepReferenceToRelatedObject)
			: base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject)
		{
		}

		#endregion Constructors

		/// <summary>
        /// Overrides abstract method of RelationshipDef to create a new
        /// relationship
        /// </summary>
        /// <param name="owningBo">The business object that will manage
        /// this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns></returns>
        public override Relationship CreateRelationship(BusinessObject owningBo, BOPropCol lBOPropCol)
        {
            return new SingleRelationship(owningBo, this, lBOPropCol);
        }
    }

}