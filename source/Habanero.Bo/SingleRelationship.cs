//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
//using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship : Relationship
    {
        //TODO: Implement logging private static readonly ILog log = LogManager.GetLogger("Habanero.BO.SingleRelationship");
        private IBusinessObject _relatedBo;
        private string _storedRelationshipExpression;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public SingleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
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
        /// Returns the related object 
        /// </summary>
        /// <returns>Returns the related business object</returns>
        public IBusinessObject GetRelatedObject(IDatabaseConnection connection)
        {
        	return GetRelatedObject<BusinessObject>();
        }

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        public virtual IBusinessObject GetRelatedObject()
        {
            IExpression newRelationshipExpression = _relKey.RelationshipExpression();
            if (_relatedBo == null ||
                (_storedRelationshipExpression != newRelationshipExpression.ExpressionString()))
            {
                //log.Debug("Retrieving related object, in relationship " + this.RelationshipName) ;
                if (HasRelationship())
                {
                    //log.Debug("HasRelationship returned true, loading object.") ; 
                    IBusinessObject busObj = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject(this);
                    // Peter-Working:  IBusinessObject busObj = (BusinessObject)Activator.CreateInstance(_relDef.RelatedObjectClassType, true);
                    // Peter-Working:  busObj = BOLoader.Instance.GetBusinessObject(busObj, newRelationshipExpression);
                    if (_relDef.KeepReferenceToRelatedObject)
                    {
                        _relatedBo = busObj;
                        _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
                    }
                    else
                    {
                        return busObj;
                    }
                }
                else
                {
                    _relatedBo = null;
                    _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
                }
            }
            else
            {
                //log.Debug("Related Object is already loaded, returning cached one.") ;
            }
            return _relatedBo;
        }


        /// <summary>
        /// Returns the related object 
        /// </summary>
        /// <returns>Returns the related business object</returns>
        public virtual T GetRelatedObject<T>()
			where T : class, IBusinessObject, new()
        {
            return (T) GetRelatedObject();
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        public virtual void SetRelatedObject(IBusinessObject relatedObject)
        {
            _relatedBo = relatedObject;
            foreach (RelProp relProp in _relKey)
            {
                object relatedObjectValue;
                if (_relatedBo != null)
                {
                    relatedObjectValue = _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
                } else
                {
                    relatedObjectValue = null;
                }
                _owningBo.SetPropertyValue(relProp.OwnerPropertyName, relatedObjectValue);
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


        protected override IBusinessObjectCollection GetRelatedBusinessObjectColInternal<TBusinessObject>()
        {
            BusinessObjectCollection<TBusinessObject> col = new BusinessObjectCollection<TBusinessObject>();

            TBusinessObject bo = this.GetRelatedObject<TBusinessObject>();
            if (bo !=null)
            {
                col.Add(bo);
            }
            return col;
        }

    }

    
}