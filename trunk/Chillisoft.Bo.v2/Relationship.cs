using Chillisoft.Bo.ClassDefinition.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a super-class for relationships between business objects
    /// </summary>
    public abstract class Relationship
    {
        protected RelationshipDef mRelDef;
        protected readonly BusinessObjectBase itsOwningBo;
        protected RelKey mRelKey;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object from where the 
        /// relationship originates</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public Relationship(BusinessObjectBase owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
        {
            mRelDef = lRelDef;
            itsOwningBo = owningBo;
            mRelKey = mRelDef.RelKeyDef.CreateRelKey(lBOPropCol);
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        internal string RelationshipName
        {
            get { return mRelDef.RelationshipName; }
        }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        internal RelationshipDef RelationshipDef
        {
            get { return mRelDef; }
        }
    }
}