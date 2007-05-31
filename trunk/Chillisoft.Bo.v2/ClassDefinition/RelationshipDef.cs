using System;
using Chillisoft.Bo.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Describes the relationship between an object and some other object(s)
    /// </summary>
    public abstract class RelationshipDef
    {
        protected Type mRelatedObjectClassType;
        protected RelKeyDef mRelKeyDef;
        protected string mRelationshipName;
        protected bool mKeepReferenceToRelatedObject;

        /// <summary>
        /// Constructor to create a new relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object</param>
        /// TODO ERIC - review above param
        public RelationshipDef(string relationshipName,
                               Type relatedObjectClassType,
                               RelKeyDef relKeyDef,
                               bool keepReferenceToRelatedObject)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(relatedObjectClassType, "relatedObjectClassType");
            ArgumentValidationHelper.CheckArgumentNotNull(relKeyDef, "relKeyDef");
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
            ArgumentValidationHelper.CheckArgumentIsSubType(relatedObjectClassType, "relatedObjectClassType",
                                                            typeof (BusinessObjectBase));

            mRelatedObjectClassType = relatedObjectClassType;
            mRelKeyDef = relKeyDef;
            mRelationshipName = relationshipName;
            mKeepReferenceToRelatedObject = keepReferenceToRelatedObject;
        }

        /// <summary>
        /// A name for the relationship
        /// </summary>
        internal string RelationshipName
        {
            get { return mRelationshipName; }
        }

        /// <summary>
        /// The class type of the related object
        /// </summary>
        public Type RelatedObjectClassType
        {
            get { return mRelatedObjectClassType; }
        }

        /// <summary>
        /// The related key definition
        /// </summary>
        public RelKeyDef RelKeyDef
        {
            get { return mRelKeyDef; }
        }

        /// <summary>
        /// Whether to keep a reference to the related object
        /// </summary>
        /// TODO ERIC - review
        public bool KeepReferenceToRelatedObject
        {
            get { return mKeepReferenceToRelatedObject; }
        }

        /// <summary>
        /// Create and return a new Relationship
        /// </summary>
        /// <param name="owningBo">The business object that will
        /// manage this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns>The new relationship object created</returns>
        internal abstract Relationship CreateRelationship(BusinessObjectBase owningBo, BOPropCol lBOPropCol);
    }
}