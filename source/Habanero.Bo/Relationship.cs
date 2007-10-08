using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for relationships between business objects
    /// </summary>
    public abstract class Relationship
    {
        protected RelationshipDef _relDef;
        protected readonly BusinessObject _owningBo;
        protected internal RelKey _relKey;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object from where the 
        /// relationship originates</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public Relationship(BusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
        {
            _relDef = lRelDef;
            _owningBo = owningBo;
            _relKey = _relDef.RelKeyDef.CreateRelKey(lBOPropCol);
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public string RelationshipName
        {
            get { return _relDef.RelationshipName; }
        }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        internal RelationshipDef RelationshipDef
        {
            get { return _relDef; }
        }
    }
}