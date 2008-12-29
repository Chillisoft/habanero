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

using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
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

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        public override bool IsDirty
        {
            get
            {
                return _relatedBo != null && _relatedBo.Status.IsDirty;
            }
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

            if (RelatedBoForeignKeyHasChanged()) _relatedBo = null;

            if (_relatedBo == null || (_storedRelationshipExpression != newRelationshipExpression.ExpressionString()))
            {
                if (HasRelationship())
                {
                    //log.Debug("HasRelationship returned true, loading object.") ; 
                    IBusinessObject busObj = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject(this);
//                    if (_relDef.KeepReferenceToRelatedObject)
//                    {
                    _relatedBo = busObj;
                    _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
//                    }
//                    else
//                    {
//                        return busObj;
//                    }
                }
                else
                {
                    _relatedBo = null;
                    _storedRelationshipExpression = newRelationshipExpression.ExpressionString();
                }
            }
            if (RelatedBoForeignKeyHasChanged()) _relatedBo = null;
            return _relatedBo;
        }

        private bool RelatedBoForeignKeyHasChanged()
        {
            if (_relatedBo != null)
            {
                foreach (IRelProp prop in this.RelKey)
                {
                    object relatedPropValue = _relatedBo.GetPropertyValue(prop.RelatedClassPropName);
                    if (prop.BOProp.Value == null)
                    {
                        if (relatedPropValue == null) continue;
                        return true;
                    }
                    if (prop.BOProp.Value.Equals(relatedPropValue)) continue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the related object 
        /// </summary>
        /// <returns>Returns the related business object</returns>
        public virtual T GetRelatedObject<T>() where T : class, IBusinessObject, new()
        {
            return (T) GetRelatedObject();
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        public virtual void SetRelatedObject(IBusinessObject relatedObject)
        {

            if (_relatedBo == null) GetRelatedObject();
            if (_relatedBo == relatedObject) return;
            IBusinessObject previousRelatedBO = _relatedBo;
            if (relatedObject != null)
            {
                RelationshipDef.CheckCanAddChild(relatedObject);
                //Add to reverse relationship
                Relationship reverseRelationship = GetReverseRelationship(relatedObject) as Relationship;
                if (reverseRelationship != null)
                {
                    RelationshipDef relationshipDef =
                        reverseRelationship.RelationshipDef as RelationshipDef;
                    if (relationshipDef != null && _relatedBo != null && relationshipDef.RelationshipType == RelationshipType.Composition)
                    {
                        string message = "The " + relationshipDef.RelatedObjectClassName
                                         + " could not be added since the " + relationshipDef.RelationshipName
                                         +
                                         " relationship is set up as a composition relationship (AddChildAction.Prevent)";
                        throw new HabaneroDeveloperException(message, message);
                    }

                    if (reverseRelationship is MultipleRelationship && reverseRelationship.IsRelationshipLoaded)
                    {
                        reverseRelationship.GetLoadedBOColInternal().Add(this.OwningBO);
                    }

                    if (reverseRelationship is SingleRelationship)
                    {
                        reverseRelationship.RelationshipDef.CheckCanAddChild(this.OwningBO);
                        _relatedBo = relatedObject;
                        ((SingleRelationship)reverseRelationship).SetRelatedObject(this.OwningBO);
                    }
                }
            }
            //Remove the this object from the previuosly related object
            if (previousRelatedBO != null)
            {
                this.RelationshipDef.CheckCanRemoveChild(_relatedBo);

                //Remove from previous relationship
                MultipleRelationship reverseRelationship = GetReverseRelationship(_relatedBo) as MultipleRelationship;
                if (reverseRelationship != null && reverseRelationship.IsRelationshipLoaded)
                {
                    reverseRelationship.GetLoadedBOColInternal().Remove(this.OwningBO);
                }
                SingleRelationship singleReverseRelationship = GetReverseRelationship(_relatedBo) as SingleRelationship;
                if (singleReverseRelationship != null)
                {
                    singleReverseRelationship.RelationshipDef.CheckCanRemoveChild(this.OwningBO);
                }
            }
            _relatedBo = relatedObject;
            foreach (RelProp relProp in _relKey)
            {
                object relatedObjectValue = _relatedBo == null
                                                ? null
                                                : _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
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
            if (bo != null)
            {
                col.Add(bo);
            }
            return col;
        }

        protected override IBusinessObjectCollection GetRelatedBusinessObjectColInternal()
        {
            Type type = _relDef.RelatedObjectClassType;
            CheckTypeCanBeCreated(type);
            IBusinessObjectCollection boCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(type, this);
            return boCol;
        }

        ///<summary>
        /// Returns a list of all the related objects that are dirty.
        /// In the case of a composition or aggregation this will be a list of all 
        ///   dirty related objects (child objects). 
        /// In the case of association
        ///   this will only be a list of related objects that are added, removed, marked4deletion or created
        ///   as part of the relationship.
        ///</summary>
        public override IList<IBusinessObject> GetDirtyChildren()
        {
            IList<IBusinessObject> dirtyBusinessObjects = new List<IBusinessObject>();
            if (_relatedBo != null && _relatedBo.Status.IsDirty)
            {
                dirtyBusinessObjects.Add(_relatedBo);
            }
            return dirtyBusinessObjects;
        }
    }
}