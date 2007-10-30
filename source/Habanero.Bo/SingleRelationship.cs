//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using Habanero.Base;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship : Relationship
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.SingleRelationship");
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
        	return GetRelatedObject<BusinessObject>(connection);
        }

        /// <summary>
        /// Returns the related object using the database connection provided
        /// </summary>
        /// <param name="connection">A database connection</param>
        /// <returns>Returns the related business object</returns>
        public T GetRelatedObject<T>(IDatabaseConnection connection)
			where T :BusinessObject
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
                    busObj = BOLoader.Instance.GetBusinessObject(busObj, relExp);
                    if (_relDef.KeepReferenceToRelatedObject)
                    {
                        _relatedBo = busObj;
                        _storedRelationshipExpression = relExp.ExpressionString();
                    }
                    else
                    {
                        return (T)busObj;
                    }
                }
            }
            else
            {
                //log.Debug("Related Object is already loaded, returning cached one.") ;
            }
            return (T)_relatedBo;
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        public void SetRelatedObject(BusinessObject relatedObject)
        {
            _relatedBo = relatedObject;
            foreach (RelProp relProp in _relKey)
            {
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