using System;
using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.CriteriaManager;
using Habanero.Db;
using Habanero.Generic;
using log4net;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship : Relationship
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.SingleRelationship");
        private BusinessObject _relatedBo;
        private string _storedRelationshipExpression;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public SingleRelationship(BusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        /// <summary>
        /// Indicates whether the related object has been specified
        /// </summary>
        /// <returns>Returns true if related object exists</returns>
        /// TODO ERIC - review
        public virtual bool HasRelationship()
        {
            return _relKey.HasRelatedObject();
        }

        /// <summary>
        /// Returns the related object using the database connection provided
        /// </summary>
        /// <param name="connection">A database connection</param>
        /// <returns>Returns the related business object</returns>
        public BusinessObject GetRelatedObject(IDatabaseConnection connection)
        {
            if (_relatedBo == null ||
                (_storedRelationshipExpression != _relKey.RelationshipExpression().ExpressionString()))
            {
                //log.Debug("Retrieving related object, in relationship " + this.RelationshipName) ;
                if (HasRelationship())
                {
                    //log.Debug("HasRelationship returned true, loading object.") ;
                    BusinessObject busObj =
                        (BusinessObject) Activator.CreateInstance(_relDef.RelatedObjectClassType, true);
                    busObj.SetDatabaseConnection(connection);

                    IExpression relExp = _relKey.RelationshipExpression();
                    busObj = busObj.GetBusinessObject(relExp);
                    if (_relDef.KeepReferenceToRelatedObject)
                    {
                        _relatedBo = busObj;
                        _storedRelationshipExpression = relExp.ExpressionString();
                    }
                    else
                    {
                        return busObj;
                    }
                }
            }
            else
            {
                //log.Debug("Related Object is already loaded, returning cached one.") ;
            }
            return _relatedBo;
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="parentObject">The object to relate to</param>
        /// TODO ERIC - is the parameter appropriately named (relatedObject)?
        public void SetRelatedObject(BusinessObject parentObject)
        {
            _relatedBo = parentObject;
            foreach (DictionaryEntry entry in this._relKey)
            {
                RelProp relProp = (RelProp) entry.Value;
                _owningBo.SetPropertyValue(relProp.OwnerPropertyName,
                                             _relatedBo.GetPropertyValue(relProp.RelatedClassPropName));
            }
            _storedRelationshipExpression = _relKey.RelationshipExpression().ExpressionString();
        }

        /// <summary>
        /// Sets the related business object to null, ensuring that
        /// it must be reloaded
        /// </summary>
        public void ClearCache()
        {
            _relatedBo = null;
        }
    }

    
}