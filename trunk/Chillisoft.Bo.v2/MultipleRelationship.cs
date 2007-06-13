using System;
using Chillisoft.Bo.ClassDefinition.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship : Relationship
    {
        private BusinessObjectCollection _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        internal MultipleRelationship(BusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects</returns>
        internal BusinessObjectCollection GetRelatedBusinessObjectCol()
        {
            BusinessObject busObj =
                (BusinessObject) Activator.CreateInstance(_relDef.RelatedObjectClassType, true);
            if (this._relDef.KeepReferenceToRelatedObject)
            {
                // TODO - Add a check to see if the count of objects has changed.  Removed this keep reference because if an object
                // gets added with the foreign key nothing will pick that up other than a reload.
                //if (_boCol == null) {
                _boCol = busObj.GetBusinessObjectCol(_relKey.RelationshipExpression(),
                                                     ((MultipleRelationshipDef) _relDef).OrderBy);
                //}
                return _boCol;
            }
            else
            {
                return busObj.GetBusinessObjectCol(_relKey.RelationshipExpression(),
                                                   ((MultipleRelationshipDef) _relDef).OrderBy);
            }
        }
    }
}