using System;
using Habanero.Base.Exceptions;
using Habanero.Bo.ClassDefinition;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship : Relationship
    {
        private BusinessObjectCollection<BusinessObject> _boCol;

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
        public BusinessObjectCollection<BusinessObject> GetRelatedBusinessObjectCol()
        {
            BusinessObject busObj;
            try
            {
                busObj = (BusinessObject) Activator.CreateInstance(_relDef.RelatedObjectClassType, true);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException(String.Format(
                    "An error occurred while attempting to load a related " +
                    "business object collection, with the type given as '{0}'. " +
                    "Check that the given type exists and has been correctly " +
                    "defined in the relationship and class definitions for the classes " +
                    "involved.", _relDef.RelatedObjectClassType));
            }
            if (this._relDef.KeepReferenceToRelatedObject)
            {
                // TODO - Add a check to see if the count of objects has changed.  Removed this keep reference because if an object
                // gets added with the foreign key nothing will pick that up other than a reload.
                //if (_boCol == null) {
                _boCol = BOLoader.Instance.GetBusinessObjectCol(busObj.GetType(), _relKey.RelationshipExpression(),
                                                     ((MultipleRelationshipDef) _relDef).OrderBy);
                //}
                return _boCol;
            }
            else
            {
                return BOLoader.Instance.GetBusinessObjectCol(busObj.GetType(), _relKey.RelationshipExpression(),
                                                   ((MultipleRelationshipDef) _relDef).OrderBy);
            }
        }
    }
}